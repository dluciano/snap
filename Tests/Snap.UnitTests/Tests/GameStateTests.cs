using System.Threading.Tasks;
using Dawlin.Util.Impl.Exceptions;
using GameSharp.Entities.Enums;
using Shouldly;
using Snap.Entities;
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

        public GameStateTests(BackgroundHelper backgroundHelper)
        {
            _backgroundHelper = backgroundHelper;
        }

        private readonly BackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_create_game_state_should_be_awaiting_player()
        {
            //When
            var game = (await _backgroundHelper.CreateGameAsync()).GameData;

            //Then
            game.CurrentState.ShouldBe(GameState.AWAITING_PLAYERS);
        }

        [Fact]
        public async Task When_game_start_then_game_should_be_in_state_playing()
        {
            //Background
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);

            //When
            game = await _backgroundHelper.StartGameAsync(game);

            //Then
            game.GameData
                .CurrentState
                .ShouldBe(GameState.PLAYING);
        }

        [Fact]
        public async Task When_restarting_the_game_then_it_should_throw_an_error()
        {
            //Background
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);

            //When
            await StartGameAsync();

            //Then
            await StartGameAsync().ShouldThrowAsync<InvalidChangeTransition>();

            async Task<SnapGame> StartGameAsync()
            {
                return await _backgroundHelper.StartGameAsync(game);
            }
        }
    }
}