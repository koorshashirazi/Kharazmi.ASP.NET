using System;
using Kharazmi.AspNetMvc.Core.Helpers;

namespace Kharazmi.AspNetMvc.Core.Managers
{
    public abstract class ManagerBase
    {
        private readonly object _lock = new object();

        public void Execute(Action codeExecute)
        {
            try
            {
                codeExecute();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public object Execute(Func<object> codeExecute)
        {
            try
            {
                return codeExecute();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T Execute<T>(Func<T> codeExecute)
        {
            try
            {
                return codeExecute();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void TryExecute(Action codeExecute, int retryCount = 3, int waitSeconds = 0)
        {
            ThreadingHelper.TryLock(codeExecute, _lock, 3);
        }

        public object TryExecute(Func<object> codeExecute, int retryCount = 3, int waitSeconds = 0)
        {
            return ThreadingHelper.TryLock(codeExecute, _lock, 3);
        }

        public T TryExecute<T>(Func<T> codeExecute, int retryCount = 3, int waitSeconds = 0)
        {
            return ThreadingHelper.TryLock(codeExecute, _lock, 3);
        }
    }
}