using Packliste.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Packliste.Data
{
    public class Traveler: INotifyPropertyChanged
    {
        public Person Person { get; set; }

        private int _TotalWeight;

        public int TotalWeight
        {
            get { return _TotalWeight; }
            set
            { 
                _TotalWeight = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollectionWithItemNotify<ItemSet> itemSets { get; set; }

        public Traveler(Person person)
        {
            itemSets = new ObservableCollectionWithItemNotify<ItemSet>();
            itemSets.CollectionChanged += ItemSets_CollectionChanged;
            Person = person;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ItemSets_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var WeightSum = 0;
            foreach (var itemSet in itemSets)
            {
                WeightSum += itemSet.Count * itemSet.Item.Weight;
            }
            TotalWeight = WeightSum;
        }
    }
}
