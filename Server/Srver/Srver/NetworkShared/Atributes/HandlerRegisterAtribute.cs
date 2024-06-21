namespace NetworkShared
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerRegisterAtribute : Attribute
    {
        public PacketType PacketType { get; set; }

        public HandlerRegisterAtribute(PacketType type) 
        {
            PacketType = type;      
        }
    }
}
