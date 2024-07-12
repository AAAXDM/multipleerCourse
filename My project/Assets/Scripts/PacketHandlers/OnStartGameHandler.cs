using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

[HandlerRegisterAtribute(PacketType.OnStartGame)]
public class OnStartGameHandler : IPacketHandler
{
    int sceneNumber = 2;

    public void Handle(INetPacket packet, int connectionId)
    {
        var msg = (OnStartGame) packet;

        SceneManager.LoadScene(sceneNumber);
    }
}

