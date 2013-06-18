namespace Barchart.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReflectionMagic;
    using ddfplus;

    [TestFixture]
    public class ParsedDdfQuoteTests : AssertionHelper
    {
        [Test]
        public void Create_FromQuote_MapsHighAndLow()
        {
            var quote = new Quote();
            var session = AddCombinedSession(quote);
            session.AsDynamic().High = 2;
            session.AsDynamic().Low = 1;

            var parsed = new ParsedDdfQuote(quote);

            parsed.High.ShouldBeEquivalentTo(2);
            parsed.Low.ShouldBeEquivalentTo(1);
        }

        private static Session AddCombinedSession(Quote quote)
        {
            var session = new Session();
            quote.AsDynamic().AddSession("combined", session);
            return session;
        }

        [Test]
        public void Create_FromQuote_MapsSymbol()
        {
            var quote = new Quote();
            AddCombinedSession(quote);
            quote.AsDynamic().Symbol = "CZ13";

            var parsed = new ParsedDdfQuote(quote);

            parsed.Symbol.ShouldBeEquivalentTo("CZ13");
        }

        [Test]
        public void IsInitialized_UninitializedQuote_ReturnsFalse()
        {
            var uninitialized = new ParsedDdfQuote {High = 0};

            uninitialized.IsInitialized().Should().BeFalse();
        }
    }
}