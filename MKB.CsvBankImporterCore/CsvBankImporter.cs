using System;
using System.Collections.Generic;
using MKB.CsvBankImporterCore.Models;
using MKB.CsvBankImporterCore.Service;
using MKB.CsvBankImporterCore.Wrappers;

namespace MKB.CsvBankImporterCore
{
    public abstract class CsvBankImporter : ICsvBankImporter
    {
        protected readonly ICsvSplitterAndStripper CsvSplitterAndStripper;
        protected readonly IFileWrapper FileWrapper;

        public string PathICanSupport = "NotImplemented";

        protected CsvBankImporter(IFileWrapper fileWrapper, ICsvSplitterAndStripper csvSplitterAndStripper)
        {
            FileWrapper = fileWrapper;
            CsvSplitterAndStripper = csvSplitterAndStripper;
        }

        public virtual IEnumerable<StandardBankOutputModel> Import(string path)
        {
            throw new NotImplementedException();
        }
    }
}