using FirstAidAPI.DTO.Cart;

namespace FirstAidAPI.Service
{
    public interface ICartService
    {
        Task<CartResponseDto?> GetOrCreateCartByUserIdAsync(int userId);

        Task<CartItemResponseDto> AddCartItemAsync(int userId, AddCartItemRequestDto cartItem);

        Task RemoveCartItemAsync(int cartItemId);

        Task UpdateCartItemAsync(CartItemResponseDto cartItem);

        Task<IEnumerable<CartItemResponseDto>> GetCartItemsAsync(int cartId);

        Task ClearCartAsync(int cartId);

        Task<bool> ExistAsync(int userId);
    }
}