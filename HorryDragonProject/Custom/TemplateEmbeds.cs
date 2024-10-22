using Discord;
using HorryDragonProject.api.e621;
using HorryDragonProject.Service;

namespace HorryDragonProject.Custom {
    public class FormatPost
    {
        public static string GetSouces(List<string> Sources)
        {
            string text = "";
            try
            {
                foreach (string Source in Sources)
                {
                    if (Source == null)
                    {
                        return null;
                    }

                    Uri uri = new Uri(Source);
                    string host = uri.Host;
                    string path = uri.AbsolutePath;
                    switch (host)
                    {
                        case "www.furaffinity.net":
                            if (path.Contains("/user"))
                            {
                                text += $"[[Furaffinity Author]]({Source}) ";
                                break;
                            } else
                            {
                                text += $"[[Furaffinity]]({Source}) ";
                                break;
                            }     
                        case "bsky.app":
                            text += $"[[Bsky]]({Source}) ";
                            break;
                        case "x.com":
                            text += $"[[Twitter]]({Source}) ";
                            break;
                        case "twitter.com":
                            text += $"[[Twitter]]({Source}) ";
                            break;
                        
                    }
                }

                return text;
            } catch (Exception ex)
            {
                return null;
            }
           
               
        }
     }
    

    public class TemplateEmbeds
    {
        public static Color errorColor { get; } = Color.Red;
        public static Color warningColor { get; } = Color.Orange;
        public static Color successColor { get; } = Color.Green;
        private static Color _e621Color { get; } = Color.Blue;
        public static string footerText { get; } = "Dragofox property :b";
        public static string footerIco { get; } = "https://avatars.githubusercontent.com/u/51517881?v=4";

        private static EmbedFooterBuilder _creatorName = new EmbedFooterBuilder() {
          Text = footerText,
          IconUrl = footerIco
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


        public static EmbedBuilder PostEmbedTemplate(Post str, string tag)
        {
            var souces = FormatPost.GetSouces(str.Sources);
            string description = $"## Search tags:```{tag}```\n";
            description += $"## Tags: ```{string.Join(", ", str.Tags.General.Take(25))}```\n\n";
            description += $"[[LINK SOURCE]]({str.File.Url}) | [[Page e621]](https://e621.net/posts/{str.Id})\n";
            if (souces != null)
            {
                description += $"## Sources:\n{souces}";
            }
            

            EmbedBuilder postEmbed = new EmbedBuilder()
            {
                Description = description,
                ImageUrl = str.File.Url,
                Color = _e621Color 
            };

            return postEmbed;
        }

    }

    public class MessageImagePaged {

        private IReadOnlyCollection<Embed> Pages { get; }
        internal IUser User { get; }
        internal AppearanceOptions Options { get; }
        internal int CurrentPage { get; set; }
        internal List<Post> contextPost { get; set; }
        internal int Count => Pages.Count;

        public MessageImagePaged(IEnumerable<EmbedBuilder> builders, List<Post> post, IUser user = null,  AppearanceOptions options = null)
        {
            List<Embed> embeds = new List<Embed>();

            int i = 1;
            int n = 0;
            foreach (EmbedBuilder builder in builders)
            {

                builder.Footer ??= new EmbedFooterBuilder().WithText($"Page: {i++}/{builders.Count()} -- Post id:{post[n++].Id}").WithIconUrl(TemplateEmbeds.footerIco);
                embeds.Add(builder.Build());
            }
            Pages = embeds;
            contextPost = post;
            User = user;
            Options = options;
            CurrentPage = 1;
        }

        internal Embed GetEmbed()
        {
            var page = Pages.ElementAtOrDefault(CurrentPage - 1);
            if (page != null) {
                return page;
            }

            throw new ArgumentNullException();
        }

    }
}


