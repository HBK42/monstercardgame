using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model
{
    public class Package
    {
        [JsonProperty("cost")]
        public int PackageCost { get; } = 5;

        [JsonProperty("packageId")]
        public string Id { get; set; }

        public Package()
        {
            // Default constructor
        }

        public Package(string id)
        {
            Id = id;
        }
    }
}

