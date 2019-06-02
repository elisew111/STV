using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;

namespace MSUnitTests {
    [TestClass]
    /* Just an example of an MSUnit test class to show how to write one. */
    public class DungeonTests {
        [TestMethod]
        public void CheckValid() {
            Dungeon dungeon = new Dungeon(5, 5);
            DungeonSpecs ds = new DungeonSpecs();
            Assert.IsTrue(ds.ValidSpec(dungeon));
        }

        [TestMethod]
        public void CheckFullyConnected() {
            Dungeon dungeon = new Dungeon(5, 5);
            DungeonSpecs ds = new DungeonSpecs();
            Assert.IsTrue(ds.FullyConnectedSpec(dungeon));

        }

        [TestMethod]
        public void AtLeastTwoNodes() {
            Dungeon dungeon = new Dungeon(5, 5);
            DungeonSpecs ds = new DungeonSpecs();
            Assert.IsTrue(ds.EnoughNodesSpec(dungeon));
        }

        [TestMethod]
        public void ExitReachable() {
            Dungeon dungeon = new Dungeon(5, 5);
            DungeonSpecs ds = new DungeonSpecs();
            Assert.IsTrue(ds.ExitnodeReachableSpec(dungeon));
        }

        [TestMethod]
        public void CheckBridges() {
            Dungeon dungeon = new Dungeon(5, 5);
            DungeonSpecs ds = new DungeonSpecs();
            Assert.IsTrue(ds.BridgesSpec(dungeon));
        }

    }
}
