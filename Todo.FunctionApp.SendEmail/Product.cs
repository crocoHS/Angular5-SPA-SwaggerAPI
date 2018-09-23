﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.FunctionApp.SendEmail
{
    public class Product
    {
        public string ProductId { get; set; }
        public string Description { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UOM { get; set; }
        public decimal Price { get; set; }
    }
}
