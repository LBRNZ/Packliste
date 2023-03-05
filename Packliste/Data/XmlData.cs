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
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Journey> Journeys { get; set; }
        private static XmlSerializer serializer = new XmlSerializer(typeof(XmlData));

        public XmlData()
        {
            Persons = new ObservableCollection<Person>();
            Categories = new ObservableCollection<Category>();
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
                    if (innerReader.NodeType is XmlNodeType.Element)
                    {
                        do
                        {
                            Person person = (Person)childSerializer.Deserialize(innerReader);
                            person.SetXmlData(this);
                            Persons.Add(person);
                        } while (innerReader.Depth > 0);
                    }

                }
            }
            if (reader.NodeType is XmlNodeType.EndElement)
            {
                reader.ReadEndElement();
            }


            childSerializer = new XmlSerializer(typeof(Category));
            if (reader.Name == "Categories" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Category");
                    if (innerReader.NodeType is XmlNodeType.Element)
                    {
                        do
                        {
                            Category category = (Category)childSerializer.Deserialize(innerReader);
                            category.SetXmlData(this);
                            Categories.Add(category);
                        } while (innerReader.Depth > 0);
                    }
                }
            }
            if (reader.NodeType is XmlNodeType.EndElement)
            {
                reader.ReadEndElement();
            }

            childSerializer = new XmlSerializer(typeof(Item));
            if (reader.Name == "Items" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Item");
                    if (innerReader.NodeType is XmlNodeType.Element)
                    {
                        do
                        {
                            Item item = new Item(this);
                            item.ReadXml(innerReader);
                            Items.Add(item);
                        } while (innerReader.ReadToNextSibling("Item"));
                    }
                }
            }
            if (reader.NodeType is XmlNodeType.EndElement)
            {
                reader.ReadEndElement();
            }

            if (reader.Name == "Journeys" && reader.IsStartElement())
            {
                using (var innerReader = reader.ReadSubtree())
                {
                    innerReader.ReadToFollowing("Journey");
                    if (innerReader.NodeType is XmlNodeType.Element)
                    {
                        do
                        {
                            Journey journey = new Journey(this);
                            journey.ReadXml(innerReader);
                            Journeys.Add(journey);
                            reader.ReadEndElement();
                        } while (innerReader.ReadToNextSibling("Journey"));
                    }
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

            writer.WriteStartElement("Categories");
            childSerializer = new XmlSerializer(typeof(Category));
            foreach (Category child in Categories)
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

        public void RemoveCategory(Category category)
        {
            if (Items.Any(x => x.Category == category))
            {
                string items = string.Join(", ", Items.Where(x => x.Category == category).Select(i => i.Name)); 
                throw new InvalidOperationException("Die Kategorie wird noch von folgenden Gegenständen genutzt: " + items);
            }
            Categories.Remove(category);
        }

        public void RemoveItem(Item item)
        {
            if (Journeys.Any(x => x.Travelers.Any(t => t.itemSets.Any(i => i.Item ==item))))
            {
                string journeys = string.Join(", ", Journeys.Where(x => x.Travelers.Any(t => t.itemSets.Any(i => i.Item == item))).Select(j => j.Destination));
                throw new InvalidOperationException("Der Gegenstand wird noch von folgenden Reisen genutzt: " + journeys);
            }
            Items.Remove(item);
        }

        public void RemovePerson(Person person)
        {
            if (Journeys.Any(x => x.Travelers.Any(t => t.Person == person)))
            {
                string journeys = string.Join(", ", Journeys.Where(x => x.Travelers.Any(t => t.Person == person)).Select(j => j.Destination));
                throw new InvalidOperationException("Die Person wird noch von folgenden Reisen genutzt: " + journeys);
            }
            Persons.Remove(person);
        }
    }
}
