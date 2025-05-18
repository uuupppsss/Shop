using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientShop.Services;

namespace WpfClientShop.ViewModel
{
    public class MainWindowVM:BaseVM
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

		private int _selectedTypeId;

		public int SelectedTypeId
		{
			get { return _selectedTypeId; }
			set 
			{
				_selectedTypeId = value;
				Signal();
			}
		}

		private int _selectedBrandId;

		public int SelectedBrandId
		{
			get { return _selectedBrandId; }
			set 
			{
				_selectedBrandId = value; 
				Signal() ;
			}
		}


		public MainWindowVM()
        {
			LoadData();
		}

		private async void LoadData()
		{
			_usingService=UsingService.Instance;

			Products = await _usingService.GetProductsList();
			Brands = await _usingService.GetBrandsList();
			TypesList= await _usingService.GetTypesList();
		}
    }
}
