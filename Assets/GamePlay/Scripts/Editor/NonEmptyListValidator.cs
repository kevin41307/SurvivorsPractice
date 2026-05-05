using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Scripts.Core;
using GamePlay.Scripts.Editor;
using GamePlay.Scripts.Equipment.Config;
using GamePlay.Scripts.Item.Config;
using GamePlay.Scripts.MetaProgress.Config;
using Sirenix.OdinInspector.Editor.Validation;

[assembly: RegisterValidator(typeof(NonEmptyListValidator<MetaPowerUpDefinition>))]
[assembly: RegisterValidator(typeof(NonEmptyListValidator<WeaponViewDefinition>))]
[assembly: RegisterValidator(typeof(NonEmptyListValidator<PassiveItemDefinition>))]


namespace GamePlay.Scripts.Editor
{
    public class NonEmptyListValidator<T> : AttributeValidator<NonEmptyListAttribute, List<T>>
    {
        protected override void Validate(ValidationResult result)
        {
            
            if (ValueEntry == null || ValueEntry.SmartValue == null)
            {
                result.AddError("List 為 null。");
                return;
            }
            
            
            if (!ValueEntry.SmartValue.Any())
            {
                result.AddError("List 不能為空。");
            }
        }
    }
}
