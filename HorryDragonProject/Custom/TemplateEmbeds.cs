using Discord;

namespace HorryDragonProject.Custom {
    public class TemplateEmbeds
    {
        public static Color errorColor { get; } = Color.Red;
        public static Color warningColor { get; } = Color.Orange;
        public static Color successColor { get; } = Color.Green;
        private static Color _e621Color { get; } = Color.Blue;

        private static EmbedFooterBuilder _creatorName = new EmbedFooterBuilder() {
          Text = "Dragofox property :b",
          IconUrl = "https://media.discordapp.net/attachments/896091932000927745/1250845431823335537/dragofox_new_pfp.png?ex=66c6bad6&is=66c56956&hm=2e27c45b3d14fdd21cc8cf9b899e9dc4f9d95b7e5ee8df72a4e56380243a3b1b&=&format=webp&quality=lossless&width=575&height=450"
        };
        
        public static Embed embedError(string msg, Color? color = null) {
            EmbedBuilder embedEr = new EmbedBuilder() {
                Color = color ?? errorColor,
                Title = "Exception!",
                Description = msg
            };

            embedEr.WithFooter(_creatorName);
            return embedEr.Build();

         
        }

        public static Embed embedWarning(string msg, string Title, Color? color = null) {
            EmbedBuilder embedWarn = new EmbedBuilder() {
                Color = color ?? warningColor,
                Title = Title,
                Description = msg
            };

            embedWarn.WithFooter(_creatorName);
            return embedWarn.Build();

        }
        
        public static Embed embedSuccess(string msg, string? title = null,  Color? color = null) {
            EmbedBuilder embed = new EmbedBuilder() {
                Color = color ?? successColor,
                Title = title ?? "Success!",
                Description = msg
            };

            embed.WithFooter(_creatorName);
            return embed.Build();
        }


        
    }
}


