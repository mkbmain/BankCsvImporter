using System.Collections.Generic;

namespace MKB.CsvBankImporterCore.Service
{
    public interface ICsvSplitterAndStripper
    {
        IEnumerable<string> GetPartsOfLine(string line);
    }
}