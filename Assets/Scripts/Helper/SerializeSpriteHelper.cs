using UnityEngine;


public static class SerializeSpriteHelper
{
    /// <summary>
    /// Converts a sprite to a SerializedSprite.
    /// </summary>
    /// <param name="_sprite"></param>
    /// <returns></returns>
    public static SerializedSprite SerializeSprite(Sprite _sprite)
    {
        //Position
        int x = Mathf.FloorToInt(_sprite.rect.x);
        int y = Mathf.FloorToInt(_sprite.rect.y);

        //Scale
        int width = Mathf.FloorToInt(_sprite.rect.width);
        int height = Mathf.FloorToInt(_sprite.rect.height);

        //Pixels
        Color[] pixels = _sprite.texture.GetPixels(x, y, width, height);

        //Texture
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();

        //Get raw byte data
        byte[] bytes = texture.GetRawTextureData();

        //Create and return serializedSprite
        return new SerializedSprite(bytes, width, height);
    }


    /// <summary>
    /// Converts a SerializedSprite to a sprite.
    /// </summary>
    /// <param name="_json"></param>
    /// <returns></returns>
    public static Sprite DeserializeSprite(SerializedSprite _jsonSprite)
    {
        //Create texture
        Texture2D tex = new Texture2D(_jsonSprite.width, _jsonSprite.height);

        //Apply byte data
        tex.LoadRawTextureData(_jsonSprite.bytes);
        tex.Apply();

        //Create and return sprite
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, _jsonSprite.width, _jsonSprite.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
