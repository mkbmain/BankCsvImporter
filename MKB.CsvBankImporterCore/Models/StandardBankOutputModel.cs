using System;

namespace MKB.CsvBankImporterCore.Models
{
    public class StandardBankOutputModel
    {
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? Balance { get; set; }
        public string Description { get; set; }

        public bool TransactionIsCredit { get; set; }
    }
}