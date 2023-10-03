using ExpertService.Entities;

namespace ExpertService.UnitTests;

public class ExpertEntityTests
{
    [Fact]
    public void HasReservePrice_ReservePriceGtZero_True()
    {
        // arrange
        var Expert = new Expert { Id = Guid.NewGuid(), ReservePrice = 10 };

        // act
        var result = Expert.HasReservePrice();

        // assert
        Assert.True(result);
    }

    [Fact]
    public void HasReservePrice_ReservePriceIsZero_False()
    {
        // arrange
        var Expert = new Expert { Id = Guid.NewGuid(), ReservePrice = 0 };

        // act
        var result = Expert.HasReservePrice();

        // assert
        Assert.False(result);
    }
}