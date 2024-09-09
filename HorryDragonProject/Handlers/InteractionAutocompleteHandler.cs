using System;
using Discord;
using Discord.Interactions;

namespace HorryDragonProject.Handlers {
    public class E621typeAutocomplete : AutocompleteHandler {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            IEnumerable<AutocompleteResult> results = new[] {
                new AutocompleteResult("gif", "type:gif"),
                new AutocompleteResult("webm", "type:webm"),
                new AutocompleteResult("png", "type:png"),
                new AutocompleteResult("jpg", "type:jpg")
            };
            
            return AutocompletionResult.FromSuccess(results);
        }
    }


    public class ephemeralView : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            IEnumerable<AutocompleteResult> results = new[] {
                new AutocompleteResult("True", true),
                new AutocompleteResult("False", false)
            };

            return AutocompletionResult.FromSuccess(results);
        }
    }
}
