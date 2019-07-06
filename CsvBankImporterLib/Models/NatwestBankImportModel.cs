namespace CsvBankImporterLib.Models
{
    public class NatwestBankImportModel : StandardBankOutputModel
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Type { get; set; }
    }
}