using System;


public struct JsonDateTime
{
    public long value;


    public static implicit operator DateTime(JsonDateTime _jsonDateTime)
    {
        return DateTime.FromFileTimeUtc(_jsonDateTime.value);
    }


    public static implicit operator JsonDateTime(DateTime _dt)
    {
        JsonDateTime jsonDateTime = new JsonDateTime();
        jsonDateTime.value = _dt.ToFileTimeUtc();

        return jsonDateTime;
    }
}
