using Dawlin.Abstract.Entities;

namespace Snap.Entities
{
    public class Player : IEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }
    }
}