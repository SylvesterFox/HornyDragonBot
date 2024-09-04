
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HorryDragonProject.api.e621;

namespace HorryDragonProject.Service
{
    public enum StopAction {
        Clear,
        DeleteMessage
    }

    public enum DisplayStyle {
        Full,
        Minimal,
        Selector,
        ViewerE
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

    public class MessageVideoPaged {

        private IReadOnlyCollection<string> Pages { get; }
        internal IUser User { get; }
        internal AppearanceOptions Options { get; }
        internal int CurrentPage { get; set; }
        internal List<Post> contextPost { get; set; }
        internal int Count => Pages.Count;


        public MessageVideoPaged(IEnumerable<string> messagesVideo, List<Post> post, IUser user = null,  AppearanceOptions options = null)
        {
            List<string> msgPage = new List<string>();

            int i = 1;
            int n = 0;
            foreach (var message in messagesVideo) {
                string msg = message + $"```\nPage: {i++}/{messagesVideo.Count()} -- Post id:{post[n++].Id}```";
                msgPage.Add(msg);
            }
            Pages = msgPage;
            contextPost = post;
            User = user;
            Options = options;
            CurrentPage = 1;
        }

         internal string GetMessage()
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
        private readonly Dictionary<ulong, MessageVideoPaged> videoMessage;

        public ServicePaged(DiscordSocketClient client)
        {
            imageMessage = new Dictionary<ulong, MessageImagePaged>();
            videoMessage = new Dictionary<ulong, MessageVideoPaged>();
            client.ButtonExecuted += ButtonHandlerImage;
        }

        public async Task<IUserMessage> SendMessage(SocketInteractionContext ctx, MessageImagePaged paged,  bool folloup = false)
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

                    case DisplayStyle.ViewerE:
                        builder.WithButton("<--", "first", ButtonStyle.Secondary);
                        builder.WithButton("<-", "back", ButtonStyle.Primary);
                        builder.WithButton("X", "stop", ButtonStyle.Danger);
                        builder.WithButton("->", "next", ButtonStyle.Primary);
                        builder.WithButton("-->", "last", ButtonStyle.Secondary);
                        builder.WithButton("Prev page", "p", ButtonStyle.Secondary, row:1);
                        builder.WithButton("More", "n", ButtonStyle.Secondary, row: 1);                      
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

        public async Task<IUserMessage> SendMessageVideoPost(SocketInteractionContext ctx, MessageVideoPaged paged,  bool folloup = false)
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

                    case DisplayStyle.ViewerE:
                        builder.WithButton("<--", "first", ButtonStyle.Secondary);
                        builder.WithButton("<-", "back", ButtonStyle.Primary);
                        builder.WithButton("X", "stop", ButtonStyle.Danger);
                        builder.WithButton("->", "next", ButtonStyle.Primary);
                        builder.WithButton("-->", "last", ButtonStyle.Secondary);
                        builder.WithButton("Prev page", "p", ButtonStyle.Secondary, row:1);
                        builder.WithButton("More", "n", ButtonStyle.Secondary, row: 1);                      
                        break;
                }

                if (folloup)
                {
                    message = await ctx.Interaction.FollowupAsync(paged.GetMessage(), components: builder.Build());
                } else
                {
                    await ctx.Interaction.RespondAsync(paged.GetMessage(), components: builder.Build());
                    message = await ctx.Interaction.GetOriginalResponseAsync();
                }

            } else
            {
                if (folloup)
                {
                    message = await ctx.Interaction.FollowupAsync(paged.GetMessage());
                } else
                {
                    await ctx.Interaction.RespondAsync(paged.GetMessage());
                    message = await ctx.Interaction.GetOriginalResponseAsync();
                }

                return message;
            }

            if (videoMessage != null)
            {
                videoMessage.Add(message.Id, paged);
            }
            else
            {
                throw new ArgumentNullException();
            }

            if (paged.Options.Timeout != TimeSpan.Zero)
            {
                Task _ = Task.Delay(paged.Options.Timeout).ContinueWith(async t =>
                {
                    if (!videoMessage.ContainsKey(message.Id))
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



        // Оптимизировать вот эту хуйню

        public async Task ButtonHandlerImage(SocketMessageComponent component)
        {
            MessageImagePaged? pageImage;
            MessageVideoPaged? pageVideo;


            if (imageMessage.TryGetValue(component.Message.Id, out pageImage))
            {
                if (component.User.Id != pageImage.User.Id)
                {
                    return;
                }

                switch (component.Data.CustomId)
                {
                    case "first":
                        if (pageImage.CurrentPage != 1)
                        {
                            pageImage.CurrentPage = 1;
                            await component.UpdateAsync(x => x.Embed = pageImage.GetEmbed());
                        }
                        break;

                    case "back":
                        if (pageImage.CurrentPage != 1)
                        {
                            pageImage.CurrentPage--;
                            await component.UpdateAsync(x => x.Embed = pageImage.GetEmbed());
                        }
                        break;

                    case "next":
                        if (pageImage.CurrentPage != pageImage.Count)
                        {
                            pageImage.CurrentPage++;
                            await component.UpdateAsync(x => x.Embed = pageImage.GetEmbed());
                        }
                        break;

                    case "last":
                        if (pageImage.CurrentPage != pageImage.Count)
                        {
                            pageImage.CurrentPage = pageImage.Count;
                            await component.UpdateAsync(x => x.Embed = pageImage.GetEmbed());
                        }
                        break;

                    case "stop":
                        switch (pageImage.Options.OnStop)
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

                    case "n":
                        await component.Message.DeleteAsync();
                        imageMessage.Remove(component.Message.Id);
                        break;

                    case "p":
                        break;
                } 
            } else if (videoMessage.TryGetValue(component.Message.Id, out pageVideo)) {

                 if (component.User.Id != pageVideo.User.Id)
                {
                    return;
                }

                switch (component.Data.CustomId)
                {
                    case "first":
                        if (pageVideo.CurrentPage != 1)
                        {
                            pageVideo.CurrentPage = 1;
                            await component.UpdateAsync(x => x.Content = pageVideo.GetMessage());
                        }
                        break;

                    case "back":
                        if (pageVideo.CurrentPage != 1)
                        {
                            pageVideo.CurrentPage--;
                            await component.UpdateAsync(x => x.Content = pageVideo.GetMessage());
                        }
                        break;

                    case "next":
                        if (pageVideo.CurrentPage != pageVideo.Count)
                        {
                            pageVideo.CurrentPage++;
                            await component.UpdateAsync(x => x.Content = pageVideo.GetMessage());
                        }
                        break;

                    case "last":
                        if (pageVideo.CurrentPage != pageVideo.Count)
                        {
                            pageVideo.CurrentPage = pageVideo.Count;
                            await component.UpdateAsync(x => x.Content = pageVideo.GetMessage());
                        }
                        break;

                    case "stop":
                        switch (pageVideo.Options.OnStop)
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

                    case "n":
                        await component.Message.DeleteAsync();
                        imageMessage.Remove(component.Message.Id);
                        break;

                    case "p":
                        break;
                }
                
            }
        }

        
    }
}


