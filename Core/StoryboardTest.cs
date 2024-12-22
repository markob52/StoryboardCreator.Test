using StoryboardCreator.Core;

namespace StoryboardCreator.Test.Core;

/// <summary>
/// Tests for the Storyboard object
/// </summary>
[TestFixture]
[TestOf(typeof(Storyboard))]
public class StoryboardTest
{
    [SetUp][TearDown]
    public void RemoveTempDir()
    {
        try
        {
            Directory.Delete(Path.Combine(Path.GetTempPath(), "StoryboardCreator"),true);
        }
        catch (DirectoryNotFoundException)
        {
            
        }
    }

    /// <summary>
    /// Basic tests for the Storyboard object
    /// </summary>
    [Test]
    public void BasicTest()
    {
        var storyboard = new Storyboard()
        {
            Title = "Test Title",
            Author = "Test Author",
        };
        storyboard.AddShot(-1);
        storyboard.Shots[0].Title = "Test Shot Title";
        storyboard.Shots[0].Body = "Test Shot Body";
        storyboard.AddImageToShot(Path.Join(
            Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName, 
            "Assets", "sample1.jpg"),0);
        storyboard.AddShot(-1,new Shot()
        {
            Title = "Test Shot Title 2",
            Body = "Test Shot Body 2",
        },Path.Join(
            Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName, 
            "Assets", "sample2.jpg"));
        Assert.Multiple(() =>
        {
            Assert.That(storyboard.Title, Is.EqualTo("Test Title"));
            Assert.That(storyboard.Author, Is.EqualTo("Test Author"));
            Assert.That(storyboard.Shots[0].Title, Is.EqualTo("Test Shot Title"));
            Assert.That(storyboard.Shots[0].Body, Is.EqualTo("Test Shot Body"));
            Assert.That(storyboard.Shots[0].ImageFileName, Is.EqualTo("0.jpg"));
            Assert.That(storyboard.Shots[1].Title, Is.EqualTo("Test Shot Title 2"));
            Assert.That(storyboard.Shots[1].Body, Is.EqualTo("Test Shot Body 2"));
            Assert.That(storyboard.Shots[1].ImageFileName, Is.EqualTo("1.jpg"));
            Assert.That(Directory.Exists(storyboard.CachePath), Is.True);
            Assert.That(storyboard.CachePath,Is.EqualTo(Path.Combine(Path.GetTempPath(), "StoryboardCreator","0")));
            Assert.That(Directory.Exists(Path.Join(storyboard.CachePath,"Images")),Is.True);
            storyboard.Close();
            Assert.That(Directory.Exists(Path.Combine(Path.GetTempPath(), "StoryboardCreator")),Is.False);
        });
        
    }

    /// <summary>
    /// Testing the loading and saving of the object
    /// </summary>
    [Test]
    public void LoadSaveTest()
    {
        var storyboard = new Storyboard()
        {
            Title = "Test Title",
            Author = "Test Author",
        };
        storyboard.AddShot(-1);
        storyboard.Shots[0].Title = "Test Shot Title";
        storyboard.Shots[0].Body = "Test Shot Body";
        storyboard.AddImageToShot(Path.Join(
            Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName,
            "Assets", "sample1.jpg"), 0);
        storyboard.AddShot(-1, new Shot()
        {
            Title = "Test Shot Title 2",
            Body = "Test Shot Body 2",
        }, Path.Join(
            Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName,
            "Assets", "sample2.jpg"));
        storyboard.Save(Path.Join(Directory.GetCurrentDirectory(), "test.sb.zip"));
        storyboard.Close();
        var loadedStoryboard = Storyboard.Load(Path.Join(Directory.GetCurrentDirectory(), "test.sb.zip"));
        Assert.Multiple(() =>
        {
            Assert.That(loadedStoryboard.Title, Is.EqualTo("Test Title"));
            Assert.That(loadedStoryboard.Author, Is.EqualTo("Test Author"));
            Assert.That(loadedStoryboard.Shots[0].Title, Is.EqualTo("Test Shot Title"));
            Assert.That(loadedStoryboard.Shots[0].Body, Is.EqualTo("Test Shot Body"));
            Assert.That(loadedStoryboard.Shots[0].ImageFileName, Is.EqualTo("0.jpg"));
            Assert.That(loadedStoryboard.Shots[1].Title, Is.EqualTo("Test Shot Title 2"));
            Assert.That(loadedStoryboard.Shots[1].Body, Is.EqualTo("Test Shot Body 2"));
            Assert.That(loadedStoryboard.Shots[1].ImageFileName, Is.EqualTo("1.jpg"));
            Assert.That(Directory.Exists(loadedStoryboard.CachePath), Is.True);
            Assert.That(Directory.Exists(Path.Join(loadedStoryboard.CachePath, "Images")), Is.True);
            loadedStoryboard.Close();
            Assert.That(Directory.Exists(Path.Combine(Path.GetTempPath(), "StoryboardCreator")), Is.False);
        });
    }
}