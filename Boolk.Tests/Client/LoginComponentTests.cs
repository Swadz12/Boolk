extern alias BoolkClient;
using Bunit;
using BoolkClient::Boolk.Client.Pages;
using BoolkClient::Boolk.Client.ApiClients;
using BoolkClient::Boolk.Client.Auth;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Blazored.LocalStorage;
using Moq.Protected;
using System.Net;
using Boolk.Application.DTOs;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Boolk.Tests.Client;

public class LoginComponentTests : TestContext
{
    private readonly Mock<ILocalStorageService> _mockLocalStorage;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

    public LoginComponentTests()
    {
        _mockLocalStorage = new Mock<ILocalStorageService>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        // Register services required by Login component
        Services.AddScoped<ILocalStorageService>(_ => _mockLocalStorage.Object);
        Services.AddScoped(sp => httpClient);
        Services.AddScoped<AuthApiClient>();
        Services.AddScoped<JwtAuthenticationStateProvider>();
        Services.AddAuthentication();
        Services.AddAuthorization();
    }

    [Fact]
    public void Login_ShouldRenderFormCorrectly()
    {
        // Act
        var cut = RenderComponent<Login>();

        // Assert
        cut.Find("h2").TextContent.Should().Contain("Login");
        cut.Find("input[type='email']").Should().NotBeNull();
        cut.Find("input[type='password']").Should().NotBeNull();
        cut.Find("button[type='submit']").Should().NotBeNull();
    }

    [Fact]
    public void Login_WhenFormSubmitted_ShouldCallLoginApi()
    {
        // Arrange
        var authResponse = new AuthResponse { Success = true, Token = "fake-token" };
        var jsonResponse = JsonSerializer.Serialize(authResponse);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri!.ToString().Contains("login")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var cut = RenderComponent<Login>();

        // Act
        cut.Find("input[type='email']").Change("user@example.com");
        cut.Find("input[type='password']").Change("password");
        cut.Find("form").Submit();

        // Assert
        // Verify mock was called (implies ApiClient was used)
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
