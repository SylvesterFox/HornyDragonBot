
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HorryDragonProject.api.e621;
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
/*        private string Title { get; }
        private Color EmbedColor { get; }*/
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

                builder.Footer ??= new EmbedFooterBuilder().WithText($"Page: {i++}/{builders.Count()} -- Post id:{post[n++].Id}");
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

    public class AppearanceOptions
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.Zero;
        public DisplayStyle Style { get; set; } = DisplayStyle.Full;
        public StopAction OnStop { get; set; } = StopAction.Clear;
        public StopAction TimeoutAction { get; set; } = StopAction.Clear;
    }


    public class ServicePaged
    {
        private readonly Dictionary<ulong, MessageImagePaged> imageMessage;

        public ServicePaged(DiscordSocketClient client)
        {
            imageMessage = new Dictionary<ulong, MessageImagePaged>();
            client.ButtonExecuted += ButtonHandlerEmbedImage;
        }

        public async Task<IUserMessage> SendImageMessage(SocketInteractionContext ctx, MessageImagePaged paged, bool folloup = false)
        {
            IUserMessage message;

            if (paged.Count > 1)
            {
                ComponentBuilder builder = new ComponentBuilder();

                switch (paged.Options.Style)
                {
                    case DisplayStyle.Full:
                        builder.WithButton("<--", "first", ButtonStyle.Secondary);
                        builder.WithButton("<-", "back", ButtonStyle.Primary);
                        builder.WithButton("X", "stop", ButtonStyle.Danger);
                        builder.WithButton("->", "next", ButtonStyle.Primary);
                        builder.WithButton("-->", "last", ButtonStyle.Secondary);
                        break;

                    case DisplayStyle.Minimal:
                        builder.WithButton("<-", "back", ButtonStyle.Primary);
                        builder.WithButton("X", "stop", ButtonStyle.Danger);
                        builder.WithButton("->", "next", ButtonStyle.Primary);
                        break;

                    case DisplayStyle.Selector:
                        builder.WithButton("<-", "back", ButtonStyle.Primary);
                        builder.WithButton("Select", "select", ButtonStyle.Success);
                        builder.WithButton("->", "next", ButtonStyle.Primary);
                        break;
                }

                if (folloup)
                {
                    message = await ctx.Interaction.FollowupAsync(embed: paged.GetEmbed(), components: builder.Build());
                } else
                {
                    await ctx.Interaction.RespondAsync(embed: paged.GetEmbed(), components: builder.Build());
                    message = await ctx.Interaction.GetOriginalResponseAsync();
                }

            } else
            {
                if (folloup)
                {
                    message = await ctx.Interaction.FollowupAsync(embed: paged.GetEmbed());
                } else
                {
                    await ctx.Interaction.RespondAsync(embed: paged.GetEmbed());
                    message = await ctx.Interaction.GetOriginalResponseAsync();
                }

                return message;
            }

            if (imageMessage != null)
            {
                imageMessage.Add(message.Id, paged);
            }
            else
            {
                throw new ArgumentNullException();
            }

            if (paged.Options.Timeout != TimeSpan.Zero)
            {
                Task _ = Task.Delay(paged.Options.Timeout).ContinueWith(async t =>
                {
                    if (!imageMessage.ContainsKey(message.Id))
                    {
                        return;
                    }

                    switch (paged.Options.TimeoutAction)
                    {
                        case StopAction.DeleteMessage:
                            await message.DeleteAsync();
                            break;
                        case StopAction.Clear:
                            await message.RemoveAllReactionsAsync();
                            break;
                    }

                    imageMessage.Remove(message.Id);
                });
            }

            return message;

        }

        public async Task ButtonHandlerEmbedImage(SocketMessageComponent component)
        {
            MessageImagePaged? page;


            if (imageMessage.TryGetValue(component.Message.Id, out page))
            {
                if (component.User.Id != page.User.Id)
                {
                    return;
                }

                switch (component.Data.CustomId)
                {
                    case "first":
                        if (page.CurrentPage != 1)
                        {
                            page.CurrentPage = 1;
                            await component.UpdateAsync(x => x.Embed = page.GetEmbed());
                        }
                        break;

                    case "back":
                        if (page.CurrentPage != 1)
                        {
                            page.CurrentPage--;
                            await component.UpdateAsync(x => x.Embed = page.GetEmbed());
                        }
                        break;

                    case "next":
                        if (page.CurrentPage != page.Count)
                        {
                            page.CurrentPage++;
                            await component.UpdateAsync(x => x.Embed = page.GetEmbed());
                        }
                        break;

                    case "last":
                        if (page.CurrentPage != page.Count)
                        {
                            page.CurrentPage = page.Count;
                            await component.UpdateAsync(x => x.Embed = page.GetEmbed());
                        }
                        break;

                    case "stop":
                        switch (page.Options.OnStop)
                        {
                            case StopAction.DeleteMessage:
                                await component.Message.DeleteAsync();
                                break;

                            case StopAction.Clear:
                                await component.UpdateAsync(x => x.Components = null);
                                break;
                        }

                        imageMessage.Remove(component.Message.Id);
                        break;

                    case "select":
                        await component.UpdateAsync(x => x.Components = null);
                        imageMessage.Remove(component.Message.Id);
                        break;
                }
            }
        }
        
    }
}


