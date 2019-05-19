using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;
using System;
using System.Collections.Generic;

namespace MSUnitTests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void TryDisconnect()
        {
            Node node1 = new Node(NodeType.COMMONnode, "test1");
            Node node2 = new Node(NodeType.COMMONnode, "test2");
            node1.connect(node2);
            node1.disconnect(node2);
            Assert.IsTrue(node1.neighbors.Count == 0);
        }
        
        [TestMethod]
        public void GetCapacity()
        {
            Node node = new Node(NodeType.COMMONnode, "test");
            node.capacity = 4;
            Assert.IsTrue(node.getCapacity() == 4);
        }

        [TestMethod]
        public void CheckReachable()
        {
            Node node1 = new Node(NodeType.COMMONnode, "test1");
            Node node2 = new Node(NodeType.COMMONnode, "test2");
            Node node3 = new Node(NodeType.COMMONnode, "test3");
            node1.connect(node2);
            node2.connect(node3);
            Assert.IsTrue(node1.isReachable(node3));
        }
    }
}
