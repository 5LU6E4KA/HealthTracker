using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing.Chart;

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

        public static void ExportToExcelPulse(List<Pulses> pulses)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"PulseChartOutput_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx"; // Добавляем временный штамп к имени файла
            string filePath = Path.Combine(downloadsPath, fileName);

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PulseData");

                // Добавляем заголовки
                worksheet.Cells[1, 1].Value = "Время измерения";
                worksheet.Cells[1, 2].Value = "Пульс";

                if (pulses.Any())
                {
                    // Добавляем данные
                    int row = 2;
                    foreach (var pulse in pulses)
                    {
                        worksheet.Cells[row, 1].Value = pulse.Time;
                        worksheet.Cells[row, 2].Value = pulse.PulseGraf;
                        row++;
                    }

                    // Устанавливаем формат для времени
                    worksheet.Column(1).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Устанавливаем выравнивание для данных
                    worksheet.Cells[2, 1, row - 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, row - 1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
               
                // Автоподбор ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохраняем изменения
                package.Save();
            }
            MessageBox.Show($"Файл сохранен в папке \"Загрузки\": {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ExportToExcelBloodPressure(List<Pressure> pressures)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"BloodPressureChartOutput_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx"; // Добавляем временный штамп к имени файла
            string filePath = Path.Combine(downloadsPath, fileName);

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("BloodPressureData");

                // Добавляем заголовки
                worksheet.Cells[1, 1].Value = "Время измерения";
                worksheet.Cells[1, 2].Value = "Сиастолический показатель";
                worksheet.Cells[1, 3].Value = "Диастолический показатель";

                if(pressures.Any())
                {
                    // Добавляем данные
                    int row = 2;
                    foreach (var pressure in pressures)
                    {
                        worksheet.Cells[row, 1].Value = pressure.PressureTime;
                        worksheet.Cells[row, 2].Value = pressure.Siastolic;
                        worksheet.Cells[row, 3].Value = pressure.Diastolic;
                        row++;
                    }

                    // Устанавливаем формат для времени
                    worksheet.Column(1).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    // Устанавливаем выравнивание для заголовков
                    worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Устанавливаем выравнивание для данных
                    worksheet.Cells[2, 1, row - 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, row - 1, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                

                // Автоподбор ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохраняем изменения
                package.Save();
            }
            MessageBox.Show($"Файл сохранен в папке \"Загрузки\": {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ExportToExcelTempeeature(List<Temperatures> temperatures)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string fileName = $"TemperatureChartOutput_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx"; // Добавляем временный штамп к имени файла
            string filePath = Path.Combine(downloadsPath, fileName);

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PulseData");

                // Добавляем заголовки
                worksheet.Cells[1, 1].Value = "Время измерения";
                worksheet.Cells[1, 2].Value = "Температура тела";

                if (temperatures.Any()) // Проверяем, есть ли данные
                {
                    // Добавляем данные
                    int row = 2;
                    foreach (var temperature in temperatures)
                    {
                        worksheet.Cells[row, 1].Value = temperature.TemperatureTime;
                        worksheet.Cells[row, 2].Value = temperature.Temperature;
                        row++;
                    }

                    // Устанавливаем формат для времени
                    worksheet.Column(1).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Устанавливаем выравнивание для данных
                    worksheet.Cells[2, 1, row - 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, row - 1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Автоподбор ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохраняем изменения
                package.Save();
            }
            MessageBox.Show($"Файл сохранен в папке \"Загрузки\": {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonExportToExcelPulse_Click(object sender, RoutedEventArgs e)
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

            ExportToExcelPulse(pulses);
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

        private void ButtonExportToExcelBloodPressure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentDate = DateTime.Now.Date;
                var info = DatabaseContext.DBContext.Context.BloodPressureInformations.ToList()
                    .Where(x => x.Users == _currentUser && x.MeasurementTimeBloodPressure > currentDate)
                    .OrderBy(x => x.MeasurementTimeBloodPressure);

                List<Pressure> pressures = new List<Pressure>();
                foreach (var pressureMeasurement in info)
                {
                    pressures.Add(new Pressure
                    {
                        PressureTime = pressureMeasurement.MeasurementTimeBloodPressure.Value,
                        Siastolic = (int)pressureMeasurement.SystolicPressure,
                        Diastolic = (int)pressureMeasurement.DiastolicPressure
                    });
                }

                // Вызываем функцию с передачей списка давлений
                ExportToExcelBloodPressure(pressures);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void ButtonExportToExcelTemperature_Click(object sender, RoutedEventArgs e)
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

            ExportToExcelTempeeature(temperatures);
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

                // Установка источника данных для каждой линии
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

            if (Convert.ToDecimal(TemperatureTextBox.Text) > 42 || Convert.ToDecimal(TemperatureTextBox.Text) < 29)
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
