namespace Kharazmi.AspNetMvc.Core.Models
{
    public interface IEditModel : IModel
    {
        byte[] RowVersion { get; set; }
    }
}