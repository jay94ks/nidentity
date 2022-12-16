namespace NIdentity.Connector.AspNetCore
{
    /// <summary>
    /// Requester Accessor.
    /// </summary>
    public class RequesterAccessor 
    {
        private IHttpContextAccessor m_Accessor;

        /// <summary>
        /// Initialize a new <see cref="RequesterAccessor"/> instance.
        /// </summary>
        /// <param name="Accessor"></param>
        public RequesterAccessor(IHttpContextAccessor Accessor) => m_Accessor = Accessor;

        /// <summary>
        /// Requester instance.
        /// </summary>
        public Requester Requester => m_Accessor.HttpContext != null
            ? Requester.FromHttpContext(m_Accessor.HttpContext)
            : null;
    }
}
