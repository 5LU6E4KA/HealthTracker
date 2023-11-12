using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;


namespace HealthTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для MealPage.xaml
    /// </summary>
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
           
            DataContext = this;
        }



        //private void UpdateChart()
        //{
        //    var info = DatabaseContext.DBContext.Context.FoodInformations.ToList()
        //      .Where(x => x.MealTime > GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Monday));


        //    Series currentSeries = ChartCalories.Series.FirstOrDefault();
        //    currentSeries.ChartType = SeriesChartType.Column;
        //    currentSeries.Points.Clear();

        //    foreach (var category in dayOfWeeks)
        //    {
        //        var calories = info.Where(x => x.MealTime.Value.DayOfWeek == category.Key);
        //        var cal = calories.Count() == 0 ? 0 : calories.First().AmountOfCalories;
        //        currentSeries.Points.AddXY(category.Value, cal);
        //    }
        //    ChartCalories.Update();
        //}

        //private void UpdateDate()
        //{
        //    PlotModel plotModel = new PlotModel();
        //    CategoryAxis categoryAxis = new CategoryAxis();
        //    BarSeries series = new BarSeries();
        //    var items = new List<BarItem>();
        //    var info = DatabaseContext.DBContext.Context.FoodInformations
        //        .Where(x => x.MealTime > GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Monday))
        //        .OrderBy(x => x.MealTime);

        //    foreach (var item in dayOfWeeks.OrderBy(x => (int)x.Key))
        //    {
        //        categoryAxis.Labels.Add(item.Value);
        //    }

        //    foreach (var item in info)
        //    {
        //        int day = (int)item.MealTime.Value.DayOfWeek + 1;
        //        items.Add(new BarItem((double)item.AmountOfCalories));
        //    }

        //    series.ItemsSource = items;

        //    var valueAxis = new LinearAxis
        //    {
        //        Position = AxisPosition.Left,
        //        MinimumPadding = 0,
        //        MaximumPadding = 0.06,
        //        AbsoluteMinimum = 0,
        //        Maximum = info.Max(x => (double)x.AmountOfCalories)
        //    };

        //    plotModel.Series.Add(series);
        //    plotModel.Axes.Add(categoryAxis);
        //    plotModel.Axes.Add(valueAxis);
        //    Grafic.Model = plotModel;

        //}

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
