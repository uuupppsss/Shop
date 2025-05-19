using Microsoft.Win32;
using ShopLib;
using System.IO;
using WpfAdminClient.Model;
using WpfAdminClient.Services;

namespace WpfAdminClient.ViewModel
{
    public class AddProductVM:BaseVM
    {
        private string _title;
        private string _description;
        private decimal _price;
        private List<byte[]> _images = new List<byte[]>();

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Signal();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                Signal();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                Signal();
            }
        }

        public List<byte[]> Images
        {
            get => _images;
            set
            {
                _images = value;
                Signal();
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


        public CustomCommand AddImageCommand { get; }
        public CustomCommand SaveCommand { get; }

        public AddProductVM()
        {
            AddImageCommand = new CustomCommand(AddImage);
            SaveCommand = new CustomCommand(SaveProduct);
            LoadData();
        }

        private void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    byte[] imageBytes = File.ReadAllBytes(file);
                    Images.Add(imageBytes);
                }
            }
        }

        private async void SaveProduct()
        {

            if (!string.IsNullOrWhiteSpace(Title) ||
                !string.IsNullOrWhiteSpace(Description) ||
                Price <= 0)
            {
                await AdminService.Instance.AddNewProduct(new ProductDTO
                {
                    Title = Title,
                    Description = Description,
                    Price = Price,
                    TypeId=SelectedType.Id,
                    BrandId = SelectedBrand.Id,
                }, 
                Images);
                return;
            }

            
        }

        private async void LoadData()
        {
            Brands = await UsingService.Instance.GetBrandsList();
            TypesList = await UsingService.Instance.GetTypesList();
            
        }
    }
}
