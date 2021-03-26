using Grpc.Core;
using System;
using System.Threading.Tasks;
using FibonacciGRPC;

namespace FibonacciClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(@"localhost:50051", ChannelCredentials.Insecure);

            bool useStream = true;
            var client = new Fibonacci.FibonacciClient(channel);

            if (useStream)
            {
                Console.Write("Enter how many Digits of Fibonacci you would like to receive: ");
                String number = Console.ReadLine();
                int seqNumber = Int32.Parse(number);
                using (var stream = client.StreamFibonacci(new FibonacciRequest { Number = seqNumber }))
                {
                    while (await stream.ResponseStream.MoveNext())
                    {
                        Console.WriteLine(stream.ResponseStream.Current.Result);
                    }
                }
                Console.ReadLine();
            }
            else
            {
                while (true)
                {
                    Console.Write("Enter Sequence Number: ");
                    String number = Console.ReadLine();
                    int seqNumber = Int32.Parse(number);
                    var reply = client.FindFibonacci(new FibonacciRequest { Number = seqNumber });
                    Console.WriteLine(String.Format("Fibonacci gRPC Service Response: {0}", reply.Result));
                }
            } 
        }
    }
}
