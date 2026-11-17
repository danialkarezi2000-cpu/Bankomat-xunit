using ATM;

using Xunit;
namespace ATM.Tests;

public class CardTest
{
    private Card card = new Card("1111-2222", "1234", new Account(5000));

    [Theory]
    [InlineData("1234", true)]
    [InlineData("2345", false)]
    public void MatchesPin_ReturnsExpectedResult(string pin, bool expected)
    {
        Assert.Equal(expected, card.MatchesPin(pin));
    }
}
