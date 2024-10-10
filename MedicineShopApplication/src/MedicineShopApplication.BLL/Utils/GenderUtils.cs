using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Utils
{
    public static class GenderUtils
    {
        public static string GetGender(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Male",
                Gender.Female => "Female",
                Gender.Unknown => "Unknown",
                _ => gender.ToString()
            };
        }
    }
}
