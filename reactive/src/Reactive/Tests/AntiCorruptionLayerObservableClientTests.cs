namespace Reactive.Tests
{
    using System.Linq;
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class AntiCorruptionLayerObservableClientTests : AssertionHelper
    {
        [Test]
        public void OnFuturesQuote_WithAValidContract_StreamsQuoteWithContract()
        {
            var validFuturesQuote = new FuturesQuote
                {
                    Symbol = "CZ2013"
                };
            var scheduler = new TestScheduler();
            var quotes = scheduler.CreateColdObservable(ReactiveTest.OnNext(0, validFuturesQuote));
            var quotesWithContractClient = new AntiCorruptionLayerObservableClient(quotes);

            var result = scheduler.Start(() => quotesWithContractClient.Quotes);

            result.Messages.Should().HaveCount(1);
            var quoteWithContract = result.Messages.Single().Value.Value;
            quoteWithContract.Quote.ShouldBeEquivalentTo(validFuturesQuote);
        }

        [Test]
        public void OnFuturesQuote_WithAnInvalidContract_StreamsNothing()
        {
            var validFuturesQuote = new FuturesQuote
                {
                    Symbol = "invalidcontract"
                };
            var scheduler = new TestScheduler();
            var quotes = scheduler.CreateColdObservable(ReactiveTest.OnNext(0, validFuturesQuote));
            var quotesWithContractClient = new AntiCorruptionLayerObservableClient(quotes);

            var result = scheduler.Start(() => quotesWithContractClient.Quotes);

            result.Messages.Should().BeEmpty();
        }
    }
}