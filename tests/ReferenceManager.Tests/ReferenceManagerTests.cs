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
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("John Doe");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);
        }

        [Fact]
        public void filterbyOneAuthorButFoundMultiple()
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
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("Doe");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);

            // Verify second reference
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {Liisa Doe}") &&
                s.Contains("title = {Sample Title2}") &&
                s.Contains("journal = {Tech Journal2}") &&
                s.Contains("year = {2022}"))),
                Times.Once);
        }
        
        [Fact]
        public void filterbyOneAuthorButFoundNothing()
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
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("Maija Doe");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("No references match the given criteria."), Times.Once);
        }

        [Fact]

        public void filterbyOneAuthorExplicitly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Sample Title3",
                Journal = "Tech Journal3",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John",
                Title = "Sample Title4",
                Journal = "Tech Journal4",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John, Liisa Doe, John Doe",
                Title = "Sample Title5",
                Journal = "Tech Journal5",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("\"John Doe\"");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe, Liisa Doe, John Doa}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);

            // Verify second reference
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John, Liisa Doe, John Doe}") &&
                s.Contains("title = {Sample Title5}") &&
                s.Contains("journal = {Tech Journal5}") &&
                s.Contains("year = {2022}"))),
                Times.Once);
        }
        
        [Fact]
        public void filterbyMultipleAuthorsExplicitly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Sample Title3",
                Journal = "Tech Journal3",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John",
                Title = "Sample Title4",
                Journal = "Tech Journal4",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John, Liisa Doe, John Doe",
                Title = "Sample Title5",
                Journal = "Tech Journal5",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("\"John Doe\", \"Liisa Doe\"");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe, Liisa Doe, John Doa}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);
        }

        [Fact]
        public void filterAuthorExplicitlyAndBroadlyAtOnce()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Sample Title3",
                Journal = "Tech Journal3",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John",
                Title = "Sample Title4",
                Journal = "Tech Journal4",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John, Liisa Doe, John Doe",
                Title = "Sample Title5",
                Journal = "Tech Journal5",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("author")
                .Returns("\"John Doe\", Liisa");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter author (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe, Liisa Doe, John Doa}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);
        }

        [Fact]
        public void filterTitleBroadlyFoundMultiple()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Title",
                Journal = "Tech Journal3",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("title")
                .Returns("Sample Title");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter title (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe, Liisa Doe, John Doa}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);
            
            // Verify second reference
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {Liisa Doe}") &&
                s.Contains("title = {Sample Title2}") &&
                s.Contains("journal = {Tech Journal2}") &&
                s.Contains("year = {2022}"))),
                Times.Once);
        }

 [Fact]
        public void filterTitleExplicitly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "Liisa Doe",
                Title = "Sample Title2",
                Journal = "Tech Journal2",
                Year = "2022"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Title",
                Journal = "Tech Journal3",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("title")
                .Returns("\"Sample Title\"");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter title (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe, Liisa Doe, John Doa}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);
        }

        [Fact]

        public void filterJournalBroadly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new InProceedingsReference
            {
                Author = "Jane Smith",
                Title = "Conference Paper",
                BookTitle = "Tech Journal",
                Year = "2023"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Title",
                Journal = "Tech",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("journal")
                .Returns("Tech Journal");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter journal (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doe, Liisa Doe, John Doa}") &&
                s.Contains("title = {Sample Title}") &&
                s.Contains("journal = {Tech Journal}") &&
                s.Contains("year = {2024}"))),
                Times.Once);
        }

        [Fact]

        public void filterJournalExplicitly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();
            references.Add(new InProceedingsReference
            {
                Author = "Jane Smith",
                Title = "Conference Paper",
                BookTitle = "Tech",
                Year = "2023"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doe, Liisa Doe, John Doa",
                Title = "Sample Title",
                Journal = "Tech Journal",
                Year = "2024"
            });
            references.Add(new ArticleReference
            {
                Author = "John Doa",
                Title = "Title",
                Journal = "Tech",
                Year = "2022"
            });

            mockIO.SetupSequence(io => io.Read())
                .Returns("journal")
                .Returns("\"Tech\"");

            mockIO.Setup(io => io.Write(It.IsAny<string>()))
                .Verifiable();

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            // Act
            program.FilterReferences(references);

            // Verify initial prompts
            mockIO.Verify(io => io.Write("Select filter criteria (e.g., 'author year', 'title', 'author journal'):"), Times.Once);
            mockIO.Verify(io => io.Write("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)"), Times.Once);
            mockIO.Verify(io => io.Write("Available criteria: author, journal, year, title"), Times.Once);
            mockIO.Verify(io => io.Write("Enter journal (or leave blank to skip): "), Times.Once);
            mockIO.Verify(io => io.Write("Filtered references (matching criteria):"), Times.Once);

            // Verify BibTeX output using It.Is to match the exact format
            mockIO.Verify(io => io.Write(It.Is<string>(s =>
                s.StartsWith("@article{") &&
                s.Contains("author = {John Doa}") &&
                s.Contains("title = {Title}") &&
                s.Contains("journal = {Tech}") &&
                s.Contains("year = {2022}"))),
                Times.Once);
        }

        



        [Fact]
        public void TestPrintReferences()
        {
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            var references = new List<Reference>();

            // Arrange
            string tempFilePath = Path.GetTempFileName();

            // Set up the mock to capture output
            var outputCapture = new StringWriter();
            mockIO.Setup(io => io.Write(It.IsAny<string>())).Callback<string>(s => outputCapture.Write(s));

            // Create a reference to test
            var referenceContent = "@article{Kalle1999T,\n" +
                                   "author = {Kalle, Eetu},\n" +
                                   "title = {Taiteiden perusteet},\n" +
                                   "journal = {Kallen tarinat},\n" +
                                   "year = {1999},\n" +
                                   "volume = {3},\n" +
                                   "pages = {37--59}\n" +
                                   "}";

            // Simulate writing the reference to the file
            File.WriteAllText(tempFilePath, referenceContent);

            try
            {
                // Set file path for the ReferenceManager
                Program.FilePath = tempFilePath;  

                // Create the Program instance and call PrintReferences
                var program = new Program(mockIO.Object, mockReferenceLoader.Object);
                program.PrintReferences(references);  

                // Act
                string actualOutput = outputCapture.ToString().Trim();

                // Assert
                string expectedOutput =
                    "Listing all references:\n" +
                    "References from file:" +
                    "Kalle, Eetu. Taiteiden perusteet. Kallen tarinat. 1999. 3. 37--59.";

                Assert.Equal(expectedOutput, actualOutput);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        [Fact]
        public void LoadReferencesFromFile_WithValidArticleReference_LoadsCorrectly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            string tempFilePath = Path.GetTempFileName();
            string referenceContent = "@article{Smith2023T,\n" +
                                "author = {Smith, John},\n" +
                                "title = {Test Title},\n" +
                                "journal = {Test Journal},\n" +
                                "year = {2023}\n" +
                                "}";

            File.WriteAllText(tempFilePath, referenceContent);
            Program.FilePath = tempFilePath;

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            try
            {
                // Act
                var references = program.LoadReferencesFromFile();

                // Assert
                Assert.Single(references);
                var reference = references[0] as ArticleReference;
                Assert.NotNull(reference);
                Assert.Equal("Smith, John", reference.Author);
                Assert.Equal("Test Title", reference.Title);
                Assert.Equal("Test Journal", reference.Journal);
                Assert.Equal("2023", reference.Year);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }


        [Fact]
        public void LoadReferencesFromFile_WithMultipleReferences_LoadsAllCorrectly()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            string tempFilePath = Path.GetTempFileName();
            string referenceContent = 
                "@article{Smith2023T,\n" +
                "author = {Smith, John},\n" +
                "title = {Test Title},\n" +
                "journal = {Test Journal},\n" +
                "year = {2023}\n" +
                "}\n\n" +
                "@inproceedings{Jones2022P,\n" +
                "author = {Jones, Bob},\n" +
                "title = {Conference Paper},\n" +
                "booktitle = {Test Conference},\n" +
                "year = {2022}\n" +
                "}";

            File.WriteAllText(tempFilePath, referenceContent);
            Program.FilePath = tempFilePath;

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            try
            {
                // Act
                var references = program.LoadReferencesFromFile();

                // Assert
                Assert.Equal(2, references.Count);
                Assert.Contains(references, r => r is ArticleReference);
                Assert.Contains(references, r => r is InProceedingsReference);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void LoadReferencesFromFile_WithNonexistentFile_ReturnsEmptyList()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            
            // Ensure the file path points to a nonexistent file
            string nonexistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            Program.FilePath = nonexistentPath;
            
            // Explicitly set up the mock to return empty list
            mockReferenceLoader
                .Setup(l => l.LoadReferences())
                .Returns(new List<Reference>());
            
            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            try 
            {
                // Act
                var references = program.LoadReferencesFromFile();

                // Assert
                Assert.Empty(references);
                mockIO.Verify(io => io.Write("References file not found."), Times.Once);
            }
            finally 
            {
                // Cleanup
                if (File.Exists(nonexistentPath))
                {
                    File.Delete(nonexistentPath);
                }
            }
        }

        [Fact]
        public void LoadReferencesFromFile_WithInvalidFormat_HandlesErrorGracefully()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var mockReferenceLoader = new Mock<IReferenceLoader>();
            string tempFilePath = Path.GetTempFileName();
            string invalidContent = "This is not a valid BibTeX format";

            File.WriteAllText(tempFilePath, invalidContent);
            Program.FilePath = tempFilePath;

            var program = new Program(mockIO.Object, mockReferenceLoader.Object);

            try
            {
                // Act
                var references = program.LoadReferencesFromFile();

                // Assert
                Assert.Empty(references);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void GetAuthors_SingleAuthor_ReturnsCorrectly()
        {
            var mockIO = new Mock<ConsoleIO>();
            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")    // First author
                .Returns("");           // Empty to finish

            var program = new Program(mockIO.Object, Mock.Of<IReferenceLoader>());
            var result = program.GetAuthors();

            Assert.Equal("John Doe", result);
        }

        [Fact]
        public void GetAuthors_MultipleAuthors_ReturnsSortedList()
        {
            var mockIO = new Mock<ConsoleIO>();
            mockIO.SetupSequence(io => io.Read())
                .Returns("Charles Darwin")
                .Returns("Alan Turing")
                .Returns("Bob Smith")
                .Returns("");

            var program = new Program(mockIO.Object, Mock.Of<IReferenceLoader>());
            var result = program.GetAuthors();

            Assert.Equal("Alan Turing, Bob Smith, Charles Darwin", result);
        }

        [Fact]
        public void GetAuthors_EmptyInput_PromptsUntilValid()
        {
            var mockIO = new Mock<ConsoleIO>();
            mockIO.SetupSequence(io => io.Read())
                .Returns("")            // Empty input first
                .Returns("   ")        // Whitespace input
                .Returns("John Doe")   // Valid input
                .Returns("");          // Finish

            var program = new Program(mockIO.Object, Mock.Of<IReferenceLoader>());
            program.GetAuthors();

            mockIO.Verify(io => io.Write("At least one author is required. Please add an author."), Times.Exactly(2));
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
