using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GenderIdentification
{
    public class sound
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("content")]
        public string content { get; set; }
        [JsonProperty("language")]
        public string language { get; set; }
    }
    
    public class genderResult
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("gender")]
        public string gender { get; set; }
    }
}
