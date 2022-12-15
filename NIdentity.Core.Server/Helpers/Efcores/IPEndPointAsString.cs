using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NIdentity.Core.Server.Helpers;
using System.Net;

namespace NIdentity.Core.Server.Helpers.Efcores
{
    public class IPEndPointAsString : EfcoreNotationAttribute<IPEndPoint>
    {
        /// <summary>
        /// Convert string to <see cref="IPEndPointAsString"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        internal static IPEndPoint ToAddressOrNull(string Input)
        {
            if (string.IsNullOrWhiteSpace(Input))
                return null;

            IPEndPoint.TryParse(Input, out var Addr);
            return Addr;
        }

        /// <summary>
        /// Make the string expression for the address.
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        internal static string ToAddressString(IPEndPoint Address)
        {
            if (Address is null)
                return string.Empty;

            return Address.ToString();
        }

        ///<inheritdoc/>
        public override void Apply(PropertyBuilder<IPEndPoint> Property)
        {
            Property.HasConversion(X => ToAddressString(X), X => ToAddressOrNull(X),
                new ValueComparer<IPEndPointAsString>((X, Y) => X == Y, X => X.GetHashCode()))
                .HasMaxLength(45);
        }
    }
}
