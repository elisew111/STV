using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;

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
        public void AtLeastTwoNodes()
        {
            Dungeon dungeon = new Dungeon(3, 5);
            Assert.IsTrue(checkTwoNodes(dungeon));
        }
        
        public bool checkTwoNodes(Dungeon dungeon)
        {
            bool result = true;
            foreach(Zone zone in dungeon.getZones())
            {
                if (zone.getNodes().Count < 2) result = false;
            }
            return result;
        }


        [TestMethod]
        public void CheckFullyConnected()
        {
            Dungeon dungeon = new Dungeon(5, 5);
            Assert.IsTrue(CheckConnections(dungeon));

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

        [TestMethod]
        public void CheckStartnodeExists()
        {
            Dungeon dungeon = new Dungeon(5, 5);
            Zone startzone = dungeon.getZones()[0];
            Assert.IsTrue(startzone.getType() == zoneType.STARTzone);
            Node startnode = startzone.getNodes()[0];
            Assert.IsTrue(startnode.getType() == NodeType.STARTnode);
        }

        [TestMethod]
        public void CheckExitnodeExists()
        {
            Zone zone = new Zone("test", zoneType.EXITzone, 3, 5);
            List<Node> nodes = zone.getNodes();
            int counter = 0;
            foreach(Node node in nodes)
            {
                if (node.type == NodeType.EXITnode) counter += 1;
            }
            Assert.IsTrue(counter == 1);
        }



        [TestMethod]
        public void OneBridgePerZone()
        {
            Zone zone = new Zone("test", zoneType.InBETWEENzone, 3, 5);
            List<Node> nodes = zone.getNodes();
            int counter = 0;
            foreach(Node node in nodes)
            {
                if (node.type == NodeType.BRIDGE) counter += 1;
            }
            Assert.IsTrue(counter == 1);
        }

    }
}