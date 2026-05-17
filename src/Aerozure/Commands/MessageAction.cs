namespace Aerozure.Commands;

public class MessageOption(MessageAction action)
{
    public MessageAction Action => action;
    
    public static MessageOption Complete() => new MessageOption(MessageAction.Complete);
    public static MessageOption Defer(TimeSpan timeSpan) => new DeferMessageOption(timeSpan);
    public static MessageOption Deadletter(string reason) => new DeadletterMessageOption(reason);
    public static MessageOption Abandon()=> new MessageOption(MessageAction.Abandon);
}

public enum MessageAction
{
    Complete,
    Abandon,
    Deadletter,
    Defer,
    None
}

public class DeadletterMessageOption(string reason) : MessageOption(MessageAction.Deadletter)
{
    public string Reason => reason;
}

public class DeferMessageOption(TimeSpan timespan, string? messageId = null) : MessageOption(MessageAction.Defer)
{
    public TimeSpan Timespan => timespan;
    public string? MessageId { get; } = messageId;
}
