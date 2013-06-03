namespace Reactive
{
    using System;

    public class FuturesQuote
    {
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Last { get; set; }
        public DateTime Time { get; set; }
        public string Symbol { get; set; }
        public string Market { get; set; }
    }
}