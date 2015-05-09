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
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(@"I could only take your hand
and lead you to the crossing road where we first met,
we’d take a skip together like the good old days
and laugh beneath a brilliantly blue sky.")));

            var test = Lexer.Lex(stream).Select(t => t.ToArray()).ToArray();

            Console.ReadLine();
        }
    }
}
