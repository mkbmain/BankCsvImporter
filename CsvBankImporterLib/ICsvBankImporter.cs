using System.Collections.Generic;
using CsvBankImporterLib.Models;

namespace CsvBankImporterLib
{
    public interface ICsvBankImporter
    {
        IEnumerable<StandardBankOutputModel> Import(string path);
    }
}