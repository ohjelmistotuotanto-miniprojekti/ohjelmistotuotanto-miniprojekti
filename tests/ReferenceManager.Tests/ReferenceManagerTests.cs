namespace ReferenceManager.Tests;
using Xunit;

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
    public void TestListReferences()
    {
        List<Reference> references = new List<Reference>();

            var testReference = new ArticleReference
            {
                Key = "CBH91",
                Author = "Allan Collins and John Seely Brown and Ann Holum",
                Title = "Cognitive apprenticeship: making thinking visible",
                Journal = "American Educator",
                Year = "1991",
                Volume = "6",
                Pages = "38--46"
            };
            references.Add(testReference);

            var sw = new StringWriter();
            Console.SetOut(sw);
            var program = new ReferenceManager.Program(new ReferenceManager.ConsoleIO());
            program.ListReferences(references);
            Assert.Equal(
                @$"@article{{CBH91, author = {{Allan Collins and John Seely Brown and Ann Holum}},
                title = {{Cognitive apprenticeship: making thinking visible}},
                journal = {{American Educator}},
                year = {{1991}},
                volume = {{6}},
                pages = {{38--46}},
                }}",
                sw.ToString());
                
            File.WriteAllText(ReferenceManager.Program.FilePath, string.Empty);
    }
}
