using Autofac;

namespace BankCsvImporter.IoC
{
    public class GetContainer
    {
        public static IContainer GetMeContainer()
        {
            var container = new ContainerBuilder();
            container.RegisterModule(new CsvBankImporterLibModule());
            return container.Build();
        }
    }
}