
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Application.Contracts.ReCaptcha;
using MsfServer.Application.Contracts.ReCaptcha.Dto;

namespace MsfServer.Application.Services
{
    public class ReCaptchaService(HttpClient httpClient, IConfiguration configuration) : IReCaptchaService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _secretKey = configuration["ReCaptcha:SecretKey"]!;

        public async Task<bool> VerifyTokenAsync(string recaptcha)
        {
            try
            {
                var response = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={recaptcha}", null);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonResponse);
                    if (result != null && result.Success)
                    {
                        return true;
                    }
                    else
                    {
                        throw new CustomException(StatusCodes.Status400BadRequest, "Mã lỗi: " + string.Join(", ", result?.ErrorCodes ?? []));
                    }
                }else
                {
                    throw new CustomException(StatusCodes.Status500InternalServerError, "Không thể xác minh reCAPTCHA token.");
                }
            }
            catch (HttpRequestException e)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Yêu cầu lỗi: {e.Message}");
            }
            catch (Exception e)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Có lỗi sảy ra: {e.Message}");
            }
        }
    }

}
