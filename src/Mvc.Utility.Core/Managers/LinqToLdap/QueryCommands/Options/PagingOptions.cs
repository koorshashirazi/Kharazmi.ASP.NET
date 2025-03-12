namespace Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands.Options
{
    internal class PagingOptions : IPagingOptions
    {
        public PagingOptions(int pageSize, byte[] nextPage)
        {
            NextPage = nextPage;
            PageSize = pageSize;
        }

        public byte[] NextPage { get; }
        public int PageSize { get; }
    }
}