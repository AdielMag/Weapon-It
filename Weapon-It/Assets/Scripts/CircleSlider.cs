using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CircleSlider : MonoBehaviour
{
    public Image fill;
    public float fillEndValue;

    [Range(0,1)]
    public float value;

    float targetValue;

    private void Update()
    {
        targetValue = value * fillEndValue;

        fill.fillAmount = targetValue;
    }
}
