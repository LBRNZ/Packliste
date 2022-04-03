using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Packliste.Data
{
    public class Person : Identifiable
    {
        public string Name { get; set; }
        public Person(XmlData xmlData) : base(xmlData)
        {

        }

        public Person()
        {

        }
    }
}
