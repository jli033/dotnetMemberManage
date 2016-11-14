using WebAppBase.Enums;
using System;

namespace WebAppBase.Models.SystemMenus
{
    public class SystemMenuModel
    {
        public int MenuID { get; set; }
        public int ParentMenuID { get; set; }

        public string MenuName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public int ParentID
        {
            get
            {
                return ParentMenuID;
            }
            set
            {
                ParentMenuID = value;
            }
        }
        public int UserRole
        {
            get
            {
                return Convert.ToInt32(RollEnum);
            }
            set
            {
                RollEnum = (SystemRollEnum)Enum.Parse(typeof(SystemRollEnum), value.ToString());
            }
        }

        public bool IsRootMenu
        {
            get
            {
                return this.MenuID == ParentMenuID;
            }
        }

        public string CssClass { get; set; }

        public SystemRollEnum RollEnum { get; set; }

        public SystemMenuModel()
        {
        }

        //public SystemMenuModel(int menuID, int parentMenuID, string menuName, string actionName, string controllerName,string cssClass, SystemRollEnum systemRollEnum)
        //{
        //    MenuID = menuID;
        //    ParentMenuID = parentMenuID;
        //    MenuName = menuName;
        //    ActionName = actionName;
        //    ControllerName = controllerName;
        //    RollEnum = systemRollEnum;
        //    CssClass = string.Format("{0}", cssClass);
        //}
    }
}