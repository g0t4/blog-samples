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
            var futuresQuotes = scheduler.CreateColdObservable(ReactiveTest.OnNext(0, validFuturesQuote));
            var quotesWithContractClient = new AntiCorruptionLayerObservableClient(futuresQuotes);

            var quotesWithContracts = scheduler.Start(() => quotesWithContractClient.Quotes);

            quotesWithContracts.Messages.Should().HaveCount(1);
            var quoteWithContract = quotesWithContracts.Messages.Single().Value.Value;
            quoteWithContract.Quote.ShouldBeEquivalentTo(validFuturesQuote);
        }

        [Test]
        public void OnFuturesQuote_WithAnInvalidContract_StreamsNothing()
        {
            var invalidFuturesQuote = new FuturesQuote
                {
                    Symbol = "invalidcontract"
                };
            var scheduler = new TestScheduler();
            var futuresQuotes = scheduler.CreateColdObservable(ReactiveTest.OnNext(0, invalidFuturesQuote));
            var quotesWithContractClient = new AntiCorruptionLayerObservableClient(futuresQuotes);

            var quotesWithContracts = scheduler.Start(() => quotesWithContractClient.Quotes);

            quotesWithContracts.Messages.Should().BeEmpty();
        }
    }
}