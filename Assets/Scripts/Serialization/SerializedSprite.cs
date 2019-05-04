using System;


[Serializable]
public class SerializedSprite
{
    public byte[] bytes;

    public int width;
    public int height;


    public SerializedSprite(byte[] _bytes, int _width, int _height)
    {
        bytes = _bytes;
        width = _width;
        height = _height;
    }
}
