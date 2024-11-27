using Discord.WebSocket;
using HornyDragonBot.Custom;
using HornyDragonBot.Service;

namespace HornyDragonBot.Handlers.CallbackButtonComponet {

    public static class VideoButtonCallback
    {
        public static async Task CallbackAsync(Dictionary<ulong, MessageVideoPaged> videoMessage, SocketMessageComponent component, MessageVideoPaged pageVideo) {
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

                        videoMessage.Remove(component.Message.Id);
                        break;

                    case "select":
                        await component.UpdateAsync(x => x.Components = null);
                        videoMessage.Remove(component.Message.Id);
                        break;

                    case "n":
                        await component.Message.DeleteAsync();
                        videoMessage.Remove(component.Message.Id);
                        break;

                    case "p":
                        break;
                }
        }

    }

}


