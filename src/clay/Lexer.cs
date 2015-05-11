using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace clay
{
    static class Lexer {
        private const char QuotationMark = '\'';

        public static IEnumerable<IEnumerable<object>> Lex(StreamReader code) {
            var line = "";
            for (;!code.EndOfStream;) {
                switch (code.Peek()) {
                    case '\n':
                    case '\r':
                        code.Read();
                        if (line.Trim(" \n\r\t".ToCharArray()) != "") { yield return SplitTrimMultipleSpaces(line); }
                        line = "";
                        break;
                    default:
                        line += (char)code.Read();
                        break;
                }
            }
            if (line != "") { yield return SplitTrimMultipleSpaces(line); }
        }

        private enum FragmentType {
            @String,
            @Integer
        }

        //TODO: I really want to rewrite this in a functional matter
        private static IEnumerable<object> SplitTrimMultipleSpaces(string code) {
            var i = 0;

            var fragment = "";
            var inQuotes = false;

            while (i < code.Length) {
                while (i < code.Length && (code[i] != ' ' || inQuotes)) {
                    inQuotes ^= code[i] == QuotationMark;
                    fragment += code[i++];
                }

                if (fragment.Any()) {
                    if (fragment.Any(c => !"0123456789".Contains(c))) {
                        if (fragment.Length > 1 && fragment.First() == QuotationMark && fragment.Last() == QuotationMark) { //TODO:haven't decided yet what to do when there's only one quotation mark
                            yield return fragment.Substring(1, fragment.Length - 2);
                        } else {
                            yield return fragment;
                        }
                    } else {
                        yield return BigInteger.Parse(fragment);
                    }
                }

                i++; //progress
                fragment = "";
            }
        }
    }
}
