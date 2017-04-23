
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Techamante.Web
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        public async override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var currentPersonnelId = context.ExceptionContext.RequestContext.Principal.Identity?.GetUserId<int>();
            var currentPersonnelName = context.ExceptionContext.RequestContext.Principal.Identity?.GetUserName();


        }
    }
}