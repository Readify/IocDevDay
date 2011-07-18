using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TinySheets.Web.Mvc
{
    public class ExtensibleActionInvoker : ControllerActionInvoker
    {
        readonly IEnumerable<IActionFilter> _actionFilters;
        readonly IEnumerable<IAuthorizationFilter> _authorizationFilters;
        readonly IEnumerable<IExceptionFilter> _exceptionFilters;
        readonly IEnumerable<IResultFilter> _resultFilters;

        public ExtensibleActionInvoker(
            IEnumerable<IActionFilter> actionFilters, 
            IEnumerable<IAuthorizationFilter> authorizationFilters, 
            IEnumerable<IExceptionFilter> exceptionFilters, 
            IEnumerable<IResultFilter> resultFilters)
        {
            if (actionFilters == null) throw new ArgumentNullException("actionFilters");
            if (authorizationFilters == null) throw new ArgumentNullException("authorizationFilters");
            if (exceptionFilters == null) throw new ArgumentNullException("exceptionFilters");
            if (resultFilters == null) throw new ArgumentNullException("resultFilters");
            _actionFilters = actionFilters;
            _authorizationFilters = authorizationFilters;
            _exceptionFilters = exceptionFilters;
            _resultFilters = resultFilters;
        }

        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);
            SetFilters(filters.ActionFilters, _actionFilters);
            SetFilters(filters.AuthorizationFilters, _authorizationFilters);
            SetFilters(filters.ExceptionFilters, _exceptionFilters);
            SetFilters(filters.ResultFilters, _resultFilters);
            return filters;
        }

        static void SetFilters<T>(ICollection<T> existing, IEnumerable<T> additional)
        {
            foreach (var add in additional)
                existing.Add(add);
        }
    }
}