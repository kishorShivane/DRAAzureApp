using DRA.DataProvider.Interfaces;
using DRA.DataProvider.Models;
using System;
using System.Collections;
using System.Configuration;

namespace DRA.DataProvider.Helpers
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private DRAContext context;

        private Hashtable repositories = new Hashtable();
        public UnitOfWork()
        {
            var connString = ConfigurationManager.ConnectionStrings["DRAContext"].ConnectionString;
            if (string.IsNullOrEmpty(connString))
                context = new DRAContext();
            else
                context = new DRAContext(connString);
        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            if (!repositories.Contains(typeof(T)))
            {
                repositories.Add(typeof(T), new Repository<T>(context));
            }
            return (IRepository<T>)repositories[typeof(T)];
        }

        public void Save()
        {
            context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
