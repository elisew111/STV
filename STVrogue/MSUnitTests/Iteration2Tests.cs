﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        new GamePlay("4-of-4-kills_use-2-of-2-C-in-and-out-combat-both-before-killing-final-monster.txt"),
        new GamePlay("23-06-2019_06-59-49gameplay.txt"),
        new GamePlay("23-06-2019_07-51-38gameplay.txt"),
        new GamePlay("23-06-2019_07-57-22gameplay.txt"),
        new GamePlay("23-06-2019_09-47-43gameplay.txt"),
        new GamePlay("23-06-2019_09-49-15gameplay.txt"),
        new GamePlay("23-06-2019_09-51-12gameplay.txt")
        };


        public int threshold = 3;

        [TestMethod]
        public void HPTest()
        {
            TemporalSpecification RHP = new Always(G => G.player.HP <= G.player.HPmax);
            Assert.AreEqual(Judgement.RelevantlyValid, RHP.evaluate(testsuites, threshold));
        }
        [TestMethod]
        public void Exit()
        {
            TemporalSpecification RExit = new Always(G => G.player.location.isReachable(G.dungeon.getExitnode()));
            Assert.AreEqual(Judgement.RelevantlyValid, RExit.evaluate(testsuites, threshold));
        }
        [TestMethod]
        public void NodeCap()
        {
            TemporalSpecification RNodeCap = new Always(G => HelperPredicates.forall(G.dungeon.nodes(), n => n.monsters.Count <= n.capacity));
            Assert.AreEqual(Judgement.RelevantlyValid, RNodeCap.evaluate(testsuites, threshold));
        }
        [TestMethod]
        public void Alive()
        {
            TemporalSpecification RAlive = new Unless(G => G.player.HP > 0, G => G.player.inCombat);
            Assert.AreEqual(Judgement.RelevantlyValid, RAlive.evaluate(testsuites, threshold));
        }
        [TestMethod]
        public void Item()
        {
            SpecificationFamily family = new SpecificationFamily();
            for(int x = 1;x < 6;x++)
            {
                int X = x;
                TemporalSpecification RItem = new Unless(G => G.dungeon.items.Count == X, G => G.dungeon.items.Count < X);
                family.add(RItem);
            }
            Assert.AreEqual(Judgement.RelevantlyValid, family.evaluate(testsuites, threshold));
        }
        [TestMethod]
        public void Decay()
        {
            //RDecay: when the game has no item left(lying around in nodes nor in the player’s bag), the player’s HP can only decrease.

            int[] HPValues = new int[] { 2,4,6,8,10 };
            SpecificationFamily family = new SpecificationFamily();
            foreach (int x in HPValues)
            {
                int X = x;
                TemporalSpecification RDecay = new Unless(G => G.player.HP == X && (G.dungeon.items.Count <= 0 && G.player.bag.Count <= 0), G => G.player.HP < X);
                family.add(RDecay);
            }
            Assert.AreEqual(Judgement.RelevantlyValid, family.evaluate(testsuites, threshold));

        }
        [TestMethod]
        public void monsterHP()
        {
            //RMonster: The HP of every monster can only decrease, and it never leaves its zone.

            SpecificationFamily family = new SpecificationFamily();

            
            for(int I = 1; I < 5; I++)
            {
                int i = I;
                string mid = "M" + i;
                for(int x = 1; x<6; x+=2) //max hp = 5, player attack = 2 of 4
                {
                    int X = x;
                    TemporalSpecification RMonster1 = new Unless(G => G.getMonster(mid) != null && G.getMonster(mid).HP == X, G => G.getMonster(mid).HP < X);
                    family.add(RMonster1);
                }
            }
            Assert.AreEqual(Judgement.RelevantlyValid, family.evaluate(testsuites, threshold));
        }

        [TestMethod]
        public void monsterZone()
        {
            TemporalSpecification RMonster2 = new Always( G => { bool result = true; for (int i = 0; i < 20; i++) { result = result && (G.getMonster("M" + i) == null || G.getMonster("M" + i).location.zone == G.getMonster("M" + i).prevZone); } return result; });
            
            Assert.AreEqual(Judgement.RelevantlyValid, RMonster2.evaluate(testsuites, threshold));
        }
    }
}
