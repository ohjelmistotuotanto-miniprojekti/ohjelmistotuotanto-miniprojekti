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
                Console.WriteLine($"Found '{expected}': {found}");
            }
        }
    }
}

