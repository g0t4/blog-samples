namespace Reactive.Tests
{
    using System.Linq;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class AntiCorruptionLayerObservableClientTests : AssertionHelper
    {
        [Test]
        public void OnFuturesQuote_AValidContractOnAFuturesQuote_TriggersQuoteWithContract()
        {
            var validFuturesQuote = new FuturesQuote
                {
                    Symbol = "CZ2013"
                };
            var scheduler = new TestScheduler();
            var quotes = scheduler.CreateColdObservable(ReactiveTest.OnNext(0, validFuturesQuote));
            var quotesWithContractClient = new AntiCorruptionLayerObservableClient(quotes);

            var result = scheduler.Start(() => quotesWithContractClient.Quotes);

            var quoteWithContract = result.Messages.Single().Value.Value;
            Expect(quoteWithContract.Quote, Is.EqualTo(validFuturesQuote));
        }
    }
}