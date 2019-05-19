using System;
using System.Collections.Generic;
using STVrogue.GameControl;

namespace STVrogue.GameLogic
{

    public enum PlayerState
    {
        NOTinCombat,
        CombatStart, CombatCommitted, CombatMTR,
        CombatStartAndBoosted, CombatComittedAndBoosted, CombatMTRAndBoosted,
        CombatEnd
    }

    /* This class represents the whole game state of STV-Rogue */
    public class Game
    {
        public Player player;
        /* all monsters currently live in the game. */
        public List<Monster> monsters = new List<Monster>();
        /* all items in the game */
        public List<Item> items = new List<Item>();
        public Crystal crystal;
        public HealingPotion healingPotion;
        /* The dungeon */
        public Dungeon dungeon;

        /* To count the number of passed turns. */
        public int turnNumber = 0;
        /* The creature that currently has the turn. */
        public Creature whoHasTheTurn;



        public PlayerState playerstate;

        public int z_; // no real use except for debug purposes

        public Game(){ }

        /*
         * Create a game with a dungeon of the specified level and capacityMultiplier.
         * This also creates a proper instance of Player.
         */
        public Game(int level, int capacityMultiplier)
        {
            Dungeon dungeon = new Dungeon(6, 3);
            Player player = new Player("player1");
            //throw new NotImplementedException();
        }

        /* return all nodes in the game. */
        public List<Node> nodes() { throw new NotImplementedException(); }   // Iteration-2
        public List<Node> bridges() { throw new NotImplementedException(); } // Iteration-2
        public List<Zone> zones() { return dungeon.getZones() ; }


        /* 
         * Return the monster with the given id, if it still lives in the game. 
         * If the monster does not exist anymore, null is returned.
         */
        public Monster monster(String id) { throw new NotImplementedException(); } // Iteration-2
        public Node node(String id) { throw new NotImplementedException(); }   // Iteration-2
        public Node bridge(String id) { throw new NotImplementedException(); } // Iteration-2
        public Zone zone(String id) { throw new NotImplementedException(); } // Iteration-2

        /* Check if a monster with the given id still lives in the game. */
        public Boolean monsterExists(String id) { return monster(id) != null; }
        public Boolean nodeExists(String id) { return node(id) != null; }
        public Boolean bridgeExists(String id) { return bridge(id) != null; }
        public Boolean zoneExists(String id) { return zone(id) != null; }

        /*
         * Update the game by a single turn, carried out by the creature C (which can
         * be either the player or a monster). The command cmd specifies which action
         * C wants to do.
         * It returns true if the command can be successfully carried out, and else
         * false.
         */
        public Boolean doNCTurn(Creature C, Command cmd)
        {
			String s = cmd.returnArgs();
            switch(cmd) 
            {
                case CommandType.MOVE:
                    if(C is Player || C is Monster)
                    {
                        if(s != "none")
                            return C.Move(this, ); //TODO: wat in te vullen om te bewegen naar een volgende node????
                        else
                            throw Exception();
                    }
                    else return false;
                case CommandType.DoNOTHING:
                    if(C is Player || C is Monster)
                        return true;
                    else return false;
                case CommandType.USE:
                    if(C is Player)
                    {
                        if(s == "potion")
                            healingPotion.Use(this, C);
                        else if (s == "crystal")
                            crystal.Use(this, C);
                        return true;
                    }                   
                    else return false;
                default:
                    throw new Exception("Ongeldig command");
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
        public Boolean doOneCombatRound(Command cmd)
        {   
            if(player.inCombat == true)
            {
                if(player.boosted == true)
                {
                    playerstate = PlayerState.CombatStartAndBoosted;
                    return doAction(cmd);
                }
                else
                {
                    playerstate = PlayerState.CombatStart;
                    return doAction(cmd);
                }
            }
        }

        public Boolean doAction(Command cmd)
        {
            String s = cmd.returnArgs();
            switch(cmd.name)
                {
                    case CommandType.ATTACK:
                        player.Attack(this, ); //How to find where a monster is?
                        return routineAfterAttack();
                    case CommandType.FLEE:
                        if(player.Flee(this, ) == true)//How to determine which node?
                        {
                            playerstate = PlayerState.CombatEnd; player.boosted = false; player.inCombat = false;
                        }                        
                        else
                        {
                            //What to do if a flee is unsuccesful?
                        }
                        break;
                    case CommandType.USE:
                        if(s == "potion")
                        {
                            healingPotion.Use(this, C);
                            playerstate = PlayerState.CombatCommitted;
                            player.Attack(this, );//how to determine which monster?
                            return routineAfterAttack();
                        }
                        else if (s == "crystal")
                        {
                            crystal.Use(this, C);
                            playerstate = PlayerState.CombatComittedAndBoosted;
                            boosted = true;
                            player.Attack(this, );//how to determine which monster?
                            return routineAfterAttack();
                        }
                        else
                            throw Exception();
                        break;
                }
        }
        
        public Boolean routineAfterAttack()
        {
            if(/*amount of monsters in current node is zero*/)
            {
                playerstate = PlayerState.CombatEnd; player.boosted = false; player.inCombat = false;
                return true;
            }

            //player dead??? nothing specifies what happens if the player is dead.

            if(player.boosted == true)
            {
                //TODO: monster turns
                playerstate = PlayerState.CombatStartAndBoosted;
                return false;
            }
            else
            {
                //TODO: monster turns
                playerstate = PlayerState.CombatStart;
                return false;
            }
        }
    }
}
