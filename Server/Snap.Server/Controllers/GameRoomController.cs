using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Annotations;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoomController : ControllerBase
    {
        private readonly IGameRoomServices _roomService;
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly ISnapGameServices _snapGameServices;

        public GameRoomController(IGameRoomServices roomService,
            IGameRoomPlayerServices gameRoomService,
            ISnapGameServices snapGameServices)
        {
            _roomService = roomService;
            _gameRoomService = gameRoomService;
            _snapGameServices = snapGameServices;
        }

        [HttpPost]
        public async Task<ActionResult<GameRoom>> PostAsync(CancellationToken token) =>
            await _roomService.CreateAsync(token);

        [HttpPost("{roomId}/Players")]
        public async Task<ActionResult<GameRoomPlayer>> PostPlayerAsync([NotNull][FromRoute]int roomId,
            bool isViewer,
            CancellationToken token) =>
            await _gameRoomService.AddPlayersAsync(roomId, isViewer, token);

        [HttpPost("{roomId}/Game")]
        public async Task<ActionResult<SnapGame>> PostAsync([NotNull] [FromQuery] int roomId, CancellationToken token) =>
            await _snapGameServices.StarGameAsync(roomId, token);
    }
}
