using System;
using System.Reflection;
using Microsoft.Win32;
using PacificSystem.Utility;
using SharedUtilitys.Attributes;
using SharedUtilitys.Enums;

namespace SharedUtilitys.Models
{
    public class BaseRegistryModel
    {
        public void Reload()
        {
            var members = GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (var member in members)
            {
                if (!_isAutoProperty(member.Name))
                {
                    continue;
                }

                var value = GetType().InvokeMember(member.Name, BindingFlags.GetProperty, null, this, null);
                _checkContract(member.Name, value);

                _setContractValue(member.Name, member.PropertyType);
            }
        }

        public void Save()
        {
            var members = GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (var member in members)
            {
                if (!_isAutoProperty(member.Name))
                {
                    continue;
                }

                var value = GetType().InvokeMember(member.Name, BindingFlags.GetProperty, null, this, null);
                _checkContract(member.Name, value);

                SetValue(member.Name, Converts.ToTryString(value));
            }
        }

        public string GetValue(string name, string defaultValue)
        {
            var regkey = _getRegkey();

            if (regkey != null)
            {
                var result = Converts.ToTryString(regkey.GetValue(name, defaultValue));
                regkey.Close();
                return result;
            }

            return String.Empty;
        }

        public void SetValue(string name, object value)
        {
            var regkey = _getRegkey();

            if (regkey != null)
            {
                regkey.SetValue(name, value);
                regkey.Close();
            }
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

        private RegistryKey _getRegkey()
        {
            var customAttribute = (RegistryLocationAttribute)Attribute.GetCustomAttribute(GetType(), typeof(RegistryLocationAttribute));

            if (customAttribute == null)
            {
                return null;
            }

            if (customAttribute.RegistryLocation == RegistryLocationEnum.CurrentUser)
            {
                return Registry.CurrentUser.CreateSubKey(customAttribute.ParentKey);
            }
            
            return Registry.LocalMachine.CreateSubKey(customAttribute.ParentKey);
        }

        private void _checkContract(string propertyName, object value)
        {
            if (value != null && value.GetType().IsGenericType)
            {
                _throwAutoPropertyExceptoin(propertyName);
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
                || value == null
                )
            {
                return;
            }

            _throwAutoPropertyExceptoin(propertyName);
        }

        private void _setContractValue(string propertyName, Type type)
        {
            var registryValue = GetValue(propertyName, String.Empty);
            object value = null; 

            if (type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64))
            {
                value = Converts.ToTryInt(registryValue);
            }
            else if (type == typeof (String))
            {
                value = Converts.ToTryString(registryValue);
            }
            else if (type == typeof(Boolean))
            {
                value = Converts.ToTryBool(registryValue);
            }
            else if (type == typeof(Decimal) || type == typeof(Decimal?))
            {
                value = Converts.ToTryDecimal(registryValue);
            }
            else if (type == typeof(Double) || type == typeof(Double?))
            {
                value = Converts.ToTryDouble(registryValue, 0);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                value = Converts.ToTryDateTimeNullable(registryValue, null);
            }
            else
            {
                _throwAutoPropertyExceptoin(propertyName);
            }

            GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, this, new[] { value });
        }

        private void _throwAutoPropertyExceptoin(string propertyName)
        {
            throw new Exception("AutoPropertyに定義された型に誤りがあります。プロパティ名：" + propertyName);
        }
    }
}
