namespace Reactive
{
    public class CommodityBarrierOption
    {
        // note there are other properties that would existing in reality like call/put and strike but they aren't pertinent to the example so I've excluded them
        public Barrier Barrier { get; set; }
        public CommodityContract Contract { get; set; }
    }
}