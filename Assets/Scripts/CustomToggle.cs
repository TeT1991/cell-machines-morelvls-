using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    [SerializeField] private Image toggleImage; 
    [SerializeField] private Sprite enabledSprite; 
    [SerializeField] private Sprite disabledSprite;

    [SerializeField] private UnityEvent<bool> onToggle; 

    public bool IsEnabled { get; private set; } = true;

    public void Switch()
    {
        IsEnabled = !IsEnabled;
        toggleImage.sprite = IsEnabled ? enabledSprite : disabledSprite;

        onToggle?.Invoke(IsEnabled);
    }
}
