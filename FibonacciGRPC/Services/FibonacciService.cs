using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace FibonacciGRPC.Services
{
    public class FibonacciService : Fibonacci.FibonacciBase
    {

        private static Dictionary<int, BigInteger> _dictionary = new Dictionary<int, BigInteger>();


        public override Task<FibonacciResult> FindFibonacci(FibonacciRequest request, ServerCallContext context)
        {
            int seqNumber = request.Number;

            return Task.FromResult(new FibonacciResult
            {
                Result = FibAlgorithm(seqNumber).ToString()
            });
        }

        public override async Task StreamFibonacci(FibonacciRequest request, IServerStreamWriter<FibonacciResult> responseStream, ServerCallContext context)
        {
            int fibCount = request.Number;

            int index = 0;
            while (index < fibCount)
            {
                var result = new FibonacciResult { Result = QuickFibAlgorithm(index).ToString() };
                await responseStream.WriteAsync(result);
                index++;
            }
        }


        private static int FibAlgorithm(int n)
        {
            if (n == 0 || n == 1)
            {                
                return n;
            }

            return FibAlgorithm(n - 1) + FibAlgorithm(n - 2);
        }

        private static BigInteger QuickFibAlgorithm(int n)
        {
            if (n == 0 || n == 1)
            {
                _dictionary[n] = n;
                return n;
            }

            _dictionary[n] = _dictionary[n - 1] + _dictionary[n - 2];

            return _dictionary[n];
        }
    }
}
