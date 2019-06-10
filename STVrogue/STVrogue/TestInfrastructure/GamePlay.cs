using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure {
    /* For ITERATION 2.
     * Representing a replayable game play. 
     */
    public class GamePlay {
        protected int turn = 0;
        protected int length;
        protected List<string> commands = new List<string>();
        protected Game game;
        protected Game copyGame;

        public GamePlay() { }

        public GamePlay(String savefile) {
            /*
             * Het nut van deze functie is dat we een GamePlay aanmaken die een spel opnieuw kan spelen.
             * De savefile moet een 3-tal dingen bevatten om dit succesvol te kunnen doen:
             * 1: parameters van de Game(pparam1, param2) constructor die werden gebruikt om de Game te 
             *    instantiëren.
             * 2: De seed die gebruikt is tijdens het instantiëren van de game.
             * 3: Een array van commands die bepalen wie welke turns/moves deed in de verloop van het spel.
             * Het mooie van het gebruik van een seed is nu dat alle PSEUDOrandom verloop. Dit betekent dat
             * alle monster locaties, items locaties, dungeon size en monster moves identiek zijn voor elke
             * seed. Dus als een game eenmaal met een seed aangemaakt is, zal de game zich vanzelf spelen en
             * zal de GameState in de verloop van het spel voor een bepaalde seed op een bepaald moment na
             * een x aantal commands altijd hetzelfde zijn. 
             * 
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
            /*
             * Huidige turn opnieuw spelen houdt pretty much in dat we in commands[] gaan 
             * kijken en het commands dat op index "turn" staat uitvoeren en vervolgens de
             * turn variable met 1 verhogen.
             */
            string action = commands[turn];
            // execute action
            turn++;
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
