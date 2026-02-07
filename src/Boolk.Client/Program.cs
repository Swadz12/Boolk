using Boolk.Client;
using Boolk.Client.ApiClients;
using Boolk.Client.Auth;
using Boolk.Infrastructure;
using Boolk.Infrastructure.Persistence.Firebase;
using Boolk.Infrastructure.Auth;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;

var culture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// ============================================================
// HYBRID MODE: Both direct DB access AND API client available
// This allows gradual migration of pages to use API
// ============================================================

// Option 1: Add Infrastructure services for direct Firebase access (legacy mode)
// This can be removed once all pages are migrated to use API
var firebaseConfig = new FirebaseConfig
{
    ProjectId = builder.Configuration["Firebase:ProjectId"] ?? "boolk-11546",
    CredentialsPath = builder.Configuration["Firebase:CredentialsPath"] ?? "firebase-credentials.json"
};

var jwtSettings = new JwtSettings
{
    Secret = builder.Configuration["Jwt:Secret"] ?? "MySecretKeyForLocalDevelopment123!@#",
    Issuer = builder.Configuration["Jwt:Issuer"] ?? "Boolk.API",
    Audience = builder.Configuration["Jwt:Audience"] ?? "Boolk.Client",
    ExpirationMinutes = int.TryParse(builder.Configuration["Jwt:ExpirationMinutes"], out var exp) ? exp : 60
};

builder.Services.AddInfrastructure(firebaseConfig, jwtSettings);

// Option 2: Add Client services for API access (new mode)
// Configure the base URL for the API (must match where API is actually running)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5108";
builder.Services.AddBoolkClient(apiBaseUrl);

// Add Authorization for Blazor pages
builder.Services.AddAuthorizationCore();

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
