using System.Diagnostics.CodeAnalysis;

namespace NIdentity.Core.X509.Documents
{
    public struct DocumentIdentity : IEquatable<DocumentIdentity>
    {
        /// <summary>
        /// Initialize <see cref="DocumentIdentity"/> using <paramref name="Owner"/> and <paramref name="PathName"/>.
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="PathName"></param>
        public DocumentIdentity(CertificateIdentity Owner, string PathName)
        {
            this.Owner = Owner;
            this.PathName = NormalizePathName(PathName);
        }

        /// <summary>
        /// Parse the input string and make <see cref="CertificateIdentity"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static DocumentIdentity Parse(string Input)
        {
            var Eq = Input.Split(':', 2, StringSplitOptions.None);
            if (Eq is null || Eq.Length <= 0)
                return default;

            var Owner = Eq.FirstOrDefault(); var PathName = Eq.LastOrDefault();
            return new DocumentIdentity(CertificateIdentity.Parse(Owner), PathName);
        }

        /// <summary>
        /// Normalize the <see cref="PathName"/>.
        /// </summary>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public static string NormalizePathName(string PathName) 
            => string.Join("/", (PathName ?? string.Empty).Trim(' ', '/')
                .Split('/').Where(X => !string.IsNullOrWhiteSpace(X)));

        /// <summary>
        /// Owner.
        /// </summary>
        public CertificateIdentity Owner { get; }

        /// <summary>
        /// Path Name.
        /// </summary>
        public string PathName { get; }

        /// <inheritdoc/>
        public bool Equals(DocumentIdentity Other)
        {
            if (Owner != Other.Owner)
                return false;

            if (PathName is null)
                return Other.PathName is null;

            return PathName.Equals(Other.PathName);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object Obj)
        {
            if (Obj is DocumentIdentity Identity)
                return Equals(Identity);

            return base.Equals(Obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => $"{Owner}:{NormalizePathName(PathName)}".GetHashCode();

        // --------
        public static bool operator ==(DocumentIdentity L, DocumentIdentity R) => L.Equals(R);
        public static bool operator !=(DocumentIdentity L, DocumentIdentity R) => !(L == R);
    }
}
