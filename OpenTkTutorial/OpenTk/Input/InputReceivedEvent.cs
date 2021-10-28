using System;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkProject
{
    public class InputReceivedEvent
    {
        public float time;
        public KeyboardState keyboard;
        public MouseState mouse;

        public InputReceivedEvent(object source, KeyboardState keyboard, MouseState mouse, float time)
        {
            this.keyboard = keyboard;
            this.mouse = mouse;
            this.time = time;
        }
    }
}
