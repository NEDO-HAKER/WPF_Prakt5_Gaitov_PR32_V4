using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Prakt5_Gaitov_PR32_V4
{
    public class Product_pos : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(propName));
        }

        private string name;
        private decimal price;
        private int count;
        private decimal itemTotal;

        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        public decimal Price { get { return price; } set { price = value; OnPropertyChanged(); RecalculateTotal(); } }
        public int Count { get { return count; } set { count = value; OnPropertyChanged(); RecalculateTotal(); } }
        public decimal ItemTotal { get { return itemTotal; } set { itemTotal = value; OnPropertyChanged(); } }

        private void RecalculateTotal()
        {
            ItemTotal = Price * Count;
        }
    }
}
