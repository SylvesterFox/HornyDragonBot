using Discord.WebSocket;
using HornyDragonBot.Custom;
using HornyDragonBot.Service;

namespace HornyDragonBot.Handlers.CallbackButtonComponet {
    public static class ImageButtonCallback
    {
        public static async Task CallbackAsync(Dictionary<ulong, MessageImagePaged> imageMessage, SocketMessageComponent component, MessageImagePaged pageImage) {
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
        }
    }
}


