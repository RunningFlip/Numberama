using System.Collections.Generic;


public static class NumberHelper
{
    /// <summary>
    /// Creates a list from numbercomponents out of numberelements.
    /// </summary>
    /// <param name="_addedNumbersList"></param>
    /// <returns></returns>
    public static List<NumberComponent> CreateNumbersListFromSer(ref List<SerializableNumberField> _addedNumbersList)
    {
        List<NumberComponent> addedNumbers = new List<NumberComponent>();

        for (int i = 0; i < _addedNumbersList.Count; i++)
        {
            SerializableNumberField n = _addedNumbersList[i];
            addedNumbers.Add(NumberField.Instance.GetNumberComponent(n.x, n.y));
        }
        return addedNumbers;
    }


    /// <summary>
    /// Creates a list of serializable numberfields of a given numberfield.
    /// </summary>
    /// <param name="_numberField"></param>
    /// <returns></returns>
    public static List<SerializableNumberField> CreateSerNumberFieldList(ref NumberField _numberField)
    {
        //Create number list
        List<SerializableNumberField> numberFields = new List<SerializableNumberField>();

        for (int i = 0; i < _numberField.numberFieldComponents.Count; i++)
        {
            for (int j = 0; j < _numberField.numberFieldComponents[i].Count; j++)
            {
                numberFields.Add(new SerializableNumberField(_numberField.numberFieldComponents[i][j]));
            }
        }
        return numberFields;
    }
}
