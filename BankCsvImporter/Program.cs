using System;
using System.Collections.Generic;
using System.Linq;
using CsvBankImporterLib;
using CsvBankImporterLib.Models;
using CsvBankImporterLib.Service;
using CsvBankImporterLib.Wrappers;

namespace BankCsvImporter
{
    // please note this is not a program more a actual test of what you can do as in get rows back show break down etc you could then put in db etc
    // hence why its not tested 
    class Program
    {
        private static CsvBankImporter[] _importers;

        static void Main(string[] args)
        {
            // could use dependency injection like autofac here
            // todo this is expensive maybe parse in args and base it on type name so only activates CsvBankImporterLib that we have a arg for
            _importers = GetNonAbstractClassesOfTypes<CsvBankImporter>(new FileWrapper(), new CsvSplitterAndStripper()).ToArray();

            decimal? lastmont = 0;
            var amounts = new List<decimal?>();
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                var importer = _importers.FirstOrDefault(x => arg.Contains(x.PathICanSupport)); // todo fiddly could be problematic over time
                var items = new List<StandardBankOutputModel>();
                foreach (var file in System.IO.Directory.GetFiles(arg))
                {
                    var part = importer.Import(file).OrderBy(x => x.TransactionDate);
                    items.AddRange(part);
                }

                var byYear = items.GroupBy(x => x.TransactionDate.Year).ToDictionary(x => x.Key,
                    x => x.GroupBy(t => t.TransactionDate.Month).ToDictionary(t => t.Key, t => t.OrderBy(f => f.TransactionDate).FirstOrDefault()));
                foreach (var q in byYear)
                {
                    foreach (var i in q.Value)
                    {
                        var amount = i.Value.Balance - lastmont;
                        Console.WriteLine($"{i.Value.TransactionDate:d}   {i.Value.Balance} {amount}");
                        amounts.Add(amount);
                        lastmont = i.Value.Balance;
                    }
                }

                Console.WriteLine(amounts.Skip(1).Sum());
            }

            Console.ReadLine();
        }


        private static IEnumerable<T> GetNonAbstractClassesOfTypes<T>(params object[] paramArray)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => x.IsClass && x.IsAbstract == false)
                .Select(item => (T) Activator.CreateInstance(item, paramArray));
        }
    }
}