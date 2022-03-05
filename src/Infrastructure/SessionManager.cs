using System;
using System.Web;

namespace StepByStep.Infrastructure
{
    public class SessionManager
    {
        #region Fields

        private readonly HttpSessionStateBase Session;

        #endregion

        #region Ctor

        public SessionManager(HttpSessionStateBase sessionStateBase)
        {
            Session = sessionStateBase;
        }

        #endregion

        #region Utilities

        protected virtual void Insert<T>(T obj)
        {
            Session.Add(typeof(T).Name, obj);
        }

        protected virtual void Update<T>(T obj)
        {
            Remove<T>();
            Insert(obj);
        }

        #endregion

        #region Methods

        public T Get<T>(T obj)
        {
            if (Session[typeof(T).Name] == null)
                if (obj != null)
                    Insert(obj);
                else
                    Insert((T)Activator.CreateInstance(typeof(T)));
            else
            {
                if (obj != null)
                    Update(obj);
            }

            return (T)Session[typeof(T).Name];
        }

        public virtual void Remove<T>()
        {
            Session.Remove(typeof(T).Name);
        }

        #endregion
    }
}