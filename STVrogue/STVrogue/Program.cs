﻿using System;
using STVrogue.GameLogic;
using STVrogue.GameControl;


namespace STVrogue
{
    class Program
    {

        static Game game;
        static readonly int seed = 100100;
        static Random pseudorandom = new Random(seed);

        static void Main(string[] args)
        {
            
            game = new Game(5, 5);
            DrawDungeon(game.dungeon.getStartnode());
            GameLoop();

        }

        public static void GameLoop()
        {
            while (true)
            {
                
                Console.WriteLine("neighbors:");
                int counter = 1;
                foreach(Node neighbor in game.player.location.neighbors)
                {
                    Console.WriteLine(counter+ ": " + neighbor.ID);
                    counter++;
                }
                Console.WriteLine("Type M + the corresponding number to move");


                string commandstr = Console.ReadLine();
                if (commandstr.StartsWith("M"))
                {
                    int dest = int.Parse(commandstr.Split(" ")[1]);
                    if (dest <= counter)
                    {
                        Command move = new Command(CommandType.MOVE, new string[] { (dest - 1).ToString() });
                        game.doNCTurn(game.player, move);
                        
                    }
                }
                if (commandstr.StartsWith("A"))
                {
                    Command attack = new Command(CommandType.ATTACK, new string[] { "0" });
                    game.doOneCombatRound(attack);
                    //game.player.Attack(game, game.player.location.monsters[0]);
                }
                if(commandstr.StartsWith("H"))
                {
                    HealingPotion hp = new HealingPotion("id", 5);
                    Command heal = new Command(CommandType.USE, new string[] { "potion" });
                    game.doNCTurn(game.player, heal);
                }
                if(commandstr.StartsWith("C"))
                {
                    Crystal cr = new Crystal("id");
                    Command boost = new Command(CommandType.USE, new string[] { "crystal" });
                    game.doNCTurn(game.player, boost);
                }
                Update();
            }
        }

        public static void DrawDungeon(Node node)
        {
            if (game.player.location == game.dungeon.getExitnode())
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine(node.zone.ID);
                HealingPotion hp = new HealingPotion("id", 5);
                Crystal cr = new Crystal("id");
                if (hp.hasHealingPotion(game.player))
                {
                    Console.WriteLine("H");
                }
                if (cr.hasCrystal(game.player))
                {
                    Console.WriteLine("C");
                }
                if (game.player.boosted)
                {
                    Console.WriteLine("boosted");
                }
                Console.WriteLine("Kill#:" + game.player.KP);
                Console.WriteLine("HP: " + game.player.HP + "/" + game.player.HPmax);

                if (node.neighbors.Count > 1)
                {
                    Console.WriteLine("xxxxxxxxxxxxxx  xxxxxxxxxxxxxx");
                }
                else { Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"); }

                Console.WriteLine("x                            x");
                Console.WriteLine("x    P                       x");
                Console.WriteLine("x                            x");
                if (node.neighbors.Count > 2)
                {
                    Console.WriteLine("                              ");
                    Console.WriteLine("                              ");
                }
                else
                {
                    Console.WriteLine("                             x");
                    Console.WriteLine("                             x");
                }

                int monsters = node.monsters.Count;
                for (int i = 1; i <= monsters; i++)
                {
                    Console.WriteLine("x                   M        x");
                }
                for (int i = 1; i < (node.capacity - monsters); i++)
                {
                    Console.WriteLine("x                            x");
                }
                if (node.neighbors.Count > 3)
                {
                    Console.WriteLine("xxxxxxxxxxxxxx  xxxxxxxxxxxxxx");
                }
                else
                {
                    Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                }
                Console.WriteLine("type A to attack");
                Console.WriteLine("type H to use a healingpotion");
                Console.WriteLine("type C to use a crystal");
            }

        }

        

        public static void Update()
        {
            Console.Clear();
            /*foreach (Monster monster in game.player.location.monsters)
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
