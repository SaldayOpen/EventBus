
namespace Salday.EventBus
{
    /// <summary>
    /// The base class for all cancelable events
    /// </summary>
    public abstract class CancelableEventBase : EventBase
    {
        public bool Canceled { get; set; } = false;
    }
}
