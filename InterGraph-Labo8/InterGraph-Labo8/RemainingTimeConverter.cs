using System;
using System.Windows.Data;

namespace InterGraph_Labo8
{
    class RemainingTimeConverter : IMultiValueConverter
    {
        #region FinalColorConverter Members

        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values[0] is TimeSpan && values[1] is TimeSpan)
            {
                TimeSpan totalTime = ((TimeSpan)values[0]);
                TimeSpan currentTime = ((TimeSpan)values[1]);
                return (totalTime - currentTime);
            }
            else return "";

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
