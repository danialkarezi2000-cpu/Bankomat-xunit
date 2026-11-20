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

    [Fact]
    public void Withdraw_5000_DeductedFromAccountAndAtm()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        var result = atm.Withdraw(5000);
        Assert.Equal(WithdrawResult.Success, result);
        Assert.Equal(0, account.GetBalance());
        Assert.Equal(2000, atm.AtmBalance);
    }

    [Fact]
    public void Withdraw_6000_AccountLacksFunds_NothingDeducted()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        var result = atm.Withdraw(6000);
        Assert.Equal(WithdrawResult.InsufficientAccountFunds, result);
        Assert.Equal(5000, account.GetBalance());
        Assert.Equal(7000, atm.AtmBalance);
    }

    [Fact]
    public void Withdraw_8000_AtmLacksCash_NothingDeducted()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        atm.Deposit(3000);   // nu har kontot 8000
        var result = atm.Withdraw(8000);
        Assert.Equal(WithdrawResult.InsufficientAtmFunds, result);
        Assert.Equal(8000, account.GetBalance());
        Assert.Equal(7000, atm.AtmBalance);
    }
    [Fact]
    public void Withdraw_NoCardInserted_ReturnsNoCardInserted()
    {
        var result = atm.Withdraw(1000);
        Assert.Equal(WithdrawResult.NoCardInserted, result);
        Assert.Equal(7000, atm.AtmBalance);
    }

    [Fact]
    public void Withdraw_WrongPin_ReturnsNotAuthenticated()
    {
        atm.InsertCard(card);
        atm.EnterPin("2345");
        var result = atm.Withdraw(1000);
        Assert.Equal(WithdrawResult.NotAuthenticated, result);
        Assert.Equal(5000, account.GetBalance());
    }

    [Fact]
    public void GetBalance_NotAuthenticated_Throws()
    {
        atm.InsertCard(card);
        Assert.Throws<InvalidOperationException>(() => atm.GetBalance());
    }

    [Fact]
    public void Deposit_NotAuthenticated_Throws()
    {
        atm.InsertCard(card);
        Assert.Throws<InvalidOperationException>(() => atm.Deposit(1000));
        card = new Card("1111-2222", "1234", new Account(5000));

    }

    [Fact]
    public void GetBalance_Authenticated_ReturnsBalance()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        var balance = atm.GetBalance();
        Assert.Equal(5000, balance);
    }
    [Fact]
    public void EnterPin_WrongThreeTimes_CardIsBlocked()
    {
        atm.InsertCard(card);
        for (int i = 0; i < 3; i++)
        {
            atm.EnterPin("0000");
        }
        Assert.True(card.IsBlocked);
    }
    [Fact]
    public void EnterPin_CorrectAfterBlocked_NotAuthenticated()
    {
        atm.InsertCard(card);
        for (int i = 0; i < 3; i++)
        {
            atm.EnterPin("0000");
        }
        bool result = atm.EnterPin("1234");
        Assert.False(result);
        Assert.False(atm.IsAuthenticated);
    }
    [Fact]
    public void EjectCard_AfterLogin_NotAuthenticated()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        atm.EjectCard();
        Assert.False(atm.IsAuthenticated);
    }
    [Fact]
    public void Withdraw_WithoutCard_BalanceUnchanged()
    {
        var result = atm.Withdraw(1000);
        Assert.Equal(WithdrawResult.NoCardInserted, result);
        Assert.Equal(7000, atm.AtmBalance);
        Assert.Equal(5000, card.Account.GetBalance());
    }
    [Fact]
    public void Withdraw_CardInsertedWrongPin_BalanceUnchanged()
    {
        atm.InsertCard(card);
        atm.EnterPin("0000");
        atm.Withdraw(1000);
        Assert.Equal(5000, account.GetBalance());
    }
    [Fact]
    public void Withdraw_OverLimit_IsDenied()
    {
        account.Deposit(2000);
        atm.InsertCard(card);
        atm.EnterPin("1234");
        var result = atm.Withdraw(6000);
        Assert.Equal(WithdrawResult.OverLimit, result);
        Assert.Equal(7000, account.GetBalance());
    }
        [Fact]
    public void Cancel_EjectsCard_WithdrawIsDenied()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        atm.Cancel();
        var result = atm.Withdraw(500);
        Assert.Equal(WithdrawResult.NoCardInserted, result);
    }
    [Fact]
    public void GetBalance_withoutCard_Throws()
    {
        Assert.Throws<InvalidOperationException>
        (() => atm.GetBalance());
    }
    [Fact]
    public void Deposit_withoutCard_Throws()
    {
        Assert.Throws<InvalidOperationException>
        (() => atm.Deposit(500));
    }
    [Fact]
    public void Deposit_withdraw_150_ReturnsInvalidAmount()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        var result = atm.Withdraw(150);
        Assert.Equal(WithdrawResult.InvalidAmount, result);
    }
    [Fact]
    public void Rescept_AfterWithdraw500_ShowsAmountAndBalance()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        atm.Withdraw(500);
        var receipt = atm.GetReceipt();
        Assert.Contains("Uttag: 500 kr", receipt);
        Assert.Contains("Saldo: 4500 kr", receipt);
    }
        [Fact]
    public void EnterPin_WrongThreeTimes_CardIsRetained()
    {
        atm.InsertCard(card);
        atm.EnterPin("2345");
        atm.EnterPin("2345");
        atm.EnterPin("2345");

        Assert.False(atm.HasCardInserted);
        Assert.False(atm.EnterPin("1234"));
    }
        [Fact]
    public void EnterPin_NewCard_ResetsFailedAttempts()
    {
        atm.InsertCard(card);
        atm.EnterPin("2345");
        atm.EnterPin("2345");
        atm.EjectCard();

        atm.InsertCard(card);
        atm.EnterPin("2345");

        Assert.True(atm.HasCardInserted);
    }
        [Theory]
    [InlineData(0)]
    [InlineData(-500)]
    public void Withdraw_ZeroOrNegative_ReturnsInvalidAmount(int amount)
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        var result = atm.Withdraw(amount);
        Assert.Equal(WithdrawResult.InvalidAmount, result);
    }
        [Fact]
    public void Receipt_ShowsTodaysDate()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        atm.Withdraw(500);
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.Contains(today, atm.GetReceipt());
    }
    [Fact]
    public void Deposit_ValidAmount_IncreasesBalance()
    {
        atm.InsertCard(card);
        atm.EnterPin("1234");
        bool success = atm.Deposit(1000);
        Assert.True(success);
        Assert.Equal(6000, account.GetBalance());
    }
}
