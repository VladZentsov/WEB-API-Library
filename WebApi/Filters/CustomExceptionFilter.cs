using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    public class CustomExceptionFilter : Attribute, IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var action = context.ActionDescriptor.DisplayName;
            var callStack = context.Exception.StackTrace;
            var exceptionMessage = context.Exception.Message;
            if (context.Exception is LibraryException)
            {
                exceptionMessage = "BadRequest";
            }

            context.Result = new ContentResult
            {
                Content = $"Calling {action} failed, because: {exceptionMessage}. Callstack: {callStack}.",
                StatusCode =500
            };
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
