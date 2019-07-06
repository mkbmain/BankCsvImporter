using System.Collections.Generic;
using System.Linq;
using CsvBankImporterLib.Service;
using Shouldly;
using Xunit;

namespace CsvBankImporterLibTests
{
    public class CsvSpliiterandStripperTests
    {
        //  COULD PROBS DO A THEORY HERE AND JUST PARSE DIFFRENT STRINGS IN VIA A PARAM BUT I LIKE HAVING A SEPERATE METHOD NAME TO SHOW WHAT EACH IS TESTING
        [Fact]
        public void Ensure_we_split_lines_correctly()
        {
            var details = $@"31/12/2018,D/D,'RandomShop Payment,-3.99,43703.30,'tony M,'001631-00541111,";
            var sut = new CsvSplitterAndStripper();

            var result = sut.GetPartsOfLine(details);
            
            result.Count().ShouldBe(7);
            var collection = result as List<string> ?? result.ToList();
            collection[0].ShouldBe("31/12/2018");
            collection[1].ShouldBe("D/D");
            collection[2].ShouldBe("'RandomShop Payment");
            collection[3].ShouldBe("-3.99");
            collection[4].ShouldBe("43703.30");
            collection[5].ShouldBe("'tony M");
            collection[6].ShouldBe("'001631-00541111");
        }
        [Fact]
        public void Ensure_we_strip_quotes()
        {
            var details = $@"31/12/2018,D/D,""'RandomShop Payment"",-3.99,43703.30,'tony M,'001631-00541111,";

            var sut = new CsvSplitterAndStripper();

            var result = sut.GetPartsOfLine(details);
            
            result.Count().ShouldBe(7);
            var collection = result as List<string> ?? result.ToList();
            collection[0].ShouldBe("31/12/2018");
            collection[1].ShouldBe("D/D");
            collection[2].ShouldBe("'RandomShop Payment");
            collection[3].ShouldBe("-3.99");
            collection[4].ShouldBe("43703.30");
            collection[5].ShouldBe("'tony M");
            collection[6].ShouldBe("'001631-00541111");
        }
        [Fact]
        public void Ensure_if_there_is_a_comma_inside_a_quote_we_ignore_it_and_do_not_Return_it()
        {
            var details = $@"31/12/2018,D/D,""'RandomShop,,,,, Payment"",-3.99,43703.30,'tony M,'001631-00541111,";
            var sut = new CsvSplitterAndStripper();

            var result = sut.GetPartsOfLine(details);
            
            result.Count().ShouldBe(7);
            var collection = result as List<string> ?? result.ToList();
            collection[0].ShouldBe("31/12/2018");
            collection[1].ShouldBe("D/D");
            collection[2].ShouldBe("'RandomShop Payment");
            collection[3].ShouldBe("-3.99");
            collection[4].ShouldBe("43703.30");
            collection[5].ShouldBe("'tony M");
            collection[6].ShouldBe("'001631-00541111");
        }
    }
}