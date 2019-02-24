using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Snap.Tests.Helpers;
using Snap.Tests.Module;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public class TurnsTests
    {
        public TurnsTests()
        {
        }

        public TurnsTests(IBackgroundHelper backgroundHelper)
        {
            _backgroundHelper = backgroundHelper;
        }

        private readonly IBackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_choosing_the_turns_should_not_be_repeated()
        {
            //Background or When
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);
            var game = await _backgroundHelper.StartGameAsync(room);

            //Then
            game.GameData
                .Turns
                .Select(p => p.Player.Username)
                .ShouldBeUnique();
            ;
        }

        [Fact]
        public async Task When_game_start_with_2_player_then_player_2_should_be_the_current_player()
        {
            //Background or When
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);
            var game = await _backgroundHelper.StartGameAsync(room);

            //Then
            game.CurrentTurn
                .PlayerTurn
                .Player
                .Username
                .ShouldBe(PlayerServiceSeedHelper.SecondPlayerUsername);
        }
    }
}