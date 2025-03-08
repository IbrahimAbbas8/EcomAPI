namespace Ecom.Core.Entities.Orders
{
    public class ProductItemOrder
    {
        public ProductItemOrder()
        {
            
        }
        public ProductItemOrder(int productItemId, string productItemName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductItemName = productItemName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string PictureUrl { get; set; }
    }
}