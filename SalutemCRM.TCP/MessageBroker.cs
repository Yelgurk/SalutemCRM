using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.TCP;

public enum MBEnums : ushort
{
    NONE = 10000,
    STRING,
    FILE_JSON,
    GET_FILE_JSON,
    USER_JSON
};

public class MessageBroker
{
    public static string EndMessage { get; } = "<[END]>";

    private static List<string> IncomingMessages { get; } = new List<string>();
    
    public MBEnums MessageType { get; private set; } = MBEnums.NONE;

    public string MessageContainer { get; private set; } = "";

    public List<DataReceivedArgs>? IncomingPackage(string data)
    {
        bool IsEnded = false;

        MessageContainer += data;

        if (MessageContainer.Contains(EndMessage))
        {
            MessageContainer
                .Split(EndMessage)
                .SkipLast(1)
                .DoForEach(IncomingMessages.Add);

            MessageContainer = MessageContainer.Split(EndMessage).Last();

            IsEnded = true;
        }

        if (IsEnded)
            return new List<DataReceivedArgs>()
                .DoInst(res => IncomingMessages.DoForEach(x => res.Add(new() {
                    MessageType = Convert.ToUInt16(x.Substring(0, 5)).EnumCast<MBEnums>(),
                    ReceivedBytes = x.Length - 5,
                    Message = x.Remove(0, 5)
                })))
                .Do(res => IncomingMessages.Clear());
        else
            return null;
    }
}
