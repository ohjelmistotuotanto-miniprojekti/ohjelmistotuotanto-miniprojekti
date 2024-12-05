using ArticleReference = ReferenceManager.ArticleReference;
using InProceedingsReference = ReferenceManager.InProceedingsReference;
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
            Author = "Allan Collins and John Seely Brown and Ann Holum",
            Title = "Cognitive apprenticeship: making thinking visible",
            Journal = "American Educator",
            Year = "1991",
            Volume = "6",
            Pages = "38--46"
        };
        string result = testReference.ToBibtex();
        Assert.Equal(
            $"@article{{Allan1991C,\n" +
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
    public void TestInProceedingsToBibtex()
    {
        var testReference = new InProceedingsReference
        {
            Author = "Vihavainen, Arto and Paksula, Matti and Luukkainen, Matti",
            Title = "Extreme Apprenticeship Method in Teaching Programming for Beginners.",
            Year = "2011",
            BookTitle = "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education"
        };
        string result = testReference.ToBibtex();
        Assert.Equal(
            $"@inproceedings{{Vihavainen2011E,\n" +
            $"  author = {{Vihavainen, Arto and Paksula, Matti and Luukkainen, Matti}},\n" +
            $"  title = {{Extreme Apprenticeship Method in Teaching Programming for Beginners.}},\n" +
            $"  booktitle = {{SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education}},\n" +
            $"  year = {{2011}}\n" +
            $"}}",
            result);
    }

    [Fact]
    public void TestInProceedingsToBibtexFail()
    {
        var testReference = new InProceedingsReference
        {
            Author = "Vihavainen, Arto and Paksula, Matti and Luukkainen, Matti",
            Title = "Extreme Apprenticeship Method in Teaching Programming for Beginners.",
            Year = "",
            BookTitle = "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education"
        };
        string result = testReference.ToBibtex();
        Assert.Equal("", result);
    }


    [Fact]
    public void TestInProceedingsToBibtexFile()
    {
        string tempFilePath = Path.GetTempFileName();
        ReferenceManager.Program.FilePath = tempFilePath;

        try
        {
            var reference = new InProceedingsReference
            {
                Author = "Vihavainen, Arto",
                Title = "Extreme Apprenticeship Method in Teaching Programming for Beginners.",
                Year = "2011",
                BookTitle = "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education"
            };

            Assert.True(reference.ToBibtexFile(), "Failed to write reference to file.");

            string fileContent = File.ReadAllText(tempFilePath).Trim();
            string normalizedFileContent = fileContent.Replace("\r\n", "\n");

            string expectedBibtex =
                $"@inproceedings{{Vihavainen2011E,\n" +
                $"  author = {{Vihavainen, Arto}},\n" +
                $"  title = {{Extreme Apprenticeship Method in Teaching Programming for Beginners.}},\n" +
                $"  booktitle = {{SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education}},\n" +
                $"  year = {{2011}}\n" +
                $"}}";

            string normalizedExpectedBibtex = expectedBibtex.Replace("\r\n", "\n");

            Assert.Equal(normalizedExpectedBibtex, normalizedFileContent);
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
    public void TestKeyGeneration()
    {
        var testReference = new InProceedingsReference
        {
            Author = "Vihavainen, Arto and Paksula, Matti and Luukkainen, Matti",
            Title = "Extreme Apprenticeship Method in Teaching Programming for Beginners.",
            Year = "2011",
            BookTitle = "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education"
        };

        Assert.Equal("Vihavainen2011E", testReference.Key);
    }

    /*
    [Fact]
    public void TestToBibtexFile()
    {
        var testReference = new ArticleReference
        {
            Author = "Allan Collins and John Seely Brown and Ann Holum",
            Title = "Cognitive apprenticeship: making thinking visible",
            Journal = "American Educator",
            Year = "1991",
            Volume = "6",
            Pages = "38--46"
        };
        var result = testReference.ToBibtexFile();
        Assert.Equal(
            $"@article{{Allan1991C,\n" +
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
    }*/
}
