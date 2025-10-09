namespace PhoneDirectory
{
    /// <summary>
    /// Represents a phone directory entry with a phone number and associated name
    /// </summary>
    public class PhoneEntry
    {
        public required string PhoneNumber { get; set; }
        public required string Name { get; set; }
    }

    /// <summary>
    /// Enumeration of all possible phone states in the telephone switching system
    /// </summary>
    public enum PhoneState
    {
        ONHOOK,
        OFFHOOK_DIALTONE,
        CALLING,
        RINGING,
        TALKING_2WAY,
        TALKING_3WAY,
        TRANSFERRING,
        CONFERENCING
    }
}