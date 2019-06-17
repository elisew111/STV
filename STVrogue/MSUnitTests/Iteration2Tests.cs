using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using STVrogue.TestInfrastructure;
using System.Collections.Generic;
using STVrogue;

namespace MSUnitTests
{
    [TestClass]
    class Iteration2Tests
    {
        public List<GamePlay> testsuites;
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
        public void monster()
        {
            //RMonster: The HP of every monster can only decrease, and it never leaves its zone.



            //TemporalSpecification RMonster1 = new Unless(G => HelperPredicates.imp(G.getMonster(mid) != null,  G.getMonster(mid).HP == X)), G => G.getMonsters(mid).HP < X);
               
            //TemporalSpecification RMonster2 = new Always(
        }
    }
}