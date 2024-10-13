using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Invoice;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IInvoiceService
    {
        Task<ApiResponse<InvoiceDto>> CreateInvoice(int orderId);
        Task<ApiResponse<InvoiceDto>> GetInvoiceById(int orderId);
        Task<ApiResponse<InvoiceDto>> GetInvoiceByOrderId(int orderId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<InvoiceDto>> CreateInvoice(int orderId)
        {
            var order = await _unitOfWork.OrderRepository
                .FindByConditionWithTrackingAsync(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (order.HasNoValue())
            {
                return new ApiResponse<InvoiceDto>(null, false, "Order not found.");
            }

            var existingInvoice = await _unitOfWork.OrderRepository
                .FindByConditionWithTrackingAsync(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (existingInvoice.HasNoValue())
            {
                return new ApiResponse<InvoiceDto>(null, false, "Invoice already exists for this order.");
            }

            var invoice = new Invoice
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount
            };

            await _unitOfWork.InvoiceRepository.CreateAsync(invoice);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<InvoiceDto>(null, false, "An error occurred while creating the invoice.");
            }

            var invoiceResponse = new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                TotalAmount = invoice.TotalAmount,
                InvoiceAt = invoice.InvoiceAt,
                OrderId = invoice.OrderId,
                OrderDto = new OrderDto
                {
                    OrderId = order.OrderId,
                    PaymentStatus = order.PaymentStatus
                }
            };

            return new ApiResponse<InvoiceDto>(invoiceResponse, true, "Invoice created successfully.");
        }

        public Task<ApiResponse<InvoiceDto>> GetInvoiceById(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<InvoiceDto>> GetInvoiceByOrderId(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
