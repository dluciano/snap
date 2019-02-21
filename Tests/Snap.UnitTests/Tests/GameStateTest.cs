using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities.Enums;
using GameSharp.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class GameStateTest
    {
        [Fact]
        public async Task When_create_game_state_should_be_awaiting_player()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //When
                var game = (await module.CreateGameAsync()).GameData;

                //Then
                game.CurrentState.ShouldBe(GameState.AWAITING_PLAYERS);
            }
        }

        [Fact]
        public async Task When_game_start_then_game_should_be_in_state_playing()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);

                //When
                game = await module.GameStart(game);

                //Then
                game.GameData
                    .CurrentState
                    .ShouldBe(GameState.PLAYING);

            }
        }

        [Fact]
        public async Task When_restarting_the_game_then_it_should_throw_an_error()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);

                //When
                await StartGameAsync();

                //Then
                await StartGameAsync().ShouldThrowAsync<InvalidChangeTransition>();

                async Task<SnapGame> StartGameAsync()
                {
                    return await module.GameStart(game);
                }
            }
        }
    }
}
