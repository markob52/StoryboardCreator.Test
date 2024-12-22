// Storyboard Creator - A cross-platform app made for making storyboards.
// Copyright (C) 2024 Marko Bošnjak
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License version 3 as published by
// the Free Software Foundation.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using StoryboardCreator.Core;

namespace StoryboardCreator.Test.Core;

/// <summary>
/// Tests for the ImageCacheController object
/// </summary>
[TestFixture]
[TestOf(typeof(ImageCacheController))]
public class ImageCacheControllerTest
{
    [SetUp]
    public void DeleteTestDirs()
    {
        try
        {
            Directory.Delete(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest"),true);
        }
        catch (DirectoryNotFoundException)
        {
            
        }
    }
    /// <summary>
    /// Test creation of the new ImageCache directory
    /// </summary>
    [Test]
    [Order(0)]
    public void NewDirectoryTest()
    {
        var controller = new ImageCacheController(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest"));
        Assert.Multiple(() =>
        {
            Assert.That(controller.CachePath, Is.EqualTo(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest")));
            Assert.That(Directory.Exists(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest")), Is.True);
        });
    }
    
    /// <summary>
    /// Test adding images
    /// </summary>
    [Test]
    [Order(1)]
    public void AddImageTest()
    {
        var controller = new ImageCacheController(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest"));
        var imagepaths = Directory.GetFiles(Path.Join(
            Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName,
            "Assets"));
        Assert.Multiple(() =>
        {
            for (var i = 0; i < imagepaths.Length; i++)
            {
                var filename = controller.AddImage(imagepaths[i]);
                Assert.Multiple(() =>
                {
                    Assert.That(filename, Is.EqualTo($"{i}.jpg"));
                    Assert.That(File.Exists(Path.Join(controller.CachePath, filename)), Is.True);
                    Assert.That(File.ReadAllBytes(imagepaths[i]),
                        Is.EqualTo(File.ReadAllBytes(Path.Join(controller.CachePath, filename))));
                });
            }
            Assert.That(controller.LastId, Is.EqualTo(imagepaths.Length-1));
        });
    }
    
    /// <summary>
    /// Test opening an existing ImageCache
    /// </summary>
    [Test]
    [Order(2)]
    public void ExistingDirectoryTest()
    {
        var controller1 = new ImageCacheController(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest"));
        var imagePaths = Directory.GetFiles(Path.Join(
        Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName,
        "Assets"));
        foreach (var t in imagePaths)
        {
            var filename = controller1.AddImage(t);
        }

        var controller2 = new ImageCacheController(Path.Join(Directory.GetCurrentDirectory(), "ImageCacheTest"));
        Assert.Multiple(() =>
        {
            Assert.That(controller2.LastId,Is.EqualTo(controller1.LastId));
            Assert.That(controller2.CachePath,Is.EqualTo(controller1.CachePath));
        });
    }
}