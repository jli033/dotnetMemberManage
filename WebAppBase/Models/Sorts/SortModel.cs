using SharedUtilitys.Models;

namespace WebAppBase.Models.Sorts
{
    public class SortModel : BaseModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public int ColumnOrder { get; set; }
    }
}