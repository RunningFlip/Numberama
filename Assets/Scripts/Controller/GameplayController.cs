using System;
using System.Collections.Generic;
using UnityEngine;


public class GameplayController : MonoBehaviour
{
    //Singelton
    public static GameplayController Instance;


    [Header("Game Parameter")]
    public GameParameter GameParameter;

    [Header("UI Controller")]
    public UIController uiController;

    [Header("Field Spawn")]
    public Transform spawnParent;
    public GameObject numberFieldPrefab_Left;
    public GameObject numberFieldPrefab_Right;

    [Header("Field Selection")]
    public NumberComponent fieldSelectionA;
    public NumberComponent fieldSelectionB;


    //Flag
    private bool right;
    private bool started;
    private bool isDirty;

    //Time
    public float passedTime;

    //Configs
    private NumberPatternConfig patternConfig;

    //UI Values
    private int strikedPairs = 0;
    private int undoCount = 0; 

    //Numberfield Spawn Values
    private int numberFieldsCount = 1;

    //Backlog
    private BackLog backLog;

    //Striked lines
    private List<int> strikedLines;

    //2D-Array
    private int currentX = 0;
    private int currentY = 0;
    [HideInInspector]
    public List<List<NumberComponent>> numberFieldComponents;


    private void Awake()
    {
        //Singelton init
        if (!Instance)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        //Lists
        numberFieldComponents = new List<List<NumberComponent>>();
        numberFieldComponents.Add(new List<NumberComponent>());
        strikedLines = new List<int>();

        //UI
        uiController.SetStrikedPairsCount(strikedPairs);
        uiController.SetUndoCount(undoCount);

        //PlayerPrefs
        int gameMode = PlayerPrefs.GetInt("SavegameMode");
        int savegameTimestamp = PlayerPrefs.GetInt("SavegameTimestamp");

        //Game Parameter
        if (gameMode == 0)      patternConfig = GameParameter.defaultNumberPatternConfig;
        else if (gameMode == 1) patternConfig = GameParameter.hardNumberPatternConfig;

        //Savegames
        if (savegameTimestamp != -1)
        {   
            //Loads a savegame
            Savegame savegame = DataHelper.LoadProgress(savegameTimestamp);
            LoadGameBySavgame(savegame);
            backLog = new BackLog(savegame.serializedBackLog);
        }
        else
        {
            //Creates a new game
            AddNumberFields(patternConfig.numberStartPattern);
            backLog = new BackLog();
        }
    }


    private void LateUpdate()
    {
        passedTime += Time.deltaTime;
    }


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

        GameObject numField = Instantiate(prefab, Vector3.zero, Quaternion.identity, spawnParent);
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
            passedTime = _savegame.passedTime;

            //UI
            uiController.SetStrikedPairsCount(strikedPairs);
            uiController.SetUndoCount(undoCount);

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
                if (_savegame.serializedNumberFields[i].x == GameParameter.maxLineLength - 1)
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
            if (currentX == GameParameter.maxLineLength)
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
        }

        if (fieldSelectionA != null && fieldSelectionB != null) CheckPair();
    }


    /// <summary>
    /// Checks if the pairs a valid and striks if they are valid them.
    /// </summary>
    private void CheckPair()
    {
        if (fieldSelectionA.number == fieldSelectionB.number 
            || fieldSelectionA.number + fieldSelectionB.number == 10)
        {
            //VERTICAL-CHECK
            //Are both selected numberComponents on the same x-Position?
            if (fieldSelectionA.positionX == fieldSelectionB.positionX)             
            {
                //Sort by position
                if (fieldSelectionA.positionY > fieldSelectionB.positionY)
                {
                    NumberComponent backlog = fieldSelectionA;
                    fieldSelectionA = fieldSelectionB;
                    fieldSelectionB = backlog;
                }

                //Check
                for (int i = fieldSelectionA.positionY + 1; i <= fieldSelectionB.positionY; i++)
                {
                    NumberComponent component = numberFieldComponents[i][fieldSelectionA.positionX];

                    if (!component.strike && (component != fieldSelectionA && component != fieldSelectionB))
                    {
                        SelectionReset();
                        return;
                    }
                    else
                    {
                        SelectionStrike();
                        return;
                    }
                }
            }
            //VERTICAL-CHECK
            //Is entered, if the numberComponents are in the same or different row, but don't have the same x-position
            else
            {
                if (fieldSelectionA.positionY != fieldSelectionB.positionY)
                {
                    //Sort by position
                    if (fieldSelectionA.positionY > fieldSelectionB.positionY)
                    {
                        NumberComponent backlog = fieldSelectionA;
                        fieldSelectionA = fieldSelectionB;
                        fieldSelectionB = backlog;
                    }
                }
                else
                {
                    //Sort by position
                    if (fieldSelectionA.positionX > fieldSelectionB.positionX)
                    {
                        NumberComponent backlog = fieldSelectionA;
                        fieldSelectionA = fieldSelectionB;
                        fieldSelectionB = backlog;
                    }
                }

                NumberComponent component = null;
                int numX = fieldSelectionA.positionX; //Current x-position of the component-element
                int numY = fieldSelectionA.positionY; //Current y-position of the component-element

                while (component != fieldSelectionB)
                {
                    //Checks if a new line gots reached
                    if (numX == GameParameter.maxLineLength)
                    {
                        numX = 0;
                        numY++;
                    }

                    //stores the new numberComponent
                    component = numberFieldComponents[numY][numX];

                    //Checks if an invalid element got reached
                    if (!component.strike && (component != fieldSelectionA && component != fieldSelectionB))
                    {
                        SelectionReset();
                        return;
                    }
                    //Increments to the next element in line
                    numX++;
                }

                //Is called if loop does not reach the "return".
                SelectionStrike();
                return;
            }
        }
        else
        {
            SelectionReset();
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

        //Strikes the selected fields
        fieldSelectionA.FieldStrike();
        fieldSelectionB.FieldStrike();

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
        strikedPairs++;
        uiController.SetStrikedPairsCount(strikedPairs);
    }


    /// <summary>
    /// Resets the selection and the fields.
    /// </summary>
    public void SelectionReset()
    {
        if (fieldSelectionA != null) fieldSelectionA.FieldSelectionReset();
        if (fieldSelectionB != null) fieldSelectionB.FieldSelectionReset();
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

        if (numberFieldComponents[_y].Count < GameParameter.maxLineLength)
        {
            strikedLineFound = false;
        }
        else
        {
            for (int i = 0; i < GameParameter.maxLineLength; i++)
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
    }


    /// <summary>
    /// Creates a savegame and returns it.
    /// </summary>
    /// <returns></returns>
    public Savegame GetSavegame()
    {
        //Create number list
        List<SerializableNumberField> numberFields = new List<SerializableNumberField>();

        for (int i = 0; i < numberFieldComponents.Count; i++)
        {
            for (int j = 0; j < numberFieldComponents[i].Count; j++)
            {
                numberFields.Add(new SerializableNumberField(numberFieldComponents[i][j]));
            }
        }

        string gameName = PlayerPrefs.GetString("SavegameName");
        int gameMode = PlayerPrefs.GetInt("SavegameMode");

        //savegame
        return new Savegame(numberFields, strikedLines, gameName, gameMode, passedTime, backLog, strikedPairs, undoCount);
    }


    /// <summary>
    /// Undos the last action in the backlog.
    /// </summary>
    public void UndoLastAction()
    {
        backLog.UndoLastAction();
    }


    /// <summary>
    /// Gets a numbercomponent by given cordinates.
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <returns></returns>
    public NumberComponent GetNumberComponent(int _x, int _y)
    {
        return numberFieldComponents[_y][_x];
    }


    /// <summary>
    /// Increase the pair counter by the added value.
    /// </summary>
    /// <param name="_add"></param>
    public void IncreasePairByInt(int _add)
    {
        strikedPairs += _add;
        uiController.SetStrikedPairsCount(strikedPairs);
    }


    /// <summary>
    /// Increase the undo counter by the added value.
    /// </summary>
    /// <param name="_add"></param>
    public void IncreaseUndoByInt(int _add)
    {
        undoCount += _add;
        uiController.SetUndoCount(undoCount);
    }


    /// <summary>
    /// Returns if changes were made.
    /// </summary>
    /// <returns></returns>
    public bool IsDirty()
    {
        return isDirty;
    }
}
