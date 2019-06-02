﻿using System;
using System.Collections.Generic;

namespace STVrogue.GameLogic {
    public class Creature : GameEntity {

        public String name = "goblin";
        public int HP = 3;
        public Node location;
        public int attackRating = 1;

        public Creature(String ID) : base(ID) { }

        /*
         * Attack the foe. This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual void Attack(Game G, Creature foe) {
            throw new NotImplementedException();
        }

        /*
         * Move this creature to the given node. Return true if the move is
         * successful, else false.
         * This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual Boolean Move(Game G, Node n) {
            throw new NotImplementedException();
        }

        /*
         * This creature flees to the given node. Return true is this is successful,
         * else false.
         * This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual Boolean Flee(Game G, Node n) {
            throw new NotImplementedException();
        }
    }

    public class Monster : Creature {

        // Monster has a maximum of 5 HP
        public int HPmax = 5;

        public bool rnd;
        public int nr;
        public Monster(String ID) : base(ID) {
            HP = HPmax;
            name = "orc";
            rnd = true;
        }

        public Monster(String ID, int nr) : base(ID) {
            this.nr = nr;
            HP = HPmax;
            name = "orc";
            rnd = false;
        }

        /*
         * Attack the foe. 
         */
        public override void Attack(Game G, Creature foe) {
            foe.HP -= this.attackRating;
            if (foe.HP <= 0)
                G.playerstate = PlayerState.Dead;
        }

        /*
         * Move this monster to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean Move(Game G, Node n) {
            // Get location of concerning Monster.
            Node currentLocation = this.location;
            // Node monster wants to move to is not a neighbouring node.
            List<Node> possibleMoves = n.neighbors;
            if (!possibleMoves.Contains(currentLocation))
                return false;
            // Node monster wants to move to is not in the same zone.
            string currentZoneID = "";
            string targetZoneID = "";
            List<Zone> gameZones = G.dungeon.getZones();
            foreach (Zone zone in gameZones) {
                List<Node> zoneNodes = zone.getNodes();
                if (zoneNodes.Contains(this.location))
                    currentZoneID = zone.ID;
                if (zoneNodes.Contains(n))
                    targetZoneID = zone.ID;
            }
            if (currentZoneID != targetZoneID)
                return false;
            // Node monster wants to move to is already at capacity.
            int targetNodeCapacity = n.getCapacity();
            int amountMonstersTargetNode = n.monsters.Count;
            if (amountMonstersTargetNode == targetNodeCapacity)
                return false;
            // Update monster's location and return true.
            this.location = n;
            return true;
        }

        /*
         * This monster flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean Flee(Game G, Node n) {
            // Get location of concerning Monster.
            Node currentLocation = this.location;
            // Node monster wants to move to is not a neighbouring node.
            List<Node> possibleMoves = n.neighbors;
            if (!possibleMoves.Contains(currentLocation))
                return false;
            // Node monster wants to move to is not in the same zone.
            string currentZoneID = "";
            string targetZoneID = "";
            List<Zone> gameZones = G.dungeon.getZones();
            foreach (Zone zone in gameZones) {
                List<Node> zoneNodes = zone.getNodes();
                if (zoneNodes.Contains(this.location))
                    currentZoneID = zone.ID;
                if (zoneNodes.Contains(n))
                    targetZoneID = zone.ID;
            }
            if (currentZoneID != targetZoneID)
                return false;
            // Node monster wants to move to already contains the player.
            Node playerLocation = G.player.location;
            if (n == playerLocation)
                return false;
            // Node monster wants to move to is already at capacity.
            int targetNodeCapacity = n.getCapacity();
            int amountMonstersTargetNode = n.monsters.Count;
            if (amountMonstersTargetNode == targetNodeCapacity)
                return false;
            // Update monster's location and return true.
            this.location = n;
            return true;
        }

        public int decideAttack(Game g) {
            if (rnd == true) {
                int roll = g.r.Next(0, 1);
                return roll;
            } else {
                return nr;
            }
        }
    }

    public class Player : Creature {
        // Player starts with having made 0 kills
        public int KP = 0;
        // Player has a maximum of 10 HP
        public int HPmax = 10;
        public Boolean boosted = false;
        public Boolean inCombat = false;
        // Player starts with an empty bag
        public List<Item> bag = new List<Item>();

        public Player(String ID) : base(ID) {
            location = Dungeon.startnode;
            HP = HPmax;
            name = "player";
            attackRating = 2;
        }

        /*
         * Attack the foe. 
         */
        public override void Attack(Game G, Creature foe) {
            Monster enemy = foe as Monster;
            Player player = G.player;
            // Decrease foe's HP by player's attackrating, accounting for
            // whether the player is boosted.
            if (player.boosted)
                foe.HP -= (2 * player.attackRating);
            else
                foe.HP -= player.attackRating;
            // Add to kill count if player defeats foe and delete foe
            // from game's and node's monster list.
            if (foe.HP <= 0) {
                player.KP++;
                G.monsters.Remove(enemy);
                player.location.monsters.Remove(enemy);
            }
        }

        /*
         * Move the player to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean Move(Game G, Node n) {
            Player player = G.player;
            // Node player wants to move to is not a neighbouring node.
            List<Node> possibleMoves = player.location.neighbors;
            if (!possibleMoves.Contains(n))
                return false;
            // Update player's location.
            player.location = n;
            // Add any items in new node to player's bag and remove items from
            // the node from the game's list of items and the node's list of
            // items.
            List<Item> nodeItems = n.items;
            if (nodeItems.Count != 0) {
                foreach (Item item in nodeItems)
                    player.bag.Add(item);
                nodeItems.Clear();
            }
            // Set 'inCombat' to true if new node contains a monster.
            List<Creature> nodeMonsters = n.monsters;
            if (nodeMonsters.Count != 0)
                player.inCombat = true;
            return true;
        }

        /*
         * The player flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean Flee(Game G, Node n) {
            Player player = G.player;
            // Node player wants to flee to is not a neighbouring node.
            List<Node> possibleMoves = player.location.neighbors;
            if (!possibleMoves.Contains(n))
                return false;
            // Node player wants to flee to already contains a monster.
            List<Creature> nodeMonsters = n.monsters;
            if (nodeMonsters.Count != 0)
                return false;
            // Update player's location.
            player.location = n;
            // Add any items in new node to player's bag and remove items from
            // the node from the game's list of items and the node's list of
            // items.
            List<Item> nodeItems = n.items;
            if (nodeItems.Count != 0) {
                foreach (Item item in nodeItems)
                    player.bag.Add(item);
                nodeItems.Clear();
            }
            return true;
        }
    }
}