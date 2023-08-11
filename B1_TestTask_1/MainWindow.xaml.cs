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

namespace B1_TestTask_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FormMergingFiles taskWindow = new FormMergingFiles();
            taskWindow.Show();
        }

        private void Button_Click_Import(object sender, RoutedEventArgs e)
        {
            DataBaseImport importWindow = new DataBaseImport();
            importWindow.Show();
        }

    }
}
