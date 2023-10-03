using Contracts;
using MassTransit;

namespace ExpertService;

public class ExpertCreatedFaultConsumer : IConsumer<Fault<ExpertCreated>>
{
    public async Task Consume(ConsumeContext<Fault<ExpertCreated>> context)
    {
        Console.WriteLine("--> Consuming faulty creation");

        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == "System.ArgumentException")
        {
            context.Message.Message.Model = "FooBar";
            await context.Publish(context.Message.Message);
        }
        else 
        {
            Console.WriteLine("Not an argument exception - update error dashboard somewhere");
        }
    }
}
