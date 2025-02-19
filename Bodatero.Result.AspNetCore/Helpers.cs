using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bodatero.Result.AspNetCore
{
    internal static class Helpers
    {
        public static Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> GetFilterFuncNonGeneric(ResultServiceConfig? resultServiceConfig = null)
        {
            return (async (context, next) =>
            {
                var response = await next(context);
                var resultType = response?.GetType();

                if (resultType != null && resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var isSuccessProperty = resultType.GetProperty("IsSuccess", BindingFlags.Public | BindingFlags.Instance);
                    var valueProperty = resultType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
                    var errorProperty = resultType.GetProperty("Error", BindingFlags.Public | BindingFlags.Instance);

                    if (isSuccessProperty != null && valueProperty != null && errorProperty != null)
                    {
                        bool isSuccess = (bool)isSuccessProperty.GetValue(response)!;
                        var value = valueProperty.GetValue(response);
                        var error = (Exception)errorProperty!.GetValue(response)!;

                        var service = resultServiceConfig != null
                            ? new ResultService(resultServiceConfig)
                            : context.HttpContext.RequestServices.GetService<ResultService>();

                        if (isSuccess)
                        {
                            return service?.SuccessResultHandler?.Invoke(context, value) ?? value;
                        }

                        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return service?.FailResultHandler?.Invoke(context, error) ?? error;
                    }
                }

                return response;
            });
        }
    }
}
