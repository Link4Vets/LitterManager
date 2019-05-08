namespace LitterManager.Models
{
    public class FeatureDescription
    {
        public AnimalSize Size { get; set; }
        public AnimalFunction Function { get; set; }
        public AnimalFurType FurType { get; set; }
        public int EnergyLevel { get; set; }
    }
}

namespace LitterManager
{ 

    public enum AnimalSize { Small, Medium, Large }

    public enum AnimalFunction { Gezelschap, Jachthond, Waakhond, Spelenenwandelen, Andere }

    public enum AnimalFurType { Langharig, Kortharig, Ruwharig }
}
