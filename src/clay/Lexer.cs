using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay
{
    static class Lexer {
        private const char QuotationMark = '\'';

        public static IEnumerable<IEnumerable<string>> Lex(StreamReader code) {
            var line = "";
            for (; !code.EndOfStream; line += (char) code.Read()) {
                if (code.Peek() == '\n') {
                    yield return SplitTrimMultipleSpaces(line);
                    line = "";
                    code.Read();
                }
            }
            if (line != "") { yield return SplitTrimMultipleSpaces(line); }
        }

        private static IEnumerable<string> SplitTrimMultipleSpaces(string code) {
            var inquotes = false;
            var fragment = "";
            foreach (var @char in code) {
                if (@char != ' ' || inquotes) { //we don't want to add spaces to our fragment, since we want to split on spaces
                    if (@char == QuotationMark) { inquotes ^= true; continue; }
                    fragment += @char;
                } else {
                    if (fragment == "") { continue; } //when we encounter two consecutive spaces we just omit them all together
                    yield return fragment;
                    fragment = ""; 
                }
            }
            if(fragment != "") { yield return fragment; } //I'd love to have an simple and clean way to tell if we were in the last iteration of the foreach-loop
        }
    }
}
