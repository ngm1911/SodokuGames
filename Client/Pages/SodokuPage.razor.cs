using BlazorSodokuApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorSodokuApp.Client.Pages
{
    public partial class SodokuPage : OwningComponentBase
    {
        private SudokuGame game = new();
        List<SudokuGameResponse> gamesSaved;

        private async Task CheckIsValid()
        {
            if (game.IsValidAll())
            {
                await JSRuntime.InvokeVoidAsync("showAlert", "Success!");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showAlert", "Error!");
            }
        }

        private async Task SolveSudokuGame()
        {
            if (SolveSudoku(0, 0) == false)
            {
                await JSRuntime.InvokeVoidAsync("showAlert", "Error!");
            }
            else
            {
                var result = await ISodokuApi.SaveSodoku(new(game.SaveBoardToString()));
                if (result.IsSuccessStatusCode)
                {
                    await JSRuntime.InvokeVoidAsync("showAlert", "Save Successful!");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("showAlert", await result.Content.ReadAsStringAsync());
                }
            }
        }
        
        private void CreateEmptyBoard()
        {
            game = new();
        }

        private void GenerateNewGame()
        {
            CreateEmptyBoard();
            game.GenerateNewGame();
        }

        [JSInvokable("SelectionChanged")]
        public async Task SelectionChanged(int selectedId)
        {
            var dataSaved = gamesSaved.FirstOrDefault(x => x.Id == selectedId);
            if (dataSaved != null)
            {
                game.LoadBoardFromString(dataSaved.Board);
                await InvokeAsync(StateHasChanged);
            }
        }

        private void UpdateCell(int row, int col, string input)
        {
            if (int.TryParse(input, out int value) && value >= 0 && value <= 9)
            {
                game.Board[row, col] = value;
            }
            else
            {
                game.Board[row, col] = 0;
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
                    if (SolveSudoku(row, col + 1)) return true;
                    game.Board[row, col] = 0;
                }
            }
            return false;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            ISodokuApi.GetSodoku().ContinueWith(async t =>
            {
                gamesSaved = t.Result;
                await JSRuntime.InvokeVoidAsync("updateDataGrid", t.Result);
            });

            var dotnetHelper = DotNetObjectReference.Create(this);
            JSRuntime.InvokeVoidAsync("DotnetHelpers.setDotNetHelper", dotnetHelper);
        }
    }
}
