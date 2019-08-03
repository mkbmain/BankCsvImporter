using Autofac;
using CsvBankImporterLib;
using CsvBankImporterLib.Service;
using CsvBankImporterLib.Wrappers;

namespace BankCsvImporter.IoC
{
    public class CsvBankImporterLibModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvSplitterAndStripper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FileWrapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<NatwestCsvBankImporter>().AsSelf().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}