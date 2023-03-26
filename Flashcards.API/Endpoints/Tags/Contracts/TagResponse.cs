using Flashcards.API.Domain.Tags;

namespace Flashcards.API.Endpoints.Tags.Contracts;


public class TagResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public static class Mapper
{
    public static TagResponse ToResponse(this Tag tag)
    {
        return new TagResponse()
        {
            Id = (Guid)tag.Id,
            Name = (string)tag.Name
        };
    }  
}