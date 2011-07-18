using System.Data;

namespace TinySheets.Persistence
{
    public interface ITransactionContext
    {
        void BeginTransaction(IsolationLevel isolationLevel);
        void EndTransaction();
        void SetAbort();
    }
}
