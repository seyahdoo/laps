
namespace LapsRuntime {
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
