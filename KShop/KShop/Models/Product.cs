using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Models
{
    public class Product
    {

        int productId;
        string productName,image;
        int quantity;
        float price;
        int categoryId;
        DateTime createdDate;
        bool status;

        public Product() {
        }

        public Product(string productName, string image, int quantity, float price, int categoryId) {
            this.productName = productName;
            this.image = image;
            this.quantity = quantity;
            this.price = price;
            this.categoryId = categoryId;
        }

        public Product(int productId, string productName, string image, int quantity, float price, int categoryId, DateTime createdDate, bool status)
        {
            this.productId = productId;
            this.productName = productName;
            this.image = image;
            this.quantity = quantity;
            this.price = price;
            this.categoryId = categoryId;
            this.createdDate = createdDate;
            this.status = status;
        }

        public int ProductId { get => productId; set => productId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public float Price { get => price; set => price = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
        public bool Status { get => status; set => status = value; }
        public string Image { get => image; set => image = value; }
    }
}
