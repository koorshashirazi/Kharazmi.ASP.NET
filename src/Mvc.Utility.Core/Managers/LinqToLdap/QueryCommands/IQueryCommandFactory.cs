using Mvc.Utility.Core.Managers.LinqToLdap.Mapping;
using Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands.Options;

namespace Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands
{
    internal interface IQueryCommandFactory
    {
        IQueryCommand GetCommand(QueryCommandType type, IQueryCommandOptions options, IObjectMapping mapping);
    }
}