using Newtonsoft.Json;

namespace HellGame.Api.ViewModels.ApiPayload.Locale
{
    [JsonObject]
    public class GetLocale
    {
        public string Locale { get; set; }
    }
}
