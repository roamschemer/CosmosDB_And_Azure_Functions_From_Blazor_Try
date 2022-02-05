using Newtonsoft.Json; //Nugetからinstallが必要

namespace Data {
    public class Item {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        public Item(string name, string content) {
            Name = name;
            Content = content;
        }
    }
}
