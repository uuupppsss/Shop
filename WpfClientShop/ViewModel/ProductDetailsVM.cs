using ShopLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfClientShop.Model;
using WpfClientShop.Services;
using WpfClientShop.View;

namespace WpfClientShop.ViewModel
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

        private bool _unauthorize;
        public bool Unauthorize
        {
            get { return _unauthorize; }
            set
            {
                _unauthorize = value;
                Signal();
            }
        }

        public CustomCommand PreviousCommand { get; }
        public CustomCommand NextCommand { get; }
        public CustomCommand AddToBasketCommand {  get; }

        private int _productId;

        public ProductDetailsVM(int product_id)
        {
            _productId = product_id;
            PreviousCommand = new CustomCommand(Previous);
            NextCommand = new CustomCommand(Next);
            AddToBasketCommand = new CustomCommand(AddToBasket);

            LoadData();
            Unauthorize = AuthService.Instance.IsAuthorized();

            NoteService.Instance.ProductUpdated += ProductUpdated;
            NoteService.Instance.ProductDeleted += ProductDeleted;
            NoteService.Instance.ProductImagesUpdated += ProductImagesUpdated;
            NoteService.Instance.ProductSizesUpdated += ProductSizesUpdated;
        }
        private async void LoadData()
        {
            _images = await UsingService.Instance.GetProductImages(_productId);
            _currentIndex = 0;
            Signal(nameof(CurrentImage));
            Product = await UsingService.Instance.GetProductDetails(_productId);
            ProductSizes=await UsingService.Instance.GetProductSizes(_productId);

        }

        public void ProductDeleted(int product_id)
        {
            if (_productId == product_id)
            {
                MessageBox.Show("Продукт был удален");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainControl = new MainControl();
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.MainContentControl.Content = mainControl;
                });
            }
        }

        public async void ProductSizesUpdated(int product_id)
        {
            if (_productId == product_id)
            {
                ProductSizes = await UsingService.Instance.GetProductSizes(_productId);
            }
        }

        public async void ProductImagesUpdated(int product_id)
        {
            if (_productId == product_id)
            {
                _images = await UsingService.Instance.GetProductImages(_productId);
                _currentIndex = 0;
            }
        }

        public async void ProductUpdated(int product_id)
        {
            if(_productId==product_id)
            {
                Product = await UsingService.Instance.GetProductDetails(_productId);
            }
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

        private async void AddToBasket()
        {
            if (SelectedSize == null)
            {
                MessageBox.Show("Выберите размер");
                return;
            }
            await ClientService.Instance.AddProductToBasket(Product.Id,SelectedSize.Size);
        }
    }
}
