using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageTest : NetworkBehaviour
{
    uint i = 0;
    float delay = 2f;
    float timer = 0f;

	[ClientCallback]
	private void Start()
	{
		MessageManager.Instance.RegisteMessage (MessageType.STRING_MSG, OnStringMessageRecived);
	}

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer) return;

        timer = Time.deltaTime + timer;
        if (timer > delay)
        {
            timer = 0;

            i++;
			StringMessage message = new StringMessage();
            message.PlayerId = 1;
			message.Content = "str: " + i;

            MessageRequest.Instance.SendMessage(message);
        }
    }

	public void OnStringMessageRecived(BaseMessage msg)
	{
		StringMessage strMsg = msg as StringMessage;

		if (strMsg != null)
		{
			Debug.Log ("OnStringMessageRecived" + strMsg.Content);
		}
	}
}
