using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.DI;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class TurnsTests
    {
        [Fact]
        public async Task When_choosing_the_turns_should_not_be_repeated()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background or When
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);
                game = await module.GameStart(game);

                //Then
                game.GameData
                    .PlayerTurns
                    .Select(p => p.Player.Username)
                    .ShouldBeUnique(); ;
            }
        }

        [Fact]
        public async Task When_game_start_with_2_player_then_player_2_should_be_the_current_player()
        {
            using (var module = (await TestModuleHelpers
                .CreateAndBuildWithDefaultsAsync())
                .WithSecondPlayerFirstRandomizer())
            {
                //Background or When
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);
                game = await module.GameStart(game);

                //Then
                game.CurrentTurn
                    .PlayerTurn
                    .Player
                    .Username
                    .ShouldBe(PlayerServiceSeedHelper.SecondPlayerUsername);
            }
        }
    }
}