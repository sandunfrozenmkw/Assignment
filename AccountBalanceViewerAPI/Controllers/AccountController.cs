using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Areas.Identity.Data.User;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration Configuration;

        public AccountController(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [Route("UploadData")]
        [HttpPost]
        public async Task<ActionResult> UploadData([FromBody] IEnumerable<UploadDataDto> request)
        {
            try
            {
                int count = request.ToList<UploadDataDto>().Count;
                if (count > 0) {
                    _context.UserAccounts.RemoveRange(_context.UserAccounts.Where(x => x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year));
                    await _context.SaveChangesAsync();
                }

                foreach (var item in request)
                {
                    if (item.DataColumns.Count != 2) {
                        continue;
                    }

                    var account = new UserAccounts
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item.DataColumns[0],
                        Date = DateTime.Now,
                        Amount = item.DataColumns[1],
                        Identifier = String.Empty,
                        UserId = String.Empty
                    };

                    await _context.UserAccounts.AddAsync(account);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetAccountBalances")]
        [HttpGet]
        public async Task<ActionResult> GetAccountBalances()
        {
            try
            {
                var data = _context.UserAccounts.AsEnumerable();
                data = data.Where(x => x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("GetReport")]
        [HttpGet]
        public async Task<ActionResult> GetReport()
        {
            try
            {
                var data = _context.UserAccounts.AsEnumerable();
                var orderData = data.GroupBy(x => x.Name);
                var result = new List<AccountData>();

                foreach (var item in orderData)
                {
                    var account = new AccountData();
                    account.Account = item.Key;
                    account.Amount = item.OrderByDescending(x => x.Date).Select(x=>x.Amount).ToList();

                    result.Add(account);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
