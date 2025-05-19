using ShopLib;
using System.Windows;
using WpfAdminClient.Convert;
using WpfAdminClient.Model;
using WpfAdminClient.Services;
using WpfAdminClient.View;
using WpwAdminClient.View;

namespace WpfAdminClient.ViewModel
{
    public class MainControlVM:BaseVM
    {
        private UsingService _usingService;

        private List<ProductDTO> _products;

        public List<ProductDTO> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                Signal();
            }
        }

        private List<ProductDisplay> _productsView;

        public List<ProductDisplay> ProductsView
        {
            get { return _productsView; }
            set 
            {
                _productsView = value; 
                Signal() ;
            }
        }


        private List<BrandDTO> _brands;

        public List<BrandDTO> Brands
        {
            get { return _brands; }
            set
            {
                _brands = value;
                Signal();
            }
        }

        private List<ProductTypeDTO> _typesList;

        public List<ProductTypeDTO> TypesList
        {
            get { return _typesList; }
            set
            {
                _typesList = value;
                Signal();
            }
        }

        private ProductTypeDTO _selectedType;

        public ProductTypeDTO SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                Signal();
            }
        }

        private BrandDTO _selectedBrand;

        public BrandDTO SelectedBrand
        {
            get { return _selectedBrand; }
            set
            {
                _selectedBrand = value;
                Signal();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                Signal();
            }
        }

        public CustomCommand<ProductDisplay> ShowProductDetailsCommand { get; set; }

        public MainControlVM()
        {
            LoadData();
            ShowProductDetailsCommand = new CustomCommand<ProductDisplay>(ShowProductDetails);
        }

        private async void LoadData()
        {
            _usingService = UsingService.Instance;

            Products = await _usingService.GetProductsList();
            Brands = await _usingService.GetBrandsList();
            TypesList = await _usingService.GetTypesList();
            ProductsView= ProductConverter.Instance.ConvertToProductDisplay(Products);
        }

        private void ShowProductDetails(ProductDisplay product)
        {
            var detailsControl = new ProductDetailsControl(product.Id);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = detailsControl; 
        }
    }
}
