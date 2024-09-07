namespace MedicineShopApplication.BLL.Dtos
{
    public class UserDto : BaseEntityDto
    {
        public int UserDtoId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public CartDto CartDto { get; set; }
        public List<OrderDto> OrderDtos { get; set; }
        public List<PaymentDto> PaymentDtos { get; set; }
    }
}
