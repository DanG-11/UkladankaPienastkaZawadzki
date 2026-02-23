using System.Data.Common;
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

namespace UkładankaPienastkaZawadzki
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

        private (int Row, int Col) WhereIsEmptySpace()
        {
            int rows = MainGrid.RowDefinitions.Count;
            int cols = MainGrid.ColumnDefinitions.Count;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    bool occupied = false;
                    foreach (Button button in MainGrid.Children)
                    {
                        if (Grid.GetRow(button) == r && Grid.GetColumn(button) == c)
                        {
                            occupied = true;
                            break;
                        }
                    }

                    if (!occupied)
                    {
                        return (r, c);
                    }
                }
            }

            return (-1, -1);
        }

        private void CheckForFreePlace_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            var emptySpace  = WhereIsEmptySpace();

            if (emptySpace.Row == row)
            {
                int emptyToBtnDiff = System.Math.Abs(emptySpace.Col - col);
                if (emptyToBtnDiff == 1)
                {
                    Grid.SetColumn(button, emptySpace.Col);
                }
                return;
            }

            if (emptySpace.Col == col)
            {
                int emptyToBtnDiff = System.Math.Abs(emptySpace.Row - row);
                if (emptyToBtnDiff == 1)
                {
                    Grid.SetRow(button, emptySpace.Row);
                }
                return;
            }
        }
    }
}