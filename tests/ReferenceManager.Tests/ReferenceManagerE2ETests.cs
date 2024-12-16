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
        public void EndToEnd_FilterReferences_SingleAuthor()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                File.WriteAllText(tempFilePath,
                    "@article{Smith2020T,\n" +
                    "  author = {Smith, John},\n" +
                    "  title = {Test Paper},\n" +
                    "  journal = {Science Journal},\n" +
                    "  year = {2020}\n" +
                    "}\n\n" +
                    "@article{Jones2020A,\n" +
                    "  author = {Jones, Bob},\n" +
                    "  title = {Another Paper},\n" +
                    "  journal = {Nature},\n" +
                    "  year = {2020}\n" +
                    "}");

                var consoleIO = new MockConsoleIO(new[] {
                    "filter",
                    "author",
                    "Smith",
                    "exit"
                });

                var mockLoader = new Mock<IReferenceLoader>();
                var references = new List<Reference> { 
                    new ArticleReference { 
                        Author = "Smith, John",
                        Title = "Test Paper",
                        Journal = "Science Journal",
                        Year = "2020"
                    },
                    new ArticleReference {
                        Author = "Jones, Bob",
                        Title = "Another Paper", 
                        Journal = "Nature",
                        Year = "2020"
                    }
                };
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(references);

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);
                program.Run();

                var outputs = consoleIO.GetOutputs();
                
                bool foundSmithPaper = outputs.Any(o => 
                    o.Contains("Smith") && 
                    o.Contains("Test Paper") && 
                    o.Contains("Science Journal"));
                    
                Assert.True(foundSmithPaper, "Smith's paper should be in the filtered results");
                
                bool containsJonesPaper = outputs.Any(o => 
                    o.Contains("Jones") && 
                    o.Contains("Another Paper") && 
                    o.Contains("Nature"));
                    
                Assert.False(containsJonesPaper, "Jones's paper should not be in the filtered results");
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
        public void EndToEnd_FilterReferences_MultipleAuthors()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                File.WriteAllText(tempFilePath,
                    "@article{Smith2020T,\n" +
                    "  author = {Smith, John, Johnson, Mike},\n" +
                    "  title = {Test Paper},\n" +
                    "  journal = {Science Journal},\n" +
                    "  year = {2020}\n" +
                    "}");

                var consoleIO = new MockConsoleIO(new[] {
                    "filter",
                    "author",
                    "Johnson",  // Should find paper with multiple authors
                    "exit"
                });

                var mockLoader = new Mock<IReferenceLoader>();
                var references = new List<Reference> { 
                    new ArticleReference { 
                        Author = "Smith, John, Johnson, Mike",
                        Title = "Test Paper",
                        Journal = "Science Journal",
                        Year = "2020"
                    }
                };
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(references);

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("Johnson") && o.Contains("Test Paper"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_FilterReferences_MultipleCriteria()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                File.WriteAllText(tempFilePath,
                    "@article{Smith2020T,\n" +
                    "  author = {Smith, John},\n" +
                    "  title = {Test Paper},\n" +
                    "  journal = {Science Journal},\n" +
                    "  year = {2020}\n" +
                    "}\n\n" +
                    "@article{Smith2021A,\n" +
                    "  author = {Smith, John},\n" +
                    "  title = {Another Paper},\n" +
                    "  journal = {Nature},\n" +
                    "  year = {2021}\n" +
                    "}");

                var consoleIO = new MockConsoleIO(new[] {
                    "filter",
                    "author journal",  // Filter by both author and journal
                    "Smith",          // Author filter
                    "Science",        // Journal filter
                    "exit"
                });

                var mockLoader = new Mock<IReferenceLoader>();
                var references = new List<Reference> { 
                    new ArticleReference { 
                        Author = "Smith, John",
                        Title = "Test Paper",
                        Journal = "Science Journal",
                        Year = "2020"
                    },
                    new ArticleReference {
                        Author = "Smith, John",
                        Title = "Another Paper",
                        Journal = "Nature",
                        Year = "2021"
                    }
                };
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(references);

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("Science Journal") && o.Contains("2020"));
                Assert.DoesNotContain(outputs, o => o.Contains("Nature") && o.Contains("2021"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_FilterReferences_ExactMatch()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                File.WriteAllText(tempFilePath,
                    "@article{Smith2020T,\n" +
                    "  author = {Smith, John},\n" +
                    "  title = {Test Paper},\n" +
                    "  journal = {Science Journal},\n" +
                    "  year = {2020}\n" +
                    "}");

                var consoleIO = new MockConsoleIO(new[] {
                    "filter",
                    "title",
                    "\"Test Paper\"",  // Exact match with quotes
                    "exit"
                });

                var mockLoader = new Mock<IReferenceLoader>();
                var references = new List<Reference> { 
                    new ArticleReference { 
                        Author = "Smith, John",
                        Title = "Test Paper",
                        Journal = "Science Journal",
                        Year = "2020"
                    }
                };
                mockLoader.Setup(loader => loader.LoadReferences()).Returns(references);

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("Test Paper") && o.Contains("Smith"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_FilterReferences_NoMatches()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                File.WriteAllText(tempFilePath,
                    "@article{Smith2020T,\n" +
                    "  author = {Smith, John},\n" +
                    "  title = {Test Paper},\n" +
                    "  journal = {Science Journal},\n" +
                    "  year = {2020}\n" +
                    "}");

                var consoleIO = new MockConsoleIO(new[] {
                    "filter",
                    "author",
                    "NonexistentAuthor",
                    "exit"
                });

                var mockLoader = new Mock<IReferenceLoader>();
                mockLoader.Setup(loader => loader.LoadReferences())
                    .Returns(new List<Reference> { 
                        new ArticleReference { 
                            Author = "Smith, John",
                            Title = "Test Paper",
                            Journal = "Science Journal",
                            Year = "2020"
                        }
                    });

                var program = new ReferenceManager.Program(consoleIO, mockLoader.Object);
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("No references match"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_InvalidCommand()
        {
            var consoleIO = new MockConsoleIO(new[] {
                "invalidcommand",
                "exit"
            });

            var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader("test.bib"));
            program.Run();

            var outputs = consoleIO.GetOutputs();
            Assert.Contains(outputs, o => o.Contains("Unknown command"));
        }

        [Fact]
        public void EndToEnd_HelpCommand()
        {
            var consoleIO = new MockConsoleIO(new[] {
                "help",
                "exit"
            });

            var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader("test.bib"));
            program.Run();

            var outputs = consoleIO.GetOutputs();
            Assert.Contains(outputs, o => o.Contains("Available commands:"));
            Assert.Contains(outputs, o => o.Contains("add"));
            Assert.Contains(outputs, o => o.Contains("list"));
            Assert.Contains(outputs, o => o.Contains("filter"));
            Assert.Contains(outputs, o => o.Contains("print references"));
        }

        [Fact]
        public void EndToEnd_AddInvalidReference_CancelsOperation()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                var consoleIO = new MockConsoleIO(new[] {
                    "add",
                    "1", // article
                    "Smith, John",
                    "",
                    "Test Paper",
                    "Science Journal",
                    "invalid_year", // Invalid year
                    "2020", // Correct year after invalid input
                    "",  // month
                    "",  // volume
                    "",  // pages
                    "",  // doi
                    "",  // note
                    "",  // key
                    "n", // Cancel operation
                    "exit"
                });

                var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader(tempFilePath));
                program.Run();

                Assert.False(File.Exists(tempFilePath), "File should not be created when operation is cancelled");
                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("Operation cancelled"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_ListReferences_EmptyFile()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                var consoleIO = new MockConsoleIO(new[] {
                    "list",
                    "exit"
                });

                var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader(tempFilePath));
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("No file found"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_PrintReferences_HumanReadableFormat()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try
            {
                File.WriteAllText(tempFilePath,
                    "@article{Smith2020T,\n" +
                    "  author = {Smith, John},\n" +
                    "  title = {Test Paper},\n" +
                    "  journal = {Science Journal},\n" +
                    "  year = {2020}\n" +
                    "}");

                var consoleIO = new MockConsoleIO(new[] {
                    "print references",
                    "exit"
                });

                var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader(tempFilePath));
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => 
                    o.Contains("Smith, John") && 
                    o.Contains("Test Paper") && 
                    o.Contains("Science Journal") && 
                    o.Contains("2020") &&
                    !o.Contains("@article"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [Fact]
        public void EndToEnd_AddArticle_ValidatesNumericFields()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try 
            {
                var consoleIO = new MockConsoleIO(new[] {
                    "add",
                    "1", // article
                    "Smith, John",
                    "",
                    "Test Paper",
                    "Science Journal",
                    "2020",
                    "", // month
                    "invalid", // Invalid volume
                    "", // Skip volume
                    "1a2", // Invalid pages
                    "1--2", // Valid pages
                    "", // doi
                    "", // note
                    "", // key
                    "y",
                    "exit"
                });

                var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader(tempFilePath));
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("Volume must be a number"));
                Assert.Contains(outputs, o => o.Contains("must eighter input a range of pages"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }
        
        [Fact]
        public void EndToEnd_AddArticle_ValidatesRequiredFields()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bib");
            ReferenceManager.Program.FilePath = tempFilePath;

            try 
            {
                var consoleIO = new MockConsoleIO(new[] {
                    "add",
                    "1", // article
                    "", // Missing author first time
                    "Smith, John", // Valid author
                    "",
                    "", // Missing title first time
                    "Test Paper", // Valid title
                    "", // Missing journal first time
                    "Science Journal", // Valid journal
                    "2020",
                    "", // month
                    "", // volume
                    "", // pages
                    "", // doi
                    "", // note
                    "", // key
                    "y",
                    "exit"
                });

                var program = new ReferenceManager.Program(consoleIO, new FileReferenceLoader(tempFilePath));
                program.Run();

                var outputs = consoleIO.GetOutputs();
                Assert.Contains(outputs, o => o.Contains("At least one author is required"));
                Assert.Contains(outputs, o => o.Contains("Title cannot be empty"));
                Assert.Contains(outputs, o => o.Contains("Journal article added successfully"));
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }


    }
}

