using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAppBase.Models.SystemMenus
{
    public class SystemMenuModelReg
    {
        public static List<SystemMenuModel> GetMenus(int UserRole)
        {

            var list = new List<SystemMenuModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var Sql = new StringBuilder();

                Sql.Append(@" SELECT 
                                  M1.MenuID, M1.MenuName, M1.ParentID, ifnull(M1.ActionName,'') as ActionName , ifnull(M1.ControllerName,'') as ControllerName , M2.UserRole,ifnull(M1.CssClass,'') as CssClass  
                             FROM 
                                  [account_m_menus] M1 
                             INNER JOIN 
                                  [account_m_rolemenus] M2 ON  
                                  M2.MenuID = M1.MenuID  
                             ORDER BY M2.DisplayNo,M1.MenuID ");


                utility.AddParameter("UserRole", UserRole);
                utility.ExecuteReaderModelList(Sql.ToString(), list);
            }

            return list;
        }

    }
}
