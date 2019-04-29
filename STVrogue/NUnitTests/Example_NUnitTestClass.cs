using NUnit.Framework;
using STVrogue.GameLogic;

namespace NUnitTests
{
    /* Just an example of an NUnit test class to show how to write one. */
    public class NUnitTestClass1
    {
        [SetUp]
        public void Setup(){ }

        [Test]
        /* We will test the constructor of Creature. */
        public void test_Creature_contr()
        {
            Creature C = new Creature("99");
            Assert.IsTrue(C.HP > 0);
            Assert.IsTrue(C.AttackRating > 0);
        }
    }
}