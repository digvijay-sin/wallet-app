namespace Expense_Tracker.Service
{
    public static class TransactionURLService
    {
        private const string BASE_URL = "https://localhost:7247/";

        
        public const string GET_ALL_TRANSACTIONS = BASE_URL + "v1/TransactionAPI/GetAllTransactions";
        public const string GET_TRANSACTION = BASE_URL + "v1/TransactionAPI/GetTransaction/";
        public const string ADD_TRANSACTION = BASE_URL + "v1/TransactionAPI/AddTransaction";
        public const string UPDATE_TRANSACTION = BASE_URL + "v1/TransactionAPI/UpdateTransaction/";
        public const string DELETE_TRANSACTION = BASE_URL + "v1/TransactionAPI/DeleteTransaction/";
    }
}
