namespace FIFO_Infineon.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime EntryDate { get; set; }
        public string? MasterItemID { get; set; }
        public MasterItem? MasterItem { get; set; }
    }
}