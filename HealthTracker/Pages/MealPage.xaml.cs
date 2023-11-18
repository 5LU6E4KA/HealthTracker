using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static HealthTracker.DateClass.DateInfo;



namespace HealthTracker.Pages
{
    
    public class Calorie
    {
        public string Day { get; set; }
        public double Calories { get; set; }
    }

    public class Water
    {
        public string Day { get; set; }
        public double Liquid { get; set; }
    }

    public partial class MealPage : Page
    {
        private Users _currentUser;
        public List<string> Eating { get; set; }
        public List<string> FluidType { get; set; }
        public List<string> DayOfWeeks { get; set; }
        private DateTime _currentMealChartDate = DateTime.Now;
        private DateTime _currentWaterChartDate = DateTime.Now;

        public MealPage(Users user)
        {
            _currentUser = user;
            InitializeComponent();
            Eating = eating;
            EatingComboBox.SelectedIndex = 0;
            FluidType = fluidType;
            FluidTypeComboBox.SelectedIndex = 0;
            ChartUpdateMeal(DateTime.Now);
            ChartUpdateWater(DateTime.Now);
            DataContext = this;
        }

        private void ChartUpdateMeal(DateTime dateTime)
        {
            _currentMealChartDate = dateTime;
            StartDateMealTextBlock.Text = GetFirstDateOfWeek(dateTime, DayOfWeek.Monday).ToString("dd.MM.yyyy");
            FinishDateMealTextBlock.Text = GetFirstDateOfWeek(dateTime.AddDays(7), DayOfWeek.Monday).
                AddDays(-1).ToString("dd.MM.yyyy");
            var info = DatabaseContext.DBContext.Context.FoodInformations.ToList().
                Where(x => x.Users == _currentUser && x.MealTime > GetFirstDateOfWeek(dateTime, DayOfWeek.Monday) && 
                x.MealTime < GetFirstDateOfWeek(dateTime.AddDays(7), DayOfWeek.Monday));

            List<Calorie> calories = new List<Calorie>();
            foreach (var item in dayOfWeeks)
            {
                var calorie = info.Where(x => x.MealTime.Value.DayOfWeek == item.Key);
                calories.Add(new Calorie { Day = item.Value, Calories = calorie.Count() == 0 ? 0 : (double)calorie.Sum(x => x.AmountOfCalories) });
            }
            ColGraficCalroies.ItemsSource = calories;
        }

        private void ChartUpdateWater(DateTime dateTime)
        {
            _currentWaterChartDate = dateTime;
            StartDateWaterTextBlock.Text = GetFirstDateOfWeek(dateTime, DayOfWeek.Monday).ToString("dd.MM.yyyy");
            FinishDateWaterTextBlock.Text = GetFirstDateOfWeek(dateTime.AddDays(7), DayOfWeek.Monday).
                AddDays(-1).ToString("dd.MM.yyyy");
            var info = DatabaseContext.DBContext.Context.WaterInformations.ToList().
                Where(x => x.Users == _currentUser && x.DrinkingTime > GetFirstDateOfWeek(dateTime, DayOfWeek.Monday) &&
                x.DrinkingTime < GetFirstDateOfWeek(dateTime.AddDays(7), DayOfWeek.Monday));

            List<Water> waters = new List<Water>();
            foreach (var item in dayOfWeeks)
            {
                var water = info.Where(x => x.DrinkingTime.Value.DayOfWeek == item.Key);
                waters.Add(new Water { Day = item.Value, Liquid = water.Count() == 0 ? 0 : (double)water.Sum(x => x.LiquidLevel) });
            }
            ColGraficWater.ItemsSource = waters;
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
                SaveMeal();
                ChartUpdateMeal(DateTime.Now);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        public void SaveMeal()
        {

            if (string.IsNullOrWhiteSpace(CarloriesGainedTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите количество съеденных калорий");
                return;
            }

            DateTime mealTime = DateTime.Now;

            DatabaseContext.DBContext.Context.FoodInformations.Add(new FoodInformations
            {
                UserID = _currentUser.UserID,
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
        public void SaveWater()
        {

            if (string.IsNullOrWhiteSpace(WaterLevelTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите количество выпитой жидкости");
                return;
            }

            DateTime waterTime = DateTime.Now;

            DatabaseContext.DBContext.Context.WaterInformations.Add(new WaterInformations
            {
                UserID = _currentUser.UserID,
                LiquidLevel = Convert.ToDecimal(WaterLevelTextBox.Text),
                LiquidType = FluidTypeComboBox.SelectedItem?.ToString(),
                DrinkingTime = waterTime
            }); 

            DatabaseContext.DBContext.Context.SaveChanges();
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            
            if (!char.IsDigit(e.Text, 0) && e.Text != ",")
            {
                e.Handled = true;
            }

           
            if (e.Text == "," && textBox.Text.Contains(","))
            {
                e.Handled = true;
            }

            if (e.Text == "," && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "0,";
                textBox.CaretIndex = 2; 
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

        private void ButtonSaveWater_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SaveWater();
                ChartUpdateWater(DateTime.Now);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        private void ButtonLeftCalories_Click(object sender, RoutedEventArgs e)
        {
             _currentMealChartDate = _currentMealChartDate.AddDays(-7);
            ChartUpdateMeal(_currentMealChartDate);
        }

        private void ButtonRightCalories_Click(object sender, RoutedEventArgs e)
        {
            _currentMealChartDate = _currentMealChartDate.AddDays(7);
            ChartUpdateMeal(_currentMealChartDate);
        }

        private void ButtonThisWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentMealChartDate = DateTime.Now;
            ChartUpdateMeal(_currentMealChartDate);
        }

        private void ButtonLeftWater_Click(object sender, RoutedEventArgs e)
        {
            _currentWaterChartDate = _currentWaterChartDate.AddDays(-7);
            ChartUpdateWater(_currentWaterChartDate);
        }

        private void ButtonRightWater_Click(object sender, RoutedEventArgs e)
        {
            _currentWaterChartDate = _currentWaterChartDate.AddDays(7);
            ChartUpdateWater(_currentWaterChartDate);
        }

        private void ButtonThisWeekWater_Click(object sender, RoutedEventArgs e)
        {
            _currentWaterChartDate = DateTime.Now;
            ChartUpdateWater(_currentWaterChartDate);
        }
    }
}
