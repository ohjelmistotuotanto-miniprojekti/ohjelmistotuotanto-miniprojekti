using Xunit;
using Moq;
using System.Collections.Generic;
using ReferenceManager;

namespace ReferenceManager.Tests
{
    public class UnitTest1
    {
        [Fact]
        // For the CI/CD test
        public void Test1()
        {
            int result = 2 + 2;
            Assert.Equal(4, result);
        }

        [Fact]
        public void Test_AddJournalArticleConfirms()
        {
            // Arrange
            var mockIO = new Mock<ConsoleIO>();
            var references = new List<Reference>();

            // Stimulated user input
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
            Assert.Single(references); // Cheking list has one reference
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
        public void Test_UserCancels()
        {
            {
                // Arrange
                var mockIO = new Mock<ConsoleIO>();
                var references = new List<Reference>();

                //stimulated user input
                mockIO.SetupSequence(io => io.Read())
                    .Returns("John Doe")       // Author
                    .Returns("Sample Title")   // Title
                    .Returns("Tech Journal")   // Journal
                    .Returns("2024")           // Year
                    .Returns("12")             // Volume
                    .Returns("34-56")          // Pages
                    .Returns("n");             // Confirmation ('n' = user calsens adding)

                mockIO.Setup(io => io.Write(It.IsAny<string>()));

                // Using programm class
                var program = new Program(mockIO.Object);

                // Act
                program.AddJournalArticle(references);  

                // Assert
                Assert.Empty(references);  

                // Checking correct return message
                mockIO.Verify(io => io.Write("Operation cancelled by the user."), Times.Once);
            }
        }
    }
}
