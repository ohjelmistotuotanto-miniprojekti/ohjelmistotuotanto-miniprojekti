using Xunit;
using Moq;
using System.Collections.Generic;
using ReferenceManager;

namespace ReferenceManager.Tests
{
    public class ReferenceManagerTests
    {
        [Fact]
        public void Test_AddInProceedingsConfirms()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Vihavainen, Arto") // Author
                .Returns("")                // confirms
                .Returns("Extreme Apprenticeship Method in Teaching Programming for Beginners.") // Title
                .Returns("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education") // BookTitle
                .Returns("2011")            // Year
                .Returns("")  // Editor
                .Returns("")  // Volume
                .Returns("")  // Series
                .Returns("")  // Pages
                .Returns("")  // Address
                .Returns("")  // Month
                .Returns("")  // Organization
                .Returns("")  // Publisher
                .Returns("")  // Note
                .Returns("")  // key
                .Returns("y");              // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

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
            Assert.Equal("", addedReference.Editor);
            Assert.Equal("", addedReference.Volume);
            Assert.Equal("", addedReference.Series);
            Assert.Equal("", addedReference.Pages);
            Assert.Equal("", addedReference.Address);
            Assert.Equal("", addedReference.Month);
            Assert.Equal("", addedReference.Organization);
            Assert.Equal("", addedReference.Publisher);
            Assert.Equal("", addedReference.Note);
            Assert.Equal("Vihavainen2011E", addedReference.Key);
            mockIO.Verify(io => io.Write("Adding an inproceedings article..."), Times.Once);
        }


        [Fact]
        public void Test_UserCancelsJournalArticle()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("")                // confirms
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("")
                .Returns("")             // Volume
                .Returns("")
                .Returns("")          // Pages
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("n");             // Confirmation ('n' = user cancels)

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            Assert.NotNull(mockIO.Object);

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

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
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Vihavainen, Arto") // Author
                .Returns("")                // confirms
                .Returns("Extreme Apprenticeship Method in Teaching Programming for Beginners.") // Title
                .Returns("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education") // BookTitle
                .Returns("2011")            // Year
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("")
                .Returns("n");              // Confirmation ('n' = user cancels)

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Empty(references); // Ensure no references are added
            mockIO.Verify(io => io.Write("Operation cancelled by the user."), Times.Once);
        }


        [Fact]
        public void Test_AddJournalInProceedingsUserDoesNotGiveNeededInformation()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Virtanen Juho")  // Author
                .Returns("")  // Confirm authors
                .Returns("Extreme Apprenticeship")  // Title
                .Returns("SIGCSE '11")  // Book Title
                .Returns("2011")  // Year
                .Returns("")  // Month (optional)
                .Returns("")  // Editor (optional)
                .Returns("4")  // Volume
                .Returns("")  // Series (optional)
                .Returns("")  // Pages (optional)
                .Returns("")  // Address (optional)
                .Returns("")  // Organization (optional)
                .Returns("")  // Publisher (optional)
                .Returns("")  // Note (optional)
                .Returns("Key123")  // Key
                .Returns("y");  // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Single(references); // Ensure one reference is added
            var addedReference = references[0] as InProceedingsReference;
            Assert.NotNull(addedReference);
            Assert.Equal("Virtanen Juho", addedReference.Author);
            Assert.Equal("Extreme Apprenticeship", addedReference.Title);
            Assert.Equal("2011", addedReference.Year);
            Assert.Equal("SIGCSE '11", addedReference.BookTitle);
            Assert.Equal("4", addedReference.Volume);
        }

        
        [Fact]
        public void Test_AddJournalArticleWithInvalidInputs()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            mockIO.SetupSequence(io => io.Read())
                .Returns("")               // Invalid Author (empty)
                .Returns("John Doe")       // Valid Author
                .Returns("")               // Confirmation
                .Returns("")               // Invalid Title (empty)
                .Returns("Sample Title")   // Valid Title
                .Returns("")               // Invalid Journal (empty)
                .Returns("Tech Journal")   // Valid Journal
                .Returns("")               // Invalid Year (empty)
                .Returns("abcd")           // Invalid Year (non-numeric)
                .Returns("2024")           // Valid Year
                .Returns("January")        // Month
                .Returns("A1")             // Volume (invalid)
                .Returns("2")              // Volume (valid)
                .Returns("page 1-2")       // Pages (invalid)
                .Returns("14--16")         // Pages (valid)
                .Returns("")               // Doi
                .Returns("")               // Note
                .Returns("")               // Key
                .Returns("y");             // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

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
            Assert.Equal("14--16", addedReference.Pages);
            Assert.Equal("January", addedReference.Month);
        }
        


        [Fact]
        public void Test_AddInProceedingsWithKey()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Virtanen Juho")  // Author
                .Returns("")                // Confirm authors
                .Returns("Extreme Apprenticeship")  // Title
                .Returns("SIGCSE '11")  // Book Title
                .Returns("2011")  // Year
                .Returns("")  // Month (optional)
                .Returns("")  // Editor (optional)
                .Returns("4")  // Volume
                .Returns("")  // Series (optional)
                .Returns("")  // Pages (optional)
                .Returns("")  // Address (optional)
                .Returns("")  // Organization (optional)
                .Returns("")  // Publisher (optional)
                .Returns("")  // Note (optional)
                .Returns("Key123")  // Key
                .Returns("y");  // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Single(references); // Ensure one reference is added
            var addedReference = references[0] as InProceedingsReference;
            Assert.NotNull(addedReference);
            Assert.Equal("Virtanen Juho", addedReference.Author);
            Assert.Equal("Extreme Apprenticeship", addedReference.Title);
            Assert.Equal("2011", addedReference.Year);
            Assert.Equal("SIGCSE '11", addedReference.BookTitle);
            Assert.Equal("4", addedReference.Volume);
        }


        
        [Fact]
        public void Test_AddJournalArticleWithKey()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("")               // Confirmation
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("")               // Month
                .Returns("")               // Volume
                .Returns("")               // Pages
                .Returns("")               // Doi
                .Returns("")               // Note
                .Returns("customKey")      // Key
                .Returns("y");             // Confirmation

            mockIO.Setup(io => io.Write(It.IsAny<string>()));

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.AddJournalArticle(references);

            // Assert
            Assert.Single(references); // Ensure one reference is added
            var addedReference = references[0] as ArticleReference;
            Assert.NotNull(addedReference);
            Assert.Equal("customKey", addedReference.Key);

            // Verify relevant output
            mockIO.Verify(io => io.Write("Adding journal article..."), Times.Once);
        }
        
        [Fact]
        public void HelpCommandListsAvailableCommands()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            mockIO.SetupSequence(io => io.Read())
                .Returns("help")
                .Returns("exit");

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.Run();

            // Assert
            mockIO.Verify(io => io.Write("Available commands:"), Times.Once);
            mockIO.Verify(io => io.Write("  add - Add a new reference"), Times.Once);
            mockIO.Verify(io => io.Write("  list - List all references"), Times.Once);
            mockIO.Verify(io => io.Write("  filter - Filter references by author, journal, year, or title"), Times.Once);
            mockIO.Verify(io => io.Write("  help - Show available commands"), Times.Once);
            mockIO.Verify(io => io.Write("  exit - Exit the application"), Times.Once);
            mockIO.Verify(io => io.Write("Exiting the application. Goodbye!"), Times.Once);
        }

        [Fact]
        public void UnkownCommandPrintsCorrectOutput()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            mockIO.SetupSequence(io => io.Read())
                .Returns("unknown")
                .Returns("exit");

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.Run();

            // Assert
            mockIO.Verify(io => io.Write(It.Is<string>(s => s.Contains("Unknown command. Type 'help' to see available commands."))), Times.Once);
        }

        [Fact]
        public void InvalidCommandAtReferenceSelectionReturnsToMainMenu()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            mockIO.SetupSequence(io => io.Read())
                .Returns("add")
                .Returns("invalidcommand")
                .Returns("exit");

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.Run();

            // Assert
            mockIO.Verify(io => io.Write(It.Is<string>(s => s.Contains("Invalid choice. Returning to main menu."))), Times.Once);
            mockIO.Verify(io => io.Write(It.Is<string>(s => s.Contains("Choose a command (type 'help' for available commands):"))), Times.Exactly(2));

        }

        [Fact]

        public void filterbyOneAuthor()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            references.Add(new ArticleReference
            {
                Author = "John Doe",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("John Doe");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify relevant output
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip):"), Times.Once);
            mockIO.Verify(io => io.Write("@article{John2024S"), Times.Once);
            mockIO.Verify(io => io.Write("author ={John Doe}"), Times.Once);
            mockIO.Verify(io => io.Write("title = {Sample Title},"), Times.Once);
            mockIO.Verify(io => io.Write("journal = {Tech Journal},"), Times.Once);
            mockIO.Verify(io => io.Write("title = {Sample Title},"), Times.Once);
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