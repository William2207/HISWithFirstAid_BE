using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement

{
    public class CartRepository : ICartRepository
    {
        private readonly FirstAidContext _context;

        public CartRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetOrCreateCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
               .Include(c => c.CartItems)
                   .ThenInclude(ci => ci.PracticalCourse) // Include course để map DTO
               .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart; // Luôn return Cart (không null)
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int userId)
        {
            return await _context.Carts.AnyAsync(c => c.UserId == userId);
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.PracticalCourse) // Include course để map DTO
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }
    }
}