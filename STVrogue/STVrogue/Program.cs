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
            DrawDungeon();
            GameLoop();

        }

        public static void GameLoop()
        {
            while(true)
            {
                Console.WriteLine("nr of neighbors = " + game.player.location.neighbors.Count);
                Console.WriteLine("type M + nr of target neighbor to move");
                string commandstr = Console.ReadLine();
                if (commandstr.StartsWith("M"))
                {
                    game.player.Move(game, game.player.location.neighbors[(int.Parse(commandstr.Split(" ")[1])) - 1]);
                }
                Update();
            }
        }

        public static void Update()
        {
            Console.Clear();
            DrawDungeon();
        }

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
        }
    }
}
