using System;
using System.Collections.Generic;
using CsvBankImporterLib.Models;
using CsvBankImporterLib.Service;
using CsvBankImporterLib.Wrappers;

namespace CsvBankImporterLib
{
    public abstract class CsvBankImporter : ICsvBankImporter
    {
        protected readonly IFileWrapper FileWrapper;
        protected readonly ICsvSplitterAndStripper CsvSplitterAndStripper;

        public  string PathICanSupport = "NotImplemented";
        
        protected CsvBankImporter(IFileWrapper fileWrapper,ICsvSplitterAndStripper csvSplitterAndStripper)
        {
            FileWrapper = fileWrapper;
            CsvSplitterAndStripper = csvSplitterAndStripper;
        }

        public virtual IEnumerable<StandardBankOutputModel> Import(string path)
        {
            throw  new NotImplementedException();
        }
    }
}