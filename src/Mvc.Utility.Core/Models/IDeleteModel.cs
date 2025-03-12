namespace Mvc.Utility.Core.Models
{
    public interface IDeleteModel : IModel
    {
        byte[] RowVersion { get; set; }
    }
}
