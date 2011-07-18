using System;
using System.Data;
using NHibernate;
using TinySheets.Eventing;
using TinySheets.Monitoring;

namespace TinySheets.Persistence.NHibernate
{
    public class NHibernateTransactionContext : IDisposable, ITransactionContext
    {
        readonly ISession _session;
        readonly ILog<NHibernateTransactionContext> _log;
        readonly IDomainEventDispatcher _domainEventDispatcher;
        ITransaction _currentTransaction;
        bool _abort;
        const int EventProcessingCycleLimit = 100;

        public NHibernateTransactionContext(ISession session, ILog<NHibernateTransactionContext> log, IDomainEventDispatcher domainEventDispatcher)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (log == null) throw new ArgumentNullException("log");
            if (domainEventDispatcher == null) throw new ArgumentNullException("domainEventDispatcher");

            _session = session;
            _log = log;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("A transaction is already active.");

            _log.Debug("Beginning transaction.");
            _currentTransaction = _session.BeginTransaction(isolationLevel);
            _abort = false;
        }

        public void EndTransaction()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("There is no active transaction to end.");

            _log.Debug("Ending transaction.");

            try
            {
                if (_abort)
                {
                    _currentTransaction.Rollback();
                    _log.Debug("Transaction rolled back.");
                }
                else
                {
                    DispatchDomainEvents();
                    _currentTransaction.Commit();
                    _log.Debug("Transaction committed.");
                }
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        void DispatchDomainEvents()
        {
            var flushCycles = 0;
            do
            {
                if (++flushCycles > EventProcessingCycleLimit)
                    throw new InvalidOperationException("Potentially infinite cycle detected in domain event processing.");

                _session.Flush();
            }
            while (_domainEventDispatcher.DispatchEvents());
        }

        public void SetAbort()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("There is no active transaction to abort.");

            _log.Debug("Abort flag is set.");
            _abort = true;
        }

        public void Dispose()
        {
            if (_currentTransaction == null)
                return;

            _log.Warning("The active transaction was never comitted nor aborted, now rolling back.");
            var transaction = _currentTransaction;
            _currentTransaction = null;
            transaction.Rollback();
        }
    }
}