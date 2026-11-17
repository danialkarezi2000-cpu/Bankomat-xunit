namespace ATM;

public class AtmService
{
    private Card? _currentCard;
    private bool _isAuthenticated;

    public bool HasCardInserted => _currentCard != null;
    public bool IsAuthenticated => _isAuthenticated;
    
    public int AtmBalance { get; private set; }

    public AtmService(int initialBalance)
    {
        AtmBalance = initialBalance;
    }

    public void InsertCard(Card card)
    {
        _currentCard = card;
        _isAuthenticated = false;
    }

    public void EjectCard()
    {
        _currentCard = null;
        _isAuthenticated = false;
    }

    public bool EnterPin(string pinCode)
    {
        if (_currentCard == null)
        {
            return false;
        }

        _isAuthenticated = _currentCard.MatchesPin(pinCode);
        return _isAuthenticated;
    }

    public int GetBalance()
    {
        EnsureAuthenticated();
        return _currentCard!.Account.GetBalance();
    }

    public WithdrawResult Withdraw(int amount)
    {
        if (_currentCard == null)
            return WithdrawResult.NoCardInserted;

        if (!_isAuthenticated)
            return WithdrawResult.NotAuthenticated;

        if (amount > AtmBalance)
            return WithdrawResult.InsufficientAtmFunds;

        if (!_currentCard.Account.Withdraw(amount))
            return WithdrawResult.InsufficientAccountFunds;

        AtmBalance -= amount;
        return WithdrawResult.Success;
    }
    
    public bool Deposit(int amount)
    {
        EnsureAuthenticated();
        return _currentCard!.Account.Deposit(amount);
    }
    
    public void EnsureAuthenticated()
    {
        if (_currentCard == null || !_isAuthenticated)
        {
            throw new InvalidOperationException("Ingen autentiserad session.");
        }
    }
}