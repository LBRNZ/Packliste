using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Packliste.Data
{
    public class XmlData : IXmlSerializable
    {
       
        public ObservableCollection<Person> Persons { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Journey> Journeys { get; set; }
        private static XmlSerializer serializer = new XmlSerializer(typeof(XmlData));

        public XmlData()
        {
            Persons = new ObservableCollection<Person>();
            Items = new ObservableCollection<Item>();
            Journeys = new ObservableCollection<Journey>();
        }

        public void Save()
        {
            using (var writer = new StreamWriter(@"Data.xml"))
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true }))
                {
                    serializer.Serialize(xmlWriter, this);
                }
            }
        }

        public XmlData Load()
        {
            using (StreamReader xmlFileReader = File.OpenText(@"Data.xml"))
            {
                return (XmlData)serializer.Deserialize(xmlFileReader);
            }
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            XmlSerializer childSerializer = new XmlSerializer(typeof(Person));
            if (reader.Name == "Persons" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Person");
                    do
                    {
                        Person person = (Person)childSerializer.Deserialize(innerReader);
                        person.SetXmlData(this);
                        Persons.Add(person);
                    } while (innerReader.Depth > 0);
                }
            }
            reader.ReadEndElement();

            childSerializer = new XmlSerializer(typeof(Item));
            if (reader.Name == "Items" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Item");
                    do
                    {
                        Item item = (Item)childSerializer.Deserialize(innerReader);
                        item.SetXmlData(this);
                        Items.Add(item);
                    } while (innerReader.Depth > 0);
                }
            }
            reader.ReadEndElement();

            if (reader.Name == "Journeys" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Journey");
                    do
                    {
                        Journey journey = new Journey(this);
                        journey.SetXmlData(this);
                        journey.ReadXml(innerReader);
                        Journeys.Add(journey);
                    } while (innerReader.ReadToNextSibling("Journey"));
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            writer.WriteStartElement("Persons");
            XmlSerializer childSerializer = new XmlSerializer(typeof(Person));
            foreach (Person child in Persons)
            {
                childSerializer.Serialize(writer, child, ns);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Items");
            childSerializer = new XmlSerializer(typeof(Item));
            foreach (Item child in Items)
            {
                childSerializer.Serialize(writer, child, ns);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Journeys");
            childSerializer = new XmlSerializer(typeof(Journey));
            foreach (Journey child in Journeys)
            {
                childSerializer.Serialize(writer, child, ns);
            }
            writer.WriteEndElement();
        }
    }
}
