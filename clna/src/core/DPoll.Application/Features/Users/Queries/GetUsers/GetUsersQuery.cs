using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using MediatR;

namespace DPoll.Application.Features.Users.Queries.GetUsers;
public class GetUsersQuery : IRequest<Result<List<User>>> { }
