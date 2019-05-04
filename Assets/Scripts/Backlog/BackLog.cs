using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public class BackLog
{
    public int currentPosition = -1;
    public int maxBackLogLength;
    public List<BackLogObject> backLogList;


    public BackLog(SerializedBackLog _serializedBackLog = null)
    {
        backLogList = new List<BackLogObject>();

        if (_serializedBackLog != null)
        {
            currentPosition = _serializedBackLog.GetCurrentPosition();
            maxBackLogLength = _serializedBackLog.GetMaxBackLogLength();
            ReconnectBacklog(_serializedBackLog.GetSerializedBackLogList());
        }
        else
        {
            maxBackLogLength = GameplayController.Instance.GameParameter.maxBackTracking;
        }
    }


    /// <summary>
    /// Returns the max backlog length.
    /// </summary>
    /// <returns></returns>
    public int GetMaxBackLogLength()
    {
        return maxBackLogLength;
    }

    /// <summary>
    /// Returns the current position.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentPosition()
    {
        return currentPosition;
    }


    /// <summary>
    /// Returns the backlog list.
    /// </summary>
    /// <returns></returns>
    public List<BackLogObject> GetBackLogList()
    {
        return backLogList;
    }


    /// <summary>
    /// Reconnects the backup with the new backLog.
    /// </summary>
    /// <param name="_serializedBackup"></param>
    private void ReconnectBacklog(List<SerializedBackLogObject> _serializedBackup)
    {
        for (int i = 0; i < _serializedBackup.Count; i++)
        {
            SerializedBackLogObject b = _serializedBackup[i];

            switch (b.backLogType)
            {
                case BackLogType.BackLog_Fields:
                    backLogList.Add(new BackLogObject(
                        GameplayController.Instance.GetNumberComponent(b.numA.x, b.numA.y),
                        GameplayController.Instance.GetNumberComponent(b.numB.x, b.numB.y)));
                    break;

                case BackLogType.BackLog_Line:
                    backLogList.Add(new BackLogObject(
                        GameplayController.Instance.GetNumberComponent(b.numA.x, b.numA.y),
                        GameplayController.Instance.GetNumberComponent(b.numB.x, b.numB.y),
                        b.lineA,
                        b.lineB));
                    break;

                case BackLogType.BackLog_MoreNumbers:                  
                    backLogList.Add(new BackLogObject(CreateAddedNumbersListFromBackup(b.addedNumbersList)));
                    break;
            }
        }
    }


    /// <summary>
    /// Creates a list from numbercomponents out of numberelements.
    /// </summary>
    /// <param name="_addedNumbersList"></param>
    /// <returns></returns>
    private List<NumberComponent> CreateAddedNumbersListFromBackup(List<SerializableNumberField> _addedNumbersList)
    {
        List<NumberComponent> addedNumbers = new List<NumberComponent>();

        for (int i = 0; i < _addedNumbersList.Count; i++)
        {         
            SerializableNumberField n = _addedNumbersList[i];
            Debug.Log(n.x + " / " + n.y);
            addedNumbers.Add(GameplayController.Instance.GetNumberComponent(n.x, n.y));
        }
        return addedNumbers;
    }


    /// <summary>
    /// Adds added numbers to the backlog.
    /// </summary>
    /// <param name="_addedNumbers"></param>
    public void AddToBackLog(List<NumberComponent> _addedNumbers)
    {
        BackLogObject backLogObject = new BackLogObject(_addedNumbers);

        if (backLogList.Count < maxBackLogLength)
        {
            backLogList.Add(backLogObject);
        }
        else
        {
            backLogList.RemoveAt(0);
            backLogList.Add(backLogObject);
        }

        //Position in backlog
        currentPosition++;

        //Clamp
        ClampBacklog();
    }


    /// <summary>
    /// Adds numbercomponents and/or striked lines from the backlog.
    /// </summary>
    /// <param name="_numA"></param>
    /// <param name="_numB"></param>
    /// <param name="_lineA"></param>
    /// <param name="_lineB"></param>
    public void AddToBackLog(NumberComponent _numA, NumberComponent _numB, int _lineA = -1, int _lineB = -1)
    {
        BackLogObject backLogObject = null;

        if (_lineA > -1 || _lineB > -1) backLogObject = new BackLogObject(_numA, _numB, _lineA, _lineB);
        else backLogObject = new BackLogObject(_numA, _numB);

        if (backLogList.Count < maxBackLogLength)
        {
            backLogList.Add(backLogObject);
        }
        else
        {
            backLogList.RemoveAt(0);
            backLogList.Add(backLogObject);
        }

        //Position in backlog
        currentPosition++;

        //Clamp
        ClampBacklog();
    }


    /// <summary>
    /// Clamps the backlog.
    /// </summary>
    private void ClampBacklog()
    {
        int count = backLogList.Count;

        if (currentPosition < count - 1)
        {
            int elementsToClamp = (count - 1) - currentPosition;

            if (elementsToClamp > 0)
            {
                backLogList.RemoveRange(currentPosition, elementsToClamp);
                currentPosition = backLogList.Count - 1;
            }
        }
    }


    /// <summary>
    /// Undos the last action that is in backlog.
    /// </summary>
    public void UndoLastAction()
    {
        if (backLogList.Count > 0 && currentPosition > -1)
        {
            //BackLogObject backLogObject = backLogList[count - 1];
            BackLogObject backLogObject = backLogList[currentPosition];

            //Undo
            backLogObject.UndoAction();

            //Update strikes pairs in the UI
            if (backLogObject.GetBackLogType() != BackLogType.BackLog_MoreNumbers)
            {
                GameplayController.Instance.IncreasePairByInt(-1);
            }

            //Updates the undo count in the UI
            GameplayController.Instance.IncreaseUndoByInt(1);

            if (currentPosition > -1)
            {
                currentPosition--;
            }
        }
    }


    /// <summary>
    /// Redos the last action that is in backlog..
    /// </summary>
    public void RedoLastAction()
    {
        if (currentPosition < backLogList.Count - 1)
        {
            currentPosition++;

            BackLogObject backLogObject = backLogList[currentPosition];

            //Redo
            backLogObject.RedoAction();

            //Update strikes pairs in the UI
            if (backLogObject.GetBackLogType() != BackLogType.BackLog_MoreNumbers)
            {
                GameplayController.Instance.IncreasePairByInt(1);
            }

            //Updates the undo count in the UI
            GameplayController.Instance.IncreaseUndoByInt(-1);
        }
    }
}
