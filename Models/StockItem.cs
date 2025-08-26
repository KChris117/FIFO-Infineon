namespace FIFO_Infineon.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public int Jumlah { get; set; }
        public DateTime TanggalMasuk { get; set; }
        public string? MasterItemID { get; set; }
        public MasterItem? MasterItem { get; set; }
    }
}