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
                .Returns("")               // Month
                .Returns("12")             // Volume
                .Returns("")               // Number
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
                .Returns("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education") // BookTitle
                .Returns("2011")            // Year
                .Returns("")  // Editor
                .Returns("")  // Volume
                .Returns("")  // Number
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
                .Returns("")
                .Returns("")             // Volume
                .Returns("")
                .Returns("")          // Pages
                .Returns("")
                .Returns("")
                .Returns("")
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
                .Returns("")  // Failed Author
                .Returns("")  // Failed Author
                .Returns("")  // Failed Author
                .Returns("Virtanen Juho") // Author
                .Returns("")  // Failed Title
                .Returns("")  // Failed Title
                .Returns("Extreme Apprenticeship") // Title
                .Returns("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education") // BookTitle
                .Returns("")  // Failed Year
                .Returns("2011")  // Year
                .Returns("12")  // Month
                .Returns("Kalle")  // Editor
                .Returns("4")  // Volume
                .Returns("3")  // Number
                .Returns("2")  // Series
                .Returns("6-8")  // Pages
                .Returns("kaivokatu")  // Address
                .Returns("jyväskylän yliopisto")  // Organization
                .Returns("yliopisto")  // Publisher
                .Returns("note")  // Note
                .Returns("Vir2011")  // key
                .Returns("y");              // Confirmation

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
            Assert.Equal("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education", addedReference.BookTitle);
            Assert.Equal("3", addedReference.Number);
            Assert.Equal("12", addedReference.Month);
            Assert.Equal("Kalle", addedReference.Editor);
            Assert.Equal("4", addedReference.Volume);
            Assert.Equal("2", addedReference.Series);
            Assert.Equal("6-8", addedReference.Pages);
            Assert.Equal("kaivokatu", addedReference.Address);
            Assert.Equal("jyväskylän yliopisto", addedReference.Organization);
            Assert.Equal("yliopisto", addedReference.Publisher);
            Assert.Equal("note", addedReference.Note);
            mockIO.Verify(io => io.Write("Adding an inproceedings article..."), Times.Once);
        }


                [Fact]
        public void Test_AddJournalArticleUserDoesNotGiveNeededInformation()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("")               // Failed Author
                .Returns("Hans Doen")      // Author
                .Returns("")               // Failed Title
                .Returns("Sample Title2")  // Title
                .Returns("")               // Failed Journal
                .Returns("")               // Failed Journal
                .Returns("")               // Failed Journal
                .Returns("lehti")          // Journal
                .Returns("")               // Failed Year
                .Returns("")               // Failed Year
                .Returns("")               // Failed Year
                .Returns("")               // Failed Year
                .Returns("")               // Failed Year
                .Returns("2025")           // Year
                .Returns("3")              // Month
                .Returns("12")             // Volume
                .Returns("6")              // Number
                .Returns("23-43")          // Pages
                .Returns("ffff")           // Doi
                .Returns("muistiinpano")   // Note
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
            Assert.Equal("Hans Doen", addedReference.Author);
            Assert.Equal("Sample Title2", addedReference.Title);
            Assert.Equal("lehti", addedReference.Journal);
            Assert.Equal("2025", addedReference.Year);
            Assert.Equal("12", addedReference.Volume);
            Assert.Equal("23-43", addedReference.Pages);
            Assert.Equal("ffff", addedReference.Doi);
            Assert.Equal("muistiinpano", addedReference.Note);
            Assert.Equal("3", addedReference.Month);
            Assert.Equal("6", addedReference.Number);


            mockIO.Verify(io => io.Write("Adding journal article..."), Times.Once);
        }



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
                    "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education", // BookTitle
                    "2011", // Year
                    "", //month
                    "", //editor
                    "", //volume
                    "", //Number
                    "", // series
                    "", // pages
                    "", // address
                    "", // organization
                    "", // publisher
                    "", // note
                    "", // key
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
