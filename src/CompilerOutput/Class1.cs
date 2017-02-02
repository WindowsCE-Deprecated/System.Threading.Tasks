using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompilerOutput
{
    public class Class1
    {
        public Task<bool> Creator()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10);
                return true;
            });
        }

        public async Task CallCreatorAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                await Creator();
            }
        }
    }
}
