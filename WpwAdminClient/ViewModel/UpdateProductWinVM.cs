using Microsoft.Win32;
using ShopLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfAdminClient.Model;
using WpfAdminClient.Services;

namespace WpfAdminClient.ViewModel
{
    public class UpdateProductWinVM:BaseVM
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
                //SizeSelectionChanged();
            }
        }

        private string _size;
        public string Size
        {
            get { return _size; }
            set
            {
                _size = value;
                Signal();
            }
        }

        private int _sizeCount;
        public int SizeCount
        {
            get { return _sizeCount; }
            set
            {
                _sizeCount = value;
                Signal();
            }
        }

        private List<ImageDisplay> _images = new List<ImageDisplay>();
        public List<ImageDisplay> Images
        {
            get => _images;
            set
            {
                _images = value;
                Signal();
            }
        }

        private int _productId;

        public CustomCommand AddNewSize { get; }
        public CustomCommand UpdateSizeCommand { get; }
        public CustomCommand CancelChanges {  get; }
        public CustomCommand SaveCommand { get; }
        public CustomCommand UpdateImagesCommand { get; }
        public CustomCommand ClearImagesCommand { get; }

        public UpdateProductWinVM(int product_id)
        {
            _productId = product_id;
            AddNewSize = new CustomCommand(AddSize);
            UpdateSizeCommand = new CustomCommand(SizeSelectionChanged);
            CancelChanges = new CustomCommand(CanselSizeChanges);
            SaveCommand=new CustomCommand(SaveChanges);
            UpdateImagesCommand=new CustomCommand(UpdateImages);
            ClearImagesCommand=new CustomCommand(ClearImages);
            LoadData(); 
        }

        private async void LoadData()
        {
            Product=await UsingService.Instance.GetProductDetails(_productId);
            ProductSizes = await UsingService.Instance.GetProductSizes(_productId);
           
        }

        private void ClearImages()
        {
            Images = new();
        }

        private void UpdateImages()
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
                        Data = imageBytes
                    });
                    Images = [.. Images];
                }
            }
        }

        private void AddSize()
        {
            if (string.IsNullOrWhiteSpace(Size)) return;
           if(SelectedSize==null)
            {
                ProductSizeDTO size = new ProductSizeDTO
                {
                    Size = Size,
                    Count = SizeCount,
                    ProductId = 0
                };
                ProductSizes.Add(size);
            }
            else
            {
                UpdateSize();
            }

            ProductSizes = [.. ProductSizes];
            Size = string.Empty;
            SizeCount = 0;
        }

        private void UpdateSize()
        {
            if(!string.IsNullOrWhiteSpace(Size))
            {
                int index=ProductSizes.IndexOf(SelectedSize);
                ProductSizes[index].Size = Size;
                ProductSizes[index].Count = SizeCount;

            }
        }

        private void SizeSelectionChanged()
        {
            if(SelectedSize!=null)
            {
                Size = SelectedSize.Size;
                SizeCount = SelectedSize.Count;
            }
        }

        private void CanselSizeChanges()
        {
            Size = string.Empty;
            SizeCount = 0;
            SelectedSize = null;
        }

        private async void SaveChanges()
        {
            var byteImages = new List<byte[]>();
            if(Images?.Count!=0)
            {
                byteImages = Images.Select(i => i.Data).ToList();
            }
            await AdminService.Instance.UpdateProduct(Product, ProductSizes,byteImages);
        }

    }
}
