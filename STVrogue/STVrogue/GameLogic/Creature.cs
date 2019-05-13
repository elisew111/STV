using System;
using System.Collections.Generic;
using System.Linq;

namespace STVrogue.GameLogic {
    public class Creature : GameEntity {

        public String name = "goblin";
        public int HP = 1;     // current HP, should never exceed HPmax
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
        public virtual Boolean Move(Game G, Node nd) {
            throw new NotImplementedException();
        }

        /*
         * This creature flees to the given node. Return true is this is successful,
         * else false.
         * This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual Boolean Flee(Game G, Node nd) {
            throw new NotImplementedException();
        }
    }

    public class Monster : Creature {

        /* Monster has a maximum of 5 HP */
        int HPmax = 5;
        public Monster(String ID) : base(ID) {
            // Add position initialization (accounting for capacity of node).
            HP = HPmax;
            attackRating = 1;
            name = "orc";
        }

        /*
         * Attack the foe. 
         */
        public override void Attack(Game G, Creature foe) {
            foe.HP -= attackRating;
            // Check if player's HP is below 0 and kill the player if this is the case.
        }


        /*
         * Move this monster to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean Move(Game G, Node nd) {
            // Node monster wants to move to is not a neighbouring node.
            List<Node> possibleMoves = nd.neighbors;
            if (!possibleMoves.Contains(this.location))
                return false;
            // Node monster wants to move to is not in the same zone.
            Zone currentZone = this.location.zone;
            if (currentZone != nd.zone)
                return false;
            // Update monster's location and return true.
            this.location = nd;
            return true;
        }

        /*
         * This monster flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean Flee(Game G, Node nd) {
            // Node monster wants to move to is not a neighbouring node.
            List<Node> possibleMoves = nd.neighbors;
            if (!possibleMoves.Contains(this.location))
                return false;
            // Node monster wants to move to is not in the same zone.
            Zone currentZone = this.location.zone;
            if (currentZone != nd.zone)
                return false;
            // Node monster wants to move to already contains the player.
            Node playerLocation = G.player.location;
            if (nd != playerLocation)
                return false;
            // Update monster's location and return true.
            this.location = nd;
            return true;
        }
    }

    public class Player : Creature {

        // Player starts with having made 0 kills
        int KP = 0;
        // Player has a maximum of 10 HP
        public int HPmax = 10;
        public Boolean boosted = false;
        public Boolean inCombat = false;
        // Player starts with an empty bag
        public List<Item> bag = new List<Item>();

        public Player(String ID) : base(ID) {
            // Add position initialization (start node)
            HP = HPmax;
            name = "player";
            attackRating = 2;
        }

        /*
         * Attack the foe. 
         */
        public override void Attack(Game G, Creature foe) {
            // Decrease foe's HP by player's attackrating
            foe.HP -= G.player.attackRating;
            // Add to kill count if player defeats foe.
            if (foe.HP <= 0)
                G.player.KP++;
        }

        /*
         * Move the player to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean Move(Game G, Node nd) {
            // Node player wants to move to is not a neighbouring node.
            List<Node> possibleMoves = nd.neighbors;
            if (!possibleMoves.Contains(G.player.location))
                return false;
            // Update player's location.
            G.player.location = nd;
            // Add any items in new node to player's bag.
            List<Item> nodeItems = nd.items;
            if (!nodeItems.Any()) {
                foreach(Item item in nodeItems)
                    G.player.bag.Add(item);
            }
            // Set 'inCombat' to true if new node contains a monster.
            List<Creature> nodeCreatures = nd.monsters;
            if (!nodeCreatures.Any())
                G.player.inCombat = true;
            return true;
        }

        /*
         * The player flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean Flee(Game G, Node nd) {
            // Node player wants to move to is not a neighbouring node.
            List<Node> possibleMoves = nd.neighbors;
            if (!possibleMoves.Contains(G.player.location))
                return false;
            // Node player wants to flee to already contains a monster.
            List<Creature> nodeCreatures = nd.monsters;
            if (!nodeCreatures.Any())
                return false;
            // Update player's location.
            G.player.location = nd;
            // Add any items in new node to player's bag.
            List<Item> nodeItems = nd.items;
            if (!nodeItems.Any()) {
                foreach (Item item in nodeItems)
                    G.player.bag.Add(item);
            }
            return true;
        }
    }
}
