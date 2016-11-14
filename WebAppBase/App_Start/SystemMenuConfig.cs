using System.Collections.Generic;
using WebAppBase.Enums;
using WebAppBase.Models.SystemMenus;
using System;

namespace WebAppBase.Configs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SystemMenuConfig
    {

        private static List<SystemMenuModel> _menus = new List<SystemMenuModel>();
        private static object lockObj = new object();
        static SystemMenuConfig()
        {
            RefreshMenu();
        }

        public static void RefreshMenu()
        {
            lock (lockObj)
            {
                _menus = SystemMenuModelReg.GetMenus(Convert.ToInt32(SystemRollEnum.SysAdmin));
            }
        }

        public static List<SystemMenuModel> GetMenus(SystemRollEnum rollEnum)
        {
            var menus2 = _menus.FindAll(m => Convert.ToInt32(m.RollEnum) == Convert.ToInt32(rollEnum));
            return menus2;
        }

        public static bool CheckRoll( string controller,string action, SystemRollEnum rollEnum)
        {
            var menus =_menus.FindAll(m =>  m.ControllerName.Equals(controller) && m.ActionName.Equals(action));
            if (menus == null || menus.Count==0)
            {
                return true;
            }
            return menus.FindIndex(m => m.RollEnum == rollEnum) >= 0;
            //return Convert.ToInt32(menu.RollEnum) == Convert.ToInt32(rollEnum);
            
        }
    }
}