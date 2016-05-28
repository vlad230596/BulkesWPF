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

namespace Bulkes
{
    /// <summary>
    /// Interaction logic for EndGameDialog.xaml
    /// </summary>
    public partial class EndGameDialog : Window
    {
        public EndGameDialog()
        {
            InitializeComponent();
        }

        private void exitButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
