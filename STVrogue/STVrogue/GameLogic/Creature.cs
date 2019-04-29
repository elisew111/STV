using System;
using System.Collections.Generic;

namespace STVrogue.GameLogic
{
    public class Creature : GameEntity
    {

        public String name = "goblin";
        public int HP = 1;     // current HP, should never exceed HPmax
        public Node location;
        public int AttackRating = 1;

        public Creature(String ID) : base(ID){ }

        /*
         * Attack the foe. This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual void attack(Game G, Creature foe)
        {
            throw new NotImplementedException();
        }

        /*
         * Move this creature to the given node. Return true if the move is
         * successful, else false.
         * This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual Boolean move(Game G, Node nd)
        {
            throw new NotImplementedException();
        }

        /*
         * This creature flees to the given node. Return true is this is successful,
         * else false.
         * This one just throws an exception; you need to
         * override it in the corresponding subclasses (Monster and Player).
         */
        public virtual Boolean flee(Game G, Node nd)
        {
            throw new NotImplementedException();
        }
    }

    public class Monster : Creature
    {
        public Monster(String ID) : base(ID)
        {
            // you need to decide how to initialize the other attributes
            throw new NotImplementedException();
        }
       
        /*
         * Attack the foe. 
         */
        public override void attack(Game G, Creature foe)
            {
                throw new NotImplementedException();
            }

    
        /*
         * Move this monster to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean move(Game G, Node nd)
            {
                throw new NotImplementedException();
            }

        /*
         * This monster flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean flee(Game G, Node nd)
            {
                throw new NotImplementedException();
            }
    }

    public class Player : Creature
    {
        /** kill point */
        int KP = 0;
        int HPmax;
        public Boolean boosted = false;
        public Boolean inCombat = false;
        public List<Item> bag = new List<Item>();

        public Player(String ID) : base(ID)
        {
            // you need to decide how to initialize the other attributes
            throw new NotImplementedException();
        }

        /*
         * Attack the foe. 
         */
        public override void attack(Game G, Creature foe)
        {
            throw new NotImplementedException();
        }

        /*
         * Move the player to the given node. Return true if the move is
         * successful, else false.
         */
        public override Boolean move(Game G, Node nd)
        {
            throw new NotImplementedException();
        }

        /*
         * The player flees to the given node. Return true is this is successful,
         * else false.
         */
        public override Boolean flee(Game G, Node nd)
        {
            throw new NotImplementedException();
        }


    }




}
