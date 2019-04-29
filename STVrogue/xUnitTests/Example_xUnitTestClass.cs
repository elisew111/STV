using System;
using STVrogue.GameLogic;
using Xunit;

namespace xUnitTests
{
    /* Just an example of an xUnit test class to show how to write one. */
    public class xUnitTestClass1
    {
        [Fact]
        /* We will test the constructor of Creature. */
        public void test_Creature_contr()
        {
            Creature C = new Creature("99");
            Assert.True(C.HP > 0);
            Assert.True(C.AttackRating > 0);
        }
    }
}
