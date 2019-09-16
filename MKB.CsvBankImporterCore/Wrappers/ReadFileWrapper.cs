using System.Collections.Generic;

namespace MKB.CsvBankImporterCore.Wrappers
{
    // TODO INTEGRATION TESTS
    public class FileWrapper : IFileWrapper
    {
        public bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        public IEnumerable<string> ReadFile(string path)
        {
            return System.IO.File.ReadAllLines(path);
        }
    }
}