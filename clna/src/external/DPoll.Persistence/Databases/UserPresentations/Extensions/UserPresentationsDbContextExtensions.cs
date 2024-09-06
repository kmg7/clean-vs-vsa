using Bogus;
using Dpoll.Domain.Entities;
using DPoll.Persistence.Databases.UserPresentations;

namespace DPoll.Persistence.Databases.UserPresentations.Extensions;
internal static class UserPresentationsDbContextExtensions
{
    public static UserPresentationsDbContext AddData(this UserPresentationsDbContext context)
    {
        var users = new Faker<User>()
            .RuleFor(a => a.Id, _ => Guid.NewGuid())
            .RuleFor(a => a.Username, f => f.Person.UserName)
            .RuleFor(a => a.Email, f => f.Person.Email)
            .RuleFor(a => a.ClerkId, f => f.Person.Address.ZipCode) // :)
            .RuleFor(a => a.CreatedAt, f => DateTime.SpecifyKind(f.Date.Past(), DateTimeKind.Utc))
            .RuleFor(a => a.UpdatedAt, f => DateTime.SpecifyKind(f.Date.Past(), DateTimeKind.Utc))
            .RuleFor(a => a.IsActive, f => f.PickRandom(true, false))
            .Generate(15);

        context.AddRange(users);

        var presentations = new Faker<Presentation>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.UserId, f => f.PickRandom(users).Id)
            .RuleFor(r => r.Title, f => f.Commerce.ProductName())
            .RuleFor(r => r.CreatedAt, f => DateTime.SpecifyKind(f.Date.Past(), DateTimeKind.Utc))
            .RuleFor(r => r.UpdatedAt, f => DateTime.SpecifyKind(f.Date.Past(), DateTimeKind.Utc))
            .Generate(50);

        context.AddRange(presentations);

        _ = context.SaveChanges();

        return context;
    }
}
