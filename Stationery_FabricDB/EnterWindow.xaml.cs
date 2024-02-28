using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Xml.Linq;

namespace Stationery_FabricDB
{
    /// <summary>
    /// Interaction logic for EnterWindow.xaml
    /// </summary>
    public partial class EnterWindow : Window
    {

        public string Value { get; private set; }
        public EnterWindow(string text = "")
        {
            InitializeComponent();

            txtName.Text = text;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            Value = txtName.Text;

            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Please enter object name!");
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

