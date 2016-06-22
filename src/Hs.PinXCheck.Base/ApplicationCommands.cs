using System;
using Prism.Commands;

namespace Hs.PinXCheck.Base
{
    public static class ApplicationCommands
    {
        public static CompositeCommand ShowFlyoutCommand = new CompositeCommand();

        public static CompositeCommand ShowProgressDialog = new CompositeCommand();
    }

    public interface IApplicationCommands
    {
        CompositeCommand ShowFlyoutCommand { get; }

        CompositeCommand ShowProgressDialog { get; }
    }

    public class ApplicationCommandsProxy : IApplicationCommands
    {
        public CompositeCommand ShowFlyoutCommand
        {
            get { return ApplicationCommands.ShowFlyoutCommand; }
        }

        public CompositeCommand ShowProgressDialog
        {
            get { return ApplicationCommands.ShowProgressDialog; }
        }
    }
}
