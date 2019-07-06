using System.Collections.Generic;

namespace CsvBankImporterLib.Wrappers
{
    public interface IFileWrapper
    {
        bool FileExists(string filePath);

        IEnumerable<string> ReadFile(string path);
    }
}