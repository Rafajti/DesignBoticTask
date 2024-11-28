using Moq;
using DesignBoticTask.Commands;
using System.Reflection.Metadata;

namespace DesignBoticTaskTests.Commands;

public class ShowSelectedPropertiesCommandTests
{
    private readonly Mock<ExternalCommandData> _mockCommandData;
    private readonly Mock<UIDocument> _mockUiDoc;
    private readonly Mock<Document> _mockDoc;

    public ShowSelectedPropertiesCommandTests()
    {
        // Initialize mocks
        _mockCommandData = new Mock<ExternalCommandData>();
        _mockUiDoc = new Mock<UIDocument>();
        _mockDoc = new Mock<Document>();

        // Setup mock command data to return mocked UIDocument
        _mockCommandData.Setup(cd => cd.Application.ActiveUIDocument).Returns(_mockUiDoc.Object);
        _mockUiDoc.Setup(doc => doc.Document).Returns(_mockDoc.Object);
    }

    [Fact]
    public void Execute_NoElementsSelected_ShowsMessageAndReturnsSucceeded()
    {
        // Arrange
        _mockUiDoc.Setup(doc => doc.Selection.GetElementIds()).Returns(new List<ElementId>());
        var command = new ShowSelectedPropertiesCommand();

        // Act
        string message = string.Empty;
        var result = command.Execute(_mockCommandData.Object, ref message, new ElementSet());

        // Assert
        Assert.Equal(Result.Succeeded, result);
        _mockUiDoc.Verify(doc => doc.Selection.GetElementIds(), Times.Once);
    }

    [Fact]
    public void Execute_ElementsSelected_DisplaysPropertiesInWindow()
    {
        // Arrange
        var mockElementId = new ElementId(1);
        var mockElement = new Mock<Element>();
        mockElement.Setup(e => e.Name).Returns("MockElement");

        var mockParameter = new Mock<Parameter>();
        var mockDefinition = new Mock<Definition>();
        mockDefinition.Setup(d => d.Name).Returns("MockParameter");
        mockParameter.Setup(p => p.Definition).Returns(mockDefinition.Object);
        mockParameter.Setup(p => p.AsString()).Returns("MockValue");

        mockElement.Setup(e => e.Parameters).Returns(new ParameterSet { mockParameter.Object });

        _mockUiDoc.Setup(doc => doc.Selection.GetElementIds()).Returns(new List<ElementId> { mockElementId });
        _mockDoc.Setup(d => d.GetElement(mockElementId)).Returns(mockElement.Object);

        var command = new ShowSelectedPropertiesCommand();

        // Act
        string message = string.Empty;
        var result = command.Execute(_mockCommandData.Object, ref message, new ElementSet());

        // Assert
        Assert.Equal(Result.Succeeded, result);
        mockElement.Verify(e => e.Name, Times.Once);
        mockParameter.Verify(p => p.AsString(), Times.Once);
    }
}
