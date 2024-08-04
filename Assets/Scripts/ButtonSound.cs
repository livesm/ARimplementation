using UnityEngine;


public class ButtonSound : MonoBehaviour
{
    public void PlaySound()
    {
        AudioManager.instance.PlayButtonClick();
    }
}
