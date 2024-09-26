namespace Expense_Tracker.Service
{
    public static class CategoryURLService
    {
        private const string BASE_URL = "https://localhost:7247/";

        
        public const string GET_ALL_CATEGORIES = BASE_URL + "v1/CategoryAPI/GetAllCateogries";
        public const string GET_CATEGORY = BASE_URL + "v1/CategoryAPI/GetCategory/";
        public const string ADD_CATEGORY = BASE_URL + "v1/CategoryAPI/AddCategory";
        public const string UPDATE_CATEGORY = BASE_URL + "v1/CategoryAPI/UpdateCategory/";
        public const string DELETE_CATEGORY = BASE_URL + "v1/CategoryAPI/Delete/";
    }
}
