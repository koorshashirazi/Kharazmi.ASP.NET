namespace Mvc.Utility.Core.Models
{
    public interface IEditModel : IModel
    {
        byte[] RowVersion { get; set; }
    }
}