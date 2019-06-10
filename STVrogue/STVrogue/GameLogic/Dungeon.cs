using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace STVrogue.GameLogic {
    /* A dungeon is made of a sequence of zones/levels. */
    public class Dungeon {
        List<Zone> zones = new List<Zone>();
        public static Node startnode;
        public static Node exitnode = null;
        public static int capacityMultiplier;
        readonly int seed = 100100;
        int amountNodes;

        public List<Zone> getZones() { return zones; }
        public Node getStartnode() { return startnode; }
        public Node getExitnode() { return exitnode; }

        public const string NotEnoughZones = "numberOfZones should be 3 or more";

        /*
         * Create a dungeon with the indicated number of zones (should be at least 3). This creates
         * the start and exit zones. The zones should be linked linearly to each other with bridges.
         */
        public Dungeon(int numberOfZones, int capacity) {
            Random pseudoRandom = new Random(seed);
            if (numberOfZones < 3) throw new ArgumentException(NotEnoughZones);
            capacityMultiplier = capacity;
            // creating the start zone:
            int numOfNodesInstartZone = pseudoRandom.Next(3, 5);
            amountNodes += numOfNodesInstartZone;
            Zone startZone = new Zone(generateID(), zoneType.STARTzone, 1, numOfNodesInstartZone);
            zones.Add(startZone);

            // adding in-between zones:
            Zone previousZone = startZone;
            for (int z = 2; z < numberOfZones; z++) {
                int numOfNodes = pseudoRandom.Next(4, 6);
                amountNodes += numOfNodes;
                Zone zone = new Zone(generateID(), zoneType.InBETWEENzone, z, numOfNodes);
                zones.Add(zone);
                connectWithBridge(previousZone, zone);
                previousZone = zone;
            }
            // creating the exit zone:
            int numOfNodesInExitZone = pseudoRandom.Next(3, 5);
            amountNodes += numOfNodesInExitZone;
            Zone exitZone = new Zone(generateID(), zoneType.EXITzone, numberOfZones, numOfNodesInExitZone);
            zones.Add(exitZone);
            connectWithBridge(previousZone, exitZone);
            seedMonstersAndItems();
        }

        public string generateID() {
            return Guid.NewGuid().ToString("N");
        }

        /* return all nodes in the dungeon. */
        public List<Node> nodes() {
            List<Node> dungeonNodes = new List<Node>();
            List<Zone> dungeonZones = this.getZones();
            foreach (Zone zone in dungeonZones) {
                List<Node> zoneNodes = zone.getNodes();
                foreach (Node node in zoneNodes) {
                    dungeonNodes.Add(node);
                }
            }
            return dungeonNodes;
        }

        /* Drop monsters and items into the dungeon. */
        public void seedMonstersAndItems() {
            Random pseudoRandom = new Random(seed);
            int amountMonsters = (int)Math.Round((double)amountNodes / 5);
            int amountHealingPotions = (int)Math.Round((double)amountNodes / 8);
            int amountCrystals = (int)Math.Round((double)amountNodes / 8);
            Node startNode = this.getStartnode();
            Node exitNode = this.getExitnode();
            List<Node> dungeonNodes = this.nodes();
            dungeonNodes.Remove(startNode);
            amountNodes--;
            dungeonNodes.Remove(exitNode);
            amountNodes--;
            for (int i = 0; i < amountMonsters; i++) {
                Monster monster = new Monster(generateID());
                int monsterLocation = pseudoRandom.Next(0, amountNodes - 1);
                Node selectedNode = dungeonNodes[monsterLocation];
                monster.location = selectedNode;
                selectedNode.monsters.Add(monster);
                Game.monsters.Add(monster);
                if (selectedNode.capacity == selectedNode.monsters.Count) {
                    dungeonNodes.Remove(selectedNode);
                    amountNodes--;
                }
            }
            for (int i = 0; i < amountHealingPotions; i++) {
                HealingPotion healingPotion = new HealingPotion(generateID(), HealingPotion.HPvalue);
                int healingPotionLocation = pseudoRandom.Next(0, amountNodes - 1);
                Node selectedNode = dungeonNodes[healingPotionLocation];
                selectedNode.items.Add(healingPotion);
                Game.items.Add(healingPotion);
            }
            for (int i = 0; i < amountCrystals; i++) {
                Crystal crystal = new Crystal(generateID());
                int crystalLocation = pseudoRandom.Next(0, amountNodes - 1);
                Node selectedNode = dungeonNodes[crystalLocation];
                selectedNode.items.Add(crystal);
                Game.items.Add(crystal);
            }
        }

        /* Link zone1 to zone2 through a bridge. The bridge should be part of zone1 (or, you can
         * alternatively convert a node in zone1 to become a bridge. Make sure that all paths from
         * zone1 to zone2 MUST pass through the bridge.
         */
        static private void connectWithBridge(Zone zone1, Zone zone2) {
            List<Node> nodes1 = zone1.getNodes(); //connect de bridge van zone 1 met de startnode van zone 2
            List<Node> nodes2 = zone2.getNodes();
            foreach (Node node1 in nodes1) {
                if (node1.type == NodeType.BRIDGE) {
                    nodes2[0].connect(node1);
                }
            }
        }
    }

    /** Different types of zones. */
    public enum zoneType {
        STARTzone,  // should contain a single start node
        EXITzone,   // should contain a single exit node
        InBETWEENzone, // should not contain start nor exit nodes
    }

    /*
     * Representing a "Zone" in a dungeon. A Zone consists of at least two nodes
     * connected to each other. The nodes in a Zone should form a connected graph.
     * That is, there is always a path in this graph from which we can go from
     * any node x to any node y in the graph.
     */
    public class Zone : GameEntity {
        List<Node> nodes = new List<Node>();
        zoneType type;
        int level;
        readonly int seed = 100100;

        public List<Node> getNodes() { return nodes; }
        public zoneType getType() { return type; }
        public int getLevel() { return level; }


        public const string LevelTooLow = "Zone level should be at least 1";

        /* Create a zone of the specified type and number of nodes. */
        public Zone(String ID, zoneType ty, int zoneLevel, int numberOfnodes) : base(ID) {
            if (zoneLevel < 1 || numberOfnodes < 2) throw new ArgumentException(LevelTooLow);
            type = ty;
            level = zoneLevel;

            if (ty == zoneType.STARTzone) //eerste node van startzone is startnode
            {
                level = 1;
                Node startnode = new Node(NodeType.STARTnode, generateID());
                startnode.capacity = 0;
                startnode.zone = this;
                nodes.Add(startnode);
                Dungeon.startnode = startnode;

                for (int i = 2; i < numberOfnodes; i++) {
                    addCommonNode();
                }
                makeBridge();
            }

            if (ty == zoneType.InBETWEENzone) {
                for (int i = 1; i < numberOfnodes; i++) {
                    addCommonNode();
                }
                makeBridge();
            }

            if (ty == zoneType.EXITzone) {
                for (int i = 1; i < numberOfnodes; i++) {
                    addCommonNode();
                }
                Node exitnode = new Node(NodeType.EXITnode, generateID());
                exitnode.capacity = 0;
                exitnode.zone = this;
                connectRandom(exitnode);
                nodes.Add(exitnode);
                Dungeon.exitnode = exitnode;
            }

            // When compiled in the Debug-build, check the following conditions:
            Debug.Assert(nodes.Count >= 2);
            Debug.Assert(ty == zoneType.STARTzone ? HelperPredicates.hasOneStartZone(this) : true);
            Debug.Assert(ty == zoneType.EXITzone ? HelperPredicates.hasOneExitZone(this) : true);
            Debug.Assert(HelperPredicates.isConnected(this));
        }

        public string generateID() {
            return Guid.NewGuid().ToString("N");
        }

        //connect met een random node die al in de nodes lijst staat
        public void connectRandom(Node node) {
            Random pseudoRandom = new Random(seed);
            int index = pseudoRandom.Next(0, nodes.Count);
            node.connect(nodes[index]);
        }

        public void addCommonNode() {
            Node commonnode = new Node(NodeType.COMMONnode, generateID());
            commonnode.capacity = Dungeon.capacityMultiplier;
            commonnode.zone = this;
            if (nodes.Count > 0)
                connectRandom(commonnode); //eerste node kan niet verbinden aan previousnode als die nog niet bestaat
            nodes.Add(commonnode);
        }

        public void makeBridge() {
            Node bridge = new Node(NodeType.BRIDGE, generateID());
            connectRandom(bridge);
            bridge.capacity = Dungeon.capacityMultiplier * level;
            bridge.zone = this;
            nodes.Add(bridge);
        }
    }

    /* Representing different types of nodes. */
    public enum NodeType {
        STARTnode,  // the starting node of the player. 
        EXITnode,   // representing the player's final destination.
        BRIDGE,
        COMMONnode  // the type of the rest of the nodes. 
    }

    /*
     * Representing a node in a dungeon.
     */
    public class Node : GameEntity {
        /* 
         * Neighbors are nodes that are considered connected to this node. 
         * The connection is bidirectional. 
         */
        public List<Node> neighbors = new List<Node>();
        public List<Creature> monsters = new List<Creature>();
        public List<Item> items = new List<Item>();

        /** the zone to which this node belongs to: */
        public Zone zone;

        public NodeType type;
        public NodeType getType() { return type; }

        public int capacity;

        /* the capacity of this node */
        public int getCapacity() {
            return capacity;
        }

        public Node(NodeType ty, String ID) : base(ID) {
            type = ty;
        }

        /* To connect this node to another node. */
        public void connect(Node nd) {
            neighbors.Add(nd); nd.neighbors.Add(this);
        }

        /* To disconnect this node from the given node. */
        public void disconnect(Node nd) {
            // this only removes the first occurrence
            neighbors.Remove(nd); nd.neighbors.Remove(this);
        }

        /* return the set of nodes reachable from this node. */
        public List<Node> reachableNodes() //werkt dit zo? niet super sure wat hier gebeurd
        {
            Node x = this;
            List<Node> seen = new List<Node>();
            List<Node> todo = new List<Node>();
            todo.Add(x);
            while (todo.Count > 0) {
                x = todo[0]; todo.RemoveAt(0);
                seen.Add(x);
                foreach (Node y in x.neighbors) {
                    if (seen.Contains(y) || todo.Contains(y)) continue;
                    todo.Add(y);
                }
            }
            return seen;
        }

        /* Check if the node nd is reachable from this node. */
        public Boolean isReachable(Node nd) {
            return reachableNodes().Contains(nd);
        }
    }
}
