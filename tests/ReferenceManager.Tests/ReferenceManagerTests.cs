using Xunit;
using Moq;
using System.Collections.Generic;
using ReferenceManager;

namespace ReferenceManager.Tests
{
    public class ReferenceManagerTests
    {
        [Fact]
        public void Test_AddJournalArticleConfirms()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("12")             // Volume
                .Returns("34-56")          // Pages
                .Returns("y");             // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object);

            // Act
            program.AddJournalArticle(references);

            // Assert
            Assert.Single(references); // Ensure one reference is added
            var addedReference = references[0] as ArticleReference;
            Assert.NotNull(addedReference);
            Assert.Equal("John Doe", addedReference.Author);
            Assert.Equal("Sample Title", addedReference.Title);
            Assert.Equal("Tech Journal", addedReference.Journal);
            Assert.Equal("2024", addedReference.Year);
            Assert.Equal("12", addedReference.Volume);
            Assert.Equal("34-56", addedReference.Pages);

            mockIO.Verify(io => io.Write("Adding journal article..."), Times.Once);
        }

        [Fact]
        public void Test_AddInProceedingsConfirms()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Vihavainen, Arto") // Author
                .Returns("Extreme Apprenticeship Method in Teaching Programming for Beginners.") // Title
                .Returns("2011")            // Year
                .Returns("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education") // BookTitle
                .Returns("y");              // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object);

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Single(references); // Ensure one reference is added
            var addedReference = references[0] as InProceedingsReference;
            Assert.NotNull(addedReference);
            Assert.Equal("Vihavainen, Arto", addedReference.Author);
            Assert.Equal("Extreme Apprenticeship Method in Teaching Programming for Beginners.", addedReference.Title);
            Assert.Equal("2011", addedReference.Year);
            Assert.Equal("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education", addedReference.BookTitle);

            mockIO.Verify(io => io.Write("Adding an inproceedings article..."), Times.Once);
        }

        [Fact]
        public void Test_UserCancelsJournalArticle()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("12")             // Volume
                .Returns("34-56")          // Pages
                .Returns("n");             // Confirmation ('n' = user cancels)

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object);

            // Act
            program.AddJournalArticle(references);

            // Assert
            Assert.Empty(references); // Ensure no references are added
            mockIO.Verify(io => io.Write("Operation cancelled by the user."), Times.Once);
        }

        [Fact]
        public void Test_UserCancelsInProceedings()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Vihavainen, Arto") // Author
                .Returns("Extreme Apprenticeship Method in Teaching Programming for Beginners.") // Title
                .Returns("2011")            // Year
                .Returns("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education") // BookTitle
                .Returns("n");              // Confirmation ('n' = user cancels)

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object);

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Empty(references); // Ensure no references are added
            mockIO.Verify(io => io.Write("Operation cancelled by the user."), Times.Once);
        }

        [Fact]
        public void TestGetOneAuthor()
        {
            var mockIO = new Mock<ConsoleIO>();

            // Simulate user input: "John Doe" followed by an empty input to end the loop
            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe") // Author
                .Returns("");        // End input

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object);

            // Act
            var authors = program.GetAuthors();

            // Assert
            Assert.Equal("John Doe", authors); // Verify returned authors string

            // Verify that the correct prompts were written
            mockIO.Verify(io => io.Write("Enter author name, at least one author required:"), Times.Exactly(2));
        }

        [Fact]
        public void Test_GetAuthorsWithEmptyInputFirst()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();

            // Simulate user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("")        // Empty input
                .Returns("John Doe") // Valid input
                .Returns("");       // End input

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object);

            // Act
            var authors = program.GetAuthors();

            // Assert
            Assert.Equal("John Doe", authors); // Verify returned authors string
            mockIO.Verify(io => io.Write("At least one author is Required. Please add author"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author name, at least one author required:"), Times.Exactly(3));
        }

    }

    /// <summary>
    /// Mock implementation of ConsoleIO for testing.
    /// </summary>
    public class MockConsoleIO : ReferenceManager.ConsoleIO
    {
        private readonly Queue<string> _inputs;
        private readonly List<string> _outputs;

        public MockConsoleIO(IEnumerable<string> inputs)
        {
            _inputs = new Queue<string>(inputs);
            _outputs = new List<string>();
        }

        public override void Write(string text)
        {
            _outputs.Add(text);
        }

        public override string Read()
        {
            return _inputs.Count > 0 ? _inputs.Dequeue() : string.Empty;
        }

        public IEnumerable<string> GetOutputs()
        {
            return _outputs;
        }
    }
}
