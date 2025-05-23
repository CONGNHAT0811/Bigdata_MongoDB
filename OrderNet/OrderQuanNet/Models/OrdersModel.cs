using OrderQuanNet.Services;

namespace OrderQuanNet.Models
{
    public class OrdersModel
    {
        internal object date;

        public int? id { get; set; }
        public int? users_id { get; set; }
        public int? product_id { get; set; }
        public int? amount { get; set; }
        public int? price { get; set; }
        public string? status { get; set; }
        public string? created_at { get; set; }
        public string? updated_at { get; set; }

        public void create()
        {
            this.id = null;
            OrdersService ordersService = new OrdersService();
            ordersService.Insert(this);
        }

        public void save()
        {
            OrdersService ordersService = new OrdersService();
            ordersService.Update(this);
        }

        public void delete()
        {
            OrdersService ordersService = new OrdersService();
            ordersService.Delete(this);
        }
    }
}
