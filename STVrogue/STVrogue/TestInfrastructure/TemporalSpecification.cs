using System;
using System.Collections.Generic;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure
{

    public enum Judgement {
        RelevantlyValid, TriviallyValid, Invalid
    }

    /* ITERATION 2.
     * A "temporal specification" represents a correctness property that should hold over
     * an entire gameplay.
     */
    public abstract class TemporalSpecification
    {

        /* 
         * Check if this specification holds on the given gameplay. It returns a Judgement
         * with the following meaning:
         *    Invalid : the gameplay violates this specification (in other words this spec
         *        is invalid on the gameplay).
         *    TriviallyValid : the gameplay satisfies this specification, but only because it does not
         *        meet this specification's assumptions (if there are any). Therefore
         *        the specification is only trivially valid on the gameplay.
         *    RelevantlyValid : the gameplay satisfies this specification's assumptions and its claim.
         *        Therefore it is relevantly valid.
         */
        abstract public Judgement evaluate(GamePlay gameplay);

        /*
        *  Evaluate this specification on a bunch of gameplays. It only returns RelevantlyValid (2)
        *  if there is no violation and furthermore the number of gameplays on which this
        *  specification is relevantly valid is at least the specified threshold.
        */
        public Judgement evaluate(GamePlay[] gameplays, int threshold)
        {
            int countRelevantlyValid = 0;
            for (int k = 0; k < gameplays.Length; k++)
            {
                Judgement verdict = evaluate(gameplays[k]);
                if (verdict == Judgement.Invalid) return Judgement.Invalid;
                if (verdict == Judgement.RelevantlyValid) countRelevantlyValid++;
            }
            if (countRelevantlyValid >= threshold) return Judgement.RelevantlyValid;
            return Judgement.TriviallyValid;
        }

        /* Evaluate this specification on a bunch of gameplays. */
        public Judgement evaluate(List<GamePlay> gameplays, int threshold)
        {
            return evaluate(gameplays.ToArray(), threshold);
        }
    }

    /*
     * Representing a temporal property of the form "Always p". A gameplay
     * satisfies this property if p holds on the game state through out the
     * play.
     */
    public class Always : TemporalSpecification
    {
        Predicate<Game> p;

        public Always(Predicate<Game> p) { this.p = p; }
       
        public override Judgement evaluate(GamePlay sigma)
        {
            sigma.reset();
            // check the initial state:
            Boolean ok = p(sigma.getState());
            if (!ok)
            {
                // the predicate p is violated!
                Utils.log("violation of Always at turn " + sigma.getTurn());
                return Judgement.Invalid;
            }
            while (!sigma.atTheEnd())
            {
                // replay the current turn (and get the next turn)
                sigma.replayCurrentTurn();
                // check if p holds on the state that resulted from replaying the turn
                ok = p(sigma.getState());
                if (!ok)
                {
                    // the predicate p is violated!
                    Utils.log("violation of Always at turn " + sigma.getTurn());
                    return Judgement.Invalid;
                }

            }
            // if we reach this point than p holds on every state in the gameplay:
            return Judgement.RelevantlyValid;
        }
    }

    public class Unless : TemporalSpecification
    {
        Predicate<Game> p;
        Predicate<Game> q;
        public Unless(Predicate<Game> p, Predicate<Game> q) { this.p = p; this.q = q; }

        private Judgement judgement(Boolean ok, Boolean relevant)
        {
            if (ok && relevant) return Judgement.RelevantlyValid;
            if (ok && !relevant) return Judgement.TriviallyValid;
            return Judgement.Invalid;
        }

        public override Judgement evaluate(GamePlay sigma)
        {
            sigma.reset();
            Boolean relevant = false;
            Boolean ok = true;
            Boolean previous_pAndNotq; // to keep track if p && ~q holds in the previous state
            if (sigma.atTheEnd()) return judgement(ok, relevant);
            // else we have at least one turn
            Game currentState = sigma.getState();
            previous_pAndNotq = p(currentState) && !q(currentState);
            relevant = relevant || previous_pAndNotq;

            while (!sigma.atTheEnd())
            {
                sigma.replayCurrentTurn();
                currentState = sigma.getState();
                if (previous_pAndNotq) ok = p(currentState) || q(currentState);
                else ok = true;
                if (!ok)
                {
                    // the predicate p is violated!
                    Utils.log("violation of Unless at turn " + sigma.getTurn());
                    return judgement(ok, relevant);
                }
                previous_pAndNotq = p(currentState) && !q(currentState);
                relevant = relevant || previous_pAndNotq;
            }
            return judgement(ok, relevant);
        }
    }


}
