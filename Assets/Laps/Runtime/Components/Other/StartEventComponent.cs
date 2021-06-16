
namespace LapsRuntime {
    [LapsAddMenuOptions("Other/Start Event")]
    public class StartEventComponent : LapsComponent {
        private void Update() {
            enabled = false;
            FireOutput(0, null);
        }
        public override void GetOutputSlots(SlotList slots) {
            slots.Add("start", 0);
        }
    }
}
