using System;
using System.Drawing;
using Discord;
using Color = Discord.Color;

namespace HorryDragonProject.Service
{
    public enum StopAction {
        Clear,
        DeleteMessage
    }

    public enum DisplayStyle {
        Full,
        Minimal,
        Selector
    }

    public class MessageImagePaged {
        private string title { get; }
        private Color embedColor { get; }
        private IReadOnlyCollection<Embed> Pages { get; }
        internal IUser user { get; }
        internal AppearanceOptions options { get; }
        internal int CurrentPage { get; set; }

        internal int Count => Pages.Count;

        public MessageImagePaged(IEnumerable<EmbedBuilder> builders, String image ,Color? embedColor = null, IUser? user = null,  AppearanceOptions options = null)
        {
            
        }

    }

    public class AppearanceOptions
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.Zero;
        public DisplayStyle Style { get; set; } = DisplayStyle.Full;
        public StopAction OnStop { get; set; } = StopAction.Clear;
        public StopAction TimeoutAction { get; set; } = StopAction.Clear;
    }


    public class ServicePaged
    {

    }
}


