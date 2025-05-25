using OrderQuanNet.Models;

namespace OrderQuanNet.DataManager
{
    public static class OrderDataManager
    {
        private static List<OrdersModel> _Orders = new List<OrdersModel>();

        public static List<OrdersModel> Orders
        {
            get { return _Orders; }
            set { _Orders = value; }
        }

        public static void UpdateOrder(OrdersModel Order)
        {
            if (Order == null) throw new ArgumentNullException(nameof(Order), "Order cannot be null.");
            var id = _Orders.FindIndex(p => p._id == Order._id);
            if (id != -1) _Orders[id] = Order;
        }
    }
}
