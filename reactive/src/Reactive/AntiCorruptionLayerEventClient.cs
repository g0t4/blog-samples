namespace Reactive
{
    /// <summary>
    ///     This demonstrates composition with events, and how it gets not so pretty :(
    /// </summary>
    public class AntiCorruptionLayerEventClient
    {
        public AntiCorruptionLayerEventClient(IFuturesQuoteClient client)
        {
            client.Quotes += TransformToQuoteWithContract;
            // neglects handling unsubscribing from the event and cascading that up the chain :)
        }

        private void TransformToQuoteWithContract(object sender, FuturesQuote quote)
        {
            OnQuotes(new NotifyOnBarrierEventsReactive.QuoteWithContract(quote));
        }

        public delegate void QuoteWithContractHandler(object sender, NotifyOnBarrierEventsReactive.QuoteWithContract args);

        public event QuoteWithContractHandler Quotes;

        protected virtual void OnQuotes(NotifyOnBarrierEventsReactive.QuoteWithContract quote)
        {
            var handler = Quotes;
            if (handler != null) handler(this, quote);
        }
    }

    /// <summary>
    ///     An interface to abstract the quote source.
    /// </summary>
    public interface IFuturesQuoteClient
    {
        event FuturesQuoteClient.QuoteHandler Quotes;
    }

    /// <summary>
    ///     Implementation of real wrapper around quote source.
    /// </summary>
    public class FuturesQuoteClientWrapper : IFuturesQuoteClient
    {
        public FuturesQuoteClientWrapper(FuturesQuoteClient client)
        {
            client.Quotes += (sender, quote) =>
                {
                    var handler = Quotes;
                    if (handler != null) handler(this, quote);
                };
        }

        public event FuturesQuoteClient.QuoteHandler Quotes;
    }
}