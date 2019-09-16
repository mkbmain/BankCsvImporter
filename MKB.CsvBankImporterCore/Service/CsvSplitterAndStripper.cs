using System.Collections.Generic;
using System.Text;

namespace MKB.CsvBankImporterCore.Service
{
    public class CsvSplitterAndStripper : ICsvSplitterAndStripper
    {
        public IEnumerable<string> GetPartsOfLine(string line)
        {
            var quote = false;
            var part = new List<string>();
            var stringBuilder = new StringBuilder();
            foreach (var c in line)
            {
                if (c == '"')
                {
                    // i want these stripped
                    quote = !quote;
                    continue;
                }

                if (c == ',')
                {
                    if (quote)
                    {
                        continue; // skip these as there commas inside " meaning there not seperators
                    }

                    part.Add(stringBuilder.ToString());
                    stringBuilder.Clear();
                    continue;
                }

                stringBuilder.Append(c);
            }

            return part;
        }
    }
}