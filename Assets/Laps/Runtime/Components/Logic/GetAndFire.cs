
namespace LapsRuntime {
    [LapsAddMenuOptions("Logic/Get And Fire")]
    public class GetAndFire : LapsComponent {
        public override object HandleInput(int slotId, object parameter, LapsComponent eventSource) {
            switch (slotId) {
                case 0: FireOutput(1, FireOutput(0)); return null;
                default: return null;
            }
        }
        public override void GetInputSlots(SlotList slots) {
            slots.Add("trigger", 0);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("get value", 0);
            slots.Add("fire wire", 1);
        }
    }
}
