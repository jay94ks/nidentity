using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NIdentity.Core.Server.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class EfcoreNotationAttribute : Attribute
    {
        /// <summary>
        /// Apply the notated behaviour.
        /// </summary>
        /// <param name="Property"></param>
        public abstract void Apply(PropertyBuilder Property);
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class EfcoreNotationAttribute<TProperty> : EfcoreNotationAttribute
    {
        /// <summary>
        /// Apply the notated behaviour.
        /// </summary>
        /// <param name="Property"></param>
        public abstract void Apply(PropertyBuilder<TProperty> Property);

        /// <inheritdoc/>
        public sealed override void Apply(PropertyBuilder Property)
        {
            if (Property is PropertyBuilder<TProperty> TProp)
                Apply(TProp);
        }
    }
}
