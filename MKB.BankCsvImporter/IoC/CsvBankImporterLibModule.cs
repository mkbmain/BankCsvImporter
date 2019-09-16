using Autofac;
using MKB.CsvBankImporterCore.Service;
using MKB.CsvBankImporterCore.Wrappers;

namespace MKB.BankCsvImporterExample.IoC
{
    public class CsvBankImporterLibModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
//            var toRegister = MKB.NatwestCsvBankImporter.IoC.IoCStandardClass.TypesAsImplementedInterface();
//
//            foreach (var r in toRegister)
//            {
//                builder.RegisterType(r).AsImplementedInterfaces().InstancePerMatchingLifetimeScope();
//            }

            builder.RegisterType<CsvSplitterAndStripper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FileWrapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<NatwestCsvBankImporter.NatwestCsvBankImporter>().AsSelf().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}