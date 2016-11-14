using SharedUtilitys.Models;
using System.Collections;

namespace WebAppBase.Models.Base
{
    public class SortTargetModel : BaseModel
    {
        public SortTargetModel()
        {
            DisplayNoColumn = "DisplayNo";
            DisplayFlagColumn = "DisplayFlag";
        }

        public string OrganizationID { get; set; }
        public string TableName { get; set; }
        public string IdColumn { get; set; }
        public string DisplayColumn { get; set; }
        public string DisplayNoColumn { get; set; }
        public string DisplayFlagColumn { get; set; }
        public string StatusFlagColumn { get; set; }
        public string RedirectController { get; set; }
        public string RedirectAction { get; set; }

        public object RouteValues { get; set; }
        public IList SortItems { get; set; }

    }
}