using GamePlay.Scripts.Status;

namespace GamePlay.Scripts.Actor
{
    public class Character
    {
        public Build Build { get; set; } = new();

        public StatusContainer StatusContainer { get; } = new();
    }
}
