using ExpertService.Controllers;
using ExpertService.DTOs;
using ExpertService.Entities;
using ExpertService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ExpertService.UnitTests;

public class ExpertControllerTests
{
    private readonly Mock<IExpertRepository> _ExpertRepo;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Fixture _fixture;
    private readonly ExpertsController _controller;
    private readonly IMapper _mapper;

    public ExpertControllerTests()
    {
        _fixture = new Fixture();
        _ExpertRepo = new Mock<IExpertRepository>();
        _publishEndpoint = new Mock<IPublishEndpoint>();

        var mockMapper = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        _mapper = new Mapper(mockMapper);
        _controller = new ExpertsController(_ExpertRepo.Object, _mapper, _publishEndpoint.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = Helpers.GetClaimsPrincipal() }
            }
        };
    }

    [Fact]
    public async Task GetExperts_WithNoParams_Returns10Experts()
    {
        // arrange
        var Experts = _fixture.CreateMany<ExpertDto>(10).ToList();
        _ExpertRepo.Setup(repo => repo.GetExpertsAsync(null)).ReturnsAsync(Experts);

        // act
        var result = await _controller.GetAllExperts(null);

        // assert
        Assert.Equal(10, result.Value.Count);
        Assert.IsType<ActionResult<List<ExpertDto>>>(result);
    }

    [Fact]
    public async Task GetExpertById_WithValidGuid_ReturnsExpert()
    {
        // arrange
        var Expert = _fixture.Create<ExpertDto>();
        _ExpertRepo.Setup(repo => repo.GetExpertByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Expert);

        // act
        var result = await _controller.GetExpertById(Expert.Id);

        // assert
        Assert.Equal(Expert.Make, result.Value.Make);
        Assert.IsType<ActionResult<ExpertDto>>(result);
    }

    [Fact]
    public async Task GetExpertById_WithInvalidGuid_ReturnsNotFound()
    {
        // arrange
        _ExpertRepo.Setup(repo => repo.GetExpertByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // act
        var result = await _controller.GetExpertById(Guid.NewGuid());

        // assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateExpert_WithValidCreateExpertDto_ReturnsCreatedAtAction()
    {
        // arrange
        var Expert = _fixture.Create<CreateExpertDto>();
        _ExpertRepo.Setup(repo => repo.AddExpert(It.IsAny<Expert>()));
        _ExpertRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // act
        var result = await _controller.CreateExpert(Expert);
        var createdResult = result.Result as CreatedAtActionResult;

        // assert
        Assert.NotNull(createdResult);
        Assert.Equal("GetExpertById", createdResult.ActionName);
        Assert.IsType<ExpertDto>(createdResult.Value);
    }

    [Fact]
    public async Task CreateExpert_FailedSave_Returns400BadRequest()
    {
        // arrange
        var ExpertDto = _fixture.Create<CreateExpertDto>();
        _ExpertRepo.Setup(repo => repo.AddExpert(It.IsAny<Expert>()));
        _ExpertRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(false);

        // act
        var result = await _controller.CreateExpert(ExpertDto);

        // assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateExpert_WithUpdateExpertDto_ReturnsOkResponse()
    {
        // arrange
        var Expert = _fixture.Build<Expert>().Without(x => x.Item).Create();
        Expert.Item = _fixture.Build<Item>().Without(x => x.Expert).Create();
        Expert.Seller = "test";
        var updateDto = _fixture.Create<UpdateExpertDto>();
        _ExpertRepo.Setup(repo => repo.GetExpertEntityById(It.IsAny<Guid>()))
            .ReturnsAsync(Expert);
        _ExpertRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // act
        var result = await _controller.UpdateExpert(Expert.Id, updateDto);

        // assert
        Assert.IsType<OkResult>(result);  
    }

    [Fact]
    public async Task UpdateExpert_WithInvalidUser_Returns403Forbid()
    {
         // arrange
        var Expert = _fixture.Build<Expert>().Without(x => x.Item).Create();
        Expert.Seller = "not-test";
        var updateDto = _fixture.Create<UpdateExpertDto>();
        _ExpertRepo.Setup(repo => repo.GetExpertEntityById(It.IsAny<Guid>()))
            .ReturnsAsync(Expert);

        // act
        var result = await _controller.UpdateExpert(Expert.Id, updateDto);

        // assert
        Assert.IsType<ForbidResult>(result);  
    }

    [Fact]
    public async Task UpdateExpert_WithInvalidGuid_ReturnsNotFound()
    {
        // arrange
        var Expert = _fixture.Build<Expert>().Without(x => x.Item).Create();
        var updateDto = _fixture.Create<UpdateExpertDto>();
        _ExpertRepo.Setup(repo => repo.GetExpertEntityById(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // act
        var result = await _controller.UpdateExpert(Expert.Id, updateDto);

        // assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteExpert_WithValidUser_ReturnsOkResponse()
    {
        // arrange
        var Expert = _fixture.Build<Expert>().Without(x => x.Item).Create();
        Expert.Seller = "test";

        _ExpertRepo.Setup(repo => repo.GetExpertEntityById(It.IsAny<Guid>()))
            .ReturnsAsync(Expert);
        _ExpertRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // act
        var result = await _controller.DeleteExpert(Expert.Id);

        // assert
        Assert.IsType<OkResult>(result);  
    }

    [Fact]
    public async Task DeleteExpert_WithInvalidGuid_Returns404Response()
    {
        // arrange
        var Expert = _fixture.Build<Expert>().Without(x => x.Item).Create();
        _ExpertRepo.Setup(repo => repo.GetExpertEntityById(It.IsAny<Guid>()))
            .ReturnsAsync(value: null);

        // act
        var result = await _controller.DeleteExpert(Expert.Id);

        // assert
        Assert.IsType<NotFoundResult>(result); 
    }

    [Fact]
    public async Task DeleteExpert_WithInvalidUser_Returns403Response()
    {
        // arrange
        var Expert = _fixture.Build<Expert>().Without(x => x.Item).Create();
        Expert.Seller = "not-test";
        _ExpertRepo.Setup(repo => repo.GetExpertEntityById(It.IsAny<Guid>()))
            .ReturnsAsync(Expert);

        // act
        var result = await _controller.DeleteExpert(Expert.Id);

        // assert
        Assert.IsType<ForbidResult>(result); 
    }
}
