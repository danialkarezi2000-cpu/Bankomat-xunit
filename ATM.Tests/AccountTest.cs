using ATM;

using Xunit;
namespace ATM.Tests;

public class AccountTest
{
    private Account account = new Account(5000);
    [Fact]
    public void GetBalance_ReturnsInitialBalance()
    {
        Assert.Equal(5000, account.GetBalance());
    }
    [Fact]
    public void Deposit_IncreasesBalance()
    {
        var result = account.Deposit(1000);
        Assert.True(result);
        Assert.Equal(6000, account.GetBalance());
    }
    [Fact]
    public void Withdraw_DecreasesBalance()
    {
        var result = account.Withdraw(1000);
        Assert.True(result);
        Assert.Equal(4000, account.GetBalance());
    }
}
