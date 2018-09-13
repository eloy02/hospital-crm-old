using System.ComponentModel;

namespace Core.Types.Enumerations
{
    public enum EPatientType
    {
        [Description("Инвалид")]
        Invalid,

        [Description("ОВЗ")]
        OVZ
    }
}