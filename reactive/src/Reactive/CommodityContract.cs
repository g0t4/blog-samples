namespace Reactive
{
    public class CommodityContract
    {
        public string ProductCode { get; set; }
        public int ContractMonth { get; set; }
        public int ContractYear { get; set; }

        protected bool Equals(CommodityContract other)
        {
            return string.Equals(ProductCode, other.ProductCode) && ContractMonth == other.ContractMonth && ContractYear == other.ContractYear;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommodityContract) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ProductCode != null ? ProductCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ ContractMonth;
                hashCode = (hashCode*397) ^ ContractYear;
                return hashCode;
            }
        }
    }
}