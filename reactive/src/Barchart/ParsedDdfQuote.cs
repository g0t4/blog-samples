namespace Barchart
{
    using System;
    using ddfplus;

    public class ParsedDdfQuote
    {
        public ParsedDdfQuote()
        {
        }

        public ParsedDdfQuote(Quote quote)
        {
            var combinedSession = quote.Sessions["combined"];
            High = Convert.ToDecimal(combinedSession.High);
            Low = Convert.ToDecimal(combinedSession.Low);
            Symbol = quote.Symbol;
        }

        public decimal High { get; set; }
        public decimal Low { get; set; }
        public string Symbol { get; set; }

        public bool IsInitialized()
        {
            return High > 0;
        }
    }
}