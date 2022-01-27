using Newtonsoft.Json; //Nugetからinstallが必要
using System;

namespace Data {
    public class Item {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
