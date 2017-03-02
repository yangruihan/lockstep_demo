using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageTest : NetworkBehaviour {

    [ClientCallback]
	void Start () {
        if (!isLocalPlayer) return;

        BaseMessage message = new BaseMessage();
        message.PlayerId = 1;
        message.Type = 0;

        MessageRequest.Instance.SendMessage(message);
        Debug.Log("发送命令成功");
	}
}
