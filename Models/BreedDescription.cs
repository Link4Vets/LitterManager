using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LitterManager.Models
{
    public class BreedDescription
    {
        public int Id { get; set; }
        public int FciId { get; set; }
        public int GroupId { get; set; }
        public int SectionId { get; set; }
        public string BreedName { get; set; }
        public string SectionName { get; set; }
        public string GroupName { get; set; }
        public List<LitterBreedDescriptions> Litters { get; set; }
    }

    public class LitterBreedDescriptions
    {
        public int LitterId { get; set; }
        public Litter  Litter { get; set; }
        public int BreedDescriptionId { get; set; }
        public BreedDescription BreedDescription { get; set; }
    }
}