﻿using System;
using STVrogue.GameLogic;
using STVrogue.GameControl;
using STVrogue.TestInfrastructure;
using System.Collections.Generic;

namespace STVrogue {
    class Program {
        static Game game;
        public static List<string> commands = new List<string>();

        static void Main() {
            Console.WriteLine("Do you want to replay a game or play a new game?");
            Console.WriteLine("Type 'replay' to replay a game or 'new' to start a new game");
            string mode = Console.ReadLine();
            if (mode == "new") {
                Console.Clear();
                game = new Game(3, 1);
                int level = game.dungeon.getZones().Count;
                int capacityMultiplier = game.dungeon.getCapacityMultiplier();
                string gameParams = level + ", " + capacityMultiplier;
                commands.Add(gameParams);
                DrawDungeon(game.dungeon.getStartnode());
                GameLoop();
            } else if (mode == "replay") {
                Console.WriteLine("Type the name of the file containing the game you want to replay");
                string file = Console.ReadLine();
                Console.Clear();
                GamePlay gameplay = new GamePlay(file);
                ReplayLoop(gameplay);
            }
        }

        public static void ReplayLoop(GamePlay g)
        {
            g.drawDungeon(g.game.dungeon.getStartnode());
            while (!g.atTheEnd())
            {
                g.replayCurrentTurn();
                if (g.turn == g.commands.Count)
                {
                    Console.WriteLine("Next turn is final turn");
                    Console.WriteLine("Press enter to execute next move: " + g.commands[g.turn - 1]);
                }
                else
                    Console.WriteLine("Press enter to execute next move: " + g.commands[g.turn - 1]);
                int items = g.game.dungeon.items.Count + g.game.player.bag.Count;
                Console.WriteLine("nr of items = " + items);
                Console.ReadLine();
                g.Update();
            }
        }

        public static void GameLoop() {
            while (true) {
                if (game.player.location.monsters.Count == 0) {
                    game.player.inCombat = false;
                } else {
                    game.player.inCombat = true;
                }

                if (!game.player.inCombat) {
                    Console.WriteLine("Type M + the corresponding number to move to specified node");
                    Console.WriteLine("Type N to do nothing");
                }
                else
                    Console.WriteLine("Type F + the corresponding number to flee to specified node");
                Console.WriteLine("Neighbors:");
                int counter = 0;
                foreach (Node neighbor in game.player.location.neighbors) {
                    counter++;
                    Console.WriteLine(counter + ": " + neighbor.ID);
                    
                }

                string commandstr = Console.ReadLine();
                commands.Add(commandstr);
                if (commandstr.StartsWith("M") && !game.player.inCombat) {
                    int dest = int.Parse(commandstr.Split(" ")[1]);
                    if (dest <= counter) {
                        Command move = new Command(CommandType.MOVE, new string[] { (dest - 1).ToString() });
                        game.doNCTurn(game.player, move);
                    }
                }
                if (commandstr.StartsWith("F") && game.player.inCombat) {
                    int dest = int.Parse(commandstr.Split(" ")[1]);
                    if (dest <= counter) {
                        Command flee = new Command(CommandType.FLEE, new string[] { (dest - 1).ToString() });
                        game.doOneCombatRound(flee);
                    }
                }
                if (commandstr.StartsWith("A") && game.player.inCombat) {
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
                if (commandstr.StartsWith("N") && !game.player.inCombat) {
                    if (game.player.location.monsters.Count == 0) {
                        Command doNothing = new Command(CommandType.DoNOTHING, new string[] { "do nothing" });
                        game.doNCTurn(game.player, doNothing);
                    }
                    else                    
                        game.routineAfterAttack();
                }
                Update();
            }
        }

        public static void DrawDungeon(Node node) {
            bool hasCrystal = false;
            bool hasHealingPotion = false;
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
                if (game.player.inCombat)
                    Console.WriteLine("Type A to attack");
                foreach (Item i in game.player.bag) {
                    if (i is Crystal)
                        hasCrystal = true;                      
                    if (i is HealingPotion)
                        hasHealingPotion = true;                       
                }
                if (hasCrystal)
                    Console.WriteLine("Type C to use a crystal");
                if (hasHealingPotion)
                    Console.WriteLine("Type H to use a healingpotion");
            }
        }

        public static void Update() {
            Console.Clear();
            DrawDungeon(game.player.location);
        }
    }
}
