using FluentValidation;

namespace BlazorSodokuApp.Shared
{
    public class SudokuGameResponse
    {
        public int Id { get; set; }
        public string? Board { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
