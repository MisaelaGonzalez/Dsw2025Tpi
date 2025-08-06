﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dsw2025Tpi.Application.Dtos

{
    public record ProductModel
    {
        //Request

        public record ProductRequest(string Sku, string Name, decimal CurrentUnitPrice, string InternalCode, string Description, int StockQuantity);

        // Response
        
        public record ProductResponse(Guid Id, string Sku, string Name, decimal CurrentUnitPrice, string InternalCode, string Description, int StockQuantity);
        public record ProductResponseUpdate(Guid Id, string Sku, string Name, decimal CurrentUnitPrice, string InternalCode, string Description, int StockQuantity, bool IsActive);
   
        public record ProductResponseID(Guid Id);
    }
}
