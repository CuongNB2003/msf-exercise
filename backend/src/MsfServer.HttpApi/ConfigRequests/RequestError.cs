using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsfServer.HttpApi.ConfigRequests.ConfigDto;

namespace MsfServer.HttpApi.ConfigRequests
{
    public static class RequestError
    {
        public static IActionResult BadRequest(object data, string message)
        {
            return new ObjectResult(new Response
            {
                StatusCode = StatusCodes.Status400BadRequest, 
                Message = message,
                Data = data,
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        public static IActionResult NotFound(object data, string message)
        {
            return new ObjectResult(new Response
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = message,
                Data = data,
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public static IActionResult InternalServerError(object data, string message)
        {
            return new ObjectResult(new Response
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = message,
                Data = data,
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
