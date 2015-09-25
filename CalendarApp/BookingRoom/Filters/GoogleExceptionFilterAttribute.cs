using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using BookingRoom.Helpers;
using Google;
using log4net;

namespace BookingRoom.Filters
{
    public class GoogleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log =
            LogManager.GetLogger(AppSettingsHelper.GetAppSetting(AppSetingsConst.LoggerName));


        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is GoogleApiException)
            {
               
                actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(actionExecutedContext.Exception.ToString()),
                    StatusCode = HttpStatusCode.InternalServerError,
                };

                _log.Error("Exception - \n" + actionExecutedContext.Exception);
            }
        }
    }
}
