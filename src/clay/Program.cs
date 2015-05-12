using System;

namespace clay
{
    class Program {
        static void Main(string[] args) {
            var r = new Runtime();
            //r.Run(@"tell 'primes' $newline
            //set $prime 1
            //marker nextprime
            //add $prime 1
            //set $tad $prime
            //root $tad
            //add $tad 1
            //marker test
            //sub $tad 1
            //xiflt $tad 2
            //tell $prime $newline
            //xiflt $tad 2
            //jump nextprime
            //set $remainder $prime
            //mod $remainder $tad
            //xifzero $remainder
            //jump nextprime
            //jump test");

            r.Run(@"

tell 'test'

");

            Console.WriteLine("\n////// finished execution //////");
            Console.ReadLine();
        }
    }
}
