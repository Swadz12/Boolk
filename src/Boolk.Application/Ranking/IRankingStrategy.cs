using Boolk.Domain.Entities;

namespace Boolk.Application.Ranking;

public interface IRankingStrategy
{
    string Name { get; }
    
    string Description { get; }
    
    List<RestaurantBase> Rank(List<RestaurantBase> restaurants, List<Review> reviews);
}
