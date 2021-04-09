using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Services;
using SocialMedia.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SocialMedia.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost("movecard")]
        public IActionResult MoveCard([FromBody] MoveCardCommand command)
        {
            _boardService.Move(command);

            return Ok(new
            {
                Moved = true
            });
        }

        [HttpGet("GetUsers")]
        public IEnumerable<BoardList.Board> GetUsers(double Longitude, double Latitude)
        {
            var model = _boardService.ListBoard(Latitude, Longitude).Boards;
            return model;
        }
    }
}