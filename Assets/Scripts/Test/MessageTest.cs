using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageTest : NetworkBehaviour
{
    uint i = 0;
    float delay = 5f;
    float timer = 0f;

    [ClientCallback]

    private void Update()
    {
        if (!isLocalPlayer) return;

        timer = Time.deltaTime + timer;
        if (timer > delay)
        {
            timer = 0;

            i++;
            BaseMessage message = new BaseMessage();
            message.PlayerId = i;
            message.Type = 0;

            MessageRequest.Instance.SendMessage(message);
        }
    }
}
