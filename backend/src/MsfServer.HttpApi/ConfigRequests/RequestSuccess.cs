using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsfServer.HttpApi.ConfigRequests.ConfigDto;

namespace MsfServer.HttpApi.ConfigRequests
{
    public class RequestSuccess
    {
        // sử dụng cho lấy dữ liệu thành công hoặc đăng nhập thành công
        public static IActionResult OK(object data, string message)
        {
            return new ObjectResult(new Response
            {
                StatusCode = StatusCodes.Status200OK,
                Message = message,
                Data = data,
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        // sử dụng cho thêm thành công
        public static IActionResult Create(object data, string message)
        {
            return new ObjectResult(new Response
            {
                StatusCode = StatusCodes.Status201Created,
                Message = message,
                Data = data,
            })
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        // sử dụng cho sửa hoặc xóa thành công
        public static IActionResult NoContent(object data, string message)
        {
            return new ObjectResult(new Response
            {
                StatusCode = StatusCodes.Status200OK,
                Message = message,
                Data = data,
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
