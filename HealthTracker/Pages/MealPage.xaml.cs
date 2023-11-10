using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

namespace HealthTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для MealPage.xaml
    /// </summary>
    public partial class MealPage : Page
    {
        public List<string> Eating { get; set; }

        public MealPage()
        {
            InitializeComponent();
            Eating = eating;
            EatingComboBox.SelectedIndex = 0;
            DataContext = this;
        }

        private List<string> eating = new List<string>
        {
        "Не указано", "Завтрак", "Обед", "Ужин", "Перекус"
        };

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        public void Save()
        {

            if (string.IsNullOrWhiteSpace(CarloriesGainedTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните поле.");
                return;
            }

            if (!decimal.TryParse(SugarTextBox.Text, out decimal amountOfSugar) ||
                !decimal.TryParse(CarloriesGainedTextBox.Text, out decimal amountOfCalories) ||
                !decimal.TryParse(CarbohydratesTextBox.Text, out decimal amountOfCarbohydrates) ||
                !decimal.TryParse(FatsTextBox.Text, out decimal amountOfFat) ||
                !decimal.TryParse(ProteinTextBox.Text, out decimal amountOfProtein))
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.");
                return;
            }

            TimeSpan mealTime = DateTime.Now.TimeOfDay;

            DatabaseContext.DBContext.Context.FoodInformations.Add(new FoodInformations
            {
                AmountOfSugar = Convert.ToDecimal(SugarTextBox.Text),
                AmountOfCalories = Convert.ToDecimal(CarloriesGainedTextBox.Text),
                Intake = EatingComboBox.SelectedItem?.ToString(),
                FoodProduct = FoodProductTextBox.Text,
                AmountOfCarbohydrates = Convert.ToDecimal(CarbohydratesTextBox.Text),
                AmountOfFat = Convert.ToDecimal(FatsTextBox.Text),
                AmountOfProtein = Convert.ToDecimal(ProteinTextBox.Text),
                MealTime = mealTime
            });

            DatabaseContext.DBContext.Context.SaveChanges();
        }

    }
}
