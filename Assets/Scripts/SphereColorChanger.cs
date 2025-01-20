using Mirror;
using UnityEngine;

public class SphereColorChanger : NetworkBehaviour
{
    public Renderer sphereRenderer; // Reference to the sphere's renderer
    private Material sphereMaterial; // Reference to the material of the sphere
    private Color currentColor; // Current color of the sphere

    void Start()
    {
        // Ensure we have the sphere's renderer
        if (sphereRenderer == null)
            sphereRenderer = GetComponent<Renderer>();

        // Get the material of the sphere
        sphereMaterial = sphereRenderer.material;

        // Initialize the current color to the sphere's initial color
        currentColor = sphereMaterial.color;
    }

    // This method will be called when a player interacts with the sphere
    public void Interact()
    {
        if (!isLocalPlayer) return; // Only allow the local player to trigger the color change

        // Call the command to change the color on the server
        CmdChangeColor(Random.ColorHSV());
    }

    // Command to change the color on the server
    [Command]
    void CmdChangeColor(Color newColor)
    {
        // Change color on the server
        currentColor = newColor;
        RpcChangeColor(newColor); // Synchronize color change with all clients
    }

    // ClientRpc to change the color on all clients
    [ClientRpc]
    void RpcChangeColor(Color newColor)
    {
        // Update the material's color on all clients
        sphereMaterial.color = newColor;
    }
}
