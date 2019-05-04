using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializedBackLog
{
    [SerializeField] private int currentPosition;
    [SerializeField] private int maxBackLogLength;
    [SerializeField] private List<SerializedBackLogObject> backLogList;


    public SerializedBackLog(BackLog _backLog)
    {
        currentPosition = _backLog.GetCurrentPosition();
        maxBackLogLength = _backLog.GetMaxBackLogLength();

        CreateBackLogList(_backLog.GetBackLogList());
    }


    private void CreateBackLogList(List<BackLogObject> _backLogList)
    {
        backLogList = new List<SerializedBackLogObject>();

        for (int i = 0; i < _backLogList.Count; i++)
        {
            BackLogObject b = _backLogList[i];

            switch (_backLogList[i].GetBackLogType())
            {
                case BackLogType.BackLog_Fields:
                    backLogList.Add(new SerializedBackLogObject(b.GetNumberComponentA(), b.GetNumberComponentB()));
                    break;

                case BackLogType.BackLog_Line:
                    backLogList.Add(new SerializedBackLogObject(b.GetNumberComponentA(), b.GetNumberComponentB(), b.GetLineA(), b.GetLineB()));
                    break;

                case BackLogType.BackLog_MoreNumbers:
                    backLogList.Add(new SerializedBackLogObject(b.GetAddedNumbersList()));
                    break;
            }
        }
    }


    /// <summary>
    /// Returns the max backlog length that is stored.
    /// </summary>
    /// <returns></returns>
    public int GetMaxBackLogLength()
    {
        return maxBackLogLength;
    }


    /// <summary>
    /// Returns the current position in the backlog that is stored.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentPosition()
    {
        return currentPosition;
    }


    /// <summary>
    /// Returns the stored backlog list.
    /// </summary>
    /// <returns></returns>
    public List<SerializedBackLogObject> GetSerializedBackLogList()
    {
        return backLogList;
    }
}
