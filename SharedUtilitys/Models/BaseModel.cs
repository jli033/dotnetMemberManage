using System;
using System.Collections.Generic;
using System.Reflection;
using PacificSystem.Utility;
using SharedUtilitys.Attributes;

namespace SharedUtilitys.Models
{
    [Serializable]
    public abstract class BaseModel
    {
        public Dictionary<string, object> ToDictionary()
        {
            var result = new Dictionary<string, object>();

            PropertyInfo[] members =
                GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (PropertyInfo member in members)
            {
                object value = GetType().InvokeMember(member.Name, BindingFlags.GetProperty, null, this, null);

                if (value != null && value.GetType().IsGenericType)
                {
                    continue;
                }

                if (!_checkContract(value))
                {
                    continue;
                }

                result.Add(member.Name, value);
            }

            return result;
        }

        public void SetDictionary(Dictionary<string, object> item)
        {
            var members = GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (var member in members)
            {
                if (!_isAutoProperty(member.Name))
                {
                    continue;
                }

                _setContractValue(member.Name, member.PropertyType, item[member.Name]);
            }
        }

        public object GetPropertyValue(string name)
        {
            return GetType().InvokeMember(name, BindingFlags.GetProperty, null, this, null);
        }

        public void SetPropertyValue(string name, object value)
        {
            GetType().InvokeMember(name, BindingFlags.SetProperty, null, this, new[] {value});
        }

        private bool _checkContract(object value)
        {
            if (value is Int16
                || value is Int32
                || value is Int64
                || value is Byte
                || value is Byte[]
                || value is String
                || value is Boolean
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                || value is Decimal || value is Decimal?
                || value is Double || value is Double?
                || value is float || value is float?
                || value is DateTime || value is DateTime?
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                || value is List<String>
                || value is List<List<String>>
                || value is List<Int16> || value is List<Int16?>
                || value is List<Int32> || value is List<Int32?>
                || value is List<Int64> || value is List<Int64?>
                || value is List<Decimal> || value is List<Decimal?>
                || value is List<Double> || value is List<Double?>
                || value is List<DateTime> || value is List<DateTime?>
                || value is Dictionary<string, object>
                || value is Dictionary<string, string>
                || value is Dictionary<int, int>
                || value is List<Dictionary<string, object>>
                || value is List<List<Dictionary<string, object>>>
                || value is DBNull
                || value == null
                )
            {
                return true;
            }
            return false;
        }

        private void _setContractValue(string propertyName, Type type, object value)
        {
            object contractValue = null;

            if (type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64))
            {
                contractValue = Converts.ToTryInt(value);
            }
            else if (type == typeof(String))
            {
                contractValue = Converts.ToTryString(value);
            }
            else if (type == typeof(Boolean))
            {
                contractValue = Converts.ToTryBool(value);
            }
            else if (type == typeof(Decimal) || type == typeof(Decimal?))
            {
                contractValue = Converts.ToTryDecimal(value);
            }
            else if (type == typeof(Double) || type == typeof(Double?))
            {
                contractValue = Converts.ToTryDouble(value, 0);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                contractValue = Converts.ToTryDateTimeNullable(value, null);
            }
            else
            {
                _throwAutoPropertyExceptoin(propertyName);
            }

            GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new[] { contractValue });
        }

        private bool _isAutoProperty(string propertyName)
        {
            var autoProperty = Attribute.GetCustomAttributes(
                GetType().GetProperty(propertyName),
                typeof(AutoPropertyAttribute)) as AutoPropertyAttribute[];

            if (autoProperty != null && autoProperty.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void _throwAutoPropertyExceptoin(string propertyName)
        {
            throw new Exception("AutoPropertyに定義された型に誤りがあります。プロパティ名：" + propertyName);
        }
    }
}