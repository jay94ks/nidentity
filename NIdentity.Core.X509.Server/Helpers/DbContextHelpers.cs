using Microsoft.EntityFrameworkCore;

namespace NIdentity.Core.X509.Server.Helpers
{
    internal static class DbContextHelpers
    {
        /// <summary>
        /// Create a row.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="DbContext"></param>
        /// <param name="New"></param>
        /// <returns></returns>
        public static bool DbCreate<TEntity>(this DbContext DbContext, TEntity New) where TEntity : class
        {
            try
            {
                DbContext.Set<TEntity>().Add(New);
                DbContext.SaveChanges();
                return true;
            }

            catch { }
            return false;
        }

        /// <summary>
        /// Update a row.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="DbContext"></param>
        /// <param name="New"></param>
        /// <returns></returns>
        public static bool DbUpdate<TEntity>(this DbContext DbContext, TEntity New) where TEntity : class
        {
            try
            {
                DbContext.Set<TEntity>().Update(New);
                return DbContext.SaveChanges() > 0;
            }

            catch { }
            return false;
        }

        /// <summary>
        /// Remove a row.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="DbContext"></param>
        /// <param name="New"></param>
        /// <returns></returns>
        public static bool DbRemove<TEntity>(this DbContext DbContext, TEntity New) where TEntity : class
        {
            try
            {
                DbContext.Set<TEntity>().Remove(New);
                return DbContext.SaveChanges() > 0;
            }

            catch { }
            return false;
        }
    }
}
