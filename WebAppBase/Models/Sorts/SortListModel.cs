using System;
using System.Collections.Generic;
using SharedUtilitys.DataBases;
using WebAppBase.Models.Base;
using PacificSystem.Utility;

namespace WebAppBase.Models.Sorts
{
    public class SortListModel
    {
        public List<SortModel> Items { get; set; }

        public SortListModel()
        {
            Items = new List<SortModel>();
        }

        public SortListModel(SortTargetModel model)
        {
            Items = new List<SortModel>();

            if (model.SortItems != null)
            {
                if (model.SortItems.Count > 0)
                {
                    var firstItem = model.SortItems[0];
                    var type = firstItem.GetType();
                    var IdColumnPropInfo = type.GetProperty(model.IdColumn);
                    var DisplayNoColumnPropInfo = type.GetProperty(model.DisplayNoColumn);
                    var DisplayColumnPropInfo = type.GetProperty(model.DisplayColumn);

                    foreach (var itm in model.SortItems)
                    {
                        Items.Add(new SortModel
                        {
                            Id = Convert.ToInt64(IdColumnPropInfo.GetValue(itm, null)),
                            ColumnOrder = Converts.ToTryInt(DisplayNoColumnPropInfo.GetValue(itm, null)),
                            Description = Converts.ToTryString(DisplayColumnPropInfo.GetValue(itm, null))
                        });
                    }
                }
                return;
            }

            var where = @" WHERE 1 = 1 ";

            using (var utility = DbUtility.GetInstance())
            {
                if (!String.IsNullOrEmpty(model.OrganizationID))
                {
                    where += @" AND OrganizationID = ?OrganizationID ";
                    utility.AddParameter("OrganizationID", model.OrganizationID);
                }

                if (!String.IsNullOrEmpty(model.DisplayFlagColumn))
                {
                    where += String.Format(@" AND {0} = 1  ", model.DisplayFlagColumn);
                }

                if (!String.IsNullOrEmpty(model.StatusFlagColumn))
                {
                    where += String.Format(@" AND {0} = 0  ", model.StatusFlagColumn);
                }

                var sql = String.Format(@"
                        SELECT 
                            {0}
                          , {1}
                          , {2}
                        FROM 
                            {3}
                        {4}
                        ORDER BY
                            {2} ", model.IdColumn, model.DisplayColumn, model.DisplayNoColumn, model.TableName, where);

                var list = utility.ExecuteReader(sql);

                foreach (var item in list)
                {
                    Items.Add(new SortModel
                    {
                        Id = Convert.ToInt64(item[model.IdColumn]),
                        ColumnOrder = Converts.ToTryInt(item[model.DisplayNoColumn]),
                        Description = Converts.ToTryString(item[model.DisplayColumn])
                    });
                }
            }
        }

        public static SortListModel GetInstance()
        {
            return new SortListModel();
        }

        public void Update(SortTargetModel model, List<SortModel> items)
        {
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                foreach (var item in items)
                {
                    var sql = String.Format(@"UPDATE {0} SET {1} = {2} WHERE {3} = {4}",
                        model.TableName, model.DisplayNoColumn, item.ColumnOrder + 1, model.IdColumn, item.Id);

                    utility.ExecuteNonQuery(sql);
                }

                utility.Commit();
            }
        }
    }
}