using System;
using CTP;

namespace Sttp.Tests.Commands
{
    public static class CommandObjectExtensions
    {
        public static void DebugToConsole(this CommandObject cmd)
        {
            Console.WriteLine("-------Schema----------");

            for (int x = 0; x < cmd.Schema.NodeCount; x++)
            {
                Console.WriteLine(cmd.Schema[x]);
            }

            Console.WriteLine("-------Data----------");


            var rdr3 = cmd.ToCommand().MakeDataReader();
            while (!rdr3.IsEmpty)
            {
                Console.WriteLine(rdr3.Read());
            }

            Console.WriteLine("-------Reader----------");
            var rdr = cmd.ToCommand().MakeReader();

            Console.WriteLine(rdr.ToString());
            while (rdr.Read())
            {
                Console.WriteLine(rdr.ToString());
            }
            Console.WriteLine(rdr.ToString());

            Console.WriteLine("-------Text----------");

            Console.WriteLine(cmd.ToString());
        }

    }
}