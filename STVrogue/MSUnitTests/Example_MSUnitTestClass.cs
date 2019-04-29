using Microsoft.VisualStudio.TestTools.UnitTesting;
using STVrogue.GameLogic;

namespace MSUnitTests
{
    [TestClass]
    /* Just an example of an MSUnit test class to show how to write one. */
    public class MSUnitTestClass1
    {
        [TestMethod]
        /* We will test the constructor of Creature. */
        public void test_Creature_contr()
        {
            Creature C = new Creature("99");
            Assert.IsTrue(C.HP > 0);
            Assert.IsTrue(C.AttackRating > 0);
        }
    }
}
