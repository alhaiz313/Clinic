using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Clinic.Common
{

    public class MyConverter2 : IValueConverter
    {

        bool flag = false;
        SolidColorBrush brush1 = new SolidColorBrush(Colors.White);
        SolidColorBrush brush2 = new SolidColorBrush(Colors.White);
     

        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            flag = !flag;
            return flag ? brush1 : brush2;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
