using Microsoft.Win32;
using ShopLib;
using System.IO;
using System.Windows;
using WpfAdminClient.Model;
using WpfAdminClient.Services;

namespace WpfAdminClient.ViewModel
{
    public class AddProductVM:BaseVM
    {
        private string _title;
        private string _description;
        private decimal _price;
        private List<ImageDisplay> _images = new List<ImageDisplay>();

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

        public List<ImageDisplay> Images
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

        private string _brandTitle;
        public string BrandTitle
        {
            get { return _brandTitle; }
            set 
            {
                _brandTitle = value;
                Signal();
            }
        }

        private string _typeTitle;
        public string TypeTitle
        {
            get { return _typeTitle; }
            set
            {
                _typeTitle = value;
                Signal();
            }
        }

        private BrandDTO _brandToDrop;
        public BrandDTO BrandToDrop
        {
            get { return _brandToDrop; }
            set 
            {
                _brandToDrop = value; 
                Signal() ;
            }
        }

        private ProductTypeDTO _typeToDrop;
        public ProductTypeDTO TypeToDrop
        {
            get { return _typeToDrop; }
            set 
            {
                _typeToDrop = value;
                Signal();
            }
        }



        public CustomCommand AddImageCommand { get; }
        public CustomCommand SaveCommand { get; }
        public CustomCommand AddTypeCommand { get; }
        public CustomCommand AddBrandCommand { get; }
        public CustomCommand RemoveTypeCommand {  get; }
        public CustomCommand RemoveBrandCommand { get; }

        public AddProductVM()
        {
            AddImageCommand = new CustomCommand(AddImage);
            SaveCommand = new CustomCommand(SaveProduct);
            AddTypeCommand = new CustomCommand(SaveType);
            AddBrandCommand = new CustomCommand(SaveBrand);

            RemoveBrandCommand=new CustomCommand(RemoveBrand);
            RemoveTypeCommand = new CustomCommand(RemoveType);
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
                    Images.Add(new ImageDisplay
                    {
                        Name = file,
                        Data= imageBytes
                    });
                    Images = [..Images];
                }
            }
        }

        private async void SaveProduct()
        {

            if (!string.IsNullOrWhiteSpace(Title) &&
                !string.IsNullOrWhiteSpace(Description) &&
                Price > 0 && SelectedBrand != null && SelectedType != null&&Images?.Count!=null)
            {
                await AdminService.Instance.AddNewProduct(new ProductDTO
                {
                    Title = Title,
                    Description = Description,
                    Price = Price,
                    TypeId = SelectedType.Id,
                    BrandId = SelectedBrand.Id,
                },
                Images.Select(i=>i.Data).ToList());
                return;
            }
            else MessageBox.Show("Заполните все поля");

            
        }

        private async void LoadData()
        {
            Brands = await UsingService.Instance.GetBrandsList();
            TypesList = await UsingService.Instance.GetTypesList();
            
            NoteService.Instance.TypesCollectionChanged+= 
                async ()=> TypesList = await UsingService.Instance.GetTypesList();
            NoteService.Instance.BrandsCollectionChanged +=
                async () => Brands = await UsingService.Instance.GetBrandsList();
        }

        private async void SaveType()
        {
            if(!string.IsNullOrWhiteSpace(TypeTitle))
            {
                await AdminService.Instance.AddNewCategory(TypeTitle);
                TypeTitle = string.Empty;
            }
        }

        private async void SaveBrand()
        {
            if (!string.IsNullOrWhiteSpace(BrandTitle))
            {
                await AdminService.Instance.AddNewBrand(BrandTitle);
                BrandTitle = string.Empty;
            }
        }

        private async void RemoveType()
        {
            if (TypeToDrop != null)
            {
                var result = MessageBox.Show
                    ($"Удалить категорию {TypeToDrop.Title}?", "Все товары этой категории так же будут удалены", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await AdminService.Instance.RemoveCategory(TypeToDrop.Id);
                }
            }
            else MessageBox.Show("Выберите категорию");
        }

        private async void RemoveBrand()
        {
            if (BrandToDrop != null)
            {
                var result = MessageBox.Show
                    ($"Удалить категорию {BrandToDrop.Title}?", "Все товары этой категории так же будут удалены", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await AdminService.Instance.RemoveBrand(BrandToDrop.Id);
                }
            }
            else MessageBox.Show("Выберите бренд");
        }
    }
}
