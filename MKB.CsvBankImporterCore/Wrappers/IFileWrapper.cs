using System.Collections.Generic;

namespace MKB.CsvBankImporterCore.Wrappers
{
    public interface IFileWrapper
    {
        bool FileExists(string filePath);

        IEnumerable<string> ReadFile(string path);
    }
}