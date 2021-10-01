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
        PassportUSSR = 1,

        /// <summary>
        /// Свидетельство о рождении
        /// </summary>
        BirthCertificate = 3,

        /// <summary>
        /// Паспорт иностранного гражданина
        /// </summary>
        PassportForeign = 10,

        /// <summary>
        /// Вид на жительство в России
        /// </summary>
        ResidencePermit = 12,

        /// <summary>
        /// Разрешение на временное проживание в России
        /// </summary>
        ResidencePermitTemp = 15,

        /// <summary>
        /// Свидетельство о предоставлении временного убежища на территории России
        /// </summary>
        AsylumCertificateTemp = 19,

        /// <summary>
        /// Паспорт гражданина России
        /// </summary>
        PassportRussia = 21,

        /// <summary>
        /// Свидетельство о рождении, выданное уполномоченным органом иностранного государства
        /// </summary>
        BirthCertificateForeign = 23,

        /// <summary>
        /// Вид на жительство иностранного гражданина
        /// </summary>
        ResidencePermitForeign = 62
    }
}
