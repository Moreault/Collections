using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBX.Collections.UnitTesting.Customizations;
using ToolBX.Collections.UnitTesting.Extensions;
using ToolBX.Eloquentest.Customizations;

namespace Collections.ReadOnly.Tests.Customizations;

[AutoCustomization]
public sealed class UnitTestingCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.WithCollectionCustomizations();
    }
}