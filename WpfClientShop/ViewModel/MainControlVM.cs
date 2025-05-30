﻿using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfClientShop.Convert;
using WpfClientShop.Model;
using WpfClientShop.Services;
using WpfClientShop.View;

namespace WpfClientShop.ViewModel
{
    public class MainControlVM : BaseVM
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
                UpdateData();
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
                UpdateData();
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
                UpdateData();
            }
        }


        public CustomCommand<ProductDisplay> ShowProductDetailsCommand { get; set; }

        public MainControlVM()
        {
            LoadData();
            ShowProductDetailsCommand = new CustomCommand<ProductDisplay>(ShowProductDetails);
        }

        private void LoadData()
        {
            _usingService = UsingService.Instance;

            LoadTypes();
            LoadBrands();

            NoteService.Instance.TypesUpdated += LoadTypes;
            NoteService.Instance.BrandsUpdated += LoadBrands;
            NoteService.Instance.ProductDeleted += ProductDeleted;
        }

        public void ProductDeleted(int product_id)
        {
            var product = ProductsView.FirstOrDefault(p=>p.Id==product_id);
            if (product!=null)
            {
                ProductsView.Remove(product);
                Signal(nameof(ProductsView));
            }
        }

        private void ShowProductDetails(ProductDisplay product)
        {
            var detailsControl = new ProductDetailsControl(product.Id);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = detailsControl;
        }

        private async void LoadTypes()
        {
            TypesList = await _usingService.GetTypesList();
            TypesList.Insert(0, new ProductTypeDTO { Id = 0, Title = "Все" });
            SelectedType = TypesList[0];
        }

        private async void LoadBrands()
        {
            Brands = await _usingService.GetBrandsList();
            Brands.Insert(0, new BrandDTO { Id = 0, Title = "Все" });
            SelectedBrand = Brands[0];
        }

        private async void UpdateData()
        {
            if (SelectedType != null && SelectedBrand != null)
            {
                Products = await _usingService.GetProductsList(SearchText, SelectedType.Id, SelectedBrand.Id);
                ProductsView = ProductConverter.Instance.ConvertToProductDisplay(Products);
            }
        }
    }
}
