using ArticleReference = ReferenceManager.ArticleReference;
namespace Reference.Tests;
using Xunit;

public class UnitTest2
{
    [Fact]
    // For the CI/CD test
    public void Test1()
    {
        int result = 2 + 2;
        Assert.Equal(4, result);
    }

    [Fact]
    public void TestToBibtex()
    {
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
        string result = testReference.ToBibtex();
        Assert.Equal(
            $"@article{{CBH91,\n" +
            $"  author = {{Allan Collins and John Seely Brown and Ann Holum}},\n" +
            $"  title = {{Cognitive apprenticeship: making thinking visible}},\n" +
            $"  journal = {{American Educator}},\n" +
            $"  year = {{1991}},\n" +
            $"  volume = {{6}},\n" +
            $"  pages = {{38--46}}\n" +
            $"}}",
            result);
    }

    [Fact]
    public void TestToBibtexFail()
    {
        var testReference = new ArticleReference
        {
            Key = "CBH91",
            Author = "Allan Collins and John Seely Brown and Ann Holum",
            Title = "Cognitive apprenticeship: making thinking visible",
            Journal = "",
            Year = "1991",
            Volume = "",
            Pages = "38--46"
        };
        string result = testReference.ToBibtex();
        Assert.Equal("", result);
    }

    [Fact]
    public void TestToBibtexFile()
    {
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
        var result = testReference.ToBibtexFile();
        Assert.Equal(
            $"@article{{CBH91,\n" +
            $"  author = {{Allan Collins and John Seely Brown and Ann Holum}},\n" +
            $"  title = {{Cognitive apprenticeship: making thinking visible}},\n" +
            $"  journal = {{American Educator}},\n" +
            $"  year = {{1991}},\n" +
            $"  volume = {{6}},\n" +
            $"  pages = {{38--46}}\n" +
            $"}}\n\n",
            File.ReadAllText(ReferenceManager.Program.FilePath)
        );
        Assert.True(
            result
        );
        File.WriteAllText(ReferenceManager.Program.FilePath, string.Empty);
    }
}
