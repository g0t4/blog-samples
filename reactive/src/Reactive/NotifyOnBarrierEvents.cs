namespace Reactive
{
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
            var contract = NotifyOnBarrierEventsReactive.QuoteWithContract.ContractFromQuote(quote);
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