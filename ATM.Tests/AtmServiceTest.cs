using ATM;
using Xunit;
namespace ATM.Tests;

public class AtmServiceTest
{
    private AtmService atm;
    private Account account;
    private Card card;

    public AtmServiceTest()
    {
        account = new Account(5000);
        card = new Card("1111-2222", "1234", account);
        atm = new AtmService(7000);
    }

    [Fact]
    public void InsertCard_AtmKnowsCardIsInserted()
    {
        atm.InsertCard(card);
        Assert.True(atm.HasCardInserted);
    }

    [Fact]
    public void EnterPin_Wrong_NotAuthenticated()
    {
        atm.InsertCard(card);
        bool result = atm.EnterPin("2345");
        Assert.False(result);
        Assert.False(atm.IsAuthenticated);
    }

    [Fact]
    public void EnterPin_Correct_Authenticated()
    {
        atm.InsertCard(card);
        bool result = atm.EnterPin("1234");
        Assert.True(result);
        Assert.True(atm.IsAuthenticated);
    }

    [Fact]
    public void EjectCard_CardIsNoLongerInserted()
    {
        atm.InsertCard(card);
        atm.EjectCard();
        Assert.False(atm.HasCardInserted);
    }
}
