using System.Collections.Generic;
using GamePlay.Scripts.Equipment;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Status.Buff;
using System.Linq;


namespace GamePlay.Scripts.Actor
{
    public class Build
    {
        public List<PassiveItem> PassiveItems { get; set; } = new();
        public List<PowerUp> PowerUps { get; set; } = new();
        public List<Weapon> Weapons { get; set; } = new();

        public void AddPassiveItem(PassiveItem item)
        {
            PassiveItems.Add(item);
        }

        public void AddPowerUp(PowerUp item)
        {
            PowerUps.Add(item);
        }

        public void AddWeapon(Weapon item)
        {
            Weapons.Add(item);
        }

        public void AddBuild(Build other)
        {
            other.PassiveItems.ForEach(AddPassiveItem);
            other.PowerUps.ForEach(AddPowerUp);
            other.Weapons.ForEach(AddWeapon);
        }


    }
}