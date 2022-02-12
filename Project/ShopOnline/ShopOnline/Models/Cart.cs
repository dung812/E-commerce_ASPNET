using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.Models
{
    public class Cart
    {
        menfashionEntities db = new menfashionEntities();
        public int IdItem { get; set; }
        public string NameItem { get; set; }
        public string ImageItem { get; set; }
        public int PriceItem { get; set; }
        public int unitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public Double PriceTotal
        {
            get
            {
                return Quantity * PriceItem;
            }
        }
        public Cart(int idProduct)
        {
            this.IdItem = idProduct;
            Product item = db.Products.Single(model => model.productId == IdItem);
            this.NameItem = item.productName;
            this.ImageItem = item.image;
            this.unitPrice = int.Parse(item.price.ToString());
            this.PriceItem = int.Parse(item.price.ToString()) - ((int.Parse(item.price.ToString()) * int.Parse(item.discount.ToString())) / 100);
            this.Quantity = 1;
            this.Discount = ((int.Parse(item.price.ToString()) * int.Parse(item.discount.ToString())) / 100) * this.Quantity;
        }
    }
}