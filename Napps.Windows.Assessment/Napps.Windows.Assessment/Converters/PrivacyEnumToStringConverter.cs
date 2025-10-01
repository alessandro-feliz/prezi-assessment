using Napps.Windows.Assessment.Domain;
using Napps.Windows.Assessment.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Napps.Windows.Assessment.Converters
{
    public class PrivacyEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Privacy privacy)
            {
                switch (privacy)
                {
                    case Privacy.Public:
                        return Resources.PrivacyPublic;
                    case Privacy.Private:
                        return Resources.PrivacyPrivate;
                    case Privacy.Hidden:
                        return Resources.PrivacyHidden;
                    default:
                        return Resources.PrivacyUnknown;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}