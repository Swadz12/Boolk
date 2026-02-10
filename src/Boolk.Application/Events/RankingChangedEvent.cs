using MediatR;

namespace Boolk.Application.Events;

public record RankingChangedEvent : INotification
{
    public Guid? RestaurantId { get; init; }
    
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
