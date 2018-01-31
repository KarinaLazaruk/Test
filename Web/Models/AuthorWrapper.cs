﻿using Newtonsoft.Json;

namespace Web.Models
{
    public class AuthorWrapper
    {
        [JsonProperty("approved")]
        public bool Approved { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("user")]
        public Author User { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}