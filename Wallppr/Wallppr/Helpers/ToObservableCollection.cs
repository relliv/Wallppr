using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Wallppr.Helpers
{
    public static class ObservableCollectionHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
