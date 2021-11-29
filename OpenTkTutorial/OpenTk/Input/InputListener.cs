using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkProject
{
    public class InputListener
    {
        // Properties
        private static bool Listen;
        private Thread Thread;
        private Window Window;
        private double Delay;
        private Keys EndKey;
        private Keys[] CameraKeys;

        // Events properties
        private List<IInputListener> Observers;

        // Methods
        public InputListener(Window Window, double Delay)
        {
            this.Window = Window;
            this.Delay = Delay;
            this.Observers = new List<IInputListener>();
        }

        public void Start()
        {
            Listen = true;
            Thread = new Thread(new ThreadStart(Process));
            Thread.Start();
        }

        public void Stop()
        {
            Listen = false;
        }

        private void Process()
        {
            Console.WriteLine("(Input listener)\tInput listener up.");

            DateTime lastTry = System.DateTime.Now;
            DateTime now;

            while (Listen)
            {
                now = System.DateTime.Now;

                //Console.WriteLine("(Input listener)\tListen attemp at {0}.", now);
                if (now >= lastTry.AddMilliseconds(Delay) && Window.IsFocused)
                {
                    try
                    {
                        // Check keyboard and mouse
                        if ( Window.KeyboardState.IsAnyKeyDown )
                        {
                            Console.WriteLine("(Input listener)\tInput received at {0}.", now);
                            OnInputReceived(Window.KeyboardState, Window.MouseState);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unhandled expection detected.");
                    }

                    lastTry = now;
                }
            }            
            Console.WriteLine("(Input listener)\tInput listener down.");
        }

        // Interface Methods
        public void RegisterObserver(IInputListener observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IInputListener observer)
        {
            Observers.Remove(observer);
        }

        // Dispachers
        public void OnInputReceived(KeyboardState keyboard, MouseState mouse)
        {
            InputReceivedEvent e = new InputReceivedEvent(this, keyboard, mouse, (float)Delay/ 1000);
            foreach (IInputListener observer in Observers)
            {
                observer.OnInputReceived(e);
            }
        }
    }
}
