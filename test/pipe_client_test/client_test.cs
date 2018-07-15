using NamedPipeWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace pipe_client_test
{
    class Program
    {
        static Stopwatch sw;
        static void Main(string[] args)
        {

            Console.WriteLine("input ID pipe:");
            string id = Console.ReadLine();

            var client = new NamedPipeClient<byte[]>(id);
            client.ServerMessage += OnServerMessage;
            client.Error += OnError;
            client.AutoReconnect = true;
            client.Start();

            Console.WriteLine("pipe connected,  sending data to pipe Server!!!");
            //Console.ReadKey();


            string fi = @"F:\_mmf\SocTrang\160108_60.mmf";
            //fi = @"F:\_mmf\SocTrang\160113_60.mmf";
            //fi = @"F:\_mmf\SocTrang\160113_160.mmf";
            //fi = @"F:\_mmf\SocTrang\160113_180.mmf";
            //fi = @"F:\_mmf\SocTrang\160113_750.mmf";
            fi = @"F:\_mmf\SocTrang\150115_850.mmf";

            
            byte[] buffer = hostFile.read_byte_MMF(fi);

            List<byte> allbytes = new List<byte>();
            allbytes.AddRange(BitConverter.GetBytes(buffer.Length));
            allbytes.AddRange(buffer);
            buffer = allbytes.ToArray();

            sw = new Stopwatch();
            sw.Start();
            client.PushMessage(buffer);


            Console.ReadKey();
            Console.ReadKey();
        }

        private static void OnServerMessage(NamedPipeConnection<byte[], byte[]> connection, byte[] message)
        {
            Console.WriteLine("Server says: {0}", message);
            //sw.Stop();
            //Console.WriteLine("Push ok: {0}  ms", sw.ElapsedMilliseconds);
        }

        private static void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }


    }//end class
}
