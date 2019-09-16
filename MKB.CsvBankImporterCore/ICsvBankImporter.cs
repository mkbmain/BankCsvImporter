using System.Collections.Generic;
using MKB.CsvBankImporterCore.Models;

namespace MKB.CsvBankImporterCore
{
    public interface ICsvBankImporter
    {
        IEnumerable<StandardBankOutputModel> Import(string path);
    }
}