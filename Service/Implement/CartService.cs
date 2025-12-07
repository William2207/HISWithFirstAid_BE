using FirstAidAPI.Repository;
using FirstAidAPI.DTO.Cart;
using AutoMapper;
using FirstAidAPI.Models;
using FirstAidAPI.Exceptions;

namespace FirstAidAPI.Service.Implement
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;
        private readonly IMapper _mapper;
        private readonly IPracticalCourseRepository _courseRepository;

        public CartService(ICartRepository cartRepository, ILogger<CartService> logger, IMapper mapper, IPracticalCourseRepository courseRepository)
        {
            _cartRepository = cartRepository;
            _logger = logger;
            _mapper = mapper;
            _courseRepository = courseRepository;
        }

        public async Task<CartItemResponseDto> AddCartItemAsync(int userId, AddCartItemRequestDto request)
        {
            if (request == null)
            {
                _logger.LogError("Cart item request is null");
                throw new ArgumentNullException(nameof(request));
            }

            // 1. Get or create cart
            var cart = await _cartRepository.GetOrCreateCartByUserIdAsync(userId);

            // 2. Validate course tồn tại và lấy giá
            var course = await _courseRepository.GetByIdAsync(request.PracticalCourseId);
            if (course == null)
            {
                _logger.LogWarning($"Course {request.PracticalCourseId} not found");
                throw new NotFoundException($"Course with ID {request.PracticalCourseId} not found");
            }

            // 3. Check duplicate item trong cart
            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.PracticalCourseId == request.PracticalCourseId);

            if (existingItem != null)
            {
                // Đã có -> cập nhật số lượng
                existingItem.Quantity += request.Quantity;
                await _cartRepository.UpdateCartItemAsync(existingItem);

                // Load lại với course info
                var updatedItem = await _cartRepository.GetCartItemByIdAsync(existingItem.Id);
                return _mapper.Map<CartItemResponseDto>(updatedItem);
            }

            // 4. Tạo mới cart item
            var cartItem = _mapper.Map<CartItem>(request);
            cartItem.CartId = cart.Id;
            cartItem.Price = course.Price; // Lấy giá từ DB, không tin client
            cartItem.AddedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            // 5. Thêm vào DB
            var newItem = await _cartRepository.AddCartItemAsync(cartItem);

            // 6. Load lại với course info để map đầy đủ
            var itemWithCourse = await _cartRepository.GetCartItemByIdAsync(newItem.Id);

            return _mapper.Map<CartItemResponseDto>(itemWithCourse);
        }

        public async Task<CartResponseDto?> GetOrCreateCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetOrCreateCartByUserIdAsync(userId);

            return _mapper.Map<CartResponseDto>(cart);
        }

        public async Task ClearCartAsync(int cartId)
        {
            await _cartRepository.ClearCartAsync(cartId);
        }

        public async Task<bool> ExistAsync(int userId)
        {
            return await _cartRepository.ExistAsync(userId);
        }

        public async Task<IEnumerable<CartItemResponseDto>> GetCartItemsAsync(int cartId)
        {
            var cartItems = await _cartRepository.GetCartItemsAsync(cartId);
            return _mapper.Map<IEnumerable<CartItemResponseDto>>(cartItems);
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            await _cartRepository.RemoveCartItemAsync(cartItemId);
        }

        public async Task UpdateCartItemAsync(CartItemResponseDto cartItem)
        {
            var item = _mapper.Map<CartItem>(cartItem);
            await _cartRepository.UpdateCartItemAsync(item);
        }
    }
}