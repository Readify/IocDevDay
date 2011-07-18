using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace TinySheets.Persistence.NHibernate
{
    public class NHibernateRepository<T> : IRepository<T>
    {
        readonly ISession _session;

        public NHibernateRepository(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
        }

        public void Add(T item)
        {
            _session.Save(item);
        }

        public void Remove(T item)
        {
            _session.Delete(item);
        }

        public IQueryable<T> Items
        {
            get { return _session.Query<T>(); }
        }
    }
}