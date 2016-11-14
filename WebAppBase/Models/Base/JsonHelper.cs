using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebAppBase.Models.Base
{
    public class JsonHelper
    {
        public static JsonHelper GetInstance()
        {
            return new JsonHelper();
        }

        private IQueryable<T> _sortIQueryable<T>(IQueryable<T> data, string sortOrder)
        {
            if (sortOrder == "none")
            {
                return data;
            }

            const string fieldName = "GridUniqueID";

            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = Expression.Parameter(typeof(T), "i");
            Expression conversion = Expression.Convert(Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression) : data.OrderBy(mySortExpression);
        }

        public JsonResult ToJsonResult(int page, int rows, string sord, List<IJqGridModel> listData)
        {
            var datas = listData.AsQueryable();
            var sortedData = _sortIQueryable(datas, sord);

            var data = (from s in sortedData
                        select new
                        {
                            id = s.GridUniqueID,
                            cell = s.GridFields
                        }).ToArray();


            var totalRecords = listData.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)rows);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return new JsonResult
            {
                Data = jsonData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}