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
    }

    public class EndToEndTests
    {
        [Fact]
        public void EndToEnd_AddInProceedingsReference()
        {
            string tempFilePath = "test_referencesE2E.bib";
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                var consoleIO = new MockConsoleIO(new[]
                {
                    "add", // Add command
                    "2",   // InProceedings type
                    "Vihavainen, Arto", // Author
                    "Extreme Apprenticeship Method in Teaching Programming for Beginners.", // Title
                    "2011", // Year
                    "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education", // BookTitle
                    "y",    // Confirm addition
                    "list", // List command
                    "exit"  // Exit
                });

                var program = new ReferenceManager.Program(consoleIO);

                // Act
                program.Run();

                // Assert: Verify file content
                Assert.True(File.Exists(tempFilePath), "The BibTeX file was not created.");

                string fileContent = File.ReadAllText(tempFilePath).Trim();
                string expectedBibtex =
                    $"@inproceedings{{Vihavainen2011E,\n" +
                    $"  author = {{Vihavainen, Arto}},\n" +
                    $"  title = {{Extreme Apprenticeship Method in Teaching Programming for Beginners.}},\n" +
                    $"  booktitle = {{SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education}},\n" +
                    $"  year = {{2011}}\n" +
                    $"}}";

                Assert.Equal(expectedBibtex, fileContent);
            }
            finally
            {
                // Ensure file cleanup
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
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
