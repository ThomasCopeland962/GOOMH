/*using UnityEngine;
using UnityEngine.UI;
public class FastforwardToggle : MonoBehaviour
{
    public Sprite spriteA;
    public Sprite spriteB;
    private Image targetImage;
    public bool isStateA = true;

    void Start()
    {
        targetImage = GetComponent<Image>();
        targetImage.sprite = spriteA;
    }

    public void ToggleSprite()
    {
        if(Time.timeScale > 0f)
        {
            isStateA = !isStateA; 
            targetImage.sprite = isStateA ? spriteA : spriteB;
        }

    }   
}*/

using UnityEngine;
using UnityEngine.UI;

public class FastforwardToggle : MonoBehaviour
{
    public Sprite spriteA;
    public Sprite spriteB;

    private Image targetImage;
    public bool isStateA = true;

    void Start()
    {
        targetImage = GetComponent<Image>();
        UpdateVisuals();
    }

    public void ToggleSprite()
    {
        if (Time.timeScale > 0f)
        {
            isStateA = !isStateA;
            UpdateVisuals();
        }
    }

    private void UpdateVisuals()
    {
        if (targetImage != null)
        {
            targetImage.sprite = isStateA ? spriteA : spriteB;
        }
    }
}
