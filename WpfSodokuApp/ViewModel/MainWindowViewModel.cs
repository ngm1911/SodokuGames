using BlazorSodokuApp.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;

namespace WpfSodokuApp.ViewModel
{
    public partial class CellViewModel : ObservableObject
    {
        [ObservableProperty]
        private int row;


        [ObservableProperty]
        private int column;


        [ObservableProperty]
        private string value;


        [ObservableProperty]
        private bool isReadOnly = false;

        partial void OnValueChanged(string value)
        {
            WeakReferenceMessenger.Default.Send(new CellValueChanged(this));
        }
    }

    public class CellValueChanged : ValueChangedMessage<CellViewModel>
    {
        public CellValueChanged(CellViewModel value) : base(value)
        {
        }
    }

    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<BlockViewModel> blocks = new ObservableCollection<BlockViewModel>();

        public BlockViewModel Block0 => Blocks[0];
        public BlockViewModel Block1 => Blocks[1];
        public BlockViewModel Block2 => Blocks[2];
        public BlockViewModel Block3 => Blocks[3];
        public BlockViewModel Block4 => Blocks[4];
        public BlockViewModel Block5 => Blocks[5];
        public BlockViewModel Block6 => Blocks[6];
        public BlockViewModel Block7 => Blocks[7];
        public BlockViewModel Block8 => Blocks[8];

        public bool CheckEnabled => game.SaveBoardToString().Contains('0');

        private SudokuGame? game;

        public MainWindowViewModel()
        {
            GenerateNewGameCommand.Execute(null);

            WeakReferenceMessenger.Default.Register<CellValueChanged>(this, (r, m) =>
            {
                if (int.TryParse(m.Value.Value, out int value) && value >= 0 && value <= 9)
                {
                    game.Board[m.Value.Row, m.Value.Column] = value;
                }
                else
                {
                    game.Board[m.Value.Row, m.Value.Column] = 0;
                }
                OnPropertyChanged(nameof(CheckEnabled));
            });
        }

        [RelayCommand(CanExecute = nameof(CheckEnabled))]
        public void CheckSudoku()
        {
            if (game.IsValidAll())
            {
                MessageBox.Show("Success!");
            }
            else
            {
                MessageBox.Show("Error!");
            }
        }

        [RelayCommand]
        public void CreateEmptyBoard()
        {
            game = new();
            FillBoard();
        }

        [RelayCommand]
        public void GenerateNewGame()
        {
            game = new();
            game.GenerateNewGame();
            FillBoard();
        }

        [RelayCommand]
        public void SolveSudokuGame()
        {
            var success = SolveSudoku(0, 0);
            FillBoard();
            if (success == false)
            {
                MessageBox.Show("Error!");
            }
            else
            {
                MessageBox.Show("Success!");
            }
        }

        public bool SolveSudoku(int row = 0, int col = 0)
        {
            if (row == 9) return true;
            if (col == 9) return SolveSudoku(row + 1, 0);
            if (game.Board[row, col] != 0)
                return SolveSudoku(row, col + 1);

            for (int num = 1; num <= 9; num++)
            {
                if (game.IsValidMove(row, col, num))
                {
                    game.Board[row, col] = num;
                    if (SolveSudoku(row, col + 1)) 
                        return true;
                    game.Board[row, col] = 0;
                }
            }
            return false;
        }

        private void FillBoard()
        {
            Blocks.Clear();
            for (int i = 0; i < 9; i++)
            {
                var block = new BlockViewModel();
                for (int j = 0; j < 9; j++)
                {
                    block.Cells.Add(new CellViewModel()
                    {
                        Row = i,
                        Column = j,
                        Value = game.Board[i, j] == 0 ? string.Empty : game.Board[i, j].ToString(),
                        IsReadOnly = game.BoardInit[i, j] != 0 ? true : false
                    });
                }
                Blocks.Add(block);
            }

            OnBlocksChanged(null);
        }

        partial void OnBlocksChanged(ObservableCollection<BlockViewModel> value)
        {
            OnPropertyChanged(nameof(Block0));
            OnPropertyChanged(nameof(Block1));
            OnPropertyChanged(nameof(Block2));
            OnPropertyChanged(nameof(Block3));
            OnPropertyChanged(nameof(Block4));
            OnPropertyChanged(nameof(Block5));
            OnPropertyChanged(nameof(Block6));
            OnPropertyChanged(nameof(Block7));
            OnPropertyChanged(nameof(Block8));
        }
    }
}
