namespace TechyolProject.Repositories
{
    public interface ICartRepository
    {

        Task<int> AddItem(int productId, int qty);

        Task<int> RemoveItem(int productId);

        Task<ShoppingCart> GetUserCart();

        Task<ShoppingCart> GetCart(string userId);

        string GetUserId();

        Task<int> GetCartItemCount(string userId = "");

        Task<bool> DoCheckout();

        Task<int> RemoveAll(int productId);

    }
}