namespace Salday.EventBus
{
    /// <summary>
    /// Handler priority. Handlers with higher priority are called first
    /// </summary>
    public enum HandlerPriority : byte
    {
        Lowest = 1,

        Low = 2,

        Medium = 3,

        High = 4,

        Highest = 5
    }
}