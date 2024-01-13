namespace Collections.Grid.Tests;

[TestClass]
public class OverlapGridExtensionsTests
{
    [TestClass]
    public class ToOverlapGrid : Tester
    {
        [TestMethod]
        public void WhenCellsIsNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> cells = null!;

            //Act
            var action = () => cells.ToOverlapGrid();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
        }

        [TestMethod]
        public void WhenCellsIsEmpty_CreateEmptyGrid()
        {
            //Arrange
            var cells = Array.Empty<Cell<Dummy>>();

            //Act
            var result = cells.ToOverlapGrid();

            //Assert
            result.Should().BeOfType<OverlapGrid<Dummy>>();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCellsIsNotEmpty_CreateOverlapGrid()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Act
            var result = cells.ToOverlapGrid();

            //Assert
            result.Should().BeOfType<OverlapGrid<Dummy>>();
            result.Should().BeEquivalentTo(cells);
        }
    }
}