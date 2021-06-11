using System.Collections.Generic;

namespace LapsRuntime {
    [LapsAddMenuOptions("Logic/Value/Number")]
    public class NumberValue : BaseValue<float> {
        public override object HandleInput(int index, object argument, LapsComponent eventSource) {
            switch(index) {
                case 21: value += (float)argument;  OnValueChanged(); return null;
                case 22: value -= (float)argument;  OnValueChanged(); return null;
                case 23: value++;                   OnValueChanged(); return null;
                case 24: value--;                   OnValueChanged(); return null;
            }
            return base.HandleInput(index, argument, eventSource);;
        }
        public override void GetInputSlots(List<LogicSlot> slots) {
            base.GetInputSlots(slots);
            slots.Add(new LogicSlot("increment value by", 21, typeof(float)));
            slots.Add(new LogicSlot("decrement value by", 22, typeof(float)));
            slots.Add(new LogicSlot("increment value by one", 23));
            slots.Add(new LogicSlot("decrement value by one", 24));
        }
    }
}
