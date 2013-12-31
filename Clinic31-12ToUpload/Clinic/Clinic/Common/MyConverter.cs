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
  
        public class MyConverter : IValueConverter
        {
            bool flag = false;
            SolidColorBrush brush1 = new SolidColorBrush(Colors.Gray);
            SolidColorBrush brush2 = new SolidColorBrush(Colors.White);
            //string brush1 = "Gray";
            //string brush2 = "LightGray";
            //public object Convert(object value,
            //                      Type targetType,
            //                      object parameter,
            //                      System.Globalization.CultureInfo culture)
            //{

            //    flag = !flag;
            //    return flag ? brush1 : brush2;

            //}

            //public object ConvertBack(object value,
            //                          Type targetType,
            //                          object parameter,
            //                          System.Globalization.CultureInfo culture)
            //{
            //    throw new NotImplementedException();
            //}

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
