using System;
using System.Collections.Generic;
using System.Linq;

namespace STVrogue.GameLogic {
    public class Creature : GameEntity {

        public String name = "goblin";
        public int HP = 1;
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
        public Monster(String ID) : base(ID) {
            /*
             * Add position initialization (accounting for capacity of node).
             */
            HP = HPmax;
            name = "orc";
        }

        /*
         * Attack the foe. 
         */
        public override void Attack(Game G, Creature foe) {
            foe.HP -= this.attackRating;
            /*
             * Check if player's HP is below 0 and kill the player if this is the case.
             */
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
            Zone currentZone = currentLocation.zone;
            if (currentZone != n.zone)
                return false;
            // Update monster's location and return true.
            currentLocation = n;
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
            Zone currentZone = currentLocation.zone;
            if (currentZone != n.zone)
                return false;
            // Node monster wants to move to already contains the player.
            Node playerLocation = G.player.location;
            if (n != playerLocation)
                return false;
            // Update monster's location and return true.
            currentLocation = n;
            return true;
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
            HP = HPmax;
            name = "player";
            attackRating = 2;
        }

        /*
         * Attack the foe. 
         */
        public override void Attack(Game G, Creature foe) {
            Monster enemy = foe as Monster;
            Player P = G.player;
            // Decrease foe's HP by player's attackrating, accounting for
            // whether the player is boosted.
            if (P.boosted)
                foe.HP -= 2 * P.attackRating;
            else 
                foe.HP -= P.attackRating;        
            // Add to kill count if player defeats foe and delete foe
            // from Game's monster list.
            if (foe.HP <= 0) { 
                P.KP++;
                G.monsters.Remove(enemy);
            }
        }

        /*
         * Move the player to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean Move(Game G, Node n) {
            Player P = G.player;
            // Node player wants to move to is not a neighbouring node.
            List<Node> possibleMoves = n.neighbors;
            if (!possibleMoves.Contains(P.location))
                return false;
            // Update player's location.
            P.location = n;
            // Add any items in new node to player's bag and clear nodeItems.
            List<Item> nodeItems = n.items;
            if (!nodeItems.Any()) {
                foreach(Item item in nodeItems)
                    P.bag.Add(item);
                nodeItems.Clear();
            } 
            // Set 'inCombat' to true if new node contains a monster.
            List<Creature> nodeMonsters = n.monsters;
            if (!nodeMonsters.Any())
                P.inCombat = true;
            return true;
        }

        /*
         * The player flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean Flee(Game G, Node n) {
            Player P = G.player;
            // Node player wants to move to is not a neighbouring node.
            List<Node> possibleMoves = n.neighbors;
            if (!possibleMoves.Contains(P.location))
                return false;
            // Node player wants to flee to already contains a monster.
            List<Creature> nodeMonsters = n.monsters;
            if (!nodeMonsters.Any())
                return false;
            // Update player's location.
            P.location = n;
            // Add any items in new node to player's bag and clear nodeItems.
            List<Item> nodeItems = n.items;
            if (!nodeItems.Any()) {
                foreach (Item item in nodeItems)
                    P.bag.Add(item);
                nodeItems.Clear();
            }
            return true;
        }
    }
}
