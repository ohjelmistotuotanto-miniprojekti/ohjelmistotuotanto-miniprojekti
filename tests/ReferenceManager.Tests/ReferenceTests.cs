using ArticleReference = ReferenceManager.ArticleReference;
using InProceedingsReference = ReferenceManager.InProceedingsReference;
namespace Reference.Tests;

using System.Net;
using System.Reflection;

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
    public void TestJournalArticle_Properties_Are_Set_Correctly()
    {
        var articleReference = new ArticleReference
        {
            Author = "Collins, Allan and Brown, John Seely and Holumm Ann",
            Title = "Cognitive apprenticeship: making thinking visible",
            Journal = "American Educator",
            Year = "1991",
            Volume = "6",
            Pages = "38--46"
        };

        Assert.Equal("Collins, Allan and Brown, John Seely and Holumm Ann", articleReference.Author);
        Assert.Equal("Cognitive apprenticeship: making thinking visible", articleReference.Title);
        Assert.Equal("American Educator", articleReference.Journal);
        Assert.Equal("1991", articleReference.Year);
        Assert.Equal("6", articleReference.Volume);
        Assert.Equal("38--46", articleReference.Pages);

        Assert.Equal("", articleReference.Month);
        Assert.Equal("", articleReference.Note);
        Assert.Equal("", articleReference.Doi);
    }

    [Fact]
    public void TestInproceedings_Properties_Are_Set_Correctly()
    {
        var articleReference = new InProceedingsReference
        {
            Author = "Vihavainen, Arto",
            Title = "Extreme Apprenticeship Method in Teaching Programming for Beginners.",
            BookTitle = "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education",
            Year = "2011",
        };

        Assert.Equal("Vihavainen, Arto", articleReference.Author);
        Assert.Equal("Extreme Apprenticeship Method in Teaching Programming for Beginners.", articleReference.Title);
        Assert.Equal("SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education", articleReference.BookTitle);
        Assert.Equal("2011", articleReference.Year);
        Assert.Equal("", articleReference.Editor);
        Assert.Equal("", articleReference.Number);
        Assert.Equal("", articleReference.Series);
        Assert.Equal("", articleReference.Pages);
        Assert.Equal("", articleReference.Address);
        Assert.Equal("", articleReference.Organization);
        Assert.Equal("", articleReference.Publisher);
        Assert.Equal("", articleReference.Note);
    }

    // ===================== TestYear =====================
    [Fact]
    public void TestYear_ValidYear_SetsSuccessfully()
    {
        var reference = new ArticleReference();
        string validYear = "2023";

        reference.Year = validYear;

        Assert.Equal(validYear, reference.Year);
    }

    [Fact]
    public void TestYear_InvalidYear_ThrowsArgumentException()
    {
        var reference = new ArticleReference();
        string invalidYear = "10000"; // Year outside the valid range

        var exception = Assert.Throws<ArgumentException>(() => reference.Year = invalidYear);
    }

    [Fact]
    public void TestYear_NegativeYear_ThrowsArgumentException()
    {
        var reference = new ArticleReference();
        string negativeYear = "-100"; // Negative year is invalid

        var exception = Assert.Throws<ArgumentException>(() => reference.Year = negativeYear);
    }

    [Fact]
    public void TestYear_NonNumericYear_ThrowsArgumentException()
    {
        var reference = new ArticleReference();
        string nonNumericYear = "Year2023"; // Non-numeric year is invalid

        var exception = Assert.Throws<ArgumentException>(() => reference.Year = nonNumericYear);
    }

    [Fact]
    public void TestYear_EmptyString_ThrowsArgumentException()
    {
        var reference = new ArticleReference();

        var exception = Assert.Throws<ArgumentException>(() => reference.Year = "");
        Assert.Equal("Invalid year", exception.Message);
    }

    [Fact]
    public void TestYear_ValidBoundaryYear_One_SetsSuccessfully()
    {
        var reference = new ArticleReference();
        string validBoundaryYear = "1";

        reference.Year = validBoundaryYear;
        Assert.Equal(validBoundaryYear, reference.Year);
    }

    [Fact]
    public void TestYear_ValidBoundaryYear_Max_SetsSuccessfully()
    {
        var reference = new ArticleReference();
        string validBoundaryYear = "9999";

        reference.Year = validBoundaryYear;
        Assert.Equal(validBoundaryYear, reference.Year);
    }
    // ===================== TestYear ends =====================

    // ===================== TestVolume =====================
    [Fact]
    public void TestVolume_NonNumericVolume_ThrowsArgumentException()
    {
        var reference = new ArticleReference();
        var inProceedingsReference = new InProceedingsReference();
        string invalidVolume = "abc1";

        var exception = Assert.Throws<ArgumentException>(() => reference.Volume = invalidVolume);
        var exception2 = Assert.Throws<ArgumentException>(() => inProceedingsReference.Volume = invalidVolume);
    }

    [Fact]
    public void TestYear_ValidVolume_SetsSuccessfully()
    {
        var reference = new ArticleReference();
        var inProceedingsReference = new InProceedingsReference();
        string validVolume = "1";


        reference.Volume = validVolume;
        Assert.Equal(validVolume, reference.Volume);

        inProceedingsReference.Volume = validVolume;
        Assert.Equal(validVolume, reference.Volume);
    }

    // ===================== TestVolume ends =====================


    // ===================== Test IsInt =====================

    [Fact]
    public void IsInt_ValidInteger_ReturnsTrue()
    {
        var testClass = new ArticleReference();
        string validInt = "123";

        bool result = testClass.isInt(validInt);
        Assert.True(result);
    }

    [Fact]
    public void IsInt_InvalidInteger_ReturnsFalse()
    {
        var testClass = new ArticleReference();
        string invalidInt = "abc";

        bool result = testClass.isInt(invalidInt);
        Assert.False(result);
    }

    [Fact]
    public void IsInt_EmptyString_ReturnsFalse()
    {
        var testClass = new ArticleReference();
        string emptyString = "";

        bool result = testClass.isInt(emptyString);
        Assert.False(result);
    }

    [Fact]
    public void IsInt_NegativeInteger_ReturnsTrue()
    {
        var testClass = new ArticleReference();
        string negativeInt = "-123";

        bool result = testClass.isInt(negativeInt);
        Assert.True(result);
    }

    [Fact]
    public void IsInt_Zero_ReturnsTrue()
    {
        var testClass = new ArticleReference();
        string zero = "0";

        bool result = testClass.isInt(zero);
        Assert.True(result);
    }

    [Fact]
    public void IsInt_Float_ReturnsFalse()
    {
        var testClass = new ArticleReference();
        string floatString = "123.45";

        bool result = testClass.isInt(floatString);
        Assert.False(result);
    }

    [Fact]
    public void IsInt_Whitespace_ReturnsFalse()
    {
        var testClass = new ArticleReference();
        string whitespace = "   ";

        bool result = testClass.isInt(whitespace);
        Assert.False(result);
    }
    // ===================== Test IsInt ends =====================

    // ===================== Test Pages ==========================

    [Fact]
    public void TestPages_EmptyInputSetsValue()
    {
        // Arrange
        var inProceedings = new InProceedingsReference();

        // Act
        inProceedings.Pages = "";

        // Assert
        Assert.Equal("", inProceedings.Pages);
    }

    [Fact]
    public void TestPages_ValidSinglePageSetsValue()
    {
        // Arrange
        var reference = new InProceedingsReference();

        // Act
        reference.Pages = "5";

        // Assert
        Assert.Equal("5", reference.Pages);
    }

    [Fact]
    public void TestPages_ValidPageRangeSetsValue()
    {
        // Arrange
        var reference = new InProceedingsReference();

        // Act
        reference.Pages = "5--8";

        // Assert
        Assert.Equal("5--8", reference.Pages);
    }

    [Fact]
    public void TestPages_InvalidCharactersThrowsException()
    {
        // Arrange
        var reference = new InProceedingsReference();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => reference.Pages = "5a");
        Assert.Equal("You must eighter input a range of pages or a single page. Range must be separated by '--'. If there is no pages to input, leave this empty.", exception.Message);
    }

    [Fact]
    public void TestPages_InvalidRangeSeparatorThrowsException()
    {
        // Arrange
        var reference = new InProceedingsReference();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => reference.Pages = "5-10");
        Assert.Equal("You must eighter input a range of pages or a single page. Range must be separated by '--'. If there is no pages to input, leave this empty.", exception.Message);
    }

    [Fact]
    public void TestBookTitle_ÉmptyInputThrowsArgumentException()
    {
        // Arrange
        var reference = new InProceedingsReference();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => reference.BookTitle = "");
        Assert.Equal("You must input a book title.", exception.Message);
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
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            new ArticleReference
            {
                Author = "Allan Collins and John Seely Brown and Ann Holum",
                Title = "Cognitive apprenticeship: making thinking visible",
                Journal = "",
                Year = "1991",
                Volume = "",
                Pages = "38--46"
            };
        }
        );
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
        var exception = Assert.Throws<ArgumentException>(() =>
        {
            new InProceedingsReference
            {
                Author = "Vihavainen, Arto and Paksula, Matti and Luukkainen, Matti",
                Title = "Extreme Apprenticeship Method in Teaching Programming for Beginners.",
                Year = "",
                BookTitle = "SIGCSE '11: Proceedings of the 42nd SIGCSE technical symposium on Computer science education"
            };
        }
        );
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

    [Fact]
    public void TestToBibtexFile()
    {
        // Arrange
        var article = new ArticleReference
        {
            Author = "John Doe",
            Title = "Sample Title",
            Journal = "Tech Journal",
            Year = "2024",
            Pages = "38--46"
        };

        // Expected BibTeX output
        string expectedBibtex = "@article{John2024S,\n" +
                                "  author = {John Doe},\n" +
                                "  title = {Sample Title},\n" +
                                "  journal = {Tech Journal},\n" +
                                "  year = {2024},\n" +
                                "  pages = {38--46}\n" +
                                "}\n";

        // Normalize expected and actual output
        string normalizedExpected = expectedBibtex.Replace("\r\n", "\n").Trim();
        string normalizedActual = article.ToBibtex().Replace("\r\n", "\n").Trim();

        // Assert
        Assert.Equal(normalizedExpected, normalizedActual);
    }


}
