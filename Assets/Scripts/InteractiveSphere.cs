using UnityEngine;
using Mirror;

public class InteractiveSphere : NetworkBehaviour
{
    private Renderer sphereRenderer;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
    }

    [Command]
    public void CmdChangeColor()
    {
        Color newColor = new Color(Random.value, Random.value, Random.value);
        RpcChangeColor(Random.ColorHSV());
    }

    [ClientRpc]
    void RpcChangeColor(Color color)
    {
        sphereRenderer.material.color = color;
    }

    void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.E)) // Change 'E' to your desired key
        {
            CmdChangeColor();
        }
    }
}