using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model.Card
{
    public class SpellCard: Card
    {
        [JsonConstructor]
        public SpellCard(string id, string name, int damage) : base(name, damage, id)
        { 
        }

        public SpellCard(Element type, string name, int damage, string weakness, Element typeWeakness,
                         string id, string nameAndType, string packageId, string userId)
                         : base(type, name, damage, weakness, typeWeakness, id, nameAndType, packageId, userId)
        {
        }
    }
}
