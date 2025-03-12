using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;

namespace Mvc.Utility.Core.Models
{
    public class CreateFullTextIndexOperation : MigrationOperation
    {
        public CreateFullTextIndexOperation(object anonymousArguments = null) : base(anonymousArguments)
        {
        }

        public override bool IsDestructiveChange => false;
        public string Table { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Columns { get; set; }
    }
}