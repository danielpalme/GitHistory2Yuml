using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Palmmedia.GitHistory2Yuml.Wpf.ValueConverters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            byte[] image = value as byte[];

            if (image == null)
            {
                throw new ArgumentException("Expected byte array", "value");
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(image);
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
