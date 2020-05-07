using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallppr.ViewModel
{
    // source: https://www.technical-recipes.com/2016/using-relaycommand-icommand-to-handle-events-in-wpf-and-mvvm/

    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
