using Microsoft.EntityFrameworkCore;

namespace UttrekkFamilia.ModelsBVV
{
    public partial class BVVDBContext : DbContext
    {
        private readonly string ConnectionString;
        public BVVDBContext(string connectionString)
        {
            ConnectionString = connectionString;
            Database.SetCommandTimeout(300);
        }

        public BVVDBContext(DbContextOptions<BVVDBContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(300);
        }

        public virtual DbSet<ActivitylogActivitylog> ActivitylogActivitylogs { get; set; }
        public virtual DbSet<AppointmentAppointment> AppointmentAppointments { get; set; }
        public virtual DbSet<AppointmentAppointmentcorrespondence> AppointmentAppointmentcorrespondences { get; set; }
        public virtual DbSet<AppointmentEmployee> AppointmentEmployees { get; set; }
        public virtual DbSet<AppointmentParticipant> AppointmentParticipants { get; set; }
        public virtual DbSet<AppointmentSetting> AppointmentSettings { get; set; }
        public virtual DbSet<AppointmentShipmentlog> AppointmentShipmentlogs { get; set; }
        public virtual DbSet<Assessment> Assessments { get; set; }
        public virtual DbSet<Assessmentevaluation> Assessmentevaluations { get; set; }
        public virtual DbSet<Assessmentreport> Assessmentreports { get; set; }
        public virtual DbSet<Assessmentrespondent> Assessmentrespondents { get; set; }
        public virtual DbSet<Assessmentsetting> Assessmentsettings { get; set; }
        public virtual DbSet<AuthorizationBluelight> AuthorizationBluelights { get; set; }
        public virtual DbSet<AuthorizationSetting> AuthorizationSettings { get; set; }
        public virtual DbSet<BaseregistryBaseregistry> BaseregistryBaseregistries { get; set; }
        public virtual DbSet<CaseCase> CaseCases { get; set; }
        public virtual DbSet<CaseCaseadditionalcategory> CaseCaseadditionalcategories { get; set; }
        public virtual DbSet<CaseCasecaseworker> CaseCasecaseworkers { get; set; }
        public virtual DbSet<CaseCaseexternalcaseworker> CaseCaseexternalcaseworkers { get; set; }
        public virtual DbSet<CaseCasestatus> CaseCasestatuses { get; set; }
        public virtual DbSet<ClientClient> ClientClients { get; set; }
        public virtual DbSet<ClientClientclientgroup> ClientClientclientgroups { get; set; }
        public virtual DbSet<ClientClientemployeedeviation> ClientClientemployeedeviations { get; set; }
        public virtual DbSet<ClientClientgroup> ClientClientgroups { get; set; }
        public virtual DbSet<ClientClientgrouporg> ClientClientgrouporgs { get; set; }
        public virtual DbSet<ClientClientstatus> ClientClientstatuses { get; set; }
        public virtual DbSet<CorrespondenceConnection> CorrespondenceConnections { get; set; }
        public virtual DbSet<CorrespondenceCorrespondence> CorrespondenceCorrespondences { get; set; }
        public virtual DbSet<CorrespondenceCorrespondenceattachment> CorrespondenceCorrespondenceattachments { get; set; }
        public virtual DbSet<CorrespondenceCorrespondencecaseworker> CorrespondenceCorrespondencecaseworkers { get; set; }
        public virtual DbSet<CorrespondenceDocument> CorrespondenceDocuments { get; set; }
        public virtual DbSet<CorrespondenceIncomingpost> CorrespondenceIncomingposts { get; set; }
        public virtual DbSet<CorrespondenceIncomingpostattachment> CorrespondenceIncomingpostattachments { get; set; }
        public virtual DbSet<CorrespondenceIncomingpostmetadatum> CorrespondenceIncomingpostmetadata { get; set; }
        public virtual DbSet<CustomerOrganization> CustomerOrganizations { get; set; }
        public virtual DbSet<DocumentDocumenttemplate> DocumentDocumenttemplates { get; set; }
        public virtual DbSet<EmployeeEmployee> EmployeeEmployees { get; set; }
        public virtual DbSet<EmployeeEmployeeposition> EmployeeEmployeepositions { get; set; }
        public virtual DbSet<EmployeeEmployeeteamrole> EmployeeEmployeeteamroles { get; set; }
        public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public virtual DbSet<EmployeeTeam> EmployeeTeams { get; set; }
        public virtual DbSet<EmployeeTeamclient> EmployeeTeamclients { get; set; }
        public virtual DbSet<EmployeeTeamclientgroup> EmployeeTeamclientgroups { get; set; }
        public virtual DbSet<EmployeeTeamorg> EmployeeTeamorgs { get; set; }
        public virtual DbSet<EnquiryDocument> EnquiryDocuments { get; set; }
        public virtual DbSet<EnquiryEnquiry> EnquiryEnquiries { get; set; }
        public virtual DbSet<EnquiryEnquirycaseworker> EnquiryEnquirycaseworkers { get; set; }
        public virtual DbSet<EnumAppointmentparticipanttype> EnumAppointmentparticipanttypes { get; set; }
        public virtual DbSet<EnumApprovalstatus> EnumApprovalstatuses { get; set; }
        public virtual DbSet<EnumAssessmentevaluationstatus> EnumAssessmentevaluationstatuses { get; set; }
        public virtual DbSet<EnumAssessmentevaluationtype> EnumAssessmentevaluationtypes { get; set; }
        public virtual DbSet<EnumCasestatus> EnumCasestatuses { get; set; }
        public virtual DbSet<EnumCasetype> EnumCasetypes { get; set; }
        public virtual DbSet<EnumClientemployeedeviationaccesspoint> EnumClientemployeedeviationaccesspoints { get; set; }
        public virtual DbSet<EnumClientstatus> EnumClientstatuses { get; set; }
        public virtual DbSet<EnumCorrespondencedirection> EnumCorrespondencedirections { get; set; }
        public virtual DbSet<EnumEmployeepreferredlanguage> EnumEmployeepreferredlanguages { get; set; }
        public virtual DbSet<EnumEventelementtype> EnumEventelementtypes { get; set; }
        public virtual DbSet<EnumFollowuprecipienttype> EnumFollowuprecipienttypes { get; set; }
        public virtual DbSet<EnumGender> EnumGenders { get; set; }
        public virtual DbSet<EnumHeaderfooterpagesetting> EnumHeaderfooterpagesettings { get; set; }
        public virtual DbSet<EnumIncomingpoststatus> EnumIncomingpoststatuses { get; set; }
        public virtual DbSet<EnumIncomingposttype> EnumIncomingposttypes { get; set; }
        public virtual DbSet<EnumInternalorwithheldtype> EnumInternalorwithheldtypes { get; set; }
        public virtual DbSet<EnumOperationtype> EnumOperationtypes { get; set; }
        public virtual DbSet<EnumOrgtype> EnumOrgtypes { get; set; }
        public virtual DbSet<EnumPdfpagemargin> EnumPdfpagemargins { get; set; }
        public virtual DbSet<EnumSecondheaderfooterpagesetting> EnumSecondheaderfooterpagesettings { get; set; }
        public virtual DbSet<EnumSenderrecipientkind> EnumSenderrecipientkinds { get; set; }
        public virtual DbSet<EnumSmsstatus> EnumSmsstatuses { get; set; }
        public virtual DbSet<EnumSvarutstatus> EnumSvarutstatuses { get; set; }
        public virtual DbSet<EnumTemplatetype> EnumTemplatetypes { get; set; }
        public virtual DbSet<Followup> Followups { get; set; }
        public virtual DbSet<FollowupMarkedreademployee> FollowupMarkedreademployees { get; set; }
        public virtual DbSet<FollowupRecipient> FollowupRecipients { get; set; }
        public virtual DbSet<JournalDocument> JournalDocuments { get; set; }
        public virtual DbSet<JournalJournal> JournalJournals { get; set; }
        public virtual DbSet<JournalJournalcaseworker> JournalJournalcaseworkers { get; set; }
        public virtual DbSet<NotificationSm> NotificationSms { get; set; }
        public virtual DbSet<OrganizationOrganization> OrganizationOrganizations { get; set; }
        public virtual DbSet<OrganizationOrganizationperson> OrganizationOrganizationpeople { get; set; }
        public virtual DbSet<PersonNetworkpersonrole> PersonNetworkpersonroles { get; set; }
        public virtual DbSet<PersonPerson> PersonPeople { get; set; }
        public virtual DbSet<PersonPersoncitizenshipnationality> PersonPersoncitizenshipnationalities { get; set; }
        public virtual DbSet<PersonPersonrole> PersonPersonroles { get; set; }
        public virtual DbSet<ReferralReferral> ReferralReferrals { get; set; }
        public virtual DbSet<ReferralReferralparent> ReferralReferralparents { get; set; }
        public virtual DbSet<TimeregistrationEventelement> TimeregistrationEventelements { get; set; }
        public virtual DbSet<TimeregistrationTimeregistration> TimeregistrationTimeregistrations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivitylogActivitylog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("activitylog_activitylog");

                entity.Property(e => e.ActivityLogActivityLogId)
                    .HasColumnName("ActivityLog_ActivityLogId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ApiPath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Angir (http) api path til metoden som trigget en føring i aktivitetsloggen");

                entity.Property(e => e.CaseId).HasComment("Enkelte loggede handlinger er knytt til en bestemt sak, dette feltet inneholder da id til denne saken");

                entity.Property(e => e.ChangeDateTime).HasComment("Dato da endring av en rad fant sted");

                entity.Property(e => e.ChangedFromAdminTools).HasComment("Endre fra admintool (av visma)");

                entity.Property(e => e.ClientAccessDeviation).HasComment("Avvik");

                entity.Property(e => e.ClientId).HasComment("Enkelte loggede handlinger er knytt til en bestemt klient, dette feltet inneholder da id til denne klienten");

                entity.Property(e => e.CorrelationGuid)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("Nøkkel brukt til å sammenstille data fra forskjellige logger");

                entity.Property(e => e.CreatedBy).HasComment("Hvilken ansatt som opprettet eller trigget at denne raden ble opprettet");

                entity.Property(e => e.CreatedByOrgUnits)
                    .IsUnicode(false)
                    .HasComment("Enheter (fra organisasjonsstrukturen) som ansatt hadde tilgang til på aktuelt tidspunkt");

                entity.Property(e => e.CreatedDate).HasComment("Dato da denne loggføringen ble opprettet. Info: Alle datoer er uten lokal tid (dvs UTC).");

                entity.Property(e => e.CreatedRoleId).HasComment("Id til rollen som ansatt benyttet da endringen/handlingen som er logget ble utført.");

                entity.Property(e => e.EmployeePositionId).HasComment("Id til stillingen som ansatt benyttet da endringen/handlingen som er logget ble utført.");

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("Navnet på entitet som var påvirket av handlingen ansatt gjennomførte");

                entity.Property(e => e.Ipaddress)
                    .IsRequired()
                    .HasColumnName("IPAddress")
                    .HasComment("IPadressen til ansatt som gjør handlingen som blir logget");

                entity.Property(e => e.NewValueJson)
                    .IsUnicode(false)
                    .HasComment("Json som angir de nye verdiene som er gjort ved evt. endring");

                entity.Property(e => e.OldValueJson)
                    .IsUnicode(false)
                    .HasComment("Json som angir gamle/original verdier før evt. endring");

                entity.Property(e => e.OperationDetails)
                    .IsUnicode(false)
                    .HasComment("Detaljer om handlingen som ble gjennomført av ansatt");

                entity.Property(e => e.OperationType).HasComment("Hvilken type handling som ble gjennomført av ansatt");

                entity.Property(e => e.OrganizationId).HasComment("Enkelte loggede handlinger er knytt til en bestemt organisasjon, dette feltet inneholder da id til denne organisasjonen");

                entity.Property(e => e.PersonId).HasComment("Enkelte loggede handlinger er knytt til en bestemt person, dette feltet inneholder da id til denne personen");

                entity.Property(e => e.TableId).HasComment("Id på entiteten som ble endret av ansatt");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("Navnet på tabellen som var påvirket av handlingen ansatt gjennomførte");

                entity.Property(e => e.WindowsUserId)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Noen tjenester bruker windows autentisering, dette feltet angir hvilken windows-bruker som utførte endring/handling logge i aktivitetsloggen");
            });

            modelBuilder.Entity<AppointmentAppointment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("appointment_appointment");

                entity.Property(e => e.Address)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasComment("Adresse");

                entity.Property(e => e.CancelDate).HasComment("Kansellert dato");

                entity.Property(e => e.CancellationReason)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Årsak til kansellering av avtale");

                entity.Property(e => e.CancellationReasonRegistryId).HasComment("Id til kanselleringsgrunn");

                entity.Property(e => e.CancelledBy).HasComment("Ansatt som kansellerte avtalen");

                entity.Property(e => e.CancelledByName)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn på ansatt som kansellerte avtalen");

                entity.Property(e => e.CaseId).HasComment("Id til tilknytt sak (personsak eller systemsak)");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Navn/Beskrivelse av Avtalekategori");

                entity.Property(e => e.CategoryRegistryId).HasComment("Id for avtalekategori, verdien refererer til grunnregister for avtalekategorier");

                entity.Property(e => e.ChangedBy).HasComment("Angir hvilken ansatt som har endret på avtalen");

                entity.Property(e => e.ChangedByName)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Angir navnet på den som har endret på en avtale");

                entity.Property(e => e.City)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Poststed");

                entity.Property(e => e.ClientBirthDate).HasComment("Klientens fødselsdato");

                entity.Property(e => e.ClientId).HasComment("Klientid");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Klientens navn");

                entity.Property(e => e.ConnectionType).HasComment("Angir tilnytning avtale har, den kan være knyttet til en klient/personsak, en organisasjon(systemsak) eller ha ingen tilknytning.");

                entity.Property(e => e.CorrespondenceCategoryName)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Korrespondanse kategori");

                entity.Property(e => e.CorrespondenceCategoryRegistryId).HasComment("Id til korrespondanse kategori");

                entity.Property(e => e.CreatedBy).HasComment("Hvilken ansatt som opprettet eller trigget at denne raden ble opprettet");

                entity.Property(e => e.CreatedByName)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Angir navnet på den som opprettet avtalen");

                entity.Property(e => e.CreatedDate).HasComment("Angir når tid avtalen ble opprettet/lagret første gang");

                entity.Property(e => e.CustomerGuid)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("Nøkkel som angir hvilken organisasjon entiteten tilhører, del av multitenancy, brukes for å skille kundedata fra hverandre. Hver kunde har sin egen unike CustomerGuid.");

                entity.Property(e => e.DateFrom).HasComment("Avtalens startdato og tidspunkt");

                entity.Property(e => e.DateTo).HasComment("Avtalens sluttdato og tidspunkt");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("Beskrivelse for avtalen");

                entity.Property(e => e.ExternalAppointmentId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Id til den eksterne avtalen, id komemr fra det eksterne systemet");

                entity.Property(e => e.ExternalAppointmentIdInBytes).HasComment("Id for ekstern avtale (ExternalAppointmentId), angitt i bytes");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.IsAllDay).HasComment("Angir om dette er en avtale som varer hele dagen");

                entity.Property(e => e.IsExternalAppointment).HasComment("Angir om det er en ekstern avtale (synkronisert fra f.eks. Microsoft Exchange)");

                entity.Property(e => e.IsSyncRequired).HasComment("Angir om man trenger å synkronisere denne avtalen, f.eks. fordi det har skjedd en endring");

                entity.Property(e => e.MeetingPlace)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Møtested");

                entity.Property(e => e.OrganizationId).HasComment("Id til tilknytt Organisasjon");

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnavn");

                entity.Property(e => e.Organizer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Angir epost til den som har organisert avtalen (synkronisert fra f.eks. Microsoft Exchange).");

                entity.Property(e => e.PersonId).HasComment("Personid");

                entity.Property(e => e.PostCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("Postnummer");

                entity.Property(e => e.PostCodeRegistryId).HasComment("Id til postnummer");

                entity.Property(e => e.TemplateId).HasComment("Id til mal");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Tittel på avtale");
            });

            modelBuilder.Entity<AppointmentAppointmentcorrespondence>(entity =>
            {
                entity.ToTable("appointment_appointmentcorrespondence");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik Id");

                entity.Property(e => e.AppointmentId).HasComment("Id til avtale");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til korrespondanse");
            });

            modelBuilder.Entity<AppointmentEmployee>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("appointment_employee");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Epost til ansatt");

                entity.Property(e => e.EmployeeId).HasComment("Id til ansatt");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn til ansatt");

                entity.Property(e => e.StartDate).HasComment("Ansatt aktivert fra");

                entity.Property(e => e.StopDate).HasComment("Ansatt aktivert til");
            });

            modelBuilder.Entity<AppointmentParticipant>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("appointment_participant");

                entity.Property(e => e.Address)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Adresse");

                entity.Property(e => e.AppointmentId).HasComment("Id til avtale");

                entity.Property(e => e.BirthNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Fødselsnummer");

                entity.Property(e => e.City)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("Poststed");

                entity.Property(e => e.Country)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("Land");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettet dato");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.IsAgainstSmsNotification).HasComment("Angir om deltaker ikke ønsker sms varsel");

                entity.Property(e => e.IsGettingCorrespondence).HasComment("Angir om deltaker får en korrespondanse(invitasjon)");

                entity.Property(e => e.IsGettingSms).HasComment("Angir om deltaker får sms varsel");

                entity.Property(e => e.IsMainParticipant).HasComment("Angir om dette er hoveddeltaker/arrangør av avtalen");

                entity.Property(e => e.IsSecretAddress).HasComment("Angir om det er en hemmelig adresse");

                entity.Property(e => e.Kind).HasComment("Type tilknytning (til klient, organisasjon eller manuelt registrert)");

                entity.Property(e => e.OrganizationNumber)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnummer");

                entity.Property(e => e.OrganizationPersonName)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn til person");

                entity.Property(e => e.ParticipantBirthDate).HasComment("Fødselsdato til deltaker");

                entity.Property(e => e.ParticipantEmail)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasComment("E-Post til deltaker");

                entity.Property(e => e.ParticipantId).HasComment("Id til deltaker(person)");

                entity.Property(e => e.ParticipantName)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn på deltaker");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode til telefonnummer");

                entity.Property(e => e.Type).HasComment("Angir om deltaker er ekstern eller intern");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("Postnummer");

                entity.HasOne(d => d.KindNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Kind)
                    .HasConstraintName("appointment_participant_ibfk_4");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("appointment_participant_ibfk_3");
            });

            modelBuilder.Entity<AppointmentSetting>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("appointment_settings");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.Settings)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Json som inneholder innstillinger for avtaler.");
            });

            modelBuilder.Entity<AppointmentShipmentlog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("appointment_shipmentlog");

                entity.Property(e => e.AppointmentId).HasComment("Id for avtalen");

                entity.Property(e => e.CategoryId).HasComment("Id til kategori for korrespondanse");

                entity.Property(e => e.CorrespondenceId).HasComment("Id for korrespondanse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.TemplateId).HasComment("Id for mal benyttet i korrespondanse");
            });

            modelBuilder.Entity<Assessment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("assessment");

                entity.Property(e => e.CaseId).HasComment("Id til (person-)saken");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.CoCaseWorkerIdsJson)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Id&apos;er til medsaksbehandlere(ansatt) på kartlegging");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettet dato");

                entity.Property(e => e.FinishedDate).HasComment("Avsluttet dato");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.OwnedBy).HasComment("Id til ansatt som eier kartlegging");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Tittel");

                entity.Property(e => e.TreatmentId).HasComment("Id fra tilsvarende behandling som er blitt opprettet i Checkware");

                entity.Property(e => e.Type).HasComment("Type kartlegging. Ref AssessmentSettings.CheckWareTreatmentTypesJson");
            });

            modelBuilder.Entity<Assessmentevaluation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("assessmentevaluation");

                entity.Property(e => e.AssessmentId).HasComment("Id til kartlegging");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.ExternalEvaluationId)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Ekstern id til evaluering (benyttes av integrasjon med Checkware, dette er en id fra dem)");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.RespondentId).HasComment("Id til respondent på kartlegging");

                entity.Property(e => e.StartDate).HasComment("Startdato for evaluering");

                entity.Property(e => e.Status).HasComment("Status på sendingen til checkware.");

                entity.Property(e => e.TemplateId)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Id til mal for evaluering");

                entity.Property(e => e.Type).HasComment("Type evaluering");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("assessmentevaluation_ibfk_1");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("assessmentevaluation_ibfk_2");
            });

            modelBuilder.Entity<Assessmentreport>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("assessmentreport");

                entity.Property(e => e.AssessmentId).HasComment("Id til kartlegging");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EvaluationId).HasComment("Id til evaluering");

                entity.Property(e => e.ExternalReportId).HasComment("Ekstern id til rapport (benyttes av integrasjon med Checkware, dette er en id fra dem som vi må ta vare på)");

                entity.Property(e => e.FileDataBlob).HasComment("Fildata for PDF dokument til kartleggingsrapporten");

                entity.Property(e => e.FileId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Opprinnelig fil-identifikator til dokumentet (ikke i bruk)");

                entity.Property(e => e.FilePassword)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Opprinnelig passord til dokumentet (ikke i bruk)");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn på rapport");

                entity.Property(e => e.RespondentId).HasComment("Id til respondent");
            });

            modelBuilder.Entity<Assessmentrespondent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("assessmentrespondent");

                entity.Property(e => e.AssessmentId).HasComment("Id til kartlegging");

                entity.Property(e => e.BirthNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Fødselsnummer til respondent");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Email)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Epost til respondent");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer til respondent");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode for telefonnummer til respondent, f.eks. +47");

                entity.Property(e => e.RespondentId).HasComment("Id til respondent");

                entity.Property(e => e.RespondentKind).HasComment("Type respondent");

                entity.Property(e => e.RespondentName)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn på respondent");

                entity.Property(e => e.RoleId).HasComment("Id til respondent rolle (far, mor, lærer, elev, bror etc.). Ref AssessmentSettings.CheckWareTreatmentTypesJson");

                entity.HasOne(d => d.RespondentKindNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RespondentKind)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("assessmentrespondent_ibfk_1");
            });

            modelBuilder.Entity<Assessmentsetting>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("assessmentsettings");

                entity.Property(e => e.CheckWareIndividualAssessmentsJson)
                    .IsUnicode(false)
                    .HasComment("Liste over individuelle kartlegginger fra checkware som kan benyttes i kartlegging");

                entity.Property(e => e.CheckWarePlansJson)
                    .IsUnicode(false)
                    .HasComment("Planer hentet fra Checkware, lagret lokalt for å øke responstid/brukervennlighet");

                entity.Property(e => e.CheckWareTreatmentTypesJson)
                    .IsUnicode(false)
                    .HasComment("Behandlingstyper hentet fra Checkware, lagret lokalt for å øke responstid/brukervennlighet");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Enabled).HasComment("Felt for å slå på/av kartlegging, kunder som ikke bruker kartlegging kan ha det avslått");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Passord(hashed) til integrasjon mot Checkware");

                entity.Property(e => e.Urlprefix)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasColumnName("URLPrefix")
                    .HasComment("Url prefiks, benyttes av integrasjonen mot Checkware. Man har en fast url + en prefiks til denne.");

                entity.Property(e => e.Username)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Brukernavn til integrasjon mot Checkware");
            });

            modelBuilder.Entity<AuthorizationBluelight>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("authorization_bluelight");

                entity.Property(e => e.AuthorizationBlueLightId)
                    .HasColumnName("Authorization_BlueLightId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.CreatedBy).HasComment("Id til Ansatt som har fått aktivert blålys-tilgang til angitt klient i ClientId");

                entity.Property(e => e.ReasonForBlueLight)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Id til grunn for å aktivere blålys-tilgang");
            });

            modelBuilder.Entity<AuthorizationSetting>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("authorization_settings");

                entity.Property(e => e.AuthorizationSettingsId)
                    .HasColumnName("Authorization_SettingsId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.IsMultipleOrgUnitsAllowed).HasComment("Om det er lov å gi tiltang til flere enheter enn en (fra organisasjonsstrukturen) ved innlogging");
            });

            modelBuilder.Entity<BaseregistryBaseregistry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("baseregistry_baseregistry");

                entity.Property(e => e.BaseRegistryBaseRegistryId)
                    .HasColumnName("BaseRegistry_BaseRegistryId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.RegistryCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Registerkode, angir hvilken type grunnregister dette er (tre-bokstav forkortelser).");

                entity.Property(e => e.RegistrySubCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("Tilleggskode for grunnregister. feks om en kategori for korrespondanse er utgående eller inngående");

                entity.Property(e => e.RegistryValue)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasComment("Verdien en grunnregister kode har. feks en beskrivelse &apos;Volda&apos; for poststed-grunnregister");

                entity.Property(e => e.RegistryValueCode)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("Grunnregister kode. feks postnr 6100 for et poststed");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<CaseCase>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("case_case");

                entity.Property(e => e.AffiliationId).HasComment("Id for tilhørighet (for systemsaker)");

                entity.Property(e => e.ArchiveJobQueueId).HasComment("Id til arkivjobb");

                entity.Property(e => e.AssignmentDescription)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Oppdragsbeskrivelse");

                entity.Property(e => e.AssignmentOrdererId).HasComment("Id for oppdragsgiver (for systemsaker)");

                entity.Property(e => e.AssignmentRegistryId).HasComment("Id for type oppdrag (for systemsaker)");

                entity.Property(e => e.CaseCaseId)
                    .HasColumnName("Case_CaseId")
                    .HasComment("Primærid");

                entity.Property(e => e.ClientId).HasComment("Klientid");

                entity.Property(e => e.ClosingReasonAssignmentRegistryId).HasComment("Id for konklusjonen på oppdrag, referanse til grunnregister (for systemsaker)");

                entity.Property(e => e.ClosingReasonRegistryId).HasComment("Grunn for å å lukke saken");

                entity.Property(e => e.ContactPersonId).HasComment("Id til kontaktperson (ansatt)");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til korrespondanse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettetdato");

                entity.Property(e => e.Description)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Saksbeskrivelse");

                entity.Property(e => e.EventElementsLastChanged).HasComment("Dato som angir når tid en hendelse var sist endret på i denne saken");

                entity.Property(e => e.HistoricMigrationDataJson)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Data fra evt migrering fra annet fagsystem");

                entity.Property(e => e.MainCategoryRegistryId).HasComment("Id for hovedkategori på saken");

                entity.Property(e => e.Number)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Saksnummer på format År-Nummer");

                entity.Property(e => e.OrganizationId).HasComment("Organisasjonsid");

                entity.Property(e => e.OwnedBy).HasComment("Angir hovedsaksbehandler(ansatt)");

                entity.Property(e => e.OwnedByTeamId).HasComment("Angir hvilket team som &apos;eier&apos; saken. Bare informasjon, og ikke noe med autorisasjon å gjøre.");

                entity.Property(e => e.PersoncaseTypeRegistryId).HasComment("Id til sakstype for personsaker");

                entity.Property(e => e.PrereferralCallDate).HasComment("Dato for når Kontaktperson (ansatt) satt");

                entity.Property(e => e.Public).HasComment("Om saken er offentlig (true) eller unntatt offentligheten. (for systemsaker)");

                entity.Property(e => e.ReceivedDate).HasComment("Mottattdato");

                entity.Property(e => e.ReferralClient).HasComment("Henvisning er fra klienten");

                entity.Property(e => e.Status).HasComment("Saksstatus");

                entity.Property(e => e.StatusClosedDate).HasComment("Dato for når saken fikk status Lukket");

                entity.Property(e => e.StatusInProgressDate).HasComment("Dato for når saken fikk status Under behandling");

                entity.Property(e => e.StatusNewDate).HasComment("Dato for når saken fikk status Ny");

                entity.Property(e => e.StatusTimeStamp).HasComment("Tidsstempel for når saksstatus ble satt");

                entity.Property(e => e.SystemcaseTypeRegistryId).HasComment("Id til sakstype for systemsaker");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Sakstittel");

                entity.Property(e => e.Type).HasComment("Sakstype");

                entity.Property(e => e.UseDefaultDeadline).HasComment("Angir om standard frist bruker skal benyttes");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("case_case_ibfk_16");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("case_case_ibfk_15");
            });

            modelBuilder.Entity<CaseCaseadditionalcategory>(entity =>
            {
                entity.ToTable("case_caseadditionalcategory");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Primærnøkkel");

                entity.Property(e => e.CaseId).HasComment("Referanse til primærnøkkel til saken");

                entity.Property(e => e.CategoryRegistryId).HasComment("Referanse til kategori");
            });

            modelBuilder.Entity<CaseCasecaseworker>(entity =>
            {
                entity.ToTable("case_casecaseworker");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Primærnøkkel");

                entity.Property(e => e.CaseId).HasComment("Saksid, referanse til primærnøkkel til saken");

                entity.Property(e => e.CaseworkerId).HasComment("Saksbehandlerid");
            });

            modelBuilder.Entity<CaseCaseexternalcaseworker>(entity =>
            {
                entity.ToTable("case_caseexternalcaseworker");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Primærnøkkel");

                entity.Property(e => e.CaseId).HasComment("Saksid, referanse til primærnøkkel til saken");

                entity.Property(e => e.PersonId).HasComment("Person id, referanse til persontabellen");
            });

            modelBuilder.Entity<CaseCasestatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("case_casestatus");

                entity.Property(e => e.CaseCaseStatusId)
                    .HasColumnName("Case_CaseStatusId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CaseId).HasComment("Saksid, referanse til primærnøkkel til saken");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EndStatus).HasComment("Type sluttstatus");

                entity.Property(e => e.EndStatusDate).HasComment("Dato for slutt status");

                entity.Property(e => e.Status).HasComment("Type status");

                entity.Property(e => e.StatusDate).HasComment("Status start dato");

                entity.HasOne(d => d.EndStatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EndStatus)
                    .HasConstraintName("case_casestatus_ibfk_4");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("case_casestatus_ibfk_3");
            });

            modelBuilder.Entity<ClientClient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_client");

                entity.Property(e => e.AffiliationId).HasComment("Id for tilhørighet");

                entity.Property(e => e.Allergies)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Felt for å angi evt. allergier klienten har");

                entity.Property(e => e.ArchiveJobQueueId).HasComment("Id til arkiv kø (for &apos;dossiermappe&apos;), benyttes ved arkivering");

                entity.Property(e => e.CivilStatusRegistryId).HasComment("Id til klientens sivilstatus");

                entity.Property(e => e.ClientCaseFileArchiveJobQueueId).HasComment("Id til arkiv kø (&apos;generell saksmappe&apos;), benyttes ved arkivering");

                entity.Property(e => e.ClientClientId)
                    .HasColumnName("Client_ClientId")
                    .HasComment("Primærid");

                entity.Property(e => e.Comment)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("Felt for fritekst kommentar på klient");

                entity.Property(e => e.ConsentChangeDate).HasComment("Angir datoen for når samtykke er gitt/fjernet");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.HasClosedCasesConsent).HasComment("Angir om klienten har samtykket til at saksbehandler også kan se tidligere lukkede saker");

                entity.Property(e => e.InternalId).HasComment("Intern unik id for klienten. Telles fra 1 og oppover per kunde");

                entity.Property(e => e.PersonId).HasComment("Referanse til person i persontabell");

                entity.Property(e => e.RelatedOrganizationDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navn på organisasjon som klienten er tilknyttet");

                entity.Property(e => e.RelatedOrganizationId).HasComment("Id til organisasjon som klienten er tilknyttet, kan f.eks. være id til en skole");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.Status).HasComment("Id til klientens status");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<ClientClientclientgroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_clientclientgroup");

                entity.Property(e => e.ClientClientClientGroupId)
                    .HasColumnName("Client_ClientClientGroupId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ClientGroupId).HasComment("Id til klientgruppen");

                entity.Property(e => e.ClientId).HasComment("Id til klient (klientid)");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");
            });

            modelBuilder.Entity<ClientClientemployeedeviation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_clientemployeedeviation");

                entity.Property(e => e.Access).HasComment("Typen tilgang som er registrert(om man har fått tilgang, eller fjernet tilgang)");

                entity.Property(e => e.AffiliationIdsJson)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ClientClientEmployeeDeviationId)
                    .HasColumnName("Client_ClientEmployeeDeviationId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Kommentar til endringen i tilgang som er lagt inn. F.eks. at tilgang er fjernet fordi saksbehandler er i familie med klient.");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EmployeeId).HasComment("Id til ansatt");

                entity.HasOne(d => d.AccessNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Access)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("client_clientemployeedeviation_ibfk_4");
            });

            modelBuilder.Entity<ClientClientgroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_clientgroup");

                entity.Property(e => e.ClientClientGroupId)
                    .HasColumnName("Client_ClientGroupId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Navn på klientgruppe");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<ClientClientgrouporg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_clientgrouporg");

                entity.Property(e => e.ClientClientGroupOrgId)
                    .HasColumnName("Client_ClientGroupOrgId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ClientGroupId).HasComment("Id til klientgruppen");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CustomerOrganizationId).HasComment("Id til tilhørighet(nivå i organisasjonsstruktur)");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<ClientClientstatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_clientstatus");

                entity.Property(e => e.ClientClientStatusId)
                    .HasColumnName("Client_ClientStatusId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EndStatus).HasComment("Sluttstatus for klienten");

                entity.Property(e => e.EndStatusDate).HasComment("Dato for sluttstatus");

                entity.Property(e => e.Status).HasComment("Klientstatus");

                entity.Property(e => e.StatusDate).HasComment("Dato for klientstatus");

                entity.HasOne(d => d.EndStatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EndStatus)
                    .HasConstraintName("client_clientstatus_ibfk_3");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("client_clientstatus_ibfk_2");
            });

            modelBuilder.Entity<CorrespondenceConnection>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("correspondence_connection");

                entity.Property(e => e.CorrespondenceConnectionId)
                    .HasColumnName("Correspondence_ConnectionId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til korrespondanse");

                entity.Property(e => e.CorrespondenceParentId).HasComment("Id til korrespondanse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.ShipmentReferenceId)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Shipmentid fra KS for den inngående forsendelsen vi har hentet ned");
            });

            modelBuilder.Entity<CorrespondenceCorrespondence>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("correspondence_correspondence");

                entity.Property(e => e.AlwaysOnTopOfList).HasComment("Angir om korrespondansen skal vises i toppen av listen over hendelser på en klient.");

                entity.Property(e => e.ArchiveJobQueueId).HasComment("Id til arkivkø, for bruk ved arkivering");

                entity.Property(e => e.BasedOnDocumentTemplateId).HasComment("Id til malen som brevet er basert på");

                entity.Property(e => e.CaseId).HasComment("Id til saken, dersom registrert på en sak (personsak eller systemsak)");

                entity.Property(e => e.ClientId).HasComment("Id til klienten (er ikke utfylt om det gjelder systemsak)");

                entity.Property(e => e.CopyToClientsJson)
                    .IsUnicode(false)
                    .HasComment("Liste over id til klienter som korrespondansen skal kopieres til");

                entity.Property(e => e.CorrespondenceCategoryRegistryId).HasComment("Id til kategori for korrespondansen");

                entity.Property(e => e.CorrespondenceCorrespondenceId)
                    .HasColumnName("Correspondence_CorrespondenceId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CorrespondenceDate).HasComment("Dato for korrespondanse(sendt/mottatt)");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettet dato");

                entity.Property(e => e.Direction).HasComment("Retning, feks. inngående/utgående");

                entity.Property(e => e.FinishedDate).HasComment("Dato for ferdigstilling av korrespondanse");

                entity.Property(e => e.HistoricMigrationDataJson)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Ved evt. tidligere migrering vises tidligere verdier i tidligere system her");

                entity.Property(e => e.InternalOrWithheld).HasComment("Angir om dette er en intern eller unndratt korrespondanse");

                entity.Property(e => e.InternalOrWithheldType).HasComment("Angir hvilken type inndratt eller unndragelse som gjelder");

                entity.Property(e => e.IsEpj)
                    .HasColumnName("isEPJ")
                    .HasComment("Angir om denne korrespondansen er opprettet av en saksbehandler som er logget med en helserolle (&apos;elektronisk pasient journal&apos;)");

                entity.Property(e => e.NumberOfAttachments).HasComment("Antall vedlegg på korrespondansen");

                entity.Property(e => e.OwnedBy).HasComment("Id til hovedsaksbehandler (ansatt)");

                entity.Property(e => e.ReceivedDate).HasComment("Dato da korrespondansen er mottatt");

                entity.Property(e => e.RegistrationNumber).HasComment("Registreringsnummer (sett i forbindelse med kolonne Year)");

                entity.Property(e => e.SendersRecipients)
                    .IsUnicode(false)
                    .HasComment("Liste over avsendere/mottakere av korrespondanse. Oppført som json-objekt.");

                entity.Property(e => e.SentDate).HasComment("Dato da korrespondansen er sendt");

                entity.Property(e => e.SentForApprovalBy).HasComment("Ansatt som sendte korrespondanse til godkjenning");

                entity.Property(e => e.SentForApprovalDate).HasComment("Dato da korrespondasen er sendt til godkjenning");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Felt for tittel");

                entity.Property(e => e.Year).HasComment("Registreringsår (sett i forbindelse med kolonne RegistrationNumber)");

                entity.HasOne(d => d.DirectionNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Direction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correspondence_correspondence_ibfk_8");

                entity.HasOne(d => d.InternalOrWithheldTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.InternalOrWithheldType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correspondence_correspondence_ibfk_9");
            });

            modelBuilder.Entity<CorrespondenceCorrespondenceattachment>(entity =>
            {
                entity.ToTable("correspondence_correspondenceattachment");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.AttachmentId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("Id til vedlegg");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til korrespondanse");

                entity.Property(e => e.EndDate).HasComment("-");

                entity.Property(e => e.EventElementId).HasComment("Id til hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.EventElementType).HasComment("Type hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.FileData).HasComment("Fil-data");

                entity.Property(e => e.FileId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Id til fil");

                entity.Property(e => e.FilePassword)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Passord til filen");

                entity.Property(e => e.InternalOrWithheld).HasComment("Angir om vedlegget er internt eller inndratt");

                entity.Property(e => e.InternalOrWithheldType).HasComment("Angir hvilken type internt or inndratt");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navn på vedlegg");

                entity.Property(e => e.StartDate).HasComment("-");
            });

            modelBuilder.Entity<CorrespondenceCorrespondencecaseworker>(entity =>
            {
                entity.ToTable("correspondence_correspondencecaseworker");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CaseworkerId).HasComment("Id til saksbehandler(ansatt)");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til korrespondanse");
            });

            modelBuilder.Entity<CorrespondenceDocument>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("correspondence_document");

                entity.Property(e => e.CorrespondenceDocumentId)
                    .HasColumnName("Correspondence_DocumentId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til korrespondanse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.DocumentCommentsJson)
                    .IsUnicode(false)
                    .HasComment("Liste over kommentarer på en korrespondanse, benyttes av saksbehandlere for å samarbeide ved utarbeiding av en korrespondanse.");

                entity.Property(e => e.DocumentText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Tekstfelt for korrespondanse, benyttes under skriving av korrespondanse før den er ferdigstilt og pdf er generert");

                //entity.Property(e => e.FileDataBlob).HasComment("Fildata");

                entity.Property(e => e.FileId)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Id til fil");

                entity.Property(e => e.FileName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navn på fil");

                entity.Property(e => e.IsCk5Template).HasComment("Angir om det er benyttet mal for versjon 5 av CK editor");

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Passord til filen");
            });

            modelBuilder.Entity<CorrespondenceIncomingpost>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("correspondence_incomingpost");

                entity.Property(e => e.AddedDate).HasComment("Dato da den blir lagt inn i listen over innkommende post");

                entity.Property(e => e.CorrespondenceIncomingPostId)
                    .HasColumnName("Correspondence_IncomingPostId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.FileCreatedDate).HasComment("Dato da filen i den innkommende post var laget");

                entity.Property(e => e.FileData)
                    .IsRequired()
                    .HasComment("Selve fildataene om de ikke er lagret i FileDataBlob.");

                entity.Property(e => e.FileDataBlob).HasComment("Fildata for PDF dokument");

                entity.Property(e => e.FileId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Opprinnelig fil-identifikator til dokumentet (ikke i bruk)");

                entity.Property(e => e.FileName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Filnavn");

                entity.Property(e => e.OrganizationNumber)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnummer som dokumentet tilhører");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Opprinnelig passord til dokumentet (ikke i bruk)");

                entity.Property(e => e.SpecialistSystemName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navnet på fagsystemet som dokumentet kommer fra");

                entity.Property(e => e.Status).HasComment("Status");

                entity.Property(e => e.Type).HasComment("Angir hvilken type post det er, f.eks. skannet, svarinn etc.");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correspondence_incomingpost_ibfk_3");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correspondence_incomingpost_ibfk_2");
            });

            modelBuilder.Entity<CorrespondenceIncomingpostattachment>(entity =>
            {
                entity.ToTable("correspondence_incomingpostattachment");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.AttachmentId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("Vedlegg id");

                entity.Property(e => e.FileData).HasComment("Fildata for PDF dokument. Om denne er null er fil lagret i FileDataBlob");

                entity.Property(e => e.FileDataBlob).HasComment("Fildata for PDF dokument. Om denne er null er fil lagret i FileData");

                entity.Property(e => e.FileId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Opprinnelig fil-identifikator til dokumentet (ikke i bruk)");

                entity.Property(e => e.FilePassword)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Opprinnelig passord til dokumentet (ikke i bruk)");

                entity.Property(e => e.IncomingPostId).HasComment("FK");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navn");
            });

            modelBuilder.Entity<CorrespondenceIncomingpostmetadatum>(entity =>
            {
                entity.ToTable("correspondence_incomingpostmetadata");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.Address1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Adressefelt på avsender");

                entity.Property(e => e.Address2)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Adressefelt på avsender");

                entity.Property(e => e.Address3)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Adressefelt på avsender");

                entity.Property(e => e.BirthNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Fødselsnummer på avsender");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("By på avsender");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Land på avsender");

                entity.Property(e => e.Date).HasComment("Dato for dokumentet");

                entity.Property(e => e.IncomingPostId).HasComment("FK");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navn på avsender");

                entity.Property(e => e.OrganizationNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnummer på avsender");

                entity.Property(e => e.PostNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Postnr på avsender");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Tittel");
            });

            modelBuilder.Entity<CustomerOrganization>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("customer_organization");

                entity.Property(e => e.ActivateAssessment).HasComment("Angir om kartlegging er aktivert for denne kunden");

                entity.Property(e => e.ActiveDirectoryDomains)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasComment("Active directory domener");

                entity.Property(e => e.ActiveDirectoryEnabled).HasComment("Om &apos;active directory&apos; er aktivert, benyttet i samband med autentisering");

                entity.Property(e => e.Address)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Adresse");

                entity.Property(e => e.AddressPostCodeRegistryId).HasComment("Id til postnr");

                entity.Property(e => e.BankAccountNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasComment("Kontonummer");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasComment("Kontaktperson (fritekst)");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CrmNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("CRM nummer er et internt id nummer som Visma bruker, alle kunder er registert i et CRM verktøy.");

                entity.Property(e => e.CustomerOrganizationId)
                    .HasColumnName("Customer_OrganizationId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("E-post");

                entity.Property(e => e.IdportalIdentity)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("IDPortalIdentity")
                    .HasComment("Id som benyttes for integrasjon mot ID-porten (benyttes for autentisering)");

                entity.Property(e => e.MunicipalityNumber)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("Kommunenummer");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Navn på organisasjon/organisasjonsnivå");

                entity.Property(e => e.OrgType).HasComment("Type organisasjon");

                entity.Property(e => e.OrganizationCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Kode for organisasjonen");

                entity.Property(e => e.OrganizationNumber)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnummer");

                entity.Property(e => e.ParentOrganizationId).HasComment("Id til organisasjon som ligger over i organisasjonsstrukturen");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefisk for telefonnummer, f.eks. +47");

                entity.Property(e => e.SingleSignOnEnabled).HasComment("Angir om &apos;single sign on&apos; er aktivert");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.VisitAddress)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Besøksadresse");

                entity.Property(e => e.VisitAddressPostCodeRegistryId).HasComment("Id til postnummer for besøksadresse");

                entity.Property(e => e.VismaConnectEnabled).HasComment("Angir om Visma Connect integrasjon er aktivert");

                entity.Property(e => e.VismaConnectIdentity)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Id som benyttes for Visma Connect integrasjon (benyttes for autentisering)");

                entity.HasOne(d => d.OrgTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.OrgType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("customer_organization_ibfk_5");
            });

            modelBuilder.Entity<DocumentDocumenttemplate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("document_documenttemplate");

                entity.Property(e => e.BasedOnDocumentTemplateId).HasComment("Angir om malen er basert på en annen mal");

                entity.Property(e => e.CategoryRegistryId).HasComment("Angir hvilken kategori for valge hendelsestype malen er ment for");

                entity.Property(e => e.Ck5Ready).HasComment("Om malen er forberedt for CK editor versjon 5");

                entity.Property(e => e.Ck5TemplateText)
                    .IsUnicode(false)
                    .HasComment("Maltekst når man benytter CK editor versjon 5");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.DocumentDocumentTemplateId)
                    .HasColumnName("Document_DocumentTemplateId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.EventElementType).HasComment("Angir hvilken type hendelse er malen ment for, f.eks. korrespondanse, journal, avtale etc.");

                entity.Property(e => e.FooterDocumentTemplateId).HasComment("Id til mal for bunntekst");

                entity.Property(e => e.FooterPageSetting).HasComment("Sideinnstillinger for bunntekst");

                entity.Property(e => e.HeaderDocumentTemplateId).HasComment("Id til mal for topptekst");

                entity.Property(e => e.HeaderPageSetting).HasComment("Sideinnstillinger for topptekst");

                entity.Property(e => e.Margin).HasComment("Innstillingre for marger");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Navn på malen");

                entity.Property(e => e.SecondFooterDocumentTemplateId).HasComment("Angir mal for bunntekst for side to og utover");

                entity.Property(e => e.SecondFooterPageSetting).HasComment("Sideinnstillinger for bunntekst for side 2 og utover");

                entity.Property(e => e.SecondHeaderDocumentTemplateId).HasComment("Angir mal for topptekst for side to og utover");

                entity.Property(e => e.SecondHeaderPageSetting).HasComment("Sideinnstillinger for topptekst for side 2 og utover");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.TemplateText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Maltekst");

                entity.Property(e => e.TemplateType).HasComment("Type mal");

                entity.HasOne(d => d.EventElementTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EventElementType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_documenttemplate_ibfk_5");

                entity.HasOne(d => d.FooterPageSettingNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.FooterPageSetting)
                    .HasConstraintName("document_documenttemplate_ibfk_7");

                entity.HasOne(d => d.HeaderPageSettingNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.HeaderPageSetting)
                    .HasConstraintName("document_documenttemplate_ibfk_6");

                entity.HasOne(d => d.MarginNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Margin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_documenttemplate_ibfk_10");

                entity.HasOne(d => d.SecondFooterPageSettingNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.SecondFooterPageSetting)
                    .HasConstraintName("document_documenttemplate_ibfk_9");

                entity.HasOne(d => d.SecondHeaderPageSettingNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.SecondHeaderPageSetting)
                    .HasConstraintName("document_documenttemplate_ibfk_8");

                entity.HasOne(d => d.TemplateTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.TemplateType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_documenttemplate_ibfk_4");
            });

            modelBuilder.Entity<EmployeeEmployee>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_employee");

                entity.Property(e => e.Address)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Adresse");

                entity.Property(e => e.BirthNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Fødselsnummer");

                entity.Property(e => e.CellPhone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Mobilnummer");

                entity.Property(e => e.CellPhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks til mobilnummer");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Dnumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("DNumber")
                    .HasComment("DNummer");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("E-Post adresse");

                entity.Property(e => e.EmployeeEmployeeId)
                    .HasColumnName("Employee_EmployeeId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Fornavn");

                entity.Property(e => e.Initials)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Initialer");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Etternavn");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Mellomnavn");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks til telefonnummer");

                entity.Property(e => e.PostCodeRegistryId).HasComment("Id til postnummer");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<EmployeeEmployeeposition>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_employeeposition");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EmployeeEmployeePositionId)
                    .HasColumnName("Employee_EmployeePositionId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.EmployeeId).HasComment("Id til ansatt");

                entity.Property(e => e.EmployeeNumber)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Ansattnummer");

                entity.Property(e => e.EmploymentPercentage)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("Stillingsprosent");

                entity.Property(e => e.OrganizationId).HasComment("Id til organisasjon stillingen er knytt til");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Stillingens tittel");
            });

            modelBuilder.Entity<EmployeeEmployeeteamrole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_employeeteamrole");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EmployeeEmployeeTeamRoleId)
                    .HasColumnName("Employee_EmployeeTeamRoleId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.EmployeeId).HasComment("Id til ansatt");

                entity.Property(e => e.RoleId).HasComment("Id til rolle");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.TeamId).HasComment("Id til team");
            });

            modelBuilder.Entity<EmployeeRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_role");

                entity.Property(e => e.AccessPoints)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Angir tilgangene for rollen");

                entity.Property(e => e.ApprovalAuthority).HasComment("Angir om ansatte med denne rollen har tilgang til godkjenning");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettet dato");

                entity.Property(e => e.EmployeeRoleId)
                    .HasColumnName("Employee_RoleId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.IsHealthrole).HasComment("Angir om denne rollen er en rolle relatert til helseinformasjon (EPJ)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Navn på rollen");

                entity.Property(e => e.ReasonRequired).HasComment("Bestemmer om man må angi en grunn når man bytter til denne rollen");

                entity.Property(e => e.ReauthenticationRequired).HasComment("Angir om ansatt må autentisere seg på nytt når man bytter til denne rollen");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.UpdatedBy).HasComment("Oppdatert av (ansatt)");
            });

            modelBuilder.Entity<EmployeeTeam>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_team");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EmployeeTeamId)
                    .HasColumnName("Employee_TeamId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Navn på team");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<EmployeeTeamclient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_teamclient");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Date).HasComment("Dato for tilknytningen mellom team og klient");

                entity.Property(e => e.EmployeeTeamClientId)
                    .HasColumnName("Employee_TeamClientId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.TeamId).HasComment("Id til team");
            });

            modelBuilder.Entity<EmployeeTeamclientgroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_teamclientgroup");

                entity.Property(e => e.ClientClientGroupId).HasComment("Id til klientgruppe");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EmployeeTeamClientGroupId)
                    .HasColumnName("Employee_TeamClientGroupId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.TeamId).HasComment("Id til team");
            });

            modelBuilder.Entity<EmployeeTeamorg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("employee_teamorg");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CustomerOrganizationId).HasComment("Id til organisasjonsstruktur(tilhørighet)");

                entity.Property(e => e.EmployeeTeamOrgId)
                    .HasColumnName("Employee_TeamOrgId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.TeamId).HasComment("Id til team");
            });

            modelBuilder.Entity<EnquiryDocument>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("enquiry_document");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.DocumentText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("HTML versjon av dokument");

                entity.Property(e => e.EnquiryDocumentId)
                    .HasColumnName("Enquiry_DocumentId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.EnquiryId).HasComment("FK");
            });

            modelBuilder.Entity<EnquiryEnquiry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("enquiry_enquiry");

                entity.Property(e => e.AdditionalCategoryRegistryIdsJson)
                    .IsUnicode(false)
                    .HasComment("Tilleggskategori");

                entity.Property(e => e.AffiliationId).HasComment("Id for tilhørighet");

                entity.Property(e => e.ArchiveJobQueueId).HasComment("Id til arkivjobb i arkiv køen");

                entity.Property(e => e.CaseId).HasComment("Id til saken");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.ContactInfo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Kontaktinformasjon");

                entity.Property(e => e.CorrespondenceId).HasComment("Tilhørende inngående korrespondanse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettetdato");

                entity.Property(e => e.EnquiryEnquiryId)
                    .HasColumnName("Enquiry_EnquiryId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.FinishedDate).HasComment("Ferdig dato");

                entity.Property(e => e.InternalOrWithheld).HasComment("Angir om dette er en intern eller unndratt hendendelse");

                entity.Property(e => e.InternalOrWithheldType).HasComment("Angir hvilken type inndratt eller unndragelse som gjelder");

                entity.Property(e => e.IsImportant).HasComment("Markering om viktig");

                entity.Property(e => e.IsInitialEnquiry).HasComment("Er initiell hendelse på en sak");

                entity.Property(e => e.MainCategoryRegistryId).HasComment("Hovedkategori");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Kallenavn på klienten");

                entity.Property(e => e.OrganizationId).HasComment("Id til Organisasjon");

                entity.Property(e => e.ReportedDate).HasComment("Rapportertdato");

                entity.Property(e => e.ReporterTypeRegistryId).HasComment("Type melder");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Tittel");
            });

            modelBuilder.Entity<EnquiryEnquirycaseworker>(entity =>
            {
                entity.ToTable("enquiry_enquirycaseworker");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CaseworkerId).HasComment("Saksbehandler");

                entity.Property(e => e.EnquiryId).HasComment("FK");
            });

            modelBuilder.Entity<EnumAppointmentparticipanttype>(entity =>
            {
                entity.ToTable("enum_appointmentparticipanttype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumApprovalstatus>(entity =>
            {
                entity.ToTable("enum_approvalstatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumAssessmentevaluationstatus>(entity =>
            {
                entity.ToTable("enum_assessmentevaluationstatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumAssessmentevaluationtype>(entity =>
            {
                entity.ToTable("enum_assessmentevaluationtype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumCasestatus>(entity =>
            {
                entity.ToTable("enum_casestatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumCasetype>(entity =>
            {
                entity.ToTable("enum_casetype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumClientemployeedeviationaccesspoint>(entity =>
            {
                entity.ToTable("enum_clientemployeedeviationaccesspoint");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumClientstatus>(entity =>
            {
                entity.ToTable("enum_clientstatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumCorrespondencedirection>(entity =>
            {
                entity.ToTable("enum_correspondencedirection");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumEmployeepreferredlanguage>(entity =>
            {
                entity.ToTable("enum_employeepreferredlanguage");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumEventelementtype>(entity =>
            {
                entity.ToTable("enum_eventelementtype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumFollowuprecipienttype>(entity =>
            {
                entity.ToTable("enum_followuprecipienttype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumGender>(entity =>
            {
                entity.ToTable("enum_gender");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumHeaderfooterpagesetting>(entity =>
            {
                entity.ToTable("enum_headerfooterpagesetting");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumIncomingpoststatus>(entity =>
            {
                entity.ToTable("enum_incomingpoststatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumIncomingposttype>(entity =>
            {
                entity.ToTable("enum_incomingposttype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumInternalorwithheldtype>(entity =>
            {
                entity.ToTable("enum_internalorwithheldtype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumOperationtype>(entity =>
            {
                entity.ToTable("enum_operationtype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumOrgtype>(entity =>
            {
                entity.ToTable("enum_orgtype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumPdfpagemargin>(entity =>
            {
                entity.ToTable("enum_pdfpagemargin");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumSecondheaderfooterpagesetting>(entity =>
            {
                entity.ToTable("enum_secondheaderfooterpagesetting");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumSenderrecipientkind>(entity =>
            {
                entity.ToTable("enum_senderrecipientkind");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumSmsstatus>(entity =>
            {
                entity.ToTable("enum_smsstatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumSvarutstatus>(entity =>
            {
                entity.ToTable("enum_svarutstatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnumTemplatetype>(entity =>
            {
                entity.ToTable("enum_templatetype");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Followup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("followup");

                entity.Property(e => e.CaseId).HasComment("Id til sak");

                entity.Property(e => e.CaseNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Saksnummer");

                entity.Property(e => e.CaseStatus).HasComment("Saksstatus");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Klientens navn");

                entity.Property(e => e.ConnectionType).HasComment("Angir om oppfølging er tilknyttet klient, sak, hendelse, eller ingenting");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedByName)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Opprettet av (navn på ansatt)");

                entity.Property(e => e.CreatedDate).HasComment("Opprettet dato");

                entity.Property(e => e.Deadline).HasComment("Dato for frist");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("Beskrivelse");

                entity.Property(e => e.EventElementId).HasComment("Id til hendelse");

                entity.Property(e => e.EventElementType).HasComment("Type hendelse (korrespondanse, journal, avtale etc.");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.OrganizationId).HasComment("Id til organisasjon");

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Navn på organisasjon");

                entity.Property(e => e.OwnerChangeDate).HasComment("Dato hvis og når hovedsaksbehandler er byttet til en annen ansatt");

                entity.Property(e => e.OwnerId).HasComment("Hovedsaksbehandler(ansatt)");

                entity.Property(e => e.OwnerName)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn på hovedsaksbehandler");

                entity.Property(e => e.PersonId).HasComment("Id til person");

                entity.Property(e => e.SmsId).HasComment("Id til SMS som evt har opprette oppfølgingen (skjer feks om en sms feiler slik at noen kan følge opp feilen)");

                entity.Property(e => e.Status).HasComment("Status på oppfølgingen");

                entity.Property(e => e.StatusChangeDate).HasComment("Dato for når status på oppfølgingen er endret");

                entity.Property(e => e.SystemGeneratedType).HasComment("Angir om denne oppfølgingen ble generert aut. av systemet.");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("Tittel");

                entity.HasOne(d => d.EventElementTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EventElementType)
                    .HasConstraintName("followup_ibfk_6");
            });

            modelBuilder.Entity<FollowupMarkedreademployee>(entity =>
            {
                entity.ToTable("followup_markedreademployee");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.EmployeeId).HasComment("Id til ansatt");

                entity.Property(e => e.FollowUpId).HasComment("Id to followup");
            });

            modelBuilder.Entity<FollowupRecipient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("followup_recipients");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.FollowUpId).HasComment("Id til Oppfølging");

                entity.Property(e => e.Id).HasComment("Unik primærnøkkel");

                entity.Property(e => e.RecipientId).HasComment("Id til mottaker, må ses i sammenheng av kolonnen Type");

                entity.Property(e => e.RecipientInitials)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Initialer på mottaker");

                entity.Property(e => e.RecipientName)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasComment("Navn på mottaker");

                entity.Property(e => e.Type).HasComment("Angir om oppfølgingen tilhører en ansatt eller et team");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("followup_recipients_ibfk_3");
            });

            modelBuilder.Entity<JournalDocument>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("journal_document");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.DocumentCommentsJson)
                    .IsUnicode(false)
                    .HasComment("Liste over kommentarer til dokumenttekst");

                entity.Property(e => e.DocumentText)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasComment("Dokument tekst, basis for pdf generering");

                //entity.Property(e => e.FileDataBlob).HasComment("Fildata når konvertert til pdf");

                entity.Property(e => e.FileId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IsCk5Template).HasComment("Id til mal");

                entity.Property(e => e.JournalDocumentId)
                    .HasColumnName("Journal_DocumentId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.JournalId).HasComment("Id til journal");

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JournalJournal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("journal_journal");

                entity.Property(e => e.ArchiveJobQueueId).HasComment("Id til arkivjobb i arkiv køen");

                entity.Property(e => e.BasedOnDocumentTemplateId).HasComment("Id til dokumentmal");

                entity.Property(e => e.CaseId).HasComment("Id til saken");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.CopyToClientsJson)
                    .IsUnicode(false)
                    .HasComment("Id over klienter(søsken) som journalen er kopiert til");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.CreatedDate).HasComment("Opprettetdato");

                entity.Property(e => e.FinishedDate).HasComment("Ferdig dato");

                entity.Property(e => e.HistoricMigrationDataJson)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("Ved evt. tidligere migrering vises tidligere verdier i tidligere system her");

                entity.Property(e => e.InternalOrWithheld).HasComment("Angir om dette er en intern eller unndratt korrespondanse");

                entity.Property(e => e.InternalOrWithheldType).HasComment("Angir hvilken type inndratt eller unndragelse som gjelder");

                entity.Property(e => e.IsEpj)
                    .HasColumnName("isEPJ")
                    .HasComment("Angir om denne journalen er opprettet av en ansatt som er logget med en helserolle");

                entity.Property(e => e.IsHealthRelated).HasComment("Angir om denne journalen inneholder helserelaterteopplysninger");

                entity.Property(e => e.JournalCategoryRegistryId).HasComment("Id til journalkategori");

                entity.Property(e => e.JournalDate).HasComment("Journaldato");

                entity.Property(e => e.JournalJournalId)
                    .HasColumnName("Journal_JournalId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.OwnedBy).HasComment("Hovedsaksbehandler(ansatt) på journalen");

                entity.Property(e => e.RegistrationNumber).HasComment("Registreringsnummer");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Tittel");

                entity.HasOne(d => d.InternalOrWithheldTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.InternalOrWithheldType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("journal_journal_ibfk_7");
            });

            modelBuilder.Entity<JournalJournalcaseworker>(entity =>
            {
                entity.ToTable("journal_journalcaseworker");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CaseworkerId).HasComment("Id til ansatt");

                entity.Property(e => e.JournalId).HasComment("Id til journal");
            });

            modelBuilder.Entity<NotificationSm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("notification_sms");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EventElementDate).HasComment("Dato fra aktuell hendelse");

                entity.Property(e => e.EventElementId).HasComment("Id til hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.EventElementType).HasComment("Type hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.ExternalSmsId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("Id fra operatør når sms er sendt");

                entity.Property(e => e.NotificationReasonId).HasComment("For sms&apos;er knytt til Avtaler(Appointments) så beskriver dette status årsak til sms ble opprettet betyr, 1=Opprettet, 2=24 timer før avtalen, 3=Avlyst, 4=Oppdatert");

                entity.Property(e => e.NotificationSmsId)
                    .HasColumnName("Notification_SmsId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.Phone)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Telefon sms sendes til");

                entity.Property(e => e.RecipientId).HasComment("Id på mottaker (Person)");

                entity.Property(e => e.SendDate).HasComment("SMS sendt dato (sendt til operatør)");

                entity.Property(e => e.SmsSender)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Avsender-tekst på sms");

                entity.Property(e => e.SmsText)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Teksten på sms&apos;en");

                entity.Property(e => e.Status).HasComment("Status på sms-sending");

                entity.Property(e => e.StatusMessage)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasComment("Status ved feil på sending av sms");

                entity.HasOne(d => d.EventElementTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EventElementType)
                    .HasConstraintName("notification_sms_ibfk_3");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("notification_sms_ibfk_2");
            });

            modelBuilder.Entity<OrganizationOrganization>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("organization_organization");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Adresse");

                entity.Property(e => e.AddressPostCodeRegistryId).HasComment("Id til postnummer for adresse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("E-Postadresse");

                entity.Property(e => e.MunicipalityRegistryId).HasComment("Id til kommunenr");

                entity.Property(e => e.Number)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnummer");

                entity.Property(e => e.NumberForCorrespondence)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasComment("Organisasjonsnummer brukt i elektronisk kommunikasjon(sikker post/svarut)");

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComment("Navn på organisasjonen");

                entity.Property(e => e.OrganizationOrganizationId)
                    .HasColumnName("Organization_OrganizationId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.OrganizationType).HasComment("Id til Type organisasjon (grunnregister)");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode for telefonnummer");

                entity.Property(e => e.SecondaryPhone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Sekundært telefonnummer");

                entity.Property(e => e.SecondaryPhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode for sekundært telefonnummer");

                entity.Property(e => e.ShipmentTypeRegistryId).HasComment("Id til SvarUt forsendelsestype som skal benyttes ved sending til SvarUt for denne organisasjonen");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.SvarUtIncludeBirthNumber).HasComment("Angir om man skal inkludere fødselsnummer ved SvarUt forsendelser til denne organisasjonen");

                entity.Property(e => e.VisitAddress)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Besøksadresse");

                entity.Property(e => e.VisitAddressPostCodeRegistryId).HasComment("Id til postnummer for besøksadresse");
            });

            modelBuilder.Entity<OrganizationOrganizationperson>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("organization_organizationperson");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("E-Post for denne person tilknytt oppgitt organisasjon");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasComment("Notat for denne person tilknytt oppgitt organisasjon");

                entity.Property(e => e.OrganizationId).HasComment("Id til organisasjon");

                entity.Property(e => e.OrganizationOrganizationPersonId)
                    .HasColumnName("Organization_OrganizationPersonId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.PersonId).HasComment("Id til person");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer for denne person tilknytt oppgitt organisasjon");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode til telefonnummer for denne person tilknytt oppgitt organisasjon");

                entity.Property(e => e.RoleRegistryId).HasComment("Id til rolle personen har i denne organisasjonen (rektor, lærer, lege, psykolog etc.");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<PersonNetworkpersonrole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("person_networkpersonrole");

                entity.Property(e => e.CaseId).HasComment("Id til sak dersom nettverksperson er knytt til person vedrørende en bestemt sak");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.IsClosestRelation).HasComment("Angir om det er en nær relasjon");

                entity.Property(e => e.IsCopyOfPost).HasComment("Om vedkommende skal få kopi av av post");

                entity.Property(e => e.Notes)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasComment("Id til organisasjon dersom nettverksperson er tilknyttet en bestemt organisasjon (skole, legesenter, kommune, etc.");

                entity.Property(e => e.OrganizationPersonId).HasComment("Id til organisasjonsperson, når en person er knytt til en organisasjon og inneholder info relatert til denne personens\\u0020\\u0020som en ansatt i denne organisasjonen, type telefonnr, epost, adresse etc.");

                entity.Property(e => e.PersonId).HasComment("Id til person som er &apos;nettverksperson&apos; til klienten");

                entity.Property(e => e.PersonNetworkPersonRoleId)
                    .HasColumnName("Person_NetworkPersonRoleId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.RelatedClientId).HasComment("Id til klient");

                entity.Property(e => e.RoleRegistryId).HasComment("Id til rollen for nettverksperson (lærer, lege, psykolog etc.)");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<PersonPerson>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("person_person");

                entity.Property(e => e.Address)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Adresse");

                entity.Property(e => e.BirthNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("Fødselsnummer");

                entity.Property(e => e.Birthdate).HasComment("Fødselsdato");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.DeathDate).HasComment("Dato for død");

                entity.Property(e => e.Dnumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("DNumber")
                    .HasComment("Dnummer");

                entity.Property(e => e.Dufnumber)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("DUFNumber")
                    .HasComment("DUFnummer");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasComment("E-Postadresse");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Fornavn");

                entity.Property(e => e.Gender).HasComment("Kjønn");

                entity.Property(e => e.Hnumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("HNumber")
                    .IsFixedLength()
                    .HasComment("HNummer");

                entity.Property(e => e.IsAgainstSmsNotification).HasComment("Angir om personen ikke ønsker sms varslinger");

                entity.Property(e => e.IsClient).HasComment("Angir om personen er en klient og dermed har en rad i CLIENT_CLIENT tabellen også");

                entity.Property(e => e.IsHighlyRestricted).HasComment("Angir om det er ekstra hemmelig adresse");

                entity.Property(e => e.IsSecretAddress).HasComment("Angir om det er hemmelig adresse");

                entity.Property(e => e.IsSecretVisitingAddress).HasComment("Angir om besøksadressen er hemmelig");

                entity.Property(e => e.LanguageRegistryId).HasComment("Id til personens språk");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Etternavn");

                entity.Property(e => e.LivingWithId).HasComment("Id til personen som personen bor hos");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Mellomnavn");

                entity.Property(e => e.MunicipalityRegistryId).HasComment("Id til kommune for adresse");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Kallenavn/kortnavn");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasComment("Notat");

                entity.Property(e => e.PersonPersonId)
                    .HasColumnName("Person_PersonId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Telefonnummer");

                entity.Property(e => e.PhonePrefix)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode for telefonnummer");

                entity.Property(e => e.PostCodeRegistryId).HasComment("Id til postnummer");

                entity.Property(e => e.SecondaryPhone)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasComment("Sekundært telefonnummer");

                entity.Property(e => e.SecondaryPhonePrefix)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("Prefiks/landskode til sekundært telefonnummer");

                entity.Property(e => e.SecretAddress)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Inneholder addresse, hvis personen har hemmelig besøksadresse");

                entity.Property(e => e.SecretPostCodeRegistryId).HasComment("Inneholder id til postnummer for adresse, hvis personen har hemmelig adresse");

                entity.Property(e => e.SecretVisitingAddress)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Inneholder besøksaddresse, hvis personen har hemmelig besøksadresse");

                entity.Property(e => e.SecretVisitingAddressPostCodeRegistryId).HasComment("Inneholder id til postnummer for besøksadresse, hvis personen har hemmelig besøksadresse");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");

                entity.Property(e => e.VisitingAddress)
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasComment("Besøksadresse");

                entity.Property(e => e.VisitingAddressMunicipalityRegistryId).HasComment("Id til kommune for besøksadresse");

                entity.Property(e => e.VisitingAddressPostCodeRegistryId).HasComment("Id til postnummer for besøksadresse");

                entity.HasOne(d => d.GenderNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Gender)
                    .HasConstraintName("person_person_ibfk_10");
            });

            modelBuilder.Entity<PersonPersoncitizenshipnationality>(entity =>
            {
                entity.ToTable("person_personcitizenshipnationality");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.CitizenshipNationalityRegistryId).HasComment("Id til nasjonalitet");

                entity.Property(e => e.PersonId).HasComment("Id til person");
            });

            modelBuilder.Entity<PersonPersonrole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("person_personrole");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.FamilyroleRegistryId).HasComment("Id til familierollen");

                entity.Property(e => e.HasParentalRights).HasComment("Har personen foreldrerett");

                entity.Property(e => e.IsClosestRelation).HasComment("Er personen nærmeste relasjon");

                entity.Property(e => e.IsCopyOfPost).HasComment("Om vedkommende skal få kopi av av post");

                entity.Property(e => e.Notes)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId).HasComment("Id til person");

                entity.Property(e => e.PersonPersonRoleId)
                    .HasColumnName("Person_PersonRoleId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.RelatedPersonId).HasComment("Id til personen som denne personen har en rolle til (PersonId er &apos;onkel&apos; til RelatedPersonId)");

                entity.Property(e => e.StartDate).HasComment("Aktiv fra");

                entity.Property(e => e.StopDate).HasComment("Aktiv til");

                entity.Property(e => e.StoppedBy).HasComment("Deaktivert av");
            });

            modelBuilder.Entity<ReferralReferral>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("referral_referral");

                entity.Property(e => e.CaseId).HasComment("Id til sak");

                entity.Property(e => e.CorrespondenceId).HasComment("Id til tilhørende inngående korrespondanse");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("Beskrivelse");

                entity.Property(e => e.ForwardedToOrganizationTypeRegistryId).HasComment("Id til organisasjon som henvisningen evt. er videresendt til");

                entity.Property(e => e.IntakeMeetingConclusionRegistryId).HasComment("Id til konklusjon av inntaksmøte");

                entity.Property(e => e.IntakeMeetingDate).HasComment("Dato for inntaksmøte");

                entity.Property(e => e.ReasonRegistryId).HasComment("Id til begrunnelse for henvisning");

                entity.Property(e => e.ReceivedDate).HasComment("Mottattdato");

                entity.Property(e => e.ReferralReferralId)
                    .HasColumnName("Referral_ReferralId")
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ReferrerClientId).HasComment("Id til klienten henvisningen er knytt til");

                entity.Property(e => e.ReferrerOrganizationId).HasComment("Id til organisasjon når henvisningen er knytt til en organisasjon");
            });

            modelBuilder.Entity<ReferralReferralparent>(entity =>
            {
                entity.ToTable("referral_referralparent");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unik primærnøkkel");

                entity.Property(e => e.ParentId).HasComment("Id til Person");

                entity.Property(e => e.ReferralId).HasComment("Id til Henvisning");
            });

            modelBuilder.Entity<TimeregistrationEventelement>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("timeregistration_eventelement");

                entity.Property(e => e.CaseId).HasComment("Id til sak");

                entity.Property(e => e.CategoryRegistryId).HasComment("Id til Kategori");

                entity.Property(e => e.ClientId).HasComment("Id til klient");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.EventElementId).HasComment("Id til hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.EventElementType).HasComment("Type hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.FinishedDate).HasComment("Ferdigstilt dato");

                entity.Property(e => e.Minutes).HasComment("Tid angitt i minutt (hentes fra TimeRegistration.Minutes ved lagring)");

                entity.Property(e => e.OrganizationId).HasComment("Id til organisasjon");

                entity.Property(e => e.TimeRegistrationEventElementId)
                    .HasColumnName("TimeRegistration_EventElementId")
                    .HasComment("Unik primærnøkkel");

                entity.HasOne(d => d.EventElementTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EventElementType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("timeregistration_eventelement_ibfk_4");
            });

            modelBuilder.Entity<TimeregistrationTimeregistration>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("timeregistration_timeregistration");

                entity.Property(e => e.CategoryRegistryId).HasComment("Id til aktuell kategori");

                entity.Property(e => e.CreatedBy).HasComment("Opprettet av");

                entity.Property(e => e.Date).HasComment("Dato når det gjelder fra");

                entity.Property(e => e.EventElementType).HasComment("Type hendelse, kan være korrespondanse, journal etc.");

                entity.Property(e => e.Minutes).HasComment("Tid angitt i minutt");

                entity.Property(e => e.TimeRegistrationTimeRegistrationId)
                    .HasColumnName("TimeRegistration_TimeRegistrationId")
                    .HasComment("Unik primærnøkkel");

                entity.HasOne(d => d.EventElementTypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EventElementType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("timeregistration_timeregistration_ibfk_3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
