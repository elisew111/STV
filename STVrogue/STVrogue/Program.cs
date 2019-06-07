using System;
using STVrogue.GameLogic;


namespace STVrogue
{
    class Program
    {

        static Game game;

        static void Main(string[] args)
        {

            game = new Game(5, 3);
            Update();

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
