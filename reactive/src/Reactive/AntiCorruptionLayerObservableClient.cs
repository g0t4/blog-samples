namespace Reactive
{
    using System;
    using System.Reactive.Linq;

    public class AntiCorruptionLayerObservableClient
    {
        private static IObservable<FuturesQuote> FuturesQuotesSource(FuturesQuoteClient client)
        {
            // this is equivalent to FuturesQuoteClientWrapper, the reactive framework abstracts events for you :)
            return Observable
                .FromEvent<FuturesQuoteClient.QuoteHandler, FuturesQuote>(h => client.Quotes += h, h => client.Quotes -= h);
        }

        public AntiCorruptionLayerObservableClient(FuturesQuoteClient client)
            : this(FuturesQuotesSource(client))
        {
        }


        public AntiCorruptionLayerObservableClient(IObservable<FuturesQuote> quotes)
        {
            Quotes = quotes
                .Select(q => new NotifyOnBarrierEventsReactive.QuoteWithContract(q));
        }

        public IObservable<NotifyOnBarrierEventsReactive.QuoteWithContract> Quotes { get; private set; }
    }
}