namespace WebAppBase.Models.Base
{
    public interface IJqGridModel
    {
        int GridUniqueID { get; }
        object[] GridFields { get; }
    }
}