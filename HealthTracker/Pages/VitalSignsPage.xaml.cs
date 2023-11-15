using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using static HealthTracker.DateClass.DateInfo;

namespace HealthTracker.Pages
{
    
    public class Pulses
    {
        public DateTime Time { get; set; }
        public double PulseGraf { get; set; }
    }

    public class Temperatures
    {
        public DateTime TemperatureTime { get; set; }
        public double Temperature { get; set; }
    }


    public partial class VitalSignsPage : Page
    {
        public List<string> Place { get; set; }

        private List<string> place = new List<string>
        {
            "Не указано", "Подмышка", "Лоб", "Запястье", "Височная артерия"
        };

        private Users _currentUser;
        public VitalSignsPage(Users user)
        {
            InitializeComponent();
            Place = place;
            PlaceComboBox.SelectedIndex = 0;
            _currentUser = user;
            ChartUpdatePulse();
            ChartUpdateTemperature();
            DataContext = this;
        }

        private void ChartUpdatePulse()
        {
            var info = DatabaseContext.DBContext.Context.PulseInformations.ToList()
            .Where(x => x.Users == _currentUser && x.MeasurementTimePulse > GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Monday));

            List<Pulses> pulses = new List<Pulses>();
            foreach (var item in dayOfWeeks)
            {
                var pulseData = info.Where(x => x.MeasurementTimePulse.Value.DayOfWeek == item.Key);

                foreach (var pulseMeasurement in pulseData)
                {
                    pulses.Add(new Pulses
                    {
                        Time = pulseMeasurement.MeasurementTimePulse.Value,
                        PulseGraf = (double)pulseMeasurement.Pulse
                    });
                }
            }

            LineGraficPulse.ItemsSource = pulses;
        }

        private void ChartUpdateTemperature()
        {
            var info = DatabaseContext.DBContext.Context.TemperatureInformations.ToList()
            .Where(x => x.Users == _currentUser && x.MeasurementTimeTemperature > GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Monday));

            List<Temperatures> temperatures = new List<Temperatures>();
            foreach (var item in dayOfWeeks)
            {
                var temperaturesData = info.Where(x => x.MeasurementTimeTemperature.Value.DayOfWeek == item.Key);

                foreach (var elem in temperaturesData)
                {
                    temperatures.Add(new Temperatures
                    {
                        TemperatureTime = elem.MeasurementTimeTemperature.Value,
                        Temperature = (double)elem.BodyTemperature
                    });
                }
            }

            LineGraficTemperature.ItemsSource = temperatures;
        }
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

        public void SavePulse()
        {

            if (string.IsNullOrWhiteSpace(PulseTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите пульс");
                return;
            }

            DatabaseContext.DBContext.Context.PulseInformations.Add(new PulseInformations
            {
                UserID = _currentUser.UserID,
                Pulse = Convert.ToInt32(PulseTextBox.Text),
                MeasurementTimePulse = DateTime.Now
            });

            DatabaseContext.DBContext.Context.SaveChanges();
        }

        public void SaveTemperature()
        {

            if (string.IsNullOrWhiteSpace(TemperatureTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите температуру тела");
                return;
            }

            if(Convert.ToDecimal(TemperatureTextBox.Text) > 42 || Convert.ToDecimal(TemperatureTextBox.Text) < 29)
            {
                MessageBox.Show("Человек не может иметь такую температуру");
                return;
            }

            DatabaseContext.DBContext.Context.TemperatureInformations.Add(new TemperatureInformations
            {
                UserID = _currentUser.UserID,
                BodyTemperature = Convert.ToDecimal(TemperatureTextBox.Text),
                MeasurementTimeTemperature = DateTime.Now,
                MeasurementPlace = PlaceComboBox.SelectedItem?.ToString()
            });

            DatabaseContext.DBContext.Context.SaveChanges();
        }

        private void ButtonSavePulse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SavePulse();
                ChartUpdatePulse();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        private void ButtonSaveTemperature_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveTemperature();
                ChartUpdateTemperature();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        private void ButtonSavePressure_Click(object sender, RoutedEventArgs e)
        {
            
        }

        
    }
}
