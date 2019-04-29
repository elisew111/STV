using System;
namespace STVrogue
{
    public class Utils
    {
        /* You can change the behavior of this logger. */
        public static void log(String s)
        {
            Console.Out.WriteLine("** " + s);
        }

        /* Return a pseudo-random generator, using the given seed. */
        static public Random initializeWithSeed(int seed)
        {
            return new Random(seed);
        }

    }
}
