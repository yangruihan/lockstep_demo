using System;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : Singleton<MessageManager>
{
    private static Dictionary<byte, Action<BaseMessage>> _msgHandlerMap = new Dictionary<byte, Action<BaseMessage>>();

    public void RegisteMessage(byte type, Action<BaseMessage> action)
    {
        _msgHandlerMap.Add(type, action);
    }

    public Action<BaseMessage> GetAction(byte type)
    {
        if (_msgHandlerMap.ContainsKey(type))
        {
            return _msgHandlerMap[type];
        }

        return null;
    }
}