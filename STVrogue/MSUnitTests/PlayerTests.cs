using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System.Collections.Generic;
using System.Linq;

namespace MSUnitTests {
    [TestClass]
    public class PlayerTests {

        // This test method tests the Player.Attack() function. It checks if
        // attacking a monster correctly decreases its HP.
        [TestMethod]
        public void TestPlayerAttack1() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.attackRating = 3;
            player.inCombat = true;
            Node playerLocation = player.location;          
            Monster monster = new Monster("1");
            monster.HPmax = 10;
            monster.HP = 10;
            playerLocation.monsters.Add(monster);
            Assert.IsTrue(cs.PlayerAttackSpec(game, monster));
        }

        // This test method tests the Player.Attack() function. It checks if
        // attacking a monster so that its HP is <= 0 correctly registers as
        // a kill.
        [TestMethod]
        public void TestPlayerAttack2() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.attackRating = 5;
            player.inCombat = true;
            Node playerLocation = player.location;
            Monster monster = new Monster("1");
            monster.HPmax = 3;
            monster.HP = 3;
            playerLocation.monsters.Add(monster);
            Assert.IsTrue(cs.PlayerAttackSpec(game, monster));
        }

        // This test method tests the Player.Attack() function. It checks if
        // attacking a monster correctly decreases its HP when the player is 
        // boosted.
        [TestMethod]
        public void TestPlayerAttack3() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.attackRating = 3;
            player.inCombat = true;
            player.boosted = true;
            Node playerLocation = player.location;
            Monster monster = new Monster("1");
            monster.HPmax = 10;
            monster.HP = 10;
            playerLocation.monsters.Add(monster);
            Assert.IsTrue(cs.PlayerAttackSpec(game, monster));
        }

        // This test method tests the Player.Attack() function. It checks if
        // attacking a monster so that its HP is <= 0 correctly registers as
        // a kill when the player is boosted.
        [TestMethod]
        public void TestPlayerAttack4() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.attackRating = 1;
            player.inCombat = true;
            player.boosted = true;
            Node playerLocation = player.location;
            Monster monster = new Monster("1");
            monster.HPmax = 5;
            monster.HP = 5;
            playerLocation.monsters.Add(monster);
            Assert.IsTrue(cs.PlayerAttackSpec(game, monster));
        }

        // This test method tests the Player.Move() function. It checks if
        // moving to one of the neighbouring nodes of the player's original
        // location actually moves him to said node.
        [TestMethod]
        public void TestPlayerMove1() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            Assert.IsTrue(cs.PlayerMoveSpec(game, targetNode));
        }

        // This test method tests the Player.Move() function. It checks if
        // moving to a node that is not a neighbouring node of the player's
        // original location returns false.
        [TestMethod]
        public void TestPlayerMove2() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Node targetNode = game.player.location;
            Assert.IsTrue(cs.PlayerMoveSpec(game, targetNode));
        }

        // This test method tests the Player.Move() function. It checks if
        // moving to one of the neighbouring nodes of the player's original
        // location results in all the items in that node being put in the 
        // player's bag.
        [TestMethod]
        public void TestPlayerMove3() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Crystal crystal = new Crystal("1");
            HealingPotion healingPotion = new HealingPotion("2", 5);
            Player player = game.player;
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            targetNode.items.Add(crystal);
            targetNode.items.Add(healingPotion);
            Assert.IsTrue(cs.PlayerMoveSpec(game, targetNode));
        }

        // This test method tests the Ptayer.Move() function. It checks if
        // moving to one of the neighbouring nodes of the player's original
        // location that contains a monster correctly sets the player's 
        // 'inCombat' variable to true.
        [TestMethod]
        public void TestPlayerMove4() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            Monster monster = new Monster("1");
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            targetNode.monsters.Add(monster);
            Assert.IsTrue(cs.PlayerMoveSpec(game, targetNode));
        }

        // This test method tests the Player.Flee() function. It checks if
        // fleeing to one of the neighbouring nodes of the player's original
        // location actually moves him to said node.
        [TestMethod]
        public void TestPlayerFlee1() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            Assert.IsTrue(cs.PlayerFleeSpec(game, targetNode));
        }

        // This test method tests the Player.Flee() function. It checks if
        // fleeing to a node that is not a neighbouring node of the player's
        // original location returns false.
        [TestMethod]
        public void TestPlayerFlee2() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Node targetNode = game.player.location;
            Assert.IsTrue(cs.PlayerFleeSpec(game, targetNode));
        }

        // This test method tests the Player.Flee() function. It checks if
        // fleeing to one of the neighbouring nodes of the player's original
        // location results in all the items in that node being put in the
        // player's bag.
        [TestMethod]
        public void TestPlayerFlee3() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Crystal crystal = new Crystal("1");
            HealingPotion healingPotion = new HealingPotion("2", 5);
            Player player = game.player;
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            targetNode.items.Add(crystal);
            targetNode.items.Add(healingPotion);
            Assert.IsTrue(cs.PlayerFleeSpec(game, targetNode));
        }

        // This test method tests the Ptayer.Flee() function. It checks if
        // fleeing to one of the neighbouring nodes of the player's original
        // location that contains a monster returns false.
        [TestMethod]
        public void TestPlayerFlee4() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            Monster monster = new Monster("1");
            Node playerLocation = player.location;
            Node targetNode = playerLocation.neighbors.First();
            targetNode.monsters.Add(monster);
            Assert.IsTrue(cs.PlayerFleeSpec(game, targetNode));
        }
    }
}