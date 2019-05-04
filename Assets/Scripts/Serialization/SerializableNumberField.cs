using System;


[Serializable]
public class SerializableNumberField
{
    public int id;

    public bool strike;
    public int number = -1;

    public int x = -1;
    public int y = -1;


    public SerializableNumberField(NumberComponent _numberComponent)
    {
        id = _numberComponent.id;

        strike = _numberComponent.strike;
        number = _numberComponent.number;

        x = _numberComponent.positionX;
        y = _numberComponent.positionY;
    }
}
