using System;
using System.Linq;
using System.Reflection;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Core;
using GamePlay.Scripts.Core.Ports;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.LevelUp;
using GamePlay.Scripts.LevelUp.Commands;
using GamePlay.Scripts.Service;
using NUnit.Framework;
using UnityEngine;
using VContainer;
using VContainer.Diagnostics;

namespace Tests.EditMode
{
    public sealed class LevelUpServiceTests
    {
        private sealed class DummyResolver : IObjectResolver
        {
            public object ApplicationOrigin => null;
            public DiagnosticsCollector Diagnostics { get; set; }

            public object Resolve(Type type, object key = null) =>
                throw new NotSupportedException("DummyResolver does not resolve services.");

            public bool TryResolve(Type type, out object resolved, object key = null)
            {
                resolved = null;
                return false;
            }

            public object Resolve(Registration registration) =>
                throw new NotSupportedException("DummyResolver does not resolve registrations.");

            public IScopedObjectResolver CreateScope(Action<IContainerBuilder> installation = null) =>
                throw new NotSupportedException("DummyResolver does not create scopes.");

            public void Inject(object instance)
            {
                // Not needed for these tests.
            }

            public bool TryGetRegistration(Type type, out Registration registration, object key = null)
            {
                registration = null;
                return false;
            }

            public void Dispose()
            {
            }
        }

        [Test]
        public void Ctor_ShouldThrow_WhenAnyDependencyIsNull()
        {
            var invalidWeaponViewDef = ScriptableObject.CreateInstance<GamePlay.Scripts.Equipment.Config.WeaponViewDefinition>();
            ForceSetGuid(invalidWeaponViewDef, "weapon-1");

            var invalidPassiveDef = ScriptableObject.CreateInstance<GamePlay.Scripts.Item.Config.PassiveItemDefinition>();
            ForceSetGuid(invalidPassiveDef, "passive-1");

            var weaponRegistry = new WeaponRegistry(new[] { invalidWeaponViewDef });
            var passiveRegistry = new PassiveItemRegistry(new[] { invalidPassiveDef });
            var weaponFactory = new WeaponFactory(new DummyResolver());
            var passiveItemFactory = new PassiveItemFactory();
            var randomProvider = new RandomProvider(runSeed: 123);

            Assert.Throws<ArgumentNullException>(() => _ = new LevelUpService(
                weaponRegistry: null,
                passiveItemRegistry: passiveRegistry,
                weaponFactory: weaponFactory,
                passiveItemFactory: passiveItemFactory,
                randomProvider: randomProvider));

            Assert.Throws<ArgumentNullException>(() => _ = new LevelUpService(
                weaponRegistry: weaponRegistry,
                passiveItemRegistry: null,
                weaponFactory: weaponFactory,
                passiveItemFactory: passiveItemFactory,
                randomProvider: randomProvider));

            Assert.Throws<ArgumentNullException>(() => _ = new LevelUpService(
                weaponRegistry: weaponRegistry,
                passiveItemRegistry: passiveRegistry,
                weaponFactory: null,
                passiveItemFactory: passiveItemFactory,
                randomProvider: randomProvider));

            Assert.Throws<ArgumentNullException>(() => _ = new LevelUpService(
                weaponRegistry: weaponRegistry,
                passiveItemRegistry: passiveRegistry,
                weaponFactory: weaponFactory,
                passiveItemFactory: null,
                randomProvider: randomProvider));

            Assert.Throws<ArgumentNullException>(() => _ = new LevelUpService(
                weaponRegistry: weaponRegistry,
                passiveItemRegistry: passiveRegistry,
                weaponFactory: weaponFactory,
                passiveItemFactory: passiveItemFactory,
                randomProvider: null));
        }

        [Test]
        public void RollOptions_ShouldFallbackToGold_WhenRegistriesContainOnlyInvalidDefinitions()
        {
            // LevelUpService 的 ctor 會要求 registry 不可為空；因此這裡放入「有 guid 但內容不完整」的定義，
            // 讓 BuildCandidatePool 不產生任何升級命令，最後由金幣選項補滿。
            var invalidWeaponViewDef = ScriptableObject.CreateInstance<GamePlay.Scripts.Equipment.Config.WeaponViewDefinition>();
            ForceSetGuid(invalidWeaponViewDef, "weapon-1");
            invalidWeaponViewDef.definition = null;
            invalidWeaponViewDef.prefab = null;

            var invalidPassiveDef = ScriptableObject.CreateInstance<GamePlay.Scripts.Item.Config.PassiveItemDefinition>();
            ForceSetGuid(invalidPassiveDef, "passive-1");

            var weaponRegistry = new WeaponRegistry(new[] { invalidWeaponViewDef });
            var passiveRegistry = new PassiveItemRegistry(new[] { invalidPassiveDef });

            var sut = new LevelUpService(
                weaponRegistry,
                passiveRegistry,
                new WeaponFactory(new DummyResolver()),
                new PassiveItemFactory(),
                new RandomProvider(runSeed: 123));

            var character = new Character();

            // 塞滿被動欄位，確保任何「新被動」都會被過濾掉，讓候選池保持為空。
            for (int i = 0; i < LevelUpService.MaxPassiveItems; i++)
            {
                character.Build.PassiveItems.Add(new PassiveItem(invalidPassiveDef));
            }

            var options = sut.RollOptions(character, optionCount: 3);

            Assert.AreEqual(3, options.Count);
            Assert.IsTrue(options.All(o => o is AddGoldCoinCommand));

            foreach (var option in options) option.Execute();

            Assert.AreEqual(300, character.GoldCoin); // level(1) * 100 * 3
        }

        private static void ForceSetGuid(GuidScriptableObject obj, string guid)
        {
            var field = typeof(GuidScriptableObject).GetField("guid", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null) throw new MissingFieldException(typeof(GuidScriptableObject).FullName, "guid");
            field.SetValue(obj, guid);
        }
    }
}

