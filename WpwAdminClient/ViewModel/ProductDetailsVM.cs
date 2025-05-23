using ShopLib;
using System.IO;
using System.Windows.Media.Imaging;
using WpfAdminClient.Model;
using WpfAdminClient.Services;
using WpfAdminClient.View;

namespace WpfAdminClient.ViewModel
{
    public class ProductDetailsVM : BaseVM
    {
        private ProductDTO _product;
        public ProductDTO Product
        {
            get { return _product; }
            set
            {
                _product = value;
                Signal();
            }
        }

        private List<ProductSizeDTO> _productSizes;
        public List<ProductSizeDTO> ProductSizes
        {
            get { return _productSizes; }
            set 
            {
                _productSizes = value;
                Signal();
            }
        }

        private ProductSizeDTO _selectedSize;
        public ProductSizeDTO SelectedSize
        {
            get { return _selectedSize; }
            set 
            {
                _selectedSize = value;
                Signal();
            }
        }


        public BitmapImage CurrentImage => GetCurrentImage();

        private List<ProductImageDTO> _images;
        private int _currentIndex;

        public CustomCommand PreviousCommand { get; }
        public CustomCommand NextCommand { get; }

        public CustomCommand UpdateProductCommand { get; }

        public ProductDetailsVM(int product_id)
        {
            PreviousCommand = new CustomCommand(Previous);
            NextCommand = new CustomCommand(Next);
            UpdateProductCommand=new CustomCommand(UpdateProduct);

            LoadData(product_id);
        }
        private async void LoadData(int product_id)
        {
            _images = await UsingService.Instance.GetProductImages(product_id);
            _currentIndex = 0;
            Signal(nameof(CurrentImage));
            Product = await UsingService.Instance.GetProductDetails(product_id);
            ProductSizes=await UsingService.Instance.GetProductSizes(product_id);

        }

        private void UpdateProduct()
        {
            var win = new UpdateProductWin(Product.Id);
            win.ShowDialog();
        }

        private BitmapImage GetCurrentImage()
        {
            if (_images!=null&&_images.Count > 0)
            {
                var currentImage = _images[_currentIndex].Image;
                var bitmap = new BitmapImage();
                using (var stream = new MemoryStream(currentImage))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }
                return bitmap;
            }
            return null;
        }

        private void Previous()
        {
            if (_images.Count > 0)
            {
                _currentIndex = (_currentIndex - 1 + _images.Count) % _images.Count;
                Signal(nameof(CurrentImage));
            }
        }

        private void Next()
        {
            if (_images.Count > 0)
            {
                _currentIndex = (_currentIndex + 1) % _images.Count;
                Signal(nameof(CurrentImage));
            }
        }

    }
}
