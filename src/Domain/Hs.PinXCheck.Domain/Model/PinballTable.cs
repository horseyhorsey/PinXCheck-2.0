using System.Xml.Serialization;

namespace Hs.PinXCheck.Domain.Model
{
    public abstract class PinballTable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("manufacturer")]
        public string Manufacturer { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }
        
        [XmlElement("type")]       
        public string Type { get; set; }

        [XmlElement("genre")]
        public string Genre { get; set; }

        private bool isDescriptionMatched;
        [XmlIgnore]
        public bool IsDescriptionMatched
        {
            get { return isDescriptionMatched; }
            set { isDescriptionMatched = value; }
        }


    }
}
