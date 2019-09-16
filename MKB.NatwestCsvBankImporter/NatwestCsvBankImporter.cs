using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MKB.CsvBankImporterCore;
using MKB.CsvBankImporterCore.Models;
using MKB.CsvBankImporterCore.Service;
using MKB.CsvBankImporterCore.Wrappers;

namespace MKB.NatwestCsvBankImporter
{
    public class NatwestCsvBankImporter : CsvBankImporter
    {
        public NatwestCsvBankImporter(IFileWrapper fileWrapper, ICsvSplitterAndStripper csvSplitterAndStripper) : base(fileWrapper, csvSplitterAndStripper)
        {
            PathICanSupport = "natwest";
        }

        public override IEnumerable<StandardBankOutputModel> Import(string path)
        {
            if (!FileWrapper.FileExists(path))
            {
                throw new ArgumentException("file path not found");
            }

            var lines = FileWrapper.ReadFile(path);

            if (lines == null || !lines.Any())
            {
                throw new InvalidDataException("file found but no data was found");
            }

            var enumerable = lines as string[] ?? lines.ToArray();
            var outputList = new List<NatwestBankImportModel>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var line in enumerable.Where(x => !x.Contains("Date, Type, Description,")))
            {
                var parts = CsvSplitterAndStripper.GetPartsOfLine(line);

                var collection = parts as string[] ?? parts.ToArray();
                var amount = decimal.Parse(collection[3]);
                // try parse might be nice but if it fails would we want to continue and skip a row ??? that could be worse than just throwing a exception to discuss with client before implementing
                outputList.Add(new NatwestBankImportModel
                {
                    TransactionDate = DateTime.Parse(collection[0]),
                    Type = collection[1],
                    Description = collection[2].TrimStart('\''),
                    // remove ' at start is in here and not stripper as for natest  they add them to start of all descriptions specific to natwest so removed in the natwest implementation
                    TransactionAmount = amount < 0 ? amount * -1 : amount,
                    TransactionIsCredit = amount >= 0,
                    Balance = decimal.Parse(collection[4]),
                    AccountName = collection[5],
                    AccountNumber = collection[6],
                });
            }

            return outputList;
        }
    }
}