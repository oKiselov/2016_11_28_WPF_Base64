using System;
using System.CodeDom;
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

namespace Kiselov_HW_WPF_Base64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonConvert_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxConvertFrom.Text) ||
                    (RadioButtonTo.IsChecked == false && RadioButtonFrom.IsChecked == false))
                {
                    throw new Exception("Please insert text and choose the conversion type!");
                }

                if (RadioButtonTo.IsChecked == true)
                {
                    TextBoxConvertTo.Text =
                        Controller.ConvertFromTextToBase64(TextBoxConvertFrom.Text);
                }
                if (RadioButtonFrom.IsChecked == true)
                {
                    TextBoxConvertTo.Text =
                        Controller.ConvertFromBase64ToText(TextBoxConvertFrom.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
