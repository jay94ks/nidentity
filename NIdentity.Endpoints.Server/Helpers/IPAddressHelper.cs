using System.Net;

namespace NIdentity.Endpoints.Server.Helpers
{
    public static class IPAddressHelper
    {
        /// <summary>
        /// Make 8-bit sliced address string by subnet mask.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="SubnetMask"></param>
        /// <returns></returns>
        public static string ToDotBytes(this IPAddress Address, int SubnetMask = -1)
        {
            var AddressBytes = Address.GetAddressBytes();
            var NumericDots = AddressBytes.Select(X => X.ToString());

            if (SubnetMask >= 0)
                NumericDots = NumericDots.Take(SubnetMask / 8);

            return string.Join(".", NumericDots);
        }

        /// <summary>
        /// Make 8-bit sliced address string by subnet mask and returns remainder bits.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="SubnetMask"></param>
        /// <param name="Remainder"></param>
        /// <returns></returns>
        public static string ToDotBytes(this IPAddress Address, int SubnetMask, out int Remainder)
        {
            var AddressBytes = Address.GetAddressBytes();
            var NumericDots = AddressBytes.Select(X => X.ToString());

            if (SubnetMask < 0)
                Remainder = 0;

            else
            {
                NumericDots = NumericDots.Take(SubnetMask / 8);
                var Dotbytes = NumericDots.Count();
                var Totalbits = AddressBytes.Length * 8;
                Remainder = Totalbits - Dotbytes * 8;
            }

            return string.Join(".", NumericDots);
        }

        /// <summary>
        /// Parse Dot Bytes encoded string.
        /// </summary>
        /// <param name="DotBytesString"></param>
        /// <returns></returns>
        public static IPAddress ParseDotBytes(string DotBytesString)
        {
            return new IPAddress(DotBytesString.Split('.').Select(X => byte.Parse(X)).ToArray());
        }
    }
}
