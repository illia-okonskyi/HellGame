using Newtonsoft.Json;

namespace HellGame.Api.ViewModels
{
    [JsonObject]
    public class ApiRequest<TPayload> where TPayload : class
    {
        public TPayload Payload { get; set; }

        public static ApiRequest<TPayload> Make(TPayload payload)
        {
            return new ApiRequest<TPayload> { Payload = payload };
        }
    }
}
