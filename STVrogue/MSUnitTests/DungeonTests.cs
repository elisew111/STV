using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;

namespace MSUnitTests
{
    [TestClass]
    /* Just an example of an MSUnit test class to show how to write one. */
    public class DungeonTests
    {

        [TestMethod]
        public void WhenNrOfZonesLessThan3ThrowArgumentException()
        {
            
            try
            {
                Dungeon dungeon = new Dungeon(2, 5);
            }
            catch(ArgumentException e)
            {
                StringAssert.Contains(e.Message, Dungeon.NotEnoughZones);
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");

        }


        [TestMethod]
        public void CheckFullyConnected()
        {
            Dungeon dungeon = new Dungeon(5, 5);
            //Assert.IsTrue(false);

            Assert.IsTrue(CheckConnections(dungeon));
            Assert.Fail("oeps");

        }

        public bool CheckConnections(Dungeon dungeon)
        {
            bool result = true;
            foreach (Zone zone in dungeon.getZones())
            {
                foreach (Node node in zone.getNodes())
                {
                    if (node.neighbors.Count <= 0) result = false;

                }
            }
            return result;
        }

    }
}