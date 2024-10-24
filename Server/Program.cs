using BlazorSodokuApp.Server.Database.DbContext;
using BlazorSodokuApp.Server.Database.Model;
using BlazorSodokuApp.Shared;
using Microsoft.EntityFrameworkCore;
using SudokuGame = BlazorSodokuApp.Server.Database.Model.SudokuGame;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SudokuContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapPost("api/sodoku", async (SudokuContext db, PostSodokuRequest board, CancellationToken token) =>
{
    var result = new PostSodokuRequestValidator().Validate(board);
    if (result.IsValid == false)
    {
        return Results.BadRequest(result.Errors);
    }
    else
    {
        await db.SudokuGames.AddAsync(new SudokuGame()
        {
            Board = board.Board
        }, token);
        await db.SaveChangesAsync(token);
        return Results.Ok();
    }
});

app.MapGet("api/sodoku", async (SudokuContext db, CancellationToken token) =>
{
    var result = await db.SudokuGames.ToListAsync(token);
    return Results.Ok(result);
});

app.Run();
