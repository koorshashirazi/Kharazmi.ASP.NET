namespace Kharazmi.AspNetMvc.Core.Models
{
    public interface IDeleteModel : IModel
    {
        byte[] RowVersion { get; set; }
    }
}
