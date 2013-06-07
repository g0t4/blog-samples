namespace Reactive.Tests
{
    using System.Collections.Generic;
    using System.Linq;
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
            var quotes = new List<NotifyOnBarrierEventsReactive.QuoteWithContract>();
            quotesWithContractClient.Quotes += (sender, quote) => quotes.Add(quote);

            futuresQuoteClient.Raise(c => c.Quotes += null, null, validFuturesQuote);

            quotes.Should().HaveCount(1);
            var quoteWithContract = quotes.Single();
            quoteWithContract.Quote.ShouldBeEquivalentTo(validFuturesQuote);
        }
    }
}