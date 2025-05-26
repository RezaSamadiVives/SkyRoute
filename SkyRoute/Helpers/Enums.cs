using System.Runtime.Serialization;
namespace SkyRoute.Helpers
{
    public enum TripClass
    {
        [EnumMember(Value = "economy")]
        Economy,

        [EnumMember(Value = "business")]
        Business
    }

    public enum TripType
    {
        [EnumMember(Value = "enkel")]
        Enkel,


        [EnumMember(Value = "retour")]
        Retour
    }

}
