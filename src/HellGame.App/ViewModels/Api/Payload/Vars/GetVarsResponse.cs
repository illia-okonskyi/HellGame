using Newtonsoft.Json;
using System.Collections.Generic;

namespace HellGame.App.ViewModels.Api.Payload.Vars
{
    [JsonObject]
    public class GetVarsResponse
    {
        public List<GameVar> Vars { get; set; } = new List<GameVar>();
    }
}
