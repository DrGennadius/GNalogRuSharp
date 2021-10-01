using System.ComponentModel;

namespace GNalogRuSharp
{
    /// <summary>
    /// Вид документа, удостоверяющего личность
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// Паспорт гражданина СССР
        /// </summary>
        [Description("Паспорт гражданина СССР")]
        PassportUSSR = 1,

        /// <summary>
        /// Свидетельство о рождении
        /// </summary>
        [Description("Свидетельство о рождении")]
        BirthCertificate = 3,

        /// <summary>
        /// Паспорт иностранного гражданина
        /// </summary>
        [Description("Паспорт иностранного гражданина")]
        PassportForeign = 10,

        /// <summary>
        /// Вид на жительство в России
        /// </summary>
        [Description("Вид на жительство в России")]
        ResidencePermit = 12,

        /// <summary>
        /// Разрешение на временное проживание в России
        /// </summary>
        [Description("Разрешение на временное проживание в России")]
        ResidencePermitTemp = 15,

        /// <summary>
        /// Свидетельство о предоставлении временного убежища на территории России
        /// </summary>
        [Description("Свидетельство о предоставлении временного убежища на территории России")]
        AsylumCertificateTemp = 19,

        /// <summary>
        /// Паспорт гражданина России
        /// </summary>
        [Description("Паспорт гражданина России")]
        PassportRussia = 21,

        /// <summary>
        /// Свидетельство о рождении, выданное уполномоченным органом иностранного государства
        /// </summary>
        [Description("Свидетельство о рождении, выданное уполномоченным органом иностранного государства")]
        BirthCertificateForeign = 23,

        /// <summary>
        /// Вид на жительство иностранного гражданина
        /// </summary>
        [Description("Вид на жительство иностранного гражданина")]
        ResidencePermitForeign = 62
    }
}
