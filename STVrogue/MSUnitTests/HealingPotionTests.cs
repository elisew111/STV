using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;

namespace MSUnitTests {
    [TestClass]
    public class HealingPotionTests {

        [TestMethod]
        public void TestHasHealingPotionTrue() {
            Player player = new Player("player");
            HealingPotion healingPotion = new HealingPotion("potion", 5);
            player.bag.Add(healingPotion);
            Assert.IsTrue(healingPotion.hasHealingPotion(player));
        }

        [TestMethod]
        public void TestHasHealingPotionFalse() {
            Player player = new Player("player");
            HealingPotion healingPotion = new HealingPotion("potion", 5);
            Assert.IsFalse(healingPotion.hasHealingPotion(player));
        }

        [TestMethod]
        public void TestUseNotMaxHP() {
            Game game = new Game();
            Player player = new Player("player");
            player.HP = 2;
            HealingPotion healingPotion = new HealingPotion("potion", 5);
            player.bag.Add(healingPotion);
            healingPotion.Use(game, player);
            Assert.IsTrue(!healingPotion.hasHealingPotion(player) && player.HP == 7);
        }

        [TestMethod]
        public void TestUseNotMaxHP2() {
            Game game = new Game();
            Player player = new Player("player");
            player.HP = 7;
            HealingPotion healingPotion = new HealingPotion("potion", 5);
            player.bag.Add(healingPotion);
            healingPotion.Use(game, player);
            Assert.IsTrue(!healingPotion.hasHealingPotion(player) && player.HP == player.HPmax);
        }

        [TestMethod]
        public void TestUseMaxHP() {
            Game game = new Game();
            Player player = new Player("player");
            player.HP = player.HPmax;
            int prevHP = player.HP;
            HealingPotion healingPotion = new HealingPotion("potion", 5);
            player.bag.Add(healingPotion);
            healingPotion.Use(game, player);
            Assert.IsTrue(!healingPotion.hasHealingPotion(player) && player.HP == player.HPmax);
        }

        [TestMethod]
        public void TestDelete() {
            Player player = new Player("player");
            HealingPotion healingPotion = new HealingPotion("potion", 5);
            Crystal crystal = new Crystal("crystal");
            player.bag.Add(crystal);
            player.bag.Add(healingPotion);
            healingPotion.deleteHealingPotion(player);
            Assert.IsTrue(!healingPotion.hasHealingPotion(player));
        }



    }
}
