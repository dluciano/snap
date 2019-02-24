using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.AspNetCore.SignalR;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Server.Services
{
    internal sealed class SignalRNotificationHub : Hub
    {
        private readonly ISnapGameCommands _commands;

        public SignalRNotificationHub(ISnapGameCommands commands)
        {
            _commands = commands;
        }

        public async Task<GameRoomPlayer> JoinGame(int roomId,
           bool isViewer, CancellationToken token = default(CancellationToken))
        {
            var roomPlayer = await _commands.JoinGame(roomId, isViewer, token);
            await NotifyRoomGroup(nameof(JoinGame), roomPlayer.GameRoom, roomPlayer, token);
            return roomPlayer;
        }

        public async Task<SnapGame> StartGame(int roomId, CancellationToken token = default(CancellationToken))
        {
            var game = await _commands.StartGameAsync(roomId, token);
            await NotifyRoomGroup(nameof(StartGame), game.GameData.Room, game, token);
            return game;
        }

        public async Task<PlayerGameplay> PopCard(int gameId, CancellationToken token = default(CancellationToken))
        {
            var gameplay = await _commands.PopCardAsync(gameId, token);
            await NotifyRoomGroup(nameof(PopCard), gameplay.GameData.Room, gameplay, token);
            return gameplay;
        }

        private async Task NotifyRoomGroup<TData>(string method, GameRoom room, TData data, CancellationToken token) =>
            await Clients.Group(room.GameIdentifier.ToString()).SendAsync(method, data, token);
    }
}
