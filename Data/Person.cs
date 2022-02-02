using Newtonsoft.Json; //Nugetからinstallが必要
using System;

namespace Data {
    public class Person {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public List<string> Attributes { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<Item> Items { get; set; }
        public Person(Guid? id, string name, string job, List<string> attributes, List<Item> items) {
            Id = id;
            Name = name;
            Job = job;
            Attributes = attributes;
            Items = items;
        }
        public Person(Person person) {
            Id = person.Id;
            Name = person.Name;
            Job = person.Job;
            Attributes = person.Attributes;
            Items = person.Items;
        }
        public Person() { }
    }
}