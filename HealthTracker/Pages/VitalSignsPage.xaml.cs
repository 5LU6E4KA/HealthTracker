using HealthTracker.Entities;
using Syncfusion.UI.Xaml.Charts;
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

    public class Pressure
    {
        public DateTime PressureTime { get; set; }
        public int Siastolic { get; set; }
        public int Diastolic { get; set; }
    }



    public partial class VitalSignsPage : Page
    {
        public List<string> Place { get; set; }
        public List<string> Position { get; set; }

        private List<string> _place = new List<string>
        {
            "Не указано", "Подмышка", "Лоб", "Запястье", "Височная артерия"
        };

        private List<string> _position = new List<string>
        {
            "Не указано", "Положение стоя", "Положение лежа", "Положение сидя"
        };

        private Users _currentUser;
        public VitalSignsPage(Users user)
        {
            InitializeComponent();
            Place = _place;
            Position = _position;
            BodyPositionComboBox.SelectedIndex = 0;
            PlaceComboBox.SelectedIndex = 0;
            _currentUser = user;
            ChartUpdatePulse();
            ChartUpdateTemperature();
            ChartUpdateBloodPressure();
            DataContext = this;
        }

        private void ChartUpdatePulse()
        {
            var currentDate = DateTime.Now.Date;
            var info = DatabaseContext.DBContext.Context.PulseInformations.ToList().Where(x => x.Users == _currentUser && x.MeasurementTimePulse > currentDate);
            List<Pulses> pulses = new List<Pulses>();
            foreach (var pulseMeasurement in info)
            {
                pulses.Add(new Pulses
                {
                    Time = pulseMeasurement.MeasurementTimePulse.Value,
                    PulseGraf = (double)pulseMeasurement.Pulse
                });
            }

            LineGraficPulse.ItemsSource = pulses;
        }

        private void ChartUpdateTemperature()
        {
            var currentDate = DateTime.Now.Date;
            var info = DatabaseContext.DBContext.Context.TemperatureInformations
                .ToList()
                .Where(x => x.Users == _currentUser && x.MeasurementTimeTemperature > currentDate);

            List<Temperatures> temperatures = new List<Temperatures>();
            foreach (var temperatureMeasurement in info)
            {
                temperatures.Add(new Temperatures
                {
                    TemperatureTime = temperatureMeasurement.MeasurementTimeTemperature.Value,
                    Temperature = (double)temperatureMeasurement.BodyTemperature
                });
            }

            LineGraficTemperature.ItemsSource = temperatures;
        }

        private void ChartUpdateBloodPressure()
        {
            try
            {
                var currentDate = DateTime.Now.Date;
                var info = DatabaseContext.DBContext.Context.BloodPressureInformations.ToList()
                    .Where(x => x.Users == _currentUser && x.MeasurementTimeBloodPressure > currentDate)
                    .OrderBy(x => x.MeasurementTimeBloodPressure);

                List<Pressure> diastolicPressures = new List<Pressure>();
                List<Pressure> systolicPressures = new List<Pressure>();
                foreach (var pressureMeasurement in info)
                {
                    diastolicPressures.Add(new Pressure
                    {
                        PressureTime = pressureMeasurement.MeasurementTimeBloodPressure.Value,
                        Diastolic = (int)pressureMeasurement.DiastolicPressure
                    });

                    systolicPressures.Add(new Pressure
                    {
                        PressureTime = pressureMeasurement.MeasurementTimeBloodPressure.Value,
                        Siastolic = (int)pressureMeasurement.SystolicPressure
                    });
                }

                LineGraficDiastolic.ItemsSource = diastolicPressures;
                LineGraficSiastolic.ItemsSource = systolicPressures;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
            
        }

        

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

        public void SaveBloodPressure()
        {

            if (string.IsNullOrWhiteSpace(DiastolickPressureTextBox.Text) || string.IsNullOrWhiteSpace(SystolickPressureTextBox.Text))
            {
                MessageBox.Show("Один из параметров артериального давления не заполнен");
                return;
            }

            if (Convert.ToInt32(SystolickPressureTextBox.Text) > 240 || Convert.ToInt32(SystolickPressureTextBox.Text) < 80)
            {
                MessageBox.Show("Человек не может иметь такое сиастолическое давление");
                return;
            }

            if (Convert.ToInt32(DiastolickPressureTextBox.Text) > 90 || Convert.ToInt32(DiastolickPressureTextBox.Text) < 55)
            {
                MessageBox.Show("Человек не может иметь такое диастолическое давление");
                return;
            }

            DatabaseContext.DBContext.Context.BloodPressureInformations.Add(new BloodPressureInformations
            {
                UserID = _currentUser.UserID,
                DiastolicPressure = Convert.ToInt32(DiastolickPressureTextBox.Text),
                SystolicPressure = Convert.ToInt32(SystolickPressureTextBox.Text),
                BodyPosition = BodyPositionComboBox.SelectedItem?.ToString(),
                MeasurementTimeBloodPressure = DateTime.Now
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
            try
            {
                SaveBloodPressure();
                ChartUpdateBloodPressure();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        
    }
}
