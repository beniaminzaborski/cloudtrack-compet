﻿using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public class Competition : Entity<CompetitionId>, IAggregateRoot
{
    private Competition() { }

    public Competition(
        CompetitionId id,
        string name,
        Distance dictance,
        DateTime startAt,
        int maxCompetitors,
        CompetitionPlace place)
    {
        if (maxCompetitors <= 0) throw new ArgumentException("Numbers of maximum competitors must be greater than 0", nameof(maxCompetitors));

        Id = id;
        Name = name;
        Distance = dictance;
        StartAt = startAt;
        MaxCompetitors = maxCompetitors;
        Place = place;
        Status = CompetitionStatus.Draft;

        AddCheckpoint(CreateStartLineCheckpoint());
        AddCheckpoint(CreateFinishLineCheckpoint());
    }

    public string Name { get; private set; }

    public Distance Distance { get; private set; }

    public CompetitionPlace Place { get; private set; }

    public DateTime StartAt { get; private set; }

    public int MaxCompetitors { get; private set; }

    public CompetitionStatus Status { get; private set; }

    private IList<Checkpoint> _checkpoints = new List<Checkpoint>();
    public IReadOnlyCollection<Checkpoint> Checkpoints => _checkpoints.OrderBy(c => c.TrackPoint.Amount).ToList().AsReadOnly();

    public void AddCheckpoint(Checkpoint checkpoint)
    {
        if (_checkpoints.Any(c => c.TrackPoint.Equals(checkpoint.TrackPoint))) throw new CheckpointAlreadyExistsException();
        _checkpoints.Add(checkpoint);
        QueueDomainEvent(new CompetitionCheckpointAdded(Id, checkpoint.Id, checkpoint.TrackPoint));
    }

    public void RemoveCheckpoint(CheckpointId checkpointId)
    {
        var checkpoint = _checkpoints.FirstOrDefault(c => c.Id.Equals(checkpointId));
        if (checkpoint is null) throw new CheckpointNotExistsException();
        _checkpoints.Remove(checkpoint);
        QueueDomainEvent(new CompetitionCheckpointRemoved(Id, checkpoint.Id, checkpoint.TrackPoint));
    }

    public void OpenRegistration()
    {
        if (Status != CompetitionStatus.Draft) throw new CannotOpenRegistrationException();

        Status = CompetitionStatus.OpenedForRegistration;
        QueueDomainEvent(new CompetitionOpenedForRegistration(Id, Place, Distance, StartAt, MaxCompetitors, Checkpoints));
    }

    public void CompleteRegistration()
    {
        if (Status != CompetitionStatus.OpenedForRegistration) throw new CannotCompleteRegistrationException();

        Status = CompetitionStatus.RegistrationCompleted;
        QueueDomainEvent(new CompetitionRegistrationCompleted(Id));
    }

    public void ChangeMaxCompetitors(int maxCompetitors)
    {
        switch (Status)
        {
            case CompetitionStatus.Draft or CompetitionStatus.OpenedForRegistration
                when maxCompetitors > MaxCompetitors:
                MaxCompetitors = maxCompetitors;
                QueueDomainEvent(new CompetitionMaxCompetitorsIncreased(Id, MaxCompetitors));
                break;
            case CompetitionStatus.Draft
                when maxCompetitors < MaxCompetitors:
                MaxCompetitors = maxCompetitors;
                QueueDomainEvent(new CompetitionMaxCompetitorsDecreased(Id, MaxCompetitors));
                break;
            default:
                throw new CompetitionMaxCompetitorsChangeNotAllowedException();
        }
    }

    private Checkpoint CreateStartLineCheckpoint()
    {
        return new Checkpoint(CheckpointId.From(Guid.NewGuid()), Id, new Distance(0, Distance.Unit));
    }

    private Checkpoint CreateFinishLineCheckpoint()
    {
        return new Checkpoint(CheckpointId.From(Guid.NewGuid()), Id, new Distance(Distance.Amount, Distance.Unit));
    }
}
