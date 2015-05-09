using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay
{
    class Program
    {
        static void Main(string[] args) {
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(@"
set himmel sky

tell 'I could only take your hand' $newline ''
tell 'and lead you to the crossing road where we first met,' $newline ''
tell 'we’d take a skip together like the good old days' $newline ''
tell 'and laugh beneath a brilliantly blue ' $himmel $newline ''
")));

            var vars = new Dictionary<string, object> {{"$newline", "\n\r"}};

            foreach (var lineArray in Lexer.Lex(stream).Select(line => line.ToArray())) {
                switch(lineArray[0]) {
                    case "tell":
                        foreach (var argument in Enumerable.Range(1, lineArray.Count() - 1).Select(i => lineArray[i])) {
                            Console.Write(argument[0] == '$' ? vars[argument] : argument.Replace("\r", "")); //ignore \r for now since it will make lines overlap when there is a \n\r in the original file >_>
                        }
                        break;

                    case "set":
                        vars.Add('$' + lineArray[1], lineArray[2]);
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
