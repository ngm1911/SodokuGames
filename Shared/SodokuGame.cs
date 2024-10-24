using System.Text;

namespace BlazorSodokuApp.Shared
{
    public class SudokuGame
    {
        public int[,] Board { get; set; }
        public int[,] BoardInit { get; set; }

        public SudokuGame()
        {
            Board = new int[9, 9];
            BoardInit = new int[9, 9];
        }

        public bool IsValidMove(int row, int col, int num)
        {
            if (num != 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (i != col && Board[row, i] == num)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    if (i != row && Board[i, col] == num)
                    {
                        return false;
                    }
                }

                int startRow = row / 3 * 3;
                int startCol = col / 3 * 3;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (startRow + i != row
                            && startCol + i != col
                            && Board[startRow + i, startCol + j] == num)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool IsValidAll()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!IsValidMove(i, j, Board[i, j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void GenerateNewGame()
        {
            FillDiagonal();
            FillRemaining(0, 0);
            RemoveNumbers(40);
            BoardInit = Board.Clone() as int[,];

            void FillDiagonal()
            {
                for (int i = 0; i < 9; i += 3)
                    FillBox(i, i);
            }

            void FillBox(int row, int col)
            {
                Random rand = new Random();
                List<int> numbers = Enumerable.Range(1, 9).OrderBy(x => rand.Next()).ToList();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Board[row + i, col + j] = numbers[i * 3 + j];
                    }
                }
            }

            bool FillRemaining(int row, int col)
            {
                try
                {
                    if (row >= 9 && col >= 9)
                    {
                        return true;
                    }
                    if (col >= 9)
                    {
                        row++;
                        col = 0;
                    }
                    if (Board[row, col] != 0)
                    {
                        return FillRemaining(row, col + 1);
                    }

                    Random rand = new();
                    List<int> numbers = Enumerable.Range(1, 9).OrderBy(x => rand.Next()).ToList();

                    foreach (var num in numbers)
                    {
                        if (IsSafe(row, col, num))
                        {
                            Board[row, col] = num;

                            if (FillRemaining(row, col + 1))
                            {
                                return true;
                            }

                            Board[row, col] = 0;
                        }
                    }
                    return false;
                }
                catch
                {
                    return true;
                }
            }

            bool IsSafe(int row, int col, int num)
            {
                // Row check
                for (int x = 0; x < 9; x++)
                {
                    if (Board[row, x] == num)
                    {
                        return false;
                    }
                }

                // Column check
                for (int x = 0; x < 9; x++)
                {
                    if (Board[x, col] == num)
                    {
                        return false;
                    }
                }

                // Check 3x3 block
                int startRow = row - row % 3, startCol = col - col % 3;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (Board[i + startRow, j + startCol] == num)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            void RemoveNumbers(int count)
            {
                Random rand = new();
                while (count > 0)
                {
                    int i = rand.Next(9);
                    int j = rand.Next(9);
                    if (Board[i, j] != 0)
                    {
                        Board[i, j] = 0;
                        count--;
                    }
                }
            }
        }

        public string SaveBoardToString()
        {
            var boardString = new StringBuilder(81);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    boardString.Append(Board[i, j]);
                }
            }
            return boardString.ToString();
        }
    }
}
