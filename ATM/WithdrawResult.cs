namespace ATM;

public enum WithdrawResult
{
    Success,
    NoCardInserted,
    NotAuthenticated,
    InsufficientAtmFunds,
    InsufficientAccountFunds
}
