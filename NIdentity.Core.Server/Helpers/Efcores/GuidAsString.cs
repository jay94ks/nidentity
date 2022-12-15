using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NIdentity.Core.Server.Helpers;

namespace NIdentity.Core.Server.Helpers.Efcores
{
    public class GuidAsString : EfcoreNotationAttribute<Guid>
    {
        /// <summary>
        /// Parse a guid from input string and make empty if failed.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static Guid ToGuidOrEmpty(string Input)
        {
            Guid.TryParse(Input, out var Return);
            return Return;
        }

        /// <inheritdoc/>
        public override void Apply(PropertyBuilder<Guid> Property)
        {
            Property.HasConversion(X => X.ToString(), X => ToGuidOrEmpty(X),
                new ValueComparer<Guid>((X, Y) => X == Y, X => X.GetHashCode()))
            .HasMaxLength(40);
        }
    }
}
