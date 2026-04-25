using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Ports;
using GamePlay.Scripts.Service.Ports;

namespace GamePlay.Scripts.Service
{
    public class PlayerLocatorService : IPlayerLocator
    {
        public Player Player { get; private set;}
        public void SetPlayer(Player player) => Player = player;
    }
}