using Xunit;
using Moq;
using System.Collections.Generic;
using ReferenceManager;

namespace ReferenceManager.Tests
{
    public class ReferenceManagerTests
    {
        /*
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
                .Returns("")               // Month
                .Returns("12")             // Volume
                .Returns("34-56")          // Pages
                .Returns("")               // Doi
                .Returns("")               // Note
                .Returns("")               // Key
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
            Assert.Equal("", addedReference.Month);
            Assert.Equal("12", addedReference.Volume);
            //Assert.Equal("34-56", addedReference.Pages);
            Assert.Equal("", addedReference.Doi);
            Assert.Equal("", addedReference.Note);
            Assert.Equal("John2024S", addedReference.Key);

            mockIO.Verify(io => io.Write("Adding journal article..."), Times.Once);
        }*/

        [Fact]
        public void Test_AddInProceedingsConfirms()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
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
            //Assert.Equal("", addedReference.Editor);
            //Assert.Equal("", addedReference.Volume);
            //Assert.Equal("", addedReference.Series);
            //Assert.Equal("", addedReference.Pages);
            //Assert.Equal("", addedReference.Address);
            //Assert.Equal("", addedReference.Month);
            //Assert.Equal("", addedReference.Organization);
            //Assert.Equal("", addedReference.Publisher);
            //Assert.Equal("", addedReference.Note);
            Assert.Equal("Vihavainen2011E", addedReference.Key);
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

            var program = new Program(mockIO.Object);

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

            var program = new Program(mockIO.Object);

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

        /*
        [Fact]
        public void Test_AddJournalArticleWithInvalidInputs()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
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
        }
        */


        [Fact]
        public void Test_AddInProceedingsWithKey()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
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

            var program = new Program(mockIO.Object);

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


        /*
        [Fact]
        public void Test_AddJournalArticleWithKey()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("")                // confirms
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("2")              // Month
                .Returns("14")             // Volume
                .Returns("3-5")            // Pages
                .Returns("doi")            // DOI
                .Returns("muistiinpano")   // Note
                .Returns("key")            // Key
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
            Assert.Equal("key", addedReference.Key);
            Assert.Equal("14", addedReference.Volume);
            Assert.Equal("3-5", addedReference.Pages);
            Assert.Equal("doi", addedReference.Doi);
            Assert.Equal("muistiinpano", addedReference.Note);
            Assert.Equal("2", addedReference.Month);

            // Verify relevant output
            mockIO.Verify(io => io.Write("Adding journal article..."), Times.Once);
        }
        */

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
