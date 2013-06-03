namespace Reactive
{
    public class FuturesQuoteClient
    {
        public delegate void QuoteHanlder(object sender, FuturesQuote quote);

        public event QuoteHanlder Quotes;

        protected virtual void OnQuote(FuturesQuote quote)
        {
            var handler = Quotes;
            if (handler != null) handler(this, quote);
        }

        public void Subscribe()
        {
            // todo generate simulated data
        }
    }
}