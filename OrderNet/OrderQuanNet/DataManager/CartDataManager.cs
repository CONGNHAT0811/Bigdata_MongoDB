using MongoDB.Bson;
using OrderQuanNet.Services;

namespace OrderQuanNet.DataManager
{
    public static class CartDataManager
    {
        public static Dictionary<ObjectId, int> cartItems = new Dictionary<ObjectId, int>();

        public static void addItem(ObjectId item, int amount = 1)
        {
            if (cartItems.ContainsKey(item))
                cartItems[item] += amount;
            else
                cartItems.Add(item, amount);
        }
        public static void removeItem(ObjectId item)
        {
            if (cartItems.ContainsKey(item)) cartItems.Remove(item);
        }

        public static int getTotalItems()
        {
            return cartItems.Values.Sum();
        }

        public static decimal getTotalPrice()
        {
            ProductsService productsService = new ProductsService();
            return (decimal)cartItems.Sum(item => productsService.SelectById(item.Key).price * item.Value);
        }
    }
}
