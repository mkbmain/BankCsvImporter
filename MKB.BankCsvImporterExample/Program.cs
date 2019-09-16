using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MKB.BankCsvImporterExample.IoC;
using MKB.CsvBankImporterCore;
using MKB.CsvBankImporterCore.Models;

namespace MKB.BankCsvImporterExample
{
    // please note this is not a program more a actual test of what you can do as in get rows back show break down etc you could then put in db etc
    // hence why its not tested 
    static class Program
    {
        private static CsvBankImporter[] _importers;

        static Program()
        {
            // with autofac
            _importers = GetNonAbstractClassesOfTypes<CsvBankImporter>(GetContainer.GetMeContainer()).ToArray();

            // could optimise this to only load one in real time when a folder with files is discovered but meh
            // with out auto fac
            //  _importers = GetNonAbstractClassesOfTypes<CsvBankImporter>(new FileWrapper(), new CsvSplitterAndStripper()).ToArray();
        }

        static void Main(string[] args)
        {
            decimal? lastmont = 0;
            var amounts = new List<decimal?>();
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                var importer = _importers.FirstOrDefault(x => arg.ToLower().Contains(x.PathICanSupport)); // todo fiddly could be problematic over time
                var items = new List<StandardBankOutputModel>();
                foreach (var file in System.IO.Directory.GetFiles(arg))
                {
                    var part = importer.Import(file).OrderBy(x => x.TransactionDate);
                    items.AddRange(part);
                }

                var byYear = items.GroupBy(item => item.TransactionDate.Year).ToDictionary(item => item.Key,
                    item => item.GroupBy(t => t.TransactionDate.Month)
                        .ToDictionary(t => t.Key, t => t.OrderByDescending(f => f.TransactionDate).FirstOrDefault()));
                foreach (var q in byYear)
                {
                    foreach (var (_, value) in q.Value)
                    {
                        var amount = value.Balance - lastmont;
                        Console.WriteLine($"{value.TransactionDate:d}   {value.Balance} {amount}");
                        amounts.Add(amount);
                        lastmont = value.Balance;
                    }
                }

                Console.WriteLine(amounts.Skip(1).Sum());
            }

            Console.ReadLine();
        }

        // using autoFac
        private static IEnumerable<T> GetNonAbstractClassesOfTypes<T>(IContainer container)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => x.IsClass && x.IsAbstract == false).Select(x => (T) container.Resolve(x));
        }

        // Using Activator
        private static IEnumerable<T> GetNonAbstractClassesOfTypes<T>(params object[] paramArray)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => x.IsClass && x.IsAbstract == false)
                .Select(item => (T) Activator.CreateInstance(item, paramArray));
        }
    }
}