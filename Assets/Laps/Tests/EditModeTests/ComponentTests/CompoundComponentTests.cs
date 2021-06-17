using NUnit.Framework;

namespace LapsEditModeTests {
    public class CompoundComponentTests {
        //nagative slot id's are for the inside connections
        //for each slot negative id mode 
        
        [Test] public void SignalFromOutsideToInside() { Assert.Fail(); }
        [Test] public void SignalFromInsideToOutside() { Assert.Fail(); }
        [Test] public void AddSlotInformationToInputSlotsChangesInputSlotsOnOutside() { Assert.Fail(); }
        [Test] public void AddSlotInformationToInputSlotsChangesOutputSlotsOnInside() { Assert.Fail(); }
        [Test] public void AddSlotInformationToOutputSlotsChangesOutputSlotsOnOutside() { Assert.Fail(); }
        [Test] public void AddSlotInformationToOutputSlotsChangesInputSlotsOnInside() { Assert.Fail(); }
    }
}
