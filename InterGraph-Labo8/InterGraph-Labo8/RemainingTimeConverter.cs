using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InterGraph_Labo8
{
    class RemainingTimeConverter : IMultiValueConverter
    {
        #region FinalColorConverter Members

        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            TimeSpan totalTime = ((TimeSpan)values[0]);
            TimeSpan currentTime = ((TimeSpan)values[1]);
            return (totalTime- currentTime).ToString(@"mm\:ss");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
