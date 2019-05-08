using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LitterManager.Models
{
    public class Litter
    {
        public int LitterId { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public bool MotherPresent { get; set; }
        public List<LitterBreedDescriptions> BreedDescriptions { get; set; }
        public PriceRange Prices { get; set; }
    }
}
