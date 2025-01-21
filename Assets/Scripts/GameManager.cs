using Mirror;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject spherePrefab; // Assign your sphere prefab here in the Inspector
    private GameObject spawnedSphere;

    void Start()
    {
        if (isServer)
            Debug.Log("Sphere spawned on the server.");

        if (isClient)
            Debug.Log("Sphere visible on the client.");
    }

    public override void OnStartServer()
    {
        // Ensure this runs on the server
        base.OnStartServer();

        // Spawn the sphere at a specific position
        Vector3 spawnPosition = new Vector3(0, 1, 0); // Example position
        Quaternion spawnRotation = Quaternion.identity;

        spawnedSphere = Instantiate(spherePrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(spawnedSphere);

        // Assign authority to the first connected client after spawning
        AssignAuthority();
    }

    private void AssignAuthority()
    {
        // Assign authority only after the object is spawned
        if (spawnedSphere != null && NetworkServer.connections.Count > 0)
        {
            // Assuming the first connected player should have authority over the sphere
            NetworkIdentity networkIdentity = spawnedSphere.GetComponent<NetworkIdentity>();
            if (networkIdentity != null)
            {
                // Assign the first connected client authority
                networkIdentity.AssignClientAuthority(NetworkServer.connections.Values.First());
                Debug.Log("[Server] Authority assigned to the first client.");
            }
            else
            {
                Debug.LogError("[Server] NetworkIdentity not found on the spawned sphere!");
            }
        }
    }
}
