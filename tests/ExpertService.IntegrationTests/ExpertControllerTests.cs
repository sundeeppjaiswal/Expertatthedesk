using System.Net;
using System.Net.Http.Json;
using ExpertService.Data;
using ExpertService.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace ExpertService.IntegrationTests;

[Collection("Shared collection")]
public class ExpertControllerTests : IAsyncLifetime
{
    private readonly CustomWebAppFactory _factory;
    private readonly HttpClient _httpClient;
    private const string _gT_ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

    public ExpertControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetExperts_ShouldReturn3Experts()
    {
        // arrange? 

        // act
        var response = await _httpClient.GetFromJsonAsync<List<ExpertDto>>("api/Experts");

        // assert
        Assert.Equal(3, response.Count);
    }

    [Fact]
    public async Task GetExpertById_WithValidId_ShouldReturnExpert()
    {
        // arrange? 

        // act
        var response = await _httpClient.GetFromJsonAsync<ExpertDto>($"api/Experts/{_gT_ID}");

        // assert
        Assert.Equal("GT", response.Model);
    }

    [Fact]
    public async Task GetExpertById_WithInvalidId_ShouldReturn404()
    {
        // arrange? 

        // act
        var response = await _httpClient.GetAsync($"api/Experts/{Guid.NewGuid()}");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetExpertById_WithInvalidGuid_ShouldReturn400()
    {
        // arrange? 

        // act
        var response = await _httpClient.GetAsync($"api/Experts/notaguid");

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateExpert_WithNoAuth_ShouldReturn401()
    {
        // arrange? 
        var Expert = new CreateExpertDto { Make = "test" };

        // act
        var response = await _httpClient.PostAsJsonAsync($"api/Experts", Expert);

        // assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateExpert_WithAuth_ShouldReturn201()
    {
        // arrange? 
        var Expert = GetExpertForCreate();
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

        // act
        var response = await _httpClient.PostAsJsonAsync($"api/Experts", Expert);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdExpert = await response.Content.ReadFromJsonAsync<ExpertDto>();
        Assert.Equal("bob", createdExpert.Seller);
    }

    [Fact]
    public async Task CreateExpert_WithInvalidCreateExpertDto_ShouldReturn400()
    {
        // arrange? 
        var Expert = GetExpertForCreate();
        Expert.Make = null;
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

        // act
        var response = await _httpClient.PostAsJsonAsync($"api/Experts", Expert);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateExpert_WithValidUpdateDtoAndUser_ShouldReturn200()
    {
        // arrange? 
        var updateExpert = new UpdateExpertDto { Make = "Updated" };
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

        // act
        var response = await _httpClient.PutAsJsonAsync($"api/Experts/{_gT_ID}", updateExpert);

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateExpert_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
    {
        // arrange? 
        var updateExpert = new UpdateExpertDto { Make = "Updated" };
        _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("notbob"));

        // act
        var response = await _httpClient.PutAsJsonAsync($"api/Experts/{_gT_ID}", updateExpert);

        // assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ExpertDbContext>();
        DbHelper.ReinitDbForTests(db);
        return Task.CompletedTask;
    }

    private static CreateExpertDto GetExpertForCreate()
    {
        return new CreateExpertDto
        {
            Make = "test",
            Model = "testModel",
            ImageUrl = "test",
            Color = "test",
            Mileage = 10,
            Year = 10,
            ReservePrice = 10
        };
    }
}
