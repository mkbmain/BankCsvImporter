using System.Collections.Generic;

namespace CsvBankImporterLib.Service
{
    public interface ICsvSplitterAndStripper
    {
        IEnumerable<string> GetPartsOfLine(string line);
    }
}