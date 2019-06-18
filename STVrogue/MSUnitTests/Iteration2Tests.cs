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
        public List<GamePlay> testsuites = new List<GamePlay>
        { new GamePlay("17-06-2019_02-13-40gameplay.txt"),
        new GamePlay("18-06-2019_12-24-00gameplay.txt"),
        new GamePlay("18-06-2019_12-28-04gameplay.txt")};


        public int threshold = 3;

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
            foreach (int X in NrOfItems)
            {
                TemporalSpecification RItem = new Unless(Game => Game.items.Count == X, Game => Game.items.Count <= X);
                Assert.IsTrue(RItem.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
            }
        }
        [TestMethod]
        public void Decay()
        {
            //RDecay: when the game has no item left(lying around in nodes nor in the player’s bag), the player’s HP can only decrease.

            int[] HPvalues = { 2, 4, 8, 16 };
            foreach (int X in HPvalues)
            {
                TemporalSpecification RDecay = new Unless(Game => Game.player.HP == X && (Game.items.Count <= 0 && Game.player.bag.Count <= 0), Game => Game.player.HP <= X);
                Assert.IsTrue(RDecay.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);

            }
            
        }
        [TestMethod]
        public void monsterHP()
        {
            //RMonster: The HP of every monster can only decrease, and it never leaves its zone.

            for(int i = 0; i < 20; i++)
            {
                string mid = "M" + i + 1;
                for(int X = 1; X<10;X++)
                {
                    TemporalSpecification RMonster1 = new Unless(G => HelperPredicates.imp(G.getMonster(mid) == null, G.getMonster(mid).HP == X), G => G.getMonster(mid).HP < X);
                    Assert.IsTrue(RMonster1.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
                }
                
            }

            
        }

        [TestMethod]
        public void monsterZone()
        {
            for (int i = 0; i < 20; i++)
            {
                string mid = "M" + i + 1;
                TemporalSpecification RMonster2 = new Always(G => HelperPredicates.imp(G.getMonster(mid) != null, G.getMonster(mid).location.zone == G.getMonster(mid).prevZone));
                Assert.IsTrue(RMonster2.evaluate(testsuites, threshold) == Judgement.RelevantlyValid);
            }
        }
    }
}