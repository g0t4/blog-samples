namespace Reactive.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class AntiCorruptionLayerEventClientTests : AssertionHelper
    {
        [Test]
        public void OnFuturesQuote_AValidContractOnAFuturesQuote_TriggersQuoteWithContract()
        {
            var validFuturesQuote = new FuturesQuote
                {
                    Symbol = "CZ2013"
                };
            var futuresQuoteClient = MockRepository.GenerateStub<IFuturesQuoteClient>();
            var quotesWithContractClient = new AntiCorruptionLayerEventClient(futuresQuoteClient);
            NotifyOnBarrierEventsReactive.QuoteWithContract quote = null;
            quotesWithContractClient.Quotes += (sender, quoteWithContract) => quote = quoteWithContract;

            futuresQuoteClient.Raise(c => c.Quotes += null, null, validFuturesQuote);

            quote.Quote.ShouldBeEquivalentTo(validFuturesQuote);
        }
    }
}