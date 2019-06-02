using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using STVrogue.GameControl;
using System;

namespace MSUnitTests {
    [TestClass]
    public class GameTests {
        [TestMethod]
        public void TestDoNCPlayerMove() {
            CommandType name = CommandType.MOVE;
            Game game = new Game(5, 1);
            Player player = game.player;
            string[] args = { "0" };
            Command cmd = new Command(name, args);
            Assert.IsTrue(game.doNCTurn(player, cmd));
        }

        [TestMethod]
        public void TestDoNCMonsterMove() {
            CommandType name = CommandType.MOVE;
            Game game = new Game(5, 2);
            Monster monster = new Monster("somemonster");
            monster.location = game.dungeon.getStartnode();
            string[] args = { "0" };
            Command cmd = new Command(name, args);
            Assert.IsTrue(game.doNCTurn(monster, cmd));
        }

        [TestMethod]
        public void TestDoNCInvalidCreatureMove() {
            Creature creature = new Creature("someid");
            CommandType name = CommandType.MOVE;
            Game game = new Game(5, 1);
            string[] args = { "0" };
            Command cmd = new Command(name, args);
            Assert.IsFalse(game.doNCTurn(creature, cmd));
        }

        [TestMethod]
        public void TestDoNCMoveNoArgs() {
            CommandType name = CommandType.MOVE;
            Game game = new Game(5, 1);
            Player player = game.player;
            string[] args = { };
            Command cmd = new Command(name, args);
            try {
                game.doNCTurn(player, cmd);
            } catch (Exception e) {
                StringAssert.Contains(e.Message, "incorrect move args");
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");
        }

        [TestMethod]
        public void TestDoNCDoNothingPlayer() {
            CommandType name = CommandType.DoNOTHING;
            Game game = new Game(5, 1);
            Player player = game.player;
            string[] args = { };
            Command cmd = new Command(name, args);
            Assert.IsTrue(game.doNCTurn(player, cmd));
        }

        [TestMethod]
        public void TestDoNCDoNothingMonster() {
            CommandType name = CommandType.DoNOTHING;
            Game game = new Game(5, 1);
            Monster monster = new Monster("somemonster");
            string[] args = { };
            Command cmd = new Command(name, args);
            Assert.IsTrue(game.doNCTurn(monster, cmd));
        }

        [TestMethod]
        public void TestDoNCDoNothingInvalidCreature() {
            CommandType name = CommandType.DoNOTHING;
            Game game = new Game(5, 1);
            Creature creature = new Creature("somecreature");
            string[] args = { };
            Command cmd = new Command(name, args);
            Assert.IsFalse(game.doNCTurn(creature, cmd));
        }

        [TestMethod]
        public void TestDoNCUseNotPlayer() {
            HealingPotion healingPotion = new HealingPotion("pot", 5);
            CommandType name = CommandType.USE;
            Game game = new Game(5, 1);
            Creature creature = new Creature("somecreature");
            Monster monster = new Monster("somemonster");
            string[] args = { "potion" };
            Command cmd = new Command(name, args);
            Assert.IsFalse(game.doNCTurn(creature, cmd) && game.doNCTurn(monster, cmd));
        }

        [TestMethod]
        public void TestDoNCUsePotion() {
            CommandType name = CommandType.USE;
            HealingPotion healingPotion = new HealingPotion("pot", 5);

            Game game = new Game(5, 1);
            Player player = game.player;
            player.bag.Add(healingPotion);
            string[] args = { "potion" };
            Command cmd = new Command(name, args);
            Assert.IsTrue(game.doNCTurn(player, cmd));
        }

        [TestMethod]
        public void TestDoNCUseCrystal() {
            CommandType name = CommandType.USE;
            Game game = new Game(5, 1);
            Player player = game.player;
            string[] args = { "crystal" };
            Command cmd = new Command(name, args);
            Assert.IsTrue(game.doNCTurn(player, cmd));
        }

        [TestMethod]
        public void TestDoNCUseInvalidItem() {
            CommandType name = CommandType.USE;
            Game game = new Game(5, 1);
            Player player = game.player;
            string[] args = { "knife" };
            Command cmd = new Command(name, args);
            try {
                game.doNCTurn(player, cmd);
            } catch (Exception e) {
                StringAssert.Contains(e.Message, "incorrect item args");
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");
        }

        [TestMethod]
        public void TestDoNCinvalidCommand() {
            CommandType name = CommandType.ATTACK;
            Game game = new Game(5, 1);
            Player player = game.player;
            string[] args = { };
            Command cmd = new Command(name, args);
            try {
                game.doNCTurn(player, cmd);
            } catch (Exception e) {
                StringAssert.Contains(e.Message, "Ongeldig command");
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");
        }

        [TestMethod]
        public void TestDoOneCombatRoundFalse() {
            Game game = new Game(5, 1);
            Player player = game.player;
            player.inCombat = false;
            Command cmd = new Command(CommandType.ATTACK, null);
            Assert.IsFalse(game.doOneCombatRound(cmd));
        }

        [TestMethod]
        public void TestDoOneCombatRoundMonsterKill() {
            Game game = new Game(5, 1);
            Player player = game.player;
            Monster monster = new Monster("somemonster", 1);
            monster.HP = 1;
            player.location.monsters.Add(monster);
            player.inCombat = true;
            string[] args = { "0" };
            Command cmd = new Command(CommandType.ATTACK, args);
            Assert.IsTrue(game.doOneCombatRound(cmd));
        }

        [TestMethod]
        public void TestDoOneCombatRoundMonsterSurvive() {
            Game game = new Game(5, 1);
            Player player = game.player;
            Monster monster = new Monster("somemonster", 1);
            monster.HP = monster.HPmax;
            player.location.monsters.Add(monster);
            player.inCombat = true;
            string[] args = { "0" };
            Command cmd = new Command(CommandType.ATTACK, args);
            Assert.IsTrue(!game.doOneCombatRound(cmd) && game.playerstate == PlayerState.CombatStart);
        }

        [TestMethod]
        public void TestDoOneCombatRoundUseCrsytalMonsterSurvive() {
            Game game = new Game(5, 1);
            Player player = game.player;
            Monster monster = new Monster("somemonster", 1);
            monster.HP = monster.HPmax;
            player.location.monsters.Add(monster);
            player.inCombat = true;
            string[] args = { "crystal" };
            Command cmd = new Command(CommandType.USE, args);
            Assert.IsTrue(!game.doOneCombatRound(cmd) && game.playerstate == PlayerState.CombatStartAndBoosted && player.boosted == true);
        }

        [TestMethod]
        public void TestDoOneCombatRoundUsePotionMonsterSurvive() {
            Game game = new Game(5, 1);
            Player player = game.player;
            Monster monster = new Monster("somemonster", 1);
            monster.HP = monster.HPmax;
            player.location.monsters.Add(monster);
            player.inCombat = true;
            string[] args = { "potion" };
            Command cmd = new Command(CommandType.USE, args);
            Assert.IsTrue(!game.doOneCombatRound(cmd) && game.playerstate == PlayerState.CombatStart);
        }

        [TestMethod]
        public void TestDoOneCombatRoundFleeSucces() {
            Game game = new Game(5, 1);
            Node randomnode = new Node(NodeType.COMMONnode, "somenode");
            Node randomnode2 = new Node(NodeType.COMMONnode, "somenode2");
            Player player = game.player;
            Monster monster = new Monster("somemonster", 1);
            Monster monster2 = new Monster("othermonster", 1);
            player.location.monsters.Add(monster);
            player.location.neighbors[0] = randomnode;
            try { player.location.neighbors[1] = randomnode2; } catch {
                player.location.neighbors.Add(randomnode2);
            }

            player.location.neighbors[1].monsters.Add(monster2);
            player.inCombat = true;
            string[] args = { "0" };
            Command cmd = new Command(CommandType.FLEE, args);
            Assert.IsTrue(game.doOneCombatRound(cmd));
        }

        [TestMethod]
        public void TestDoOneCombatRoundBoostedFleeFails() {
            Game game = new Game(5, 1);
            Node randomnode = new Node(NodeType.COMMONnode, "somenode");
            Node randomnode2 = new Node(NodeType.COMMONnode, "somenode2");
            Player player = game.player;
            player.boosted = true;
            Monster monster = new Monster("somemonster", 1);
            Monster monster2 = new Monster("othermonster", 1);
            player.location.monsters.Add(monster);
            player.location.neighbors[0] = randomnode;
            try { player.location.neighbors[1] = randomnode2; } catch {
                player.location.neighbors.Add(randomnode2);
            }

            player.location.neighbors[1].monsters.Add(monster2);
            player.inCombat = true;
            string[] args = { "1" };
            Command cmd = new Command(CommandType.FLEE, args);
            Assert.IsTrue(!game.doOneCombatRound(cmd) && game.playerstate == PlayerState.CombatStartAndBoosted);
        }

        [TestMethod]
        public void TestDoOneCombatRoundFleeFails() {
            Game game = new Game(5, 1);
            Node randomnode = new Node(NodeType.COMMONnode, "somenode");
            Node randomnode2 = new Node(NodeType.COMMONnode, "somenode2");
            Player player = game.player;
            player.boosted = false;
            Monster monster = new Monster("somemonster", 1);
            Monster monster2 = new Monster("othermonster", 1);
            player.location.monsters.Add(monster);
            player.location.neighbors[0] = randomnode;
            try { player.location.neighbors[1] = randomnode2; } catch {
                player.location.neighbors.Add(randomnode2);
            }

            player.location.neighbors[1].monsters.Add(monster2);
            player.inCombat = true;
            string[] args = { "1" };
            Command cmd = new Command(CommandType.FLEE, args);
            Assert.IsTrue(!game.doOneCombatRound(cmd) && game.playerstate == PlayerState.CombatStart);
        }

        [TestMethod]
        public void TestDoOneCombatRoundIncorrectArgs() {
            Game game = new Game(5, 1);
            Player player = game.player;
            player.inCombat = true;
            string[] args = { };
            Command cmd = new Command(CommandType.USE, args);
            try {
                game.doAction(cmd);
            } catch (Exception e) {
                StringAssert.Contains(e.Message, "incorrect item args");
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");
        }

        [TestMethod]
        public void TestDoOneCombatRoundMonsterFlee() {
            Game game = new Game(5, 1);
            Player player = game.player;
            Monster monster = new Monster("somemonster", 0);
            monster.HP = monster.HPmax;
            player.location.monsters.Add(monster);
            player.inCombat = true;
            string[] args = { "0" };
            Command cmd = new Command(CommandType.ATTACK, args);
            Assert.IsTrue(!game.doOneCombatRound(cmd) && game.playerstate == PlayerState.CombatStart);
        }

        [TestMethod]
        public void TestDoOneCombatRoundIncorrectCMD() {
            Game game = new Game(5, 1);
            Player player = game.player;
            player.inCombat = true;
            string[] args = { };
            Command cmd = new Command(CommandType.MOVE, args);
            try {
                game.doAction(cmd);
            } catch (Exception e) {
                StringAssert.Contains(e.Message, "incorrect command");
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");
        }
    }
}