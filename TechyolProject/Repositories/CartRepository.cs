using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TechyolProject.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
           UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(int productId,int qty)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try {
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged-in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId

                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                var cartItem = _context.CartDetails.FirstOrDefault(c => c.ShoppingCartId == cart.Id && c.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _context.Product.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = product.Price
                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();

            } catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }

        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("cart is empty");
                }

                var cartItem = _context.CartDetails
                                .FirstOrDefault(a=>a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                {
                    throw new Exception("No items in cart");
                } else if (cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("hata");
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }

        public async Task<int> RemoveAll(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user is not logged in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("cart is empty");
                }

                var cartItem = _context.CartDetails
                                .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                {
                    throw new Exception("No items in cart");
                }
                else
                {
                    _context.CartDetails.Remove(cartItem);
                    // Sipariş oluştur
                    var order = new Order
                    {
                        UserID = userId,
                        CreateDate = DateTime.UtcNow,
                        OrderStatusID = 1 // Tamamlandı
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    // Sipariş detayı oluştur
                    var orderDetail = new OrderDetail
                    {
                        ProductId = productId,
                        OrderId = order.Id,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("hata");
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }


        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _context.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;


        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }


        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }


        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _context.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserID = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusID = 1//işlemde
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();

                // removing the cartdetails
                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
