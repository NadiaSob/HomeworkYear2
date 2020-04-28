using Attributes;

namespace IncorrectTestProject
{
    public class IncorrectTests
    {
        [Test]
        public int IncorrectTest1() => 1234;

        [Test]
        public void IncorrectTest2(string testString)
        {
        }
    }
}
