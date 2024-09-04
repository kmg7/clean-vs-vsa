using Dpoll.Domain.Entities;
using MediatR;

namespace DPoll.Application.Features.Presentations.Queries.GetPresentations;
public class GetPresentationsQuery : IRequest<List<Presentation>> { }
