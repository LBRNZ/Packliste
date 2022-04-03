using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packliste.Data
{
    public class Item : Identifiable
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public Item (XmlData xmlData) : base(xmlData)
        {

        }

        public Item()
        {

        }
    }
}
