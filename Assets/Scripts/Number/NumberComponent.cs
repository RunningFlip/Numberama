using UnityEngine;
using UnityEngine.UI;


public class NumberComponent : MonoBehaviour
{
    [Header("Specifications")]
    public bool strike;
    [Range(1, 9)]
    public int number = 1;

    [Header("Position")]
    public int positionX = -1;
    public int positionY = -1;

    [Header("UI")]
    public Text numberLabel;
    public Button button;
    public Image backgroundImage;

    [Header("Sprites")]
    public Sprite defaultSprite;
    public Sprite selectedSprite;
    public Sprite hintSprite;
    public Sprite strikedSprite;

    [Header("Color")]
    public Color alternativeTextColor;
    public Color strikedTextColor;


    [HideInInspector]
    public int id;


    //Color
    private Color startColor;

    //Flag
    private bool selected;
    private bool hintActive;


    /// <summary>
    /// Initializes the number value and the button listener.
    /// </summary>
    /// <param name="_number"></param>
    public void FieldSetup(int _number, int _x, int _y, int _id = -1)
    {
        gameObject.SetActive(true);

        startColor = numberLabel.color;

        number = _number;
        positionX = _x;
        positionY = _y;

        if (_id != -1)
        {
            id = _id;
        }
        else
        {
            id = GetHashCode();
        }

        numberLabel.text = number.ToString();
        button.onClick.AddListener(delegate { FieldSelection(); });
    }


    /// <summary>
    /// Sets a new y position.
    /// </summary>
    /// <param name="_y"></param>
    public void SetYPosition(int _y)
    {
        positionY = _y;
    }


    /// <summary>
    /// Disables the button and
    /// </summary>
    public void FieldStrike()
    {
        strike = true;
        button.interactable = false;

        backgroundImage.sprite = strikedSprite;
        numberLabel.color = strikedTextColor;

        FieldSelectionReset();
    }


    /// <summary>
    /// Resets the shown hightlight of the button and resets the flag.
    /// </summary>
    public void FieldSelectionReset()
    {
        if (!strike)
        {
            backgroundImage.sprite = defaultSprite;
            numberLabel.color = startColor;
        }

        selected = false;

        GameplayController.Instance.FieldSelected(this, false);
    }


    /// <summary>
    /// Activates the highlight.
    /// </summary>
    private void FieldSelection()
    {
        if (!selected)
        {
            backgroundImage.sprite = selectedSprite;
            selected = true;

            GameplayController.Instance.FieldSelected(this, true);
        }
        else
        {
            FieldSelectionReset();
        }
    }


    /// <summary>
    /// Toggles the hint effect.
    /// </summary>
    public void ToggleHint()
    {
        hintActive = !hintActive;
        numberLabel.color = alternativeTextColor;

        if (hintActive)
        {
            backgroundImage.sprite = hintSprite;
        }
        else
        {
            backgroundImage.sprite = defaultSprite;
        }
    }


    /// <summary>
    /// Undos the strike on this numberfield.
    /// </summary>
    public void UndoStrike()
    {
        strike = false;
        if (button != null)
        {
            button.interactable = true;
        }

        numberLabel.color = startColor;
        backgroundImage.sprite = defaultSprite;
    }
}
