using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;


namespace WPF_Prakt5_Gaitov_PR32_V4
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page, INotifyPropertyChanged
    {
        string _file = "receipt.json";
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(propName));
        }
        private ObservableCollection<Receipt> _receipts = new ObservableCollection<Receipt>();
        public ObservableCollection<Receipt> Receipts { get { return _receipts; } set { _receipts = value; OnPropertyChanged(); } }

        private Receipt _selectedReceipt;
        public Receipt SelectedReceipt { get { return _selectedReceipt; } set { _selectedReceipt = value; OnPropertyChanged(); } }

        public Page1()
        {
            Receipts.Add(new Receipt
            {
                Id = 1,
                Date = DateTime.Now,
                CashDeskNumber = 3,
                Cashier = "Иванов",
                Items = new ObservableCollection<Product_pos>
                {
                    new Product_pos { Name = "Молоко", Price = 89, Count = 2 },
                    new Product_pos { Name = "Хлеб", Price = 39, Count = 1 },
                    new Product_pos { Name = "Яйцо", Price = 9, Count = 180,}
                }
            });
            Receipts[0].RecalculateTotal();

            Receipts.Add(new Receipt
            {
                Id = 2,
                Date = DateTime.Now,
                CashDeskNumber = 4,
                Cashier = "Петр",
                Items = new ObservableCollection<Product_pos>
                {
                    new Product_pos { Name = "Яйцо", Price = 9, Count = 180,},
                    new Product_pos { Name = "Сыр", Price = 389, Count = 1 },
                    new Product_pos { Name = "Огурец", Price = 29, Count = 3 }
                }
            });
            Receipts[1].RecalculateTotal();

            Receipts.Add(new Receipt
            {
                Id = 3,
                Date = DateTime.Now,
                CashDeskNumber = 1,
                Cashier = "Дмитрий",
                Items = new ObservableCollection<Product_pos>
                {
                    new Product_pos { Name = "Молоко", Price = 89, Count = 2 },
                    new Product_pos { Name = "Хлеб", Price = 39, Count = 1 },
                    new Product_pos { Name = "Огурец", Price = 29, Count = 3 },
                    new Product_pos { Name = "Помидор",Price = 39, Count = 20 }
                }
            });
            Receipts[2].RecalculateTotal();

            Receipts.Add(new Receipt
            {
                Id = 4,
                Date = DateTime.Now,
                CashDeskNumber = 2,
                Cashier = "Федор",
                Items = new ObservableCollection<Product_pos>
                {
                    new Product_pos { Name = "Молоко", Price = 89, Count = 2 },
                    new Product_pos { Name = "Хлеб", Price = 39, Count = 1 },
                    new Product_pos { Name = "Огурец", Price = 29, Count = 3 },
                    new Product_pos { Name = "Помидор",Price = 39, Count = 20 }
                }
            });
            Receipts[3].RecalculateTotal();

            Receipts.Add(new Receipt
            {
                Id = 5,
                Date = DateTime.Now,
                CashDeskNumber = 5,
                Cashier ="Сергей",
                Items = new ObservableCollection<Product_pos>
                {
                    new Product_pos { Name = "Молоко", Price = 89, Count = 2 },
                    new Product_pos { Name = "Хлеб", Price = 39, Count = 1 },
                    new Product_pos { Name = "Яйцо", Price = 9, Count = 180,},
                    new Product_pos { Name = "Сыр", Price = 389, Count = 1 }
                }
            });
            Receipts[4].RecalculateTotal();

            Receipts.Add(new Receipt
            {
                Id = 6,
                Date = DateTime.Now,
                CashDeskNumber = 3,
                Cashier = "Александр",
                Items = new ObservableCollection<Product_pos>
                {
                    new Product_pos { Name = "Молоко", Price = 89, Count = 2 },
                    new Product_pos { Name = "Хлеб", Price = 39, Count = 1 },
                    new Product_pos { Name = "Яйцо", Price = 9, Count = 180,},
                    new Product_pos { Name = "Сыр", Price = 389, Count = 1 },
                    new Product_pos { Name = "Огурец", Price = 29, Count = 3 },
                    new Product_pos { Name = "Помидор",Price = 39, Count = 20 }
                }
            });
            Receipts[5].RecalculateTotal();

            InitializeComponent();
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditingPage(Receipts));
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReceipt == null)
            {
                MessageBox.Show("Не выбран элемент списка");
                return;
            }
            NavigationService.Navigate(new EditingPage(Receipts, SelectedReceipt));
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReceipt == null)
            {
                MessageBox.Show("Чек не выбран");
                return;
            }
            bool confirm = MessageBox.Show(
                "Вы действительно хотите удалить запись?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes;
            if (confirm)
            {
                Receipts.Remove(SelectedReceipt);
            }
        }

        private void SaveItems_CLick(object sender, RoutedEventArgs e)
        {
            try {
                string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, _file);

                string jsonString = JsonSerializer.Serialize(Receipts);

                File.WriteAllText(filePath, jsonString);

                MessageBox.Show("Данные успешно сохранены в файл");
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении файла", "Ошибка");
            }
        }
    }
}