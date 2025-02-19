using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bodatero.Result.AspNetCore
{
    /// <summary>
    /// Provides extension methods for <see cref="RouteHandlerBuilder"/> to handle responses.
    /// </summary>
    public static class ResultRouteBuilder
    {
        /// <summary>
        /// Adds an endpoint filter that processes <see cref="Result{T}"/> responses.
        /// If the result is successful, it returns the encapsulated value; otherwise, it sets a 400 Bad Request response.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="routeBuilder">The route handler builder.</param>
        /// <returns>The modified route handler builder.</returns>
        public static RouteHandlerBuilder WithResult<T>(this RouteHandlerBuilder routeBuilder, ResultServiceConfig? resultServiceConfig = null)
        {
            routeBuilder.AddEndpointFilter(async (context, next) =>
            {
                var response = await next(context);

                if (response is Result<T> result)
                {
                    var service = resultServiceConfig != null
                        ? new ResultService(resultServiceConfig)
                        : context.HttpContext.RequestServices.GetService<ResultService>();

                    if (result.IsSuccess)
                    {
                        return service?.SuccessResultHandler?.Invoke(context, result.Value) ?? result.Value;
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return service?.FailResultHandler?.Invoke(context, result.Error!) ?? result.Error;
                }

                return response;
            });

            return routeBuilder;
        }



        /// <summary>
        /// Adds an endpoint filter that processes any <see cref="Result{T}"/> response dynamically.
        /// If the result is successful, it returns the encapsulated value; otherwise, it sets a 400 Bad Request response.
        /// </summary>
        /// <param name="routeBuilder">The route handler builder.</param>
        /// <returns>The modified route handler builder.</returns>
        public static RouteHandlerBuilder WithResult(this RouteHandlerBuilder routeBuilder, ResultServiceConfig? resultServiceConfig = null)
        {
            routeBuilder.AddEndpointFilter(Helpers.GetFilterFuncNonGeneric(resultServiceConfig));

            return routeBuilder;
        }

        public static RouteGroupBuilder WithResult(this RouteGroupBuilder routeGroupBuilder, ResultServiceConfig? resultServiceConfig = null)
        {
            routeGroupBuilder.AddEndpointFilter(Helpers.GetFilterFuncNonGeneric(resultServiceConfig));

            return routeGroupBuilder;
        }
    }
}
