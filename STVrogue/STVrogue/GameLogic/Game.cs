using System;
using System.Collections.Generic;
using STVrogue.GameControl;

namespace STVrogue.GameLogic {

    public enum PlayerState {
        NOTinCombat,
        CombatStart, CombatCommitted, CombatMTR,
        CombatStartAndBoosted, CombatComittedAndBoosted, CombatMTRAndBoosted,
        CombatEnd,
        Dead
    }

    /* This class represents the whole game state of STV-Rogue */
    public class Game {
        public Random r = new Random();
        public static int seed = 100100;
        public Random pseudorandom = new Random(seed);
        public Player player;
        /* all monsters currently live in the game. */
        public static List<Monster> monsters = new List<Monster>();
        /* all items in the game */
        public static List<Item> items = new List<Item>();
        public Crystal crystal = new Crystal("testCrystal");
        HealingPotion healingPotion = new HealingPotion("heal", 5);
        /* The dungeon */
        public Dungeon dungeon;
        public Node n;
        /* To count the number of passed turns. */
        public int turnNumber = 0;
        /* The creature that currently has the turn. */
        public Creature whoHasTheTurn;

        public List<Monster> getMonsters() { return monsters; }

        public PlayerState playerstate;

        public int z_; // no real use except for debug purposes

        public Game() { }

        /*
         * Create a game with a dungeon of the specified level and capacityMultiplier.
         * This also creates a proper instance of Player.
         */
        public Game(int level, int capacityMultiplier) {
            dungeon = new Dungeon(level, capacityMultiplier);
            player = new Player("player");
        }

        /* return all nodes in the game. */
        public List<Node> nodes() {
            List<Node> gameNodes = new List<Node>();
            List<Zone> gameZones = this.zones();
            foreach (Zone zone in gameZones) {
                List<Node> zoneNodes = zone.getNodes();
                foreach (Node node in zoneNodes) {
                    NodeType nodeType = node.getType();
                    if (nodeType is NodeType.STARTnode || nodeType is NodeType.EXITnode || nodeType is NodeType.COMMONnode)
                        gameNodes.Add(node);
                }
            }
            return gameNodes;
        }

        /* return all bridges in the game. */
        public List<Node> bridges() {
            List<Node> gameNodes = new List<Node>();
            List<Zone> gameZones = this.zones();
            foreach (Zone zone in gameZones) {
                List<Node> zoneNodes = zone.getNodes();
                foreach (Node node in zoneNodes) {
                    NodeType nodeType = node.getType();
                    if (nodeType is NodeType.BRIDGE)
                        gameNodes.Add(node);
                }
            }
            return gameNodes;
        }

        public Monster getMonster(string id)
        {
            Monster monster = null;
            foreach(Monster m in Game.monsters)
            {
                if (m.ID == id) { monster = m; }
            }
            return monster;
        }

        /* return all zones in the game. */
        public List<Zone> zones() {
            return dungeon.getZones();
        }

        /* 
         * Return the monster with the given id, if it still lives in the game. 
         * If the monster does not exist anymore, null is returned.
         */
        public Monster monster(String id) {
            foreach (Monster monster in Game.monsters)
                if (monster.ID == id)
                    return monster;
            return null;
        }

        /* 
         * Return the node with the given id. If the node does not exist, null is returned.
         */
        public Node node(String id) {
            List<Node> gameNodes = this.nodes();
            foreach (Node node in gameNodes)
                if (node.ID == id)
                    return node;
            return null;
        }

        /* 
        * Return the bridge with the given id. If the bridge does not exist, null is returned.
        */
        public Node bridge(String id) {
            List<Node> gameBridges = this.bridges();
            foreach (Node bridge in gameBridges)
                if (bridge.ID == id)
                    return bridge;
            return null;
        }

        /* 
        * Return the zone with the given id. If the zone does not exist, null is returned.
        */
        public Zone zone(String id) {
            List<Zone> gameZones = this.zones();
            foreach (Zone zone in gameZones)
                if (zone.ID == id)
                    return zone;
            return null;
        }

        /* Check if a monster with the given id still lives in the game. */
        public Boolean monsterExists(String id) {
            return monster(id) != null;
        }

        /* Check if a node with the given id exists in the game. */
        public Boolean nodeExists(String id) {
            return node(id) != null;
        }

        /* Check if a bridge with the given id exists in the game. */
        public Boolean bridgeExists(String id) {
            return bridge(id) != null;
        }

        /* Check if a zone with the given id exists in the game. */
        public Boolean zoneExists(String id) {
            return zone(id) != null;
        }

        /*
         * Update the game by a single turn, carried out by the creature C (which can
         * be either the player or a monster). The command cmd specifies which action
         * C wants to do.
         * It returns true if the command can be successfully carried out, and else
         * false.
         */
        public Boolean doNCTurn(Creature C, Command cmd) {
            foreach (Monster monster in monsters) {
                int roll = pseudorandom.Next(0, 2);
                // 50% chance to move to a neighbouring node and 50% chance to do nothing.
                if (roll == 1) {
                    int targetNode = pseudorandom.Next(0, monster.location.neighbors.Count - 1);
                    monster.Move(this, monster.location.neighbors[targetNode]);
                }
            }
            String s = cmd.returnArgs();
            switch (cmd.name) {
                case CommandType.MOVE:
                    if (s != "none") {
                        int move = Int32.Parse(s);
                        return C.Move(this, C.location.neighbors[move]);
                    } else
                        throw new Exception("Incorrect movement argument");
                case CommandType.DoNOTHING:
                    return true;
                case CommandType.USE:
                    if (s == "potion") {
                        healingPotion.Use(this, player);
                        return true;
                    } else if (s == "crystal") {
                        crystal.Use(this, player);
                        return true;
                    } else
                        throw new Exception("Incorrect item argument");
                default:
                    throw new Exception("Incorrect argument");
            }
        }

        /*
         * Update the game with an entire combat round. This can only be executed if
         * a combat is possible (if the player's current node is contested).
         * The command cmd specifies the player's command for the coming combat round
         * (either use item, attack, or flee). Note that using an item should automatically
         * cause the player to attack after using the item.
         * 
         * Also note that a single combat round could take multiple turns.
         * 
         * The method returns true if the combat round terminates the combat, and else
         * false.
         */
        public Boolean doOneCombatRound(Command cmd) {
            if (player.inCombat == true) {
                if (player.boosted == true) {
                    playerstate = PlayerState.CombatStartAndBoosted;
                    return doAction(cmd);
                } else {
                    playerstate = PlayerState.CombatStart;
                    return doAction(cmd);
                }
            } else
                return false;
        }

        public Boolean doAction(Command cmd) {
            String s = cmd.returnArgs();
            switch (cmd.name) {
                case CommandType.ATTACK: 
                    int attack = Int32.Parse(s);
                    // Attack will specify which monster in the list to attack for now.
                    player.Attack(this, player.location.monsters[attack]);
                    return routineAfterAttack();
                case CommandType.FLEE:
                    int flee = Int32.Parse(s);
                    // Flee will specify which node in the neighbouring nodes.
                    if (player.Flee(this, player.location.neighbors[flee]) == true) {
                        playerstate = PlayerState.CombatEnd;
                        player.boosted = false;
                        player.inCombat = false;
                        return true;
                    } else {
                        //What to do if a flee is unsuccesful? for now just attacking like after using an item will do?
                        if (player.boosted == true) {
                            playerstate = PlayerState.CombatComittedAndBoosted;
                            player.boosted = true;
                            player.Attack(this, player.location.monsters[0]);
                            return routineAfterAttack();
                        } else {
                            playerstate = PlayerState.CombatCommitted;
                            player.Attack(this, player.location.monsters[0]);//After using an item it will always try to attack the first monster in the list for now
                            return routineAfterAttack();
                        }
                    }
                case CommandType.USE:
                    if (s == "potion") {
                        healingPotion.Use(this, player);
                        playerstate = PlayerState.CombatCommitted;
                        player.Attack(this, player.location.monsters[0]);//After using an item it will always try to attack the first monster in the list for now
                        return routineAfterAttack();
                    } else if (s == "crystal") {
                        crystal.Use(this, player);
                        playerstate = PlayerState.CombatComittedAndBoosted;
                        player.boosted = true;
                        player.Attack(this, player.location.monsters[0]);
                        return routineAfterAttack();
                    } else
                        throw new Exception("Incorrect item argument");
                default:
                    throw new Exception("Incorrect argument");
            }
        }

        public Boolean routineAfterAttack() {
            if (player.location.monsters.Count == 0) {
                playerstate = PlayerState.CombatEnd;
                player.boosted = false;
                player.inCombat = false;
                return true;
            } else if (player.boosted == true) {
                monsterTurns();
                playerstate = PlayerState.CombatStartAndBoosted;
                return false;
            } else {
                monsterTurns();
                playerstate = PlayerState.CombatStart;
                return false;
            }
        }

        public void monsterTurns() {
            foreach (Monster m in player.location.monsters.ToArray()) {
                int roll = pseudorandom.Next(0, 2);
                if (roll == 1) {
                    m.Attack(this, player);
                } else if (roll == 0) {
                    int fleeloc = pseudorandom.Next(0, player.location.neighbors.Count - 1);
                    m.Flee(this, player.location.neighbors[fleeloc]);
                }
            }
            if (player.location.monsters.Count == 0) {
                playerstate = PlayerState.CombatEnd;
                player.boosted = false;
                player.inCombat = false;
            }
            if (playerstate == PlayerState.Dead) {
                Console.WriteLine("You died :(");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}