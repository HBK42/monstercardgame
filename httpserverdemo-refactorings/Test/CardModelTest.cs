using _08A3A4HttpServerDemo.MTCG.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class CardModelTest
    {
        [Test]
        public void ToString_ReturnsCorrectStringRepresentation()
        {
            // Arrange
            Element type = Element.FIRE;
            string name = "Fireball";
            int damage = 30;
            string weakness = "Water";
            Element typeWeakness = Element.WATER;
            string id = "spell123";
            string nameAndType = "fireball";
            string packageId = "pack456";
            string userId = "user789";

            SpellCard spellCard = new SpellCard(type, name, damage, weakness, typeWeakness, id, nameAndType, packageId, userId);

            string expectedString = $"Card{{type=FIRE, name='{name}', damage={damage}, typeWeakness=WATER, " +
                                    $"id='{id}', nameAndType='{name.ToLowerInvariant()}', packageId='{packageId}', userId='{userId}'}}";

            // Act
            string actualString = spellCard.ToString();

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void SetMonsterCardWeakness_Goblin_CorrectWeaknessSet()
        {
            // Arrange
            string id = "1";
            string name = "Goblin King";
            int damage = 20;

            // Act
            MonsterCard monsterCard = new MonsterCard(id, name, damage);

            // Assert
            Assert.AreEqual("Dragon", monsterCard.Weakness);
        }

        [Test]
        public void SetMonsterCardWeakness_Knight_CorrectWeaknessSet()
        {
            // Arrange
            string id = "2";
            string name = "Knight of the Lake";
            int damage = 25;

            // Act
            MonsterCard monsterCard = new MonsterCard(id, name, damage);

            // Assert
            Assert.AreEqual("WaterSpell", monsterCard.Weakness);
        }

        [Test]
        public void SetMonsterCardWeakness_Dragon_CorrectWeaknessSet()
        {
            // Arrange
            string id = "3";
            string name = "Fire Dragon";
            int damage = 30;

            // Act
            MonsterCard monsterCard = new MonsterCard(id, name, damage);

            // Assert
            Assert.AreEqual("FireElve", monsterCard.Weakness);
        }

        [Test]
        public void SetMonsterCardWeakness_Ork_CorrectWeaknessSet()
        {
            // Arrange
            string id = "4";
            string name = "Dark Ork";
            int damage = 28;

            // Act
            MonsterCard monsterCard = new MonsterCard(id, name, damage);

            // Assert
            Assert.AreEqual("Wizard", monsterCard.Weakness);
        }
    }
}
