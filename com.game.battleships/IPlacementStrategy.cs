namespace com.game.battleships
{
    public interface IPlacementStrategy
    {
        Cell[] FindPlacement(Target ship);
    }
}