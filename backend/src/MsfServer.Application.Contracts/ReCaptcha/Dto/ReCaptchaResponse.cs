using Newtonsoft.Json;

namespace MsfServer.Application.Contracts.ReCaptcha.Dto
{
    public class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public required List<string> ErrorCodes { get; set; }
    }
}
