using Xunit;
using System.IO;
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
}
