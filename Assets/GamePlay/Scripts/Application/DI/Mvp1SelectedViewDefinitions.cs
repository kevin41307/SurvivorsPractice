using System;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Equipment.Config;
using GamePlay.Scripts.Item.Config;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Application.DI
{
    [Serializable]
    public sealed class Mvp1SelectedViewDefinitionRefs
    {
        [Required, SerializeField] private CharacterViewDefinition selectedCharacter;
        [Required, SerializeField] private EnemyViewDefinition selectedEnemy;
        [Required, SerializeField] private ExperienceGemViewDefinition selectedExperienceGem;
        [Required, SerializeField] private WeaponViewDefinition selectedWeapon;
        [Required, SerializeField] private TreasureChestViewDefinition selectedTreasureChest;
        [Required, SerializeField] private EnemyViewDefinition selectedTurret;

        public Mvp1SelectedViewDefinitions ToDefinitions() =>
            new Mvp1SelectedViewDefinitions(
                selectedCharacter,
                selectedEnemy,
                selectedExperienceGem,
                selectedWeapon,
                selectedTreasureChest,
                selectedTurret);
    }

    public sealed class Mvp1SelectedViewDefinitions
    {
        public CharacterViewDefinition SelectedCharacter { get; }
        public EnemyViewDefinition SelectedEnemy { get; }
        public ExperienceGemViewDefinition SelectedExperienceGem { get; }
        public WeaponViewDefinition SelectedWeapon { get; }
        public TreasureChestViewDefinition SelectedTreasureChest { get; }
        public EnemyViewDefinition SelectedTurret { get; }

        public Mvp1SelectedViewDefinitions(
            CharacterViewDefinition selectedCharacter,
            EnemyViewDefinition selectedEnemy,
            ExperienceGemViewDefinition selectedExperienceGem,
            WeaponViewDefinition selectedWeapon,
            TreasureChestViewDefinition selectedTreasureChest,
            EnemyViewDefinition selectedTurret)
        {
            SelectedCharacter = selectedCharacter;
            SelectedEnemy = selectedEnemy;
            SelectedExperienceGem = selectedExperienceGem;
            SelectedWeapon = selectedWeapon;
            SelectedTreasureChest = selectedTreasureChest;
            SelectedTurret = selectedTurret;
        }
    }
}
