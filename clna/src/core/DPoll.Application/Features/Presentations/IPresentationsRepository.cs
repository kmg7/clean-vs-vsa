using Dpoll.Domain.Entities;

namespace DPoll.Application.Features.Presentations;
public interface IPresentationsRepository
{
    Task<Presentation> CreatePresentation(
        Presentation presentation,
        CancellationToken cancellationToken);

    Task<bool> DeletePresentation(Guid id, CancellationToken cancellationToken);

    Task<List<Presentation>> GetPresentations(CancellationToken cancellationToken);

    Task<Presentation?> GetPresentationById(Guid id, CancellationToken cancellationToken);

    Task<bool> PresentationExists(Guid id, CancellationToken cancellationToken);

    Task<bool> UpdatePresentation(
       Presentation presentation,
       CancellationToken cancellationToken);
}
