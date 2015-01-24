using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private const string typeName = "SW4Gmarine";
    private const string gameName = "R00mName";

    private bool isRefreshingHostList = false;
    private HostData[] hostList;

    public GameObject playerPrefab;
    
    void Start()
    {
        MasterServer.ipAddress = "192.168.63.239";
        MasterServer.port = 23466;
        Network.natFacilitatorIP = "192.168.63.239";
        Network.natFacilitatorPort = 50005;
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }

    private void StartServer()
    {
        Network.InitializeServer(4, 25000, false);
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server initialized!");
        PlayerController.GlobalInstance.CreateCrewMember();
        Ship.GlobalInstance.Init();
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected to the server!");
        PlayerController.GlobalInstance.CreateCrewMember();
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    void Update()
    {
        if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            isRefreshingHostList = false;
            hostList = MasterServer.PollHostList();
        }
    }

    private void RefreshHostList()
    {
        if (!isRefreshingHostList)
        {
            isRefreshingHostList = true;
            MasterServer.RequestHostList(typeName);
        }
    }


    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }
}
