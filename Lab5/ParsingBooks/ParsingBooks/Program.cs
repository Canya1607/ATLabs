using System;
using System.Threading.Tasks;
namespace ParsingBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Process started!");
            Parallel.For(1, 867, index => new Parser((ushort) index).parse());
        }
    }
}
