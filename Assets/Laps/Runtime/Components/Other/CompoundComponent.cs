
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LapsRuntime {
    [LapsAddMenuOptions("Other/Compound")]
    public class CompoundComponent : LapsComponent {
        public List<SlotInformation> inputSlots;
        public List<SlotInformation> outputSlots;

        public override void GetInputSlots(SlotList slots) {
            foreach (var slotInformation in inputSlots) {
                slots.Add(slotInformation.LogicSlot);
            }
        }
        public override void GetOutputSlots(SlotList slots) {
            foreach (var slotInformation in outputSlots) {
                slots.Add(slotInformation.LogicSlot);
            }
        }
        [Serializable]
        public class SlotInformation {
            public string name;
            public int id;
            public TypeEnum parameterType;
            public TypeEnum returnType;
            public Type ParameterType => TypeEnumToType(parameterType);
            public Type ReturnType => TypeEnumToType(returnType);
            public LogicSlot LogicSlot => new LogicSlot(name, id, ParameterType, ReturnType);
        }
        public enum TypeEnum {
            Null,
            Float,
            Color,
            String,
            Rigidbody,
            Rigidbody2D,
            Transform,
        }
        public static Type TypeEnumToType(TypeEnum typeEnum) {
            switch (typeEnum) {
                case TypeEnum.Null:        return null;
                case TypeEnum.Float:       return typeof(float);
                case TypeEnum.Color:       return typeof(Color);
                case TypeEnum.String:      return typeof(string);
                case TypeEnum.Rigidbody:   return typeof(Rigidbody);
                case TypeEnum.Rigidbody2D: return typeof(Rigidbody2D);
                case TypeEnum.Transform:   return typeof(Transform);
                default:                   return null;
            }
        }
    }
}
