using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Packliste.Data
{
    public class Item : Identifiable, IXmlSerializable
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public Category Category { get; set; }

        public Item (XmlData xmlData) : base(xmlData)
        {

        }

        public Item()
        {

        }

        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            if (reader.Name == "Identifier")
            {
                Identifier = Guid.Parse(reader.ReadElementContentAsString());
            }

            if (reader.Name == "Name")
            {
                Name = reader.ReadElementContentAsString();
            }

            if (reader.Name == "Weight")
            {
                Weight = reader.ReadElementContentAsInt();
            }

            if (reader.Name == "Category-REF")
            {
                var categoryID = Guid.Parse(reader.ReadElementContentAsString());
                Category = XmlData.Categories.FirstOrDefault(x => x.Identifier == categoryID);
            }

            
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Identifier", Identifier.ToString());
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("Weight", Weight.ToString());
            writer.WriteElementString("Category-REF", Category.Identifier.ToString());
        }
    }
}
