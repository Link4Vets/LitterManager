using System.Linq;

namespace LitterManager.Models
{
    public class PriceRange
    {
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
        public double PriceAvg { get; set; }

        public PriceRange()
        {
        }

        public PriceRange(int from, int to)
        {
            this.PriceFrom = from;
            this.PriceTo = to;

            int[] PriceArray = { from, to };
            this.PriceAvg = PriceArray.Average();
        }
    }
}
