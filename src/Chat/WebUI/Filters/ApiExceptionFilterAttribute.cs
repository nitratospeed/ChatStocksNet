using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebUI.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<string, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger _logger;
        private string Message = string.Empty;
        //private IEnumerable<ValidationFailure> CustomValidationMessage;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _exceptionHandlers = new Dictionary<string, Action<ExceptionContext>>
            {
                //{ nameof(ValidationException), HandleValidationException },
                //{ nameof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                //{ nameof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            var errorResponse = BaseResult<object>.Failure(null, Message);

            context.Result = new JsonResult(errorResponse);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType().Name;
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message);

            Message = "Unknown Error. Please try again in a few minutes.";

            context.ExceptionHandled = true;
        }
    }
}
