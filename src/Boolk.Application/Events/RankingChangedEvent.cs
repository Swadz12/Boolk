using MediatR;

namespace Boolk.Application.Events;

/// <summary>
/// Published when any action occurs that could affect restaurant rankings.
/// </summary>
public record RankingChangedEvent : INotification
{
    /// <summary>
    /// The ID of the restaurant affected (optional, null for global updates).
    /// </summary>
    public Guid? RestaurantId { get; init; }
    
    /// <summary>
    /// Type of change that triggered the event.
    /// </summary>
    public RankingChangeType ChangeType { get; init; }
}

public enum RankingChangeType
{
    ReviewAdded,
    ReviewDeleted,
    RestaurantCreated,
    RestaurantUpdated,
    RestaurantDeleted
}
