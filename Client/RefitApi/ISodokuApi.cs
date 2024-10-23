using BlazorSodokuApp.Client.Pages;
using BlazorSodokuApp.Shared;
using Refit;

namespace BlazorSodokuApp.Client.RefitApi
{
    public interface ISodokuApi
    {
        [Post("/api/sodoku")]
        Task<HttpResponseMessage> SaveSodoku(PostSodokuRequest board);


        [Get("/api/sodoku")]
        Task<List<SudokuGameResponse>> GetSodoku();
    }
}
