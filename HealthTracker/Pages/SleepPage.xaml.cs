using HealthTracker.Entities;
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
using static HealthTracker.DateClass.DateInfo;
using static HealthTracker.Pages.SleepPage;

namespace HealthTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для SleepPage.xaml
    /// </summary>
    public partial class SleepPage : Page
    {
        public class Sleeping
        {
            public string Day { get; set; }
            public int SleepTime { get; set; }
        }
        private Users _currentUser;
        public SleepPage(Users user)
        {
            InitializeComponent();
            _currentUser = user;
            BedTimePicker.DefaultValue = DateTime.Now;
            WakeUpTimePicker.DefaultValue = DateTime.Now;
            ChartUpdateSleep();
        }

        private void ButtonSaveSleep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveSleep();
                ChartUpdateSleep();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        private int GetDaySleepTime(DateTime dateTime)
        {
            var info = DatabaseContext.DBContext.Context.SleepInformations.ToList().Where(x => x.Users == _currentUser && x.SleepTime.Value.Date == dateTime.Date);
           
            var sleeping = new TimeSpan(0);
            foreach (var elem in info)
            {
                sleeping += elem.WakeUpTime - elem.BedTime;
            }
            return (int)sleeping.TotalHours;
        }

        public void SaveSleep()
        {
            var wakeUp = WakeUpTimePicker.Value;
            var bedTime = BedTimePicker.Value;
            if (wakeUp is null || bedTime is null )
            {
                MessageBox.Show("Пожалуйста, введите время");
                return;
            }

            if(bedTime > wakeUp)
            {
                MessageBox.Show("Пожалуйста, введите корректное значение времени");
                return;
            }
            
            if(GetDaySleepTime(DateTime.Now) + (wakeUp - bedTime).Value.TotalHours > 24)
            {
                MessageBox.Show("Вы не можете спать больше 24 часов в день");
                return;
            }

            if(bedTime > DateTime.Now || wakeUp > DateTime.Now)
            {
                MessageBox.Show("Вы не можете лечь спать или проснуться в будущем");
                return;
            }
            

            DatabaseContext.DBContext.Context.SleepInformations.Add(new SleepInformations
            {
                UserID = _currentUser.UserID,
                SleepTime = DateTime.Now,
                BedTime = (DateTime)bedTime,
                WakeUpTime = (DateTime)wakeUp
               
            });

            DatabaseContext.DBContext.Context.SaveChanges();
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

        private void ChartUpdateSleep()
        {
            var info = DatabaseContext.DBContext.Context.SleepInformations.ToList().Where(x => x.Users == _currentUser && x.SleepTime > GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Monday));

            List<Sleeping> sleepings = new List<Sleeping>();
            foreach (var item in dayOfWeeks)
            {
                var sleep = info.Where(x => x.SleepTime.Value.DayOfWeek == item.Key);
                
                
                sleepings.Add(new Sleeping { Day = item.Value, SleepTime = sleep.Count() == 0 ? 0 : GetDaySleepTime(sleep.First().SleepTime.Value) });
            }
            ColGraficSleeps.ItemsSource = sleepings;
        }
    }
    
}
