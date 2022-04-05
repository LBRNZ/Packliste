using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Packliste.Data
{
    public class Journey : Identifiable, IXmlSerializable
    {
        public string Destination { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ObservableCollection<Traveler> Travelers { get; set; }

        public Journey(XmlData xmlData) : base(xmlData)
        {
            Travelers = new ObservableCollection<Traveler>();
        }

        public Journey()
        {

        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            if(reader.Name == "Destination")
            {
                Destination = reader.ReadElementContentAsString();
            }

            if (reader.Name == "StartDate")
            {
                StartDate = DateOnly.Parse(reader.ReadElementContentAsString());
            }

            if (reader.Name == "EndDate")
            {
                EndDate = DateOnly.Parse(reader.ReadElementContentAsString());
            }

            if (reader.Name == "Travelers" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Traveler-REF");
                    do
                    {
                        var travelerId = Guid.Parse(innerReader.GetAttribute("Identifier"));
                        var traveler = new Traveler(XmlData.Persons.FirstOrDefault(x => x.Identifier == travelerId));

                        using (var itemReader = reader.ReadSubtree())
                        {
                            itemReader.ReadToFollowing("ItemSet-REF");
                            if (!itemReader.IsEmptyElement)
                            {
                                do
                                {
                                    var itemId = Guid.Parse(itemReader.GetAttribute("Identifier"));
                                    var item = XmlData.Items.FirstOrDefault(x => x.Identifier == itemId);
                                    traveler.itemSets.Add(new ItemSet { Count = itemReader.ReadElementContentAsInt(), Item = item });
                                } while (itemReader.Depth > 0);
                            }

                        }
                        Travelers.Add(traveler);
                    } while(reader.ReadToNextSibling("Traveler-REF"));
                }
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Destination", Destination);
            writer.WriteElementString("StartDate", StartDate.ToString());
            writer.WriteElementString("EndDate", EndDate.ToString());
            writer.WriteStartElement("Travelers");
            foreach (Traveler traveler in Travelers)
            {
                writer.WriteStartElement("Traveler-REF");
                writer.WriteAttributeString("Identifier", traveler.Person.Identifier.ToString());

                foreach (ItemSet itemSet in traveler.itemSets)
                {
                    writer.WriteStartElement("ItemSet-REF");
                    writer.WriteAttributeString("Identifier", itemSet.Item.Identifier.ToString());
                    writer.WriteString(itemSet.Count.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
