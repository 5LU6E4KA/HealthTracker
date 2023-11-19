using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace HealthTracker.ClearFields
{
    static class ClearField
    {
        public static void ClearTextBoxes(DependencyObject depObj)
        {
            if (depObj == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child is TextBox textBox)
                {
                    textBox.Clear();
                }

                ClearTextBoxes(child);
            }
        }
    }
}
