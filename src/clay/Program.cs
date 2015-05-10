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
tell 'primes' $newline
set $prime 1
marker nextprime
+ $prime 1
set $tad $prime
root $tad
+ $tad 1
marker test
- $tad 1
xiflt $tad 2
tell $prime $newline
xiflt $tad 2
jump nextprime
set $remainder $prime
% $remainder $tad
xifzero $remainder
jump nextprime
jump test
".Replace("\r", ""))));

            _vars = new Dictionary<string, string> {{"$newline", "\n\r"}};
            _jumps = new Dictionary<string, int>();

            var lines = Lexer.Lex(stream).ToArray();
            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++) {
                var tokens = lines[lineNumber].ToArray();

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
                            _jumps[Resolve(tokens[1])] = lineNumber;
                            break;

                        case "jump":
                            lineNumber = _jumps[Resolve(tokens[1])];
                            break;

                        case "sleep":
                            Thread.Sleep(int.Parse(tokens[1]));
                            break;
                            
                        case "xifone":
                            if (Resolve(tokens[1]) != "1") { lineNumber ++; }
                            break;

                        case "xifzero":
                            if (Resolve(tokens[1]) != "0") { lineNumber++; }
                            break;

                        case "xiflt":
                            if (int.Parse(Resolve(tokens[1])) >= int.Parse(Resolve(tokens[2]))) { lineNumber++; }
                            break;

                        case "root":
                            _vars[tokens[1]] = Math.Floor(Math.Sqrt(int.Parse(Resolve(tokens[1])))).ToString(); //TODO: add biginteger support
                            break;

                        case "%":
                            _vars[tokens[1]] = (BigInteger.Parse(Resolve(tokens[1])) % BigInteger.Parse(Resolve(tokens[2]))).ToString();
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
