using Newtonsoft.Json; //Nugetからinstallが必要
using System;

namespace Data {
    public class Person {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public List<string> Attributes { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<Item> Items { get; set; }
    }
}