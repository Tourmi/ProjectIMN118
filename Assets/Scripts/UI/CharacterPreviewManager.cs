using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterPreviewManager : MonoBehaviour
{

    public void FalconSelected()
    {
        Sprite Falcon = Resources.Load<Sprite>("Textures/Captain_Falcon_character_portrait");
        gameObject.GetComponent<Image>().sprite = Falcon;
    }

    public void FoxSelected()
    {
        Sprite Fox = Resources.Load<Sprite>("Textures/SFZ_Fox_McCloud");
        gameObject.GetComponent<Image>().sprite = Fox;
    }

    public void DLCSelected()
    {
        Sprite DLC = Resources.Load<Sprite>("Textures/DLC");
        gameObject.GetComponent<Image>().sprite = DLC;
    }
}
