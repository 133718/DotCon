using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgoApi.Clients;
using SgoApi.Models;
using SgoApi.Results;

namespace SgoApi
{
    public class SgoClient : IDisposable
    {
        readonly User user;
        readonly ConnectionClient connection;
        private bool disposedValue;

        public SgoClient(string login, string password)
        {
            user = new User(login, password);
            connection = new ConnectionClient(user);
        }

        public async Task<RuntimeResult> ConnectAsync()
        {
            return await connection.ConnectAsync();
        }

        public async Task<RuntimeResult> DisconnectAsync()
        {
            return await connection.DisconnectAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _ = connection.DisconnectAsync();
                    user.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SgoClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
