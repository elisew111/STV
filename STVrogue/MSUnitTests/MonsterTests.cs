using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSUnitTests {
    [TestClass]
    public class MonsterTests {

        // This test method tests the Monster.Attack() function. It checks if
        // attacking the player correctly decreases its HP.
        [TestMethod]
        public void TestMonsterAttack1() {
            Game game = new Game(3, 1);
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
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            Node monsterLocation = game.dungeon.getStartnode();
            Node targetNode = monsterLocation.neighbors.First();
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }

        // This test method tests the Monster.Move() function. It checks if
        // moving to a node that is not a neighbouring node of the player's
        // original location returns false.
        [TestMethod]
        public void TestMonsterMove2() {
            Game game = new Game(3, 1);
            CreatureSpecs cs = new CreatureSpecs();
            Monster monster = new Monster("1");
            game.whoHasTheTurn = monster;
            Node monsterLocation = game.dungeon.getStartnode();
            Node targetNode = monsterLocation;
            Assert.IsTrue(cs.MonsterMoveSpec(game, targetNode));
        }
    }
}