using Task16;
namespace PhoneCallUnitTests
{
    [TestFixture]
    public class PhoneCallTests
    {
        [Test]
        public void ConstructorTest()
        {
            var phoneCall = new PhoneCall(42, 2.5);
            Assert.That(phoneCall.Time, Is.EqualTo(42));
            Assert.That(phoneCall.Rate, Is.EqualTo(2.5));
            Assert.That(phoneCall.Cost, Is.EqualTo(1.75));
        }

        [TestCase(-42, 2.5)] 
        [TestCase(42, -2.5)]
        public void Constructor_InvalidValues_ShouldThrowException(int time, double rate)
        {
            Assert.Throws<ArgumentException>(() => new PhoneCall(time, rate));
        }

        [TestCase(135, 2.5, "Разговор: 135 с по 2,5 руб./мин.")]
        [TestCase(200, 1.8, "Разговор: 200 с по 1,8 руб./мин.")]
        [TestCase(60, 3, "Разговор: 60 с по 3 руб./мин.")]
        public void ToStringTest(int time, double rate, string expected)
        {
            var phoneCall = new PhoneCall(time, rate);
            string result = phoneCall.ToString();
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(120, 3.5, 120, 3.5, ExpectedResult = true)]  
        [TestCase(120, 3.5, 100, 3.5, ExpectedResult = false)] 
        [TestCase(120, 3.5, 120, 2.5, ExpectedResult = false)] 
        public bool EqualsTest(int time1, double rate1, int time2, double rate2)
        {
            var call1 = new PhoneCall(time1, rate1);
            var call2 = new PhoneCall(time2, rate2);
            return call1.Equals(call2);
        }
        [Test]
        public void Equals_CompareNull_ShouldThrowArgumentNullException()
        {
            var call1 = new PhoneCall(120, 3.5);
            Assert.Throws<ArgumentNullException>(() => call1.Equals(null));
        }
        [Test]
        public void Equals_WrongArgument_ShouldThrowArgumentException()
        {
            var call1 = new PhoneCall(120, 3.5);
            var notPhoneCall = new object();
            Assert.Throws<ArgumentException>(() => call1.Equals(notPhoneCall));
        }

        [Test]
        public static void GetHashCodeTest()
        {
            var x = new PhoneCall(42, 2.5);
            var y = new PhoneCall(42, 2.5);
            var z = new PhoneCall(30, 45);
            Assert.That(x.Equals(y), Is.True);
            Assert.That(x.Equals(z), Is.False);
        }

        [Test]
        public void AdditionTest()
        {
            var call1 = new PhoneCall(120, 2.5);
            var call2 = new PhoneCall(180, 2.5);
            var result = call1 + call2;

            Assert.That(result.Time, Is.EqualTo(300));
            Assert.That(result.Rate, Is.EqualTo(2.5));
        }
        [Test]
        public void Addition_DifferentRate_ShouldThrowInvalidOperationException()
        {
            var call1 = new PhoneCall(120, 2.5);
            var call2 = new PhoneCall(180, 3.0);
            Assert.Throws<InvalidOperationException>(() => { var result = call1 + call2; });
        }

        [TestCase(120, 2.5, 2, 120, 5)] 
        [TestCase(60, 1.8, 0.5, 60, 0.9)]
        public void MultiplicationTest(int time, double rate, double multiplier, int expectedTime, double expectedRate)
        {
            var call = new PhoneCall(time, rate);
            var result = call * multiplier;

            Assert.That(result.Time, Is.EqualTo(expectedTime));
            Assert.That(result.Rate, Is.EqualTo(expectedRate)); 
        }
        [Test]
        public void Multiplication_NegativeMultiplier_ShouldThrowArgumentException()
        {
            var call = new PhoneCall(120, 2.5);

            Assert.Throws<ArgumentException>(() => { var result = call * -1; });
            Assert.Throws<ArgumentException>(() => { var result = call * 0; });
        }
    }
}