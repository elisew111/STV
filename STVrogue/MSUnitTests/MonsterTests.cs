using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSUnitTests {
    [TestClass]
    public class MonsterTests {

        public string generateID() {
            return Guid.NewGuid().ToString("N");
        }

        // This test method tests the Monster.Attack() function. It checks if
        // attacking the player correctly decreases its HP.
        [TestMethod]
        public void TestMonsterAttack1() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.HP = 5;
            player.inCombat = true;
            List<Zone> gameZones = game.zones();
            Zone currentZone = gameZones[2];
            player.location = currentZone.getNodes()[2];
            Node playerLocation = player.location;
            Monster monster = new Monster(generateID());
            playerLocation.monsters.Clear();
            monster.location = playerLocation;
            playerLocation.monsters.Add(monster);
            game.whoHasTheTurn = monster;
            Assert.IsTrue(cs.MonsterAttackSpec(game, player));
        }

        // This test method tests the Monster.Attack() function. It checks if
        // attacking the player correctly decreases its HP.
        [TestMethod]
        public void TestMonsterAttack2() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Player player = game.player;
            player.HP = 1;
            player.inCombat = true;
            List<Zone> gameZones = game.zones();
            Zone currentZone = gameZones[2];
            player.location = currentZone.getNodes()[2];
            Node playerLocation = player.location;
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            playerLocation.monsters.Clear();
            monster.location = playerLocation;
            playerLocation.monsters.Add(monster);
            Assert.IsTrue(cs.MonsterAttackSpec(game, player));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // fleeing to one of the neighbouring nodes of the monster's original
        // location actually moves him to said node.
        [TestMethod]
        public void TestMonsterFlee1() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.zones();
            Zone currentZone = gameZones[2];
            Node monsterLocation = currentZone.getNodes()[2];
            monster.location = monsterLocation;
            monsterLocation.monsters.Clear();
            monsterLocation.monsters.Add(monster);
            Node targetNode = monsterLocation.neighbors[0];
            targetNode.monsters.Clear();
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // fleeing to a node that is not a neighbouring node of the monster's
        // original location returns false.
        [TestMethod]
        public void TestMonsterFlee2() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.zones();
            Zone currentZone = gameZones[2];
            Node monsterLocation = currentZone.getNodes()[2];
            monster.location = monsterLocation;
            monsterLocation.monsters.Clear();
            monsterLocation.monsters.Add(monster);
            Node targetNode = monsterLocation;
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // fleeing to a node that is already at capacity returns false.
        [TestMethod]
        public void TestMonsterFlee3() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.zones();
            Zone monsterZone = gameZones[2];
            monster.location = monsterZone.getNodes()[2];
            Node targetNode = monster.location.neighbors[0];
            Monster monster2 = new Monster(generateID());
            targetNode.monsters.Clear();
            targetNode.monsters.Add(monster2);
            monster2.location = targetNode;
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // fleeing to one of the neighbouring nodes of the monster's original
        // location that is in a different zone than its original location
        // returns false.
        [TestMethod]
        public void TestMonsterFlee4() {
            Game game = new Game(8, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            Node targetNode = null;
            List<Node> gameBridges = game.bridges();
            monster.location = gameBridges[0];
            monster.location.monsters.Clear();
            monster.location.monsters.Add(monster);
            foreach (Node node in monster.location.neighbors) {
                if (node.zone != monster.location.zone) {
                    targetNode = node;
                    targetNode.monsters.Clear();
                    break;
                }
            }
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Flee() function. It checks if
        // fleeing to a node that already contains the player returns false.
        [TestMethod]
        public void TestMonsterFlee5() {
            Game game = new Game(5, 2);
            Player player = game.player;
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.zones();
            Zone monsterZone = gameZones[2];
            monster.location = monsterZone.getNodes()[2];
            Node targetNode = monster.location.neighbors[0];
            targetNode.monsters.Clear();
            targetNode.monsters.Add(monster);
            player.location = targetNode;
            Assert.IsTrue(cs.MonsterFleeSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to one of the neighbouring nodes of the monster's original
        // location actually moves him to said node.
        [TestMethod]
        public void TestMonsterMove1() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            List<Zone> gameZones = game.zones();
            Zone currentZone = gameZones[2];
            Node monsterLocation = currentZone.getNodes()[2];
            monster.location = monsterLocation;
            monsterLocation.monsters.Clear();
            monsterLocation.monsters.Add(monster);
            game.whoHasTheTurn = monster;
            Node targetNode = monsterLocation.neighbors[0];
            targetNode.monsters.Clear();
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to a node that is not a neighbouring node of the monster's
        // original location returns false.
        [TestMethod]
        public void TestMonsterMove2() {
            Game game = new Game(5, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            List<Zone> gameZones = game.zones();
            Zone currentZone = gameZones[2];
            Node monsterLocation = currentZone.getNodes()[2];
            monster.location = monsterLocation;
            game.monsters.Clear();
            monsterLocation.monsters.Clear();
            monsterLocation.monsters.Add(monster);
            game.whoHasTheTurn = monster;
            Node targetNode = monsterLocation;
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to a node that is already at capacity returns false.
        [TestMethod]
        public void TestMonsterMove3() {
            Game game = new Game(5, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            List<Zone> gameZones = game.zones();
            Zone monsterZone = gameZones[2];
            monster.location = monsterZone.getNodes()[2];
            Node targetNode = monster.location.neighbors[0];
            Monster monster2 = new Monster(generateID());
            targetNode.monsters.Clear();
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
            Game game = new Game(8, 2);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster(generateID());
            game.whoHasTheTurn = monster;
            Node targetNode = null;
            List<Node> gameBridges = game.bridges();
            monster.location = gameBridges[0];
            monster.location.monsters.Clear();
            monster.location.monsters.Add(monster);
            foreach (Node node in monster.location.neighbors) {
                if (node.zone != monster.location.zone) {
                    targetNode = node;
                    targetNode.monsters.Clear();
                    break;
                }
            }
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }
    }
}