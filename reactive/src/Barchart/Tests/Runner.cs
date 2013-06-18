namespace Barchart.Tests
{
    using System;
    using System.Diagnostics;

    public class Runner
    {
        public static void Main()
        {
            // note: ddfplus Connection will log notifications via Trace
            Trace.Listeners.Add(new ConsoleTraceListener());

            var source = new ddfplusQuoteSource();
            source.QuoteStream
                  .ExcludeUninitializedQuotes()
                  .Subscribe(PrintQuote);

            // note we are subscribing to CME Globex Corn futures, ZC is the symbol, ^F means all futures contracts
            // see this link for more information about CME Globex Corn futures http://www.cmegroup.com/trading/agricultural/grain-and-oilseed/corn_contract_specifications.html
            var symbols = new[] {"ZC^F"};
            source.Start(symbols);

            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            source.Stop();
        }

        private static void PrintQuote(ParsedDdfQuote quote)
        {
            Console.WriteLine(new {quote.Symbol, quote.High, quote.Low});
        }
    }
}