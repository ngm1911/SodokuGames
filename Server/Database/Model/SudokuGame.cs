using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazorSodokuApp.Server.Database.Model
{
    [Table("SudokuGames")]
    public class SudokuGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(81)]
        public string? Board { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public SudokuGame()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
