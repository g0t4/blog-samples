namespace Reactive
{
    public class FuturesQuoteClient
    {
        public delegate void QuoteHandler(object sender, FuturesQuote quote);

        public event QuoteHandler Quotes;

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