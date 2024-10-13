using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MedicineShopApplication.BLL.Utils
{
    public static class GeneralUtils
    {
        /// <summary>
        /// Determines whether the specified object has a value.
        /// For strings, checks if it is neither null nor empty nor equal to "null".
        /// For DateTime, checks if it is not the default value.
        /// For other objects, checks if it is not null.
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns>True if the object has a value; otherwise, false.</returns>
        public static bool HasValue(this object item)
        {
            if (item is string s)
                return !string.IsNullOrEmpty(s) && s != "null";
            if (item is DateTime d)
                return d != default(DateTime);
            return item != null;
        }

        /// <summary>
        /// Determines whether the specified object has no value.
        /// For strings, checks if it is null, empty, or equal to "null".
        /// For DateTime, checks if it is the default value.
        /// For other objects, checks if it is null.
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns>True if the object has no value; otherwise, false.</returns>
        public static bool HasNoValue(this object item)
        {
            if (item is string s)
                return string.IsNullOrEmpty(s) || s == "null";
            if (item is DateTime d)
                return d == default(DateTime);
            return item == null;
        }

        /// <summary>
        /// Checks whether all public instance properties of the specified object are valid (i.e., non-null for reference types and not default or empty for strings).
        /// </summary>
        /// <typeparam name="T">The type of the object to check. Must be a reference type.</typeparam>
        /// <param name="obj">The object whose properties are being validated.</param>
        /// <returns>
        /// Returns <c>true</c> if all public instance properties of the object are valid (i.e., non-null for reference types, not default or empty for strings, and not null for nullable value types).
        /// Returns <c>false</c> if any property is null, an empty string, or an invalid value.
        /// </returns>
        /// <remarks>
        /// This method uses reflection to inspect each public instance property of the object. It verifies that reference types are not null, 
        /// strings are not empty, and nullable value types are not null.
        /// </remarks>

        public static bool HasValidProperties<T>(this T obj) where T : class
        {
            if (obj == null) return false;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);

                if (value == null)
                {
                    return false;
                }

                if (property.PropertyType == typeof(string) && string.IsNullOrEmpty((string)value))
                {
                    return false;
                }

                if (Nullable.GetUnderlyingType(property.PropertyType) != null && value == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Normalizes the specified name by converting it to uppercase, removing unnecessary trailing periods,
        /// and handling parentheses and special characters appropriately.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>A normalized version of the name with consistent casing and appropriate handling of special characters.</returns>
        public static string NormalizeName(string name)
        {
            name = name.ToUpper();
            name = name.Trim();

            name = Regex.Replace(name, @"\s+", " ");
            name = Regex.Replace(name, @"\(\s*(.*?)\s*\)\s*\.", @"($1)");

            name = name.TrimEnd('.');

            return name;
        }

        /// <summary>
        /// Generates a random alphanumeric string of the specified length using a cryptographically secure random number generator.
        /// </summary>
        /// <param name="length">The length of the random string to be generated.</param>
        /// <returns>A string containing a random combination of uppercase letters and digits.</returns>
        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomString = new char[length];
            var rng = RandomNumberGenerator.Create();

            var byteArray = new byte[length];
            rng.GetBytes(byteArray);

            for (int i = 0; i < length; i++)
            {
                randomString[i] = chars[byteArray[i] % chars.Length];
            }

            return new string(randomString);
        }

    }
}
