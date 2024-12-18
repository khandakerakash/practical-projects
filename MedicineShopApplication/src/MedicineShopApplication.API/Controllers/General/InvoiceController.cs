﻿using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class InvoiceController : ApiAuthorizeController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("{orderId}/create")]
        public async Task<IActionResult> CreateInvoice(int orderId)
        {
            var requestMaker = User.GetUserIdInt();

            var response = await _invoiceService.CreateInvoice(requestMaker, orderId);
            return ToActionResult(response);
        }

        [HttpGet("id/{invoiceId}")]
        public async Task<IActionResult> GetInvoiceById(int invoiceId)
        {
            var response = await _invoiceService.GetInvoiceById(invoiceId);
            return ToActionResult(response);
        }

        [HttpGet("orderid/{orderId}")]
        public async Task<IActionResult> GetInvoiceByOrderId(int orderId)
        {
            var response = await _invoiceService.GetInvoiceByOrderId(orderId);
            return ToActionResult(response);
        }
    }
}
