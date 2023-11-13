using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



namespace HealthTracker.Pages
{
    
    public class Cal
    {
        public string Day { get; set; }
        public double Calories { get; set; }
    }

    public partial class MealPage : Page
    {
        public List<string> Eating { get; set; }
        public List<string> FluidType { get; set; }
        public List<string> DayOfWeeks { get; set; }
        
        public MealPage()
        {
            InitializeComponent();
            Eating = eating;
            EatingComboBox.SelectedIndex = 0;
            FluidType = fluidType;
            FluidTypeComboBox.SelectedIndex = 0;
            ChartUpdate();
            DataContext = this;
        }

        private void ChartUpdate()
        {
            var info = DatabaseContext.DBContext.Context.FoodInformations.ToList().Where(x => x.MealTime > GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Monday));

            List<Cal> cals = new List<Cal>();
            foreach (var item in dayOfWeeks)
            {
                var cal = info.Where(x => x.MealTime.Value.DayOfWeek == item.Key);
                cals.Add(new Cal { Day = item.Value, Calories = cal.Count() == 0 ? 0 : (double)cal.Sum(x => x.AmountOfCalories) });
            }
            ColGraficCalroies.ItemsSource = cals;
        }

        private DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        private List<string> fluidType = new List<string>
        {
            "Не указано", "Вода", "Сок", "Молочный напиток", "Газированный напиток", "Чай", "Кофе"
        };

        private List<string> eating = new List<string>
        {
            "Не указано", "Завтрак", "Обед", "Ужин", "Перекус"
        };

        private Dictionary<DayOfWeek, string> dayOfWeeks = new Dictionary<DayOfWeek, string>
        {
            {DayOfWeek.Monday, "Пн" },
            {DayOfWeek.Tuesday, "Вт" },
            {DayOfWeek.Wednesday, "Ср" },
            {DayOfWeek.Thursday, "Чт" },
            {DayOfWeek.Friday, "Пт" },
            {DayOfWeek.Saturday, "Сб" },
            {DayOfWeek.Sunday, "Вс" }

        };

        private void ButtonSaveCalories_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
                ChartUpdate();
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
                MessageBox.Show("Пожалуйста, введите количество съеденных калорий");
                return;
            }

            DateTime mealTime = DateTime.Now;

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
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Если введенный символ не цифра и не запятая, отменяем ввод
            if (!char.IsDigit(e.Text, 0) && e.Text != ",")
            {
                e.Handled = true;
            }

            // Если введенная запятая уже присутствует в тексте, отменяем ввод
            if (e.Text == "," && textBox.Text.Contains(","))
            {
                e.Handled = true;
            }

            if (e.Text == "," && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "0,";
                textBox.CaretIndex = 2; // установка курсора после "0"
                e.Handled = true;
            }
        }

        private bool IsNumericInput(string text)
        {
            return text.All(char.IsDigit) || text == ",";
        }

        private void LettersOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!IsLetterInput(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsLetterInput(string text)
        {
            return text.All(char.IsLetter);
        }

    }
}
