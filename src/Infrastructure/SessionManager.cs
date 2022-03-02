using System;
using System.Web;

namespace StepByStep.Infrastructure
{
    public class SessionManager
    {
        private readonly HttpSessionStateBase Session;

        public SessionManager(HttpSessionStateBase sessionStateBase)
        {
            Session = sessionStateBase;
        }

        public T Get<T>()
        {
            if (Session[typeof(T).Name] == null)
                Insert((T)Activator.CreateInstance(typeof(T)));

            return (T)Session[typeof(T).Name];
        }

        private void Insert<T>(T obj)
        {
            Session.Add(typeof(T).Name, obj);
        }

        //private void Update<T>(T obj)
        //{
        //    Remove<T>();
        //    Insert(obj);
        //}

        public void Remove<T>()
        {
            Session.Remove(typeof(T).Name);
        }
    }
}