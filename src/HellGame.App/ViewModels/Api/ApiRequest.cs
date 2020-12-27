using Newtonsoft.Json;

namespace HellGame.App.ViewModels.Api
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
