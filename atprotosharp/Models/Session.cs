using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace atprotosharp.Models
{
    public class Session
    {
        [JsonPropertyName("handle")]
        public string? Handle { get; set; }

        [JsonPropertyName("did")]
        public string? Did { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}
