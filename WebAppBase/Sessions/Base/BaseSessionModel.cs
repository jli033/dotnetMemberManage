using System;
using System.Reflection;
using System.Web;
using PacificSystem.Utility;
using System.Collections;
using System.Web.SessionState;

namespace WebAppBase.Sessions.Base
{
    public class BaseSessionModel
    {
        private ISessionModel _sessionBase;

        public BaseSessionModel(ISessionModel sessionBase) 
        {
            _sessionBase = sessionBase;
        }

        public void Reload()
        {
            var members = GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (var member in members)
            {
                var value = GetType().InvokeMember(member.Name, BindingFlags.GetProperty, null, this, null);
                _checkContract(member.Name, value);

                _setContractValue(_sessionBase, member.Name, member.PropertyType);
            }
        }

        public void Save()
        {
            var members = GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (var member in members)
            {
                var value = GetType().InvokeMember(member.Name, BindingFlags.GetProperty, null, this, null);
                _checkContract(member.Name, value);

                _sessionBase[member.Name] = Converts.ToTryString(value);
            }
        }

        private void _checkContract(string propertyName, object value)
        {
            if (value != null && value.GetType().IsGenericType)
            {
                _throwPropertyExceptoin(propertyName);
            }

            if (value is Int16
                || value is Int32
                || value is Int64
                || value is String
                || value is Boolean
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                || value is Decimal || value is Decimal?
                || value is Double || value is Double?
                || value is DateTime || value is DateTime?
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                || value is Enum
                || value == null
                )
            {
                return;
            }

            _throwPropertyExceptoin(propertyName);
        }

        private void _setContractValue(ISessionModel session, string propertyName, Type type)
        {
            var sessionValue = session[propertyName];
            object value = null; 

            if (type == typeof(Int16) || type == typeof(Int32) )
            {                
                value = Converts.ToTryInt(sessionValue);
            }
            else if (type == typeof(Int64))
            {
                var l=0L;
                if (sessionValue!=null && Int64.TryParse(sessionValue.ToString(), out l))
                {
                    value = l;
                }
                else
                {
                    value = 0;
                }
            }
            else if (type == typeof (String))
            {
                value = Converts.ToTryString(sessionValue);
            }
            else if (type == typeof(Boolean))
            {
                value = Converts.ToTryBool(sessionValue);
            }
            else if (type == typeof(Decimal) || type == typeof(Decimal?))
            {
                value = Converts.ToTryDecimal(sessionValue);
            }
            else if (type == typeof(Double) || type == typeof(Double?))
            {
                value = Converts.ToTryDouble(sessionValue, 0);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                value = Converts.ToTryDateTimeNullable(sessionValue, null);
            }
            else if (type.IsEnum)
            {
                if ( sessionValue !=null && Enum.IsDefined(type, Converts.ToTryString(sessionValue)))
                {
                    value = Enum.Parse(type, Converts.ToTryString(sessionValue));// Enum.GetValues(type).GetValue(Converts.ToTryString(sessionValue));
                }
                else
                {
                    value = Enum.GetValues(type).GetValue(0);
                }
            }
            else
            {
                _throwPropertyExceptoin(propertyName);
            }

            GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new[] { value });
        }

        private void _throwPropertyExceptoin(string propertyName)
        {
            throw new Exception("Propertyに定義された型に誤りがあります。プロパティ名：" + propertyName);
        }
    }

    public interface ISessionModel
    {
        object this[string index] { get; set; }
    }

    public class HttpSessionStateBaseSessionModel : ISessionModel
    {
        private HttpSessionStateBase _session;
        public HttpSessionStateBaseSessionModel(HttpSessionStateBase session)
        {
            this._session = session;
        }
        public object this[string key]
        {
            get {
                return this._session[key];
            }
            set
            {
                this._session[key] = value;
            }
        }
    }

    public class HttpSessionStateSessionModel : ISessionModel
    {
        private HttpSessionState _session;
        public HttpSessionStateSessionModel(HttpSessionState session)
        {
            this._session = session;
        }
        public object this[string key]
        {
            get
            {
                return this._session[key];
            }
            set
            {
                this._session[key] = value;
            }
        }
    }


}
