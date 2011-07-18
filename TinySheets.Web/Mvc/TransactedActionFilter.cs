using System;
using System.Data;
using System.Web.Mvc;
using TinySheets.Persistence;

namespace TinySheets.Web.Mvc
{
    public class TransactedActionFilter : IActionFilter
    {
        readonly ITransactionContext _transactionContext;

        public TransactedActionFilter(ITransactionContext transactionContext)
        {
            if (transactionContext == null) throw new ArgumentNullException("transactionContext");
            _transactionContext = transactionContext;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _transactionContext.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                _transactionContext.SetAbort();

            _transactionContext.EndTransaction();
        }
    }
}