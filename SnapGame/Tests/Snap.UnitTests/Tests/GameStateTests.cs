using System.Threading.Tasks;
using GameSharp.Entities.Enums;
using Shouldly;
using Snap.Entities;
using Snap.Services.Impl.Exceptions;
using Snap.Tests.Helpers;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public class GameStateTests
    {
        public GameStateTests()
        {
        }

        public GameStateTests(IBackgroundHelper backgroundHelper)
        {
            _backgroundHelper = backgroundHelper;
        }

        private readonly IBackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_game_start_then_game_should_be_in_state_playing()
        {
            //Background
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);

            //When
            var game = await _backgroundHelper.StartGameAsync(room);

            //Then
            game.GameData
                .CurrentState
                .ShouldBe(GameState.PLAYING);
        }

        [Fact]
        public async Task When_try_to_start_a_ongoin_game_then_it_should_throw_an_error()
        {
            //Background
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);

            //When
            await StartGameAsync();

            //Then
            await StartGameAsync()
                .ShouldThrowAsync<GameAlreadyStartedException>();

            async Task<SnapGame> StartGameAsync()
            {
                return await _backgroundHelper.StartGameAsync(room);
            }
        }
    }
}