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
        Random random = new Random();
        bool isSolvable;
        List<int> kolejnoscPietnastki = new List<int>();
        int emptyRow = 3;
        int emptyCol = 3;
        private int[,] board = new int[4, 4];

        public MainWindow()
        {
            InitializeComponent();
            randomPietnaskaGenerator();
            UstawPlanszeNaGridzie();
        }
        
        private void randomPietnaskaGenerator()
        {
            do
            {
                kolejnoscPietnastki = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
                for (int i = kolejnoscPietnastki.Count - 1; i > 0; i--)
                {
                    int j = random.Next(0, i + 1);

                    int temp = kolejnoscPietnastki[i];
                    kolejnoscPietnastki[i] = kolejnoscPietnastki[j];
                    kolejnoscPietnastki[j] = temp;
                }
            } while (!IsSolvable(kolejnoscPietnastki.ToArray()));
        }

        private bool IsSolvable(int[] board)
        {
            int inversions = 0;

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = i + 1; j < board.Length; j++)
                {
                    if (board[i] > board[j])
                        inversions++;
                }
            }

            // Puste pole zakładamy w prawym dolnym rogu
            // Wtedy rozwiązywalność = liczba inwersji parzysta
            return inversions % 2 == 0;
        }

        private void UstawPlanszeNaGridzie()
        {
            int index = 0;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (index < kolejnoscPietnastki.Count)
                    {
                        board[row, col] = kolejnoscPietnastki[index];

                        Button btn = (Button)MainGrid.Children[index];
                        btn.Content = kolejnoscPietnastki[index].ToString();
                        Grid.SetRow(btn, row);
                        Grid.SetColumn(btn, col);

                        index++;
                    }
                    else
                    {
                        board[row, col] = 0; // puste pole
                    }
                }
            }
        }

        private (int row, int col) FindEmpty()
        {
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    if (board[r, c] == 0)
                        return (r, c);

            return (-1, -1);
        }

        private bool IsSolved()
        {
            int expected = 1;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    // ostatnie pole musi być puste
                    if (row == 3 && col == 3)
                    {
                        return board[row, col] == 0;
                    }

                    if (board[row, col] != expected)
                        return false;

                    expected++;
                }
            }

            return true;
        }

        private void CheckForFreePlace_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            var empty = FindEmpty();

            if ((Math.Abs(empty.row - row) == 1 && empty.col == col) ||
                (Math.Abs(empty.col - col) == 1 && empty.row == row))
            {
                // aktualizacja planszy w kodzie
                board[empty.row, empty.col] = board[row, col];
                board[row, col] = 0;

                // przesunięcie przycisku w GUI
                Grid.SetRow(button, empty.row);
                Grid.SetColumn(button, empty.col);

                if (IsSolved())
                {
                    MessageBox.Show("Gratulacje! Układanka rozwiązana!");
                }
            }
        }
    }
}