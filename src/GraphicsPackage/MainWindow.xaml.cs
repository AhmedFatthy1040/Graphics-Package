using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicsPackage;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string x1 = "";
    private string y1 = "";
    public MainWindow()
    {
        InitializeComponent();
    }

    private void X1_TextChanged(object sender, TextChangedEventArgs e)
    {
        x1 = X1.Text;
    }

    private void DrawLineDDA_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("The text entered is: " + x1);
    }

    private void Y1_TextChanged(object sender, TextChangedEventArgs e)
    {
        y1 = Y1.Text;
    }

    private void DrawLine_Bresenham_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("The text entered is: " + y1);
    }
}