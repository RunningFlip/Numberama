using System.Collections.Generic;
using UnityEngine;
using System;


public class NumberField
{
    //Singelton
    public static NumberField Instance;


    //Selected fields
    private NumberComponent fieldSelectionA;
    private NumberComponent fieldSelectionB;

    //Last Hints
    private NumberComponent lastHintA;
    private NumberComponent lastHintB;

    //Flag
    private bool right;
    private bool started;
    private bool isDirty;

    //Spawning   
    private Transform spawnParent;
    private int maxLineLength;
    private GameObject numberFieldPrefab_Left;
    private GameObject numberFieldPrefab_Right;

    //Configs
    private NumberPatternConfig patternConfig;

    //Coutings
    private int strikedPairs = 0;
    private int undoCount = 0;
    private int numberFieldsCount = 1;

    //Striked lines
    private List<int> strikedLines;

    //2D-Array
    private int currentX = 0;
    private int currentY = 0;
    public List<List<NumberComponent>> numberFieldComponents;

    //Backlog
    private BackLog backLog;


    public NumberField(Transform _spawnParent, NumberPatternConfig _patternConfig, Savegame _savegame = null)
    {
        //Singelton init
        Instance = this;

        //Config
        patternConfig = _patternConfig;

        //Spawning
        spawnParent = _spawnParent;
        maxLineLength           = ParameterManager.Instance.GameParameter.maxLineLength;
        numberFieldPrefab_Left  = ParameterManager.Instance.GameParameter.numberFieldPrefab_Left;
        numberFieldPrefab_Right = ParameterManager.Instance.GameParameter.numberFieldPrefab_Right;

        //Lists
        numberFieldComponents = new List<List<NumberComponent>>();
        numberFieldComponents.Add(new List<NumberComponent>());
        strikedLines = new List<int>();

        //Savegame
        if (_savegame != null)
        {
            strikedPairs = _savegame.pairsFound;
            undoCount = _savegame.undoCount;

            LoadGameBySavgame(_savegame);
            backLog = new BackLog(_savegame.serializedBackLog);
        }
        else
        {
            //Creates a new game
            AddNumberFields(patternConfig.numberStartPattern);
            backLog = new BackLog();
        }

        //UI
        GameplayController.Instance.uiController.SetStrikedPairsCount(strikedPairs);
        GameplayController.Instance.uiController.SetUndoCount(undoCount);
    }


    //GETTER---------------------------------------------------------------
    public List<int> GetStrikedLines()  { return strikedLines;      }
    public BackLog GetBackLog()         { return backLog;           }

    public int GetStrikedPairs()        { return strikedPairs;      }
    public int GetUndoCount()           { return undoCount;         }

    public bool IsDirty()               { return isDirty;           }

    public NumberComponent GetNumberComponent(int _x, int _y) { return numberFieldComponents[_y][_x]; }
    //GETTER---------------------------------------------------------------


    /// <summary>
    /// Spawns more numberfields and adds them to the game field.
    /// </summary>
    /// <returns></returns>
    private NumberComponent SpawnNextNumberField(int _x, int _y)
    {
        //Spawn
        GameObject prefab = right
            ? numberFieldPrefab_Left
            : numberFieldPrefab_Right;
        right = !right;

        GameObject numField = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, spawnParent);
        numField.name += " (" + _x + "/" + _y + ")";
        numField.SetActive(false);
        numberFieldsCount++;

        return numField.GetComponent<NumberComponent>();
    }


    /// <summary>
    /// Loads a game by a given savegame
    /// </summary>
    public void LoadGameBySavgame(Savegame _savegame)
    {
        if (_savegame != null)
        {
            //Counting
            strikedPairs = _savegame.pairsFound;
            undoCount = _savegame.undoCount;

            //UI
            GameplayController.Instance.uiController.SetStrikedPairsCount(strikedPairs);
            GameplayController.Instance.uiController.SetUndoCount(undoCount);

            //Flag
            bool wasLast = false;

            //Create fields
            for (int i = 0; i < _savegame.serializedNumberFields.Count; i++)
            {
                //Begin from the beginning of the next line
                if (wasLast)
                {
                    wasLast = false;
                    numberFieldComponents.Add(new List<NumberComponent>());
                    currentX = 0;
                    currentY++;
                }
                currentX++;

                //Spawn fields
                SerializableNumberField number = _savegame.serializedNumberFields[i];
                NumberComponent numberComponent = SpawnNextNumberField(number.x, number.y);
                numberComponent.FieldSetup(number.number, number.x, number.y, number.id);

                //Store in list
                numberFieldComponents[currentY].Add(numberComponent);

                //Strike
                if (number.strike)
                {
                    numberComponent.FieldStrike();
                }

                //End of the line got reached
                if (_savegame.serializedNumberFields[i].x == maxLineLength - 1)
                {
                    wasLast = true;
                }
            }

            //Check striked lines
            for (int i = 0; i < _savegame.strikedLines.Count; i++)
            {
                SetLineStatus(_savegame.strikedLines[i], false);
            }
        }
    }


    /// <summary>
    /// Spawns the left fields as an extension.
    /// </summary>
    public void TrySpawnMoreNumbers()
    {
        string leftNumbers = "";

        //Creates a string out of the left numbers on the gamefield
        for (int i = 0; i < numberFieldComponents.Count; i++)
        {
            for (int j = 0; j < numberFieldComponents[i].Count; j++)
            {
                NumberComponent component = numberFieldComponents[i][j];
                if (!component.strike)
                {
                    leftNumbers += component.number;
                }
            }
        }
        //Adds new numberFields with the given numbers-string that is left
        AddNumberFields(leftNumbers);

        if (leftNumbers != "")
        {
            GameplayController.Instance.addNumbersAudioClipObject.PlaySound();
        }
    }


    /// <summary>
    /// Initializes all numberfields.
    /// </summary>
    private void AddNumberFields(string _numbers)
    {
        //Increments
        int currentLetter = 0;

        //BackLogList
        List<NumberComponent> bgList = new List<NumberComponent>();

        //Update currentX
        currentX = numberFieldComponents[currentY].Count;

        //Iterate number lines
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (currentX == maxLineLength)
            {
                //Init Horizontal List
                numberFieldComponents.Add(new List<NumberComponent>());
                currentX = 0;
                currentY++;
            }

            //Horizontal Entry
            NumberComponent numberComponent = SpawnNextNumberField(currentX, currentY);

            //Add to list
            numberFieldComponents[currentY].Add(numberComponent);
            bgList.Add(numberComponent);

            //Field setup
            numberComponent.FieldSetup((int)Char.GetNumericValue(_numbers[currentLetter]), currentX, currentY);

            //Increment
            currentLetter++;
            currentX++;
        }

        if (!started)
        {
            started = true;
        }
        else
        {
            isDirty = true;

            //BackLog
            backLog.AddToBackLog(bgList);
        }
    }


    /// <summary>
    /// Removes an element from the numberComponents list.
    /// </summary>
    /// <param name="_numberComponent"></param>
    /// <param name="_y"></param>
    public void RemoveNumberFromList(NumberComponent _numberComponent)
    {
        if (_numberComponent.positionX == 0)
        {
            currentY -= 1;
        }

        numberFieldComponents[_numberComponent.positionY].Remove(_numberComponent);

        if (_numberComponent.positionY == 0)
        {
            numberFieldComponents.RemoveAt(_numberComponent.positionY);
        }
    }


    /// <summary>
    /// Adds a field to the selection.
    /// </summary>
    /// <param name="_numberComponent"></param>
    /// <param name="_selected"></param>
    public void FieldSelected(NumberComponent _numberComponent, bool _selected)
    {
        if (_selected)
        {
            if (fieldSelectionA == null)
            {
                fieldSelectionA = _numberComponent;
            }
            else if (fieldSelectionB == null)
            {
                fieldSelectionB = _numberComponent;
            }

            //Selection audio
            GameplayController.Instance.selectionAudioClipObject.PlaySound();
        }
        else
        {
            if (fieldSelectionA == _numberComponent)
            {
                fieldSelectionA = null;
            }
            else if (fieldSelectionB == _numberComponent)
            {
                fieldSelectionB = null;
            }

            //Deselection audio
            GameplayController.Instance.deselectionAudioClipObject.PlaySound();
        }

        if (fieldSelectionA != null && fieldSelectionB != null)
        {
            bool pairFound = CheckPair(fieldSelectionA, fieldSelectionB);

            if (pairFound)
            {
                SelectionStrike();
            }
            else
            {
                FieldSelectionReset();
            }
        }
    }


    /// <summary>
    /// Resets the selection and the fields.
    /// </summary>
    public void FieldSelectionReset()
    {
        if (fieldSelectionA != null) fieldSelectionA.FieldSelectionReset();
        if (fieldSelectionB != null) fieldSelectionB.FieldSelectionReset();
    }


    /// <summary>
    /// Checks if the pairs a valid and striks if they are valid them.
    /// </summary>
    private bool CheckPair(NumberComponent _selectionA, NumberComponent _selectionB)
    {
        NumberComponent selectionA = _selectionA;
        NumberComponent selectionB = _selectionB;

        if (selectionA == selectionB) return false;

        if (selectionA.number == selectionB.number
            || selectionA.number + selectionB.number == 10)
        {
            //VERTICAL-CHECK
            //Are both selected numberComponents on the same x-Position?
            if (selectionA.positionX == selectionB.positionX)
            {
                //Sort by position
                if (selectionA.positionY > selectionB.positionY)
                {
                    NumberComponent backlog = selectionA;
                    selectionA = selectionB;
                    selectionB = backlog;
                }

                //Check
                for (int i = selectionA.positionY + 1; i <= selectionB.positionY; i++)
                {
                    NumberComponent component = numberFieldComponents[i][selectionA.positionX];

                    if (!component.strike && (component != selectionA && component != selectionB))
                    {
                        return false;
                    }
                }
                return true;
            }
            //VERTICAL-CHECK
            //Is entered, if the numberComponents are in the same or different row, but don't have the same x-position
            else
            {
                if (selectionA.positionY != selectionB.positionY)
                {
                    //Sort by position
                    if (selectionA.positionY > selectionB.positionY)
                    {
                        NumberComponent backlog = selectionA;
                        selectionA = selectionB;
                        selectionB = backlog;
                    }
                }
                else
                {
                    //Sort by position
                    if (selectionA.positionX > selectionB.positionX)
                    {
                        NumberComponent backlog = selectionA;
                        selectionA = selectionB;
                        selectionB = backlog;
                    }
                }

                NumberComponent component = null;
                int numX = selectionA.positionX; //Current x-position of the component-element
                int numY = selectionA.positionY; //Current y-position of the component-element

                while (component != selectionB)
                {
                    //Checks if a new line gots reached
                    if (numX == maxLineLength)
                    {
                        numX = 0;
                        numY++;
                    }

                    //stores the new numberComponent
                    component = numberFieldComponents[numY][numX];

                    //Checks if an invalid element got reached
                    if (!component.strike && (component != selectionA && component != selectionB))
                    {
                        return false;
                    }
                    //Increments to the next element in line
                    numX++;
                }

                //Is called if loop does not reach the "return".
                return true;
            }
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// Strikes both selections.
    /// </summary>
    private void SelectionStrike()
    {
        //Dirty flag
        isDirty = true;

        NumberComponent numA = fieldSelectionA;
        NumberComponent numB = fieldSelectionB;

        int lastYA = fieldSelectionA.positionY;
        int lastYB = fieldSelectionB.positionY;

        //Checks if at least one of the numbers is a hint and resets the hint if neccessary
        if (CheckIsHint(fieldSelectionA, fieldSelectionB))
        {
            ResetHint();
        } 

        //Strikes the selected fields
        fieldSelectionA.FieldStrike();
        fieldSelectionB.FieldStrike();

        //Strike field audio
        GameplayController.Instance.strikeFieldAudioClipObject.PlaySound();

        //Checks if a row is allowed to get striked completly
        bool strikeA = false;
        bool strikeB = false;

        CheckStrikedLine(lastYA, lastYB, out strikeA, out strikeB);

        if (strikeA || strikeB)
        {
            if (strikeA && !strikeB)
            {
                backLog.AddToBackLog(numA, numB, lastYA);
                SetLineStatus(lastYA, false);
            }
            else if (!strikeA && strikeB)
            {
                backLog.AddToBackLog(numA, numB, lastYB);
                SetLineStatus(lastYB, false);
            }
            else if (strikeA && strikeB)
            {
                backLog.AddToBackLog(numA, numB, lastYA, lastYB);
                SetLineStatus(lastYA, false);
                SetLineStatus(lastYB, false);
            }
        }
        else
        {
            backLog.AddToBackLog(numA, numB);
        }

        //Update Strikes pairs in the UI
        IncreasePairByInt(1);
    }


    /// <summary>
    /// Call line checks for both striked numberfields.
    /// </summary>
    private void CheckStrikedLine(int _yPositionA, int _yPositionB, out bool _strikeA, out bool _strikeB)
    {
        _strikeA = LineIsStriked(_yPositionA);
        _strikeB = false;

        if (_yPositionA != _yPositionB)
        {
            _strikeB = LineIsStriked(_yPositionB);
        }
    }


    /// <summary>
    /// Checks if a specific line is striked.
    /// </summary>
    /// <param name="_y"></param>
    /// <returns></returns>
    private bool LineIsStriked(int _y)
    {
        bool strikedLineFound = true;

        if (numberFieldComponents[_y].Count < maxLineLength)
        {
            strikedLineFound = false;
        }
        else
        {
            for (int i = 0; i < maxLineLength; i++)
            {
                if (!numberFieldComponents[_y][i].strike)
                {
                    //Line is not completly striked
                    strikedLineFound = false;
                    break;
                }
            }
        }
        return strikedLineFound;
    }


    /// <summary>
    /// Activates or deactivates the line.
    /// </summary>
    /// <param name="_line"></param>
    /// <param name="_status"></param>
    public void SetLineStatus(int _line, bool _status)
    {
        List<NumberComponent> components = numberFieldComponents[_line];

        if (!_status)
        {
            if (!strikedLines.Contains(_line))
            {
                strikedLines.Add(_line);
            }
        }
        else
        {
            if (strikedLines.Contains(_line))
            {
                strikedLines.Remove(_line);
            }
        }

        for (int i = 0; i < components.Count; i++)
        {
            components[i].gameObject.SetActive(_status);
        }

        //Strike line audio
        GameplayController.Instance.strikeLineAudioClipObject.PlaySound();
    }


    /// <summary>
    /// Undos the last action in the backlog.
    /// </summary>
    public void UndoLastAction()
    {
        if (backLog.IsEmpty()) ResetHint();

        backLog.UndoLastAction();
    }


    /// <summary>
    /// Shows the next possible combination and highlights both fields.
    /// </summary>
    public void ShowNextHint()
    {
        bool pairFound = false;

        int x = 0;
        int y = 0;
        int maxY = numberFieldComponents.Count - 1;

        NumberComponent numA = null;
        NumberComponent numB = null;

        while (!pairFound && y < maxY)
        {
            numA = numberFieldComponents[y][x];

            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < numberFieldComponents[i].Count; j++)
                {
                    numB = numberFieldComponents[i][j];

                    if (!numA.strike && !numB.strike)
                    {
                        pairFound = CheckPair(numA, numB);
                        if (pairFound) break;
                    }
                }

                if (pairFound) break;
            }
        
            if (x < maxLineLength - 1)
            {
                x++;
            }
            else if (y < maxY - 1)
            {
                x = 0;
                y++;
            }
            else
            {
                break;
            }
        }

        if (pairFound)
        {
            ResetHint();

            lastHintA = numA;
            lastHintB = numB;

            numA.SetHintStatus(true);
            numB.SetHintStatus(true);

            //Hint audio
            GameplayController.Instance.hintAudioClipObject.PlaySound();
        }
    }


    /// <summary>
    /// Resets the hints.
    /// </summary>
    public void ResetHint()
    {
        if (lastHintA != null) lastHintA.SetHintStatus(false);
        if (lastHintB != null) lastHintB.SetHintStatus(false);
    }


    /// <summary>
    /// Returns true if one of the given numbers is a current hint.
    /// </summary>
    /// <param name="_numberComponent"></param>
    /// <returns></returns>
    public bool CheckIsHint(NumberComponent _numberComponentA, NumberComponent _numberComponentB)
    {
        return _numberComponentA == lastHintA || _numberComponentA == lastHintB 
            || _numberComponentB == lastHintA || _numberComponentB == lastHintB;
    }


    /// <summary>
    /// Increase the pair counter by the added value.
    /// </summary>
    /// <param name="_add"></param>
    public void IncreasePairByInt(int _add)
    {
        strikedPairs += _add;
        GameplayController.Instance.uiController.SetStrikedPairsCount(strikedPairs);
    }


    /// <summary>
    /// Increase the undo counter by the added value.
    /// </summary>
    /// <param name="_add"></param>
    public void IncreaseUndoByInt(int _add)
    {
        undoCount += _add;
        GameplayController.Instance.uiController.SetUndoCount(undoCount);
    }
}
