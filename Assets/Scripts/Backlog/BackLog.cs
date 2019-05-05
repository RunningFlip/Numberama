using System.Collections.Generic;


public class BackLog
{
    private int maxBackLogLength;
    private List<BackLogObject> backLogList;

    private NumberField myNumberField;


    public BackLog(SerializedBackLog _serializedBackLog = null)
    {
        backLogList = new List<BackLogObject>();

        if (_serializedBackLog != null)
        {
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
                        NumberField.Instance.GetNumberComponent(b.numA.x, b.numA.y),
                        NumberField.Instance.GetNumberComponent(b.numB.x, b.numB.y)));
                    break;

                case BackLogType.BackLog_Line:
                    backLogList.Add(new BackLogObject(
                        NumberField.Instance.GetNumberComponent(b.numA.x, b.numA.y),
                        NumberField.Instance.GetNumberComponent(b.numB.x, b.numB.y),
                        b.lineA,
                        b.lineB));
                    break;

                case BackLogType.BackLog_MoreNumbers:                  
                    backLogList.Add(new BackLogObject(NumberHelper.CreateNumbersListFromSer(ref b.addedNumbersList)));
                    break;
            }
        }
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
    }


    /// <summary>
    /// Undos the last action that is in backlog.
    /// </summary>
    public void UndoLastAction()
    {
        int count = backLogList.Count;

        if (count > 0)
        {
            BackLogObject backLogObject = backLogList[count - 1];

            //Undo
            backLogObject.UndoAction();
            backLogList.RemoveAt(count - 1);

            //Update strikes pairs in the UI
            if (backLogObject.GetBackLogType() != BackLogType.BackLog_MoreNumbers)
            {
                NumberField.Instance.IncreasePairByInt(-1);
            }

            //Updates the undo count in the UI
            NumberField.Instance.IncreaseUndoByInt(1);
        }
    }
}
