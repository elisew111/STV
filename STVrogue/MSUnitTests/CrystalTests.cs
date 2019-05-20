using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;

namespace MSUnitTests
{
    [TestClass]
    public class CrystalTests
    {
        [TestMethod]
        public void TestHasCrystalTrue()
        {
            Player player = new Player("player");
            Crystal crystal = new Crystal("crystal");
            player.bag.Add(crystal);
            Assert.IsTrue(crystal.hasCrystal(player));
        }

        [TestMethod]
        public void TestHasCrystalFalse()
        {
            Player player = new Player("player");
            Crystal crystal = new Crystal("crystal");
            Assert.IsFalse(crystal.hasCrystal(player));
        }

        [TestMethod]
        public void TestUse()
        {
            Game game = new Game();
            Player player = new Player("player");
            player.inCombat = true;
            Crystal crystal = new Crystal("crystal");
            player.bag.Add(crystal);
            crystal.Use(game, player);
            Assert.IsTrue(!crystal.hasCrystal(player) && player.boosted);
        }

        [TestMethod]
        public void TestDelete()
        {
            Player player = new Player("player");
            Crystal crystal1 = new Crystal("crystal1");
            Crystal crystal2 = new Crystal("crystal2");
            player.bag.Add(crystal1);
            player.bag.Add(crystal2);
            crystal1.deleteCrystal(player);
            Assert.IsTrue(!(crystal1.hasCrystal(player) && crystal2.hasCrystal(player)));
        }

    }
}