using System;

namespace UttrekkFamilia.ModelsBVV
{
    /// <summary>
    /// [P#MEDIUM] Aktivitetslogg, tabell hvor man logger registreringer, endringer og andre handlinger en ansatt gjør på systemet.
    /// </summary>
    public partial class ActivitylogActivitylog
    {
        /// <summary>
        /// Unik primærnøkkel
        /// </summary>
        public int ActivityLogActivityLogId { get; set; }
        /// <summary>
        /// Hvilken ansatt som opprettet eller trigget at denne raden ble opprettet
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Nøkkel brukt til å sammenstille data fra forskjellige logger
        /// </summary>
        public string CorrelationGuid { get; set; }
        /// <summary>
        /// Hvilken type handling som ble gjennomført av ansatt
        /// </summary>
        public int OperationType { get; set; }
        /// <summary>
        /// Detaljer om handlingen som ble gjennomført av ansatt
        /// </summary>
        public string OperationDetails { get; set; }
        /// <summary>
        /// Navnet på entitet som var påvirket av handlingen ansatt gjennomførte
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// Navnet på tabellen som var påvirket av handlingen ansatt gjennomførte
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Id på entiteten som ble endret av ansatt
        /// </summary>
        public int TableId { get; set; }
        /// <summary>
        /// Id til rollen som ansatt benyttet da endringen/handlingen som er logget ble utført.
        /// </summary>
        public int? CreatedRoleId { get; set; }
        /// <summary>
        /// Id til stillingen som ansatt benyttet da endringen/handlingen som er logget ble utført.
        /// </summary>
        public int EmployeePositionId { get; set; }
        /// <summary>
        /// Enkelte loggede handlinger er knytt til en bestemt organisasjon, dette feltet inneholder da id til denne organisasjonen
        /// </summary>
        public int? OrganizationId { get; set; }
        /// <summary>
        /// Enkelte loggede handlinger er knytt til en bestemt klient, dette feltet inneholder da id til denne klienten
        /// </summary>
        public int? ClientId { get; set; }
        /// <summary>
        /// Enkelte loggede handlinger er knytt til en bestemt person, dette feltet inneholder da id til denne personen
        /// </summary>
        public int? PersonId { get; set; }
        /// <summary>
        /// Enkelte loggede handlinger er knytt til en bestemt sak, dette feltet inneholder da id til denne saken
        /// </summary>
        public int? CaseId { get; set; }
        /// <summary>
        /// Dato da denne loggføringen ble opprettet. Info: Alle datoer er uten lokal tid (dvs UTC).
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Dato da endring av en rad fant sted
        /// </summary>
        public DateTime ChangeDateTime { get; set; }
        /// <summary>
        /// IPadressen til ansatt som gjør handlingen som blir logget
        /// </summary>
        public byte[] Ipaddress { get; set; }
        /// <summary>
        /// Noen tjenester bruker windows autentisering, dette feltet angir hvilken windows-bruker som utførte endring/handling logge i aktivitetsloggen
        /// </summary>
        public string WindowsUserId { get; set; }
        /// <summary>
        /// Endre fra admintool (av visma)
        /// </summary>
        public bool ChangedFromAdminTools { get; set; }
        /// <summary>
        /// Avvik
        /// </summary>
        public bool ClientAccessDeviation { get; set; }
        /// <summary>
        /// Json som angir gamle/original verdier før evt. endring
        /// </summary>
        public string OldValueJson { get; set; }
        /// <summary>
        /// Json som angir de nye verdiene som er gjort ved evt. endring
        /// </summary>
        public string NewValueJson { get; set; }
        /// <summary>
        /// Angir (http) api path til metoden som trigget en føring i aktivitetsloggen
        /// </summary>
        public string ApiPath { get; set; }
        /// <summary>
        /// Enheter (fra organisasjonsstrukturen) som ansatt hadde tilgang til på aktuelt tidspunkt
        /// </summary>
        public string CreatedByOrgUnits { get; set; }
    }
}
