using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _08A3A4HttpServerDemo.MTCG.Model.Card
{
    public abstract class Card
    {
        [JsonProperty("Type")]
       public Element Type { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Damage")]
        public int Damage { get; set; }

        [JsonProperty("Weakness")]
         public string Weakness { get; set; } = string.Empty;

        [JsonProperty("TypeWeakness")]
        public Element TypeWeakness { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("NameAndType")]
        public string NameAndType { get; set; }

        [JsonProperty("packageId")]
        public string PackageId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        public Card(string name, int damage, string id)
        {
            Id = id;
            Name = name;
            Damage = damage;
            NameAndType = name.ToLowerInvariant();

            if (Name.Contains("Water"))
            {
                Type = Element.WATER;
                TypeWeakness = Element.NORMAL;
            }
            else if (Name.Contains("Fire"))
            {
                Type = Element.FIRE;
                TypeWeakness = Element.WATER;
            }
            else
            {
                Type = Element.NORMAL;
                TypeWeakness = Element.FIRE;
                NameAndType = (Type + "" + Name).ToLowerInvariant();
            }
        }

        public Card(Element type, string name, int damage, string weakness,
                    Element typeWeakness, string id, string nameAndType, string packageId, string userId)
        {
            Type = type;
            Name = name;
            Damage = damage;
            Weakness = weakness;
            TypeWeakness = typeWeakness;
            Id = id;
            NameAndType = nameAndType;
            PackageId = packageId;
            UserId = userId;
        }

        public void ChangePackageId(string packageId)
        {
            PackageId = packageId;
        }

        public void ChangeUserId(string userId)
        {
            UserId = userId;
        }

        public override string ToString()
        {
            return $"Card{{type={Type}, name='{Name}', damage={Damage}, typeWeakness={TypeWeakness}, " +
                   $"id='{Id}', nameAndType='{NameAndType}', packageId='{PackageId}', userId='{UserId}'}}";
        }
    }
}
