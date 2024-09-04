using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Users.Queries.GetUsers;
public class GetUsersQuery : IRequest<List<User>> { }
