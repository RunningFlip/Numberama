using System.Collections.Generic;
using UnityEngine;


public class BackLogObject
{
    //Type
    private BackLogType backLogType = BackLogType.None;

    //Pair
    private NumberComponent numberComponentA;
    private NumberComponent numberComponentB;

    //Line
    private int lineA = -1;
    private int lineB = -1;

    //Added numbers
    private List<NumberComponent> addedNumbers;


    public BackLogObject(NumberComponent _componentA, NumberComponent _componentB)
    {
        backLogType = BackLogType.BackLog_Fields;

        numberComponentA = _componentA;
        numberComponentB = _componentB;
    }


    public BackLogObject(NumberComponent _componentA, NumberComponent _componentB, int _lineA, int _lineB)
    {
        backLogType = BackLogType.BackLog_Line;

        lineA = _lineA;
        lineB = _lineB;

        numberComponentA = _componentA;
        numberComponentB = _componentB;
    }

    
    public BackLogObject(List<NumberComponent> _addedNumbers)
    {
        backLogType = BackLogType.BackLog_MoreNumbers;

        addedNumbers = _addedNumbers;
    }



    /// <summary>
    /// Undos the strike on both components in the pair.
    /// </summary>
    public void UndoAction()
    {
        //Set line status
        switch (backLogType)
        {
            case BackLogType.BackLog_Fields:
                UndoFields();
                break;

            case BackLogType.BackLog_Line:
                UndoFields();
                UndoLine();
                break;

            case BackLogType.BackLog_MoreNumbers:
                UndoMoreNumbers();
                break;
        }
    }


    //UNDO------------------------------------------------------------------------------
    private void UndoFields()
    {
        if (numberComponentA != null) numberComponentA.UndoStrike();
        if (numberComponentB != null) numberComponentB.UndoStrike();
    }
    private void UndoLine()
    {
        if (lineA > -1) GameplayController.Instance.SetLineStatus(lineA, true);
        if (lineB > -1) GameplayController.Instance.SetLineStatus(lineB, true);
    }
    private void UndoMoreNumbers()
    {
        for (int i = addedNumbers.Count - 1; i > -1; i--)
        {
            NumberComponent num = addedNumbers[i];

            if (num != null)
            {
                GameplayController.Instance.SelectionReset();
                GameplayController.Instance.RemoveNumberFromList(num);

                GameObject.Destroy(num.gameObject);
            }
        }
    }
    //UNDO------------------------------------------------------------------------------


    //Checks if both components are still existing.
    public bool IsStillAlive()
    {
        return numberComponentA != null && numberComponentB != null;
    }


    /// <summary>
    /// Returns the backlogtype.
    /// </summary>
    /// <returns></returns>
    public BackLogType GetBackLogType()
    {
        return backLogType;
    }


    //Getter methods---------------------------------------------------------------
    public NumberComponent GetNumberComponentA() { return numberComponentA; }
    public NumberComponent GetNumberComponentB() { return numberComponentB; }

    public int GetLineA() { return lineA; }
    public int GetLineB() { return lineB; }

    public List<NumberComponent> GetAddedNumbersList() { return addedNumbers; }
    //Getter methods---------------------------------------------------------------
}
