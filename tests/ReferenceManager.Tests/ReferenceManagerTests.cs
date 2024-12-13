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
        public void Test_AddJournalArticle_SuccessfulAddition()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();
            var mockReference = new Mock<ArticleReference>();
            mockReference.Setup(r => r.ToBibtexFile()).Returns(true);

            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("")               // Confirm authors
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("")               // Month
                .Returns("5")              // Volume 
                .Returns("")               // Pages
                .Returns("")               // Doi
                .Returns("")               // Note
                .Returns("")               // Key
                .Returns("y");             // Confirmation

            var program = new Program(mockIO.Object, new MockReferenceLoader(references));

            // Act
            program.AddJournalArticle(references, () => mockReference.Object);

            // Assert
            Assert.Single(references);
            mockIO.Verify(io => io.Write("Journal article added successfully."), Times.Once);
        }


        [Fact]
        public void Test_AddJournalArticle_UserCancels()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();
            var mockReference = new Mock<ArticleReference>();
            mockReference.Setup(r => r.ToBibtexFile()).Returns(true);

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("John Doe")       // Author
                .Returns("")               // Confirm authors
                .Returns("Sample Title")   // Title
                .Returns("Tech Journal")   // Journal
                .Returns("2024")           // Year
                .Returns("n");             // User cancels


            var program = new Program(mockIO.Object, new MockReferenceLoader(references));

            // Act
            program.AddJournalArticle(references, () => mockReference.Object);

            // Assert
            // Ensure no references were added
            Assert.Empty(references);

            // Verify that the cancellation message is written
            mockIO.Verify(io => io.Write("Operation cancelled by the user."), Times.Once);

            // Verify `ToBibtexFile` is not called since the user cancelled
            mockArticleReference.Verify(ar => ar.ToBibtexFile(), Times.Never);
        }


        [Fact]
        public void Test_AddInProceedingsConfirms()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();
            var mockLoader = new ReferenceManager.Tests.MockReferenceLoader(references); // Instantiate MockReferenceLoader with references

            // Simulated user input
            mockIO.SetupSequence(io => io.Read())
                .Returns("Vihavainen, Arto") // Author
                .Returns("")                // Confirm authors
                .Returns("Extreme Apprenticeship Method in Teaching Programming for Beginners.") // Title
                .Returns("SIGCSE '11")      // Book Title
                .Returns("2011")            // Year
                .Returns("y");              // Confirmation

            var program = new Program(mockIO.Object, mockLoader); // Pass MockReferenceLoader

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Single(references); // Ensure one reference is added
            var addedReference = references[0] as InProceedingsReference;
            Assert.NotNull(addedReference);
            Assert.Equal("Vihavainen, Arto", addedReference.Author);
            Assert.Equal("Extreme Apprenticeship Method in Teaching Programming for Beginners.", addedReference.Title);
            Assert.Equal("2011", addedReference.Year);
            Assert.Equal("SIGCSE '11", addedReference.BookTitle);
        }


        [Fact]
        public void Test_UserCancelsInProceedings()
        {
            // Arrange
            var mockIO = new MockConsoleIO(new[] // Use MockConsoleIO for simulated input/output
            {
                "Vihavainen, Arto", // Author
                "",                 // Confirm authors
                "Extreme Apprenticeship Method in Teaching Programming for Beginners.", // Title
                "SIGCSE '11",       // Book Title
                "2011",             // Year
                "n"                 // Confirmation ('n' = user cancels)
            });

            var mockLoader = new MockReferenceLoader(new List<Reference>()); // Use MockReferenceLoader with no references
            var program = new Program(mockIO, mockLoader); // Pass mockLoader
            var references = new List<Reference>();

            // Act
            program.AddInProceedings(references);

            // Assert
            Assert.Empty(references); // Ensure no references are added
            Assert.Contains("Operation cancelled by the user.", mockIO.GetOutputs()); // Verify cancellation message
        }


        /*
                [Fact]
                public void Test_AddJournalInProceedingsUserDoesNotGiveNeededInformation()
                {
                    // Arrange
                    var mockIO = new Mock<ConsoleIO>();
                    var references = new List<Reference>();
                    var mockLoader = new Mock<IReferenceLoader>(); // Add mockLoader

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

                    var program = new Program(mockIO.Object, mockLoader.Object); // Pass mockLoader

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

        /*
                [Fact]
                public void Test_AddInProceedingsWithKey()
                {
                    // Arrange
                    var mockIO = new Mock<ConsoleIO>();
                    var mockLoader = new Mock<IReferenceLoader>(); // Declare mockLoader
                    var references = new List<Reference>();

                    // Simulated user input
                    mockIO.SetupSequence(io => io.Read())
                        .Returns("Virtanen Juho")  // Author
                        .Returns("")               // Confirm authors
                        .Returns("Extreme Apprenticeship")  // Title
                        .Returns("SIGCSE '11")     // Book Title
                        .Returns("2011")           // Year
                        .Returns("")               // Month (optional)
                        .Returns("")               // Editor (optional)
                        .Returns("4")              // Volume
                        .Returns("")               // Series (optional)
                        .Returns("")               // Pages (optional)
                        .Returns("")               // Address (optional)
                        .Returns("")               // Organization (optional)
                        .Returns("")               // Publisher (optional)
                        .Returns("")               // Note (optional)
                        .Returns("Key123")         // Key
                        .Returns("y");             // Confirmation

                    mockIO.Setup(io => io.Write(It.IsAny<string>()));

                    var program = new Program(mockIO.Object, mockLoader.Object); // Pass mockLoader

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
                public void FilterReferences_NoReferences_ShouldOutputMessage()
                {
                    // Arrange
                    var mockLoader = new Mock<IReferenceLoader>();
                    mockLoader.Setup(loader => loader.LoadReferences()).Returns(new List<Reference>());

                    var mockIO = new Mock<ConsoleIO>();
                    mockIO.Setup(io => io.Read()).Returns(""); // Simulate no criteria entered
                    var program = new Program(mockIO.Object, mockLoader.Object);

                    // Act
                    using (var consoleOutput = new StringWriter())
                    {
                        Console.SetOut(consoleOutput);

                        program.FilterReferences();

                        // Assert
                        var output = consoleOutput.ToString();
                        Assert.Contains("No references found in the file.", output);
                    }
                }

                [Fact]
                public void Test_FilterReferences_NoReferences()
                {
                    // Arrange
                    var mockIO = new MockConsoleIO(new[] { "" }); // Simulate no criteria entered
                    var mockLoader = new MockReferenceLoader(new List<Reference>()); // No references

                    var program = new Program(mockIO, mockLoader);

                    // Act
                    program.FilterReferences();

                    // Assert
                    Assert.Contains("No references found in the file.", mockIO.GetOutputs());
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

    public class MockReferenceLoader : IReferenceLoader
    {
        private readonly List<Reference> _references;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockReferenceLoader"/> class with the provided references.
        /// </summary>
        /// <param name="references">The references to simulate as loaded by the loader.</param>
        public MockReferenceLoader(IEnumerable<Reference> references)
        {
            _references = new List<Reference>(references);
        }

        /// <summary>
        /// Simulates loading references.
        /// </summary>
        /// <returns>The list of mock references.</returns>
        public List<Reference> LoadReferences()
        {
            return _references;
        }
    }
}
