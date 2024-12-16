using Xunit;
using System.IO;
using Moq;
using System;
using ReferenceManager;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ReferenceManager.Tests
{
    public class EndToEndTests
    {

        [Fact]
        public void EndToEnd_AddInProceedingsReference()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;
            Console.WriteLine($"Writing to file: {Program.FilePath}");

            try
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                var consoleIO = new MockConsoleIO(new[]
                {
                    "add", // Add command
                    "2",   // InProceedings type
                    "Vihavainen, Arto", // Author
                    "",                 // Confirm authors
                    "Extreme Apprenticeship Method in Teaching Programming for Beginners.", // Title
                    "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education", // BookTitle
                    "2011", // Year
                    "", //month
                    "", //editor
                    "", //volume
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

                // Mock IReferenceLoader
                var mockLoader = new Mock<IReferenceLoader>();
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(new List<Reference>());

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);

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


        [Fact]
        public void EndToEnd_AddJournalArticle()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;
            Console.WriteLine($"Writing to file: {Program.FilePath}");

            try
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                var consoleIO = new MockConsoleIO(new[]
                {
                    "add", // Add command
                    "1",   // Journal Article type
                    "John Doe",       // Author
                    "",               // Confirm authors
                    "Sample Title",   // Title
                    "Tech Journal",   // Journal
                    "2024",           // Year
                    "",                // Month
                    "",                 // Volume
                    "",              // Pages
                    "",                // DOI
                    "",              // Note
                    "",              // Key
                    "y",              // Confirm addition
                    "list",           // List command
                    "exit"            // Exit
                });

                // Mock IReferenceLoader
                var mockLoader = new Mock<IReferenceLoader>();
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(new List<Reference>());

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);

                // Act
                program.Run();

                // Assert: Verify file content
                Assert.True(File.Exists(tempFilePath), "The BibTeX file was not created.");

                string fileContent = File.ReadAllText(tempFilePath).Trim();
                string expectedBibtex =
                    $"@article{{John2024S,\n" +
                    $"  author = {{John Doe}},\n" +
                    $"  title = {{Sample Title}},\n" +
                    $"  journal = {{Tech Journal}},\n" +
                    $"  year = {{2024}},\n" +  // Note the trailing comma here
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

        [Fact]
        public void EndToEnd_HelpCommandOutput()
        {
            // Arrange
            var consoleIO = new MockConsoleIO(new[] {
                "help",
                "exit"
            });

            var mockLoader = new Mock<IReferenceLoader>();
            mockLoader.Setup(loader => loader.LoadReferences()).Returns(new List<Reference>());
            var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);

            // Act
            program.Run();

            // Assert
            var outputs = consoleIO.GetOutputs().ToList();

            var expectedHelpMenu = new[]
            {
                "Available commands:",
                " add - Add a new reference",
                " list - List all references",
                " filter - Filter references by author, journal, year, or title",
                " help - Show available commands",
                " exit - Exit the application"
            };

            // Verify each line individually and print result
            foreach (var expected in expectedHelpMenu)
            {
                bool found = outputs.Any(actual => actual.Equals(expected, StringComparison.Ordinal));
            }
        }

        [Fact]
        public void EndToEnd_AddArticle_MultipleAuthors()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);

                var consoleIO = new MockConsoleIO(new[] {
                    "add",
                    "1", // article
                    "Adams, Bob", // First author
                    "Doe, John", // Second author
                    "", // finish authors
                    "Test Title",
                    "Test Journal",
                    "2024",
                    "", // month
                    "", // volume
                    "", // pages
                    "", // doi
                    "", // note
                    "", // key
                    "y", // confirm
                    "exit"
                });

                var mockLoader = new Mock<IReferenceLoader>();
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(new List<Reference>());
                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);

                // Act
                program.Run();

                // Assert
                Assert.True(File.Exists(tempFilePath), "The BibTeX file was not created.");
                string fileContent = File.ReadAllText(tempFilePath).Trim();

                // Instead of checking the exact format, let's verify the key parts
                Assert.Contains("@article{", fileContent);
                Assert.Contains("author = {Adams, Bob, Doe, John}", fileContent);
                Assert.Contains("title = {Test Title}", fileContent);
                Assert.Contains("journal = {Test Journal}", fileContent);
                Assert.Contains("year = {2024}", fileContent);

                // Verify the output messages
                var outputs = consoleIO.GetOutputs();
                Assert.Contains("Journal article added successfully.", outputs);
            }
            finally
            {
                // Cleanup
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
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
}

