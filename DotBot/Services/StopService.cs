using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotBot.Services
{
    internal class StopService
    {
        CancellationTokenSource source;

        public StopService()
        {
            source = new CancellationTokenSource();
        }

        public CancellationToken GetToken() => source.Token;

        public void StopMainTask() => source.Cancel(); 
    }
}
