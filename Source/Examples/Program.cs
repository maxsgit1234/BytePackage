using System;
using BytePackage;

namespace Examples
{
    class Program
    {
        private static void W(string s)
        {
            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            //dummyFramework.Program.Main(null);
            BytePack pack = new BytePack();
            pack.AddInt(1);
            pack.AddString("hello");
            pack.AddDoubles(new[] { 1.2, 1.3, 1.4 });
            byte[] bytes = pack.Pack();

BytePackage.BytePack read = BytePackage.BytePack.Read(bytes);
int i = read.ReadInt();
string s = read.ReadString();
double[] d = read.ReadDoubles();

Console.WriteLine("int is: " + i);
Console.WriteLine("string is: " + s);
Console.WriteLine("doubles are: " + d[0] + ", " + d[1] + ", " + d[2]);
            

            W("DONE");
            Console.ReadLine();
        }
    }
}
