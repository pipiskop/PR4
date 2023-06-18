using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PR4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> Types = new List<string>();
        private List<Money> Notes = new List<Money>();
        private List<Money> CurrentNote = new List<Money>();
        private int Money;
        private int Total;

        public MainWindow()
        {
            InitializeComponent();
            Data.SelectedDate = DateTime.Today;
            var desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Notes = SerializationAndDeserialization.Serialization<List<Money>>(System.IO.Path.Combine(desktopFolderPath, "Notes.json")) ?? new List<Money>();
            Types = SerializationAndDeserialization.Serialization<List<string>>(System.IO.Path.Combine(desktopFolderPath, "TypeName.json")) ?? new List<string>();
            foreach (var Type in Types.Where(Type => !string.IsNullOrEmpty(Type)))
            {
                TypeName.Items.Add(Type);
            }

            int sum = 0;
            int summ = 0;
            foreach (var Budget in Notes)
            {
                if (Budget.IsIncome == false)
                {
                    summ -= Budget.Ammount;
                }
                else
                {
                    sum += Budget.Ammount;
                }
            }
            Total = sum + summ;
            SummText.Content = Total;
        }

        private void AddTypeClick(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            if (window.ShowDialog() == true)
            {
                var newItem = window.VvodTexta;
                TypeName.Items.Add(newItem);
                Types.Add(newItem);
                TypeName.SelectedItem = newItem;
            }
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            AddBudget();
        }

        private void AddBudget()
        {
            Money = Convert.ToInt32(Summ.Text);
            if (!string.IsNullOrEmpty(TypeName.SelectedItem?.ToString()))
            {
                Total += Money;
                SummText.Content = Total;
                Money budgetValue = new Money(Data.SelectedDate.Value, Name.Text, TypeName.SelectedItem.ToString(), Money,
                    Money >= 0);
                Notes.Add(budgetValue);
                TodayNotes();
            }
        }

        private void Data_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TodayNotes();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SerializationAndDeserialization.Deserialization(Notes, System.IO.Path.Combine(desktopFolderPath, "Notes.json"));
            SerializationAndDeserialization.Deserialization(Types, System.IO.Path.Combine(desktopFolderPath, "TypeName.json"));
        }

        private void TodayNotes()
        {
            CurrentNote.Clear();
            foreach (var Budget in Notes)
            {
                if (Budget.CurrentDate == Data.SelectedDate)
                {
                    CurrentNote.Add(Budget);
                }
            }
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = CurrentNote;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedIndex >= 0)
            {
                Name.Text = CurrentNote[DataGrid.SelectedIndex].Name;
                TypeName.SelectedItem = CurrentNote[DataGrid.SelectedIndex].Type;
                Summ.Text = CurrentNote[DataGrid.SelectedIndex].Ammount.ToString();
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteMoney();
        }
        private void DeleteMoney()
        {
            if (DataGrid.SelectedItem is Money budget)
            {
                Total -= budget.Ammount;
                SummText.Content = Total;
                Name.Text = null;
                TypeName.SelectedItem = null;
                Summ.Text = null;
                Notes.Remove(budget);
                SerializationAndDeserialization.Deserialization(Notes, System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    @"Notes.json"));
                TodayNotes();
            }
            else
            {
                return;
            }
        }

        private void EditBut(object sender, RoutedEventArgs e)
        {
            int deleteBudget = DataGrid.SelectedIndex;
            AddBudget();
            DataGrid.SelectedIndex = deleteBudget;
            DeleteMoney();
        }
    }
}
