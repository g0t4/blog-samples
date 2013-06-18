namespace Barchart.Tests
{
    using FluentAssertions;
    using Microsoft.Reactive.Testing;
    using NUnit.Framework;

    public class ddfplusQuoteSourceTests
    {
        [Test]
        public void ExcludeUninitializedQuotes()
        {
            var uninitializedQuote = new ParsedDdfQuote {High = 0};
            var scheduler = new TestScheduler();
            var quotes = scheduler.CreateHotObservable(ReactiveTest.OnNext(201, uninitializedQuote));

            var onlyInitialized = quotes.ExcludeUninitializedQuotes();
            var quotesObserver = scheduler.Start(() => onlyInitialized); // overload 

            quotesObserver.Messages.Should().BeEmpty();
        }
    }
}