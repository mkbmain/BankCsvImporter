using System;
using System.IO;
using System.Linq;
using CsvBankImporterLib;
using CsvBankImporterLib.Service;
using CsvBankImporterLib.Wrappers;
using Moq;
using Shouldly;
using Xunit;

namespace CsvBankImporterLibTests
{
    public class NatwestCsvBankImporterTests
    {
        [Fact]
        public void Ensure_if_file_path_is_invalid_we_throw_arguement_exception()
        {
            const string filePath = "badFile";
            var fileWrapper = new Mock<IFileWrapper>();
            fileWrapper.Setup(f => f.FileExists(filePath)).Returns(false);
            var sut = new NatwestCsvBankImporter(fileWrapper.Object, null);
            var result = Should.Throw<ArgumentException>(() => sut.Import(filePath));

            result.Message.ShouldContain("file path not found");
        }

        [Fact]
        public void Ensure_if_file_path_is_valid_but_readfile_returns_null_we_throw_InvalidDataException()
        {
            const string filePath = "badFile";
            var fileWrapper = new Mock<IFileWrapper>();
            fileWrapper.Setup(f => f.FileExists(filePath)).Returns(true);
            fileWrapper.Setup(f => f.ReadFile(filePath)).Returns((string[]) null);
            var sut = new NatwestCsvBankImporter(fileWrapper.Object, null);
            var result = Should.Throw<InvalidDataException>(() => sut.Import(filePath));
            result.Message.ShouldContain("file found but no data was found");
        }

        [Fact]
        public void Ensure_if_file_path_is_valid_but_readfile_returns_empty_collection_we_throw_InvalidDataException()
        {
            const string filePath = "badFile";
            var fileWrapper = new Mock<IFileWrapper>();
            fileWrapper.Setup(f => f.FileExists(filePath)).Returns(true);
            fileWrapper.Setup(f => f.ReadFile(filePath)).Returns(new string[] { });
            var sut = new NatwestCsvBankImporter(fileWrapper.Object, null);
            var result = Should.Throw<InvalidDataException>(() => sut.Import(filePath));

            result.Message.ShouldContain("file found but no data was found");
        }

        [Fact]
        public void Ensure_if_file_wrapper_throws_error_we_bubble_up_and_do_not_catch_it()
        {
            const string filePath = "badFile";
            var fileWrapper = new Mock<IFileWrapper>();
            var exception = new Exception("gag");
            fileWrapper.Setup(f => f.FileExists(filePath)).Throws(exception);
            var sut = new NatwestCsvBankImporter(fileWrapper.Object, null);
            var result = Should.Throw<Exception>(() => sut.Import(filePath));

            result.ShouldBe(exception);
        }

        [Fact]
        public void Ensure_data_parsed_back_returns_a_correct_model()
        {
            var details =
                $@"Date, Type, Description, Value, Balance, Account Name, Account Number{Environment.NewLine}31/12/2018,D/D,""'RandomShop Payment"",-3.99,43703.30,""'tony M"",""'001631-00541111"",";

            const string filePath = "goodFile";
            var fileWrapper = new Mock<IFileWrapper>();
            var csvSplitter = new Mock<ICsvSplitterAndStripper>();
            fileWrapper.Setup(f => f.FileExists(filePath)).Returns(true);
            fileWrapper.Setup(f => f.ReadFile(filePath)).Returns(details.Split(Environment.NewLine));
            var collection = new[]
            {
                "31/12/2018",
                "D/D",
                "'RandomShop Payment",
                "-3.99",
                "43703.30",
                "'tony M",
                "'001631-00541111"
            };
            csvSplitter.Setup(f => f.GetPartsOfLine(@"31/12/2018,D/D,""'RandomShop Payment"",-3.99,43703.30,""'tony M"",""'001631-00541111"","))
                .Returns(collection);

            var sut = new NatwestCsvBankImporter(fileWrapper.Object, csvSplitter.Object);
            var result = sut.Import(filePath);
            csvSplitter.Verify(f => f.GetPartsOfLine(@"31/12/2018,D/D,""'RandomShop Payment"",-3.99,43703.30,""'tony M"",""'001631-00541111"","), Times.Once());
            result.Count().ShouldBe(1);
            result.FirstOrDefault().Balance.ShouldBe(43703.30m);
            result.FirstOrDefault().TransactionAmount.ShouldBe(3.99m);
            result.FirstOrDefault().TransactionIsCredit.ShouldBe(false);
            result.FirstOrDefault().TransactionDate.ShouldBeEquivalentTo(new DateTime(2018, 12, 31));
            result.FirstOrDefault().Description.ShouldBe("RandomShop Payment");
        }
    }
}