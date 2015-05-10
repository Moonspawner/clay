using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace clay
{
    class Program {
        private static Dictionary<string, string> _vars;
        private static Dictionary<string, int> _jumps; 

        static void Main(string[] args) {
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(@"
tell 'fibonacci sequence' $newline
set $old 1
set $current 1
set $new $current
tell $old $newline
tell $current $newline
marker repeat
set $new $current
+ $new $old
tell $new $newline
set $old $current
set $current $new
sleep 350
jump repeat
".Replace("\r", ""))));

            _vars = new Dictionary<string, string> {{"$newline", "\n\r"}};
            _jumps = new Dictionary<string, int>();

            var lines = Lexer.Lex(stream).ToArray();
            for (var index = 0; index < lines.Length; index++) {
                var tokens = lines[index].ToArray();

                if (tokens.Any()) {
                    switch (tokens[0]) {
                        case "tell":
                            foreach (
                                var argument in Enumerable.Range(1, tokens.Count() - 1).Select(i => tokens[i])) {
                                Console.Write(Resolve(argument));
                            }
                            break;

                        case "set":
                            _vars[tokens[1]] = Resolve(tokens[2]);
                            break;

                        case "read":
                            _vars[tokens[1]] = Console.ReadLine();
                            break;

                        case "+":
                            _vars[tokens[1]] =
                                (BigInteger.Parse(Resolve(tokens[1])) + BigInteger.Parse(Resolve(tokens[2]))).ToString();
                            break;

                        case "-":
                            _vars[tokens[1]] =
                                (BigInteger.Parse(Resolve(tokens[1])) - BigInteger.Parse(Resolve(tokens[2]))).ToString();
                            break;

                        case "*":
                            _vars[tokens[1]] =
                                (BigInteger.Parse(Resolve(tokens[1]))* BigInteger.Parse(Resolve(tokens[2]))).ToString();
                            break;

                        case "/":
                            _vars[tokens[1]] =
                                (BigInteger.Parse(Resolve(tokens[1]))/ BigInteger.Parse(Resolve(tokens[2]))).ToString();
                            break;

                        case "abs":
                            _vars[tokens[1]] = Resolve(tokens[1]);
                            break;

                        case "marker":
                            _jumps[Resolve(tokens[1])] = index;
                            break;

                        case "jump":
                            index = _jumps[Resolve(tokens[1])];
                            break;

                        case "sleep":
                            Thread.Sleep(int.Parse(tokens[1]));
                            break;
                    }
                }
            }

            Console.ReadLine();
        }

        private static string Resolve(string identifier) {
            return identifier == "" ? "" : (identifier[0] == '$' ? _vars[identifier] : identifier);
        }
    }
}
