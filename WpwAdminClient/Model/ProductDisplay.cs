using ShopLib;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfAdminClient.Model
{
    public class ProductDisplay:ProductDTO
    {
        public BitmapImage ImageSource => Base64ToImage(HeaderImage);

        private BitmapImage Base64ToImage(byte[] header_image)
        {
            if(header_image != null)
            {
                using (MemoryStream ms = new MemoryStream(header_image))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
            }
            return null;
        }
    }
}
