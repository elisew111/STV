using System;
using STVrogue.GameLogic;
using STVrogue.GameControl;
using STVrogue.TestInfrastructure;
using System.Collections.Generic;

namespace STVrogue {
    class Program {
        static Game game;
        static List<string> commands = new List<string>();

        static void Main(string[] args) {
            Console.WriteLine("Do you want to replay a game or play a new game?");
            Console.WriteLine("Type 'replay' to replay a game or 'new' to start a new game");
            string mode = Console.ReadLine();
            if (mode == "new") {
                Console.Clear();
                game = new Game(3, 1);
                commands.Add("3, 1");
                DrawDungeon(game.dungeon.getStartnode());
                GameLoop();
            } else if (mode == "replay") {
                Console.Clear();
                GamePlay gameplay = new GamePlay("test.txt");
                gameplay.DrawDungeon(gameplay.game.dungeon.getStartnode());
                while (true) {
                    gameplay.replayCurrentTurn();
                    if (gameplay.turn == gameplay.commands.Count) {
                        Console.WriteLine("Next turn is final turn");
                        Console.WriteLine("Press enter to execute next move: " + gameplay.commands[gameplay.turn - 1]);
                    }
                    else
                        Console.WriteLine("Press enter to execute next move: " + gameplay.commands[gameplay.turn - 1]);
                    Console.ReadLine();
                    gameplay.Update();
                }
            }
        }

        public static void GameLoop() {
            while (true) {
                Console.WriteLine("Neighbors:");
                int counter = 1;
                foreach (Node neighbor in game.player.location.neighbors) {
                    Console.WriteLine(counter + ": " + neighbor.ID);
                    counter++;
                }
                Console.WriteLine("Type M + the corresponding number to move");

                string commandstr = Console.ReadLine();
                commands.Add(commandstr);
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
                    //game.player.Attack(game, game.player.location.monsters[0]);
                }
                if (commandstr.StartsWith("H")) {
                    Command heal = new Command(CommandType.USE, new string[] { "potion" });
                    game.doNCTurn(game.player, heal);
                }
                if (commandstr.StartsWith("C")) {
                    Command boost = new Command(CommandType.USE, new string[] { "crystal" });
                    game.doNCTurn(game.player, boost);
                }
                Update();
            }
        }

        public static void DrawDungeon(Node node) {
            if (game.player.location == game.dungeon.getExitnode()) {
                Console.WriteLine("You win!");
                Console.ReadLine();
                GamePlay.save(commands);
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

        public static void Update() {
            Console.Clear();
            /* Random pseudorandom = new Random(seed);
            foreach (Monster monster in game.player.location.monsters)
            {
                int action = pseudorandom.Next(1, 3);
                switch(action)
                {
                    case 1:
                        int dest = pseudorandom.Next(0, monster.location.neighbors.Count - 1);
                        Command move = new Command(CommandType.MOVE, new string[] { dest.ToString() });
                        game.doNCTurn(monster, move);
                        return;
                    default:
                        Command nothing = new Command(CommandType.DoNOTHING, new string[] { });
                        return;
                }
            }*/
            DrawDungeon(game.player.location);
        }
    }
}
