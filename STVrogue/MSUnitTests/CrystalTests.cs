using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;

namespace MSUnitTests {
    [TestClass]
    public class CrystalTests {
        [TestMethod]
        public void TestHasCrystalTrue() {
            Player player = new Player("player");
            Crystal crystal = new Crystal("crystal");
            player.bag.Add(crystal);
            Assert.IsTrue(crystal.hasCrystal(player));
        }

        [TestMethod]
        public void TestHasCrystalFalse() {
            Player player = new Player("player");
            Crystal crystal = new Crystal("crystal");
            Assert.IsFalse(crystal.hasCrystal(player));
        }

        [TestMethod]
        public void TestUseInCombat() {
            Game game = new Game();
            Player player = new Player("player");
            player.inCombat = true;
            Crystal crystal = new Crystal("crystal");
            player.bag.Add(crystal);
            crystal.Use(game, player);
            Assert.IsTrue(!crystal.hasCrystal(player) && player.boosted);
        }

        [TestMethod]
        public void TestUseOutsideCombat() {
            Game game = new Game();
            Player player = new Player("player");
            player.inCombat = false;
            Crystal crystal = new Crystal("crystal");
            player.bag.Add(crystal);
            crystal.Use(game, player);
            Assert.IsTrue(!crystal.hasCrystal(player) && !player.boosted);
        }

        [TestMethod]
        public void TestDelete() {
            Player player = new Player("player");
            Crystal crystal = new Crystal("crystal1");
            HealingPotion healing = new HealingPotion("healing", 10);
            player.bag.Add(healing);
            player.bag.Add(crystal);
            crystal.deleteCrystal(player);
            Assert.IsTrue(!crystal.hasCrystal(player)); ;
        }

    }
}