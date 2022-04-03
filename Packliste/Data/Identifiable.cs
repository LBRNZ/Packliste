using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Packliste.Data
{
    public class Identifiable
    {
        [XmlIgnore]
        protected XmlData XmlData { get; private set; }
        public Guid Identifier { get; set; }

        public Identifiable(XmlData xmlData)
        {
            Identifier = Guid.NewGuid();
            XmlData = xmlData;
        }
        public Identifiable()
        {
            Identifier = Guid.NewGuid();
        }

        public void SetXmlData(XmlData xmlData)
        {
            XmlData = xmlData;
        }
    }
}
