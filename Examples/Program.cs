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
            BytePack pack = new BytePack();
            pack.AddInt(1);
            pack.AddString("hello");
            pack.AddDoubles(new[] { 1.2, 1.3, 1.4 });
            byte[] bytes = pack.Pack();

            BytePack read = BytePack.Read(bytes);
            W("int is: " + read.ReadInt());
            W("string is: " + read.ReadString());

            double[] d = read.ReadDoubles();
            string line = "doubles are: ";
            foreach (double dd in d)
                line += dd + " ";
            W(line);


            W("DONE");
            Console.ReadLine();
        }
    }
}
