using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Invoice;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.Payment;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IInvoiceService
    {
        Task<ApiResponse<InvoiceDto>> CreateInvoice(int requestMaker, int orderId);
        Task<ApiResponse<InvoiceDto>> GetInvoiceById(int invoiceId);
        Task<ApiResponse<InvoiceDto>> GetInvoiceByOrderId(int orderId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<InvoiceDto>> CreateInvoice(int requestMaker, int orderId)
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
                TotalAmount = order.TotalAmount,
                InvoiceNumber = GenerateInvoiceNumber(order.OrderId),

                CreatedBy = requestMaker
            };

            await _unitOfWork.InvoiceRepository.CreateAsync(invoice);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<InvoiceDto>(null, false, "An error occurred while creating the invoice.");
            }

            var invoiceResponse = new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
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

        public async Task<ApiResponse<InvoiceDto>> GetInvoiceById(int invoiceId)
        {
            var invoice = await _unitOfWork.InvoiceRepository
                .FindByConditionAsync(x => x.InvoiceId == invoiceId)
                .Include(x => x.Order)
                .FirstOrDefaultAsync();

            if (invoice.HasNoValue())
            {
                return new ApiResponse<InvoiceDto>(null, false, "Invoice not found.");
            }

            var invoiceResponse = new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                TotalAmount = invoice.TotalAmount,
                InvoiceAt = invoice.InvoiceAt,
                OrderId = invoice.OrderId,
                OrderDto = new OrderDto
                {
                    OrderId = invoice.Order.OrderId,
                    PaymentStatus = invoice.Order.PaymentStatus
                }
            };

            return new ApiResponse<InvoiceDto>(invoiceResponse, true, "Invoice retrieved successfully.");
        }

        public async Task<ApiResponse<InvoiceDto>> GetInvoiceByOrderId(int orderId)
        {
            var invoice = await _unitOfWork.InvoiceRepository
                .FindByConditionAsync(x => x.OrderId == orderId)
                .Include(x => x.Order)
                .FirstOrDefaultAsync();

            if (invoice.HasNoValue())
            {
                return new ApiResponse<InvoiceDto>(null, false, "Invoice not found.");
            }

            var invoiceResponse = new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                TotalAmount = invoice.TotalAmount,
                InvoiceAt = invoice.InvoiceAt,
                OrderId = invoice.OrderId,
                OrderDto = new OrderDto
                {
                    OrderId = invoice.Order.OrderId,
                    PaymentStatus = invoice.Order.PaymentStatus
                }
            };

            return new ApiResponse<InvoiceDto>(invoiceResponse, true, "Invoice retrieved successfully.");
        }

        #region Helper methods of Invoice
        /// <summary>
        /// Generates a unique invoice number for an order based on the current date and the order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order for which the invoice is being generated.</param>
        /// <returns>A unique invoice number in the format "INV-yyyyMMdd-OrderId".</returns>
        private string GenerateInvoiceNumber(int orderId)
        {
            return $"INV-{DateTime.UtcNow:yyyyMMdd}-{orderId}";
        }

        #endregion
    }
}
