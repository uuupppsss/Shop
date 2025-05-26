using ShopLib;
using System.Collections.ObjectModel;
using System.Windows;
using WpfClientShop.Model;
using WpfClientShop.Services;
using WpfClientShop.View;

namespace WpfClientShop.ViewModel
{
    public class CartControlVM:BaseVM
    {
        private List<BasketItemDTO> _basket;
        public List<BasketItemDTO> Basket 
        {
            get { return _basket; } 
            set
            {
                _basket = value;
                Signal();
            }
        }

        private decimal _basketCost;
        public decimal BasketCost
        {
            get { return _basketCost; }
            set 
            {
                _basketCost = value;
                Signal();
            }
        }

        public bool IsBasketNotEmpty { get; set; }

        public CustomCommand<BasketItemDTO> IncreaseCommand { get; set; }
        public CustomCommand<BasketItemDTO> DecreaseCommand { get; set; }
        public CustomCommand<BasketItemDTO> RemoveCommand { get; set; }
        public CustomCommand CreateOrderCommand { get; set; }

        public CartControlVM()
        {
            IncreaseCommand = new CustomCommand<BasketItemDTO>(IncreaseCount);
            DecreaseCommand=new CustomCommand<BasketItemDTO>(DecreaseCount);
            CreateOrderCommand=new CustomCommand(GoToCreateOrder);
            RemoveCommand = new CustomCommand<BasketItemDTO>(RemoveItem);
            LoadData();
        }

        private async void LoadData()
        {
            Basket = await ClientService.Instance.GetUserBasket();
            CalculateBasketCost();
            IsBasketNotEmpty = Basket.Count > 0;
            Signal(nameof(IsBasketNotEmpty));
        }

        private void CalculateBasketCost()
        {
            decimal cost=0;
            foreach(var i in Basket)
            {
                cost += i.Cost;
            }
            BasketCost=cost;
        }

        public async void IncreaseCount(BasketItemDTO item)
       {
            int maxCount = await ClientService.Instance.GetBasketItemMaxCount(item.Id);
            if(item.Count<maxCount)
            {
                int index = Basket.IndexOf(item);
                Basket[index].Count++;

                await ClientService.Instance.UpdateBasketItem(Basket[index].Id, Basket[index].Count);
                //Basket = [.. Basket];
                //CalculateBasketCost();
                LoadData();
            }
        }

        public async void DecreaseCount(BasketItemDTO item)
        {
            if(item.Count>1)
            {
                int index = Basket.IndexOf(item);
                Basket[index].Count--;

                await ClientService.Instance.UpdateBasketItem(Basket[index].Id, Basket[index].Count);
                //Basket = [.. Basket];
                //CalculateBasketCost();
                LoadData();
            }
        }

        public async void RemoveItem(BasketItemDTO item)
        {
            await ClientService.Instance.RemoveBasketItem(item.Id);
            Basket.Remove(item);
            Basket = [..Basket];
            CalculateBasketCost();
        }

        public void GoToCreateOrder()
        {
            if(Basket.Count==0)
            {
                MessageBox.Show("Корзина пуста");
                return;
            }
            List<OrderItemDTO> result = new();
            foreach (var item in Basket)
            {
                if(!item.IfNotAbleToOrder)
                {
                    result.Add(new OrderItemDTO
                    {
                        ProductName = item.ProductName,
                        ProductId = item.ProductId,
                        Count = item.Count,
                        Size = item.Size,
                        OrderId = 0
                    });
                }
            }
            var create = new CreateOrderControl(result,BasketCost);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = create;
        }
    }
}
