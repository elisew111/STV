using System;
using STVrogue.GameLogic;
using STVrogue.GameControl;


namespace STVrogue
{
    class Program
    {

        static Game game;

        static void Main(string[] args)
        {

            game = new Game(5, 3);
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
                Console.WriteLine("Choose a destination by typing M + the corresponding number");
                string commandstr = Console.ReadLine();
                if (commandstr.StartsWith("M"))
                {
                    int dest = int.Parse(commandstr.Split(" ")[1]);
                    if (dest <= counter)
                    {
                        game.player.Move(game, game.player.location.neighbors[dest-1]);
                    }
                }
                Update();
            }
        }

        public static void DrawDungeon(Node node)
        {

            Console.WriteLine(node.zone.ID);
            HealingPotion hp = new HealingPotion("id", 5);
            Crystal cr = new Crystal("id");
            if(hp.hasHealingPotion(game.player))
            {
                Console.WriteLine("H");
            }
            if(cr.hasCrystal(game.player))
            {
                Console.WriteLine("C");
            }

            Console.WriteLine("xxxxxxxxxxxxxx  xxxxxxxxxxxxxx");
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
            for(int i = 1; i<=monsters; i++)
            {
                Console.WriteLine("x                   M        x");
            }
            for (int i = 1; i < (node.capacity - monsters); i++) 
            {
                Console.WriteLine("x                            x");
            }
            if(node.neighbors.Count > 3)
            {
                Console.WriteLine("xxxxxxxxxxxxxx  xxxxxxxxxxxxxx");
            }
            else
            {
                Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }

        }

        

        public static void Update()
        {
            Console.Clear();
            DrawDungeon(game.player.location);
        }

        /*
        static void DrawDungeon()
        {
            int zonenr = 1;
            foreach (Zone zone in game.dungeon.getZones())
            {
                Console.WriteLine("Zone #" + zonenr + ", " + zone.getType());
                zonenr++;
                FillZone(zone);
            }
        }

        static void FillZone(Zone zone)
        {
            foreach (Node node in zone.getNodes())
            {
                Console.WriteLine("Node: " + node.ID + " " + node.getType());
                if (game.player.location == node)
                {
                    Console.WriteLine("PLAYER");
                }
                Console.WriteLine("-- monsters: " + node.monsters.Count);
                Console.WriteLine("-- items: " + node.items.Count);
                Console.WriteLine("-- buren:");
                foreach (Node buur in node.neighbors)
                {
                    Console.WriteLine("--- " + buur.ID);
                }
            }
        } */
    }
}
