using MessagePack;
using StoryboardCreator.Core;
#pragma warning disable CS0618 // Type or member is obsolete

namespace StoryboardCreator.Test.Core;

/// <summary>
/// Tests for the Shot object
/// </summary>
[TestFixture]
[TestOf(typeof(Shot))]
public class ShotTest
{
    /// <summary>
    /// Testing the serialization of the Shot object
    /// </summary>
    [Test]
    public void MsgPackSerializationTest()
    {
        var shot = new Shot()
        {
            Title = "Test Title",
            Body = "Test Body",
        };
        shot.SetImageFileName("TestImage.jpg");
        var serializedData = MessagePackSerializer.Serialize(shot);
        var deserializedShot = MessagePackSerializer.Deserialize<Shot>(serializedData);
        Assert.Multiple(() =>
        {
            Assert.That(deserializedShot.Title, Is.EqualTo(shot.Title));
            Assert.That(deserializedShot.Body, Is.EqualTo(shot.Body));
            Assert.That(deserializedShot.ImageFileName, Is.EqualTo(shot.ImageFileName));
        });
    }

}