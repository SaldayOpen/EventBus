using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
