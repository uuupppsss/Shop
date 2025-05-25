using ShopLib;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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

        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                Signal();
            }
        }

        private List<ProductImageDTO> _images;
        private int _currentIndex;

        public CustomCommand PreviousCommand { get; }
        public CustomCommand NextCommand { get; }

        public CustomCommand UpdateProductCommand { get; }
        public CustomCommand RemoveProductCommand {  get; }

        private int _productId;
        public ProductDetailsVM(int product_id)
        {
            _productId=product_id;
            PreviousCommand = new CustomCommand(Previous);
            NextCommand = new CustomCommand(Next);
            UpdateProductCommand=new CustomCommand(UpdateProduct);
            RemoveProductCommand = new CustomCommand(RemoveProduct);

            LoadData();
            NoteService.Instance.ProductUpdated += ProductUpdated;
            NoteService.Instance.ProductImagesUpdated += ProductImagesUpdated;
            NoteService.Instance.ProductSizesUpdated += ProductSizesUpdated;
        }
        private async void LoadData()
        {
            _images = await UsingService.Instance.GetProductImages(_productId);
            _currentIndex = 0;
            CurrentImage=GetCurrentImage();
            Product = await UsingService.Instance.GetProductDetails(_productId);
            ProductSizes=await UsingService.Instance.GetProductSizes(_productId);

        }

        private async void RemoveProduct()
        {
            
            var result = MessageBox.Show("Удалить продукт?", "Подтвердите действие", MessageBoxButton.YesNo);
            if(result==MessageBoxResult.Yes)
            {
                await AdminService.Instance.RemoveProduct(_productId);
    
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainControl = new MainControl();
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.MainContentControl.Content = mainControl;
                });

                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    var mainControl = new MainControl();
                //    var mainWindow = Application.Current.MainWindow as MainWindow;
                //    mainWindow.MainContentControl.Content = mainControl;// Теперь создаётся в UI-потоке
                //});

            }
        }

        private async void ProductUpdated(int product_id)
        {
            if(_productId==product_id)
            {
                Product = await UsingService.Instance.GetProductDetails(_productId);
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
                CurrentImage=GetCurrentImage();
            }
        }

        private void Next()
        {
            if (_images.Count > 0)
            {
                _currentIndex = (_currentIndex + 1) % _images.Count;
                CurrentImage = GetCurrentImage();
            }
        }

    }
}
