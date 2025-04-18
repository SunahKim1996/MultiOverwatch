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

    // ����Ƽ �� �����Ʈ��ũ�� ������ ������, �÷��̾ ��� �� �� �ִ� ���� �ϳ� ����
    // ���̸� roomName / �� maxPlayerCount ���� �÷��̾ ���� �� �ִ� �� ����

    public override void OnConnectedToMaster()
        => PhotonNetwork.JoinOrCreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayerCount }, null);

    // Assets\Resources ���� �� Player ��� �̸��� �������� ���� 
    public override void OnJoinedRoom()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnTrans.position, spawnTrans.rotation);
    }
}
