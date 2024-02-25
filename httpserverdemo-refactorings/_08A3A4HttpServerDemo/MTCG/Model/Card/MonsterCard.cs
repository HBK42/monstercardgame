using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model.Card
{
    public class MonsterCard : Card
    {
        [JsonConstructor]
        public MonsterCard(string id, string name, int damage) : base(name, damage, id)
        {
            SetMonsterCardWeakness();
        }

   
        public MonsterCard(Element type, string name, int damage, string weakness, Element typeWeakness, string id,
                           string nameAndType, string packageId, string userId)
                           : base(type, name, damage, weakness, typeWeakness, id, nameAndType, packageId, userId)
        {
        }

        private void SetMonsterCardWeakness()
        {

            if (Name.Contains("Goblin"))
            {

                Weakness = "Dragon";
            }
            else if (Name.Contains("Knight"))
            {
                base.Weakness = "WaterSpell";
            }
            else if (Name.Contains("Dragon"))
            {
                base.Weakness = "FireElve";
            }
            else if (Name.Contains("Ork"))
            {
                base.Weakness = "Wizard";
            }
        }
    }
}
