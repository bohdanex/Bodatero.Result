using Microsoft.AspNetCore.Http;

namespace Bodatero.Result.AspNetCore
{


    public class ResultService
    {
        public ResultFailFunc? FailResultHandler { get; init; }
        public ResultSuccessFunc? SuccessResultHandler { get; init; }

        public ResultService()
        {
            
        }

        public ResultService(Action<ResultServiceConfig>? configAction)
        {
            ResultServiceConfig config = new ();

            configAction?.Invoke(config);

            if (config.SuccessResultHandler != null)
            {
                SuccessResultHandler = (ResultSuccessFunc)config.SuccessResultHandler.Clone();
            }

            if(config.FailResultHandler != null)
            {
                FailResultHandler = (ResultFailFunc)config.FailResultHandler.Clone();
            }
        }

        public ResultService(ResultServiceConfig config)
        {
            if (config.SuccessResultHandler != null)
            {
                SuccessResultHandler = (ResultSuccessFunc)config.SuccessResultHandler.Clone();
            }

            if (config.FailResultHandler != null)
            {
                FailResultHandler = (ResultFailFunc)config.FailResultHandler.Clone();
            }
        }
    }
}
