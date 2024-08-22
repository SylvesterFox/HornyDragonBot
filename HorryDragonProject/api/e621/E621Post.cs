using System;
using System.Text.Json.Serialization;

namespace HorryDragonProject.api.e621 {
    public class E621Post
    {
        [JsonPropertyName("posts")]
        public required List<Post> Posts { get; set; }
    }

    public class Post
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("file")]
        public required File File { get; set; }

        [JsonPropertyName("preview")]
        public required Preview Preview { get; set; }

        [JsonPropertyName("sample")]
        public required Sample Sample { get; set; }

        [JsonPropertyName("score")]
        public required Score Score { get; set; }

        [JsonPropertyName("tags")]
        public required Tags Tags { get; set; }

        [JsonPropertyName("locked_tags")]
        public required List<string> LockedTags { get; set; }

        [JsonPropertyName("change_seq")]
        public int ChangeSeq { get; set; }

        [JsonPropertyName("flags")]
        public required Flags Flags { get; set; }

        [JsonPropertyName("rating")]
        public required string Rating { get; set; }

        [JsonPropertyName("fav_count")]
        public int FavCount { get; set; }

        [JsonPropertyName("sources")]
        public required List<string> Sources { get; set; }

        [JsonPropertyName("pools")]
        public required List<int> Pools { get; set; }

        [JsonPropertyName("relationships")]
        public required Relationships Relationships { get; set; }

        [JsonPropertyName("approver_id")]
        public int? ApproverId { get; set; }

        [JsonPropertyName("uploader_id")]
        public int UploaderId { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("comment_count")]
        public int CommentCount { get; set; }

        [JsonPropertyName("is_favorited")]
        public bool IsFavorited { get; set; }

        [JsonPropertyName("has_notes")]
        public bool HasNotes { get; set; }

        [JsonPropertyName("duration")]
        public required object Duration { get; set; }
    }

public class File
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("ext")]
    public required string Ext { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("md5")]
    public required string Md5 { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }
}

public class Preview
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }
}

public class Sample
{
    [JsonPropertyName("has")]
    public bool Has { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("alternates")]
    public required Dictionary<string, string> Alternates { get; set; }
}

public class Score
{
    [JsonPropertyName("up")]
    public int Up { get; set; }

    [JsonPropertyName("down")]
    public int Down { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class Tags
{
    [JsonPropertyName("general")]
    public required List<string> General { get; set; }

    [JsonPropertyName("artist")]
    public required List<string> Artist { get; set; }

    [JsonPropertyName("copyright")]
    public required List<string> Copyright { get; set; }

    [JsonPropertyName("character")]
    public required List<string> Character { get; set; }

    [JsonPropertyName("species")]
    public required List<string> Species { get; set; }

    [JsonPropertyName("invalid")]
    public required List<object> Invalid { get; set; }

    [JsonPropertyName("meta")]
    public required List<string> Meta { get; set; }

    [JsonPropertyName("lore")]
    public required List<object> Lore { get; set; }
}

public class Flags
{
    [JsonPropertyName("pending")]
    public bool Pending { get; set; }

    [JsonPropertyName("flagged")]
    public bool Flagged { get; set; }

    [JsonPropertyName("note_locked")]
    public bool NoteLocked { get; set; }

    [JsonPropertyName("status_locked")]
    public bool StatusLocked { get; set; }

    [JsonPropertyName("rating_locked")]
    public bool RatingLocked { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}

public class Relationships
{
    [JsonPropertyName("parent_id")]
    public required object ParentId { get; set; }

    [JsonPropertyName("has_children")]
    public bool HasChildren { get; set; }

    [JsonPropertyName("has_active_children")]
    public bool HasActiveChildren { get; set; }

    [JsonPropertyName("children")]
    public required List<object> Children { get; set; }
}
}


