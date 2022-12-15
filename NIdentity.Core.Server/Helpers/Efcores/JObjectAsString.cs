using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NIdentity.Core.Server.Helpers.Efcores
{
    public class JObjectAsString : EfcoreNotationAttribute<JObject>
    {
        private static bool Equals(JObject A, JObject B)
        {
            string As = ToString(A);
            string Bs = ToString(B);
            return As == Bs;
        }

        private static string ToString(JObject A)
        {
            return A != null ? A.ToString() : null;
        }

        /// <inheritdoc/>
        public override void Apply(PropertyBuilder<JObject> Property)
        {
            Property.HasConversion(X => X != null ? X.ToString() : null,
                X => !string.IsNullOrWhiteSpace(X) ? JsonConvert.DeserializeObject<JObject>(X) : null,
                new ValueComparer<JObject>((X, Y) => Equals(X, Y), X => ToString(X).GetHashCode()));
        }
    }
}
