using FSMS.Entity.Models;
using FSMS.Service.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private IConfiguration _configuration;
        private FruitSeasonManagementSystemV10Context _context;
        public ChatsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _context = new FruitSeasonManagementSystemV10Context();
        }

        [HttpGet]
        [Route("/[controller]/History/{receiver:int}")]
        [PermissionAuthorize("Supplier", "Farmer", "Admin")]
        public async Task<IActionResult> History(int receiver)
        {
            var sender = Int32.Parse(HttpContext.User.Identity.Name);
            var result = _context.ChatHistory.Where(c => (c.Sender == sender && c.Receiver == receiver) || (c.Sender == receiver && c.Receiver == sender)).OrderByDescending(c => c.SendTimeOnUtc).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("/[controller]/Users")]
        [PermissionAuthorize("Supplier", "Farmer", "Admin")]
        public async Task<IActionResult> GetChatUsers()
        
        {
            var roleName = HttpContext.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "role")?.Value;
            var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
            //var users = _context.Users.Where(user => user.RoleId == role.RoleId).ToList();
            var users = _context.Users.ToList();

            var response = users.Select(user =>
            {
                return new
                {
                    user.UserId,
                    user.RoleId,
                    user.Email,
                    user.FullName
                };
            });

            return Ok(response);
        }
    }
}
