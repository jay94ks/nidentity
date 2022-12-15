using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace NIdentity.Core.Server.Helpers
{
    /// <summary>
    /// Efcore helpers.
    /// </summary>
    public static class EfcoreHelpers
    {
        /// <summary>
        /// Apply EFcore custom notations.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="Models"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyNotations<TContext>(this ModelBuilder Models)
        {
            var Props = typeof(TContext).GetProperties();

            foreach (var Each in Props)
            {
                var PropType = Each.PropertyType;
                if (PropType.IsConstructedGenericType)
                {
                    var GenericType = PropType.GetGenericTypeDefinition();
                    if (GenericType != typeof(DbSet<>))
                        continue;

                    var EntityType = PropType.GetGenericArguments().First();
                    ConfigureEntityType(Models.Entity(EntityType), EntityType);
                }
            }

            return Models;
        }

        /// <summary>
        /// Configure the entity type.
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Type"></param>
        private static void ConfigureEntityType(EntityTypeBuilder Entity, Type Type)
        {
            var Props = Type.GetProperties();
            var Generic = Entity.GetType().GetMethods()
                .Where(X => X.Name == nameof(Entity.Property))
                .Where(X => X.GetParameters().Length == 1)
                .FirstOrDefault(X => X.IsGenericMethod);

            foreach (var Each in Props)
            {
                if (Each.GetCustomAttribute<NotMappedAttribute>() != null)
                    continue;

                var Notations = Each.GetCustomAttributes<EfcoreNotationAttribute>(true);
                var Method = Generic.MakeGenericMethod(Each.PropertyType);
                var Target = Method.Invoke(Entity, new object[] { Each.Name })
                    as PropertyBuilder;

                foreach (var Notation in Notations)
                    Notation.Apply(Target);
            }
        }
    }
}
