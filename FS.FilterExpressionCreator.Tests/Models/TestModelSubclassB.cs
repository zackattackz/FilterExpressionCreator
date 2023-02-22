using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.FilterExpressionCreator.Tests.Models
{
    public class TestModelSubclassB<TValue> : TestModel<TValue>
    {
        public TValue SubValueB { get; set; }
    }
}
