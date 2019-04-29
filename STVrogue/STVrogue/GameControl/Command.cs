using System;
namespace STVrogue.GameControl
{
    [Serializable()]
    public enum CommandType { DoNOTHING, MOVE, ATTACK, USE, FLEE }

    /** Representing a command. */
    [Serializable()]
    public class Command
    {
        CommandType name;

        /*
         * Some commands have arguments. For example, "USE" should specify
         * what item to use (e.g. a healing potion). You should decide the format
         * of the arguments.
         */
        String[] args;

        public Command(CommandType name, String[] args)
        {
            this.name = name;
            this.args = args;
        }

        public override String ToString()
        {
            String o = "" + name;
            if (args != null && args.Length > 0)
            {
                o += "(";
                for (int i = 0; i < args.Length; i++)
                {
                    if (i > 0) o += ",";
                    o += args[i];
                }
                o += ")";
            }
            return o;
        }
    }
}
