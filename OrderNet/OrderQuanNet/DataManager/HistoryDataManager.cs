using OrderQuanNet.Models;
using OrderQuanNet.Services;

namespace OrderQuanNet.DataManager
{
    public static class HistoryDataManager
    {
        public static List<OrdersModel> OrdersHistory = new List<OrdersModel>();
        public static readonly Dictionary<string, string> StatusMappings = new Dictionary<string, string>
        {
            { "WAITING", "Đang chờ xác nhận" },
            { "PROCCESSING", "Đang xử lý" },
            { "DONE", "Đã hoàn thành" }
        };

        public static void LoadHistory()
        {
            OrdersHistory.Clear();

            OrdersService ordersService = new OrdersService();
            List<OrdersModel> orders = ordersService.SelectAll();
            foreach (var item in orders)
            {
                item.status = StatusMappings[item.status];
                OrdersHistory.Add(item);
            }
        }

    }
}
