using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using STVrogue.GameLogic;
using STVrogue.GameControl;

namespace STVrogue.TestInfrastructure {
    /* For ITERATION 2.
     * Representing a replayable game play. 
     */
    public class GamePlay {
        public int turn = 0;
        public int length;
        public List<string> commands = new List<string>();
        public Game game;
        public Game copyGame;

        public GamePlay() { }

        public GamePlay(String savefile) {
            string[] lines = File.ReadAllLines(savefile, Encoding.UTF8);
            string createGameParameters = lines[0];
            int level = (int)Char.GetNumericValue(createGameParameters[0]);
            int capacityMultiplier = (int)Char.GetNumericValue(createGameParameters[3]);
            game = new Game(level, capacityMultiplier);
            copyGame = game;
            for (int i = 1; i < lines.Length; i++)
                commands.Add(lines[i]);
            length = commands.Count;
            //drawDungeon(game.dungeon.getStartnode());
            //while (true) {
            //    replayCurrentTurn();
            //    if (turn == commands.Count) {
            //        Console.WriteLine("Next turn is final turn");
            //        Console.WriteLine("Press enter to execute next move: " + commands[turn - 1]);
            //    } else
            //        Console.WriteLine("Press enter to execute next move: " + commands[turn - 1]);
            //    //Console.ReadLine();
            //    //Update();
            //}
        }

        public static void save(List<string> commands) {
            string currentDirectory = Directory.GetCurrentDirectory();
            string currentTime = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
            string writeLocation = currentDirectory  + @"\" + currentTime + "gameplay.txt";
            File.WriteAllLines(writeLocation, commands);
        }

        /* reset the gameplay to turn 0 */
        public virtual void reset() {
            turn = 0;
            game = copyGame;
        }

        /* return the current game state */
        public virtual Game getState() {
            return game;
        }

        /* return the current turn number. */
        public int getTurn() {
            return turn;
        }

        /* true if the gameplay is at the end, hence has no more turn to do. */
        public Boolean atTheEnd() {
            return turn >= length;
        }

        /*
         * Replay the current turn, thus updating the game state.
         * This also increases the turn nr, thus shifting the current turn to the next one. 
         */
        public virtual void replayCurrentTurn() {
            //if (turn == commands.Count) {
            //    Console.WriteLine("Game finished");
            //    //Console.ReadLine();
            //    Environment.Exit(0);
            //} else {
                string action = commands[turn];
                doMove(action);
                turn++;
            //}
        }

        public void doMove(string action) {
            if (game.player.location.monsters.Count == 0) { game.player.inCombat = false; } else { game.player.inCombat = true; }
            int counter = 1;
            foreach (Node neighbor in game.player.location.neighbors)
                counter++;
            if (action.StartsWith("M") && !game.player.inCombat) {
                int dest = int.Parse(action.Split(" ")[1]);
                if (dest <= counter) {
                    Command move = new Command(CommandType.MOVE, new string[] { (dest - 1).ToString() });
                    game.doNCTurn(game.player, move);
                }
            }
            if (action.StartsWith("F") && game.player.inCombat) {
                int dest = int.Parse(action.Split(" ")[1]);
                if (dest <= counter) {
                    Command flee = new Command(CommandType.FLEE, new string[] { (dest - 1).ToString() });
                    game.doOneCombatRound(flee);
                }
            }
            if (action.StartsWith("A") && game.player.inCombat) {
                Command attack = new Command(CommandType.ATTACK, new string[] { "0" });
                game.doOneCombatRound(attack);
            }
            if (action.StartsWith("H")) {
                Command heal = new Command(CommandType.USE, new string[] { "potion" });
                game.doNCTurn(game.player, heal);
            }
            if (action.StartsWith("C")) {
                Command boost = new Command(CommandType.USE, new string[] { "crystal" });
                game.doNCTurn(game.player, boost);
            }
            if (action.StartsWith("N") && !game.player.inCombat) {
                if (game.player.location.monsters.Count == 0) {
                    Command doNothing = new Command(CommandType.DoNOTHING, new string[] { "do nothing" });
                    game.doNCTurn(game.player, doNothing);
                } else
                    game.routineAfterAttack();
            }
        }

        public void Update() {
            Console.Clear();
            drawDungeon(game.player.location);
        }

        public void drawDungeon(Node node) {
            if (game.player.location == game.dungeon.getExitnode()) {
                Console.WriteLine("You win!");
                Console.ReadLine();
                Environment.Exit(0);
            } else {
                Console.WriteLine("Current location: " + node.ID);
                Console.Write("Players bag content: ");
                foreach (Item i in game.player.bag) {
                    if (i is Crystal)
                        Console.Write("C ");
                    if (i is HealingPotion)
                        Console.Write("HP ");
                }
                Console.WriteLine();
                if (game.player.boosted)
                    Console.WriteLine("Player is boosted!");
                Console.WriteLine("Kills: " + game.player.KP);
                Console.WriteLine("HP: " + game.player.HP + "/" + game.player.HPmax);

                if (node.neighbors.Count > 1)
                    Console.WriteLine("xxxxxxxxxxxxxx  xxxxxxxxxxxxxx");
                else
                    Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

                Console.WriteLine("x                            x");
                Console.WriteLine("x    P                       x");
                Console.WriteLine("x                            x");
                if (node.neighbors.Count > 2) {
                    Console.WriteLine("                              ");
                    Console.WriteLine("                              ");
                } else {
                    Console.WriteLine("                             x");
                    Console.WriteLine("                             x");
                }

                int monsters = node.monsters.Count;
                for (int i = 1; i <= monsters; i++)
                    Console.WriteLine("x                   M        x");
                for (int i = 1; i < (node.capacity - monsters); i++)
                    Console.WriteLine("x                            x");
                if (node.neighbors.Count > 3)
                    Console.WriteLine("xxxxxxxxxxxxxx  xxxxxxxxxxxxxx");
                else
                    Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }
        }
    }

    /* A dummy GamePlay; for testing the specification classes */
    public class DummyGamePlay : GamePlay {

        int[] execution;
        Game state;

        public DummyGamePlay(int[] execution) {
            this.execution = execution;
            length = execution.Length - 1;
            state = new Game();
        }

        public override void reset() {
            turn = 0;
            state.z_ = execution[turn];
        }

        public override Game getState() {
            return state;
        }

        public override void replayCurrentTurn() {
            turn++;
            state.z_ = execution[turn];
        }
    }
}
