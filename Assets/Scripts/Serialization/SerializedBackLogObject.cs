using System;
using System.Collections.Generic;


[Serializable]
public class SerializedBackLogObject
{
    public BackLogType backLogType = BackLogType.None;

    public SerializableNumberField numA;
    public SerializableNumberField numB;

    public int lineA = -1;
    public int lineB = -1;

    public List<SerializableNumberField> addedNumbersList;


    public SerializedBackLogObject(NumberComponent _componentA, NumberComponent _componentB)
    {
        backLogType = BackLogType.BackLog_Fields;

        numA = new SerializableNumberField(_componentA);
        numB = new SerializableNumberField(_componentB);
    }


    public SerializedBackLogObject(NumberComponent _componentA, NumberComponent _componentB, int _lineA, int _lineB)
    {
        backLogType = BackLogType.BackLog_Line;

        numA = new SerializableNumberField(_componentA);
        numB = new SerializableNumberField(_componentB);

        lineA = _lineA;
        lineB = _lineB;
    }


    public SerializedBackLogObject(List<NumberComponent> _addedNumbers)
    {
        backLogType = BackLogType.BackLog_MoreNumbers;

        addedNumbersList = new List<SerializableNumberField>();

        for (int i = 0; i < _addedNumbers.Count; i++)
        {
            addedNumbersList.Add(new SerializableNumberField(_addedNumbers[i]));
        }
    }
}
