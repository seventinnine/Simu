using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simu.Common
{
    public class NotifyPropertyChanged
    {
        public static void Set<T>(ref T field, T newValue, Action action, Action propertyChanged, bool invokePropertyChanged = true)
        {
            if (!EqualityComparer<T>.Default.Equals(newValue, field))
            {
                field = newValue;

                action.Invoke();

                if (invokePropertyChanged)
                    propertyChanged.Invoke();

            }
        }
    }
}
