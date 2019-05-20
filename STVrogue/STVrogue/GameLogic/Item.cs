using System;

namespace STVrogue.GameLogic {

    public class Item : GameEntity {

        public Item(String ID) : base(ID) { }

        /*
         * Implementing the logic of what happen when the player uses this item.
         * Here it does nothing. Override this accordingly in the subclasses.
         */
        public virtual void Use(Game G, Player player) { }
    }

    public class HealingPotion : Item {

        /* HealingPotion can heal 5 HP*/
        int HPvalue = 5;

        public HealingPotion(String ID, int heal) : base(ID) {
            this.HPvalue = heal;
        }

        /*
         * Using a healing potion heals the player (but not beyond his HPmax).
         */
        public override void Use(Game G, Player player) {
            if(hasHealingPotion(player)) {
                player.HP = Math.Min(player.HPmax, player.HP + this.HPvalue);
                deleteHealingPotion(player);
            } 
        }

        public bool hasHealingPotion(Player player) {
            foreach (Item item in player.bag) {
                if (item is HealingPotion)
                    return true;
            }
            return false;
        }

        public void deleteHealingPotion(Player player) {
            String itemToDeleteID = "";
            int bagSize = player.bag.Count;
            foreach (Item item in player.bag) {
                if (item is HealingPotion) {
                    itemToDeleteID = item.ID;
                    break;
                }
            }
            for (int i = 0; i <= bagSize; i++) {
                if (player.bag[i].ID == itemToDeleteID) {
                    player.bag.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class Crystal : Item {

        public Crystal(String ID) : base(ID) { }

        /*
         * Using a crystal during a combat temporarily doubles the player's 
         * attack rating. The effect is gone once the combat ends.
         * Using a crystal while not in combat has no effect.
         */
        public override void Use(Game G, Player player) {
            if(hasCrystal(player)) {
                deleteCrystal(player);
                if(player.inCombat)
                    player.boosted = true;
            }   
        }

        public bool hasCrystal(Player player) {
            foreach (Item item in player.bag)
            {
                if (item is Crystal)
                    return true;
            }
            return false;
        }

        public void deleteCrystal(Player player) {
            String itemToDeleteID = "";
            int bagSize = player.bag.Count;
            foreach (Item item in player.bag) {
                if (item is Crystal) {
                    itemToDeleteID = item.ID;
                    break;
                }
            }
            for (int i = 0; i <= bagSize; i++) {
                if (player.bag[i].ID == itemToDeleteID) {
                    player.bag.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
