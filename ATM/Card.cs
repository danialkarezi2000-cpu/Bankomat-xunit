namespace ATM;


public class Card
{
    private int failedAttempts = 0;
    private const int MaxFailedAttempts = 3;
    public bool IsBlocked { get; private set; }

    public string CardNumber { get; }
    public string PinCode { get; }
    public Account Account { get; }

    public Card(string cardNumber, string pinCode, Account account)
    {
        CardNumber = cardNumber;
        PinCode = pinCode;
        Account = account;
    }
    public bool MatchesPin(string pinCode)
    {
        if (IsBlocked)
        {
            return false;   // spärrat kort: neka alltid
        }

        if (PinCode != pinCode)
        {
            failedAttempts++;          // ett fel försök till
            if (failedAttempts >= MaxFailedAttempts)
            {
                IsBlocked = true;      // tredje felet: spärra kortet
            }
            return false;
        }

        return true;
    }

}