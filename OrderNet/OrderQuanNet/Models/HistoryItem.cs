namespace OrderQuanNet.Models
{
    public class HistoryItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image_path { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
    }
}
