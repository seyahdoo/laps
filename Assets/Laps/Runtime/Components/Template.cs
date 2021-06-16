using System;

namespace LapsRuntime {
    public class Template {
        public Type type;
        public string menuText = "default";
        public int menuPriority = 0;
    }
    public class LapsAddMenuOptionsAttribute : Attribute {
        public LapsAddMenuOptionsAttribute(string buttonLabel = null, int menuPriority = 0, bool hidden = false) {
            this.buttonLabel = buttonLabel;
            this.menuPriority = menuPriority;
            this.hidden = hidden;
        }
        public string buttonLabel;
        public int menuPriority;
        public bool hidden;
    }
}