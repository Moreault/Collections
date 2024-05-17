namespace Collections.Grid.Tests;

[TestClass]
public class OverlapGridExtensionsTests
{
    [TestClass]
    public class ToOverlapGrid : ToolBX.Collections.UnitTesting.Tester
    {
        [TestMethod]
        public void WhenCellsIsNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> cells = null!;

            //Act
            var action = () => cells.ToOverlapGrid();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
        }

        [TestMethod]
        public void WhenCellsIsEmpty_CreateEmptyGrid()
        {
            //Arrange
            var cells = Array.Empty<Cell<Garbage>>();

            //Act
            var result = cells.ToOverlapGrid();

            //Assert
            result.Should().BeOfType<OverlapGrid<Garbage>>();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCellsIsNotEmpty_CreateOverlapGrid()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

            //Act
            var result = cells.ToOverlapGrid();

            //Assert
            result.Should().BeOfType<OverlapGrid<Garbage>>();
            result.Should().BeEquivalentTo(cells);
        }
    }
}