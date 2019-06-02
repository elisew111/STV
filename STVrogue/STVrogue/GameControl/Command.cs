using System;
using System.Text.RegularExpressions;
namespace STVrogue.GameControl {
    [Serializable()]
    public enum CommandType { DoNOTHING, MOVE, ATTACK, USE, FLEE }

    /** Representing a command. */
    [Serializable()]
    public class Command {
        public CommandType name;

        /*
         * Some commands have arguments. For example, "USE" should specify
         * what item to use (e.g. a healing potion). You should decide the format
         * of the arguments.
         */
        String[] args;

        public Command(CommandType name, String[] args) {
            this.name = name;
            this.args = args;
        }

        public override String ToString() {
            String o = "" + name + " ";
            if (args != null && args.Length > 0) {
                for (int i = 0; i < args.Length; i++) {
                    o += args[i];
                }
            }
            return o;
        }

        public String returnArgs() {
            String s = ToString();
            string[] returnString = Regex.Split(s, @"\s+");
            if (returnString.Length > 1 && returnString[1] != "")
                return returnString[1];
            else
                return "none";
        }
    }
}