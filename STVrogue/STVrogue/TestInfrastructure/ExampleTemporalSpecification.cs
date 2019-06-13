using System;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure
{
    public class ExampleTemporalSpecification
    {
        static public TemporalSpecification example1 = new Always(G => G.player.HP >= 0);
        static public TemporalSpecification example2 = new Unless(G => G.whoHasTheTurn == G.player, G => G.whoHasTheTurn is Monster);

        //RHP: the player’s HP should never exceed its HPMax
        static public TemporalSpecification RHP = new Always(G => G.player.HP <= G.player.HPmax);
        //RExit: the exit node should always be reachable from the player’s current location.
        static public TemporalSpecification RExit = new Always(G => G.player.location.isReachable(G.dungeon.getExitnode()));
        //RNodeCap: nodes should never hold more monsters than allowed by their capacity.
        static public TemporalSpecification RNodeCap = new Always(G => HelperPredicates.forall(G.dungeon.nodes(), n => n.monsters.Count <= n.capacity));
        //RAlive: the player should not die for no reason.That is, when the player is alive, it should remain alive at least until it enters a combat
        static public TemporalSpecification RAlive = new Unless(G => G.player.inCombat, G => G.player.HP > 0);
        //RItem: the total number of items in the game can only decrease
        

        //static public TemporalSpecification RItem = new Unless (Game => Game.items.Count == X, Game => Game.items.Count == X - 1);

        //RDecay: when the game has no item left(lying around in nodes nor in the player’s bag), the player’s HP can only decrease.
        //static public TemporalSpecification RDecay = new Unless(Game => Game.items.Count > 0 || Game.player.bag.Count > 0, new Unless(G => G.player.HP == X, G => G.player.HP < X));
        
        //RMonster: The HP of every monster can only decrease, and it never leaves its zone.
        //static public TemporalSpecification RMonster1 = new Always(Game => HelperPredicates.forall(Game.getmonsters(),m => new Unless(m => m.HP == X, m => m.HP < X))); 
        //static public TemporalSpecification RMonster = new Unless()
    }   //how do I refer to monsters?
}
