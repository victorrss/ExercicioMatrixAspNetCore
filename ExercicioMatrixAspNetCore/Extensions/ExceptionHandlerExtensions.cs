using ExercicioMatrix.DAL.Usuarios.Exceptions;
using ExercicioMatrixAspNetCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;

namespace ExercicioMatrix.Extensions
{
    /// <summary>
    /// Classe de extensão para preparar o builder da aplicação para utilizar um handler de exceptions para requests
    /// </summary>
    public static class ExceptionHandlerExtensions
    {
        /// <summary>
        /// Prepara um handler de exceptions para requests realizados aos controllers
        /// </summary>
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            // Inclui o handler de exceptions
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async (context) =>
                {
                    // Utiliza um status code default de internal error
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    // Resgata o exception ocorrido
                    IExceptionHandlerFeature feature = context.Features.Get<IExceptionHandlerFeature>();

                    // Pega o exception capturado
                    Exception exc = feature?.Error;

                    // Cria o ViewModel do erro ocorrido
                    ErrorViewModel errorModel = CreateError(context, exc);

                    // Atualiza o status code de retorno do response
                    context.Response.StatusCode = errorModel.status;

                    // Inclui o objeto no response
                    await context.Response.WriteAsJsonAsync(errorModel);
                }
            });

        }

        /// <summary>
        /// Cria o view model de erro para o HttpResponse
        /// </summary>
        private static ErrorViewModel CreateError(HttpContext context, Exception exc)
        {
            // 422 - Unprocessable Entity
            if (exc?.GetType().Name == typeof(EntityDuplicatedException).Name ||
                exc?.GetType().Name == typeof(EntityModelInvalidException).Name ||
                exc?.GetType().Name == typeof(EntityUniqueViolatedException).Name)
            {
                return DefaultError(StatusCodes.Status422UnprocessableEntity, exc);
            }

            // 404 - Not Found
            if (exc?.GetType().Name == typeof(EntityNotFoundException).Name)
            {
                return DefaultError(StatusCodes.Status404NotFound, exc);
            }

            // 401 - Unauthorized
            if (exc?.GetType().Name == typeof(UnauthorizedAccessException).Name)
            {
                return DefaultError(StatusCodes.Status401Unauthorized, exc);
            }

            // 500 - Internal Server Error
            return DefaultError(StatusCodes.Status500InternalServerError, exc);
        }

        /// <summary>
        /// Cria um objeto padrão de erro para o response
        /// </summary>
        private static ErrorViewModel DefaultError(int httpStatusCode, Exception exc)
        {
            return new ErrorViewModel(httpStatusCode, httpStatusCode == 500 ? exc!.ToString() : exc!.Message);
        }
    }
}
