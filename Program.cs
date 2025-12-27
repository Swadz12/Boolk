using Boolk.Firebase;
using Boolk.Repositories.Firebase;
using Boolk.Repositories.Interfaces;
using Boolk.Factory;
using Boolk.RankingEngine;
using Boolk.RankingEngine.Observers;
using Boolk.Services;
using Boolk.Facade;
using Blazored.LocalStorage;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var firebaseConfig = new FirebaseConfig
{
    ProjectId = builder.Configuration["Firebase:ProjectId"] ?? "boolk-11546",
    CredentialsPath = builder.Configuration["Firebase:CredentialsPath"] ?? "firebase-credentials.json"
};

FirebaseInitializer.Initialize(firebaseConfig);

builder.Services.AddScoped<IRestaurantRepository, FirebaseRestaurantRepository>();
builder.Services.AddScoped<IReviewRepository, FirebaseReviewRepository>();
builder.Services.AddScoped<IUserRepository, FirebaseUserRepository>();

builder.Services.AddSingleton<RestaurantFactory>();

builder.Services.AddSingleton<RankingService>(provider => RankingService.GetInstance());

builder.Services.AddScoped<RankingObserver>();

builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<RestaurantSystemFacade>();
// LocalStorage
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

