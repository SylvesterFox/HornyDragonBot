using Discord;
using HorryDragonProject.api.e621;
using HorryDragonProject.Service;

namespace HorryDragonProject.Custom {

    public static class TemplateMessage {
        public static string SendVideoTemplate(Post post, string tag)
        {
            string message = $"> ## E621 video view message\n> ## Search tags:\n> ```{tag}```\n";
            message += $"> [[Link source]]({post.File.Url}) | [Page e621](<https://e621.net/posts/{post.Id}>)";
            return message;      
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
                string msg = message + $"\n\n> ```Page: {i++}/{messagesVideo.Count()} -- Post id:{post[n++].Id}```";
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

}


