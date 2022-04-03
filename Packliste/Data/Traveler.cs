using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packliste.Data
{
    public class Traveler
    {
        public Person Person { get; set; }
        public ObservableCollection<ItemSet> itemSets { get; set; }

        public Traveler(Person person)
        {
            itemSets = new ObservableCollection<ItemSet>();
            Person = person;
        }
    }
}
