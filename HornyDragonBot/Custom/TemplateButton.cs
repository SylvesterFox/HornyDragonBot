using Discord;

namespace HornyDragonBot.Custom {
    public enum DisplayStyle {
        Full,
        Minimal,
        Selector,
        ViewerE
    }

    public abstract class TemplateButton
    {

        public static ComponentBuilder ButtonPagedImage(MessageImagePaged paged) {
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

            return builder;
        }

        public static ComponentBuilder ButtonPagedVideo(MessageVideoPaged paged) {
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

            return builder;
        }
    }

}

