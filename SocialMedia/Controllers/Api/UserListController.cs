using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Services;

namespace SocialMedia.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserListController : ControllerBase
    {
        private readonly BoardService _boardService;

        public UserListController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost("AddLocation")]
        public IActionResult AddLocation([FromBody] Users user)
        {
            //_boardService.Move(command);

            return Ok(new
            {
                Moved = true
            });
        }
    }
}
