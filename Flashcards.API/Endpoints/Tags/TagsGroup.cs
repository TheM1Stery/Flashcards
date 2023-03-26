using FastEndpoints;

namespace Flashcards.API.Endpoints.Tags;

public sealed class TagsGroup : Group
{
    public TagsGroup()
    {
        Configure("/tags", ep => ep.AllowAnonymous());
    }
}