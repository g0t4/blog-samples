namespace Reactive
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;

    public class NotifyOnBarrierEventsReactive
    {
        private ConcurrentDictionary<CommodityContract, IEnumerable<CommodityBarrierOption>> _ActiveBarrierOptionsByContract;

        private ConcurrentDictionary<CommodityContract, IEnumerable<CommodityBarrierOption>> GetActiveBarrierOptionsByContract()
        {
            return new ConcurrentDictionary<CommodityContract, IEnumerable<CommodityBarrierOption>>();
        }

        private static IObservable<FuturesQuote> FuturesQuotesSource(FuturesQuoteClient client)
        {
            return Observable
                .FromEvent<FuturesQuoteClient.QuoteHanlder, FuturesQuote>(h => client.Quotes += h, h => client.Quotes -= h);
        }

        public void Run()
        {
            _ActiveBarrierOptionsByContract = GetActiveBarrierOptionsByContract();
            var client = new FuturesQuoteClient();
            FuturesQuotesSource(client)
                .Select(q => new QuoteWithContract(q))
                .Where(IsValidContract)
                .SelectMany(BarrierBreachNotices)
                .Where(NoticeNotAlreadySent)
                .Subscribe(NotifyTheHumans);
        }

        private static bool IsValidContract(QuoteWithContract quote)
        {
            return quote.Contract != null;
        }

        private IEnumerable<BarrierEvent> BarrierBreachNotices(QuoteWithContract quote)
        {
            IEnumerable<CommodityBarrierOption> options;
            if (!_ActiveBarrierOptionsByContract.TryGetValue(quote.Contract, out options))
            {
                return Enumerable.Empty<BarrierEvent>();
            }
            return options
                .Where(option => BarrierIsBreached(option, quote.Quote))
                .Select(option => CreateNotice(option, quote));
        }

        private bool BarrierIsBreached(CommodityBarrierOption option, FuturesQuote quote)
        {
            if (option.Barrier.Direction == BarrierDirection.Down)
            {
                return option.Barrier.Level > quote.Low;
            }
            return option.Barrier.Level < quote.High;
        }

        private BarrierEvent CreateNotice(CommodityBarrierOption option, QuoteWithContract quote)
        {
            return new BarrierEvent {Option = option, Quote = quote};
        }

        private readonly ConcurrentDictionary<CommodityBarrierOption, CommodityBarrierOption> _NotifiedTrades = new ConcurrentDictionary<CommodityBarrierOption, CommodityBarrierOption>();

        private bool NoticeNotAlreadySent(BarrierEvent barrierEvent)
        {
            return _NotifiedTrades.TryAdd(barrierEvent.Option, barrierEvent.Option);
        }

        private void NotifyTheHumans(BarrierEvent barrierEvent)
        {
        }

        public class BarrierEvent
        {
            public CommodityBarrierOption Option { get; set; }
            public QuoteWithContract Quote { get; set; }
        }

        public class QuoteWithContract
        {
            public FuturesQuote Quote { get; set; }
            public CommodityContract Contract { get; set; }

            public QuoteWithContract(FuturesQuote quote)
            {
                Quote = quote;
                Contract = ContractFromQuote(quote);
            }

            public static IDictionary<string, int> Months = new Dictionary<string, int>
                {
                    {"F", 1},
                    {"G", 2},
                    {"H", 3},
                    {"J", 4},
                    {"K", 5},
                    {"M", 6},
                    {"N", 7},
                    {"Q", 8},
                    {"U", 9},
                    {"V", 10},
                    {"X", 11},
                    {"Z", 12}
                };

            public static CommodityContract ContractFromQuote(FuturesQuote quote)
            {
                var year = quote.Symbol.Substring(quote.Symbol.Length - 4);
                var monthCode = quote.Symbol.Substring(quote.Symbol.Length - 5, 1);
                var productCode = quote.Symbol.Substring(0, quote.Symbol.Length - 5);
                return new CommodityContract
                    {
                        ContractYear = Convert.ToInt32(year),
                        ContractMonth = Months[monthCode],
                        ProductCode = productCode
                    };
            }
        }
    }
}