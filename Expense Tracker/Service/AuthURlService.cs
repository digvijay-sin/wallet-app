namespace Expense_Tracker.Service
{
    public static class AuthURlService
    {
        private const string BASE_URL = "https://localhost:7247/";

        
        public const string LOGIN = BASE_URL + "v1/AuthAPI/Login";
        public const string SIGNUP = BASE_URL + "v1/AuthAPI/Signup";
     
    }
}
