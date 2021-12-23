using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LifeBar : MonoBehaviour
{
    [SerializeField]
    private Slider lifeSlider1;
    [SerializeField]
    private Slider lifeSlider2;
    [SerializeField]
    private Slider lifeSlider3;
    [SerializeField]
    private int healthMaximum;
    [SerializeField]
    private int currentHealth;

    void Update()
    {
        float healthPerBar = healthMaximum / 3f;
        //Red life
        lifeSlider3.value = Mathf.Min(1f, currentHealth / healthPerBar);
        //Yellow life
        lifeSlider2.value = Mathf.Min(1f, Mathf.Max(0f, currentHealth / healthPerBar - 1));
        //Green life
        lifeSlider1.value = Mathf.Min(1f, Mathf.Max(0f, currentHealth / healthPerBar - 2));
    }
}
