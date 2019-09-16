using Autofac;

namespace MKB.BankCsvImporterExample.IoC
{
    public static class GetContainer
    {
        public static IContainer GetMeContainer()
        {
            var container = new ContainerBuilder();
            container.RegisterModule(new CsvBankImporterLibModule());
            return container.Build();
        }
    }
}