using UnityEngine;

public class TogglePanel : MonoBehaviour //testing
{
    public GameObject panelToToggle;

    public void TogglePanelVisibility()
    {
        if (panelToToggle != null)
            panelToToggle.SetActive(!panelToToggle.activeSelf);
    }
}
