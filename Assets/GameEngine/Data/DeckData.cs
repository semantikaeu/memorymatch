namespace Assets.GameEngine.Data
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot]
    public class DeckData
    {
        public DeckData()
        {
            ImageLocation = "Internet";
        }

        [XmlElement]
        public string Id { get; set; }

        [XmlElement]
        public string Title { get; set; }

        [XmlElement]
        public string ResourceName { get; set; }

        [XmlElement]
        public string ImageLocation { get; set; }

        [XmlArray]
        public List<CardData> Cards { get; set; }
    }
}
