namespace MedicineShopApplication.BLL.Utils
{
    public static class GeneralUtils
    {
        public static bool HasValue(this object item)
        {
            if (item is string s)
                return !string.IsNullOrEmpty(s) && s != "null";
            if (item is DateTime d)
                return d != default(DateTime);
            return item != null;
        }

        public static bool HasNoValue(this object item)
        {
            if (item is string s)
                return string.IsNullOrEmpty(s) || s == "null";
            if (item is DateTime d)
                return d == default(DateTime);
            return item == null;
        }
    }
}
