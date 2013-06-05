namespace Reactive
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class NotifyOnBarrierEvents
    {
        private ConcurrentDictionary<CommodityContract, IEnumerable<CommodityBarrierOption>> _ActiveBarrierOptionsByContract;

        private ConcurrentDictionary<CommodityContract, IEnumerable<CommodityBarrierOption>> GetActiveBarrierOptionsByContract()
        {
            return new ConcurrentDictionary<CommodityContract, IEnumerable<CommodityBarrierOption>>();
        }

        public void Run()
        {
            _ActiveBarrierOptionsByContract = GetActiveBarrierOptionsByContract();
            var client = new FuturesQuoteClient();
            client.Quotes += OnQuote;
        }

        private void OnQuote(object sender, FuturesQuote quote)
        {
            var contract = ContractFromQuote(quote);
            if (IsValidContract(contract))
            {
                return;
            }
            IEnumerable<CommodityBarrierOption> activeBarrierOptionsForContract;
            if (!_ActiveBarrierOptionsByContract.TryGetValue(contract, out activeBarrierOptionsForContract))
            {
                return;
            }
            activeBarrierOptionsForContract
                .Where(option => BarrierIsBreached(option, quote))
                .Where(NoticeNotAlreadySent)
                .ToList()
                .ForEach(option => NotifyTheHumans(option, quote));
        }

        private static bool IsValidContract(CommodityContract contract)
        {
            return contract == null;
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
            try
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
            catch
            {
                // note of course logging or w/e to notify
                return null;
            }
        }

        private bool BarrierIsBreached(CommodityBarrierOption option, FuturesQuote quote)
        {
            if (option.Barrier.Direction == BarrierDirection.Down)
            {
                return option.Barrier.Level > quote.Low;
            }
            return option.Barrier.Level < quote.High;
        }

        private readonly ConcurrentDictionary<CommodityBarrierOption, CommodityBarrierOption> _NotifiedTrades = new ConcurrentDictionary<CommodityBarrierOption, CommodityBarrierOption>();

        private bool NoticeNotAlreadySent(CommodityBarrierOption option)
        {
            return _NotifiedTrades.TryAdd(option, option);
        }

        private void NotifyTheHumans(CommodityBarrierOption option, FuturesQuote quote)
        {
        }
    }
}