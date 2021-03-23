using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Models {
    public class FormCreateProduct {

        string productName;
        string quantity;
        string price;
        string categoryId;
        
        IFormFile file;

        public FormCreateProduct() {
        }

        public FormCreateProduct(string productName, string quantity, string price, string categoryId, IFormFile file) {
            this.productName = productName;
            this.quantity = quantity;
            this.price = price;
            this.categoryId = categoryId;
            this.file = file;
        }

        public string ProductName { get => productName; set => productName = value; }
        public string Quantity { get => quantity; set => quantity = value; }
        public string Price { get => price; set => price = value; }
        public string CategoryId { get => categoryId; set => categoryId = value; }
        public IFormFile File { get => file; set => file = value; }
    }
}
