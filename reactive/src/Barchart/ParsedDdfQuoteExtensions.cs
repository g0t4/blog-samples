namespace Barchart
{
    using System;
    using System.Reactive.Linq;

    public static class ParsedDdfQuoteExtensions
    {
        public static IObservable<ParsedDdfQuote> ExcludeUninitializedQuotes(this IObservable<ParsedDdfQuote> quotes)
        {
            return quotes
                .Where(q => q.IsInitialized());
        }
    }
}