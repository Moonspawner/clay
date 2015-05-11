using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;

namespace clay {
    class Runtime {
        private static Dictionary<string, object> _vars;
        private static Dictionary<string, int> _jumps;
        private int _lineNumber;

        public void SkipLine() {
            _lineNumber++;
        }

        public void SetVar(string identifier, object value) {
            _vars[identifier] = value;
        }

        public void SetJumpmarker(string name, int? line = null) {
            _jumps[name] = line ?? _lineNumber;
        }

        public void JumpToLine(int line) {
            _lineNumber = line;
        }

        public void Run(string code) {
            Run(Lexer.Lex(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(code)))).ToArray());
        }

        public int ResolveJumpmarker(string identifier) {
            return _jumps[identifier];
        }

        public object ResolveVar(object identifier)
        {
            return identifier is string
                ? (string)identifier == ""
                    ? ""
                    : ((string)identifier)[0] == '$' ? _vars[(string)identifier] : identifier
                : identifier;
        }

        public void Run(IEnumerable<object>[] lines) {
            _vars = new Dictionary<string, object> {{"$newline", "\n\r"}};
            _jumps = new Dictionary<string, int>();

            _lineNumber = 0;
            for (; _lineNumber < lines.Length; _lineNumber++) {
                var tokens = lines[_lineNumber].ToArray();

                if (tokens.Any()) {
                    typeof(Functions).GetMethod(tokens[0].ToString()).Invoke(this, new object[] {this, Enumerable.Range(1, tokens.Count() - 1).Select(i => tokens[i]).ToArray()});
                }
            }
        }
    }
}