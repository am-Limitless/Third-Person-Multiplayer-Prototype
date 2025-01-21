using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [Header("Sphere Prefab Settings")]
    public GameObject spherePrefab;

    private GameObject spawnedSphere;

    void Start()
    {
        LogClientServerState();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnSphereOnServer();
        AssignAuthorityToFirstClient();
    }

    private void LogClientServerState()
    {
        if (isServer)
            Debug.Log("Sphere spawned on the server.");

        if (isClient)
            Debug.Log("Sphere visible on the client.");
    }

    private void SpawnSphereOnServer()
    {
        // Spawn the sphere at a specific position
        Vector3 spawnPosition = new Vector3(0, 1, 0); // Example position
        Quaternion spawnRotation = Quaternion.identity;

        spawnedSphere = Instantiate(spherePrefab, spawnPosition, spawnRotation);
        NetworkServer.Spawn(spawnedSphere);
    }

    private void AssignAuthorityToFirstClient()
    {
        if (spawnedSphere == null || NetworkServer.connections.Count == 0)
        {
            Debug.LogWarning("[Server] No sphere or clients available for authority assignment.");
            return;
        }

        NetworkIdentity networkIdentity = spawnedSphere.GetComponent<NetworkIdentity>();
        if (networkIdentity == null)
        {
            Debug.LogError("[Server] NetworkIdentity not found on the spawned sphere!");
            return;
        }

        foreach (var connection in NetworkServer.connections.Values)
        {
            networkIdentity.AssignClientAuthority(connection);
            Debug.Log("[Server] Authority assigned to client: " + connection.connectionId);
        }
    }
}