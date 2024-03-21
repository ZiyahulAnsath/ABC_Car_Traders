using System;
using System.Collections.Generic;
using System.Data;
using ABC_Car_Traders.Models;
using ABC_orderItem_Traders.DataAccess;

namespace ABC_Car_Traders.Controllers
{
    public class OrderController
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(string connectionString)
        {
            _orderRepository = new OrderRepository(connectionString);
        }

        public void RegisterOrderItem(OrderItem orderItem)
        {
            _orderRepository.PlaceOrderItem(orderItem);
        }

        public List<OrderItem> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public List<OrderItem> GetSingleOrders()
        {
            return _orderRepository.GetSingleOrders(7);
        }
    }
}
