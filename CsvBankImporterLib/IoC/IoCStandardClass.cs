using System;
using System.Collections.Generic;
using CsvBankImporterLib.Service;
using CsvBankImporterLib.Wrappers;

namespace CsvBankImporterLib.IoC
{
    public class IoCStandardClass
    {
        public static List<Type> TypesAsImplementedInterface = new List<Type>
        {
            typeof(CsvSplitterAndStripper),
            typeof(FileWrapper)
        };
    }
}