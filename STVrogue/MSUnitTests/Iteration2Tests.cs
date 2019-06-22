using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using STVrogue.TestInfrastructure;
using System.Collections.Generic;
using STVrogue;

namespace MSUnitTests
{
    [TestClass]
    public class Iteration2Tests
    {
        public GamePlay[] testsuites = new GamePlay[]
         {
        new GamePlay("0-of-2-kills_all-items-out-combat-HP-at-10-of-10-HP.txt"),
        new GamePlay("0-of-2-kills_no_items.txt"),
        new GamePlay("0-of-2-kills_use-1-of-1-C-out-combat.txt"),
        new GamePlay("0-of-2-kills_use-1-of-1-HP-at-10-of-10-HP-out-combat.txt"),
        new GamePlay("1-of-2-kills_all-items-out-combat-HP-at-10-of-10-HP.txt"),
        new GamePlay("1-of-2-kills_no-items.txt"),
        new GamePlay("1-of-2-kills_use-1-of-1-C-in-combat.txt"),
        new GamePlay("1-of-2-kills_use-1-of-1-HP-at-8-of-10-HP-in-combat.txt"),
        new GamePlay("1-of-2-kills_use-1-of-1-HP-at-8-of-10-HP-in-combat-and-1-of-1-C-in-combat.txt"),
        new GamePlay("1-of-3-kills_use-1-of-2-HP-at-6-of-10-HP-out-combat.txt"),
        new GamePlay("1-of-4-kills_no-items_player-dies.txt"),
        new GamePlay("2-of-2-kills_no-items.txt"),
        new GamePlay("2-of-2-kills_use-1-of-1-C-in-combat-before-killing-final-monster.txt"),
        new GamePlay("2-of-2-kills_use-1-of-1-HP-at-8-of-10-HP-in-combat-and-1-of-1-C-in-combat-both-before-killing-final-monster.txt"),
        new GamePlay("2-of-2-kills_use-1-of-1-HP-at-8-of-10-HP-in-combat-before-killing-final-monster.txt"),
        new GamePlay("2-of-2-kills_use-1-of-1-HP-at-8-of-10-HP-out-combat-and-1-of-1-C-out-combat-both-after-killing-final-monster.txt"),
        new GamePlay("2-of-2-kills_use-1-of-1-HP-at-10-of-10-HP-out-combat-and-1-of-1-C-in-combat-both-before-killing-final-monster.txt"),
        new GamePlay("2-of-5-kills_no-items_player-dies.txt"),
        new GamePlay("3-of-3-kills_use-2-of-2-HP-at-2-and-7-HP-out-combat-both-after-killing-final-monster.txt"),
        new GamePlay("4-of-4-kills_use-2-of-2-C-in-and-out-combat-both-before-killing-final-monster.txt")
        };


        public int threshold = 1;

        [TestMethod]
        public void HPTest()
        {
            TemporalSpecification RHP = new Always(G => G.player.HP <= G.player.HPmax);
            Assert.IsTrue(RHP.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }
        [TestMethod]
        public void Exit()
        {
            TemporalSpecification RExit = new Always(G => G.player.location.isReachable(G.dungeon.getExitnode()));
            Assert.IsTrue(RExit.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }
        [TestMethod]
        public void NodeCap()
        {
            TemporalSpecification RNodeCap = new Always(G => HelperPredicates.forall(G.dungeon.nodes(), n => n.monsters.Count <= n.capacity));
            Assert.IsTrue(RNodeCap.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }
        [TestMethod]
        public void Alive()
        {
            TemporalSpecification RAlive = new Unless(G => G.player.inCombat, G => G.player.HP > 0);
            Assert.IsTrue(RAlive.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }
        [TestMethod]
        public void Item()
        {
            int[] NrOfItems = { 2, 4, 8, 16 };
            SpecificationFamily family = new SpecificationFamily();
            foreach (int x in NrOfItems)
            {
                int X = x;
                TemporalSpecification RItem = new Unless(Game => Game.items.Count == X, Game => Game.items.Count <= X);
                family.add(RItem);
            }
            Assert.IsTrue(family.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }
        [TestMethod]
        public void Decay()
        {
            //RDecay: when the game has no item left(lying around in nodes nor in the player’s bag), the player’s HP can only decrease.

            int[] HPvalues = { 2, 4, 8, 16 };
            SpecificationFamily family = new SpecificationFamily();
            foreach (int x in HPvalues)
            {
                int X = x;
                TemporalSpecification RDecay = new Unless(Game => Game.player.HP == X && (Game.items.Count <= 0 && Game.player.bag.Count <= 0), Game => Game.player.HP <= X);
                family.add(RDecay);
            }
            Assert.IsTrue(family.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
            
        }
        [TestMethod]
        public void monsterHP()
        {
            //RMonster: The HP of every monster can only decrease, and it never leaves its zone.

            SpecificationFamily family = new SpecificationFamily();
            for(int i = 0; i < 20; i++)
            {
                string mid = "M" + i + 1;
                for(int x = 1; x<10;x++)
                {
                    int X = x;
                    TemporalSpecification RMonster1 = new Unless(G => G.getMonster(mid) != null && G.getMonster(mid).HP == X, G => G.getMonster(mid).HP < X);
                    family.add(RMonster1);
                }
            }
            Assert.IsTrue(family.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }

        [TestMethod]
        public void monsterZone()
        {
            SpecificationFamily family = new SpecificationFamily();
            for (int I = 0; I < 20; I++)
            {
                int i = I;
                string mid = "M" + i + 1;
                TemporalSpecification RMonster2 = new Always(G => G.getMonster(mid) != null && G.getMonster(mid).location.zone == G.getMonster(mid).prevZone);
            //new Always(G => HelperPredicates.imp(G.getMonster(mid) != null, G.getMonster(mid).location.zone == G.getMonster(mid).prevZone));
               
            }
            Assert.IsTrue(family.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
        }
    }
}