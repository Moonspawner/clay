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
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("test1 test2  test3   ")));

            foreach (var token in Lexer.Lex(stream)) {
                Console.WriteLine("-{0}-", token);
            }

            Console.ReadLine();
        }
    }
}
