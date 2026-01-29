using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreCore.WebApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.Product.Server
{
    [ApiController]
    [Route("/api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class TestController(IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpPost(nameof(AddRole))]

        public async Task<Result> AddRole([FromBody] string roleName)
        {
            await Task.Delay(2);
            return true;
        }

    }
}
