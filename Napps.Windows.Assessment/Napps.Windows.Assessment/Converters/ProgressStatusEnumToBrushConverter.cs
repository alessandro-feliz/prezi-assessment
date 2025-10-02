using Napps.Windows.Assessment.Domain.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Napps.Windows.Assessment.Converters
{
    public class ProgressStatusEnumToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProgressStatus progressStatus)
            {
                switch (progressStatus)
                {
                    case ProgressStatus.InProgress:
                        return Brushes.Green;
                    case ProgressStatus.Online:
                        return Brushes.Green;
                    case ProgressStatus.Offline:
                        return Brushes.Orange;
                    case ProgressStatus.Error:
                        return Brushes.Red;
                    default:
                        return Brushes.Gray;
                }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}