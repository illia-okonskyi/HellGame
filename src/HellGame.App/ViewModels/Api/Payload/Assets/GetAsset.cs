using HellEngine.Core.Models.Assets;
using Newtonsoft.Json;

namespace HellGame.App.ViewModels.Api.Payload.Assets
{
    [JsonObject]
    public class GetAsset
    {
        public string Key { get; set; }
        public AssetType Type { get; set; }
        public string Data { get; set; }
    }
}
