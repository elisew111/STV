using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSUnitTests {
    [TestClass]
    public class PlayerTests {

        [TestMethod]
        public void TestPlayerMoveChangeNode() {
            Game game = new Game(3, 1);
            Player player = game.player;
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            // Assert.IsTrue(PlayerMoveSpec(game, targetNode));
        }
    }
}