using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для BMIPage.xaml
    /// </summary>
    public partial class BMIPage : Page
    {
        public BMIPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BMICalculation();
        }

        public void BMICalculation()
        {
            try
            {
                double weight = Convert.ToDouble(WeightTextBox.Text), height = Convert.ToDouble(HeightTextBox.Text);

                if (height > 3)
                {
                    MessageBox.Show("Рост не может быть больше 3 метров.", "BMI Калькулятор");
                    return; 
                }

                if (weight == 0 || height == 0)
                {
                    MessageBox.Show("Вес и рост не могут быть равны 0.", "BMI Калькулятор");
                    return; 
                }

                if (weight > 650)
                {
                    MessageBox.Show("Вес не может быть больше 650 кг.", "BMI Калькулятор");
                    return; 
                }

                double bmi = weight / Math.Pow(height, 2), roundedBmi = Math.Round(bmi, 1);

                ResultBMITextBlock.Text = $"Ваш индекс массы тела: {roundedBmi}";

                string generalization;

                if (roundedBmi >= 40)
                {
                    generalization = "Ожирение третьей степени";
                }
                else if (roundedBmi >= 35)
                {
                    generalization = "Ожирение второй степени";
                }
                else if (roundedBmi >= 30)
                {
                    generalization = "Ожирение первой степени";
                }
                else if (roundedBmi >= 25)
                {
                    generalization = "Лишний вес";
                }
                else if (roundedBmi >= 18.5)
                {
                    generalization = "Норма";
                }
                else
                {
                    generalization = "Дефицит массы тела";
                }

                GeneralizationBMITextBlock.Text = generalization;
            }
            catch (FormatException)
            {
                // Обработка ошибки преобразования пустой строки в double
                MessageBox.Show("Пожалуйста, введите числовые значения в поля.", "BMI Калькулятор");
            }
        }

        private bool IsNumeric(string text)
        {
            double result;
            return double.TryParse(text, out result);
        }

        private void HeightTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text) && e.Text != ",")
            {
                e.Handled = true; 
            }
        }

        private void WeightTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text) && e.Text != ",")
            {
                e.Handled = true;
            }
        }
    }
}
