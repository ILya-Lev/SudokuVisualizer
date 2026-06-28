using Microsoft.Extensions.Hosting.WindowsServices;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
        ? AppContext.BaseDirectory
        : null
};

var builder = WebApplication.CreateBuilder(options);

builder.Services.AddWindowsService(lifetimeOptions => { lifetimeOptions.ServiceName = "SudokuVisualizer"; });
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
