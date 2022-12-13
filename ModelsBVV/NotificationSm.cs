using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Tabell for å SMS&amp;apos;er sendt (eller skal sendes). Feks en påminnelse om en avtale til ekstern deltaker.
    /// </summary>
    public partial class NotificationSm
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int NotificationSmsId { get; set; }
        /// <summary>
        /// Opprettet av
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// SMS sendt dato (sendt til operatør)
        /// </summary>
        public DateTime SendDate { get; set; }
        /// <summary>
        /// Status på sms-sending
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Id til hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int? EventElementId { get; set; }
        /// <summary>
        /// Type hendelse, kan være korrespondanse, journal etc.
        /// </summary>
        public int? EventElementType { get; set; }
        /// <summary>
        /// Id på mottaker (Person)
        /// </summary>
        public int RecipientId { get; set; }
        /// <summary>
        /// Teksten på sms&amp;apos;en
        /// </summary>
        public string SmsText { get; set; }
        /// <summary>
        /// Telefon sms sendes til
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Id fra operatør når sms er sendt
        /// </summary>
        public string ExternalSmsId { get; set; }
        /// <summary>
        /// Status ved feil på sending av sms
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// Dato fra aktuell hendelse
        /// </summary>
        public DateTime? EventElementDate { get; set; }
        /// <summary>
        /// For sms&amp;apos;er knytt til Avtaler(Appointments) så beskriver dette status årsak til sms ble opprettet betyr, 1=Opprettet, 2=24 timer før avtalen, 3=Avlyst, 4=Oppdatert
        /// </summary>
        public int? NotificationReasonId { get; set; }
        /// <summary>
        /// Avsender-tekst på sms
        /// </summary>
        public string SmsSender { get; set; }

        public virtual EnumEventelementtype EventElementTypeNavigation { get; set; }
        public virtual EnumSmsstatus StatusNavigation { get; set; }
    }
}
