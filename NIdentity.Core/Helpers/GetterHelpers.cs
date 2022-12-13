namespace NIdentity.Core.Helpers
{
    public static class GetterHelpers
    {
        /// <summary>
        /// Get the required service with cache.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="Store"></param>
        /// <returns></returns>
        public static TReturn Cached<TReturn>(ref TReturn Store, Func<TReturn> Getter) where TReturn : class
        {
            if (Store is null)
                Store = Getter.Invoke();

            return Store;
        }

        /// <summary>
        /// Get the required service with cache.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="Store"></param>
        /// <returns></returns>
        public static TReturn Cached<TReturn>(ref TReturn? Store, Func<TReturn> Getter) where TReturn : struct
        {
            if (Store.HasValue)
                return Store.Value;

            return (Store = Getter.Invoke()).Value;
        }
    }
}
