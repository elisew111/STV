using System;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure
{
    /* For ITERATION 2.
     * Representing a replayable game play. 
     */
    public class GamePlay
    {
        protected int turn = 0;
        protected int length;

        public GamePlay(){ }

        public GamePlay(String savefile) 
        {
            throw new NotImplementedException();
        }

        /* reset the gameplay to turn 0 */
        public virtual void reset() { throw new NotImplementedException(); }

        /* return the current game state */
        public virtual Game getState() { throw new NotImplementedException(); }

        /* return the current turn number. */
        public int getTurn() { return turn; }

        /* true if the gameplay is at the end, hence has no more turn to do. */
        public Boolean atTheEnd() { return turn >= length; }

        /*
         * Replay the current turn, thus updating the game state.
         * This also increases the turn nr, thus shifting the current turn to the next one. 
         */
        public virtual void replayCurrentTurn() { throw new NotImplementedException(); }

    }

    /* A dummy GamePlay; for testing the specification classes */
    public class DummyGamePlay : GamePlay
    {
        int[] execution;
        Game state;
        public DummyGamePlay(int[] execution)
        {
            this.execution = execution;
            length = execution.Length - 1;
            state = new Game();
        }
        public override void reset() { turn = 0; state.z_ = execution[turn]; }
        public override Game getState() { return state; }
        public override void replayCurrentTurn() { turn++; state.z_ = execution[turn]; }
    }
}
