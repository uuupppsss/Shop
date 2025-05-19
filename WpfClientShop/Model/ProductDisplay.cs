using ShopLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfClientShop.Model
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
