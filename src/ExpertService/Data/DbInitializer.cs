using ExpertService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpertService.Data;

public class DbInitializer
{

    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<ExpertDbContext>());
    }

    private static void SeedData(ExpertDbContext context)
    {
        context.Database.Migrate();

        if (context.Experts.Any())
        {
            Console.WriteLine("Already have data - no need to seed");
            return;
        }

        var experts = new List<Expert>()
        {
            // 1 Sundeep: Personal Finance
            new Expert
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Online,
                ExpertName = "Sundeep Jaiswal",
                ExpertDept = "GSC/EQD/CTY",
                ExpertExp = 9,
                ContactNo = 7506308626,
                Email = "sundeep.jaiswal@socgen.com",
                Available = 1,
                Topics = new Topics
                {
                    TopicName = "Personal Finance",
                    TopicExp = "6 Years",
                    Description = "Importance of Personal Finance: These goals could be anything—having enough for short-term financial needs, planning for retirement,"
                    + "or saving for your child's college education. It depends on your income, spending, saving, investing, and personal protection (insurance and estate planning).",
                    ImageUrl = "https://funlovincriminals.tv/wp-content/uploads/2019/06/Personal-Finance-min.jpg"

                }
            },
            // 2 Suman: Mutual Funds
             new Expert
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Online,
                ExpertName = "Suman",
                ExpertDept = "GSC/EQD/CTY",
                ExpertExp = 12,
                ContactNo = 7506304896,
                Email = "suman@socgen.com",
                Available = 1,
                Topics = new Topics
                {
                    TopicName = "Mutual Funds",
                    TopicExp = "4 Years",
                    Description = "Mutual funds offer diversification or access to a wider variety of investments than an individual investor could afford to buy." +
                    "There are economies of scale in investing with a group. Monthly contributions help the investor's assets grow." +
                    "Funds are more liquid because they tend to be less volatile." ,
                    ImageUrl = "https://th.bing.com/th/id/OIP.K9MXO7hwTDlkA0CsukndPAHaEK?pid=ImgDet&rs=1"

                }
            },
            // 3 Gagan Cricket
            new Expert
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Online,
                ExpertName = "Gagan Garg",
                ExpertDept = "GSC/EQD/VOL",
                ExpertExp = 8,
                ContactNo = 7896341236,
                Email = "gagan.garg@socgen.com",
                Available = 1,
                Topics = new Topics
                {
                    TopicName = "Cricket",
                    TopicExp = "8 Years",
                    Description = "Cricket is a bat-and-ball game played between two teams of eleven players on a field at the centre of which is a 22-yard (20-metre)"
                    + "pitch with a wicket at each end, each comprising two bails balanced on three stumps.",
                    ImageUrl = "https://cdn.britannica.com/47/148847-050-C4FB5341/Cricket-bat-ball.jpg"

                }
            },
            // 4 Shruti  Profestional Lifestyle 
          new Expert
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Online,
                ExpertName = "Shruti",
                ExpertDept = "GSC/EQD/CTY",
                ExpertExp = 6,
                ContactNo = 750630,
                Email = "struti.rai@socgen.com",
                Available = 1,
                Topics = new Topics
                {
                    TopicName = "Corporate Lifestyle",
                    TopicExp = "5 Years",
                    Description = "Corporate culture refers to the values, beliefs, and behaviors that determine how a company's employees and management interact," +
                    "perform, and handle business transactions. ",
                    ImageUrl = "https://th.bing.com/th/id/OIP.K9MXO7hwTDlkA0CsukndPAHaEK?pid=ImgDet&rs=1"

                }
            },
            // 5 Shubham
            new Expert
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Online,
                ExpertName = "Shubham",
                ExpertDept = "GSC/EQD/CTY",
                ExpertExp = 2,
                ContactNo = 7506789630,
                Email = "struti.rai@socgen.com",
                Available = 1,
                Topics = new Topics
                {
                    TopicName = "Real Estate",
                    TopicExp = "2 Years",
                    Description = "Real estate is defined as the land and any permanent structures, like a home, or improvements attached to the land, whether natural or man-made." ,
                    ImageUrl = "https://meritabode.com/wp-content/uploads/2020/07/Real-estate-terms.png"

                }
            },
                   // 6 Rohit
            new Expert
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Online,
                ExpertName = "Rohit",
                ExpertDept = "GSC/EQD/CTY",
                ExpertExp = 14,
                ContactNo = 7504569630,
                Email = "Rohit@socgen.com",
                Available = 1,
                Topics = new Topics
                {
                    TopicName = "Car ",
                    TopicExp = "2 Years",
                    Description = "A car enthusiast is someone who enjoys cars or anything about the automotive world. They enjoy learning about cars, messing around with cars," +
                     "discussing them and generally just being involved in the automotive community." ,
                    ImageUrl = "hhttps://ih1.redbubble.net/image.863779528.7242/poster,504x498,f8f8f8-pad,600x600,f8f8f8.u3.jpg"

                }
            },

        };

        context.AddRange(experts);

        context.SaveChanges();
    }
}
