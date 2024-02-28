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
using System.Windows.Shapes;

namespace Stationery_FabricDB
{
    /// <summary>
    /// Interaction logic for DataRangeInput.xaml
    /// </summary>
    public partial class DataRangeInput : Window
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public DataRangeInput()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (startDatePicker.SelectedDate.HasValue && endDatePicker.SelectedDate.HasValue)
            {
                StartDate = startDatePicker.SelectedDate.Value.Date;
                EndDate = endDatePicker.SelectedDate.Value.Date.Add(new TimeSpan(23, 59, 59)); 
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select both start and end dates.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
