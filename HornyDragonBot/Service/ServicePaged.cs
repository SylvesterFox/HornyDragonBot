using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HornyDragonBot.Custom;
using HornyDragonBot.Handlers.CallbackButtonComponet;


namespace HornyDragonBot.Service
{

    public enum StopAction {
        Clear,
        DeleteMessage
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
                var builder = TemplateButton.ButtonPagedImage(paged);

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
                
                var builder = TemplateButton.ButtonPagedVideo(paged);

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



        // Писать код под балтикой 9 это пиздец

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

                await ImageButtonCallback.CallbackAsync(imageMessage, component, pageImage);

                
            } else if (videoMessage.TryGetValue(component.Message.Id, out pageVideo)) {

                 if (component.User.Id != pageVideo.User.Id)
                {
                    return;
                }

                await VideoButtonCallback.CallbackAsync(videoMessage, component, pageVideo);
                
            }
        }

        
    }
}


