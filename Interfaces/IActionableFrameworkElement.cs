using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface IActionableFrameworkElement
    {
        String SendMessage(String message);

        event PluginEventDelegate EventOccurred;
    }
}
