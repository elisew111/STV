using System;
using STVrogue.GameLogic;

namespace STVrogue.TestInfrastructure
{
    public class ExampleTemporalSpecification
    {
        static public TemporalSpecification example1 = new Always(G => G.player.HP >= 0);
        static public TemporalSpecification example2 = new Unless(G => G.whoHasTheTurn == G.player, G => G.whoHasTheTurn is Monster);

    }  
}
