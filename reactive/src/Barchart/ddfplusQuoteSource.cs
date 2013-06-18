namespace Barchart
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using ddfplus;

    public class ddfplusQuoteSource
    {
        private readonly Client _Client;
        public readonly IObservable<ParsedDdfQuote> QuoteStream;

        public ddfplusQuoteSource()
        {
            _Client = new Client();
            QuoteStream = CreateQuoteStream(_Client);
            SetupConnection();
        }

        private static IObservable<ParsedDdfQuote> CreateQuoteStream(Client client)
        {
            return Observable
                .FromEventPattern<Client.NewQuoteEventHandler, Client.NewQuoteEventArgs>(h => client.NewQuote += h, h => client.NewQuote -= h)
                .Select(e => e.EventArgs.Quote)
                .Select(q => new ParsedDdfQuote(q));
        }

        private void SetupConnection()
        {
            Connection.Username = Config.UserName;
            Connection.Password = Config.Password;
        }

        public void Start(IEnumerable<string> symbols)
        {
            _Client.Symbols = String.Join(",", symbols);
        }

        public void Stop()
        {
            _Client.Symbols = string.Empty;
            Connection.Close();
        }
    }
}