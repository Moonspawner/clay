using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay
{
    static class Lexer
    {
        public static IEnumerable<string> Lex(StreamReader code)
        {
            var lastToken = "";
            while (!code.EndOfStream) {
                if (code.Peek() == ' ') {
                    if (lastToken != "") {
                        yield return lastToken;
                        lastToken = "";
                    }
                    code.Read();
                } else {
                    lastToken += (char)code.Read();
                }
            }
            if (lastToken != "") { yield return lastToken; }
        }
    }
}
