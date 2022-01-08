using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections.ObservableList.Tests
{
    public record Dummy
    {
        public int Id { get; init; }
        public string Description { get; init; } = string.Empty;
    }
}