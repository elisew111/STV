using System;
namespace STVrogue.GameLogic
{
    public class Item : GameEntity
    {
        public Item(String ID) : base(ID){ }

        /*
         * Implementing the logic of what happen when the player uses this item.
         * Here it does nothing. Override this accordingly in the subclasses.
         */
        public virtual void use(Game G, Player player) { }
    }

    public class HealingPotion : Item
    {
        /* it can heal this many HP */
        int HPvalue;

        public HealingPotion(String ID, int heal) : base(ID)
        {
            this.HPvalue = heal;
        }

        /*
         * Using a healing potion heals the player (but not beyond his HPmax).
         */
        public override void use(Game G, Player player)
        {
            throw new NotImplementedException();
        }
    }

    public class Crystal : Item
    {

        public Crystal(String ID) : base(ID){ }

        /*
         * Using a crystal during a combat temporarily doubles the player's 
         * attack rating. The effect is gone once the combat ends.
         * Using a crystal while not in combat has no effect.
         */
        public override void use(Game G, Player player)
        {
            throw new NotImplementedException();
        }
    }

}
