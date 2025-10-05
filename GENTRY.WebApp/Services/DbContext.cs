using GENTRY.Models.Interfaces;

namespace GENTRY.BLL
{
    public class DbContext
    {
        public object Database { get; internal set; }

        internal IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            throw new NotImplementedException();
        }
    }
}