namespace Reactive.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    public class CommodityContractTests
    {
        [Test]
        public void MapFromFuturesQuote()
        {
            var quote = new FuturesQuote
                {
                    Symbol = "CZ2013"
                };

            var commodityContract = NotifyOnBarrierEventsReactive.QuoteWithContract.ContractFromQuote(quote);

            commodityContract.ContractMonth.Should().Be(12);
            commodityContract.ContractYear.Should().Be(2013);
            commodityContract.ProductCode.Should().Be("C");
        }
    }
}