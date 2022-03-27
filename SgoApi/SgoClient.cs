using System;
using SgoApi.Clients;
using SgoApi.Models;
using Serilog;

namespace SgoApi
{
    public class SgoClient : IDisposable
    {
        readonly User user;
        public ConnectionClient Connection { get; }
        public DiaryClient Diary { get; }
        private bool disposedValue;

        public SgoClient(string login, string password)
        {
            user = new User(login, password);
            Connection = new ConnectionClient(user);
            Diary = new DiaryClient(user);
            Log.Debug("[{Source}] {Message}", "Sgo", "SgoClient created");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _ = Connection.DisconnectAsync();
                    user.Dispose();
                    Log.Debug("[{Source}] {Message}", "Sgo", "SgoClient disposed");
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
