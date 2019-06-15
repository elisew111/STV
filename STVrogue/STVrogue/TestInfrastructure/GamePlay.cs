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
            /*
             * Waar ik aan zat te denken (maar dat is niet echt te implementeren als we nog geen werkend spel
             * hebben) is dat je tijdens het spelen van een spel al je commands en de seed+parameters van een 
             * bepaalde Game naar een file schrijft (daar is denk ik de gameplay.save(file) functie die ze zeggen
             * dat je moet maken, maar nergens te bekennen is, ook voor bedoeld). Het enige wat je in deze
             * functie dan hoeft te doen is de initiële GameState afhankelijk van de gebruikte seed en
             * parameters bepalen en de commands in een array op te slaan en je kan aan de slag.
             */
            string[] lines = File.ReadAllLines(savefile, Encoding.UTF8);
            string createGameParameters = lines[0];
            int level = (int)Char.GetNumericValue(createGameParameters[0]);
            int capacityMultiplier = (int)Char.GetNumericValue(createGameParameters[3]);
            game = new Game(level, capacityMultiplier);
            copyGame = game;
            for (int i = 1; i < lines.Length; i++) {
                commands.Add(lines[i]);
            }
            length = commands.Count;
        }

        public static void save(List<string> commands) {
            string currentDirectory = Directory.GetCurrentDirectory();
            string currentTime = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
            string writeLocation = currentDirectory  + @"\" + currentTime + "gameplay.txt";
            File.WriteAllLines(writeLocation, commands);
        }

        /* reset the gameplay to turn 0 */
        public virtual void reset() {
            /*
             * Bij het maken van een GamePlay object wordt de initiële GameState aangemaakt en daar moet een
             * kopie van worden gemaakt. Deze functie aanroepen zal dan dit kopie van de initïele GameState
             * kopiëren naar de huidige GameState en de turn weer op 0 zetten (wat pretty much betekent dat
             * de selector van de array van commands weer 0 wordt).
             */
            turn = 0;
            game = copyGame;
        }

        /* return the current game state */
        public virtual Game getState() {
            /*
             * Deze functie moet een Game-object returnen, maar wat dit precies inhoudt weet ik ook niet echt.
             * Game heeft een aantal class variables, namelijk Player player, List<Monster> monsters, 
             * List<Item> items, Dungeon dungeon, int turnNumber en Creature whoHasTheTurn. De meeste dingen
             * zullen redelijk voorzich spreken, maar dingen zoals Dungeon hebben zelf ook weer heel veel
             * class variables, dus of je die ook moet returnen en in welke capaciteit is mij een raadsel.
             */
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
            if (turn == commands.Count) {
                Console.WriteLine("Game finished");
                Console.ReadLine();
                Environment.Exit(0);
            } else {
                string action = commands[turn];
                DoMove(action);
                turn++;
            }
        }

        public void DoMove(string commandstr) {
            Console.WriteLine("Neighbors:");
            int counter = 1;
            foreach (Node neighbor in game.player.location.neighbors) {
                Console.WriteLine(counter + ": " + neighbor.ID);
                counter++;
            }
            if (commandstr.StartsWith("M")) {
                int dest = int.Parse(commandstr.Split(" ")[1]);
                if (dest <= counter) {
                    Command move = new Command(CommandType.MOVE, new string[] { (dest - 1).ToString() });
                    game.doNCTurn(game.player, move);

                }
            }
            if (commandstr.StartsWith("A")) {
                Command attack = new Command(CommandType.ATTACK, new string[] { "0" });
                game.doOneCombatRound(attack);
            }
            if (commandstr.StartsWith("H")) {
                Command heal = new Command(CommandType.USE, new string[] { "potion" });
                game.doNCTurn(game.player, heal);
            }
            if (commandstr.StartsWith("C")) {
                Command boost = new Command(CommandType.USE, new string[] { "crystal" });
                game.doNCTurn(game.player, boost);
            }
        }

        public void Update() {
            Console.Clear();
            DrawDungeon(game.player.location);
        }

        public void DrawDungeon(Node node) {
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
                Console.WriteLine("type A to attack");
                Console.WriteLine("type H to use a healingpotion");
                Console.WriteLine("type C to use a crystal");
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
