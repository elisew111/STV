using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System.Collections.Generic;
using System.Linq;

namespace MSUnitTests {
    [TestClass]
    public class MonsterTests {

        // This test method tests the Monster.Attack() function. It checks if
        // attacking the player correctly decreases its HP.
        [TestMethod]
        public void TestMonsterAttack1() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.HP = 5;
            player.inCombat = true;
            Node playerLocation = player.location;
            Monster monster = new Monster("1");
            playerLocation.monsters.Add(monster);
            game.whoHasTheTurn = monster;
            Assert.IsTrue(cs.MonsterAttackSpec(game, player));
        }

        // This test method tests the Monster.Attack() function. It checks if
        // attacking the player correctly decreases its HP.
        [TestMethod]
        public void TestMonsterAttack2() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.HP = 1;
            player.inCombat = true;
            Node playerLocation = player.location;
            Monster monster = new Monster("1");
            playerLocation.monsters.Add(monster);
            game.whoHasTheTurn = monster;
            Assert.IsTrue(cs.MonsterAttackSpec(game, player));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to one of the neighbouring nodes of the monster's original
        // location actually moves him to said node.
        [TestMethod]
        public void TestMonsterMove1() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = monster.location.neighbors.First();
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to a node that is not a neighbouring node of the player's
        // original location returns false.
        [TestMethod]
        public void TestMonsterMove2() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = game.dungeon.getStartnode();
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to a node that is already at capacity returns false.
        [TestMethod]
        public void TestMonsterMove3() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = monster.location.neighbors.First();
            Monster monster2 = new Monster("2");
            targetNode.monsters.Add(monster2);
            monster2.location = targetNode;
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to one of the neighbouring nodes of the monster's original
        // location that is in a different zone than its original location
        // returns false.
        [TestMethod]
        public void TestMonsterMove4() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.dungeon.getZones();
            Zone firstZone = gameZones[0];
            Zone secondZone = gameZones[1];
            Node lastNodeFirstZone = firstZone.getNodes().Last();
            monster.location = lastNodeFirstZone;
            Node firstNodeSecondZone = secondZone.getNodes().First();
            Assert.IsTrue(cs.MonsterMoveSpec(game, firstNodeSecondZone));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // moving to one of the neighbouring nodes of the monster's original
        // location actually moves him to said node.
        [TestMethod]
        public void TestMonsterFlee1() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = monster.location.neighbors.First();
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // moving to a node that is not a neighbouring node of the player's
        // original location returns false.
        [TestMethod]
        public void TestMonsterFlee2() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = game.dungeon.getStartnode();
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // moving to a node that is already at capacity returns false.
        [TestMethod]
        public void TestMonsterFlee3() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = monster.location.neighbors.First();
            Monster monster2 = new Monster("2");
            targetNode.monsters.Add(monster2);
            monster2.location = targetNode;
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // moving to a node that already contains the player returns false.
        [TestMethod]
        public void TestMonsterFlee4() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            monster.location = game.dungeon.getStartnode();
            Node targetNode = monster.location.neighbors.First();
            Player player = game.player;
            player.location = targetNode;
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // moving to one of the neighbouring nodes of the monster's original
        // location that is in a different zone than its original location
        // returns false.
        [TestMethod]
        public void TestMonsterFlee5() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.dungeon.getZones();
            Zone firstZone = gameZones[0];
            Zone secondZone = gameZones[1];
            Node lastNodeFirstZone = firstZone.getNodes().Last();
            monster.location = lastNodeFirstZone;
            Node firstNodeSecondZone = secondZone.getNodes().First();
            Assert.IsTrue(cs.MonsterFleeSpec(game, firstNodeSecondZone));
        }
    }
}