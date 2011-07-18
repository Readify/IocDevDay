using System;
using System.Data;
using TinySheets.Persistence;

namespace TinySheets.Tasks
{
    class TransactedTask : ITask
    {
        readonly ITask _inner;
        readonly ITransactionContext _transactionContext;

        public TransactedTask(ITask inner, ITransactionContext transactionContext)
        {
            if (inner == null) throw new ArgumentNullException("inner");
            if (transactionContext == null) throw new ArgumentNullException("transactionContext");
            _inner = inner;
            _transactionContext = transactionContext;
        }

        public void Run()
        {
            _transactionContext.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                _inner.Run();
            }
            catch (Exception)
            {
                _transactionContext.SetAbort();
                throw;
            }
            finally
            {
                _transactionContext.EndTransaction();
            }
        }
    }
}
