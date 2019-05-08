using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace STVrogue.GameLogic
{
    /* A dungeon is made of a sequence of zones/levels. */
    public class Dungeon
    {
        List<Zone> zones = new List<Zone>();
        Node startnode;
        Node exitnode = null;
        int capacityMultiplier;

        public List<Zone> getZones() { return zones; }
        public Node getStartnode() { return startnode; }
        public Node getExitnode() { return exitnode; }

        /*
         * Create a dungeon with the indicated number of zones (should be at least 3). This creates
         * the start and exit zones. The zones should be linked linearly to each other with bridges.
         */
        public Dungeon(int numberOfZones, int capacityMultiplier)
        {
            if (numberOfZones < 3) throw new ArgumentException();
            this.capacityMultiplier = capacityMultiplier;
            // creating the start zone:
            int numOfNodesInstartZone = 2; // FIX THIS! Decide how many nodes //random?
            Zone startZone = new Zone("Z1", zoneType.STARTzone, 1, numOfNodesInstartZone);
            zones.Add(startZone);
            foreach (Node nd in startZone.getNodes())
            {
                if (nd.type == NodeType.STARTnode)
                {
                    startnode = nd; break;
                }
            }
            // adding in-between zones:
            Zone previousZone = startZone;
            for (int z = 2; z < numberOfZones; z++)
            {
                int numOfNodes = 2; // FIX THIS! Decide how many nodes //random?
                Zone zone = new Zone("Z" + z, zoneType.InBETWEENzone, 1, numOfNodes);
                zones.Add(zone);
                connectWithBridge(previousZone, zone);
                previousZone = zone;
            }
            // creating the exit zone:
            int numOfNodesInExitZone = 2; // FIX THIS! decide how many nodes
            Zone exitZone = new Zone("Z" + numberOfZones, zoneType.EXITzone, 1, numOfNodesInExitZone);
            zones.Add(exitZone);
            connectWithBridge(previousZone, exitZone);

            foreach(Node nd in exitZone.getNodes())
            {
                if (nd.type == NodeType.EXITnode)
                {
                    exitnode = nd; break;
                }
            }
        }

        /* Drop monsters and items into the dungeon. */
        public void seedMonstersAndItems()
        {
            throw new NotImplementedException();
        }


        /* Link zone1 to zone2 through a bridge. The bridge should be part of zone1 (or, you can
         * alternatively convert a node in zone1 to become a bridge. Make sure that all paths from
         * zone1 to zone2 MUST pass through the bridge.
         */
        static private void connectWithBridge(Zone zone1, Zone zone2)
        {
            //hoe weet ik welke node met welke andere node geconnect moet worden? en hoe roep ik een node aan? hoe voeg ik een node toe aan zone1?
            List<Node> nodes1 = zone1.getNodes();
            


            throw new NotImplementedException();

        }
    }

    /** Different types of zones. */
    public enum zoneType
    {
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
    public class Zone : GameEntity
    {
        List<Node> nodes = new List<Node>();
        zoneType type;
        int level;

        public List<Node> getNodes(){ return nodes; }
        public zoneType getType(){ return type; }
        public int getLevel(){ return level; }

        /* Create a zone of the specified type and number of nodes. */
        public Zone(String ID, zoneType ty, int zoneLevel, int numberOfnodes) : base(ID)
        {
            if (zoneLevel < 1 || numberOfnodes < 2) throw new ArgumentException();
            type = ty;
            level = zoneLevel;

            // TODO .. the implementation here

            int x = 1;

            Node previousnode;

            if(ty == zoneType.STARTzone) //eerste node van startzone is startnode
            {
                level = 1;
                Node startnode = new Node(STARTnode, SN);
                nodes.Add(startnode);
                x = 2;                          //als we een startnode hebben gemaakt moeten we niet ook nog een common node maken als eerste node, anders hebben we een node te veel
                previousNode = startnode;       //elke node moet verbinden met de vorige node
            }
            for (int i = x; i < numberOfnodes; i++) //voor alle nodes bij niet startzone of alle behalve de eerste bij de startzone
            {
                if (x = numberOfnodes && ty = zoneType.EXITzone)    //laatste van de exitzone is exitnode
                {
                    Node exitnode = new Node(EXITnode, EN);
                    exitnode.connect(previousnode); 
                    nodes.Add(exitnode);

                }
                else                                                //rest van de nodes zijn common nodes
                {
                    Node commonnode = new Node(COMMONnode, N);
                    if (x > 1)
                    {
                        commonnode.connect(previousnode);           //eerste node kan niet verbinden aan previousnode als die nog niet bestaat
                    }
                    nodes.Add(commonnode);
                    previousNode = commonnode;
                }
                
            }
            if (ty != zoneType.EXITzone)                            //elke zone behalve de exitzone krijgt een bridge na zn gewone nodes
            {
                Node bridge = new Node(BRIDGE, B);
                bridge.connect(previousnode);
                nodes.Add(bridge);
                
            }



            if (true) throw new NotImplementedException();

            // When compiled in the Debug-build, check the following conditions:
            Debug.Assert(nodes.Count >= 2) ;
            Debug.Assert(ty == zoneType.STARTzone ? HelperPredicates.hasOneStartZone(this) : true) ;
            Debug.Assert(ty == zoneType.EXITzone ? HelperPredicates.hasOneExitZone(this) : true) ;
            //Debug.Assert(HelperPredicates.isConnected(this)) ;
        }


    }

    /* Representing different types of nodes. */
    public enum NodeType
    {
        STARTnode,  // the starting node of the player. 
        EXITnode,   // representing the player's final destination.
        BRIDGE,
        COMMONnode  // the type of the rest of the nodes. 
    }

    /*
     * Representing a node in a dungeon.
     */
    public class Node : GameEntity
    {
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

        /* the capacity of this node */
        public int getCapacity()
        {
            throw new NotImplementedException();
        }

        public Node(NodeType ty, String ID) : base(ID)
        {
            type = ty;
        }

        /* To connect this node to another node. */
        public void connect(Node nd)
        {
            neighbors.Add(nd); nd.neighbors.Add(this);
        }

        /* To disconnect this node from the given node. */
        public void disconnect(Node nd)
        {
            // this only removes the first occurrence
            neighbors.Remove(nd); nd.neighbors.Remove(this);
        }

        /* return the set of nodes reachable from this node. */
        public List<Node> reachableNodes()
        {
            Node x = this;
            List<Node> seen = new List<Node>();
            List<Node> todo = new List<Node>();
            todo.Add(x);
            while (todo.Count > 0)
            {
                x = todo[0] ; todo.RemoveAt(0) ;
                seen.Add(x);
                foreach (Node y in x.neighbors)
                {
                    if (seen.Contains(y) || todo.Contains(y)) continue;
                    todo.Add(y);
                }
            }
            return seen;
        }

        /* Check if the node nd is reachable from this node. */
        public Boolean isReachable(Node nd)
        {
            return reachableNodes().Contains(nd);
        }
    }



}
