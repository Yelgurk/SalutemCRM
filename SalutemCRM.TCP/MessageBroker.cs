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
    public static string BeginMessage { get; } = "<[BEGIN]>";
    public static string EndMessage { get; } = "<[END]>";
    
    public MBEnums MessageType { get; private set; } = MBEnums.NONE;

    public string MessageContainer { get; private set; } = "";

    public MessageBroker? IncomingPackage(byte[] bytes) => IncomingPackage(Encoding.UTF8.GetString(bytes));

    public MessageBroker? IncomingPackage(string data)
    {
        bool IsEnded = false;

        if (data.StartsWith(BeginMessage))
        {
            MessageContainer = "";
            data = data.Remove(0, BeginMessage.Length);

            MessageType = Convert.ToUInt16(data.Substring(0, 5)).EnumCast<MBEnums>();
            data = data.Remove(0, 5);
        }

        if (data.EndsWith(EndMessage))
        {
            IsEnded = true;
            data = data.Remove(data.Length - EndMessage.Length);
        }

        MessageContainer += data;

        if (IsEnded)
            return this;
        else
            return null;
    }
}
