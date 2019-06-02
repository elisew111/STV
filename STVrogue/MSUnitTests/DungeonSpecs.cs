using System;

namespace STVrogue.GameLogic {
    public class DungeonSpecs {
        public Boolean ValidSpec(Dungeon d) {
            return FullyConnectedSpec(d) && EnoughNodesSpec(d) && ExitnodeReachableSpec(d) && BridgesSpec(d);
        }

        public Boolean FullyConnectedSpec(Dungeon d) {
            bool result = true;
            foreach (Zone zone in d.getZones()) {
                foreach (Node node in zone.getNodes()) {
                    if (node.neighbors.Count <= 0) result = false;
                }
            }
            return result;
        }

        public Boolean EnoughNodesSpec(Dungeon d) {
            bool result = true;
            foreach (Zone zone in d.getZones()) {
                if (zone.getNodes().Count < 2) result = false;
            }
            return result;
        }

        public Boolean ExitnodeReachableSpec(Dungeon d) {
            return d.getStartnode().isReachable(d.getExitnode());
        }

        public Boolean BridgesSpec(Dungeon d) {
            bool result = true;

            foreach (Zone zone in d.getZones()) {
                foreach (Node node in zone.getNodes()) {
                    foreach (Node neighbor in node.neighbors) {
                        if (!zone.getNodes().Contains(neighbor) && neighbor.type != NodeType.BRIDGE && node.type != NodeType.BRIDGE) { result = false; }
                    }
                }
            }
            return result;
        }

    }
}
