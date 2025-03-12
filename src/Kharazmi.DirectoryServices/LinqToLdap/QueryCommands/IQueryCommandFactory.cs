using Kharazmi.DirectoryServices.Ldap.LinqToLdap.Mapping;
using Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands.Options;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.QueryCommands
{
    internal interface IQueryCommandFactory
    {
        IQueryCommand GetCommand(QueryCommandType type, IQueryCommandOptions options, IObjectMapping mapping);
    }
}