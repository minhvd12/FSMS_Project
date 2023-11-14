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
            var result = _context.ChatHistories.Where(c => (c.Sender == sender && c.Receiver == receiver) || (c.Sender == receiver && c.Receiver == sender)).OrderByDescending(c => c.SendTimeOnUtc).ToList();

            return Ok(result);
        }
    }
}
