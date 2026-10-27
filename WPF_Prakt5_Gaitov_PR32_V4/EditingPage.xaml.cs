using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Prakt5_Gaitov_PR32_V4
{
    public partial class EditingPage : Page, INotifyPropertyChanged
    {
        string[] A = { "Молоко", "Хлеб", "Яйцо", "Сыр", "Огурец", "Помидор" };
        Random rnd = new Random();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(propName));
        }

        private readonly ObservableCollection<Receipt> _receiptList;
        private readonly Receipt _originalReceipt;

        private Receipt _receipt;
        public Receipt Receipt { get { return _receipt; } set { _receipt = value; OnPropertyChanged(); } }

        private Product_pos _selectedItem;
        public Product_pos SelectedItem { get { return _selectedItem; } set { _selectedItem = value; OnPropertyChanged(); } }

        public EditingPage(ObservableCollection<Receipt> receiptList, Receipt receipt = null)
        {
            InitializeComponent();
            _receiptList = receiptList;
            _originalReceipt = receipt;

            if (_originalReceipt == null)
            {
                _receipt = new Receipt
                {
                    Id = 0,
                    Date = DateTime.Now,
                    CashDeskNumber = 1,
                    Cashier = "Иван",
                    Items = new ObservableCollection<Product_pos>()
                };
            }
            else
            {
                _receipt = new Receipt
                {
                    Id = _originalReceipt.Id,
                    Date = _originalReceipt.Date,
                    CashDeskNumber = _originalReceipt.CashDeskNumber,
                    Cashier = _originalReceipt.Cashier,
                    Items = new ObservableCollection<Product_pos>(
                        _originalReceipt.Items.Select(i => new Product_pos
                        {
                            Name = i.Name,
                            Price = i.Price,
                            Count = i.Count
                        })
                    )
                };
                _receipt.RecalculateTotal();
            }

            DataContext = this;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _receipt.RecalculateTotal();

            if (_originalReceipt == null)
            {
                _receiptList.Add(_receipt);
            }
            else
            {
                _originalReceipt.Date = _receipt.Date;
                _originalReceipt.CashDeskNumber = _receipt.CashDeskNumber;
                _originalReceipt.Cashier = _receipt.Cashier;

                _originalReceipt.Items.Clear();
                foreach (var item in _receipt.Items)
                {
                    _originalReceipt.Items.Add(item);
                }
                _originalReceipt.RecalculateTotal();
            }

            NavigationService.GoBack();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            int i = rnd.Next(0, 5);
            int price = rnd.Next(9, 100);
            int count = rnd.Next(1, 11);

            _receipt.Items.Add(new Product_pos { Name = A[i], Price = price, Count = count });
            _receipt.RecalculateTotal();
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберите товар в списке для удаления.", "Ошибка");
                return;
            }

            bool confirm = MessageBox.Show(
                "Вы действительно хотите удалить товар?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
            if (confirm)
            {
                _receipt.Items.Remove(SelectedItem);
                SelectedItem = null;
                _receipt.RecalculateTotal();
            }
        }
    }
}