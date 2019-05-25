using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;

namespace MSUnitTests {
    [TestClass]
    /* Just an example of an MSUnit test class to show how to write one. */
    public class ZoneTests {

        [TestMethod]
        public void WhenNrOfZonesLessThan3ThrowArgumentException() {

            try {
                Dungeon dungeon = new Dungeon(2, 5);
            } catch (ArgumentException e) {
                StringAssert.Contains(e.Message, Dungeon.NotEnoughZones);
                return;
            }

            Assert.Fail("Expected exception wasn't thrown");

        }

        [TestMethod]
        public void IfLevelLessThan1ThrowArgumentException() {
            try { Zone zone = new Zone("test", zoneType.InBETWEENzone, 0, 5); } catch (ArgumentException e) {
                StringAssert.Contains(e.Message, Zone.LevelTooLow);
                return;
            }
            Assert.Fail("Expected exception wasn't thrown");
        }

        [TestMethod]
        public void AtLeastTwoNodes() {
            Dungeon dungeon = new Dungeon(3, 5);
            Assert.IsTrue(checkTwoNodes(dungeon));
        }

        public bool checkTwoNodes(Dungeon dungeon) {
            bool result = true;
            foreach (Zone zone in dungeon.getZones()) {
                if (zone.getNodes().Count < 2) result = false;
            }
            return result;
        }


        [TestMethod]
        public void CheckFullyConnected() {
            Dungeon dungeon = new Dungeon(5, 5);
            Assert.IsTrue(CheckConnections(dungeon));

        }

        public bool CheckConnections(Dungeon dungeon) {
            bool result = true;
            foreach (Zone zone in dungeon.getZones()) {
                foreach (Node node in zone.getNodes()) {
                    if (node.neighbors.Count <= 0) result = false;
                }
            }
            return result;
        }

        [TestMethod]
        public void CheckStartnodeExists() {
            Dungeon dungeon = new Dungeon(5, 5);
            Zone startzone = dungeon.getZones()[0];
            Assert.IsTrue(startzone.getType() == zoneType.STARTzone);
            Node startnode = startzone.getNodes()[0];
            Assert.IsTrue(startnode.getType() == NodeType.STARTnode);
        }

        [TestMethod]
        public void CheckExitnodeExists() {
            Dungeon dungeon = new Dungeon(3, 3);
            Assert.IsTrue(dungeon.getExitnode() != null);
        }

        [TestMethod]
        public void CheckExitZoneExists()
        {
            Dungeon dungeon = new Dungeon(3, 3);
            Zone exitzone = dungeon.getZones()[2];
            Assert.IsTrue(exitzone.getType() == zoneType.EXITzone);
            Assert.IsTrue(exitzone.getNodes().Count > 0);
        }

        [TestMethod]
        public void TestInbetweenZone()
        {
            Dungeon dungeon = new Dungeon(3, 3);
            Assert.IsTrue(dungeon.getZones()[1].getNodes().Count > 0);
        }

        [TestMethod]
        public void OneBridgePerZone() {
            Zone zone = new Zone("test", zoneType.InBETWEENzone, 3, 5);
            List<Node> nodes = zone.getNodes();
            int counter = 0;
            foreach (Node node in nodes) {
                if (node.type == NodeType.BRIDGE) counter += 1;
            }
            Assert.IsTrue(counter == 1);
        }

        [TestMethod]
        public void CheckLevel() {
            Zone zone = new Zone("test", zoneType.InBETWEENzone, 3, 5);
            Assert.IsTrue(zone.getLevel() == 3);
        }

        [TestMethod]
        public void testSeeding() {
            Zone zone = new Zone("zone", zoneType.InBETWEENzone, 4, 3);
            Dungeon.seedMonstersAndItems(zone);
            Node node1 = zone.getNodes()[0];
            Node node2 = zone.getNodes()[1];

            Assert.IsTrue((node2.monsters.Count > 0 || node2.capacity == 0) && node1.items.Count == 2);
        }

    }
}