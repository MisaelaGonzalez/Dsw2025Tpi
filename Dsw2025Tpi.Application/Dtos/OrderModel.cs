﻿using Dsw2025Tpi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dsw2025Tpi.Application.Dtos
{
   
    public record OrderModel
    {
        //Requests

        public record OrderRequest(Guid CustomerId, string ShippingAddress, string BillingAddress, List<OrderItemModel> OrderItems);
       
        public record OrderItemModel(Guid ProductId, int Quantity);
        
        public record StatusUpdateRequest(string NewStatus);

        
        //Responses
        public record OrderResponse(Guid Id,
            Guid CustomerId,
            string ShippingAddress,
            string BillingAddress,
            DateTime Date,
            decimal TotalAmount,
            List<OrderItemResponse> OrderItems,
            string Status);
        
        public record OrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPriceint, int Quantity, decimal Subtotal);


    }
}
