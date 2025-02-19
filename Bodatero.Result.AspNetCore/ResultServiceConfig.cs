using Microsoft.AspNetCore.Http;

namespace Bodatero.Result.AspNetCore
{
    public delegate object? ResultFailFunc(EndpointFilterInvocationContext context, Exception error);
    public delegate object? ResultSuccessFunc(EndpointFilterInvocationContext context, object? resultValue);

    public class ResultServiceConfig
    {
        public ResultFailFunc? FailResultHandler { get; set; }
        public ResultSuccessFunc? SuccessResultHandler { get; set; }

        public ResultServiceConfig()
        {

        }

        public ResultServiceConfig(ResultSuccessFunc successResultHandler)
        {
            SuccessResultHandler = successResultHandler;
        }

        public ResultServiceConfig(ResultFailFunc failResultHandler)
        {
            FailResultHandler = failResultHandler;
        }

        public ResultServiceConfig(ResultSuccessFunc successResultHandler, ResultFailFunc failResultHandler)
        {
            FailResultHandler = failResultHandler;
            SuccessResultHandler = successResultHandler;
        }
    }
}
