using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

// 建议重命名类以避免命名冲突
public class CustomNetworkManager : MonoBehaviour
{
    private UnityTransport transport;

    void Start()
    {
        // 获取Unity Transport组件
        transport = GetComponent<UnityTransport>();
        
        // 新的配置方式 - 通过ConnectionData设置
        // transport.ConnectionData = new UnityTransport.ProtocolOptions
        // {
        //     ProtocolType = UnityTransport.ProtocolType.RelayUnityTransport
        // };
        
        // 初始化网络管理器回调
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public void StartHost()
    {
        // 启动P2P主机
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("P2P主机已启动");
        }
    }

    public void StartClient()
    {
        // 启动P2P客户端
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("正在连接到P2P主机...");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"客户端 {clientId} 已连接");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"客户端 {clientId} 已断开连接");
    }
}