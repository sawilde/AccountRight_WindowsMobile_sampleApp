using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MYOB.Sample
{
    public static class ObjectExtensions
    {
        public static TR Maybe<TO, TR>(this TO _this, Func<TO, TR> action, TR defaultValue = default(TR)) where TO : class
        {
            return _this != null ? action(_this) : defaultValue;
        }
    }
}
