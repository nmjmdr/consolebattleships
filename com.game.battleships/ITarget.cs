namespace com.game.battleships
{
    public interface ITarget
    {
        string Name
        {
            get;
        }
        void HandleHit(HitEvent hitEvent);
        int Points
        {
            get;
        }
    }
}