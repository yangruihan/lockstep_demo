using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendBtn : MonoBehaviour
{
    public InputField inputField;

    public void Start()
    {
        MessageManager.Instance.RegisteMessage(MessageType.STRING_MSG, OnMessageReceive);
    }

    public void OnClick()
    {
        string text = inputField.text;

        StringMessage strMessage = new StringMessage();
        strMessage.PlayerId = GameManager.Instance.LocalPlayer.PlayerId;
        strMessage.Content = text;

        MessageRequest.Instance.SendMessage(strMessage);
    }

    public void OnMessageReceive(BaseMessage msg)
    {
        StringMessage smsg = msg as StringMessage;

        Debug.Log(smsg.PlayerId + ": " + smsg.Content);
    }
}
