using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellGame.App.ViewModels.Api.Payload.Game
{
    [JsonObject]
    public class Transition
    {
        public string Key { get; set; }
        public string TextAssetKey { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
    }
}
