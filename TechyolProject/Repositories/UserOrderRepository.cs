using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TechyolProject.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders = await _context.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetails)
                            .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Category)
                            .Where(a => a.UserID == userId)
                            .ToListAsync();
            return orders;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

    }
}
