using GamePlay.Scripts.Actor;

namespace GamePlay.Scripts.Service.Ports
{
    public interface IPlayerLocator
    {
        Player Player { get; }

        void SetPlayer(Player player); 

    }
}