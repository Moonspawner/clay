using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay
{
    class Program
    {
        static void Main(string[] args) {
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(@"tell 'I could only take your hand'
tell 'and lead you to the crossing road where we first met,'
tell 'we’d take a skip together like the good old days'
tell 'and laugh beneath a brilliantly blue sky.'")));

            foreach (var lineArray in Lexer.Lex(stream).Select(line => line.ToArray())) {
                switch(lineArray[0])
                {
                    case "tell":
                        foreach (var argument in Enumerable.Range(1, lineArray.Count() - 1).Select(i => lineArray[i])) {
                            Console.Write(argument.Replace("\r", "")); //ignore \r for now since it will make lines overlap when there is a \n\r in the original file >_>
                        }
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
