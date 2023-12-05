using System;
using System.Threading;

namespace Chat_server
{
    class Program
    {
        static void MainThread()
        {
            while (true)
            {
                ThreadManager.UpdateMain();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Server started...");
            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            Server.Run();
        }
    }
}
