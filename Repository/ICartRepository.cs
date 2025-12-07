using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface ICartRepository
    {
        Task<Cart> GetOrCreateCartByUserIdAsync(int userId);

        Task<CartItem> AddCartItemAsync(CartItem cartItem);

        Task RemoveCartItemAsync(int cartItemId);

        Task UpdateCartItemAsync(CartItem cartItem);

        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);

        Task ClearCartAsync(int cartId);

        Task<bool> ExistAsync(int userId);

        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
    }
}