using System;
namespace STVrogue.GameLogic {
    /* A parent class representing all game entities in STV Rogue. */
    public class GameEntity {
        /* Every entity is identified by a unique ID */
        public String ID;

        public GameEntity(String uniqueID) {
            this.ID = uniqueID;
        }
    }
}
