namespace Salday.EventBus.Interfaces
{
    /// <summary>
    /// It is not necessary to implement this interface. Used for conveniance
    /// </summary>
    public interface IProxy
    {
        void SetSubscription(ISubscription subs);
    }
}
