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
    public class Receipt : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(propName));
        }

        private int id;
        private DateTime date;
        private int cashDeskNumber;
        private string cashier;
        private ObservableCollection<Product_pos> items;
        private decimal receiptTotal;

        public int Id { get { return id; } set { id = value; OnPropertyChanged(); } }
        public DateTime Date { get { return date; } set { date = value; OnPropertyChanged(); } }
        public int CashDeskNumber { get { return cashDeskNumber; } set { cashDeskNumber = value; OnPropertyChanged(); } }
        public string Cashier { get { return cashier; } set { cashier = value; OnPropertyChanged(); } }
        public ObservableCollection<Product_pos> Items { get { return items; } set { items = value; OnPropertyChanged(); RecalculateTotal(); } }
        public decimal ReceiptTotal { get { return receiptTotal; } set { receiptTotal = value; OnPropertyChanged(); } }

        public void RecalculateTotal()
        {
            if (Items != null) ReceiptTotal = Items.Sum(item => item.ItemTotal);
            else ReceiptTotal = 0;
        }
    }
}
