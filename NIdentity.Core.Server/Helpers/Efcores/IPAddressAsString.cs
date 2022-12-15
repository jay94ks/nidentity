using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NIdentity.Core.Server.Helpers;
using System.Net;

namespace NIdentity.Core.Server.Helpers.Efcores
{
    public class IPAddressAsString : EfcoreNotationAttribute<IPAddress>
    {
        /// <summary>
        /// Convert string to <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        internal static IPAddress ToAddressOrNull(string Input)
        {
            if (string.IsNullOrWhiteSpace(Input))
                return null;

            IPAddress.TryParse(Input, out var Addr);
            return Addr;
        }

        /// <summary>
        /// Make the string expression for the address.
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        internal static string ToAddressString(IPAddress Address)
        {
            if (Address is null)
                return string.Empty;

            return Address.ToString();
        }

        ///<inheritdoc/>
        public override void Apply(PropertyBuilder<IPAddress> Property)
        {
            Property.HasConversion(X => ToAddressString(X), X => ToAddressOrNull(X),
                new ValueComparer<IPAddress>((X, Y) => X == Y, X => X.GetHashCode()))
                .HasMaxLength(45);
        }
    }
}
