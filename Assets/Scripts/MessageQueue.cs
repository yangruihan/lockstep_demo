using System;

[Serializable]
public class MessageQueue
{
    public long frameIdx;
    public BaseMessage[] messages;

    public MessageQueue(long frameIdx, BaseMessage[] messages)
    {
        this.frameIdx = frameIdx;
        this.messages = messages;
    }
}
