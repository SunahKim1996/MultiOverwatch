using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string roomName;
    [SerializeField] int maxPlayerCount;
    [SerializeField] string playerPrefabName;
    [SerializeField] Transform spawnTrans;

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 유니티 내 포톤네트워크의 연결을 끝내고, 플레이어가 들어 올 수 있는 방을 하나 만듦
    // 방이름 roomName / 총 maxPlayerCount 명의 플레이어가 들어올 수 있는 방 생성

    public override void OnConnectedToMaster()
        => PhotonNetwork.JoinOrCreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayerCount }, null);

    // Assets\Resources 폴더 내 Player 라는 이름의 프리팹을 생성 
    public override void OnJoinedRoom()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnTrans.position, spawnTrans.rotation);
    }
}
