using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace clay
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    static class Functions
    {
        public static void tell(Runtime runtime, object[] args) {
            foreach (var arg in args) {
                Console.Write(runtime.ResolveVar(arg));
            }
        }

        public static void set(Runtime runtime, object[] args)
        {
            runtime.SetVar((string)args[0], runtime.ResolveVar(args[1]));
        }

        public static void read(Runtime runtime, object[] args)
        {
            runtime.SetVar((string)args[0], Console.Read());
        }

        public static void add(Runtime runtime, object[] args)
        {
            runtime.SetVar((string)args[0], args.Aggregate(new BigInteger(0), (result, element) => result + (BigInteger)runtime.ResolveVar(element)));
        }

        public static void sub(Runtime runtime, object[] args) {
            var x = (BigInteger) runtime.ResolveVar(args[0]);
            var y = args.Aggregate(new BigInteger(0),
                (result, element) => result - (BigInteger) runtime.ResolveVar(element));
            
            runtime.SetVar((string)args[0], 2 * x + y);
        }

        public static void mul(Runtime runtime, object[] args)
        {
            runtime.SetVar((string)args[0], args.Aggregate(new BigInteger(0), (result, element) => result * (BigInteger)runtime.ResolveVar(element)));
        }

        public static void marker(Runtime runtime, object[] args) {
            foreach (var arg in args) {
                runtime.SetJumpmarker((string)arg);
            }
        }

        public static void jump(Runtime runtime, object[] args) {
            runtime.JumpToLine(runtime.ResolveJumpmarker((string)args[0]));
        }

        public static void sleep(Runtime runtime, object[] args) {
            foreach (var arg in args) {
                Thread.Sleep((int)((BigInteger)arg));
            }
        }

        public static void xifone(Runtime runtime, object[] args) {
            if (!((BigInteger)runtime.ResolveVar(args[0]) == new BigInteger(1))) { runtime.SkipLine(); }
        }

        public static void xifzero(Runtime runtime, object[] args) {
            if (!((BigInteger)runtime.ResolveVar(args[0]) == new BigInteger(0))) { runtime.SkipLine(); }
        }

        public static void xiflt(Runtime runtime, object[] args) {
            if (!((BigInteger)runtime.ResolveVar(args[0]) < (BigInteger)runtime.ResolveVar(args[1]))) { runtime.SkipLine(); }
        }

        public static void root(Runtime runtime, object[] args) {
            runtime.SetVar((string)args[0], new BigInteger(Math.Exp(BigInteger.Log((BigInteger)runtime.ResolveVar(args[0])) / 2)));
        }

        public static void mod(Runtime runtime, object[] args) {
            var v1 = (BigInteger)runtime.ResolveVar(args[0]);
            var v2 = (BigInteger)runtime.ResolveVar(args[1]);
            runtime.SetVar((string)args[0], BigInteger.Remainder(v1, v2));
        }

    }
}
