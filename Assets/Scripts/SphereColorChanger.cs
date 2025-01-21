using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class SphereColorChanger : NetworkBehaviour
{
    public Button interactButton;
    public Renderer sphereRenderer;

    [SyncVar(hook = nameof(OnColorChanged))]
    private Color currentColor;

    private void Start()
    {
        if (sphereRenderer == null)
            sphereRenderer = GetComponent<Renderer>();

        sphereRenderer.material = sphereRenderer.material;

        currentColor = sphereRenderer.material.color;

        if (interactButton == null)
        {
            GameObject buttonObject = GameObject.FindWithTag("InteractButton");
            if (buttonObject != null)
                interactButton = buttonObject.GetComponent<Button>();
        }

        if (interactButton != null)
        {
            interactButton.onClick.AddListener(Interact);
        }
        else
        {
            Debug.LogWarning("[SphereColorChanger] Interact button not found!");
        }
    }

    public void Interact()
    {
        ChangeColor(Random.ColorHSV());
    }

    [Server]
    private void ChangeColor(Color newColor)
    {
        currentColor = newColor;
    }

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        sphereRenderer.material.color = newColor;
    }

    private void OnDestroy()
    {
        if (interactButton != null)
        {
            interactButton.onClick.RemoveListener(Interact);
        }
    }
}