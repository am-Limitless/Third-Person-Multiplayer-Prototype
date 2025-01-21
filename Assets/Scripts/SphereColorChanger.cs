using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class SphereColorChanger : NetworkBehaviour
{
    public Button interactButton; // Reference to the UI button
    public Renderer sphereRenderer; // Reference to the sphere's Renderer

    [SyncVar(hook = nameof(OnColorChanged))]
    private Color currentColor; // Synced variable to track color changes across the network

    private void Start()
    {
        if (sphereRenderer == null)
            sphereRenderer = GetComponent<Renderer>();

        // Ensure the sphere has a unique material instance
        sphereRenderer.material = sphereRenderer.material;

        // Set the initial color
        currentColor = sphereRenderer.material.color;

        // Assign the button dynamically if not set in the inspector
        if (interactButton == null)
        {
            GameObject buttonObject = GameObject.FindWithTag("InteractButton");
            if (buttonObject != null)
                interactButton = buttonObject.GetComponent<Button>();
        }

        // Add the button listener
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(Interact);
        }
        else
        {
            Debug.LogWarning("[SphereColorChanger] Interact button not found!");
        }
    }

    // Method called when the button is clicked
    public void Interact()
    {
        if (!isServer)
        {
            Debug.LogError("[Client] Interact() called, but this should only run on the server!");
            return;
        }

        Debug.Log("[Server] Interact() called. Changing color...");
        ChangeColor(Random.ColorHSV());
    }

    // Change the sphere's color (Server Only)
    [Server]
    private void ChangeColor(Color newColor)
    {
        Debug.Log($"[Server] Changing color to {newColor}");
        currentColor = newColor; // Update the SyncVar, triggering the hook
    }

    // Hook function triggered when `currentColor` is updated
    private void OnColorChanged(Color oldColor, Color newColor)
    {
        Debug.Log($"[Client] OnColorChanged() called. Old: {oldColor}, New: {newColor}");
        sphereRenderer.material.color = newColor; // Update the material color
    }

    private void OnDestroy()
    {
        // Remove the button listener to avoid memory leaks
        if (interactButton != null)
        {
            interactButton.onClick.RemoveListener(Interact);
        }
    }
}
