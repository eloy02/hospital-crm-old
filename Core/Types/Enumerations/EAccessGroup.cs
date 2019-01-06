using System.ComponentModel;

namespace Core.Types.Enumerations
{
    public enum EAccessGroup
    {
        [Description("Администратор")]
        Admin,

        [Description("Регистратура")]
        RegPacients,

        [Description("Центр реабилитации")]
        RehabilitationCentre
    }
}