using CloudTrack.Competitions.Domain.ManagingCompetition;
using CloudTrack.Competitions.Infrastructure.Persistence.Common;

namespace CloudTrack.Competitions.Infrastructure.Persistence.Repositories;

internal class CompetitionRepository(ApplicationDbContext dbContext) : Repository<Competition, CompetitionId, ApplicationDbContext>(dbContext), ICompetitionRepository
{
}
