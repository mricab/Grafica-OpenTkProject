using System;
namespace OpenTkProject
{
    public interface IInputListener
    {
        void OnInputReceived(InputReceivedEvent e);
    }
}
