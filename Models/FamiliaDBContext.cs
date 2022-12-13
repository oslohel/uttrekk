#region Usings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
#endregion

namespace UttrekkFamilia.Models
{
    public partial class FamiliaDBContext : DbContext
    {
        private readonly string ConnectionString;

        public FamiliaDBContext(string connectionString)
        {
            ConnectionString = connectionString;
            Database.SetCommandTimeout(300);
        }

        public FamiliaDBContext(DbContextOptions<FamiliaDBContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(300);
        }

        public virtual DbSet<Agressolev> Agressolevs { get; set; }
        public virtual DbSet<Dbmessage> Dbmessages { get; set; }
        public virtual DbSet<FaAarloepenr> FaAarloepenrs { get; set; }
        public virtual DbSet<FaAktivitet> FaAktivitets { get; set; }
        public virtual DbSet<FaAktivitetskode> FaAktivitetskodes { get; set; }
        public virtual DbSet<FaAldersgrupper> FaAldersgruppers { get; set; }
        public virtual DbSet<FaAldersskala> FaAldersskalas { get; set; }
        public virtual DbSet<FaAppointment> FaAppointments { get; set; }
        public virtual DbSet<FaArbeidsgiveravgift> FaArbeidsgiveravgifts { get; set; }
        public virtual DbSet<FaArbeidsgiversone> FaArbeidsgiversones { get; set; }
        public virtual DbSet<FaAvvikslogg> FaAvviksloggs { get; set; }
        public virtual DbSet<FaBegrFunksjoner> FaBegrFunksjoners { get; set; }
        public virtual DbSet<FaBehandlingsmal> FaBehandlingsmals { get; set; }
        public virtual DbSet<FaBehandlingsmallinje> FaBehandlingsmallinjes { get; set; }
        public virtual DbSet<FaBetalingAarsak> FaBetalingAarsaks { get; set; }
        public virtual DbSet<FaBetalinger> FaBetalingers { get; set; }
        public virtual DbSet<FaBetalingskategorier> FaBetalingskategoriers { get; set; }
        public virtual DbSet<FaBetalingsplaner> FaBetalingsplaners { get; set; }
        public virtual DbSet<FaBudsjett> FaBudsjetts { get; set; }
        public virtual DbSet<FaDato> FaDatos { get; set; }
        public virtual DbSet<FaDbversjon> FaDbversjons { get; set; }
        public virtual DbSet<FaDelmaal> FaDelmaals { get; set; }
        public virtual DbSet<FaDistrikt> FaDistrikts { get; set; }
        public virtual DbSet<FaDistriktaarloepenr> FaDistriktaarloepenrs { get; set; }
        public virtual DbSet<FaDistriktloepenrserier> FaDistriktloepenrseriers { get; set; }
        public virtual DbSet<FaDokumenter> FaDokumenters { get; set; }
        public virtual DbSet<FaEier> FaEiers { get; set; }
        public virtual DbSet<FaEksterneParametre> FaEksterneParametres { get; set; }
        public virtual DbSet<FaEngasjementsavtale> FaEngasjementsavtales { get; set; }
        public virtual DbSet<FaEngasjementslinjer> FaEngasjementslinjers { get; set; }
        public virtual DbSet<FaEngasjementsplan> FaEngasjementsplans { get; set; }
        public virtual DbSet<FaFlytvidersendstatus> FaFlytvidersendstatuses { get; set; }
        public virtual DbSet<FaForbindelser> FaForbindelsers { get; set; }
        public virtual DbSet<FaForbindelsesadresser> FaForbindelsesadressers { get; set; }
        public virtual DbSet<FaForbindelsestyper> FaForbindelsestypers { get; set; }
        public virtual DbSet<FaFriTiltakstype> FaFriTiltakstypes { get; set; }
        public virtual DbSet<FaFristoversittelser> FaFristoversittelsers { get; set; }
        public virtual DbSet<FaGenerellsak> FaGenerellsaks { get; set; }
        public virtual DbSet<FaHuskelapp> FaHuskelapps { get; set; }
        public virtual DbSet<FaInntutg> FaInntutgs { get; set; }
        public virtual DbSet<FaInntutgtype> FaInntutgtypes { get; set; }
        public virtual DbSet<FaInteresser> FaInteressers { get; set; }
        public virtual DbSet<FaJournal> FaJournals { get; set; }
        public virtual DbSet<FaJournaltype> FaJournaltypes { get; set; }
        public virtual DbSet<FaKilometergodtgjoerelse> FaKilometergodtgjoerelses { get; set; }
        public virtual DbSet<FaKkoder> FaKkoders { get; set; }
        public virtual DbSet<FaKlient> FaKlients { get; set; }
        public virtual DbSet<FaKlientSbhHistorikk> FaKlientSbhHistorikks { get; set; }
        public virtual DbSet<FaKlientadresser> FaKlientadressers { get; set; }
        public virtual DbSet<FaKlientgrupper> FaKlientgruppers { get; set; }
        public virtual DbSet<FaKlientinteresser> FaKlientinteressers { get; set; }
        public virtual DbSet<FaKlientplassering> FaKlientplasserings { get; set; }
        public virtual DbSet<FaKlienttilknytning> FaKlienttilknytnings { get; set; }
        public virtual DbSet<FaKmsatser> FaKmsatsers { get; set; }
        public virtual DbSet<FaKodeverk> FaKodeverks { get; set; }
        public virtual DbSet<FaKommuner> FaKommuners { get; set; }
        public virtual DbSet<FaKontaktpersoner> FaKontaktpersoners { get; set; }
        public virtual DbSet<FaKontoer> FaKontoers { get; set; }
        public virtual DbSet<FaKontoplan> FaKontoplans { get; set; }
        public virtual DbSet<FaKontotiltakstype> FaKontotiltakstypes { get; set; }
        public virtual DbSet<FaKsIdenter> FaKsIdenters { get; set; }
        public virtual DbSet<FaKssatser> FaKssatsers { get; set; }
        public virtual DbSet<FaKvello> FaKvellos { get; set; }
        public virtual DbSet<FaKvelloAnsvarlig> FaKvelloAnsvarligs { get; set; }
        public virtual DbSet<FaKvelloBeskFaktorer> FaKvelloBeskFaktorers { get; set; }
        public virtual DbSet<FaKvelloPersoner> FaKvelloPersoners { get; set; }
        public virtual DbSet<FaKvelloRiskFaktorer> FaKvelloRiskFaktorers { get; set; }
        public virtual DbSet<FaKvelloTiltak> FaKvelloTiltaks { get; set; }
        public virtual DbSet<FaLoennstrinn> FaLoennstrinns { get; set; }
        public virtual DbSet<FaLoennstrinnsat> FaLoennstrinnsats { get; set; }
        public virtual DbSet<FaLoepenr> FaLoepenrs { get; set; }
        public virtual DbSet<FaLogg> FaLoggs { get; set; }
        public virtual DbSet<FaLoggNoark> FaLoggNoarks { get; set; }
        public virtual DbSet<FaLogglinjer> FaLogglinjers { get; set; }
        public virtual DbSet<FaLovtekst> FaLovteksts { get; set; }
        public virtual DbSet<FaLovtekstKombinasjoner> FaLovtekstKombinasjoners { get; set; }
        public virtual DbSet<FaMarkedclientelement> FaMarkedclientelements { get; set; }
        public virtual DbSet<FaMedarbeidere> FaMedarbeideres { get; set; }
        public virtual DbSet<FaMedarbeiderinteresser> FaMedarbeiderinteressers { get; set; }
        public virtual DbSet<FaMeldinger> FaMeldingers { get; set; }
        public virtual DbSet<FaMeldingerSlettet> FaMeldingerSlettets { get; set; }
        public virtual DbSet<FaMerkantilfil> FaMerkantilfils { get; set; }
        public virtual DbSet<FaNasjoner> FaNasjoners { get; set; }
        public virtual DbSet<FaPlantiltak> FaPlantiltaks { get; set; }
        public virtual DbSet<FaPlantype> FaPlantypes { get; set; }
        public virtual DbSet<FaPostadresser> FaPostadressers { get; set; }
        public virtual DbSet<FaPostjournal> FaPostjournals { get; set; }
        public virtual DbSet<FaPostjournalkopitil> FaPostjournalkopitils { get; set; }
        public virtual DbSet<FaProsjekt> FaProsjekts { get; set; }
        public virtual DbSet<FaProsjektaktivitet> FaProsjektaktivitets { get; set; }
        public virtual DbSet<FaProsjektdeltEk> FaProsjektdeltEks { get; set; }
        public virtual DbSet<FaProsjektdeltInt> FaProsjektdeltInts { get; set; }
        public virtual DbSet<FaProsjektevaluering> FaProsjektevaluerings { get; set; }
        public virtual DbSet<FaProsjekttype> FaProsjekttypes { get; set; }
        public virtual DbSet<FaRefusjoner> FaRefusjoners { get; set; }
        public virtual DbSet<FaRefusjonskrav> FaRefusjonskravs { get; set; }
        public virtual DbSet<FaRekvisisjoner> FaRekvisisjoners { get; set; }
        public virtual DbSet<FaRoder> FaRoders { get; set; }
        public virtual DbSet<FaSaksbehKlient> FaSaksbehKlients { get; set; }
        public virtual DbSet<FaSaksbehKlientgrupper> FaSaksbehKlientgruppers { get; set; }
        public virtual DbSet<FaSaksbehandlere> FaSaksbehandleres { get; set; }
        public virtual DbSet<FaSaksjournal> FaSaksjournals { get; set; }
        public virtual DbSet<FaSakstype> FaSakstypes { get; set; }
        public virtual DbSet<FaSbhSetting> FaSbhSettings { get; set; }
        public virtual DbSet<FaSetting> FaSettings { get; set; }
        public virtual DbSet<FaSoeker> FaSoekers { get; set; }
        public virtual DbSet<FaSosialeavgifter> FaSosialeavgifters { get; set; }
        public virtual DbSet<FaSsbStatistikk> FaSsbStatistikks { get; set; }
        public virtual DbSet<FaSsbbegrunnelser> FaSsbbegrunnelsers { get; set; }
        public virtual DbSet<FaStatFylkesmann1> FaStatFylkesmann1s { get; set; }
        public virtual DbSet<FaStatFylkesmann2sum> FaStatFylkesmann2sums { get; set; }
        public virtual DbSet<FaStatFylkesmannK2Sum> FaStatFylkesmannK2Sums { get; set; }
        public virtual DbSet<FaStatFylkesmannKtrskjema> FaStatFylkesmannKtrskjemas { get; set; }
        public virtual DbSet<FaStatSsb1> FaStatSsb1s { get; set; }
        public virtual DbSet<FaStatSsb2> FaStatSsb2s { get; set; }
        public virtual DbSet<FaStatTabell22> FaStatTabell22s { get; set; }
        public virtual DbSet<FaStatTabell22sum> FaStatTabell22sums { get; set; }
        public virtual DbSet<FaStatTabell23> FaStatTabell23s { get; set; }
        public virtual DbSet<FaStatTabell23G> FaStatTabell23Gs { get; set; }
        public virtual DbSet<FaStatTabell23sum> FaStatTabell23sums { get; set; }
        public virtual DbSet<FaStatTabell241> FaStatTabell241s { get; set; }
        public virtual DbSet<FaStatTabell241G> FaStatTabell241Gs { get; set; }
        public virtual DbSet<FaStatTabell241b> FaStatTabell241bs { get; set; }
        public virtual DbSet<FaStatTabell241bRapp> FaStatTabell241bRapps { get; set; }
        public virtual DbSet<FaStatTabell241bSum> FaStatTabell241bSums { get; set; }
        public virtual DbSet<FaStatTabell241sum> FaStatTabell241sums { get; set; }
        public virtual DbSet<FaStatTabell242> FaStatTabell242s { get; set; }
        public virtual DbSet<FaStatTabell242G> FaStatTabell242Gs { get; set; }
        public virtual DbSet<FaStatTabell242Ttperioder> FaStatTabell242Ttperioders { get; set; }
        public virtual DbSet<FaStatTabell242sum> FaStatTabell242sums { get; set; }
        public virtual DbSet<FaStatTabell243> FaStatTabell243s { get; set; }
        public virtual DbSet<FaStatTabell243G> FaStatTabell243Gs { get; set; }
        public virtual DbSet<FaStatTabell243sum> FaStatTabell243sums { get; set; }
        public virtual DbSet<FaStatTabell25> FaStatTabell25s { get; set; }
        public virtual DbSet<FaStatTabell25sum> FaStatTabell25sums { get; set; }
        public virtual DbSet<FaStatTabell26> FaStatTabell26s { get; set; }
        public virtual DbSet<FaStatTabell26G> FaStatTabell26Gs { get; set; }
        public virtual DbSet<FaStatTabell26sum> FaStatTabell26sums { get; set; }
        public virtual DbSet<FaStatsborgerskapkonto> FaStatsborgerskapkontos { get; set; }
        public virtual DbSet<FaSvarinnLogg> FaSvarinnLoggs { get; set; }
        public virtual DbSet<FaTekstmaler> FaTekstmalers { get; set; }
        public virtual DbSet<FaTekstmaltyper> FaTekstmaltypers { get; set; }
        public virtual DbSet<FaTeller> FaTellers { get; set; }
        public virtual DbSet<FaTilgangsgrVindu> FaTilgangsgrVindus { get; set; }
        public virtual DbSet<FaTilgangsgrupper> FaTilgangsgruppers { get; set; }
        public virtual DbSet<FaTilstand> FaTilstands { get; set; }
        public virtual DbSet<FaTiltak> FaTiltaks { get; set; }
        public virtual DbSet<FaTiltakGmltype> FaTiltakGmltypes { get; set; }
        public virtual DbSet<FaTiltakUiKonto> FaTiltakUiKontos { get; set; }
        public virtual DbSet<FaTiltaksevaluering> FaTiltaksevaluerings { get; set; }
        public virtual DbSet<FaTiltakslinjer> FaTiltakslinjers { get; set; }
        public virtual DbSet<FaTiltaksplan> FaTiltaksplans { get; set; }
        public virtual DbSet<FaTiltaksplanevalueringer> FaTiltaksplanevalueringers { get; set; }
        public virtual DbSet<FaTiltakstyper> FaTiltakstypers { get; set; }
        public virtual DbSet<FaTmpOmberegning> FaTmpOmberegnings { get; set; }
        public virtual DbSet<FaTtkoder> FaTtkoders { get; set; }
        public virtual DbSet<FaUndersoekelser> FaUndersoekelsers { get; set; }
        public virtual DbSet<FaUndersoekelserSlettet> FaUndersoekelserSlettets { get; set; }
        public virtual DbSet<FaUndersoekelseslinjer> FaUndersoekelseslinjers { get; set; }
        public virtual DbSet<FaUtbetalingsvilkaar> FaUtbetalingsvilkaars { get; set; }
        public virtual DbSet<FaVAktiveTiltak> FaVAktiveTiltaks { get; set; }
        public virtual DbSet<FaVAktiveTiltaksplaner> FaVAktiveTiltaksplaners { get; set; }
        public virtual DbSet<FaVAlleAktiveTiltaksplaner> FaVAlleAktiveTiltaksplaners { get; set; }
        public virtual DbSet<FaVAvvikslogg> FaVAvviksloggs { get; set; }
        public virtual DbSet<FaVBudsjettforbruk> FaVBudsjettforbruks { get; set; }
        public virtual DbSet<FaVCrAapnemeldinger> FaVCrAapnemeldingers { get; set; }
        public virtual DbSet<FaVCrAapneu> FaVCrAapneus { get; set; }
        public virtual DbSet<FaVCrAktiveOmsorgsplaner> FaVCrAktiveOmsorgsplaners { get; set; }
        public virtual DbSet<FaVCrAktiveOmsorgstiltak> FaVCrAktiveOmsorgstiltaks { get; set; }
        public virtual DbSet<FaVCrAktiveTiltak> FaVCrAktiveTiltaks { get; set; }
        public virtual DbSet<FaVCrAktiveTiltaksplaner> FaVCrAktiveTiltaksplaners { get; set; }
        public virtual DbSet<FaVCrAvsattemidler> FaVCrAvsattemidlers { get; set; }
        public virtual DbSet<FaVCrForbMedAktEngavt> FaVCrForbMedAktEngavts { get; set; }
        public virtual DbSet<FaVCrFosterhjem> FaVCrFosterhjems { get; set; }
        public virtual DbSet<FaVCrHuskeliste> FaVCrHuskelistes { get; set; }
        public virtual DbSet<FaVCrKliUtenOmsorgsplan> FaVCrKliUtenOmsorgsplans { get; set; }
        public virtual DbSet<FaVCrKliUtenTiltaksplan> FaVCrKliUtenTiltaksplans { get; set; }
        public virtual DbSet<FaVCrKliYtelser> FaVCrKliYtelsers { get; set; }
        public virtual DbSet<FaVCrKlienter> FaVCrKlienters { get; set; }
        public virtual DbSet<FaVCrKlientplassering> FaVCrKlientplasserings { get; set; }
        public virtual DbSet<FaVCrMangelfullbetaling> FaVCrMangelfullbetalings { get; set; }
        public virtual DbSet<FaVCrMangelfullenglinje> FaVCrMangelfullenglinjes { get; set; }
        public virtual DbSet<FaVCrManglerFosterforeldre> FaVCrManglerFosterforeldres { get; set; }
        public virtual DbSet<FaVCrNullIverksatteTiltak> FaVCrNullIverksatteTiltaks { get; set; }
        public virtual DbSet<FaVCrPlasseringer> FaVCrPlasseringers { get; set; }
        public virtual DbSet<FaVCrSaker> FaVCrSakers { get; set; }
        public virtual DbSet<FaVCrStatGrunnlag5> FaVCrStatGrunnlag5s { get; set; }
        public virtual DbSet<FaVCrStatGrunnlag5Kobling> FaVCrStatGrunnlag5Koblings { get; set; }
        public virtual DbSet<FaVCrTilsynsfoerer> FaVCrTilsynsfoerers { get; set; }
        public virtual DbSet<FaVCrUstilflerevedtak> FaVCrUstilflerevedtaks { get; set; }
        public virtual DbSet<FaVCrUtenOmsorgsplan> FaVCrUtenOmsorgsplans { get; set; }
        public virtual DbSet<FaVCrUtenTiltaksplan> FaVCrUtenTiltaksplans { get; set; }
        public virtual DbSet<FaVCrVarSnittInstitusjon> FaVCrVarSnittInstitusjons { get; set; }
        public virtual DbSet<FaVCrYtelser> FaVCrYtelsers { get; set; }
        public virtual DbSet<FaVGodkjenning> FaVGodkjennings { get; set; }
        public virtual DbSet<FaVKlientHovedtiltak> FaVKlientHovedtiltaks { get; set; }
        public virtual DbSet<FaVKlimedsperretadresse> FaVKlimedsperretadresses { get; set; }
        public virtual DbSet<FaVStatFmkontroll> FaVStatFmkontrolls { get; set; }
        public virtual DbSet<FaVStatKontrollskjema> FaVStatKontrollskjemas { get; set; }
        public virtual DbSet<FaVStatPaaklaget> FaVStatPaaklagets { get; set; }
        public virtual DbSet<FaVStatSsb1> FaVStatSsb1s { get; set; }
        public virtual DbSet<FaVStatSsb2> FaVStatSsb2s { get; set; }
        public virtual DbSet<FaVStatSsb3> FaVStatSsb3s { get; set; }
        public virtual DbSet<FaVStatSsb4> FaVStatSsb4s { get; set; }
        public virtual DbSet<FaVTiltaksrang> FaVTiltaksrangs { get; set; }
        public virtual DbSet<FaVTiltaksstatus> FaVTiltaksstatuses { get; set; }
        public virtual DbSet<FaVUtenTiltaksplan> FaVUtenTiltaksplans { get; set; }
        public virtual DbSet<FaVYtelser> FaVYtelsers { get; set; }
        public virtual DbSet<FaVedlegg> FaVedleggs { get; set; }
        public virtual DbSet<FaVedtaksmyndighet> FaVedtaksmyndighets { get; set; }
        public virtual DbSet<FaVindu> FaVindus { get; set; }
        public virtual DbSet<FaVurdegenbet> FaVurdegenbets { get; set; }
        public virtual DbSet<GenVDokumenter> GenVDokumenters { get; set; }
        public virtual DbSet<KonvNumRow> KonvNumRows { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<TmpFamiliaKommune> TmpFamiliaKommunes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<string, string>(
                v => v,
                v => v.Trim());

            var comparer = new ValueComparer<string>(
                (l, r) => string.Equals(l, r, StringComparison.OrdinalIgnoreCase),
                v => v.ToUpper().GetHashCode(),
                v => v);

            modelBuilder.Entity<Agressolev>(entity =>
            {
                entity.HasKey(e => e.AglLevnr)
                    .IsClustered(false);

                entity.ToTable("AGRESSOLEV");

                entity.Property(e => e.AglLevnr)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("AGL_LEVNR");

                entity.Property(e => e.AglAdresse)
                    .IsRequired()
                    .HasMaxLength(160)
                    .IsUnicode(false)
                    .HasColumnName("AGL_ADRESSE");

                entity.Property(e => e.AglBankAccount)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("AGL_BANK_ACCOUNT");

                entity.Property(e => e.AglLandkode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("AGL_LANDKODE");

                entity.Property(e => e.AglLevnavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AGL_LEVNAVN");

                entity.Property(e => e.AglOrgnr)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("AGL_ORGNR");

                entity.Property(e => e.AglPostnr)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("AGL_POSTNR");

                entity.Property(e => e.AglSted)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("AGL_STED");
            });

            modelBuilder.Entity<Dbmessage>(entity =>
            {
                entity.HasKey(e => new { e.Dbid, e.Dberrorid })
                    .IsClustered(false);

                entity.ToTable("DBMESSAGES");

                entity.HasIndex(e => e.Refmsgid, "FK_DBMESSAGES1")
                    .HasFillFactor(80);

                entity.Property(e => e.Dbid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("DBID")
                    .IsFixedLength();

                entity.Property(e => e.Dberrorid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DBERRORID");

                entity.Property(e => e.Errorid)
                    .HasColumnType("numeric(28, 0)")
                    .HasColumnName("ERRORID");

                entity.Property(e => e.Refmsgid)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("REFMSGID");

                entity.HasOne(d => d.Refmsg)
                    .WithMany(p => p.Dbmessages)
                    .HasForeignKey(d => d.Refmsgid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DBMESSAG_MESSAGES__MESSAGES");
            });

            modelBuilder.Entity<FaAarloepenr>(entity =>
            {
                entity.HasKey(e => new { e.AlpIdent, e.AlpAar })
                    .IsClustered(false);

                entity.ToTable("FA_AARLOEPENR");

                entity.Property(e => e.AlpIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ALP_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.AlpAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ALP_AAR");

                entity.Property(e => e.AlpSistbruktenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ALP_SISTBRUKTENR");
            });

            modelBuilder.Entity<FaAktivitet>(entity =>
            {
                entity.HasKey(e => e.AkvLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_AKTIVITET");

                entity.HasIndex(e => new { e.GsaAar, e.GsaJournalnr }, "FK_FA_AKTIVITET1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.AkkType, "FK_FA_AKTIVITET2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_AKTIVITET3");

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_AKTIVITET4")
                    .HasFillFactor(80);

                entity.Property(e => e.AkvLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("AKV_LOEPENR");

                entity.Property(e => e.AkkType)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AKK_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.AkvFrist)
                    .HasColumnType("datetime")
                    .HasColumnName("AKV_FRIST");

                entity.Property(e => e.AkvMerknad)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("AKV_MERKNAD");

                entity.Property(e => e.AkvUtfoert)
                    .HasColumnType("datetime")
                    .HasColumnName("AKV_UTFOERT");

                entity.Property(e => e.GsaAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("GSA_AAR");

                entity.Property(e => e.GsaJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("GSA_JOURNALNR");

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.HasOne(d => d.AkkTypeNavigation)
                    .WithMany(p => p.FaAktivitets)
                    .HasForeignKey(d => d.AkkType)
                    .HasConstraintName("FK_FA_AKTIV_AKTIVITET_FA_AKTIV");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaAktivitets)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_AKTIV_SAKSBEHAN_FA_SAKSB");

                entity.HasOne(d => d.Gsa)
                    .WithMany(p => p.FaAktivitets)
                    .HasForeignKey(d => new { d.GsaAar, d.GsaJournalnr })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_AKTIV_GENERELLS_FA_GENER");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaAktivitets)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_FA_AKTIV_POSTJOURN_FA_POSTJ");
            });

            modelBuilder.Entity<FaAktivitetskode>(entity =>
            {
                entity.HasKey(e => e.AkkType)
                    .IsClustered(false);

                entity.ToTable("FA_AKTIVITETSKODE");

                entity.Property(e => e.AkkType)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AKK_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.AkkBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("AKK_BESKRIVELSE");

                entity.Property(e => e.AkkBhmaate)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("AKK_BHMAATE")
                    .IsFixedLength();

                entity.Property(e => e.AkkPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("AKK_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaAldersgrupper>(entity =>
            {
                entity.HasKey(e => e.AldIdent)
                    .IsClustered(false);

                entity.ToTable("FA_ALDERSGRUPPER");

                entity.Property(e => e.AldIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ALD_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.AldBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ALD_BESKRIVELSE");

                entity.Property(e => e.AldFraaar)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ALD_FRAAAR");

                entity.Property(e => e.AldPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ALD_PASSIVISERTDATO");

                entity.Property(e => e.AldTilaar)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ALD_TILAAR");
            });

            modelBuilder.Entity<FaAldersskala>(entity =>
            {
                entity.HasKey(e => e.AlsIdent)
                    .IsClustered(false);

                entity.ToTable("FA_ALDERSSKALA");

                entity.HasIndex(e => e.LoeTrinn, "FK_FA_ALDERSSKALA1")
                    .HasFillFactor(80);

                entity.Property(e => e.AlsIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ALS_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.AlsBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ALS_BESKRIVELSE");

                entity.Property(e => e.AlsFraaar)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ALS_FRAAAR");

                entity.Property(e => e.AlsPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ALS_PASSIVISERTDATO");

                entity.Property(e => e.AlsTilaar)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ALS_TILAAR");

                entity.Property(e => e.LoeTrinn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("LOE_TRINN")
                    .IsFixedLength();

                entity.HasOne(d => d.LoeTrinnNavigation)
                    .WithMany(p => p.FaAldersskalas)
                    .HasForeignKey(d => d.LoeTrinn)
                    .HasConstraintName("FK_FA_ALDER_LOENNTRIN_FA_LOENN");
            });

            modelBuilder.Entity<FaAppointment>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.ToTable("FA_APPOINTMENT");

                entity.Property(e => e.AppId).HasColumnName("APP_ID");

                entity.Property(e => e.AppAppointmentEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_APPOINTMENT_END_DATE");

                entity.Property(e => e.AppAppointmentStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_APPOINTMENT_START_DATE");

                entity.Property(e => e.AppClientAsConnection)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("APP_CLIENT_AS_CONNECTION");

                entity.Property(e => e.AppDescription)
                    .HasMaxLength(500)
                    .HasColumnName("APP_DESCRIPTION");

                entity.Property(e => e.AppInitialer)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("APP_INITIALER");

                entity.Property(e => e.AppPhonenumber)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("APP_PHONENUMBER");

                entity.Property(e => e.AppPlace)
                    .HasMaxLength(255)
                    .HasColumnName("APP_PLACE");

                entity.Property(e => e.AppTitle)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("APP_TITLE");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtkLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KTK_LOEPENR");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaAppointments)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_APPOINTMENT_KLIENT");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaAppointments)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_APPOINTMENT_POSTADRESSER");

                entity.HasOne(d => d.K)
                    .WithMany(p => p.FaAppointments)
                    .HasForeignKey(d => new { d.KliLoepenr, d.KtkLoepenr })
                    .HasConstraintName("FK_FA_APPOINTMENT_KLIENTTILKNYTNING");
            });

            modelBuilder.Entity<FaArbeidsgiveravgift>(entity =>
            {
                entity.HasKey(e => new { e.ArsIdent, e.AraFradato })
                    .IsClustered(false);

                entity.ToTable("FA_ARBEIDSGIVERAVGIFT");

                entity.HasIndex(e => e.ArsIdent, "FK_FA_ARBEIDSGIVERAVGIFT1")
                    .HasFillFactor(80);

                entity.Property(e => e.ArsIdent)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARS_IDENT");

                entity.Property(e => e.AraFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARA_FRADATO");

                entity.Property(e => e.AraFeriepengesats)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ARA_FERIEPENGESATS");

                entity.Property(e => e.AraSats)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ARA_SATS");

                entity.Property(e => e.AraTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARA_TILDATO");

                entity.HasOne(d => d.ArsIdentNavigation)
                    .WithMany(p => p.FaArbeidsgiveravgifts)
                    .HasForeignKey(d => d.ArsIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ARBEI_ARBEIDSGI_FA_ARBEI");
            });

            modelBuilder.Entity<FaArbeidsgiversone>(entity =>
            {
                entity.HasKey(e => e.ArsIdent)
                    .IsClustered(false);

                entity.ToTable("FA_ARBEIDSGIVERSONE");

                entity.Property(e => e.ArsIdent)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARS_IDENT");

                entity.Property(e => e.ArsSonenavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ARS_SONENAVN");
            });

            modelBuilder.Entity<FaAvvikslogg>(entity =>
            {
                entity.HasKey(e => e.AvlGuid)
                    .IsClustered(false);

                entity.ToTable("FA_AVVIKSLOGG");

                entity.HasIndex(e => e.AvlLoggtidspunkt, "IX_AVVIKSLOGG1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.AvlSbhOperatoerinitialer, "IX_AVVIKSLOGG2");

                entity.HasIndex(e => e.AvlSbhOperatoernavn, "IX_AVVIKSLOGG3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.AvlOppslagstype, "IX_AVVIKSLOGG4")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.AvlSbh1, "IX_AVVIKSLOGG5");

                entity.HasIndex(e => e.AvlSbh2, "IX_AVVIKSLOGG6");

                entity.Property(e => e.AvlGuid)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("AVL_GUID");

                entity.Property(e => e.AvlId)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("AVL_ID");

                entity.Property(e => e.AvlId2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("AVL_ID2");

                entity.Property(e => e.AvlLoggdetaljer)
                    .IsUnicode(false)
                    .HasColumnName("AVL_LOGGDETALJER");

                entity.Property(e => e.AvlLoggtidspunkt)
                    .HasColumnType("datetime")
                    .HasColumnName("AVL_LOGGTIDSPUNKT")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AvlLoggtidspunkt2)
                    .HasColumnType("datetime")
                    .HasColumnName("AVL_LOGGTIDSPUNKT2");

                entity.Property(e => e.AvlOppslagstype)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AVL_OPPSLAGSTYPE");

                entity.Property(e => e.AvlReferanse)
                    .IsUnicode(false)
                    .HasColumnName("AVL_REFERANSE");

                entity.Property(e => e.AvlSbh1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AVL_SBH1");

                entity.Property(e => e.AvlSbh2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AVL_SBH2");

                entity.Property(e => e.AvlSbhOperatoerinitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AVL_SBH_OPERATOERINITIALER");

                entity.Property(e => e.AvlSbhOperatoernavn)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("AVL_SBH_OPERATOERNAVN");
            });

            modelBuilder.Entity<FaBegrFunksjoner>(entity =>
            {
                entity.HasKey(e => e.BfuIdent)
                    .IsClustered(false);

                entity.ToTable("FA_BEGR_FUNKSJONER");

                entity.Property(e => e.BfuIdent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BFU_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.BfuBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("BFU_beskrivelse");

                entity.HasMany(d => d.TggIdents)
                    .WithMany(p => p.BfuIdents)
                    .UsingEntity<Dictionary<string, object>>(
                        "FaTilgangsgrBegrfunk",
                        l => l.HasOne<FaTilgangsgrupper>().WithMany().HasForeignKey("TggIdent").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_TILGA_TILGANGSG_FA_TILG1"),
                        r => r.HasOne<FaBegrFunksjoner>().WithMany().HasForeignKey("BfuIdent").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_TILGA_TILGANGSG_FA_BEGR2"),
                        j =>
                        {
                            j.HasKey("BfuIdent", "TggIdent").IsClustered(false);

                            j.ToTable("FA_TILGANGSGR_BEGRFUNK");

                            j.HasIndex(new[] { "TggIdent" }, "FK_FA_TILGANGSGR_BEGRFUNK1").HasFillFactor(80);

                            j.HasIndex(new[] { "BfuIdent" }, "FK_FA_TILGANGSGR_BEGRFUNK2").HasFillFactor(80);

                            j.IndexerProperty<string>("BfuIdent").HasMaxLength(10).IsUnicode(false).HasColumnName("BFU_IDENT").IsFixedLength();

                            j.IndexerProperty<string>("TggIdent").HasMaxLength(3).IsUnicode(false).HasColumnName("TGG_IDENT").IsFixedLength();
                        });
            });

            modelBuilder.Entity<FaBehandlingsmal>(entity =>
            {
                entity.HasKey(e => e.BemLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_BEHANDLINGSMAL");

                entity.HasIndex(e => e.SatSakstype, "FK_FA_BEHANDLINGSMAL1")
                    .HasFillFactor(80);

                entity.Property(e => e.BemLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("BEM_LOEPENR");

                entity.Property(e => e.BemBeskrivelse)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("BEM_BESKRIVELSE");

                entity.Property(e => e.BemBhmaate)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("BEM_BHMAATE")
                    .IsFixedLength();

                entity.Property(e => e.BemNavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("BEM_NAVN");

                entity.Property(e => e.BemPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("BEM_PASSIVISERTDATO");

                entity.Property(e => e.SatSakstype)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SAT_SAKSTYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.SatSakstypeNavigation)
                    .WithMany(p => p.FaBehandlingsmals)
                    .HasForeignKey(d => d.SatSakstype)
                    .HasConstraintName("FK_FA_BEHAN_SAKSTYPE__FA_SAKST");
            });

            modelBuilder.Entity<FaBehandlingsmallinje>(entity =>
            {
                entity.HasKey(e => e.BelLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_BEHANDLINGSMALLINJE");

                entity.HasIndex(e => e.BemLoepenr, "FK_FA_BEHANDLINGSMALLINJE1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.AkkType, "FK_FA_BEHANDLINGSMALLINJE2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_BEHANDLINGSMALLINJE3");

                entity.Property(e => e.BelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("BEL_LOEPENR");

                entity.Property(e => e.AkkType)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("AKK_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.BelFristEtterStart)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("BEL_FRIST_ETTER_START");

                entity.Property(e => e.BelIntern)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("BEL_INTERN");

                entity.Property(e => e.BelMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("BEL_MERKNAD");

                entity.Property(e => e.BemLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("BEM_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.HasOne(d => d.AkkTypeNavigation)
                    .WithMany(p => p.FaBehandlingsmallinjes)
                    .HasForeignKey(d => d.AkkType)
                    .HasConstraintName("FK_FA_BEHAN_AKTIVITET_FA_AKTIV");

                entity.HasOne(d => d.BemLoepenrNavigation)
                    .WithMany(p => p.FaBehandlingsmallinjes)
                    .HasForeignKey(d => d.BemLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_BEHAN_BEHANDLIN_FA_BEHAN");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaBehandlingsmallinjes)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_BEHAN_SAKSBEHAN_FA_SAKSB");
            });

            modelBuilder.Entity<FaBetalingAarsak>(entity =>
            {
                entity.HasKey(e => e.UaaIdent)
                    .IsClustered(false);

                entity.ToTable("FA_BETALING_AARSAK");

                entity.Property(e => e.UaaIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UAA_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.UaaBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("UAA_BESKRIVELSE");

                entity.Property(e => e.UaaPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UAA_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaBetalinger>(entity =>
            {
                entity.HasKey(e => e.UtbLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_BETALINGER");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_BETALINGER")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel3, e.KtnKontonummer3 }, "FK_FA_BETALINGER10")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel4, e.KtnKontonummer4 }, "FK_FA_BETALINGER11")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel5, e.KtnKontonummer5 }, "FK_FA_BETALINGER12")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel6, e.KtnKontonummer6 }, "FK_FA_BETALINGER13")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.VikType, "FK_FA_BETALINGER14")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.RekAar, e.RekLoepenr }, "FK_FA_BETALINGER15")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_BETALINGER16")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.BktType, e.BktKode }, "FK_FA_BETALINGER17")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_BETALINGER18")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.UaaIdent, "FK_FA_BETALINGER19")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhAnvistav, "FK_FA_BETALINGER2");

                entity.HasIndex(e => e.RerLoepenr, "FK_FA_BETALINGER20")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_BETALINGER3");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_BETALINGER4");

                entity.HasIndex(e => e.UtpLoepenr, "FK_FA_BETALINGER5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_BETALINGER6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.UtbTilbakefompostertLoepenr, "FK_FA_BETALINGER7")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel1, e.KtnKontonummer1 }, "FK_FA_BETALINGER8")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel2, e.KtnKontonummer2 }, "FK_FA_BETALINGER9")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.UtbEksternReferanse, "IX_FA_BETALINGER")
                    .HasFillFactor(80);

                entity.Property(e => e.UtbLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_LOEPENR");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.Property(e => e.KttTiltakskode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTT_TILTAKSKODE")
                    .IsFixedLength();

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.RekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("REK_AAR");

                entity.Property(e => e.RekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("REK_LOEPENR");

                entity.Property(e => e.RerLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("RER_LOEPENR");

                entity.Property(e => e.SbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_anvistav");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.Property(e => e.UaaIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UAA_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.UtbAnvisPaagaar)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVIS_PAAGAAR");

                entity.Property(e => e.UtbAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_ANVISTAAR");

                entity.Property(e => e.UtbAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_ANVISTDATO");

                entity.Property(e => e.UtbAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UTB_ANVISTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.UtbAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_ANVISTNR");

                entity.Property(e => e.UtbAnvisttilklient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVISTTILKLIENT");

                entity.Property(e => e.UtbBalanse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BALANSE")
                    .IsFixedLength();

                entity.Property(e => e.UtbBegrTilbakefoert)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BEGR_TILBAKEFOERT");

                entity.Property(e => e.UtbBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("UTB_BELOEP");

                entity.Property(e => e.UtbBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BETALINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.UtbBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BILAGSSERIE")
                    .IsFixedLength();

                entity.Property(e => e.UtbDisabledRegnskap).HasColumnName("UTB_DISABLED_REGNSKAP");

                entity.Property(e => e.UtbEksternReferanse)
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("UTB_EKSTERN_REFERANSE");

                entity.Property(e => e.UtbEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_ENDRETDATO");

                entity.Property(e => e.UtbFakturadato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_FAKTURADATO");

                entity.Property(e => e.UtbForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_FORFALLSDATO");

                entity.Property(e => e.UtbGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UTB_GMLREFERANSE");

                entity.Property(e => e.UtbMvakode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("UTB_MVAKODE")
                    .IsFixedLength();

                entity.Property(e => e.UtbNyettertbf)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_NYETTERTBF");

                entity.Property(e => e.UtbOverfoertregnskap)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_OVERFOERTREGNSKAP");

                entity.Property(e => e.UtbPeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_PERIODEAAR");

                entity.Property(e => e.UtbPeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("UTB_PERIODEMND");

                entity.Property(e => e.UtbPlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("UTB_PLANLAGTBELOEP");

                entity.Property(e => e.UtbRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_REGISTRERTDATO");

                entity.Property(e => e.UtbRetkode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UTB_RETKODE")
                    .HasDefaultValueSql("('NY')")
                    .IsFixedLength();

                entity.Property(e => e.UtbSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_SKREVET");

                entity.Property(e => e.UtbStatus)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UTB_STATUS")
                    .HasDefaultValueSql("('OK ')")
                    .IsFixedLength();

                entity.Property(e => e.UtbTelrem)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_TELREM");

                entity.Property(e => e.UtbTelrgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_TELRGN");

                entity.Property(e => e.UtbTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("UTB_TILBAKEFOERTBELOEP");

                entity.Property(e => e.UtbTilbakefompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_tilbakefompostert_LOEPENR");

                entity.Property(e => e.UtbUAvregningsnr)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_AVREGNINGSNR")
                    .IsFixedLength();

                entity.Property(e => e.UtbUBetalingsmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_BETALINGSMAATE")
                    .IsFixedLength();

                entity.Property(e => e.UtbUKid)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_KID")
                    .IsFixedLength();

                entity.Property(e => e.UtbUKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_KONTONRMOTTAKER")
                    .IsFixedLength();

                entity.Property(e => e.UtbUMeldingmottaker)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_MELDINGMOTTAKER");

                entity.Property(e => e.UtbUMottakerAdresse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_MOTTAKER_ADRESSE");

                entity.Property(e => e.UtbUMottakerPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_MOTTAKER_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.UtbUMottakerPoststed)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("UTB_U_MOTTAKER_POSTSTED");

                entity.Property(e => e.UtbURegnskapsfildato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_U_REGNSKAPSFILDATO");

                entity.Property(e => e.UtbURemitteringsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTB_U_REMITTERINGSDATO");

                entity.Property(e => e.UtbURemitteringsnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_U_REMITTERINGSNR");

                entity.Property(e => e.UtbUVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_U_VILKAAROPPFYLT");

                entity.Property(e => e.UtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTP_LOEPENR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_BETAL_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.ForLoepenr)
                    .HasConstraintName("FK_FA_BETAL_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_BETAL_KLIENT_UT_FA_KLIEN");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.ProLoepenr)
                    .HasConstraintName("FK_FA_BETAL_PROSJEKT__FA_PROSJ");

                entity.HasOne(d => d.RerLoepenrNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.RerLoepenr)
                    .HasConstraintName("FK_REFUSJONSKRAV_BETALINGER");

                entity.HasOne(d => d.SbhAnvistavNavigation)
                    .WithMany(p => p.FaBetalingerSbhAnvistavNavigations)
                    .HasForeignKey(d => d.SbhAnvistav)
                    .HasConstraintName("FK_FA_BETAL_SAKSBEH_U_FA_SAKS2");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaBetalingerSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_BETAL_SAKSBEH_U_FA_SAKS3");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaBetalingerSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_BETAL_SAKSBEH_U_FA_SAKSB");

                entity.HasOne(d => d.UaaIdentNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.UaaIdent)
                    .HasConstraintName("FK_FA_BETAL_UAARSAK_B_FA_BETAL");

                entity.HasOne(d => d.UtbTilbakefompostertLoepenrNavigation)
                    .WithMany(p => p.InverseUtbTilbakefompostertLoepenrNavigation)
                    .HasForeignKey(d => d.UtbTilbakefompostertLoepenr)
                    .HasConstraintName("FK_FA_BETAL_BETALING__FA_BETAL");

                entity.HasOne(d => d.UtpLoepenrNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.UtpLoepenr)
                    .HasConstraintName("FK_FA_BETAL_UTBPLAN_U_FA_BETAL");

                entity.HasOne(d => d.VikTypeNavigation)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => d.VikType)
                    .HasConstraintName("FK_FA_BETAL_UTBETVILK_FA_UTBET");

                entity.HasOne(d => d.Bkt)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => new { d.BktType, d.BktKode })
                    .HasConstraintName("FK_FA_BETAL_BETKAT_BE_FA_BET_2");

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaBetalingerKts)
                    .HasForeignKey(d => new { d.KtpNoekkel1, d.KtnKontonummer1 })
                    .HasConstraintName("FK_FA_BETAL_KONTO_BET_FA_KONT1");

                entity.HasOne(d => d.KtNavigation)
                    .WithMany(p => p.FaBetalingerKtNavigations)
                    .HasForeignKey(d => new { d.KtpNoekkel2, d.KtnKontonummer2 })
                    .HasConstraintName("FK_FA_BETAL_KONTO_BET_FA_KONT2");

                entity.HasOne(d => d.Kt1)
                    .WithMany(p => p.FaBetalingerKt1s)
                    .HasForeignKey(d => new { d.KtpNoekkel3, d.KtnKontonummer3 })
                    .HasConstraintName("FK_FA_BETAL_KONTO_BET_FA_KONT3");

                entity.HasOne(d => d.Kt2)
                    .WithMany(p => p.FaBetalingerKt2s)
                    .HasForeignKey(d => new { d.KtpNoekkel4, d.KtnKontonummer4 })
                    .HasConstraintName("FK_FA_BETAL_KONTO_BET_FA_KONT4");

                entity.HasOne(d => d.Kt3)
                    .WithMany(p => p.FaBetalingerKt3s)
                    .HasForeignKey(d => new { d.KtpNoekkel5, d.KtnKontonummer5 })
                    .HasConstraintName("FK_FA_BETAL_KONTO_BET_FA_KONT5");

                entity.HasOne(d => d.Kt4)
                    .WithMany(p => p.FaBetalingerKt4s)
                    .HasForeignKey(d => new { d.KtpNoekkel6, d.KtnKontonummer6 })
                    .HasConstraintName("FK_FA_BETAL_KONTO_BET_FA_KONT6");

                entity.HasOne(d => d.Rek)
                    .WithMany(p => p.FaBetalingers)
                    .HasForeignKey(d => new { d.RekAar, d.RekLoepenr })
                    .HasConstraintName("FK_FA_BETAL_REKVISISJ_FA_REKVI");
            });

            modelBuilder.Entity<FaBetalingskategorier>(entity =>
            {
                entity.HasKey(e => new { e.BktType, e.BktKode })
                    .IsClustered(false);

                entity.ToTable("FA_BETALINGSKATEGORIER");

                entity.HasIndex(e => new { e.BktMotpostType, e.BktMotpostKode }, "FK_FA_BETALINGSKATEGORIER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TtkKodeRegulativ, "FK_FA_BETALINGSKATEGORIER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TtkKodeIndividuell, "FK_FA_BETALINGSKATEGORIER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonummer }, "FK_FA_BETALINGSKATEGORIER4")
                    .HasFillFactor(80);

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktArbavgberegning)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("BKT_ARBAVGBEREGNING");

                entity.Property(e => e.BktBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("BKT_BESKRIVELSE");

                entity.Property(e => e.BktFeriepengeberegning)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("BKT_FERIEPENGEBEREGNING");

                entity.Property(e => e.BktMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("BKT_MERKNAD");

                entity.Property(e => e.BktMotpostKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_motpostKODE")
                    .IsFixedLength();

                entity.Property(e => e.BktMotpostType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_motpostTYPE")
                    .IsFixedLength();

                entity.Property(e => e.BktOverfoeresregnskap)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("BKT_OVERFOERESREGNSKAP");

                entity.Property(e => e.BktPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("BKT_PASSIVISERTDATO");

                entity.Property(e => e.BktResultatbalanse)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_RESULTATBALANSE")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.Property(e => e.TtkKodeIndividuell)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TTK_KODE_individuell")
                    .IsFixedLength();

                entity.Property(e => e.TtkKodeRegulativ)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TTK_KODE_regulativ")
                    .IsFixedLength();

                entity.HasOne(d => d.TtkKodeIndividuellNavigation)
                    .WithMany(p => p.FaBetalingskategorierTtkKodeIndividuellNavigations)
                    .HasForeignKey(d => d.TtkKodeIndividuell)
                    .HasConstraintName("FK_FA_BETAL_TTKODER_I_FA_TTKOD");

                entity.HasOne(d => d.TtkKodeRegulativNavigation)
                    .WithMany(p => p.FaBetalingskategorierTtkKodeRegulativNavigations)
                    .HasForeignKey(d => d.TtkKodeRegulativ)
                    .HasConstraintName("FK_FA_BETAL_TTKODER_R_FA_TTKOD");

                entity.HasOne(d => d.BktMotpost)
                    .WithMany(p => p.InverseBktMotpost)
                    .HasForeignKey(d => new { d.BktMotpostType, d.BktMotpostKode })
                    .HasConstraintName("FK_FA_BETAL_BETKAT_BE_FA_BETAL");

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaBetalingskategoriers)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonummer })
                    .HasConstraintName("FK_FA_BETAL_KONTOER_B_FA_KONTO");
            });

            modelBuilder.Entity<FaBetalingsplaner>(entity =>
            {
                entity.HasKey(e => e.UtpLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_BETALINGSPLANER");

                entity.HasIndex(e => new { e.SakAar, e.SakJournalnr }, "FK_FA_BETALINGSPLANER")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TilLoepenr, "FK_FA_BETALINGSPLANER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhStoppetavInitialer, "FK_FA_BETALINGSPLANER3");

                entity.HasIndex(e => e.SbhKlargjortavInitialer, "FK_FA_BETALINGSPLANER4");

                entity.HasIndex(e => e.SbhAvgjortavInitialer, "FK_FA_BETALINGSPLANER5");

                entity.HasIndex(e => e.VurLoepenr, "FK_FA_BETALINGSPLANER6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_BETALINGSPLANER7")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.BktType, e.BktKode }, "FK_FA_BETALINGSPLANER8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.RefLoepenr, "FK_FA_BETALINGSPLANER9")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.UtpStatus, "IX_FA_BETALINGSPLANER1")
                    .HasFillFactor(80);

                entity.Property(e => e.UtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTP_LOEPENR");

                entity.Property(e => e.BktKode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.RefLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("REF_LOEPENR");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_avgjortav_initialer");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_klargjortav_initialer");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_stoppetav_initialer");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.UtpAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTP_AARSAKER");

                entity.Property(e => e.UtpAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTP_AVGJORTDATO");

                entity.Property(e => e.UtpBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("UTP_BESKRIVELSE");

                entity.Property(e => e.UtpFormaal)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("UTP_FORMAAL");

                entity.Property(e => e.UtpGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UTP_GMLREFERANSE");

                entity.Property(e => e.UtpKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTP_KLARGJORTDATO");

                entity.Property(e => e.UtpStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UTP_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.UtpStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UTP_STOPPETDATO");

                entity.Property(e => e.UtpSumGodkjent)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("UTP_SUM_GODKJENT");

                entity.Property(e => e.UtpVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("UTP_VARIGHETFRA");

                entity.Property(e => e.UtpVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("UTP_VARIGHETTIL");

                entity.Property(e => e.VurLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("VUR_LOEPENR");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaBetalingsplaners)
                    .HasForeignKey(d => d.ProLoepenr)
                    .HasConstraintName("FK_FA_BETPL_PROSJEKT__FA_PROSJ");

                entity.HasOne(d => d.RefLoepenrNavigation)
                    .WithMany(p => p.FaBetalingsplaners)
                    .HasForeignKey(d => d.RefLoepenr)
                    .HasConstraintName("FK_REFUSJONER_BETALINGSPLAN");

                entity.HasOne(d => d.SbhAvgjortavInitialerNavigation)
                    .WithMany(p => p.FaBetalingsplanerSbhAvgjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhAvgjortavInitialer)
                    .HasConstraintName("FK_FA_BETAL_SAKSBEH_B_FA_SAKS2");

                entity.HasOne(d => d.SbhKlargjortavInitialerNavigation)
                    .WithMany(p => p.FaBetalingsplanerSbhKlargjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhKlargjortavInitialer)
                    .HasConstraintName("FK_FA_BETAL_SAKSBEH_B_FA_SAKS3");

                entity.HasOne(d => d.SbhStoppetavInitialerNavigation)
                    .WithMany(p => p.FaBetalingsplanerSbhStoppetavInitialerNavigations)
                    .HasForeignKey(d => d.SbhStoppetavInitialer)
                    .HasConstraintName("FK_FA_BETAL_SAKSBEH_B_FA_SAKSB");

                entity.HasOne(d => d.TilLoepenrNavigation)
                    .WithMany(p => p.FaBetalingsplaners)
                    .HasForeignKey(d => d.TilLoepenr)
                    .HasConstraintName("FK_FA_BETAL_TILTAK_UT_FA_TILTA");

                entity.HasOne(d => d.VurLoepenrNavigation)
                    .WithMany(p => p.FaBetalingsplaners)
                    .HasForeignKey(d => d.VurLoepenr)
                    .HasConstraintName("FK_FA_BETAL_VURDEREGE_FA_VURDE");

                entity.HasOne(d => d.Bkt)
                    .WithMany(p => p.FaBetalingsplaners)
                    .HasForeignKey(d => new { d.BktType, d.BktKode })
                    .HasConstraintName("FK_FA_BETAL_BETKAT_UT_FA_BETAL");

                entity.HasOne(d => d.Sak)
                    .WithMany(p => p.FaBetalingsplaners)
                    .HasForeignKey(d => new { d.SakAar, d.SakJournalnr })
                    .HasConstraintName("FK_FA_BETAL_SAK_UTBPL_FA_SAKSJ");
            });

            modelBuilder.Entity<FaBudsjett>(entity =>
            {
                entity.HasKey(e => new { e.BudBudsjettaar, e.BudLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_BUDSJETT");

                entity.HasIndex(e => new { e.KtpNoekkel1, e.KtnKontonummer1 }, "FK_FA_BUDSJETT1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel2, e.KtnKontonummer2 }, "FK_FA_BUDSJETT2")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel3, e.KtnKontonummer3 }, "FK_FA_BUDSJETT3")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel4, e.KtnKontonummer4 }, "FK_FA_BUDSJETT4")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel5, e.KtnKontonummer5 }, "FK_FA_BUDSJETT5")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel6, e.KtnKontonummer6 }, "FK_FA_BUDSJETT6")
                    .HasFillFactor(80);

                entity.Property(e => e.BudBudsjettaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("BUD_BUDSJETTAAR");

                entity.Property(e => e.BudLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("BUD_LOEPENR");

                entity.Property(e => e.BudBudsjett01)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT01");

                entity.Property(e => e.BudBudsjett02)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT02");

                entity.Property(e => e.BudBudsjett03)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT03");

                entity.Property(e => e.BudBudsjett04)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT04");

                entity.Property(e => e.BudBudsjett05)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT05");

                entity.Property(e => e.BudBudsjett06)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT06");

                entity.Property(e => e.BudBudsjett07)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT07");

                entity.Property(e => e.BudBudsjett08)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT08");

                entity.Property(e => e.BudBudsjett09)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT09");

                entity.Property(e => e.BudBudsjett10)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT10");

                entity.Property(e => e.BudBudsjett11)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT11");

                entity.Property(e => e.BudBudsjett12)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT12");

                entity.Property(e => e.BudBudsjett13)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETT13");

                entity.Property(e => e.BudBudsjettramme)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("BUD_BUDSJETTRAMME");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaBudsjettKts)
                    .HasForeignKey(d => new { d.KtpNoekkel1, d.KtnKontonummer1 })
                    .HasConstraintName("FK_FA_BUDSJ_KONTO_BUD_FA_KONTO");

                entity.HasOne(d => d.KtNavigation)
                    .WithMany(p => p.FaBudsjettKtNavigations)
                    .HasForeignKey(d => new { d.KtpNoekkel2, d.KtnKontonummer2 })
                    .HasConstraintName("FK_FA_BUDSJ_KONTO_BUD_FA_KONT6");

                entity.HasOne(d => d.Kt1)
                    .WithMany(p => p.FaBudsjettKt1s)
                    .HasForeignKey(d => new { d.KtpNoekkel3, d.KtnKontonummer3 })
                    .HasConstraintName("FK_FA_BUDSJ_KONTO_BUD_FA_KONT5");

                entity.HasOne(d => d.Kt2)
                    .WithMany(p => p.FaBudsjettKt2s)
                    .HasForeignKey(d => new { d.KtpNoekkel4, d.KtnKontonummer4 })
                    .HasConstraintName("FK_FA_BUDSJ_KONTO_BUD_FA_KONT4");

                entity.HasOne(d => d.Kt3)
                    .WithMany(p => p.FaBudsjettKt3s)
                    .HasForeignKey(d => new { d.KtpNoekkel5, d.KtnKontonummer5 })
                    .HasConstraintName("FK_FA_BUDSJ_KONTO_BUD_FA_KONT3");

                entity.HasOne(d => d.Kt4)
                    .WithMany(p => p.FaBudsjettKt4s)
                    .HasForeignKey(d => new { d.KtpNoekkel6, d.KtnKontonummer6 })
                    .HasConstraintName("FK_FA_BUDSJ_KONTO_BUD_FA_KONT2");
            });

            modelBuilder.Entity<FaDato>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FA_DATO");

                entity.Property(e => e.PlukkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLUKK_DATO");
            });

            modelBuilder.Entity<FaDbversjon>(entity =>
            {
                entity.HasKey(e => e.DbvLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_DBVERSJON");

                entity.Property(e => e.DbvLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DBV_LOEPENR");

                entity.Property(e => e.DbvDbdato)
                    .HasColumnType("datetime")
                    .HasColumnName("DBV_DBDATO");

                entity.Property(e => e.DbvFritekst)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("DBV_FRITEKST");

                entity.Property(e => e.DbvVaardato)
                    .HasColumnType("datetime")
                    .HasColumnName("DBV_VAARDATO");

                entity.Property(e => e.DbvVersjon)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("DBV_VERSJON");
            });

            modelBuilder.Entity<FaDelmaal>(entity =>
            {
                entity.HasKey(e => e.TpdLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_DELMAAL");

                entity.HasIndex(e => e.TtpLoepenr, "FK_FA_DELMAAL")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_DELMAAL2");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_DELMAAL3");

                entity.Property(e => e.TpdLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TPD_LOEPENR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhRegistrertav)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.Property(e => e.TpdDelmaal)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TPD_DELMAAL");

                entity.Property(e => e.TpdEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TPD_ENDRETDATO");

                entity.Property(e => e.TpdPrioritet)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TPD_PRIORITET");

                entity.Property(e => e.TpdRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TPD_REGISTRERTDATO");

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaDelmaalSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_DELMAAL_ENDRETAV");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaDelmaalSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_DELMAAL_REGISTRERTAV");

                entity.HasOne(d => d.TtpLoepenrNavigation)
                    .WithMany(p => p.FaDelmaals)
                    .HasForeignKey(d => d.TtpLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_DELMAAL_TILTAKSPLAN");

                entity.HasMany(d => d.TptLoepenrs)
                    .WithMany(p => p.TpdLoepenrs)
                    .UsingEntity<Dictionary<string, object>>(
                        "FaPlantiltakDelmaal",
                        l => l.HasOne<FaPlantiltak>().WithMany().HasForeignKey("TptLoepenr").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_PTILTAKDELMAAL_PLANTILT"),
                        r => r.HasOne<FaDelmaal>().WithMany().HasForeignKey("TpdLoepenr").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_PTILTAKDELMAAL_DELMAAL"),
                        j =>
                        {
                            j.HasKey("TpdLoepenr", "TptLoepenr").IsClustered(false);

                            j.ToTable("FA_PLANTILTAK_DELMAAL");

                            j.HasIndex(new[] { "TpdLoepenr" }, "FK_FA_PLANTILTAK_DELMAAL").HasFillFactor(80);

                            j.HasIndex(new[] { "TptLoepenr" }, "FK_FA_PLANTILTAK_DELMAAL2").HasFillFactor(80);

                            j.IndexerProperty<decimal>("TpdLoepenr").HasColumnType("numeric(10, 0)").HasColumnName("TPD_LOEPENR");

                            j.IndexerProperty<decimal>("TptLoepenr").HasColumnType("numeric(10, 0)").HasColumnName("TPT_LOEPENR");
                        });
            });

            modelBuilder.Entity<FaDistrikt>(entity =>
            {
                entity.HasKey(e => e.DisDistriktskode)
                    .IsClustered(false);

                entity.ToTable("FA_DISTRIKT");

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonrUHjemmet }, "FK_FA_DISTRIKT1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonrIHjemmet }, "FK_FA_DISTRIKT2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_DISTRIKT3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_DISTRIKT4")
                    .HasFillFactor(80);

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.DisAdresse1)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DIS_ADRESSE1");

                entity.Property(e => e.DisAdresse2)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DIS_ADRESSE2");

                entity.Property(e => e.DisBydelsnavn)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DIS_BYDELSNAVN");

                entity.Property(e => e.DisDistriktsnavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSNAVN");

                entity.Property(e => e.DisLedersnavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DIS_LEDERSNAVN");

                entity.Property(e => e.DisLederstittel)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DIS_LEDERSTITTEL");

                entity.Property(e => e.DisOrganisasjonsnr)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("DIS_ORGANISASJONSNR")
                    .IsFixedLength();

                entity.Property(e => e.DisPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("DIS_PASSIVISERTDATO");

                entity.Property(e => e.DisSelskap)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DIS_SELSKAP")
                    .IsFixedLength();

                entity.Property(e => e.DisSsbKode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_SSB_KODE")
                    .IsFixedLength();

                entity.Property(e => e.DisTelefaks)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("DIS_TELEFAKS")
                    .IsFixedLength();

                entity.Property(e => e.DisTelefonnr)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("DIS_TELEFONNR")
                    .IsFixedLength();

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.KtnKontonrIHjemmet)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonr_i_hjemmet")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonrUHjemmet)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonr_u_hjemmet")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaDistrikts)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_DISTRIKT_KOMMUNE");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaDistrikts)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_DISTR_POSTNR_DI_FA_POSTA");

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaDistriktKts)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonrIHjemmet })
                    .HasConstraintName("FK_FA_DISTR_KONTOER_D_FA_KONT2");

                entity.HasOne(d => d.KtNavigation)
                    .WithMany(p => p.FaDistriktKtNavigations)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonrUHjemmet })
                    .HasConstraintName("FK_FA_DISTR_KONTOER_D_FA_KONTO");

                entity.HasMany(d => d.SbhInitialers)
                    .WithMany(p => p.DisDistriktskodes)
                    .UsingEntity<Dictionary<string, object>>(
                        "FaSaksbehDistrikt",
                        l => l.HasOne<FaSaksbehandlere>().WithMany().HasForeignKey("SbhInitialer").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_SAKSB_SAKSBEH_D_FA_SAKSB"),
                        r => r.HasOne<FaDistrikt>().WithMany().HasForeignKey("DisDistriktskode").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_SAKSB_SAKSBEH_D_FA_DISTR"),
                        j =>
                        {
                            j.HasKey("DisDistriktskode", "SbhInitialer").IsClustered(false);

                            j.ToTable("FA_SAKSBEH_DISTRIKT");

                            j.HasIndex(new[] { "SbhInitialer" }, "FK_FA_SAKSBEH_DISTRIKT1");

                            j.HasIndex(new[] { "DisDistriktskode" }, "FK_FA_SAKSBEH_DISTRIKT2").HasFillFactor(80);

                            j.IndexerProperty<string>("DisDistriktskode").HasMaxLength(2).IsUnicode(false).HasColumnName("DIS_DISTRIKTSKODE").IsFixedLength();

                            j.IndexerProperty<string>("SbhInitialer").HasMaxLength(8).IsUnicode(false).HasColumnName("sbh_initialer");
                        });
            });

            modelBuilder.Entity<FaDistriktaarloepenr>(entity =>
            {
                entity.HasKey(e => new { e.DlpAar, e.DlpNrserieid })
                    .IsClustered(false);

                entity.ToTable("FA_DISTRIKTAARLOEPENR");

                entity.Property(e => e.DlpAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("DLP_AAR");

                entity.Property(e => e.DlpNrserieid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DLP_NRSERIEID")
                    .IsFixedLength();

                entity.Property(e => e.DlpSistbruktenr)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("DLP_SISTBRUKTENR");
            });

            modelBuilder.Entity<FaDistriktloepenrserier>(entity =>
            {
                entity.HasKey(e => new { e.DisDistriktskode, e.DlsIdent, e.DlsNrserieid, e.DlsNavn })
                    .IsClustered(false);

                entity.ToTable("FA_DISTRIKTLOEPENRSERIER");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_DISTRIKTLOEPENRSERIER1")
                    .HasFillFactor(80);

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.DlsIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("DLS_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.DlsNrserieid)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DLS_NRSERIEID")
                    .IsFixedLength();

                entity.Property(e => e.DlsNavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DLS_NAVN");

                entity.Property(e => e.DlsBalanse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("DLS_BALANSE")
                    .IsFixedLength();

                entity.Property(e => e.DlsPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("DLS_PASSIVISERTDATO");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaDistriktloepenrseriers)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_DISTR_DISTRIKT__FA_DISTR");
            });

            modelBuilder.Entity<FaDokumenter>(entity =>
            {
                entity.HasKey(e => e.DokLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_DOKUMENTER");

                entity.HasIndex(e => e.SbhUtsjekketavInitialer, "FK_FA_DOKUMENTER1");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_DOKUMENTER2");

                entity.HasIndex(e => e.VinUtvnavn, "FK_FA_DOKUMENTER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokType, "IX_FA_DOKUMENTER_DOKTYPE")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokEndretdato, "IX_FA_DOKUMENTER_ENDRETDATO")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLaast, "IX_FA_DOKUMENTER_LAAST")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokMimetype, "IX_FA_DOKUMENTER_MIME")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokProdusert, "IX_FA_DOKUMENTER_PROD")
                    .HasFillFactor(80);

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.CheckOutSessionId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ArkPdfaStatus)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ARK_PDFA_STATUS");

                entity.Property(e => e.ArkPdfaStatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_PDFA_STATUS_DATE");

                entity.Property(e => e.ArkPdfaStatusMessage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ARK_PDFA_STATUS_MESSAGE");

                entity.Property(e => e.ArkPdfaStatusExceptionCount)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_PDFA_STATUS_EXCEPTION_COUNT");

                entity.Property(e => e.DokDokument)
                    .HasColumnType("image")
                    .HasColumnName("DOK_DOKUMENT");

                entity.Property(e => e.DokEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("DOK_ENDRETDATO");

                entity.Property(e => e.DokImportTag)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOK_IMPORT_TAG");

                entity.Property(e => e.DokLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("DOK_LAAST");

                entity.Property(e => e.DokLagetavtekstmalerId).HasColumnName("DOK_LAGETAVTEKSTMALER_ID");

                entity.Property(e => e.DokMetaData)
                    .IsUnicode(false)
                    .HasColumnName("DOK_META_DATA");

                entity.Property(e => e.DokMimetype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DOK_MIMETYPE");

                entity.Property(e => e.DokPageorientation).HasColumnName("DOK_PAGEORIENTATION");

                entity.Property(e => e.DokProdusert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("DOK_PRODUSERT");

                entity.Property(e => e.DokSvarinnRef).HasColumnName("DOK_SVARINN_REF");

                entity.Property(e => e.DokType)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("DOK_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.DokUtsjekketDato)
                    .HasColumnType("datetime")
                    .HasColumnName("DOK_UTSJEKKET_DATO");

                entity.Property(e => e.DokUtsjekketFilnavn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOK_UTSJEKKET_FILNAVN");

                entity.Property(e => e.DokUtsjekketMaskin)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DOK_UTSJEKKET_MASKIN");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhUtsjekketavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_utsjekketav_initialer");

                entity.Property(e => e.VinUtvnavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VIN_UTVNAVN");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaDokumenterSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_DOKUM_SAKSBEH2__FA_SAKSB");

                entity.HasOne(d => d.SbhUtsjekketavInitialerNavigation)
                    .WithMany(p => p.FaDokumenterSbhUtsjekketavInitialerNavigations)
                    .HasForeignKey(d => d.SbhUtsjekketavInitialer)
                    .HasConstraintName("FK_FA_DOKUM_SAKSBEH_D_FA_SAKSB");
            });

            modelBuilder.Entity<FaEier>(entity =>
            {
                entity.HasKey(e => e.KomKommunenr)
                    .IsClustered(false);

                entity.ToTable("FA_EIER");

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_EIER1")
                    .HasFillFactor(80);

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.ArkAktivert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_AKTIVERT");

                entity.Property(e => e.ArkFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_FRADATO");

                entity.Property(e => e.ArkStatus)
                    .IsUnicode(false)
                    .HasColumnName("ARK_STATUS");

                entity.Property(e => e.ArkTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_TILDATO");

                entity.Property(e => e.EieAdresse1)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("EIE_ADRESSE1");

                entity.Property(e => e.EieAdresse2)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("EIE_ADRESSE2");

                entity.Property(e => e.EieAktiverAvvikslogg)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EIE_AKTIVER_AVVIKSLOGG")
                    .HasDefaultValueSql("('AV')")
                    .IsFixedLength();

                entity.Property(e => e.EieAngrefristEvaluering)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGREFRIST_EVALUERING");

                entity.Property(e => e.EieAngrefristGjennomgangsdok)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGREFRIST_GJENNOMGANGSDOK");

                entity.Property(e => e.EieAngrefristPlan)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EIE_ANGREFRIST_PLAN");

                entity.Property(e => e.EieAngrefristPostjournal)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGREFRIST_POSTJOURNAL");

                entity.Property(e => e.EieAngrefristUndersoekPlan)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGREFRIST_UNDERSOEK_PLAN");

                entity.Property(e => e.EieAngrefristinnbetaling)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EIE_ANGREFRISTINNBETALING");

                entity.Property(e => e.EieAngrefristjournalnotat)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGREFRISTJOURNALNOTAT");

                entity.Property(e => e.EieAngrefristutbetaling)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EIE_ANGREFRISTUTBETALING");

                entity.Property(e => e.EieAngrefristvedtak)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EIE_ANGREFRISTVEDTAK");

                entity.Property(e => e.EieAngrfrUndersoekSlutrapp)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGRFR_UNDERSOEK_SLUTRAPP");

                entity.Property(e => e.EieAngrfristjournalnotat)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANGRFRISTJOURNALNOTAT");

                entity.Property(e => e.EieAntdgUndersoekfrist)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANTDG_UNDERSOEKFRIST");

                entity.Property(e => e.EieAntdgUtloepUtbplan)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANTDG_UTLOEP_UTBPLAN");

                entity.Property(e => e.EieAntdgVarselengavtale)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANTDG_VARSELENGAVTALE");

                entity.Property(e => e.EieAntdgVarselmelding)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANTDG_VARSELMELDING");

                entity.Property(e => e.EieAntdgVarseltiltak)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANTDG_VARSELTILTAK");

                entity.Property(e => e.EieAntdgVarselundersoekelse)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_ANTDG_VARSELUNDERSOEKELSE");

                entity.Property(e => e.EieAutomatiskBilag)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_AUTOMATISK_BILAG");

                entity.Property(e => e.EieAutomatiskDoklistenr)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_AUTOMATISK_DOKLISTENR");

                entity.Property(e => e.EieAutomatiskLoenn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_AUTOMATISK_LOENN");

                entity.Property(e => e.EieAutomatiskRemfil)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_AUTOMATISK_REMFIL");

                entity.Property(e => e.EieBalanseArbgiveravg)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_BALANSE_ARBGIVERAVG")
                    .IsFixedLength();

                entity.Property(e => e.EieBalanseArbgiverferie)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_BALANSE_ARBGIVERFERIE")
                    .IsFixedLength();

                entity.Property(e => e.EieBalanseFeriepenger)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_BALANSE_FERIEPENGER")
                    .IsFixedLength();

                entity.Property(e => e.EieBbsAvtaleid)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("EIE_BBS_AVTALEID")
                    .IsFixedLength();

                entity.Property(e => e.EieBbsKundeid)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_BBS_KUNDEID")
                    .IsFixedLength();

                entity.Property(e => e.EieBetalingIkkeregnskap)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_BETALING_IKKEREGNSKAP");

                entity.Property(e => e.EieBydelsnr)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EIE_BYDELSNR")
                    .IsFixedLength();

                entity.Property(e => e.EieCkFontFamilyF)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_FAMILY_F");

                entity.Property(e => e.EieCkFontFamilyH1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_FAMILY_H1");

                entity.Property(e => e.EieCkFontFamilyH2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_FAMILY_H2");

                entity.Property(e => e.EieCkFontFamilyH3)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_FAMILY_H3");

                entity.Property(e => e.EieCkFontFamilyN)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_FAMILY_N");

                entity.Property(e => e.EieCkFontSizeF)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_SIZE_F");

                entity.Property(e => e.EieCkFontSizeH1)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_SIZE_H1");

                entity.Property(e => e.EieCkFontSizeH2)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_SIZE_H2");

                entity.Property(e => e.EieCkFontSizeH3)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_SIZE_H3");

                entity.Property(e => e.EieCkFontSizeN)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EIE_CK_FONT_SIZE_N");

                entity.Property(e => e.EieDigitalpost)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_DIGITALPOST");

                entity.Property(e => e.EieDigitalprint)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_DIGITALPRINT");

                entity.Property(e => e.EieDistriktsvisParametre)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_DISTRIKTSVIS_PARAMETRE");

                entity.Property(e => e.EieDokEttDok)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_DOK_ETT_DOK");

                entity.Property(e => e.EieEkstLnnBilagsformat)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_BILAGSFORMAT")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnBrukerid)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_BRUKERID")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnBrukernr)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_BRUKERNR")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnHovedsystem)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_HOVEDSYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnIntegrertsystem)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_INTEGRERTSYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnPassord)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_PASSORD")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnPassordendret)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_EKST_LNN_PASSORDENDRET");

                entity.Property(e => e.EieEkstLnnPassordvarighet)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_EKST_LNN_PASSORDVARIGHET");

                entity.Property(e => e.EieEkstLnnType)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.EieEkstLnnUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_LNN_URL");

                entity.Property(e => e.EieEkstUbUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_UB_URL");

                entity.Property(e => e.EieEkstVaUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EIE_EKST_VA_URL");

                entity.Property(e => e.EieEngAutoPlangodkjenning)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_ENG_AUTO_PLANGODKJENNING");

                entity.Property(e => e.EieEngFortloepende)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_ENG_FORTLOEPENDE");

                entity.Property(e => e.EieEngKravPlangodkjenning)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_ENG_KRAV_PLANGODKJENNING")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EieEtat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EIE_ETAT");

                entity.Property(e => e.EieFamiliekopiering)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_FAMILIEKOPIERING")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EieFlytIntegrasjonsnokkel)
                    .HasMaxLength(26)
                    .IsUnicode(false)
                    .HasColumnName("EIE_FLYT_INTEGRASJONSNOKKEL");

                entity.Property(e => e.EieFlytOrganisasjonsnr)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("EIE_FLYT_ORGANISASJONSNR")
                    .IsFixedLength();

                entity.Property(e => e.EieFlytUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EIE_FLYT_URL");

                entity.Property(e => e.EieFolketall)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("EIE_FOLKETALL");

                entity.Property(e => e.EieFolkregBruker)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_FOLKREG_BRUKER");

                entity.Property(e => e.EieFolkregPassord)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_FOLKREG_PASSORD");

                entity.Property(e => e.EieFolkregUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EIE_FOLKREG_URL");

                entity.Property(e => e.EieGodkjennutenvurd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_GODKJENNUTENVURD")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EieGyldighetpassord)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_GYLDIGHETPASSORD")
                    .HasDefaultValueSql("((90))");

                entity.Property(e => e.EieHindrekopiutskrift)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_HINDREKOPIUTSKRIFT");

                entity.Property(e => e.EieInngFerdigIns)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_INNG_FERDIG_INS");

                entity.Property(e => e.EieKkoder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KKODER");

                entity.Property(e => e.EieKntrlvedtakAnnensbh)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KNTRLVEDTAK_ANNENSBH");

                entity.Property(e => e.EieKontofTiltak)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KONTOF_TILTAK");

                entity.Property(e => e.EieKontonr)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("EIE_KONTONR")
                    .IsFixedLength();

                entity.Property(e => e.EieKontrollnivaavedtak)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_KONTROLLNIVAAVEDTAK")
                    .IsFixedLength();

                entity.Property(e => e.EieKopiklientmeldinger)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KOPIKLIENTMELDINGER");

                entity.Property(e => e.EieKrevePjdokInn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KREVE_PJDOK_INN");

                entity.Property(e => e.EieKrevedistrikt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KREVEDISTRIKT");

                entity.Property(e => e.EieKreveklargjoersak)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_KREVEKLARGJOERSAK");

                entity.Property(e => e.EieKtoArbgiveravg)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_KTO_ARBGIVERAVG")
                    .IsFixedLength();

                entity.Property(e => e.EieKtoArbgiverferie)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_KTO_ARBGIVERFERIE")
                    .IsFixedLength();

                entity.Property(e => e.EieKtoFeriepenger)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EIE_KTO_FERIEPENGER")
                    .IsFixedLength();

                entity.Property(e => e.EieLevregBruker)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LEVREG_BRUKER");

                entity.Property(e => e.EieLevregDblinkDb)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LEVREG_DBLINK_DB");

                entity.Property(e => e.EieLevregDblinkServer)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LEVREG_DBLINK_SERVER");

                entity.Property(e => e.EieLevregPassord)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LEVREG_PASSORD");

                entity.Property(e => e.EieLnnKundeid)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LNN_KUNDEID")
                    .IsFixedLength();

                entity.Property(e => e.EieLnnSti)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LNN_STI");

                entity.Property(e => e.EieLnnSystem)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_LNN_SYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EieLoggdoegn)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EIE_LOGGDOEGN")
                    .HasDefaultValueSql("((90))");

                entity.Property(e => e.EieMaalform)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_MAALFORM")
                    .IsFixedLength();

                entity.Property(e => e.EieMaksalder)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("EIE_MAKSALDER");

                entity.Property(e => e.EieMaxSjekkKopier)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("EIE_MAX_SJEKK_KOPIER");

                entity.Property(e => e.EieMinstealder)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("EIE_MINSTEALDER")
                    .HasDefaultValueSql("((18))");

                entity.Property(e => e.EieNotDoknrJournal)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_NOT_DOKNR_JOURNAL");

                entity.Property(e => e.EieOblUndersoekplan)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_OBL_UNDERSOEKPLAN");

                entity.Property(e => e.EieOrganisasjonsnr)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("EIE_ORGANISASJONSNR")
                    .IsFixedLength();

                entity.Property(e => e.EiePassivInternavregning)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_PASSIV_INTERNAVREGNING")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EiePassivInternfakturering)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_PASSIV_INTERNFAKTURERING")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EiePer01)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_01");

                entity.Property(e => e.EiePer02)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_02");

                entity.Property(e => e.EiePer03)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_03");

                entity.Property(e => e.EiePer04)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_04");

                entity.Property(e => e.EiePer05)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_05");

                entity.Property(e => e.EiePer06)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_06");

                entity.Property(e => e.EiePer07)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_07");

                entity.Property(e => e.EiePer08)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_08");

                entity.Property(e => e.EiePer09)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_09");

                entity.Property(e => e.EiePer10)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_10");

                entity.Property(e => e.EiePer11)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_11");

                entity.Property(e => e.EiePer12)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_PER_12");

                entity.Property(e => e.EiePostjApprovalMandatory)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_POSTJ_APPROVAL_MANDATORY");

                entity.Property(e => e.EieProxyUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EIE_PROXY_URL");

                entity.Property(e => e.EieRegnskapPassord)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REGNSKAP_PASSORD");

                entity.Property(e => e.EieRegnskapPort)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("EIE_REGNSKAP_PORT");

                entity.Property(e => e.EieRegnskapProgsrv)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REGNSKAP_PROGSRV");

                entity.Property(e => e.EieRegnskapSystem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REGNSKAP_SYSTEM");

                entity.Property(e => e.EieRegnskapUserid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REGNSKAP_USERID");

                entity.Property(e => e.EieRemDatovalg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_DATOVALG")
                    .IsFixedLength();

                entity.Property(e => e.EieRemDivisjon)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_DIVISJON")
                    .IsFixedLength();

                entity.Property(e => e.EieRemKassabilag)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_REM_KASSABILAG");

                entity.Property(e => e.EieRemKundenr)
                    .HasColumnType("numeric(11, 0)")
                    .HasColumnName("EIE_REM_KUNDENR");

                entity.Property(e => e.EieRemOppdragskonto)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_OPPDRAGSKONTO")
                    .IsFixedLength();

                entity.Property(e => e.EieRemRet)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_RET");

                entity.Property(e => e.EieRemSistedato)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_REM_SISTEDATO");

                entity.Property(e => e.EieRemSisteprodnridag)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_SISTEPRODNRIDAG")
                    .IsFixedLength();

                entity.Property(e => e.EieRemSistesekvnrAvtale)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_SISTESEKVNR_AVTALE")
                    .IsFixedLength();

                entity.Property(e => e.EieRemSti)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_STI");

                entity.Property(e => e.EieRemSystem)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_REM_SYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnAggregerMotpost)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_AGGREGER_MOTPOST")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnAggregeringsnivaa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_AGGREGERINGSNIVAA")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnBilagsart)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_BILAGSART")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnFiltype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_FILTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnFirma)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_FIRMA")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnForsystem)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_FORSYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnMvakode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_MVAKODE")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnSti)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_STI");

                entity.Property(e => e.EieRgnSystem)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_RGN_SYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EieRgnUtvidetFilnavn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_RGN_UTVIDET_FILNAVN");

                entity.Property(e => e.EieSsbKrevefelt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_SSB_KREVEFELT");

                entity.Property(e => e.EieSsbProgram)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_SSB_PROGRAM");

                entity.Property(e => e.EieSsbTeksteditor)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_SSB_TEKSTEDITOR");

                entity.Property(e => e.EieStiFolkeregister)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_STI_FOLKEREGISTER");

                entity.Property(e => e.EieStiRutinehandbok)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EIE_STI_RUTINEHANDBOK");

                entity.Property(e => e.EieSvarinnActive)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_SVARINN_ACTIVE");

                entity.Property(e => e.EieSvarinnFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_SVARINN_FRADATO");

                entity.Property(e => e.EieSvarinnTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("EIE_SVARINN_TILDATO");

                entity.Property(e => e.EieTelefaksnr)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("EIE_TELEFAKSNR")
                    .IsFixedLength();

                entity.Property(e => e.EieTelefonnr)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("EIE_TELEFONNR")
                    .IsFixedLength();

                entity.Property(e => e.EieTillaterKnv0)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_TILLATER_KNV0");

                entity.Property(e => e.EieTillaterKnv1)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_TILLATER_KNV1");

                entity.Property(e => e.EieTillaterKnv2)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_TILLATER_KNV2");

                entity.Property(e => e.EieTypeFullformat)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EIE_TYPE_FULLFORMAT")
                    .IsFixedLength();

                entity.Property(e => e.EieUndVeiviser)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_UND_VEIVISER");

                entity.Property(e => e.EieUnderSluttrappMandatory)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_UNDER_SLUTTRAPP_MANDATORY");

                entity.Property(e => e.EieUseCkEditor)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_USE_CK_EDITOR");

                entity.Property(e => e.EieUseImportDistricts)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_USE_IMPORT_DISTRICTS");

                entity.Property(e => e.EieUtbetalingFredag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_FREDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieUtbetalingLoerdag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_LOERDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieUtbetalingMandag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_MANDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieUtbetalingOnsdag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_ONSDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieUtbetalingSoendag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_SOENDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieUtbetalingTirsdag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_TIRSDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieUtbetalingTorsdag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_UTBETALING_TORSDAG")
                    .HasDefaultValueSql("('0')")
                    .IsFixedLength();

                entity.Property(e => e.EieVarsleegenbet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_VARSLEEGENBET");

                entity.Property(e => e.EieVedtakUTildato)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EIE_VEDTAK_U_TILDATO");

                entity.Property(e => e.EieVedtakvarighet)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("EIE_VEDTAKVARIGHET");

                entity.Property(e => e.EieVilkaarbehandling)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EIE_VILKAARBEHANDLING")
                    .IsFixedLength();

                entity.Property(e => e.EieVismaOkoBruker)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_VISMA_OKO_BRUKER");

                entity.Property(e => e.EieVismaOkoPassord)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EIE_VISMA_OKO_PASSORD");

                entity.Property(e => e.EieVismaOkoRegion)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("EIE_VISMA_OKO_REGION")
                    .IsFixedLength();

                entity.Property(e => e.EieVismaOkoSelskap)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("EIE_VISMA_OKO_SELSKAP")
                    .IsFixedLength();

                entity.Property(e => e.EieVismaOkoUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EIE_VISMA_OKO_URL");

                entity.Property(e => e.PnrPostnr)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithOne(p => p.FaEier)
                    .HasForeignKey<FaEier>(d => d.KomKommunenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_EIER_KOMMUNER__FA_KOMMU");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaEiers)
                    .HasForeignKey(d => d.PnrPostnr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_EIER_POSTADRES_FA_POSTA");
            });

            modelBuilder.Entity<FaEksterneParametre>(entity =>
            {
                entity.HasKey(e => e.DisDistriktskode)
                    .IsClustered(false);

                entity.ToTable("FA_EKSTERNE_PARAMETRE");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnAutofil)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EPA_LNN_AUTOFIL");

                entity.Property(e => e.EpaLnnBalanseArbgiveravg)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_BALANSE_ARBGIVERAVG")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnBalanseArbgiverferie)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_BALANSE_ARBGIVERFERIE")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnBalanseFeriepenger)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_BALANSE_FERIEPENGER")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnKtoArbgiveravg)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_KTO_ARBGIVERAVG")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnKtoArbgiverferie)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_KTO_ARBGIVERFERIE")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnKtoFeriepenger)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_KTO_FERIEPENGER")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnKundeid)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_KUNDEID")
                    .IsFixedLength();

                entity.Property(e => e.EpaLnnSti)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EPA_LNN_STI");

                entity.Property(e => e.EpaRemAutofil)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EPA_REM_AUTOFIL");

                entity.Property(e => e.EpaRemBbsAvtaleid)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_BBS_AVTALEID")
                    .IsFixedLength();

                entity.Property(e => e.EpaRemBbsKundeid)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_BBS_KUNDEID")
                    .IsFixedLength();

                entity.Property(e => e.EpaRemDatovalg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_DATOVALG")
                    .IsFixedLength();

                entity.Property(e => e.EpaRemDivisjon)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_DIVISJON")
                    .IsFixedLength();

                entity.Property(e => e.EpaRemKassabilag)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EPA_REM_KASSABILAG");

                entity.Property(e => e.EpaRemKundenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("EPA_REM_KUNDENR");

                entity.Property(e => e.EpaRemOppdragskonto)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_OPPDRAGSKONTO")
                    .IsFixedLength();

                entity.Property(e => e.EpaRemRet)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_RET");

                entity.Property(e => e.EpaRemSistedato)
                    .HasColumnType("datetime")
                    .HasColumnName("EPA_REM_SISTEDATO");

                entity.Property(e => e.EpaRemSti)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EPA_REM_STI");

                entity.Property(e => e.EpaRgnAggregerMotpost)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_AGGREGER_MOTPOST")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnAggregeringsnivaa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_AGGREGERINGSNIVAA")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnBilagsart)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_BILAGSART")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnFiltype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_FILTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnFirma)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_FIRMA")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnForsystem)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_FORSYSTEM")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnMvakode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_MVAKODE")
                    .IsFixedLength();

                entity.Property(e => e.EpaRgnSti)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EPA_RGN_STI");

                entity.Property(e => e.EpaRgnUtvidetFilnavn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EPA_RGN_UTVIDET_FILNAVN");

                entity.Property(e => e.EpaSsbBydelsnr)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("EPA_SSB_BYDELSNR")
                    .IsFixedLength();

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithOne(p => p.FaEksterneParametre)
                    .HasForeignKey<FaEksterneParametre>(d => d.DisDistriktskode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EKSTERNE_PARAMETRE_DISTRIKT");
            });

            modelBuilder.Entity<FaEngasjementsavtale>(entity =>
            {
                entity.HasKey(e => e.EngLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_ENGASJEMENTSAVTALE");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_ENGASJEMENTSAVTALE1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TilLoepenr, "FK_FA_ENGASJEMENTSAVTALE10")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhKlargjortavInitialer, "FK_FA_ENGASJEMENTSAVTALE11");

                entity.HasIndex(e => e.SbhAvgjortavInitialer, "FK_FA_ENGASJEMENTSAVTALE12");

                entity.HasIndex(e => e.SbhStoppetavInitialer, "FK_FA_ENGASJEMENTSAVTALE13");

                entity.HasIndex(e => e.EngFosterLoeTrinn, "FK_FA_ENGASJEMENTSAVTALE14")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_ENGASJEMENTSAVTALE2");

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_ENGASJEMENTSAVTALE3")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.SakAar, e.SakJournalnr }, "FK_FA_ENGASJEMENTSAVTALE4")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_ENGASJEMENTSAVTALE5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KmgIdent, "FK_FA_ENGASJEMENTSAVTALE6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LoeTrinn, "FK_FA_ENGASJEMENTSAVTALE7")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_ENGASJEMENTSAVTALE8");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_ENGASJEMENTSAVTALE9");

                entity.HasIndex(e => e.EngStatus, "IX_FA_ENGASJEMENTSAVTALE1")
                    .HasFillFactor(80);

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.EngAntBompengepasseringer)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ENG_ANT_BOMPENGEPASSERINGER");

                entity.Property(e => e.EngAntParkeringer)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ENG_ANT_PARKERINGER");

                entity.Property(e => e.EngAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_AVGJORTDATO");

                entity.Property(e => e.EngBesoekDoegn)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ENG_BESOEK_DOEGN");

                entity.Property(e => e.EngBesoekFaktor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("ENG_BESOEK_FAKTOR");

                entity.Property(e => e.EngBesoekGodtype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_BESOEK_GODTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngBesoekUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_BESOEK_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngBilKmPrtur)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ENG_BIL_KM_PRTUR");

                entity.Property(e => e.EngBompengeUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_BOMPENGE_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngBompengebeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENG_BOMPENGEBELOEP");

                entity.Property(e => e.EngDagnrtimelister)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ENG_DAGNRTIMELISTER");

                entity.Property(e => e.EngDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_DOKUMENTNR");

                entity.Property(e => e.EngEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_ENDRETDATO");

                entity.Property(e => e.EngFosterFaktor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("ENG_FOSTER_FAKTOR");

                entity.Property(e => e.EngFosterGodtype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_FOSTER_GODTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngFosterLoeTrinn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("ENG_FOSTER_LOE_TRINN")
                    .IsFixedLength();

                entity.Property(e => e.EngFosterUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_FOSTER_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_FRADATO");

                entity.Property(e => e.EngGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENG_GMLREFERANSE");

                entity.Property(e => e.EngGodFrikjoepFoster)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENG_GOD_FRIKJOEP_FOSTER");

                entity.Property(e => e.EngGodkjennunder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENG_GODKJENNUNDER");

                entity.Property(e => e.EngHuskeliste)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENG_HUSKELISTE");

                entity.Property(e => e.EngIndgodBesoek)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDGOD_BESOEK");

                entity.Property(e => e.EngIndgodFoster)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDGOD_FOSTER");

                entity.Property(e => e.EngIndgodLoenn)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDGOD_LOENN");

                entity.Property(e => e.EngIndutgBesoek)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDUTG_BESOEK");

                entity.Property(e => e.EngIndutgBil)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDUTG_BIL");

                entity.Property(e => e.EngIndutgFoster)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDUTG_FOSTER");

                entity.Property(e => e.EngIndutgLoenn)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDUTG_LOENN");

                entity.Property(e => e.EngIndutgPass)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_INDUTG_PASS");

                entity.Property(e => e.EngInngaattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_INNGAATTDATO");

                entity.Property(e => e.EngInterntStillingsnr)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ENG_INTERNT_STILLINGSNR");

                entity.Property(e => e.EngInterntStillingstekst)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENG_INTERNT_STILLINGSTEKST");

                entity.Property(e => e.EngKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_KLARGJORTDATO");

                entity.Property(e => e.EngKmUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_KM_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngKontaktperson)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ENG_KONTAKTPERSON");

                entity.Property(e => e.EngLoennGodtype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_LOENN_GODTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoennTimerMnd)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ENG_LOENN_TIMER_MND");

                entity.Property(e => e.EngLoennUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_LOENN_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoennUtgdprosent)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_LOENN_UTGDPROSENT");

                entity.Property(e => e.EngOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENG_OPPGAVER");

                entity.Property(e => e.EngOpphevunder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENG_OPPHEVUNDER");

                entity.Property(e => e.EngParkeringUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_PARKERING_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngParkeringsbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENG_PARKERINGSBELOEP");

                entity.Property(e => e.EngPassKmPrtur)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ENG_PASS_KM_PRTUR");

                entity.Property(e => e.EngPassKmgIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ENG_PASS_KMG_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.EngPassUtdektype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENG_PASS_UTDEKTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngRammebeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENG_RAMMEBELOEP");

                entity.Property(e => e.EngRapportfritekst)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("ENG_RAPPORTFRITEKST");

                entity.Property(e => e.EngRefusjonskandidat)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENG_REFUSJONSKANDIDAT");

                entity.Property(e => e.EngRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_REGISTRERTDATO");

                entity.Property(e => e.EngSatsBompenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_SATS_BOMPENGER");

                entity.Property(e => e.EngSatsParkering)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENG_SATS_PARKERING");

                entity.Property(e => e.EngStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ENG_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.EngStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_STOPPETDATO");

                entity.Property(e => e.EngTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENG_TILDATO");

                entity.Property(e => e.EngVeileder)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ENG_VEILEDER");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KmgIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KMG_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.LoeTrinn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("LOE_TRINN")
                    .IsFixedLength();

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_avgjortav_initialer");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_klargjortav_initialer");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_stoppetav_initialer");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementsavtales)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_ENGAS_DOKUMENT__FA_DOKUM");

                entity.HasOne(d => d.EngFosterLoeTrinnNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleEngFosterLoeTrinnNavigations)
                    .HasForeignKey(d => d.EngFosterLoeTrinn)
                    .HasConstraintName("FK_FA_ENGAS_REFERENCE_FA_LOENN");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementsavtales)
                    .HasForeignKey(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ENGAS_MEDARBEID_FA_MEDAR");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementsavtales)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ENGAS_KLIENT_EN_FA_KLIEN");

                entity.HasOne(d => d.KmgIdentNavigation)
                    .WithMany(p => p.FaEngasjementsavtales)
                    .HasForeignKey(d => d.KmgIdent)
                    .HasConstraintName("FK_FA_ENGAS_KMG_ENGAS_FA_KILOM");

                entity.HasOne(d => d.LoeTrinnNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleLoeTrinnNavigations)
                    .HasForeignKey(d => d.LoeTrinn)
                    .HasConstraintName("FK_FA_ENGAS_LOENNTRIN_FA_LOENN");

                entity.HasOne(d => d.SbhAvgjortavInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleSbhAvgjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhAvgjortavInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH2__FA_SAKSB");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKS3");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKS1");

                entity.HasOne(d => d.SbhKlargjortavInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleSbhKlargjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhKlargjortavInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH1__FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKS2");

                entity.HasOne(d => d.SbhStoppetavInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsavtaleSbhStoppetavInitialerNavigations)
                    .HasForeignKey(d => d.SbhStoppetavInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH3__FA_SAKSB");

                entity.HasOne(d => d.TilLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementsavtales)
                    .HasForeignKey(d => d.TilLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ENGAS_TILTAK_EN_FA_TILTA");

                entity.HasOne(d => d.Sak)
                    .WithMany(p => p.FaEngasjementsavtales)
                    .HasForeignKey(d => new { d.SakAar, d.SakJournalnr })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ENGAS_SAKSJOURN_FA_SAKSJ");
            });

            modelBuilder.Entity<FaEngasjementslinjer>(entity =>
            {
                entity.HasKey(e => e.EnlLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_ENGASJEMENTSLINJER");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_ENGASJEMENTSLINJER");

                entity.HasIndex(e => e.EnlTilbakefompostertLoepenr, "FK_FA_ENGASJEMENTSLINJER10")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.VikType, "FK_FA_ENGASJEMENTSLINJER11")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_ENGASJEMENTSLINJER12");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_ENGASJEMENTSLINJER13")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.UaaIdent, "FK_FA_ENGASJEMENTSLINJER14")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.RerLoepenr, "FK_FA_ENGASJEMENTSLINJER15")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhAnvistav, "FK_FA_ENGASJEMENTSLINJER2");

                entity.HasIndex(e => new { e.KtpNoekkel1, e.KtnKontonummer1 }, "FK_FA_ENGASJEMENTSLINJER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel2, e.KtnKontonummer2 }, "FK_FA_ENGASJEMENTSLINJER4")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel3, e.KtnKontonummer3 }, "FK_FA_ENGASJEMENTSLINJER5")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel4, e.KtnKontonummer4 }, "FK_FA_ENGASJEMENTSLINJER6")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel5, e.KtnKontonummer5 }, "FK_FA_ENGASJEMENTSLINJER7")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.KtpNoekkel6, e.KtnKontonummer6 }, "FK_FA_ENGASJEMENTSLINJER8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.EnpLoepenr, "FK_FA_ENGASJEMENTSLINJER9")
                    .HasFillFactor(80);

                entity.Property(e => e.EnlLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENL_LOEPENR");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EnlAntall)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("ENL_ANTALL");

                entity.Property(e => e.EnlAnvisPaagaar)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENL_ANVIS_PAAGAAR");

                entity.Property(e => e.EnlAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ENL_ANVISTAAR");

                entity.Property(e => e.EnlAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENL_ANVISTDATO");

                entity.Property(e => e.EnlAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENL_ANVISTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.EnlAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENL_ANVISTNR");

                entity.Property(e => e.EnlArbavgferiepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_ARBAVGFERIEPENGER");

                entity.Property(e => e.EnlArbgiveravgift)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_ARBGIVERAVGIFT");

                entity.Property(e => e.EnlBalanse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("ENL_BALANSE")
                    .IsFixedLength();

                entity.Property(e => e.EnlBegrTilbakefoert)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("ENL_BEGR_TILBAKEFOERT");

                entity.Property(e => e.EnlBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENL_BELOEP");

                entity.Property(e => e.EnlBeskrivelse)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ENL_BESKRIVELSE");

                entity.Property(e => e.EnlBetalingstype)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENL_BETALINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EnlBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENL_BILAGSSERIE")
                    .IsFixedLength();

                entity.Property(e => e.EnlEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENL_ENDRETDATO");

                entity.Property(e => e.EnlFeilbeskrivelse)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ENL_FEILBESKRIVELSE");

                entity.Property(e => e.EnlFeilkode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ENL_FEILKODE")
                    .IsFixedLength();

                entity.Property(e => e.EnlFeriepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_FERIEPENGER");

                entity.Property(e => e.EnlForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENL_FORFALLSDATO");

                entity.Property(e => e.EnlForsterkFaktor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("ENL_FORSTERK_FAKTOR");

                entity.Property(e => e.EnlKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("ENL_KONTONRMOTTAKER")
                    .IsFixedLength();

                entity.Property(e => e.EnlLoennsfildato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENL_LOENNSFILDATO");

                entity.Property(e => e.EnlLoennsfilnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENL_LOENNSFILNR");

                entity.Property(e => e.EnlMottakerAdresse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ENL_MOTTAKER_ADRESSE");

                entity.Property(e => e.EnlMottakerPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("ENL_MOTTAKER_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.EnlMottakerPoststed)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ENL_MOTTAKER_POSTSTED");

                entity.Property(e => e.EnlNyettertbf)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENL_NYETTERTBF")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EnlPeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ENL_PERIODEAAR");

                entity.Property(e => e.EnlPeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ENL_PERIODEMND");

                entity.Property(e => e.EnlPlanarbavgferiep)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_PLANARBAVGFERIEP");

                entity.Property(e => e.EnlPlanarbgiveravg)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_PLANARBGIVERAVG");

                entity.Property(e => e.EnlPlanferiepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_PLANFERIEPENGER");

                entity.Property(e => e.EnlPlanlagtantall)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("ENL_PLANLAGTANTALL");

                entity.Property(e => e.EnlPlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENL_PLANLAGTBELOEP");

                entity.Property(e => e.EnlPlanlagtforfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENL_PLANLAGTFORFALLSDATO");

                entity.Property(e => e.EnlPlanlagtsats)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_PLANLAGTSATS");

                entity.Property(e => e.EnlRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENL_REGISTRERTDATO");

                entity.Property(e => e.EnlSats)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_SATS");

                entity.Property(e => e.EnlSatsident)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("ENL_SATSIDENT")
                    .IsFixedLength();

                entity.Property(e => e.EnlSatstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ENL_SATSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EnlSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENL_SKREVET");

                entity.Property(e => e.EnlStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ENL_STATUS")
                    .HasDefaultValueSql("('OK ')")
                    .IsFixedLength();

                entity.Property(e => e.EnlTellnn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENL_TELLNN");

                entity.Property(e => e.EnlTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENL_TILBAKEFOERTBELOEP");

                entity.Property(e => e.EnlTilbakefompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENL_TILBAKEFOMPOSTERT_LOEPENR");

                entity.Property(e => e.EnlVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ENL_VILKAAROPPFYLT");

                entity.Property(e => e.EnpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENP_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.Property(e => e.RerLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("RER_LOEPENR");

                entity.Property(e => e.SbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_anvistav");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.Property(e => e.UaaIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UAA_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaEngasjementslinjers)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_ENGAS_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.EnlTilbakefompostertLoepenrNavigation)
                    .WithMany(p => p.InverseEnlTilbakefompostertLoepenrNavigation)
                    .HasForeignKey(d => d.EnlTilbakefompostertLoepenr)
                    .HasConstraintName("FK_FA_ENGAS_ENGLINJE__FA_ENGAS");

                entity.HasOne(d => d.EnpLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementslinjers)
                    .HasForeignKey(d => d.EnpLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ENGAS_ENGPLAN_E_FA_ENGAS");

                entity.HasOne(d => d.RerLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementslinjers)
                    .HasForeignKey(d => d.RerLoepenr)
                    .HasConstraintName("FK_REFUSJONSKRAV_ENGLINJER");

                entity.HasOne(d => d.SbhAnvistavNavigation)
                    .WithMany(p => p.FaEngasjementslinjerSbhAnvistavNavigations)
                    .HasForeignKey(d => d.SbhAnvistav)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKL2");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaEngasjementslinjerSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKL1");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaEngasjementslinjerSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKL3");

                entity.HasOne(d => d.UaaIdentNavigation)
                    .WithMany(p => p.FaEngasjementslinjers)
                    .HasForeignKey(d => d.UaaIdent)
                    .HasConstraintName("FK_FA_ENGAS_BETALINGS_FA_BETAL");

                entity.HasOne(d => d.VikTypeNavigation)
                    .WithMany(p => p.FaEngasjementslinjers)
                    .HasForeignKey(d => d.VikType)
                    .HasConstraintName("FK_FA_ENGAS_UTBETVILK_FA_UTBET");

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaEngasjementslinjerKts)
                    .HasForeignKey(d => new { d.KtpNoekkel1, d.KtnKontonummer1 })
                    .HasConstraintName("FK_FA_ENGAS_KONTO_ENG_FA_KONT1");

                entity.HasOne(d => d.KtNavigation)
                    .WithMany(p => p.FaEngasjementslinjerKtNavigations)
                    .HasForeignKey(d => new { d.KtpNoekkel2, d.KtnKontonummer2 })
                    .HasConstraintName("FK_FA_ENGAS_KONTO_ENG_FA_KONT2");

                entity.HasOne(d => d.Kt1)
                    .WithMany(p => p.FaEngasjementslinjerKt1s)
                    .HasForeignKey(d => new { d.KtpNoekkel3, d.KtnKontonummer3 })
                    .HasConstraintName("FK_FA_ENGAS_KONTO_ENG_FA_KONT3");

                entity.HasOne(d => d.Kt2)
                    .WithMany(p => p.FaEngasjementslinjerKt2s)
                    .HasForeignKey(d => new { d.KtpNoekkel4, d.KtnKontonummer4 })
                    .HasConstraintName("FK_FA_ENGAS_KONTO_ENG_FA_KONT4");

                entity.HasOne(d => d.Kt3)
                    .WithMany(p => p.FaEngasjementslinjerKt3s)
                    .HasForeignKey(d => new { d.KtpNoekkel5, d.KtnKontonummer5 })
                    .HasConstraintName("FK_FA_ENGAS_KONTO_ENG_FA_KONT5");

                entity.HasOne(d => d.Kt4)
                    .WithMany(p => p.FaEngasjementslinjerKt4s)
                    .HasForeignKey(d => new { d.KtpNoekkel6, d.KtnKontonummer6 })
                    .HasConstraintName("FK_FA_ENGAS_KONTO_ENG_FA_KONT6");
            });

            modelBuilder.Entity<FaEngasjementsplan>(entity =>
            {
                entity.HasKey(e => e.EnpLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_ENGASJEMENTSPLAN");

                entity.HasIndex(e => e.EngLoepenr, "FK_FA_ENGASJEMENTSPLAN")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.BktType, e.BktKode }, "FK_FA_ENGASJEMENTSPLAN2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhAvgjortavInitialer, "FK_FA_ENGASJEMENTSPLAN3");

                entity.HasIndex(e => e.SbhKlargjortavInitialer, "FK_FA_ENGASJEMENTSPLAN4");

                entity.HasIndex(e => e.SbhStoppetavInitialer, "FK_FA_ENGASJEMENTSPLAN5");

                entity.HasIndex(e => e.RefLoepenr, "FK_FA_ENGASJEMENTSPLAN6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.EnpStatus, "IX_FA_ENGASJEMENTSPLAN1")
                    .HasFillFactor(80);

                entity.Property(e => e.EnpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENP_LOEPENR");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.EnpAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_AARSAKER");

                entity.Property(e => e.EnpAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENP_AVGJORTDATO");

                entity.Property(e => e.EnpBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("ENP_BESKRIVELSE");

                entity.Property(e => e.EnpFormaal)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ENP_FORMAAL");

                entity.Property(e => e.EnpKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENP_KLARGJORTDATO");

                entity.Property(e => e.EnpOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_OPPGAVER");

                entity.Property(e => e.EnpPlantype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENP_PLANTYPE")
                    .IsFixedLength();

                entity.Property(e => e.EnpStatus)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ENP_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.EnpStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ENP_STOPPETDATO");

                entity.Property(e => e.EnpSumGodkjent)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENP_SUM_GODKJENT");

                entity.Property(e => e.EnpVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("ENP_VARIGHETFRA");

                entity.Property(e => e.EnpVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("ENP_VARIGHETTIL");

                entity.Property(e => e.RefLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("REF_LOEPENR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_avgjortav_initialer");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_klargjortav_initialer");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_stoppetav_initialer");

                entity.HasOne(d => d.EngLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementsplans)
                    .HasForeignKey(d => d.EngLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_ENGAS_ENGAVTALE_FA_ENGAS");

                entity.HasOne(d => d.RefLoepenrNavigation)
                    .WithMany(p => p.FaEngasjementsplans)
                    .HasForeignKey(d => d.RefLoepenr)
                    .HasConstraintName("FK_REFUSJONER_ENGPLAN");

                entity.HasOne(d => d.SbhAvgjortavInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsplanSbhAvgjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhAvgjortavInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKP2");

                entity.HasOne(d => d.SbhKlargjortavInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsplanSbhKlargjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhKlargjortavInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKP1");

                entity.HasOne(d => d.SbhStoppetavInitialerNavigation)
                    .WithMany(p => p.FaEngasjementsplanSbhStoppetavInitialerNavigations)
                    .HasForeignKey(d => d.SbhStoppetavInitialer)
                    .HasConstraintName("FK_FA_ENGAS_SAKSBEH_E_FA_SAKP3");

                entity.HasOne(d => d.Bkt)
                    .WithMany(p => p.FaEngasjementsplans)
                    .HasForeignKey(d => new { d.BktType, d.BktKode })
                    .HasConstraintName("FK_FA_ENGAS_BETKAT_EN_FA_BETAL");
            });

            modelBuilder.Entity<FaFlytvidersendstatus>(entity =>
            {
                entity.HasKey(e => e.FlySendRef)
                    .HasName("PK__FA_FLYTV__923203D75CE08802")
                    .IsClustered(false);

                entity.ToTable("FA_FLYTVIDERSENDSTATUS");

                entity.Property(e => e.FlySendRef)
                    .ValueGeneratedNever()
                    .HasColumnName("FLY_SEND_REF");

                entity.Property(e => e.FlyBeskrivelse)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("FLY_BESKRIVELSE");

                entity.Property(e => e.FlyStatus).HasColumnName("FLY_STATUS");
            });

            modelBuilder.Entity<FaForbindelser>(entity =>
            {
                entity.HasKey(e => e.ForLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_FORBINDELSER");

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_FORBINDELSER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ForAgressolevnr, "FK_FA_FORBINDELSER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.NasKodenr, "FK_FA_FORBINDELSER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_FORBINDELSER4");

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_FORBINDELSER5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_FORBINDELSER6");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_FORBINDELSER7")
                    .HasFillFactor(80);

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.ForAgressolevnr)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("FOR_AGRESSOLEVNR");

                entity.Property(e => e.ForArbeidssted)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("FOR_ARBEIDSSTED");

                entity.Property(e => e.ForBesoeksadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("FOR_BESOEKSADRESSE");

                entity.Property(e => e.ForBetalingsmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FOR_BETALINGSMAATE")
                    .IsFixedLength();

                entity.Property(e => e.ForDnummer)
                    .HasColumnType("numeric(11, 0)")
                    .HasColumnName("FOR_DNUMMER");

                entity.Property(e => e.ForEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FOR_EMAIL");

                entity.Property(e => e.ForEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("FOR_ENDRETDATO");

                entity.Property(e => e.ForEtternavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FOR_ETTERNAVN");

                entity.Property(e => e.ForFoedselsnummer)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("FOR_FOEDSELSNUMMER")
                    .IsFixedLength();

                entity.Property(e => e.ForFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FOR_FORNAVN");

                entity.Property(e => e.ForGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FOR_GMLREFERANSE");

                entity.Property(e => e.ForKontonummer)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("FOR_KONTONUMMER")
                    .IsFixedLength();

                entity.Property(e => e.ForLeverandoernr)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("FOR_LEVERANDOERNR")
                    .IsFixedLength();

                entity.Property(e => e.ForOrganisasjonsnr)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("FOR_ORGANISASJONSNR")
                    .IsFixedLength();

                entity.Property(e => e.ForPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("FOR_PASSIVISERTDATO");

                entity.Property(e => e.ForPostadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("FOR_POSTADRESSE");

                entity.Property(e => e.ForRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("FOR_REGISTRERTDATO");

                entity.Property(e => e.ForTelefaks)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("FOR_TELEFAKS")
                    .IsFixedLength();

                entity.Property(e => e.ForTelefonarbeid)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("FOR_TELEFONARBEID")
                    .IsFixedLength();

                entity.Property(e => e.ForTelefonmobil)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("FOR_TELEFONMOBIL")
                    .IsFixedLength();

                entity.Property(e => e.ForTelefonprivat)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("FOR_TELEFONPRIVAT")
                    .IsFixedLength();

                entity.Property(e => e.ForUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FOR_URL");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.NasKodenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("NAS_KODENR");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaForbindelsers)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_FORBI_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.ForAgressolevnrNavigation)
                    .WithMany(p => p.FaForbindelsers)
                    .HasForeignKey(d => d.ForAgressolevnr)
                    .HasConstraintName("FK_FA_FORB_AGRESSOLEV_FA_AGRES");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaForbindelsers)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_FORBI_KOMMUNER__FA_KOMMU");

                entity.HasOne(d => d.NasKodenrNavigation)
                    .WithMany(p => p.FaForbindelsers)
                    .HasForeignKey(d => d.NasKodenr)
                    .HasConstraintName("FK_FA_FORBI_NASJON_FO_FA_NASJO");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaForbindelsers)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_FORBI_POSTADRES_FA_POSTA");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaForbindelserSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_FORBI_SAKSBEH_F_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaForbindelserSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_FORBI_SAKBEH_FO_FA_SAKS2");

                entity.HasMany(d => d.FotIdents)
                    .WithMany(p => p.ForLoepenrs)
                    .UsingEntity<Dictionary<string, object>>(
                        "FaForbindelsesroller",
                        l => l.HasOne<FaForbindelsestyper>().WithMany().HasForeignKey("FotIdent").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_FORB2_FORBINDEL_FA_FORB"),
                        r => r.HasOne<FaForbindelser>().WithMany().HasForeignKey("ForLoepenr").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_FORBI1_FORBINDEL_FA_FORB"),
                        j =>
                        {
                            j.HasKey("ForLoepenr", "FotIdent").IsClustered(false);

                            j.ToTable("FA_FORBINDELSESROLLER");

                            j.HasIndex(new[] { "FotIdent" }, "FK_FORBINDELSEST1_FORBINDELSE").HasFillFactor(80);

                            j.HasIndex(new[] { "ForLoepenr" }, "FK_FORBINDELSEST2_FORBINDELSE2").HasFillFactor(80);

                            j.IndexerProperty<decimal>("ForLoepenr").HasColumnType("numeric(10, 0)").HasColumnName("FOR_LOEPENR");

                            j.IndexerProperty<string>("FotIdent").HasMaxLength(3).IsUnicode(false).HasColumnName("FOT_IDENT").IsFixedLength();
                        });
            });

            modelBuilder.Entity<FaForbindelsesadresser>(entity =>
            {
                entity.HasKey(e => new { e.ForLoepenr, e.FoaLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_FORBINDELSESADRESSER");

                entity.HasIndex(e => e.ForLoepenr, "FK_FORBINDELSESADRESSER")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhEndretav, "FK_FORBINDELSESADRESSER2");

                entity.HasIndex(e => e.PnrPostnr, "FK_FORBINDELSESADRESSER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FORBINDELSESADRESSER4")
                    .HasFillFactor(80);

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.FoaLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOA_LOEPENR");

                entity.Property(e => e.FoaBesoeksadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("FOA_BESOEKSADRESSE");

                entity.Property(e => e.FoaPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("FOA_PASSIVISERTDATO");

                entity.Property(e => e.FoaPostadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("FOA_POSTADRESSE");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaForbindelsesadressers)
                    .HasForeignKey(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_FORBI_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaForbindelsesadressers)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_FORBI_KOMMUNE_F_FA_KOMMU");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaForbindelsesadressers)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_FORBADR_POSTADRES_FA");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaForbindelsesadressers)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_FORBI_SBH_FORBA_FA_SAKSB");
            });

            modelBuilder.Entity<FaForbindelsestyper>(entity =>
            {
                entity.HasKey(e => e.FotIdent)
                    .IsClustered(false);

                entity.ToTable("FA_FORBINDELSESTYPER");

                entity.Property(e => e.FotIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("FOT_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.FotBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FOT_BESKRIVELSE");

                entity.Property(e => e.FotPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("FOT_PASSIVISERTDATO");

                entity.Property(e => e.FotPersonrelatert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("FOT_PERSONRELATERT");
            });

            modelBuilder.Entity<FaFriTiltakstype>(entity =>
            {
                entity.HasKey(e => new { e.TttTiltakstype, e.FttFriTiltakstype })
                    .IsClustered(false);

                entity.ToTable("FA_FRI_TILTAKSTYPE");

                entity.HasIndex(e => e.TttTiltakstype, "FK_FA_FRI_TILTAKSTYPE")
                    .HasFillFactor(80);

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TTT_TILTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.FttFriTiltakstype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FTT_FRI_TILTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.FttBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FTT_BESKRIVELSE");

                entity.HasOne(d => d.TttTiltakstypeNavigation)
                    .WithMany(p => p.FaFriTiltakstypes)
                    .HasForeignKey(d => d.TttTiltakstype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_FRI_T_TILTAKSTY_FA_TILTA");
            });

            modelBuilder.Entity<FaFristoversittelser>(entity =>
            {
                entity.HasKey(e => new { e.FroType, e.FroKode })
                    .IsClustered(false);

                entity.ToTable("FA_FRISTOVERSITTELSER");

                entity.Property(e => e.FroType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FRO_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.FroKode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE")
                    .IsFixedLength();

                entity.Property(e => e.FroBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("FRO_BESKRIVELSE");
            });

            modelBuilder.Entity<FaGenerellsak>(entity =>
            {
                entity.HasKey(e => new { e.GsaAar, e.GsaJournalnr })
                    .IsClustered(false);

                entity.ToTable("FA_GENERELLSAK");

                entity.HasIndex(e => new { e.GsaErstattetavAar, e.GsaErstattetavJournalnr }, "FK_FA_GENERELLSAK1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SatSakstype, "FK_FA_GENERELLSAK10")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_GENERELLSAK11");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_GENERELLSAK12")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_GENERELLSAK2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.MynVedtakstype, "FK_FA_GENERELLSAK3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SokLoepenr, "FK_FA_GENERELLSAK4")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_GENERELLSAK5");

                entity.HasIndex(e => e.SbhInitialer2, "FK_FA_GENERELLSAK6");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_GENERELLSAK7");

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_GENERELLSAK8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_GENERELLSAK9")
                    .HasFillFactor(80);

                entity.Property(e => e.GsaAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("GSA_AAR");

                entity.Property(e => e.GsaJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("GSA_JOURNALNR");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.GsaAnbefaling)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("GSA_ANBEFALING");

                entity.Property(e => e.GsaAntallbarn)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("GSA_ANTALLBARN");

                entity.Property(e => e.GsaDato)
                    .HasColumnType("datetime")
                    .HasColumnName("GSA_DATO");

                entity.Property(e => e.GsaEmne)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("GSA_EMNE");

                entity.Property(e => e.GsaEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("GSA_ENDRETDATO");

                entity.Property(e => e.GsaErstattetavAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("GSA_erstattetav_AAR");

                entity.Property(e => e.GsaErstattetavJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("GSA_erstattetav_JOURNALNR");

                entity.Property(e => e.GsaKonklusjon)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("GSA_KONKLUSJON");

                entity.Property(e => e.GsaKonklusjonsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("GSA_KONKLUSJONSDATO");

                entity.Property(e => e.GsaRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("GSA_REGISTRERTDATO");

                entity.Property(e => e.GsaSbhKommune)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GSA_SBH_KOMMUNE");

                entity.Property(e => e.GsaStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("GSA_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.GsaVideresendtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("GSA_VIDERESENDTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.MynVedtakstype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MYN_VEDTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.SatSakstype)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SAT_SAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhInitialer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer2");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.Property(e => e.SokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SOK_LOEPENR");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_GENER_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_GENER_KLIENT_GE_FA_KLIEN");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_GENER_KOMMUNE_G_FA_KOMMU");

                entity.HasOne(d => d.MynVedtakstypeNavigation)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => d.MynVedtakstype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_GENER_VEDTAKSMY_FA_VEDTA");

                entity.HasOne(d => d.SatSakstypeNavigation)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => d.SatSakstype)
                    .HasConstraintName("FK_FA_GENER_SAKSTYPE__FA_SAKST");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaGenerellsakSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_GENER_SAKSBEH3__FA_SAKSB");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaGenerellsakSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_GENER_SAKSBEH1__FA_SAKSB");

                entity.HasOne(d => d.SbhInitialer2Navigation)
                    .WithMany(p => p.FaGenerellsakSbhInitialer2Navigations)
                    .HasForeignKey(d => d.SbhInitialer2)
                    .HasConstraintName("FK_FA_GENER_SAKSBEH2__FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaGenerellsakSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_GENER_SAKSBEH4__FA_SAKSB");

                entity.HasOne(d => d.SokLoepenrNavigation)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => d.SokLoepenr)
                    .HasConstraintName("FK_FA_GENER_SOEKER_GE_FA_SOEKE");

                entity.HasOne(d => d.GsaErstattetav)
                    .WithMany(p => p.InverseGsaErstattetav)
                    .HasForeignKey(d => new { d.GsaErstattetavAar, d.GsaErstattetavJournalnr })
                    .HasConstraintName("FK_FA_GENER_GENERELLS_FA_GENER");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaGenerellsaks)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_FA_GENER_POSTJOURN_FA_POSTJ");
            });

            modelBuilder.Entity<FaHuskelapp>(entity =>
            {
                entity.HasKey(e => e.HusLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_HUSKELAPP");

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_HUSKELAPP1");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_HUSKELAPP2");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_HUSKELAPP3");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_HUSKELAPP4")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.SakAar, e.SakJournalnr }, "FK_FA_HUSKELAPP5")
                    .HasFillFactor(80);

                entity.Property(e => e.HusLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("HUS_LOEPENR");

                entity.Property(e => e.HusDato)
                    .HasColumnType("datetime")
                    .HasColumnName("HUS_DATO");

                entity.Property(e => e.HusEmne)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("HUS_EMNE");

                entity.Property(e => e.HusEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("HUS_ENDRETDATO");

                entity.Property(e => e.HusFrist)
                    .HasColumnType("datetime")
                    .HasColumnName("HUS_FRIST");

                entity.Property(e => e.HusInternt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("HUS_INTERNT");

                entity.Property(e => e.HusNotat)
                    .HasColumnType("text")
                    .HasColumnName("HUS_NOTAT");

                entity.Property(e => e.HusOppfoelging)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("HUS_OPPFOELGING");

                entity.Property(e => e.HusOppfoelgingsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("HUS_OPPFOELGINGSDATO");

                entity.Property(e => e.HusOppfyltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("HUS_OPPFYLTDATO");

                entity.Property(e => e.HusRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("HUS_REGISTRERTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaHuskelapps)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_HUSKE_KLIENT_HU_FA_KLIEN");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaHuskelappSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_HUSKE_SAKSBEH_H_FA_SAKS2");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaHuskelappSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_HUSKE_SAKSBEH_H_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaHuskelappSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_HUSKE_SAKSBEH_H_FA_SAKS3");

                entity.HasOne(d => d.Sak)
                    .WithMany(p => p.FaHuskelapps)
                    .HasForeignKey(d => new { d.SakAar, d.SakJournalnr })
                    .HasConstraintName("FK_FA_HUSKE_SAKSJOURN_FA_SAKSJ");
            });

            modelBuilder.Entity<FaInntutg>(entity =>
            {
                entity.HasKey(e => e.InuLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_INNTUTG");

                entity.HasIndex(e => e.VurLoepenr, "FK_FA_INNTUTG1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.IutType, e.IutKode }, "FK_FA_INNTUTG2")
                    .HasFillFactor(80);

                entity.Property(e => e.InuLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("INU_LOEPENR");

                entity.Property(e => e.InuBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("INU_BELOEP");

                entity.Property(e => e.InuHvem)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("INU_HVEM")
                    .IsFixedLength();

                entity.Property(e => e.InuMerknad)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("INU_MERKNAD");

                entity.Property(e => e.IutKode)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("IUT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.IutType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IUT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.VurLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("VUR_LOEPENR");

                entity.HasOne(d => d.VurLoepenrNavigation)
                    .WithMany(p => p.FaInntutgs)
                    .HasForeignKey(d => d.VurLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_INNTU_VURDEGENB_FA_VURDE");

                entity.HasOne(d => d.Iut)
                    .WithMany(p => p.FaInntutgs)
                    .HasForeignKey(d => new { d.IutType, d.IutKode })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_INNTU_IUTTYPE_I_FA_INNTU");
            });

            modelBuilder.Entity<FaInntutgtype>(entity =>
            {
                entity.HasKey(e => new { e.IutType, e.IutKode })
                    .IsClustered(false);

                entity.ToTable("FA_INNTUTGTYPE");

                entity.Property(e => e.IutType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IUT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.IutKode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("IUT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.IutBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("IUT_BESKRIVELSE");

                entity.Property(e => e.IutPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("IUT_PASSIVISERTDATO");

                entity.Property(e => e.IutStandard)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("IUT_STANDARD");
            });

            modelBuilder.Entity<FaInteresser>(entity =>
            {
                entity.HasKey(e => e.IntIdent)
                    .IsClustered(false);

                entity.ToTable("FA_INTERESSER");

                entity.Property(e => e.IntIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("INT_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.IntBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("INT_BESKRIVELSE");
            });

            modelBuilder.Entity<FaJournal>(entity =>
            {
                entity.HasKey(e => e.JouLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_JOURNAL");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_JOURNAL1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_JOURNAL2");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_JOURNAL3");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_JOURNAL4");

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_JOURNAL5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.JotIdent, "FK_FA_JOURNAL6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_JOURNAL7")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.SakUnndrAar, e.SakUnndrJournalnr }, "FK_FA_JOURNAL8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_JOURNAL9")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.JouFrist, "IX_FA_JOURNAL1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.JouOppfyltdato, "IX_FA_JOURNAL2")
                    .HasFillFactor(80);

                entity.Property(e => e.JouLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("JOU_LOEPENR");

                entity.Property(e => e.ArkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DATO");

                entity.Property(e => e.ArkJourSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_JOUR_SYSTEMID");

                entity.Property(e => e.ArkSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_SJEKK_I_VSA");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.JotIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("JOT_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.JouBegrSlettet)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("JOU_BEGR_SLETTET");

                entity.Property(e => e.JouDatonotat)
                    .HasColumnType("datetime")
                    .HasColumnName("JOU_DATONOTAT");

                entity.Property(e => e.JouDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("JOU_DOKUMENTNR");

                entity.Property(e => e.JouEmne)
                    .IsRequired()
                    .HasMaxLength(126)
                    .IsUnicode(false)
                    .HasColumnName("JOU_EMNE");

                entity.Property(e => e.JouEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("JOU_ENDRETDATO");

                entity.Property(e => e.JouEttdokument)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOU_ETTDOKUMENT");

                entity.Property(e => e.JouFerdigdato)
                    .HasColumnType("datetime")
                    .HasColumnName("JOU_FERDIGDATO");

                entity.Property(e => e.JouFrist)
                    .HasColumnType("datetime")
                    .HasColumnName("JOU_FRIST");

                entity.Property(e => e.JouGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JOU_GMLREFERANSE");

                entity.Property(e => e.JouMinutterforbruk)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("JOU_MINUTTERFORBRUK");

                entity.Property(e => e.JouNotat)
                    .HasColumnType("text")
                    .HasColumnName("JOU_NOTAT");

                entity.Property(e => e.JouOppfoelging)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("JOU_OPPFOELGING")
                    .IsFixedLength();

                entity.Property(e => e.JouOppfyltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("JOU_OPPFYLTDATO");

                entity.Property(e => e.JouRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("JOU_REGISTRERTDATO");

                entity.Property(e => e.JouSlettet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOU_SLETTET");

                entity.Property(e => e.JouTimerforbruk)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("JOU_TIMERFORBRUK");

                entity.Property(e => e.JouUnndrattbegrunnelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("JOU_UNNDRATTBEGRUNNELSE");

                entity.Property(e => e.JouUnndrattinnsyn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOU_UNNDRATTINNSYN");

                entity.Property(e => e.JouUnndrattinnsynIs)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOU_UNNDRATTINNSYN_IS");

                entity.Property(e => e.JouVurderUnndratt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOU_VURDER_UNNDRATT");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.SakUnndrAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_unndr_AAR");

                entity.Property(e => e.SakUnndrJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_unndr_JOURNALNR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaJournals)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_JOURN_DOKUMENT__FA_DOKUM");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaJournals)
                    .HasForeignKey(d => d.ForLoepenr)
                    .HasConstraintName("FK_FA_JOURN_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.JotIdentNavigation)
                    .WithMany(p => p.FaJournals)
                    .HasForeignKey(d => d.JotIdent)
                    .HasConstraintName("FK_FA_JOURN_JOURNALTY_FA_JOURN");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaJournals)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_JOURN_KLIENT_JO_FA_KLIEN");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaJournals)
                    .HasForeignKey(d => d.ProLoepenr)
                    .HasConstraintName("FK_FA_JOURN_PROSJEKT__FA_PROSJ");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaJournalSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_JOURN_SAKSBEH_J_FA_SAKS2");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaJournalSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_JOURN_SAKSBEH_J_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaJournalSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_JOURN_SAKSBEH_J_FA_SAKS3");

                entity.HasOne(d => d.SakUnndr)
                    .WithMany(p => p.FaJournals)
                    .HasForeignKey(d => new { d.SakUnndrAar, d.SakUnndrJournalnr })
                    .HasConstraintName("FK_FA_JOURN_SAK_JOURN_FA_SAKSJ");
            });

            modelBuilder.Entity<FaJournaltype>(entity =>
            {
                entity.HasKey(e => e.JotIdent)
                    .IsClustered(false);

                entity.ToTable("FA_JOURNALTYPE");

                entity.Property(e => e.JotIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("JOT_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.JotBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("JOT_BESKRIVELSE");

                entity.Property(e => e.JotForbindelse)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOT_FORBINDELSE");

                entity.Property(e => e.JotFosterhjemsbesoek)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOT_FOSTERHJEMSBESOEK");

                entity.Property(e => e.JotIntern)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOT_INTERN");

                entity.Property(e => e.JotPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("JOT_PASSIVISERTDATO");

                entity.Property(e => e.JotTilarkiv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOT_TILARKIV");

                entity.Property(e => e.JotTilsynsbesoek)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("JOT_TILSYNSBESOEK");
            });

            modelBuilder.Entity<FaKilometergodtgjoerelse>(entity =>
            {
                entity.HasKey(e => e.KmgIdent)
                    .IsClustered(false);

                entity.ToTable("FA_KILOMETERGODTGJOERELSE");

                entity.Property(e => e.KmgIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KMG_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KmgBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KMG_BESKRIVELSE");
            });

            modelBuilder.Entity<FaKkoder>(entity =>
            {
                entity.HasKey(e => e.KkoKode)
                    .IsClustered(false);

                entity.ToTable("FA_KKODER");

                entity.Property(e => e.KkoKode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KKO_KODE")
                    .IsFixedLength();

                entity.Property(e => e.KkoBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KKO_BESKRIVELSE");

                entity.Property(e => e.KkoKategori)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KKO_KATEGORI")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaKlient>(entity =>
            {
                entity.HasKey(e => e.KliLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_KLIENT");

                entity.HasIndex(e => e.KgrGruppeid, "FK_FA_KLIENT1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomPlassert, "FK_FA_KLIENT10")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.DisDistriktskode, e.RodIdent }, "FK_FA_KLIENT11")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliHusstandnr, "FK_FA_KLIENT12")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer2, "FK_FA_KLIENT13");

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_KLIENT2");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_KLIENT3");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_KLIENT4");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_KLIENT5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_KLIENT6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_KLIENT7")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.NasKodenr, "FK_FA_KLIENT8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliFamilienr, "FK_FA_KLIENT9")
                    .HasFillFactor(80);

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.ArkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DATO");

                entity.Property(e => e.ArkFnrEndret)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_FNR_ENDRET");

                entity.Property(e => e.ArkMappeSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_MAPPE_SYSTEMID");

                entity.Property(e => e.ArkNavnEndret)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_NAVN_ENDRET");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KgrGruppeid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KGR_GRUPPEID")
                    .IsFixedLength();

                entity.Property(e => e.KliAdresse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KLI_ADRESSE");

                entity.Property(e => e.KliAktiv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KLI_AKTIV")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.KliAvsluttetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_AVSLUTTETDATO");

                entity.Property(e => e.KliBehandlingsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_BEHANDLINGSDATO");

                entity.Property(e => e.KliBufetatnr)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("KLI_BUFETATNR");

                entity.Property(e => e.KliEmail)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("KLI_EMAIL");

                entity.Property(e => e.KliEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_ENDRETDATO");

                entity.Property(e => e.KliEtternavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_ETTERNAVN");

                entity.Property(e => e.KliFamilienr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_familienr");

                entity.Property(e => e.KliFamiliestatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FAMILIESTATUS")
                    .IsFixedLength();

                entity.Property(e => e.KliFlyktningestatus)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FLYKTNINGESTATUS")
                    .IsFixedLength();

                entity.Property(e => e.KliFlyttetfrabydel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FLYTTETFRABYDEL");

                entity.Property(e => e.KliFlyttetfradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_FLYTTETFRADATO");

                entity.Property(e => e.KliFlyttetktrlskjema)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KLI_FLYTTETKTRLSKJEMA");

                entity.Property(e => e.KliFlyttettilbydel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FLYTTETTILBYDEL");

                entity.Property(e => e.KliFlyttettildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_FLYTTETTILDATO");

                entity.Property(e => e.KliFoedselsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_FOEDSELSDATO");

                entity.Property(e => e.KliFornavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FORNAVN");

                entity.Property(e => e.KliFraannenkommune)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KLI_FRAANNENKOMMUNE");

                entity.Property(e => e.KliFremmedkontrollnr)
                    .HasColumnType("numeric(14, 0)")
                    .HasColumnName("KLI_FREMMEDKONTROLLNR");

                entity.Property(e => e.KliFylkesmannKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FYLKESMANN_KATEGORI")
                    .HasDefaultValueSql("('139')")
                    .IsFixedLength();

                entity.Property(e => e.KliFylkesmannStatus)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FYLKESMANN_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.KliFymStatusgendato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_FYM_STATUSGENDATO");

                entity.Property(e => e.KliFymStatusprdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_FYM_STATUSPRDATO");

                entity.Property(e => e.KliGatenavn)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("KLI_GATENAVN");

                entity.Property(e => e.KliGatenr)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KLI_GATENR")
                    .IsFixedLength();

                entity.Property(e => e.KliGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KLI_GMLREFERANSE");

                entity.Property(e => e.KliHusstandnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_husstandnr");

                entity.Property(e => e.KliInnvandrerbarn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KLI_INNVANDRERBARN");

                entity.Property(e => e.KliKjoenn)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KLI_KJOENN")
                    .IsFixedLength();

                entity.Property(e => e.KliKontonr)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("KLI_KONTONR")
                    .IsFixedLength();

                entity.Property(e => e.KliLeverandoernr)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("KLI_LEVERANDOERNR")
                    .IsFixedLength();

                entity.Property(e => e.KliMerknader)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("KLI_MERKNADER");

                entity.Property(e => e.KliOpenReasonNotSbh)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("KLI_OPEN_REASON_NOT_SBH");

                entity.Property(e => e.KliEtterverndato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_ETTERVERNDATO");

                entity.Property(e => e.KliEttervern)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KLI_ETTERVERN");

                entity.Property(e => e.KliTiltaksbarndato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_TILTAKSBARNDATO");

                entity.Property(e => e.KliTiltaksbarn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KLI_TILTAKSBARN");

                entity.Property(e => e.KliPersonnr)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("KLI_PERSONNR");

                entity.Property(e => e.KliRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_REGISTRERTDATO");

                entity.Property(e => e.KliSistbruktedoknr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_SISTBRUKTEDOKNR");

                entity.Property(e => e.KliStatsborgerskap)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KLI_STATSBORGERSKAP")
                    .IsFixedLength();

                entity.Property(e => e.KliStatsborgerskapKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KLI_STATSBORGERSKAP_KATEGORI")
                    .HasDefaultValueSql("('106')")
                    .IsFixedLength();

                entity.Property(e => e.KliTelefonarbeid)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KLI_TELEFONARBEID")
                    .IsFixedLength();

                entity.Property(e => e.KliTelefonmobil)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KLI_TELEFONMOBIL")
                    .IsFixedLength();

                entity.Property(e => e.KliTelefonprivat)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KLI_TELEFONPRIVAT")
                    .IsFixedLength();

                entity.Property(e => e.KliTidligeretiltak)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KLI_TIDLIGERETILTAK")
                    .IsFixedLength();

                entity.Property(e => e.KliUsavsluttetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_USAVSLUTTETDATO");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.KomPlassert)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_plassert");

                entity.Property(e => e.NasKodenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("NAS_KODENR");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.RodIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ROD_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhInitialer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer2");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaKlients)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_KLIEN_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.KgrGruppe)
                    .WithMany(p => p.FaKlients)
                    .HasForeignKey(d => d.KgrGruppeid)
                    .HasConstraintName("FK_FA_KLIEN_KLIENTGRU_FA_KLIEN");

                entity.HasOne(d => d.KliFamilienrNavigation)
                    .WithMany(p => p.InverseKliFamilienrNavigation)
                    .HasForeignKey(d => d.KliFamilienr)
                    .HasConstraintName("FK_FA_KLIEN_KLIENT_KL_FA_KLIEN");

                entity.HasOne(d => d.KliHusstandnrNavigation)
                    .WithMany(p => p.InverseKliHusstandnrNavigation)
                    .HasForeignKey(d => d.KliHusstandnr)
                    .HasConstraintName("FK_FA_KLIEN_KLIENT_KL_FA_KLIE2");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaKlientKomKommunenrNavigations)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_KLIEN_KOMMUNE_K_FA_KOMMU");

                entity.HasOne(d => d.KomPlassertNavigation)
                    .WithMany(p => p.FaKlientKomPlassertNavigations)
                    .HasForeignKey(d => d.KomPlassert)
                    .HasConstraintName("FK_FA_KLIEN_KOMMUNENR_FA_KOMMU");

                entity.HasOne(d => d.NasKodenrNavigation)
                    .WithMany(p => p.FaKlients)
                    .HasForeignKey(d => d.NasKodenr)
                    .HasConstraintName("FK_FA_KLIEN_NASJON_KL_FA_NASJO");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaKlients)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_KLIEN_POSTADR_K_FA_POSTA");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaKlientSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_KLIEN_SBH_KLIEN_FA_SAKS2");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaKlientSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_KLIEN_SAKSBEH_K_FA_SAKSB");

                entity.HasOne(d => d.SbhInitialer2Navigation)
                    .WithMany(p => p.FaKlientSbhInitialer2Navigations)
                    .HasForeignKey(d => d.SbhInitialer2)
                    .HasConstraintName("FK_FA_KLIEN_SAKSBEH2__FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaKlientSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_KLIEN_SBH_KLIEN_FA_SAKSB");

                entity.HasOne(d => d.FaRoder)
                    .WithMany(p => p.FaKlients)
                    .HasForeignKey(d => new { d.DisDistriktskode, d.RodIdent })
                    .HasConstraintName("FK_FA_KLIEN_RODER_KLI_FA_RODER");
            });

            modelBuilder.Entity<FaKlientSbhHistorikk>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.KshLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_KLIENT_SBH_HISTORIKK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KshLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KSH_LOEPENR");

                entity.Property(e => e.KshEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KSH_ENDRETDATO");

                entity.Property(e => e.KshSbh1Newname)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("KSH_SBH1_NEWNAME");

                entity.Property(e => e.KshSbh1Oldname)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("KSH_SBH1_OLDNAME");

                entity.Property(e => e.KshSbh2Newname)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("KSH_SBH2_NEWNAME");

                entity.Property(e => e.KshSbh2Oldname)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("KSH_SBH2_OLDNAME");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhInitialer1New)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER1_NEW");

                entity.Property(e => e.SbhInitialer1Old)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER1_OLD");

                entity.Property(e => e.SbhInitialer2New)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER2_NEW");

                entity.Property(e => e.SbhInitialer2Old)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER2_OLD");
            });

            modelBuilder.Entity<FaKlientadresser>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.PahLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_KLIENTADRESSER");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_KLIENTADRESSER")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_KLIENTADRESSER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_KLIENTADRESSER3");

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_KLIENTADRESSER4")
                    .HasFillFactor(80);

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PahLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PAH_LOEPENR");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.PahAdresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("PAH_ADRESSE");

                entity.Property(e => e.PahPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PAH_PASSIVISERTDATO");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaKlientadressers)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KLIENTADR_KL_FA_KLIEN");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaKlientadressers)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_KLIENTADR_KOMMUNE_K_FA");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaKlientadressers)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_KLIEN_POSTADRES_FA_POSTA");
            });

            modelBuilder.Entity<FaKlientgrupper>(entity =>
            {
                entity.HasKey(e => e.KgrGruppeid)
                    .IsClustered(false);

                entity.ToTable("FA_KLIENTGRUPPER");

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonummer }, "FK_FA_KLIENTGRUPPER1")
                    .HasFillFactor(80);

                entity.Property(e => e.KgrGruppeid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KGR_GRUPPEID")
                    .IsFixedLength();

                entity.Property(e => e.KgrBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KGR_BESKRIVELSE");

                entity.Property(e => e.KgrPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KGR_PASSIVISERTDATO");

                entity.Property(e => e.KtnKontonummer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaKlientgruppers)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonummer })
                    .HasConstraintName("FK_FA_KLIEN_KONTOER_K_FA_KONTO");
            });

            modelBuilder.Entity<FaKlientinteresser>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.IntIdent })
                    .IsClustered(false);

                entity.ToTable("FA_KLIENTINTERESSER");

                entity.HasIndex(e => e.IntIdent, "FK_FA_KLIENTINTERESSER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_KLIENTINTERESSER2")
                    .HasFillFactor(80);

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.IntIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("INT_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KinKommentar)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("KIN_KOMMENTAR");

                entity.HasOne(d => d.IntIdentNavigation)
                    .WithMany(p => p.FaKlientinteressers)
                    .HasForeignKey(d => d.IntIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KLIEN_INTERESSE_FA_INTER");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaKlientinteressers)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KLIEN_KLIENT_IN_FA_KLIEN");
            });

            modelBuilder.Entity<FaKlientplassering>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.KplFradato })
                    .IsClustered(false);

                entity.ToTable("FA_KLIENTPLASSERING");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_KLIENTPLASSERING1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_KLIENTPLASSERING2")
                    .HasFillFactor(80);

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KplFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KPL_FRADATO");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.KplBorhos)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KPL_BORHOS")
                    .IsFixedLength();

                entity.Property(e => e.KplBorhosKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KPL_BORHOS_KATEGORI")
                    .HasDefaultValueSql("('109')")
                    .IsFixedLength();

                entity.Property(e => e.KplMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("KPL_MERKNAD");

                entity.Property(e => e.KplOppfoelgingsbesoek)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KPL_OPPFOELGINGSBESOEK")
                    .HasDefaultValueSql("((4))");

                entity.Property(e => e.KplPlasseringbor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KPL_PLASSERINGBOR")
                    .IsFixedLength();

                entity.Property(e => e.KplPlasseringborKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KPL_PLASSERINGBOR_KATEGORI")
                    .HasDefaultValueSql("('108')")
                    .IsFixedLength();

                entity.Property(e => e.KplPlassert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KPL_PLASSERT");

                entity.Property(e => e.KplPlassertannenbydel)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KPL_PLASSERTANNENBYDEL");

                entity.Property(e => e.KplTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KPL_TILDATO");

                entity.Property(e => e.KplTilsynsansvarSelv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KPL_TILSYNSANSVAR_SELV");

                entity.Property(e => e.KplTilsynsbesoek)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KPL_TILSYNSBESOEK")
                    .HasDefaultValueSql("((4))");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaKlientplasserings)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KLIEN_KLIENT_PL_FA_KLIEN");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaKlientplasserings)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_KLIEN_KOMMUNE_P_FA_KOMMU");
            });

            modelBuilder.Entity<FaKlienttilknytning>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.KtkLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_KLIENTTILKNYTNING");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_KLIENTTILKNYTNING1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_KLIENTTILKNYTNING2")
                    .HasFillFactor(80);

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtkLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KTK_LOEPENR");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KtkDagligomsorg)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTK_DAGLIGOMSORG");

                entity.Property(e => e.KtkDeltbosted)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTK_DELTBOSTED");

                entity.Property(e => e.KtkDoeddato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTK_DOEDDATO");

                entity.Property(e => e.KtkForeldreansvar)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTK_FORELDREANSVAR");

                entity.Property(e => e.KtkForesatt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTK_FORESATT");

                entity.Property(e => e.KtkFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTK_FRADATO");

                entity.Property(e => e.KtkHovedperson)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTK_HOVEDPERSON");

                entity.Property(e => e.KtkMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("KTK_MERKNAD");

                entity.Property(e => e.KtkNypartner)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("KTK_NYPARTNER");

                entity.Property(e => e.KtkPartsrettighet)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KTK_PARTSRETTIGHET")
                    .HasDefaultValueSql("('I')")
                    .IsFixedLength();

                entity.Property(e => e.KtkPartsrettighetKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KTK_PARTSRETTIGHET_KATEGORI")
                    .HasDefaultValueSql("('117')")
                    .IsFixedLength();

                entity.Property(e => e.KtkRolle)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KTK_ROLLE")
                    .IsFixedLength();

                entity.Property(e => e.KtkRolleKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KTK_ROLLE_KATEGORI")
                    .HasDefaultValueSql("('104')")
                    .IsFixedLength();

                entity.Property(e => e.KtkTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTK_TILDATO");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaKlienttilknytnings)
                    .HasForeignKey(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KLIEN_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaKlienttilknytnings)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KLIENT_KLIENTTILKNYT");
            });

            modelBuilder.Entity<FaKmsatser>(entity =>
            {
                entity.HasKey(e => new { e.KmgIdent, e.KmsFradato })
                    .IsClustered(false);

                entity.ToTable("FA_KMSATSER");

                entity.HasIndex(e => e.KmgIdent, "FK_FA_KMSATSER1")
                    .HasFillFactor(80);

                entity.Property(e => e.KmgIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KMG_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KmsFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KMS_FRADATO");

                entity.Property(e => e.KmsSats)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("KMS_SATS");

                entity.Property(e => e.KmsTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KMS_TILDATO");

                entity.HasOne(d => d.KmgIdentNavigation)
                    .WithMany(p => p.FaKmsatsers)
                    .HasForeignKey(d => d.KmgIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KMSAT_KMG_KMSAT_FA_KILOM");
            });

            modelBuilder.Entity<FaKodeverk>(entity =>
            {
                entity.HasKey(e => new { e.KodKategori, e.KodKode })
                    .IsClustered(false);

                entity.ToTable("FA_KODEVERK");

                entity.Property(e => e.KodKategori)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KOD_KATEGORI")
                    .IsFixedLength();

                entity.Property(e => e.KodKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KOD_KODE")
                    .IsFixedLength();

                entity.Property(e => e.KodKorttekst)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KOD_KORTTEKST");

                entity.Property(e => e.KodLangtekst)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("KOD_LANGTEKST");

                entity.Property(e => e.KodSsbkode)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KOD_SSBKODE");

                entity.Property(e => e.KodSubcategory).HasColumnName("KOD_SUBCATEGORY");
            });

            modelBuilder.Entity<FaKommuner>(entity =>
            {
                entity.HasKey(e => e.KomKommunenr)
                    .IsClustered(false);

                entity.ToTable("FA_KOMMUNER");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.KomKommunenavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KOM_KOMMUNENAVN");

                entity.Property(e => e.KomPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KOM_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaKontaktpersoner>(entity =>
            {
                entity.HasKey(e => new { e.ForLoepenr, e.KpeLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_KONTAKTPERSONER");

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_KONTAKTPERSONER1")
                    .HasFillFactor(80);

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KpeLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KPE_LOEPENR");

                entity.Property(e => e.KpeEtternavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KPE_ETTERNAVN");

                entity.Property(e => e.KpeFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KPE_FORNAVN");

                entity.Property(e => e.KpeStilling)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KPE_STILLING");

                entity.Property(e => e.KpeTelefaks)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KPE_TELEFAKS")
                    .IsFixedLength();

                entity.Property(e => e.KpeTelefonarbeid)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KPE_TELEFONARBEID")
                    .IsFixedLength();

                entity.Property(e => e.KpeTelefonmobil)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KPE_TELEFONMOBIL")
                    .IsFixedLength();

                entity.Property(e => e.KpeTelefonprivat)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("KPE_TELEFONPRIVAT")
                    .IsFixedLength();

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaKontaktpersoners)
                    .HasForeignKey(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KONTA_FORBINDEL_FA_FORBI");
            });

            modelBuilder.Entity<FaKontoer>(entity =>
            {
                entity.HasKey(e => new { e.KtpNoekkel, e.KtnKontonummer })
                    .IsClustered(false);

                entity.ToTable("FA_KONTOER");

                entity.HasIndex(e => e.KtpNoekkel, "FK_FA_KONTOER1")
                    .HasFillFactor(80);

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer")
                    .IsFixedLength();

                entity.Property(e => e.KtnBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("ktn_beskrivelse");

                entity.Property(e => e.KtnKontotype)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTOTYPE")
                    .IsFixedLength();

                entity.Property(e => e.KtnPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTN_PASSIVISERTDATO");

                entity.HasOne(d => d.KtpNoekkelNavigation)
                    .WithMany(p => p.FaKontoers)
                    .HasForeignKey(d => d.KtpNoekkel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KONTO_KONTOPLAN_FA_KONTO");
            });

            modelBuilder.Entity<FaKontoplan>(entity =>
            {
                entity.HasKey(e => e.KtpNoekkel)
                    .IsClustered(false);

                entity.ToTable("FA_KONTOPLAN");

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.Property(e => e.KtpBehandlingsregel)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("KTP_BEHANDLINGSREGEL")
                    .IsFixedLength();

                entity.Property(e => e.KtpDimensjon)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTP_DIMENSJON");

                entity.Property(e => e.KtpDimensjonLengde)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KTP_DIMENSJON_LENGDE");

                entity.Property(e => e.KtpFortekst)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KTP_FORTEKST");

                entity.Property(e => e.KtpStyringsregel)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("KTP_STYRINGSREGEL")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaKontotiltakstype>(entity =>
            {
                entity.HasKey(e => e.KttTiltakskode)
                    .IsClustered(false);

                entity.ToTable("FA_KONTOTILTAKSTYPE");

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonr }, "FK_FA_KONTOTILTAKSTYPE1")
                    .HasFillFactor(80);

                entity.Property(e => e.KttTiltakskode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTT_TILTAKSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonr)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonr")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.Property(e => e.KttBeskrivelse)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("KTT_BESKRIVELSE");

                entity.Property(e => e.KttPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTT_PASSIVISERTDATO");

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaKontotiltakstypes)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonr })
                    .HasConstraintName("FK_FA_KONTO_REFERENCE_FA_KONTO");
            });

            modelBuilder.Entity<FaKsIdenter>(entity =>
            {
                entity.HasKey(e => e.KsiIdent)
                    .IsClustered(false);

                entity.ToTable("FA_KS_IDENTER");

                entity.Property(e => e.KsiIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KSI_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KsiBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KSI_BESKRIVELSE");

                entity.Property(e => e.KsiFraaar)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KSI_FRAAAR")
                    .IsFixedLength();

                entity.Property(e => e.KsiPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KSI_PASSIVISERTDATO");

                entity.Property(e => e.KsiTilaar)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KSI_TILAAR")
                    .IsFixedLength();

                entity.Property(e => e.KsiType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KSI_TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaKssatser>(entity =>
            {
                entity.HasKey(e => new { e.KsiIdent, e.KssFradato })
                    .IsClustered(false);

                entity.ToTable("FA_KSSATSER");

                entity.HasIndex(e => e.KsiIdent, "FK_FA_KSSATSER1")
                    .HasFillFactor(80);

                entity.Property(e => e.KsiIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("KSI_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KssFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KSS_FRADATO");

                entity.Property(e => e.KssArbeidsgodtgjoerelse)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("KSS_ARBEIDSGODTGJOERELSE");

                entity.Property(e => e.KssTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KSS_TILDATO");

                entity.Property(e => e.KssUtgiftsdekning)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("KSS_UTGIFTSDEKNING");

                entity.HasOne(d => d.KsiIdentNavigation)
                    .WithMany(p => p.FaKssatsers)
                    .HasForeignKey(d => d.KsiIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KSSAT_KS_IDENTE_FA_KS_ID");
            });

            modelBuilder.Entity<FaKvello>(entity =>
            {
                entity.HasKey(e => e.KveLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_KVELLO");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_KVELLO")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_KVELLO2");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_KVELLO3");

                entity.Property(e => e.KveLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("KVE_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KveAnsvarligLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_ANSVARLIG_LAAST");

                entity.Property(e => e.KveBackgroundEconomyScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_BACKGROUND_ECONOMY_SCORE");

                entity.Property(e => e.KveBackgroundFamily)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BACKGROUND_FAMILY");

                entity.Property(e => e.KveBackgroundHousingScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_BACKGROUND_HOUSING_SCORE");

                entity.Property(e => e.KveBackgroundLockFamily)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BACKGROUND_LOCK_FAMILY");

                entity.Property(e => e.KveBarnAktiviteter)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BARN_AKTIVITETER");

                entity.Property(e => e.KveBarnAktiviteterLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BARN_AKTIVITETER_LAAST");

                entity.Property(e => e.KveBarnAktiviteterScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_BARN_AKTIVITETER_SCORE");

                entity.Property(e => e.KveBarnFramtoningLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BARN_FRAMTONING_LAAST");

                entity.Property(e => e.KveBarnFramtoningScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_BARN_FRAMTONING_SCORE");

                entity.Property(e => e.KveBarnHelseLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BARN_HELSE_LAAST");

                entity.Property(e => e.KveBarnHelseLaast1)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BARN_HELSE_LAAST1");

                entity.Property(e => e.KveBarnKompetanse)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BARN_KOMPETANSE");

                entity.Property(e => e.KveBarnKompetanseLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BARN_KOMPETANSE_LAAST");

                entity.Property(e => e.KveBarnKompetanseScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_BARN_KOMPETANSE_SCORE");

                entity.Property(e => e.KveBarnPsykisk)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BARN_PSYKISK");

                entity.Property(e => e.KveBarnSelvrapport)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BARN_SELVRAPPORT");

                entity.Property(e => e.KveBarnSelvrapportLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BARN_SELVRAPPORT_LAAST");

                entity.Property(e => e.KveBarnSelvrapportScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_BARN_SELVRAPPORT_SCORE");

                entity.Property(e => e.KveBarnSomatisk)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BARN_SOMATISK");

                entity.Property(e => e.KveBoligBeskrivelse)
                    .IsUnicode(false)
                    .HasColumnName("KVE_BOLIG_BESKRIVELSE");

                entity.Property(e => e.KveBoligLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_BOLIG_LAAST");

                entity.Property(e => e.KveBoligType)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("KVE_BOLIG_TYPE");

                entity.Property(e => e.KveEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KVE_ENDRETDATO");

                entity.Property(e => e.KveFaktorBeskLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAKTOR_BESK_LAAST");

                entity.Property(e => e.KveFaktorRiskLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAKTOR_RISK_LAAST");

                entity.Property(e => e.KveFaktorStress)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FAKTOR_STRESS");

                entity.Property(e => e.KveFaktorStressLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAKTOR_STRESS_LAAST");

                entity.Property(e => e.KveFaktorStressScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FAKTOR_STRESS_SCORE");

                entity.Property(e => e.KveFaktorVurdering)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FAKTOR_VURDERING");

                entity.Property(e => e.KveFamilieKrimLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAMILIE_KRIM_LAAST");

                entity.Property(e => e.KveFamilieKrimScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FAMILIE_KRIM_SCORE");

                entity.Property(e => e.KveFamilieKriminalitet)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FAMILIE_KRIMINALITET");

                entity.Property(e => e.KveFamilieOvergrep)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FAMILIE_OVERGREP");

                entity.Property(e => e.KveFamilieOvergrepLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAMILIE_OVERGREP_LAAST");

                entity.Property(e => e.KveFamilieOvergrepScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FAMILIE_OVERGREP_SCORE");

                entity.Property(e => e.KveFamilieRus)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FAMILIE_RUS");

                entity.Property(e => e.KveFamilieRusLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAMILIE_RUS_LAAST");

                entity.Property(e => e.KveFamilieRusScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FAMILIE_RUS_SCORE");

                entity.Property(e => e.KveFamilieVold)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FAMILIE_VOLD");

                entity.Property(e => e.KveFamilieVoldLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FAMILIE_VOLD_LAAST");

                entity.Property(e => e.KveFamilieVoldScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FAMILIE_VOLD_SCORE");

                entity.Property(e => e.KveFinishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("KVE_FINISH_DATE");

                entity.Property(e => e.KveForeldreFarForst)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_FAR_FORST");

                entity.Property(e => e.KveForeldreFarForstLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_FAR_FORST_LAAST");

                entity.Property(e => e.KveForeldreFarForstScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FORELDRE_FAR_FORST_SCORE");

                entity.Property(e => e.KveForeldreFarFramt)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_FAR_FRAMT");

                entity.Property(e => e.KveForeldreFarFramtLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_FAR_FRAMT_LAAST");

                entity.Property(e => e.KveForeldreFarFramtScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FORELDRE_FAR_FRAMT_SCORE");

                entity.Property(e => e.KveForeldreFarPsy)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_FAR_PSY");

                entity.Property(e => e.KveForeldreFarPsyLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_FAR_PSY_LAAST");

                entity.Property(e => e.KveForeldreFarSom)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_FAR_SOM");

                entity.Property(e => e.KveForeldreFarSomLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_FAR_SOM_LAAST");

                entity.Property(e => e.KveForeldreMorForst)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_MOR_FORST");

                entity.Property(e => e.KveForeldreMorForstLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_MOR_FORST_LAAST");

                entity.Property(e => e.KveForeldreMorForstScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FORELDRE_MOR_FORST_SCORE");

                entity.Property(e => e.KveForeldreMorFramt)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_MOR_FRAMT");

                entity.Property(e => e.KveForeldreMorFramtLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_MOR_FRAMT_LAAST");

                entity.Property(e => e.KveForeldreMorFramtScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_FORELDRE_MOR_FRAMT_SCORE");

                entity.Property(e => e.KveForeldreMorPsy)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_MOR_PSY");

                entity.Property(e => e.KveForeldreMorPsyLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_MOR_PSY_LAAST");

                entity.Property(e => e.KveForeldreMorSom)
                    .IsUnicode(false)
                    .HasColumnName("KVE_FORELDRE_MOR_SOM");

                entity.Property(e => e.KveForeldreMorSomLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_FORELDRE_MOR_SOM_LAAST");

                entity.Property(e => e.KveFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KVE_FRADATO");

                entity.Property(e => e.KveInfoKilde)
                    .IsUnicode(false)
                    .HasColumnName("KVE_INFO_KILDE");

                entity.Property(e => e.KveInfoLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_INFO_LAAST");

                entity.Property(e => e.KveNewVersion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_NEW_VERSION");

                entity.Property(e => e.KveOekAnnet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEK_ANNET");

                entity.Property(e => e.KveOekArbsoek)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEK_ARBSOEK");

                entity.Property(e => e.KveOekArbtiltak)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEK_ARBTILTAK");

                entity.Property(e => e.KveOekBeskrivelse)
                    .IsUnicode(false)
                    .HasColumnName("KVE_OEK_BESKRIVELSE");

                entity.Property(e => e.KveOekOrdarb)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEK_ORDARB");

                entity.Property(e => e.KveOekUnderutd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEK_UNDERUTD");

                entity.Property(e => e.KveOekYtelsenav)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEK_YTELSENAV");

                entity.Property(e => e.KveOekonomiLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OEKONOMI_LAAST");

                entity.Property(e => e.KveOppsumeringBeskrLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_OPPSUMERING_BESKR_LAAST");

                entity.Property(e => e.KveOppsumeringBeskrivelse)
                    .IsUnicode(false)
                    .HasColumnName("KVE_OPPSUMERING_BESKRIVELSE");

                entity.Property(e => e.KveOppsumeringScoreLaast)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_OPPSUMERING_SCORE_LAAST");

                entity.Property(e => e.KvePeriodeLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_PERIODE_LAAST");

                entity.Property(e => e.KvePersonerLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_PERSONER_LAAST");

                entity.Property(e => e.KveRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KVE_REGISTRERTDATO");

                entity.Property(e => e.KveSamsAttenAwayFather)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_ATTEN_AWAY_FATHER");

                entity.Property(e => e.KveSamsGenFamilie)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_GEN_FAMILIE");

                entity.Property(e => e.KveSamsGenFamilieLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_GEN_FAMILIE_LAAST");

                entity.Property(e => e.KveSamsGenFamilieScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_GEN_FAMILIE_SCORE");

                entity.Property(e => e.KveSamsGeneralFather)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_GENERAL_FATHER");

                entity.Property(e => e.KveSamsGrenser)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_GRENSER");

                entity.Property(e => e.KveSamsGrenserLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_GRENSER_LAAST");

                entity.Property(e => e.KveSamsGrenserScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_GRENSER_SCORE");

                entity.Property(e => e.KveSamsImplFather)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_IMPL_FATHER");

                entity.Property(e => e.KveSamsInvolverFar)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_INVOLVER_FAR");

                entity.Property(e => e.KveSamsInvolverFarScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_INVOLVER_FAR_SCORE");

                entity.Property(e => e.KveSamsInvolverMor)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_INVOLVER_MOR");

                entity.Property(e => e.KveSamsInvolverMorScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_INVOLVER_MOR_SCORE");

                entity.Property(e => e.KveSamsOmSensitivLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_OM_SENSITIV_LAAST");

                entity.Property(e => e.KveSamsOmsorgSensScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_OMSORG_SENS_SCORE");

                entity.Property(e => e.KveSamsOmsorgSensitiv)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_OMSORG_SENSITIV");

                entity.Property(e => e.KveSamsReaksjonFar)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_REAKSJON_FAR");

                entity.Property(e => e.KveSamsReaksjonFarScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_REAKSJON_FAR_SCORE");

                entity.Property(e => e.KveSamsReaksjonLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_REAKSJON_LAAST");

                entity.Property(e => e.KveSamsReaksjonLaast1)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_REAKSJON_LAAST1");

                entity.Property(e => e.KveSamsReaksjonLaast2)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_REAKSJON_LAAST2");

                entity.Property(e => e.KveSamsReaksjonLaast3)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_REAKSJON_LAAST3");

                entity.Property(e => e.KveSamsReaksjonMor)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_REAKSJON_MOR");

                entity.Property(e => e.KveSamsReaksjonMorScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_REAKSJON_MOR_SCORE");

                entity.Property(e => e.KveSamsRoutesFather)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_ROUTES_FATHER");

                entity.Property(e => e.KveSamsRutiner)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_RUTINER");

                entity.Property(e => e.KveSamsRutinerLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_RUTINER_LAAST");

                entity.Property(e => e.KveSamsRutinerScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_RUTINER_SCORE");

                entity.Property(e => e.KveSamsScAttenFather)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_SC_ATTEN_FATHER");

                entity.Property(e => e.KveSamsScGeneralFather)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_SC_GENERAL_FATHER");

                entity.Property(e => e.KveSamsScImplFather)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_SC_IMPL_FATHER");

                entity.Property(e => e.KveSamsScRoutesFather)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_SC_ROUTES_FATHER");

                entity.Property(e => e.KveSamsScSensitivFather)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_SC_SENSITIV_FATHER");

                entity.Property(e => e.KveSamsSensitivityFather)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_SENSITIVITY_FATHER");

                entity.Property(e => e.KveSamsTilsyn)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_TILSYN");

                entity.Property(e => e.KveSamsTilsynLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_SAMS_TILSYN_LAAST");

                entity.Property(e => e.KveSamsTilsynScore)
                    .HasColumnType("numeric(2, 1)")
                    .HasColumnName("KVE_SAMS_TILSYN_SCORE");

                entity.Property(e => e.KveSamsTxAttenFather)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_TX_ATTEN_FATHER");

                entity.Property(e => e.KveSamsTxGeneralFather)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_TX_GENERAL_FATHER");

                entity.Property(e => e.KveSamsTxImplFather)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_TX_IMPL_FATHER");

                entity.Property(e => e.KveSamsTxRoutesFather)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_TX_ROUTES_FATHER");

                entity.Property(e => e.KveSamsTxSensitivFather)
                    .IsUnicode(false)
                    .HasColumnName("KVE_SAMS_TX_SENSITIV_FATHER");

                entity.Property(e => e.KveTilAktivLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_TIL_AKTIV_LAAST");

                entity.Property(e => e.KveTilAvbruttLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_TIL_AVBRUTT_LAAST");

                entity.Property(e => e.KveTilAvslaattLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_TIL_AVSLAATT_LAAST");

                entity.Property(e => e.KveTilGjennomfoertLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_TIL_GJENNOMFOERT_LAAST");

                entity.Property(e => e.KveTilPrivatLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("KVE_TIL_PRIVAT_LAAST");

                entity.Property(e => e.KveTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KVE_TILDATO");

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhRegistrertav)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaKvellos)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KVELLO_KLIENT");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaKvelloSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_KVELLO_SBHENDRETAV");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaKvelloSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KVELLO_SBHREGISTRERTAV");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaKvellos)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_FA_KVELO_POSTJOURN_FA_POSTJ");
            });

            modelBuilder.Entity<FaKvelloAnsvarlig>(entity =>
            {
                entity.HasKey(e => e.KvaLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_KVELLO_ANSVARLIG");

                entity.HasIndex(e => e.KveLoepenr, "FK_FA_KVELLO_ANSVARLIG")
                    .HasFillFactor(80);

                entity.Property(e => e.KvaLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("KVA_LOEPENR");

                entity.Property(e => e.KvaInstitusjon)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KVA_INSTITUSJON");

                entity.Property(e => e.KvaMandat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KVA_MANDAT");

                entity.Property(e => e.KvaNavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KVA_NAVN");

                entity.Property(e => e.KvaTittel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KVA_TITTEL");

                entity.Property(e => e.KveLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KVE_LOEPENR");

                entity.HasOne(d => d.KveLoepenrNavigation)
                    .WithMany(p => p.FaKvelloAnsvarligs)
                    .HasForeignKey(d => d.KveLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KVELLOANSVARLIG_KVELLO");
            });

            modelBuilder.Entity<FaKvelloBeskFaktorer>(entity =>
            {
                entity.HasKey(e => new { e.KveLoepenr, e.KbfType })
                    .IsClustered(false);

                entity.ToTable("FA_KVELLO_BESK_FAKTORER");

                entity.HasIndex(e => e.KveLoepenr, "FK_FA_KVELLO_BESK_FAKTORER")
                    .HasFillFactor(80);

                entity.Property(e => e.KveLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KVE_LOEPENR");

                entity.Property(e => e.KbfType)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("KBF_TYPE");

                entity.HasOne(d => d.KveLoepenrNavigation)
                    .WithMany(p => p.FaKvelloBeskFaktorers)
                    .HasForeignKey(d => d.KveLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KVELLO_KVELLOBESKFAKTOR");
            });

            modelBuilder.Entity<FaKvelloPersoner>(entity =>
            {
                entity.HasKey(e => new { e.KveLoepenr, e.KtkLoepenr });

                entity.ToTable("FA_KVELLO_PERSONER");

                entity.HasIndex(e => e.KtkLoepenr, "FK_KLIENTTILKNYTN_KVELLOPERS")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KveLoepenr, "FK_KVELLO_KVELLOPERSONER")
                    .HasFillFactor(80);

                entity.Property(e => e.KveLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KVE_LOEPENR");

                entity.Property(e => e.KtkLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KTK_LOEPENR");

                entity.HasOne(d => d.KveLoepenrNavigation)
                    .WithMany(p => p.FaKvelloPersoners)
                    .HasForeignKey(d => d.KveLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_KVELLOPERSON_KVELLO");
            });

            modelBuilder.Entity<FaKvelloRiskFaktorer>(entity =>
            {
                entity.HasKey(e => new { e.KveLoepenr, e.KrfType })
                    .IsClustered(false);

                entity.ToTable("FA_KVELLO_RISK_FAKTORER");

                entity.HasIndex(e => e.KveLoepenr, "FK_FA_KVELLO_RISK_FAKTORER")
                    .HasFillFactor(80);

                entity.Property(e => e.KveLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KVE_LOEPENR");

                entity.Property(e => e.KrfType)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("KRF_TYPE");

                entity.HasOne(d => d.KveLoepenrNavigation)
                    .WithMany(p => p.FaKvelloRiskFaktorers)
                    .HasForeignKey(d => d.KveLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KVELLO_KVELLORISKFAKTOR");
            });

            modelBuilder.Entity<FaKvelloTiltak>(entity =>
            {
                entity.HasKey(e => e.KvtLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_KVELLO_TILTAK");

                entity.HasIndex(e => e.KveLoepenr, "FK_FA_KVELLO_TILTAK")
                    .HasFillFactor(80);

                entity.Property(e => e.KvtLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("KVT_LOEPENR");

                entity.Property(e => e.KveLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KVE_LOEPENR");

                entity.Property(e => e.KvtBeskrivelse)
                    .IsUnicode(false)
                    .HasColumnName("KVT_BESKRIVELSE");

                entity.Property(e => e.KvtFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KVT_FRADATO");

                entity.Property(e => e.KvtGjennomfoertav)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KVT_GJENNOMFOERTAV");

                entity.Property(e => e.KvtTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KVT_TILDATO");

                entity.Property(e => e.KvtTiltakstype)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("KVT_TILTAKSTYPE");

                entity.Property(e => e.KvtType)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("KVT_TYPE");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.HasOne(d => d.KveLoepenrNavigation)
                    .WithMany(p => p.FaKvelloTiltaks)
                    .HasForeignKey(d => d.KveLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KVELLO_KVELLOTILTAK");
            });

            modelBuilder.Entity<FaLoennstrinn>(entity =>
            {
                entity.HasKey(e => e.LoeTrinn)
                    .IsClustered(false);

                entity.ToTable("FA_LOENNSTRINN");

                entity.Property(e => e.LoeTrinn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("LOE_TRINN")
                    .IsFixedLength();

                entity.Property(e => e.LoePassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LOE_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaLoennstrinnsat>(entity =>
            {
                entity.HasKey(e => new { e.LoeTrinn, e.LosFradato })
                    .IsClustered(false);

                entity.ToTable("FA_LOENNSTRINNSATS");

                entity.HasIndex(e => e.LoeTrinn, "FK_FA_LOENNSTRINNSATS1")
                    .HasFillFactor(80);

                entity.Property(e => e.LoeTrinn)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("LOE_TRINN")
                    .IsFixedLength();

                entity.Property(e => e.LosFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("LOS_FRADATO");

                entity.Property(e => e.LosAarsloenn)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LOS_AARSLOENN");

                entity.Property(e => e.LosKveldnattillegg)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("LOS_KVELDNATTILLEGG");

                entity.Property(e => e.LosTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("LOS_TILDATO");

                entity.Property(e => e.LosTimeloenn)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("LOS_TIMELOENN");

                entity.HasOne(d => d.LoeTrinnNavigation)
                    .WithMany(p => p.FaLoennstrinnsats)
                    .HasForeignKey(d => d.LoeTrinn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_LOENN_LOENNTRIN_FA_LOENN");
            });

            modelBuilder.Entity<FaLoepenr>(entity =>
            {
                entity.HasKey(e => e.LopIdent)
                    .IsClustered(false);

                entity.ToTable("FA_LOEPENR");

                entity.Property(e => e.LopIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("LOP_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.LopSistbruktenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LOP_SISTBRUKTENR");

                entity.Property(e => e.LopTabellnavn)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LOP_TABELLNAVN");
            });

            modelBuilder.Entity<FaLogg>(entity =>
            {
                entity.HasKey(e => e.LogLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_LOGG");

                entity.HasIndex(e => e.LogSaksbehandler, "FK_FA_LOGG1");

                entity.HasIndex(e => e.LogKategori, "FK_FA_LOGG2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LogType, "FK_FA_LOGG3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LogDatotid, "FK_FA_LOGG4")
                    .HasFillFactor(80);

                entity.Property(e => e.LogLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LOG_LOEPENR");

                entity.Property(e => e.LogDatotid)
                    .HasColumnType("datetime")
                    .HasColumnName("LOG_DATOTID")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LogKategori)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOG_KATEGORI")
                    .IsFixedLength();

                entity.Property(e => e.LogKlientetternavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOG_KLIENTETTERNAVN");

                entity.Property(e => e.LogKlientfornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOG_KLIENTFORNAVN");

                entity.Property(e => e.LogKlientnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LOG_KLIENTNR");

                entity.Property(e => e.LogSaksbehandler)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOG_SAKSBEHANDLER");

                entity.Property(e => e.LogSbhEtternavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOG_SBH_ETTERNAVN");

                entity.Property(e => e.LogSbhFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOG_SBH_FORNAVN");

                entity.Property(e => e.LogType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOG_TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaLoggNoark>(entity =>
            {
                entity.HasKey(e => e.LogLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_LOGG_NOARK");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_LOGG_NOARK")
                    .HasFillFactor(80);

                entity.Property(e => e.LogLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LOG_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.LogAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LOG_AAR");

                entity.Property(e => e.LogBeskrivelse)
                    .IsUnicode(false)
                    .HasColumnName("LOG_BESKRIVELSE");

                entity.Property(e => e.LogDato)
                    .HasColumnType("datetime")
                    .HasColumnName("LOG_DATO");

                entity.Property(e => e.LogDetaljbeskrivelse)
                    .IsUnicode(false)
                    .HasColumnName("LOG_DETALJBESKRIVELSE");

                entity.Property(e => e.LogNr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LOG_NR");

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOG_TYPE");
            });

            modelBuilder.Entity<FaLogglinjer>(entity =>
            {
                entity.HasKey(e => new { e.LogLoepenr, e.LglLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_LOGGLINJER");

                entity.HasIndex(e => e.LogLoepenr, "FK_FA_LOGGLINJER1")
                    .HasFillFactor(80);

                entity.Property(e => e.LogLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LOG_LOEPENR");

                entity.Property(e => e.LglLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LGL_LOEPENR");

                entity.Property(e => e.LglBeskrivelse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LGL_BESKRIVELSE");

                entity.Property(e => e.LglEndret)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LGL_ENDRET");

                entity.Property(e => e.LglFeltnavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LGL_FELTNAVN");

                entity.Property(e => e.LglGmlverdi)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LGL_GMLVERDI");

                entity.Property(e => e.LglNoekkel)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LGL_NOEKKEL");

                entity.Property(e => e.LglNyverdi)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LGL_NYVERDI");

                entity.HasOne(d => d.LogLoepenrNavigation)
                    .WithMany(p => p.FaLogglinjers)
                    .HasForeignKey(d => d.LogLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_LOGGL_LOGG_LOGG_FA_LOGG");
            });

            modelBuilder.Entity<FaLovtekst>(entity =>
            {
                entity.HasKey(e => e.LovParagraf)
                    .IsClustered(false);

                entity.ToTable("FA_LOVTEKST");

                entity.Property(e => e.LovParagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_PARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovHovedparagraf)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LOV_HOVEDPARAGRAF");

                entity.Property(e => e.LovLovtekst)
                    .HasColumnType("text")
                    .HasColumnName("LOV_LOVTEKST");

                entity.Property(e => e.LovOverskrift)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LOV_OVERSKRIFT");

                entity.Property(e => e.LovPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LOV_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaLovtekstKombinasjoner>(entity =>
            {
                entity.HasKey(e => new { e.LovParagraf, e.LovJfparagraf });

                entity.ToTable("FA_LOVTEKST_KOMBINASJONER");

                entity.Property(e => e.LovParagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_PARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJfparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JFPARAGRAF")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaMarkedclientelement>(entity =>
            {
                entity.HasKey(e => e.MceId);

                entity.ToTable("FA_MARKEDCLIENTELEMENTS");

                entity.HasIndex(e => new { e.MceFlowElementId, e.MceFlowElementType, e.SbhInitialer, e.KliLoepenr, e.MceFlowElementAar }, "UQ_FA_MARKEDCLIENTELEMENTS")
                    .IsUnique();

                entity.Property(e => e.MceId).HasColumnName("MCE_ID");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MceFlowElementAar).HasColumnName("MCE_FLOW_ELEMENT_AAR");

                entity.Property(e => e.MceFlowElementId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("MCE_FLOW_ELEMENT_ID");

                entity.Property(e => e.MceFlowElementType).HasColumnName("MCE_FLOW_ELEMENT_TYPE");

                entity.Property(e => e.SbhInitialer)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaMarkedclientelements)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MARKEDCLIENTELEMENTS_KLIENT");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaMarkedclientelements)
                    .HasForeignKey(d => d.SbhInitialer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MARKEDCLIENTELEMENTS_SBH");
            });

            modelBuilder.Entity<FaMedarbeidere>(entity =>
            {
                entity.HasKey(e => e.ForLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_MEDARBEIDERE");

                entity.HasIndex(e => e.ArsIdent, "FK_FA_MEDARBEIDERE1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_MEDARBEIDERE2");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_MEDARBEIDERE3");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.ArsIdent)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARS_IDENT");

                entity.Property(e => e.MedBegyntdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MED_BEGYNTDATO");

                entity.Property(e => e.MedEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MED_ENDRETDATO");

                entity.Property(e => e.MedLoennsnr)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("MED_LOENNSNR")
                    .IsFixedLength();

                entity.Property(e => e.MedMerknader)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MED_MERKNADER");

                entity.Property(e => e.MedPolitiattest)
                    .HasColumnType("datetime")
                    .HasColumnName("MED_POLITIATTEST");

                entity.Property(e => e.MedRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MED_REGISTRERTDATO");

                entity.Property(e => e.MedSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MED_SLUTTDATO");

                entity.Property(e => e.MedStatus)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MED_STATUS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MedStilling1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLING1");

                entity.Property(e => e.MedStilling2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLING2");

                entity.Property(e => e.MedStilling3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLING3");

                entity.Property(e => e.MedStilling4)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLING4");

                entity.Property(e => e.MedStilling5)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLING5");

                entity.Property(e => e.MedStillingid1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLINGID1");

                entity.Property(e => e.MedStillingid2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLINGID2");

                entity.Property(e => e.MedStillingid3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLINGID3");

                entity.Property(e => e.MedStillingid4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLINGID4");

                entity.Property(e => e.MedStillingid5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("MED_STILLINGID5");

                entity.Property(e => e.MedTaushetserklaering)
                    .HasColumnType("datetime")
                    .HasColumnName("MED_TAUSHETSERKLAERING");

                entity.Property(e => e.MedTilsynsfOpplaering)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MED_TILSYNSF_OPPLAERING");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.ArsIdentNavigation)
                    .WithMany(p => p.FaMedarbeideres)
                    .HasForeignKey(d => d.ArsIdent)
                    .HasConstraintName("FK_FA_MEDAR_ARBGIVERS_FA_ARBEI");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithOne(p => p.FaMedarbeidere)
                    .HasForeignKey<FaMedarbeidere>(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MEDAR_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaMedarbeidereSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_MEDAR_SAKSBEH_M_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaMedarbeidereSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_MEDAR_SAKSBEH_M_FA_SAKS2");
            });

            modelBuilder.Entity<FaMedarbeiderinteresser>(entity =>
            {
                entity.HasKey(e => new { e.ForLoepenr, e.IntIdent })
                    .IsClustered(false);

                entity.ToTable("FA_MEDARBEIDERINTERESSER");

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_MEDARBEIDERINTERESSER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.IntIdent, "FK_FA_MEDARBEIDERINTERESSER2")
                    .HasFillFactor(80);

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.IntIdent)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("INT_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.MinKommentar)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("MIN_KOMMENTAR");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaMedarbeiderinteressers)
                    .HasForeignKey(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MEDAR_MEDARBEID_FA_MEDAR");

                entity.HasOne(d => d.IntIdentNavigation)
                    .WithMany(p => p.FaMedarbeiderinteressers)
                    .HasForeignKey(d => d.IntIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MEDAR_INTERESSE_FA_INTER");
            });

            modelBuilder.Entity<FaMeldinger>(entity =>
            {
                entity.HasKey(e => e.MelLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_MELDINGER");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_MELDINGER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode2 }, "FK_FA_MELDINGER10")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode3 }, "FK_FA_MELDINGER11")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_MELDINGER12")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_MELDINGER13")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.DisDistriktskode, e.RodIdent }, "FK_FA_MELDINGER14")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosGjennomdokAar, e.PosGjennomdokLoepenr }, "FK_FA_MELDINGER15");

                entity.HasIndex(e => e.SbhMottattav, "FK_FA_MELDINGER2");

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_MELDINGER3");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_MELDINGER4");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_MELDINGER5");

                entity.HasIndex(e => e.PnrMelderPostnr, "FK_FA_MELDINGER6")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosMottattbrevAar, e.PosMottattbrevLoepenr }, "FK_FA_MELDINGER7")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosSendtkonklAar, e.PosSendtkonklLoepenr }, "FK_FA_MELDINGER8")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode1 }, "FK_FA_MELDINGER9")
                    .HasFillFactor(80);

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.ArkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DATO");

                entity.Property(e => e.ArkDocumentSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOCUMENT_SJEKK_I_VSA");

                entity.Property(e => e.ArkMappeDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_MAPPE_DATO");

                entity.Property(e => e.ArkMappeFnrEndret)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_MAPPE_FNR_ENDRET");

                entity.Property(e => e.ArkMappeNavnEndret)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_MAPPE_NAVN_ENDRET");

                entity.Property(e => e.ArkMappeStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_MAPPE_STOPP");

                entity.Property(e => e.ArkMappeSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_MAPPE_SYSTEMID");

                entity.Property(e => e.ArkMeldingSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_MELDING_SYSTEMID");

                entity.Property(e => e.ArkSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_SJEKK_I_VSA");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.FroKode1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE1")
                    .IsFixedLength();

                entity.Property(e => e.FroKode2)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE2")
                    .IsFixedLength();

                entity.Property(e => e.FroKode3)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE3")
                    .IsFixedLength();

                entity.Property(e => e.FroType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FRO_TYPE")
                    .HasDefaultValueSql("('M')")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MelAnonymmelder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_ANONYMMELDER");

                entity.Property(e => e.MelAvsluttetgjennomgang)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_AVSLUTTETGJENNOMGANG");

                entity.Property(e => e.MelBarnopplFritekst)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("MEL_BARNOPPL_FRITEKST");

                entity.Property(e => e.MelBehandlesinnen)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_BEHANDLESINNEN");

                entity.Property(e => e.MelDokFlett)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_DOK_FLETT");

                entity.Property(e => e.MelDokProdusert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_DOK_PRODUSERT");

                entity.Property(e => e.MelDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_DOKUMENTNR");

                entity.Property(e => e.MelEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_ENDRETDATO");

                entity.Property(e => e.MelEttdokument)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_ETTDOKUMENT");

                entity.Property(e => e.MelEtternavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MEL_ETTERNAVN");

                entity.Property(e => e.MelFaropplFritekst)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("MEL_FAROPPL_FRITEKST");

                entity.Property(e => e.MelFoedselsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_FOEDSELSDATO");

                entity.Property(e => e.MelFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MEL_FORNAVN");

                entity.Property(e => e.MelGjennomgangsdokferdig)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_GJENNOMGANGSDOKFERDIG");

                entity.Property(e => e.MelGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MEL_GMLREFERANSE");

                entity.Property(e => e.MelHenlagtAnnenInstans)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_HENLAGT_ANNEN_INSTANS");

                entity.Property(e => e.MelHenlagtPgaUtenforbvl)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_HENLAGT_PGA_UTENFORBVL");

                entity.Property(e => e.MelHenlagtTilAnnenBv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_HENLAGT_TIL_ANNEN_BV");

                entity.Property(e => e.MelHypotese)
                    .HasColumnType("text")
                    .HasColumnName("MEL_HYPOTESE");

                entity.Property(e => e.MelInnhAdferd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_ADFERD");

                entity.Property(e => e.MelInnhAndreBarnSitu)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_ANDRE_BARN_SITU");

                entity.Property(e => e.MelInnhAndreForeFami)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_ANDRE_FORE_FAMI");

                entity.Property(e => e.MelInnhAnnet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_ANNET");

                entity.Property(e => e.MelInnhBarnAdferdKrim)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_ADFERD_KRIM");

                entity.Property(e => e.MelInnhBarnFysiskMish)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_FYSISK_MISH");

                entity.Property(e => e.MelInnhBarnMangOmsorgp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_MANG_OMSORGP");

                entity.Property(e => e.MelInnhBarnNedsFunk)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_NEDS_FUNK");

                entity.Property(e => e.MelInnhBarnPsykProb)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_PSYK_PROB");

                entity.Property(e => e.MelInnhBarnPsykiskMish)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_PSYKISK_MISH");

                entity.Property(e => e.MelInnhBarnRelasvansker)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_RELASVANSKER");

                entity.Property(e => e.MelInnhBarnRusmisbruk)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_RUSMISBRUK");

                entity.Property(e => e.MelInnhBarnSeksuOvergr)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_SEKSU_OVERGR");

                entity.Property(e => e.MelInnhBarnVansjotsel)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_VANSJOTSEL");

                entity.Property(e => e.MelInnhForeKriminalitet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_KRIMINALITET");

                entity.Property(e => e.MelInnhForeManglerFerdigh)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_MANGLER_FERDIGH");

                entity.Property(e => e.MelInnhForePsykiskProblem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_PSYKISK_PROBLEM");

                entity.Property(e => e.MelInnhForeRusmisbruk)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_RUSMISBRUK");

                entity.Property(e => e.MelInnhForeSomatiskSykdom)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_SOMATISK_SYKDOM");

                entity.Property(e => e.MelInnhForhold)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORHOLD");

                entity.Property(e => e.MelInnhKonfliktHjemme)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_KONFLIKT_HJEMME");

                entity.Property(e => e.MelInnhOffentMelder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_OFFENT_MELDER")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MelInnhForeManglBeskyt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_MANGL_BESKYT");

                entity.Property(e => e.MelInnhForeManglStimu)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_MANGL_STIMU");

                entity.Property(e => e.MelInnhForeTilgjeng)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_TILGJENG");

                entity.Property(e => e.MelInnhForeOppfoelgBehov)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_FORE_OPPFOELG_BEHOV");

                entity.Property(e => e.MelInnhBarnForeKonflikt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_FORE_KONFLIKT");

                entity.Property(e => e.MelInnhBarnAdferd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_ADFERD");

                entity.Property(e => e.MelInnhBarnKrim)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_KRIM");

                entity.Property(e => e.MelInnhBarnMennHandel)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_BARN_MENN_HANDEL");

                entity.Property(e => e.MelInnhOmsorg)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_OMSORG");

                entity.Property(e => e.MelInnhPresBarnet)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEL_INNH_PRES_BARNET");

                entity.Property(e => e.MelInnhPresFamilie)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEL_INNH_PRES_FAMILIE");

                entity.Property(e => e.MelInnhVoldHjemme)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_INNH_VOLD_HJEMME");

                entity.Property(e => e.MelKonklTilmelderbegrunnelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("MEL_KONKL_TILMELDERBEGRUNNELSE");

                entity.Property(e => e.MelKonklusjon)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("MEL_KONKLUSJON")
                    .IsFixedLength();

                entity.Property(e => e.MelKonklusjontilmelder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_KONKLUSJONTILMELDER");

                entity.Property(e => e.MelKtrlskjemaLevertiaby)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_KTRLSKJEMA_LEVERTIABY");

                entity.Property(e => e.MelMelderadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDERADRESSE");

                entity.Property(e => e.MelMelderetternavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDERETTERNAVN");

                entity.Property(e => e.MelMelderfornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDERFORNAVN");

                entity.Property(e => e.MelMeldertelefonarbeid)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDERTELEFONARBEID")
                    .IsFixedLength();

                entity.Property(e => e.MelMeldertelefonmobil)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDERTELEFONMOBIL")
                    .IsFixedLength();

                entity.Property(e => e.MelMeldertelefonprivat)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDERTELEFONPRIVAT")
                    .IsFixedLength();

                entity.Property(e => e.MelMelding)
                    .HasColumnType("text")
                    .HasColumnName("MEL_MELDING");

                entity.Property(e => e.MelMeldingstype)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.MelMeldtAndre)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_ANDRE");

                entity.Property(e => e.MelMeldtBarnehage)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_BARNEHAGE");

                entity.Property(e => e.MelMeldtBarnet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_BARNET");

                entity.Property(e => e.MelMeldtBarnevt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_BARNEVT");

                entity.Property(e => e.MelMeldtBarnevvakt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_BARNEVVAKT");

                entity.Property(e => e.MelMeldtBup)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_BUP");

                entity.Property(e => e.MelMeldtFamilie)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_FAMILIE");

                entity.Property(e => e.MelMeldtFamilievernkontor)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_FAMILIEVERNKONTOR");

                entity.Property(e => e.MelMeldtForeldre)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_FORELDRE");

                entity.Property(e => e.MelMeldtFrivillige)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_FRIVILLIGE");

                entity.Property(e => e.MelMeldtHelsestasjon)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_HELSESTASJON");

                entity.Property(e => e.MelMeldtKrisesenter)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_KRISESENTER");

                entity.Property(e => e.MelMeldtLege)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_LEGE");

                entity.Property(e => e.MelMeldtNaboer)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_NABOER");

                entity.Property(e => e.MelMeldtOffentlig)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_OFFENTLIG");

                entity.Property(e => e.MelMeldtPedPpt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_PED_PPT");

                entity.Property(e => e.MelMeldtPoliti)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_POLITI");

                entity.Property(e => e.MelMeldtPresAndre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDT_PRES_ANDRE");

                entity.Property(e => e.MelMeldtPresAndreOffent)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MELDT_PRES_ANDRE_OFFENT");

                entity.Property(e => e.MelMeldtPsykiskHelseBarn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_PSYKISK_HELSE_BARN");

                entity.Property(e => e.MelMeldtPsykiskHelseVoksne)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_PSYKISK_HELSE_VOKSNE");

                entity.Property(e => e.MelMeldtSkole)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_SKOLE");

                entity.Property(e => e.MelMeldtSosialkt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_SOSIALKT");

                entity.Property(e => e.MelMeldtTjenesteInstans)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_TJENESTE_INSTANS");

                entity.Property(e => e.MelMeldtUdi)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_UDI");

                entity.Property(e => e.MelMeldtUtekontakt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_MELDT_UTEKONTAKT");

                entity.Property(e => e.MelMerknadfristoversittelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MERKNADFRISTOVERSITTELSE");

                entity.Property(e => e.MelMoropplFritekst)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MOROPPL_FRITEKST");

                entity.Property(e => e.MelMottattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_MOTTATTDATO");

                entity.Property(e => e.MelMottattmaate)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("MEL_MOTTATTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.MelPaabegyntdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_PAABEGYNTDATO");

                entity.Property(e => e.MelPersonnr)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEL_PERSONNR");

                entity.Property(e => e.MelRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MEL_REGISTRERTDATO");

                entity.Property(e => e.MelStatusConclusion)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("MEL_STATUS_CONCLUSION")
                    .HasDefaultValueSql("('100')")
                    .IsFixedLength();

                entity.Property(e => e.MelStatusKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("MEL_STATUS_KATEGORI")
                    .HasDefaultValueSql("('118')")
                    .IsFixedLength();

                entity.Property(e => e.MelSvarmaatemelder)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("MEL_SVARMAATEMELDER")
                    .IsFixedLength();

                entity.Property(e => e.MelTattoppAndre)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_TATTOPP_ANDRE");

                entity.Property(e => e.MelTattoppForeldre)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("MEL_TATTOPP_FORELDRE");

                entity.Property(e => e.MelTattoppHvem)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MEL_TATTOPP_HVEM");

                entity.Property(e => e.MelTidligerekjennskap)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("MEL_TIDLIGEREKJENNSKAP");

                entity.Property(e => e.PnrMelderPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_MELDER_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.PosGjennomdokAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_GJENNOMDOK_AAR");

                entity.Property(e => e.PosGjennomdokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_GJENNOMDOK_LOEPENR");

                entity.Property(e => e.PosMottattbrevAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_MOTTATTBREV_AAR");

                entity.Property(e => e.PosMottattbrevLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_MOTTATTBREV_LOEPENR");

                entity.Property(e => e.PosSendtkonklAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_SENDTKONKL_AAR");

                entity.Property(e => e.PosSendtkonklLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_SENDTKONKL_LOEPENR");

                entity.Property(e => e.RodIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ROD_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.SbhMottattav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_MOTTATTAV")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV")
                    .HasConversion(converter, comparer);

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaMeldingers)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MELDI_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaMeldingers)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_MELDI_FA_DOKUME_FA_DOKUM");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaMeldingers)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_MELDI_KLIENT_ME_FA_KLIEN");

                entity.HasOne(d => d.PnrMelderPostnrNavigation)
                    .WithMany(p => p.FaMeldingers)
                    .HasForeignKey(d => d.PnrMelderPostnr)
                    .HasConstraintName("FK_FA_MELDI_POSTADR_M_FA_POSTA");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaMeldingerSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_MELDI_SBH_MELDI_FA_SAKS3");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaMeldingerSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_MELDI_SBH_MELDI_FA_SAKS4");

                entity.HasOne(d => d.SbhMottattavNavigation)
                    .WithMany(p => p.FaMeldingerSbhMottattavNavigations)
                    .HasForeignKey(d => d.SbhMottattav)
                    .HasConstraintName("FK_FA_MELDI_SBH_MELDI_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaMeldingerSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_MELDI_SBH_MELDI_FA_SAKS2");

                entity.HasOne(d => d.FaRoder)
                    .WithMany(p => p.FaMeldingers)
                    .HasForeignKey(d => new { d.DisDistriktskode, d.RodIdent })
                    .HasConstraintName("FK_FA_MELDI_RODE_MELD_FA_RODER");

                entity.HasOne(d => d.Fro)
                    .WithMany(p => p.FaMeldingerFros)
                    .HasForeignKey(d => new { d.FroType, d.FroKode1 })
                    .HasConstraintName("FK_FA_MELDI_FRISTOVER_FA_FRIST");

                entity.HasOne(d => d.FroNavigation)
                    .WithMany(p => p.FaMeldingerFroNavigations)
                    .HasForeignKey(d => new { d.FroType, d.FroKode2 })
                    .HasConstraintName("FK_FA_MELDI_FRISTOVER_FA_FRIS2");

                entity.HasOne(d => d.Fro1)
                    .WithMany(p => p.FaMeldingerFro1s)
                    .HasForeignKey(d => new { d.FroType, d.FroKode3 })
                    .HasConstraintName("FK_FA_MELDI_FRISTOVER_FA_FRIS3");

                entity.HasOne(d => d.PosGjennomdok)
                    .WithMany(p => p.FaMeldingerPosGjennomdoks)
                    .HasForeignKey(d => new { d.PosGjennomdokAar, d.PosGjennomdokLoepenr })
                    .HasConstraintName("FK_FA_MELDING_GJENNOMDOK_POSTJ");

                entity.HasOne(d => d.PosMottattbrev)
                    .WithMany(p => p.FaMeldingerPosMottattbrevs)
                    .HasForeignKey(d => new { d.PosMottattbrevAar, d.PosMottattbrevLoepenr })
                    .HasConstraintName("FK_FA_MELDI_POSTJOURN_FA_POSTJ");

                entity.HasOne(d => d.PosSendtkonkl)
                    .WithMany(p => p.FaMeldingerPosSendtkonkls)
                    .HasForeignKey(d => new { d.PosSendtkonklAar, d.PosSendtkonklLoepenr })
                    .HasConstraintName("FK_FA_MELDI_POSTJOURN_FA_POST2");
            });

            modelBuilder.Entity<FaMeldingerSlettet>(entity =>
            {
                entity.HasKey(e => e.MelLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_MELDINGER_SLETTET");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_MELDINGER_SLETTET");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_MELDINGER_SLETTET2")
                    .HasFillFactor(80);

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.ArkMeldingSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_MELDING_SYSTEMID");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MelDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_DOKUMENTNR");

                entity.Property(e => e.MesBegrSlettet)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("MES_BEGR_SLETTET");

                entity.Property(e => e.MesRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MES_REGISTRERTDATO");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaMeldingerSlettets)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_MELDINGERSLETTET_KLIENT");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaMeldingerSlettets)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_MELDI_SBH_REGIS_FA_SAKSB");
            });

            modelBuilder.Entity<FaMerkantilfil>(entity =>
            {
                entity.HasKey(e => new { e.FilIdent, e.FilLoepenr, e.TelTeller })
                    .IsClustered(false);

                entity.ToTable("FA_MERKANTILFIL");

                entity.HasIndex(e => e.TelTeller, "FK_FA_MERKANTILFIL")
                    .HasFillFactor(80);

                entity.Property(e => e.FilIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("FIL_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.FilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FIL_LOEPENR");

                entity.Property(e => e.TelTeller)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TEL_TELLER");

                entity.Property(e => e.FilNavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FIL_NAVN");

                entity.Property(e => e.FilProdusertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("FIL_PRODUSERTDATO");

                entity.Property(e => e.FilRemFoersteProdnr)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("FIL_REM_FOERSTE_PRODNR")
                    .IsFixedLength();

                entity.Property(e => e.FilRemFoersteSekvnr)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("FIL_REM_FOERSTE_SEKVNR")
                    .IsFixedLength();

                entity.Property(e => e.FilRemSisteProdnr)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("FIL_REM_SISTE_PRODNR")
                    .IsFixedLength();

                entity.Property(e => e.FilRemSisteSekvnr)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("FIL_REM_SISTE_SEKVNR")
                    .IsFixedLength();

                entity.Property(e => e.FilSystem)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FIL_SYSTEM")
                    .IsFixedLength();

                entity.HasOne(d => d.TelTellerNavigation)
                    .WithMany(p => p.FaMerkantilfils)
                    .HasForeignKey(d => d.TelTeller)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_MERKANTILTEL_TELLER");
            });

            modelBuilder.Entity<FaNasjoner>(entity =>
            {
                entity.HasKey(e => e.NasKodenr)
                    .IsClustered(false);

                entity.ToTable("FA_NASJONER");

                entity.HasIndex(e => e.NasStatsKodenr, "FK_FA_NASJONER1")
                    .HasFillFactor(80);

                entity.Property(e => e.NasKodenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("NAS_KODENR");

                entity.Property(e => e.NasLandkode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NAS_LANDKODE")
                    .IsFixedLength();

                entity.Property(e => e.NasNasjonsnavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NAS_NASJONSNAVN");

                entity.Property(e => e.NasStatsKodenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("NAS_STATS_KODENR");

                entity.HasOne(d => d.NasStatsKodenrNavigation)
                    .WithMany(p => p.InverseNasStatsKodenrNavigation)
                    .HasForeignKey(d => d.NasStatsKodenr)
                    .HasConstraintName("FK_FA_NASJO_NASJON_NA_FA_NASJO");
            });

            modelBuilder.Entity<FaPlantiltak>(entity =>
            {
                entity.HasKey(e => e.TptLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_PLANTILTAK");

                entity.HasIndex(e => e.TtpLoepenr, "FK_FA_PLANTILTAK")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TttTiltakstype, "FK_FA_PLANTILTAK2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_PLANTILTAK3");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_PLANTILTAK4");

                entity.Property(e => e.TptLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TPT_LOEPENR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.Property(e => e.TptEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TPT_ENDRETDATO");

                entity.Property(e => e.TptFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("TPT_FRADATO");

                entity.Property(e => e.TptRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TPT_REGISTRERTDATO");

                entity.Property(e => e.TptTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("TPT_TILDATO");

                entity.Property(e => e.TptTiltak)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TPT_TILTAK");

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TTT_TILTAKSTYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaPlantiltakSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_PLANTILTAK_ENDRETAV");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaPlantiltakSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_PLANTILTAK_REGISTRERTAV");

                entity.HasOne(d => d.TtpLoepenrNavigation)
                    .WithMany(p => p.FaPlantiltaks)
                    .HasForeignKey(d => d.TtpLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PLANTILTAK_TILTAKSPLAN");

                entity.HasOne(d => d.TttTiltakstypeNavigation)
                    .WithMany(p => p.FaPlantiltaks)
                    .HasForeignKey(d => d.TttTiltakstype)
                    .HasConstraintName("FK_FA_PLANTILTAK_TILTAKSTYPE");
            });

            modelBuilder.Entity<FaPlantype>(entity =>
            {
                entity.HasKey(e => e.PtyPlankode)
                    .IsClustered(false);

                entity.ToTable("FA_PLANTYPE");

                entity.Property(e => e.PtyPlankode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANKODE")
                    .IsFixedLength();

                entity.Property(e => e.PtyLovhjemmel)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PTY_LOVHJEMMEL");

                entity.Property(e => e.PtyPlantype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANTYPE");
            });

            modelBuilder.Entity<FaPostadresser>(entity =>
            {
                entity.HasKey(e => e.PnrPostnr)
                    .IsClustered(false);

                entity.ToTable("FA_POSTADRESSER");

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_POSTADRESSER1")
                    .HasFillFactor(80);

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.PnrPoststed)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTSTED");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaPostadressers)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_POSTA_KOMMUNE_P_FA_KOMMU");
            });

            modelBuilder.Entity<FaPostjournal>(entity =>
            {
                entity.HasKey(e => new { e.PosAar, e.PosLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_POSTJOURNAL");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_POSTJOURNAL1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_POSTJOURNAL10")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosAvskriverAar, e.PosAvskriverLoepenr }, "FK_FA_POSTJOURNAL11")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.DisDistriktskode, e.RodIdent }, "FK_FA_POSTJOURNAL12")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_POSTJOURNAL13");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_POSTJOURNAL14");

                entity.HasIndex(e => new { e.SakUnndrAar, e.SakUnndrJournalnr }, "FK_FA_POSTJOURNAL15")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhGodkjennInitialer, "FK_FA_POSTJOURNAL16");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_POSTJOURNAL2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KgrGruppeid, "FK_FA_POSTJOURNAL3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_POSTJOURNAL4");

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_POSTJOURNAL5")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.SakAar, e.SakJournalnr }, "FK_FA_POSTJOURNAL6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KkoKodeFag, "FK_FA_POSTJOURNAL7")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KkoKodeFelles, "FK_FA_POSTJOURNAL8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KkoKodeTillegg, "FK_FA_POSTJOURNAL9")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.PosFrist, "IX_FA_POSTJOURNAL1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.PosOppfyltdato, "IX_FA_POSTJOURNAL2")
                    .HasFillFactor(80);

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.ArkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DATO");

                entity.Property(e => e.ArkPostSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_POST_SYSTEMID");

                entity.Property(e => e.PosReconnectedDocnrFlag)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_RECONNECTED_DOCNR_FLAG");

                entity.Property(e => e.ArkSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_SJEKK_I_VSA");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.KgrGruppeid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KGR_GRUPPEID")
                    .IsFixedLength();

                entity.Property(e => e.KkoKodeFag)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KKO_KODE_fag")
                    .IsFixedLength();

                entity.Property(e => e.KkoKodeFelles)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KKO_KODE_felles")
                    .IsFixedLength();

                entity.Property(e => e.KkoKodeTillegg)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KKO_KODE_tillegg")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.PosAbtype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("POS_ABTYPE")
                    .IsFixedLength();

                entity.Property(e => e.PosAtt)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("POS_ATT");

                entity.Property(e => e.PosAvskriverAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_avskriver_AAR");

                entity.Property(e => e.PosAvskriverLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_avskriver_LOEPENR");

                entity.Property(e => e.PosBegrSlettet)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("POS_BEGR_SLETTET");

                entity.Property(e => e.PosBesoeksadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("POS_BESOEKSADRESSE");

                entity.Property(e => e.PosBesvares)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_BESVARES");

                entity.Property(e => e.PosBesvart)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_BESVART");

                entity.Property(e => e.PosBrevtype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("POS_BREVTYPE")
                    .IsFixedLength();

                entity.Property(e => e.PosDato)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_DATO");

                entity.Property(e => e.PosDeresref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("POS_DERESREF")
                    .IsFixedLength();

                entity.Property(e => e.PosDigitalpost)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_DIGITALPOST");

                entity.Property(e => e.PosDigitalpostRef)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("POS_DIGITALPOST_REF");

                entity.Property(e => e.PosDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_DOKUMENTNR");

                entity.Property(e => e.PosEmne)
                    .IsRequired()
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("POS_EMNE");

                entity.Property(e => e.PosEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_ENDRETDATO");

                entity.Property(e => e.PosEtternavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POS_ETTERNAVN");

                entity.Property(e => e.PosFerdigdato)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_FERDIGDATO");

                entity.Property(e => e.PosFleradressater)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_FLERADRESSATER");

                entity.Property(e => e.PosForbLoepenr)
                    .HasColumnType("numeric(11, 0)")
                    .HasColumnName("POS_FORB_LOEPENR");

                entity.Property(e => e.PosFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("POS_FORNAVN");

                entity.Property(e => e.PosFrist)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_FRIST");

                entity.Property(e => e.PosGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POS_GMLREFERANSE");

                entity.Property(e => e.PosMerknad)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("POS_MERKNAD");

                entity.Property(e => e.PosNotat)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("POS_NOTAT");

                entity.Property(e => e.PosOppfoelging)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("POS_OPPFOELGING");

                entity.Property(e => e.PosOppfyltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_OPPFYLTDATO");

                entity.Property(e => e.PosOpprettsak)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_OPPRETTSAK");

                entity.Property(e => e.PosPostadresse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POS_POSTADRESSE");

                entity.Property(e => e.PosPosttype)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("POS_POSTTYPE")
                    .IsFixedLength();

                entity.Property(e => e.PosRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_REGISTRERTDATO");

                entity.Property(e => e.PosSendKlient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_SEND_KLIENT");

                entity.Property(e => e.PosSendtMottattDato)
                    .HasColumnType("datetime")
                    .HasColumnName("POS_SENDT_MOTTATT_DATO");

                entity.Property(e => e.PosSlettet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_SLETTET");

                entity.Property(e => e.PosTmpDigitalstatus)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_TMP_DIGITALSTATUS");

                entity.Property(e => e.PosUnndrattbegrunnelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("POS_UNNDRATTBEGRUNNELSE");

                entity.Property(e => e.PosUnndrattinnsyn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_UNNDRATTINNSYN");

                entity.Property(e => e.PosUnndrattinnsynIs)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_UNNDRATTINNSYN_IS");

                entity.Property(e => e.PosVaarref)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("POS_VAARREF")
                    .IsFixedLength();

                entity.Property(e => e.PosVurderUnndratt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POS_VURDER_UNNDRATT");

                entity.Property(e => e.RodIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ROD_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SakUnndrAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_unndr_AAR");

                entity.Property(e => e.SakUnndrJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_unndr_JOURNALNR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.SbhGodkjennInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_GODKJENN_INITIALER")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.TriggerFlagInsertNewDocumnet).HasColumnName("TRIGGER_FLAG_INSERT_NEW_DOCUMNET");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaPostjournals)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_POSTJ_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaPostjournals)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_POSTJ_DOKUMENT__FA_DOKUM");

                entity.HasOne(d => d.KgrGruppe)
                    .WithMany(p => p.FaPostjournals)
                    .HasForeignKey(d => d.KgrGruppeid)
                    .HasConstraintName("FK_FA_POSTJ_KLIENTGR__FA_KLIEN");

                entity.HasOne(d => d.KkoKodeFagNavigation)
                    .WithMany(p => p.FaPostjournalKkoKodeFagNavigations)
                    .HasForeignKey(d => d.KkoKodeFag)
                    .HasConstraintName("FK_FA_POSTJ_KKODE_POS_FA_KKODE");

                entity.HasOne(d => d.KkoKodeFellesNavigation)
                    .WithMany(p => p.FaPostjournalKkoKodeFellesNavigations)
                    .HasForeignKey(d => d.KkoKodeFelles)
                    .HasConstraintName("FK_FA_POSTJ_KKODE_POS_FA_KKOD3");

                entity.HasOne(d => d.KkoKodeTilleggNavigation)
                    .WithMany(p => p.FaPostjournalKkoKodeTilleggNavigations)
                    .HasForeignKey(d => d.KkoKodeTillegg)
                    .HasConstraintName("FK_FA_POSTJ_KKODE_POS_FA_KKOD2");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaPostjournals)
                    .HasForeignKey(d => d.KliLoepenr)
                    .HasConstraintName("FK_FA_POSTJ_KLIENT_PO_FA_KLIEN");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaPostjournals)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_POSTJ_POSTADR_P_FA_POSTA");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaPostjournalSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_POSTJ_SAKSBEH_P_FA_SAKS3");

                entity.HasOne(d => d.SbhGodkjennInitialerNavigation)
                    .WithMany(p => p.FaPostjournalSbhGodkjennInitialerNavigations)
                    .HasForeignKey(d => d.SbhGodkjennInitialer)
                    .HasConstraintName("FK_FA_POSTJ_SBH_PJ_GO_FA_SAKSB");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaPostjournalSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_POSTJ_SAKSBEH_P_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaPostjournalSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_POSTJ_SAKSBEH_P_FA_SAKS2");

                entity.HasOne(d => d.FaRoder)
                    .WithMany(p => p.FaPostjournals)
                    .HasForeignKey(d => new { d.DisDistriktskode, d.RodIdent })
                    .HasConstraintName("FK_FA_POSTJ_RODER_POS_FA_RODER");

                entity.HasOne(d => d.PosAvskriver)
                    .WithMany(p => p.InversePosAvskriver)
                    .HasForeignKey(d => new { d.PosAvskriverAar, d.PosAvskriverLoepenr })
                    .HasConstraintName("FK_FA_POSTJ_POSTJOURN_FA_POSTJ");

                entity.HasOne(d => d.Sak)
                    .WithMany(p => p.FaPostjournalSaks)
                    .HasForeignKey(d => new { d.SakAar, d.SakJournalnr })
                    .HasConstraintName("FK_FA_POSTJ_SAK_POSTJ_FA_SAKSJ");

                entity.HasOne(d => d.SakUnndr)
                    .WithMany(p => p.FaPostjournalSakUnndrs)
                    .HasForeignKey(d => new { d.SakUnndrAar, d.SakUnndrJournalnr })
                    .HasConstraintName("FK_FA_POSTJ_SAK_POSTJ_FA_SAKS2");
            });

            modelBuilder.Entity<FaPostjournalkopitil>(entity =>
            {
                entity.HasKey(e => new { e.PosAar, e.PosLoepenr, e.PokLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_POSTJOURNALKOPITIL");

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_POSTJOURNALKOPITIL1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_POSTJOURNALKOPITIL2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliLoepenrKopiFamilie, "FK_FA_POSTJOURNALKOPITIL3");

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.PokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POK_LOEPENR");

                entity.Property(e => e.KliLoepenrKopiFamilie)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR_KOPI_FAMILIE");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.PokAnonymisert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POK_ANONYMISERT");

                entity.Property(e => e.PokBesoeksadresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("POK_BESOEKSADRESSE");

                entity.Property(e => e.PokDigitalpost)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POK_DIGITALPOST");

                entity.Property(e => e.PokDigitalpostRef)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("POK_DIGITALPOST_REF");

                entity.Property(e => e.PokEtternavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POK_ETTERNAVN");

                entity.Property(e => e.PokForbLoepenr)
                    .HasColumnType("numeric(11, 0)")
                    .HasColumnName("POK_FORB_LOEPENR");

                entity.Property(e => e.PokFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("POK_FORNAVN");

                entity.Property(e => e.PokMedadressat)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POK_MEDADRESSAT");

                entity.Property(e => e.PokPostadresse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POK_POSTADRESSE");

                entity.Property(e => e.PokTmpDigitalstatus)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("POK_TMP_DIGITALSTATUS");

                entity.HasOne(d => d.KliLoepenrKopiFamilieNavigation)
                    .WithMany(p => p.FaPostjournalkopitils)
                    .HasForeignKey(d => d.KliLoepenrKopiFamilie)
                    .HasConstraintName("FK_FA_POSTJ_KOPITIL_KLIENT");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaPostjournalkopitils)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_POSTJ_POSTADR_P_FA_POST2");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaPostjournalkopitils)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_POSTJ_POSTJOURN_FA_POST2");
            });

            modelBuilder.Entity<FaProsjekt>(entity =>
            {
                entity.HasKey(e => e.ProLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_PROSJEKT");

                entity.HasIndex(e => e.PrtProsjekttype, "FK_FA_PROSJEKT1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_PROSJEKT2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_PROSJEKT3");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_PROSJEKT4");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_PROSJEKT5");

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonummer }, "FK_FA_PROSJEKT6")
                    .HasFillFactor(80);

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.Property(e => e.ProBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("PRO_BESKRIVELSE");

                entity.Property(e => e.ProEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRO_ENDRETDATO");

                entity.Property(e => e.ProInnhold)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRO_INNHOLD");

                entity.Property(e => e.ProMaalsetning)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRO_MAALSETNING");

                entity.Property(e => e.ProNavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PRO_NAVN");

                entity.Property(e => e.ProRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRO_REGISTRERTDATO");

                entity.Property(e => e.ProSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRO_SLUTTDATO");

                entity.Property(e => e.ProStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRO_STARTDATO");

                entity.Property(e => e.PrtProsjekttype)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("PRT_PROSJEKTTYPE")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaProsjekts)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_DISTRIKT__FA_DISTR");

                entity.HasOne(d => d.PrtProsjekttypeNavigation)
                    .WithMany(p => p.FaProsjekts)
                    .HasForeignKey(d => d.PrtProsjekttype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_PROTYPE_P_FA_PROSJ");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaProsjektSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_PROSJ_SAKSBEH_P_FA_SAKS3");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaProsjektSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_PROSJ_SAKSBEH_P_FA_SAKS1");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaProsjektSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_PROSJ_SAKSBEH_P_FA_SAKS2");

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaProsjekts)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonummer })
                    .HasConstraintName("FK_FA_PROSJ_KONTOER_P_FA_KONTO");
            });

            modelBuilder.Entity<FaProsjektaktivitet>(entity =>
            {
                entity.HasKey(e => e.PraLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_PROSJEKTAKTIVITET");

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_PROSJEKTAKTIVITET1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_PROSJEKTAKTIVITET2");

                entity.Property(e => e.PraLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRA_LOEPENR");

                entity.Property(e => e.PraBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("PRA_BESKRIVELSE");

                entity.Property(e => e.PraEksternansvarlig)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PRA_EKSTERNANSVARLIG");

                entity.Property(e => e.PraInternt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("PRA_INTERNT");

                entity.Property(e => e.PraSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRA_SLUTTDATO");

                entity.Property(e => e.PraStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRA_STARTDATO");

                entity.Property(e => e.PraStatus)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PRA_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaProsjektaktivitets)
                    .HasForeignKey(d => d.ProLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_PROSJEKT__FA_PROSA");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaProsjektaktivitets)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_PROSA_SAKSBEH_P_FA_SAKSB");
            });

            modelBuilder.Entity<FaProsjektdeltEk>(entity =>
            {
                entity.HasKey(e => new { e.ProLoepenr, e.ForLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_PROSJEKTDELT_EKS");

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_PROSJEKTDELT_EKS1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_PROSJEKTDELT_EKS2")
                    .HasFillFactor(80);

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.PrfMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PRF_MERKNAD");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaProsjektdeltEks)
                    .HasForeignKey(d => d.ForLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaProsjektdeltEks)
                    .HasForeignKey(d => d.ProLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_FORBINDEL_FA_PROS2");
            });

            modelBuilder.Entity<FaProsjektdeltInt>(entity =>
            {
                entity.HasKey(e => new { e.ProLoepenr, e.SbhInitialer })
                    .IsClustered(false);

                entity.ToTable("FA_PROSJEKTDELT_INT");

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_PROSJEKTDELT_INT1");

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_PROSJEKTDELT_INT2")
                    .HasFillFactor(80);

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.PrdMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PRD_MERKNAD");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaProsjektdeltInts)
                    .HasForeignKey(d => d.ProLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_SAKSBEH_P_FA_PROS5");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaProsjektdeltInts)
                    .HasForeignKey(d => d.SbhInitialer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_SAKSBEH_P_FA_SAKS4");
            });

            modelBuilder.Entity<FaProsjektevaluering>(entity =>
            {
                entity.HasKey(e => e.PreLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_PROSJEKTEVALUERING");

                entity.HasIndex(e => e.ProLoepenr, "FK_FA_PROSJEKTEVALUERING1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_PROSJEKTEVALUERING2");

                entity.Property(e => e.PreLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRE_LOEPENR");

                entity.Property(e => e.PreDato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRE_DATO");

                entity.Property(e => e.PreEvaluering)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PRE_EVALUERING");

                entity.Property(e => e.ProLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PRO_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.HasOne(d => d.ProLoepenrNavigation)
                    .WithMany(p => p.FaProsjektevaluerings)
                    .HasForeignKey(d => d.ProLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_PROSJ_PROSJEKT__FA_PROSE");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaProsjektevaluerings)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_PROSE_SAKSBEH_P_FA_SAKSB");
            });

            modelBuilder.Entity<FaProsjekttype>(entity =>
            {
                entity.HasKey(e => e.PrtProsjekttype)
                    .IsClustered(false);

                entity.ToTable("FA_PROSJEKTTYPE");

                entity.Property(e => e.PrtProsjekttype)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("PRT_PROSJEKTTYPE")
                    .IsFixedLength();

                entity.Property(e => e.PrtBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("PRT_BESKRIVELSE");

                entity.Property(e => e.PrtPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PRT_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaRefusjoner>(entity =>
            {
                entity.HasKey(e => e.RefLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_REFUSJONER");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_REFUSJONER")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosAarSoeknad, e.PosLoepenrSoeknad }, "FK_FA_REFUSJONER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosAarTilsagn, e.PosLoepenrTilsagn }, "FK_FA_REFUSJONER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_REFUSJONER4");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_REFUSJONER5");

                entity.Property(e => e.RefLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("REF_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PosAarSoeknad)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR_SOEKNAD");

                entity.Property(e => e.PosAarTilsagn)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR_TILSAGN");

                entity.Property(e => e.PosLoepenrSoeknad)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR_SOEKNAD");

                entity.Property(e => e.PosLoepenrTilsagn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR_TILSAGN");

                entity.Property(e => e.RefAn1UtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_AN1_UTG_DEK_BEREGNET");

                entity.Property(e => e.RefAn1UtgDekBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_AN1_UTG_DEK_BESKRIVELSE");

                entity.Property(e => e.RefAn1UtgDekJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_AN1_UTG_DEK_JUSTERT");

                entity.Property(e => e.RefAn2UtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_AN2_UTG_DEK_BEREGNET");

                entity.Property(e => e.RefAn2UtgDekBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_AN2_UTG_DEK_BESKRIVELSE");

                entity.Property(e => e.RefAn2UtgDekJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_AN2_UTG_DEK_JUSTERT");

                entity.Property(e => e.RefBesArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_BES_ARB_GIV_BEREGNET");

                entity.Property(e => e.RefBesArbGivBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_BES_ARB_GIV_BESKRIVELSE");

                entity.Property(e => e.RefBesArbGivJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_BES_ARB_GIV_JUSTERT");

                entity.Property(e => e.RefBesArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_BES_ARB_GOD_BEREGNET");

                entity.Property(e => e.RefBesArbGodBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_BES_ARB_GOD_BESKRIVELSE");

                entity.Property(e => e.RefBesArbGodJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_BES_ARB_GOD_JUSTERT");

                entity.Property(e => e.RefBesUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_BES_UTG_DEK_BEREGNET");

                entity.Property(e => e.RefBesUtgDekBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_BES_UTG_DEK_BESKRIVELSE");

                entity.Property(e => e.RefBesUtgDekJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_BES_UTG_DEK_JUSTERT");

                entity.Property(e => e.RefEgenandelBelop)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_EGENANDEL_BELOP");

                entity.Property(e => e.RefEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("REF_ENDRETDATO");

                entity.Property(e => e.RefFosArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_ARB_GIV_BEREGNET");

                entity.Property(e => e.RefFosArbGivBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_FOS_ARB_GIV_BESKRIVELSE");

                entity.Property(e => e.RefFosArbGivJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_ARB_GIV_JUSTERT");

                entity.Property(e => e.RefFosArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_ARB_GOD_BEREGNET");

                entity.Property(e => e.RefFosArbGodBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_FOS_ARB_GOD_BESKRIVELSE");

                entity.Property(e => e.RefFosArbGodJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_ARB_GOD_JUSTERT");

                entity.Property(e => e.RefFosEksUtgBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_EKS_UTG_BEREGNET");

                entity.Property(e => e.RefFosEksUtgBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_FOS_EKS_UTG_BESKRIVELSE");

                entity.Property(e => e.RefFosEksUtgJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_EKS_UTG_JUSTERT");

                entity.Property(e => e.RefFosUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_UTG_DEK_BEREGNET");

                entity.Property(e => e.RefFosUtgDekBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_FOS_UTG_DEK_BESKRIVELSE");

                entity.Property(e => e.RefFosUtgDekJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_FOS_UTG_DEK_JUSTERT");

                entity.Property(e => e.RefFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("REF_FRADATO");

                entity.Property(e => e.RefLeverandoernr)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REF_LEVERANDOERNR");

                entity.Property(e => e.RefRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("REF_REGISTRERTDATO");

                entity.Property(e => e.RefStatus)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("REF_STATUS");

                entity.Property(e => e.RefStoArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_STO_ARB_GIV_BEREGNET");

                entity.Property(e => e.RefStoArbGivBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_STO_ARB_GIV_BESKRIVELSE");

                entity.Property(e => e.RefStoArbGivJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_STO_ARB_GIV_JUSTERT");

                entity.Property(e => e.RefStoArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_STO_ARB_GOD_BEREGNET");

                entity.Property(e => e.RefStoArbGodBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_STO_ARB_GOD_BESKRIVELSE");

                entity.Property(e => e.RefStoArbGodJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_STO_ARB_GOD_JUSTERT");

                entity.Property(e => e.RefStoUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_STO_UTG_DEK_BEREGNET");

                entity.Property(e => e.RefStoUtgDekBeskrivelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("REF_STO_UTG_DEK_BESKRIVELSE");

                entity.Property(e => e.RefStoUtgDekJustert)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("REF_STO_UTG_DEK_JUSTERT");

                entity.Property(e => e.RefTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("REF_TILDATO");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaRefusjoners)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KLIENT_REFUSJONER");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaRefusjonerSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_SBH_REFENDRETAV");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaRefusjonerSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_SBH_REFREGISTRERTAV");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaRefusjonerPos)
                    .HasForeignKey(d => new { d.PosAarSoeknad, d.PosLoepenrSoeknad })
                    .HasConstraintName("FK_REFUSJONSSOEKNAD_POSTJOURN");

                entity.HasOne(d => d.PosNavigation)
                    .WithMany(p => p.FaRefusjonerPosNavigations)
                    .HasForeignKey(d => new { d.PosAarTilsagn, d.PosLoepenrTilsagn })
                    .HasConstraintName("FK_REFUSJONSTILSAGN_POSTJOURN");

                entity.HasMany(d => d.RerLoepenrs)
                    .WithMany(p => p.RefLoepenrs)
                    .UsingEntity<Dictionary<string, object>>(
                        "FaRefusjonerKrav",
                        l => l.HasOne<FaRefusjonskrav>().WithMany().HasForeignKey("RerLoepenr").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_REFUSJONSKRAV_REFKRAV"),
                        r => r.HasOne<FaRefusjoner>().WithMany().HasForeignKey("RefLoepenr").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_REFUSJON_REFKRAV"),
                        j =>
                        {
                            j.HasKey("RefLoepenr", "RerLoepenr").IsClustered(false);

                            j.ToTable("FA_REFUSJONER_KRAV");

                            j.IndexerProperty<decimal>("RefLoepenr").HasColumnType("numeric(10, 0)").HasColumnName("REF_LOEPENR");

                            j.IndexerProperty<decimal>("RerLoepenr").HasColumnType("numeric(10, 0)").HasColumnName("RER_LOEPENR");
                        });
            });

            modelBuilder.Entity<FaRefusjonskrav>(entity =>
            {
                entity.HasKey(e => e.RerLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_REFUSJONSKRAV");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_REFUSJONSKRAV")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_REFUSJONSKRAV2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_REFUSJONSKRAV3");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_REFUSJONSKRAV4");

                entity.Property(e => e.RerLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RER_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.RerAndreFradrag)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_ANDRE_FRADRAG");

                entity.Property(e => e.RerAnnBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_ANN_BEREGNET");

                entity.Property(e => e.RerBesArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_BES_ARB_GIV_BEREGNET");

                entity.Property(e => e.RerBesArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_BES_ARB_GOD_BEREGNET");

                entity.Property(e => e.RerBesBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_BES_BEREGNET");

                entity.Property(e => e.RerBesUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_BES_UTG_DEK_BEREGNET");

                entity.Property(e => e.RerEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("RER_ENDRETDATO");

                entity.Property(e => e.RerFosArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_FOS_ARB_GIV_BEREGNET");

                entity.Property(e => e.RerFosArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_FOS_ARB_GOD_BEREGNET");

                entity.Property(e => e.RerFosUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_FOS_UTG_DEK_BEREGNET");

                entity.Property(e => e.RerFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("RER_FRADATO");

                entity.Property(e => e.RerKomEgenandelKrav)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_KOM_EGENANDEL_KRAV");

                entity.Property(e => e.RerKomEgenandelMnd)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_KOM_EGENANDEL_MND");

                entity.Property(e => e.RerMilArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_MIL_ARB_GIV_BEREGNET");

                entity.Property(e => e.RerMilArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_MIL_ARB_GOD_BEREGNET");

                entity.Property(e => e.RerMilBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_MIL_BEREGNET");

                entity.Property(e => e.RerMilUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_MIL_UTG_DEK_BEREGNET");

                entity.Property(e => e.RerRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("RER_REGISTRERTDATO");

                entity.Property(e => e.RerStoArbGivBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_STO_ARB_GIV_BEREGNET");

                entity.Property(e => e.RerStoArbGodBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_STO_ARB_GOD_BEREGNET");

                entity.Property(e => e.RerStoBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_STO_BEREGNET");

                entity.Property(e => e.RerStoUtgDekBeregnet)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RER_STO_UTG_DEK_BEREGNET");

                entity.Property(e => e.RerTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("RER_TILDATO");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaRefusjonskravs)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KLIENT_REFUSJONSKRAV");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaRefusjonskravSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_SBH_REFKRAVENDRETAV");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaRefusjonskravSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_SBH_REFKRAVREGISTRERTAV");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaRefusjonskravs)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_REFUSJONSKRAV_POSTJOURNAL");
            });

            modelBuilder.Entity<FaRekvisisjoner>(entity =>
            {
                entity.HasKey(e => new { e.RekAar, e.RekLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_REKVISISJONER");

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_REKVISISJONER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.SakAar, e.SakJournalnr }, "FK_FA_REKVISISJONER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_REKVISISJONER3");

                entity.HasIndex(e => e.ForLoepenr, "FK_FA_REKVISISJONER4")
                    .HasFillFactor(80);

                entity.Property(e => e.RekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("REK_AAR");

                entity.Property(e => e.RekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("REK_LOEPENR");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.RekDato)
                    .HasColumnType("datetime")
                    .HasColumnName("REK_DATO");

                entity.Property(e => e.RekGjelder)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("REK_GJELDER");

                entity.Property(e => e.RekMaksbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("REK_MAKSBELOEP");

                entity.Property(e => e.RekMaksbeloeptekst)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("REK_MAKSBELOEPTEKST");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaRekvisisjoners)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_REKVI_DOKUMENTE_FA_DOKUM");

                entity.HasOne(d => d.ForLoepenrNavigation)
                    .WithMany(p => p.FaRekvisisjoners)
                    .HasForeignKey(d => d.ForLoepenr)
                    .HasConstraintName("FK_FA_REKVI_FORBINDEL_FA_FORBI");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaRekvisisjoners)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_REKVI_SAKSBEH_R_FA_SAKSB");

                entity.HasOne(d => d.Sak)
                    .WithMany(p => p.FaRekvisisjoners)
                    .HasForeignKey(d => new { d.SakAar, d.SakJournalnr })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_REKVI_SAKSJOURN_FA_SAKSJ");
            });

            modelBuilder.Entity<FaRoder>(entity =>
            {
                entity.HasKey(e => new { e.DisDistriktskode, e.RodIdent })
                    .IsClustered(false);

                entity.ToTable("FA_RODER");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_RODER1")
                    .HasFillFactor(80);

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.RodIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ROD_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.RodNavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ROD_NAVN");

                entity.Property(e => e.RodPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("ROD_PASSIVISERTDATO");

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaRoders)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_RODER_DISTRIKT__FA_DISTR");
            });

            modelBuilder.Entity<FaSaksbehKlient>(entity =>
            {
                entity.ToTable("FA_SAKSBEH_KLIENT");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaSaksbehKlients)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SAKSBEH_KLIENT_FA_KLIENT");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaSaksbehKlients)
                    .HasForeignKey(d => d.SbhInitialer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SAKSBEH_KLIENT_FA_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaSaksbehKlientgrupper>(entity =>
            {
                entity.HasKey(e => new { e.KgrGruppeid, e.SbhInitialer })
                    .IsClustered(false);

                entity.ToTable("FA_SAKSBEH_KLIENTGRUPPER");

                entity.Property(e => e.KgrGruppeid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KGR_GRUPPEID")
                    .IsFixedLength();

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");
            });

            modelBuilder.Entity<FaSaksbehandlere>(entity =>
            {
                entity.HasKey(e => e.SbhInitialer)
                    .IsClustered(false);

                entity.ToTable("FA_SAKSBEHANDLERE");

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_SAKSBEHANDLERE1")
                    .HasFillFactor(80);

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer")
                    .HasConversion(converter, comparer);

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.SbhAdresse1)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ADRESSE1");

                entity.Property(e => e.SbhAdresse2)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("sbh_adresse2");

                entity.Property(e => e.SbhDatelastused)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_DATELASTUSED");

                entity.Property(e => e.SbhDatetimeout)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_DATETIMEOUT");

                entity.Property(e => e.SbhEndretpassord)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_ENDRETPASSORD");

                entity.Property(e => e.SbhEtternavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sbh_etternavn");

                entity.Property(e => e.SbhFoedselsnr)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SBH_FOEDSELSNR")
                    .IsFixedLength();

                entity.Property(e => e.SbhFornavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SBH_FORNAVN");

                entity.Property(e => e.SbhHasmobileaccess).HasColumnName("SBH_HASMOBILEACCESS");

                entity.Property(e => e.SbhLastclientidaccessed).HasColumnName("SBH_LASTCLIENTIDACCESSED");

                entity.Property(e => e.SbhLastclientidaccessedtimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_LASTCLIENTIDACCESSEDTIMESTAMP");

                entity.Property(e => e.SbhLastclientidvalidationattempts).HasColumnName("SBH_LASTCLIENTIDVALIDATIONATTEMPTS");

                entity.Property(e => e.SbhLocale)
                    .HasMaxLength(10)
                    .HasColumnName("SBH_LOCALE");

                entity.Property(e => e.SbhLoennsnr)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SBH_LOENNSNR")
                    .IsFixedLength();

                entity.Property(e => e.SbhMailadresse)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("SBH_MAILADRESSE");

                entity.Property(e => e.SbhMerknad)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("sbh_merknad");

                entity.Property(e => e.SbhPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_PASSIVISERTDATO");

                entity.Property(e => e.SbhPassordhash)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SBH_PASSORDHASH");

                entity.Property(e => e.SbhPassorditer).HasColumnName("SBH_PASSORDITER");

                entity.Property(e => e.SbhPassordsalt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SBH_PASSORDSALT");

                entity.Property(e => e.SbhPincode)
                    .HasMaxLength(5)
                    .HasColumnName("SBH_PINCODE");

                entity.Property(e => e.SbhPincodesattempts).HasColumnName("SBH_PINCODESATTEMPTS");

                entity.Property(e => e.SbhPincodetimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_PINCODETIMESTAMP");

                entity.Property(e => e.SbhReducedmobileaccess).HasColumnName("SBH_REDUCEDMOBILEACCESS");

                entity.Property(e => e.SbhStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SBH_STARTDATO");

                entity.Property(e => e.SbhTelefaks)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SBH_TELEFAKS")
                    .IsFixedLength();

                entity.Property(e => e.SbhTelefonarbeid)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("sbh_telefonarbeid")
                    .IsFixedLength();

                entity.Property(e => e.SbhTelefonmobil)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SBH_TELEFONMOBIL")
                    .IsFixedLength();

                entity.Property(e => e.SbhTelefonprivat)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("sbh_telefonprivat")
                    .IsFixedLength();

                entity.Property(e => e.SbhTittel)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SBH_TITTEL");

                entity.Property(e => e.SbhToken)
                    .HasMaxLength(255)
                    .HasColumnName("SBH_TOKEN");

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaSaksbehandleres)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_SAKSB_POSTADR_S_FA_POSTA");
            });

            modelBuilder.Entity<FaSaksjournal>(entity =>
            {
                entity.HasKey(e => new { e.SakAar, e.SakJournalnr })
                    .IsClustered(false);

                entity.ToTable("FA_SAKSJOURNAL");

                entity.HasIndex(e => e.MynVedtakstype, "FK_FA_SAKSJOURNAL")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LovJmfParagraf2, "FK_FA_SAKSJOURNAL10")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_SAKSJOURNAL11")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_SAKSJOURNAL12")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhOpphevetInitialer, "FK_FA_SAKSJOURNAL13");

                entity.HasIndex(e => e.SbhBortfaltInitialer, "FK_FA_SAKSJOURNAL14");

                entity.HasIndex(e => e.MelLoepenr, "FK_FA_SAKSJOURNAL15")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_SAKSJOURNAL2")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.SakErstattetavAar, e.SakErstattetavJournalnr }, "FK_FA_SAKSJOURNAL3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_SAKSJOURNAL4");

                entity.HasIndex(e => e.SbhAvgjortavInitialer, "FK_FA_SAKSJOURNAL5");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_SAKSJOURNAL6");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_SAKSJOURNAL7");

                entity.HasIndex(e => e.LovHovedParagraf, "FK_FA_SAKSJOURNAL8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LovJmfParagraf1, "FK_FA_SAKSJOURNAL9")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SakStatus, "IX_FA_SAKSJOURNAL1")
                    .HasFillFactor(80);

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.LovHovedParagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_hovedPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfParagraf1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_jmfPARAGRAF1")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfParagraf2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_jmfPARAGRAF2")
                    .IsFixedLength();

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.MynVedtakstype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MYN_VEDTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.SakAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_AVGJORTDATO");

                entity.Property(e => e.SakAvgjortetat)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SAK_AVGJORTETAT")
                    .IsFixedLength();

                entity.Property(e => e.SakBehFylkesnemnda)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SAK_BEH_FYLKESNEMNDA")
                    .IsFixedLength();

                entity.Property(e => e.SakBortfaltaarsak)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SAK_BORTFALTAARSAK");

                entity.Property(e => e.SakBortfaltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_BORTFALTDATO");

                entity.Property(e => e.SakDato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_DATO");

                entity.Property(e => e.SakDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_DOKUMENTNR");

                entity.Property(e => e.SakEmne)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SAK_EMNE");

                entity.Property(e => e.SakEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_ENDRETDATO");

                entity.Property(e => e.SakErstattetavAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_erstattetav_AAR");

                entity.Property(e => e.SakErstattetavJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_erstattetav_JOURNALNR");

                entity.Property(e => e.SakGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SAK_GMLREFERANSE");

                entity.Property(e => e.SakGodkjenningstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SAK_GODKJENNINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.SakHovedvedtak)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SAK_HOVEDVEDTAK");

                entity.Property(e => e.SakIverksattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_IVERKSATTDATO");

                entity.Property(e => e.SakKlageaarsak)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SAK_KLAGEAARSAK")
                    .IsFixedLength();

                entity.Property(e => e.SakKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_KLARGJORTDATO");

                entity.Property(e => e.SakMerknaderAvslag)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SAK_MERKNADER_AVSLAG");

                entity.Property(e => e.SakOpphevetaarsak)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SAK_OPPHEVETAARSAK");

                entity.Property(e => e.SakOpphevetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_OPPHEVETDATO");

                entity.Property(e => e.SakOpphevunder)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SAK_OPPHEVUNDER");

                entity.Property(e => e.SakRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_REGISTRERTDATO");

                entity.Property(e => e.SakSendtadvokat)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_SENDTADVOKAT");

                entity.Property(e => e.SakSendtfylke)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_SENDTFYLKE");

                entity.Property(e => e.SakSlutningdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_SLUTNINGDATO");

                entity.Property(e => e.SakStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("SAK_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.SakStatusKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SAK_STATUS_KATEGORI")
                    .HasDefaultValueSql("('22')")
                    .IsFixedLength();

                entity.Property(e => e.SakUtbetalingsramme)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("SAK_UTBETALINGSRAMME");

                entity.Property(e => e.SakVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_VARIGHETTIL");

                entity.Property(e => e.SakVideresendtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_VIDERESENDTDATO");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_avgjortav_initialer");

                entity.Property(e => e.SbhBortfaltInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_bortfalt_initialer");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhOpphevetInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_opphevet_initialer");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaSaksjournals)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_SAKSJ_DOKUMENTE_FA_DOKUM");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaSaksjournals)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SAKSJ_KLIENT_SA_FA_KLIEN");

                entity.HasOne(d => d.LovHovedParagrafNavigation)
                    .WithMany(p => p.FaSaksjournalLovHovedParagrafNavigations)
                    .HasForeignKey(d => d.LovHovedParagraf)
                    .HasConstraintName("FK_FA_SAKSJ_LOVTEKST__FA_LOVTE");

                entity.HasOne(d => d.LovJmfParagraf1Navigation)
                    .WithMany(p => p.FaSaksjournalLovJmfParagraf1Navigations)
                    .HasForeignKey(d => d.LovJmfParagraf1)
                    .HasConstraintName("FK_FA_SAKSJ_LOVTEKST__FA_LOVT3");

                entity.HasOne(d => d.LovJmfParagraf2Navigation)
                    .WithMany(p => p.FaSaksjournalLovJmfParagraf2Navigations)
                    .HasForeignKey(d => d.LovJmfParagraf2)
                    .HasConstraintName("FK_FA_SAKSJ_LOVTEKST__FA_LOVT2");

                entity.HasOne(d => d.MelLoepenrNavigation)
                    .WithMany(p => p.FaSaksjournals)
                    .HasForeignKey(d => d.MelLoepenr)
                    .HasConstraintName("FK_FA_SAKSJ_UNDERSOEK_FA_UNDER");

                entity.HasOne(d => d.MynVedtakstypeNavigation)
                    .WithMany(p => p.FaSaksjournals)
                    .HasForeignKey(d => d.MynVedtakstype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SAKSJ_MYNDE_SAK_FA_VEDTA");

                entity.HasOne(d => d.SbhAvgjortavInitialerNavigation)
                    .WithMany(p => p.FaSaksjournalSbhAvgjortavInitialerNavigations)
                    .HasForeignKey(d => d.SbhAvgjortavInitialer)
                    .HasConstraintName("FK_FA_SAKSJ_SAKBEH_SA_FA_SAKSB");

                entity.HasOne(d => d.SbhBortfaltInitialerNavigation)
                    .WithMany(p => p.FaSaksjournalSbhBortfaltInitialerNavigations)
                    .HasForeignKey(d => d.SbhBortfaltInitialer)
                    .HasConstraintName("FK_FA_SAKSJ_SASKBEH_S_FA_SAKSB");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaSaksjournalSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_SAKSJ_SAKSBEH_S_FA_SAKS3");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaSaksjournalSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_SAKSJ_SAKSBEH_S_FA_SAKSB");

                entity.HasOne(d => d.SbhOpphevetInitialerNavigation)
                    .WithMany(p => p.FaSaksjournalSbhOpphevetInitialerNavigations)
                    .HasForeignKey(d => d.SbhOpphevetInitialer)
                    .HasConstraintName("FK_FA_SAKSJ_SAKSBEH_S_FA_SAKS4");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaSaksjournalSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_SAKSJ_SAKSBEH_S_FA_SAKS2");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaSaksjournals)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_FA_SAKSJ_POSTJOURN_FA_POSTJ");

                entity.HasOne(d => d.SakErstattetav)
                    .WithMany(p => p.InverseSakErstattetav)
                    .HasForeignKey(d => new { d.SakErstattetavAar, d.SakErstattetavJournalnr })
                    .HasConstraintName("FK_FA_SAKSJ_SAK_SAK_FA_SAKSJ");
            });

            modelBuilder.Entity<FaSakstype>(entity =>
            {
                entity.HasKey(e => e.SatSakstype)
                    .IsClustered(false);

                entity.ToTable("FA_SAKSTYPE");

                entity.Property(e => e.SatSakstype)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SAT_SAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.SatBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("SAT_BESKRIVELSE");

                entity.Property(e => e.SatGodkjennFhAnnen)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SAT_GODKJENN_FH_ANNEN");

                entity.Property(e => e.SatGodkjennFhEgen)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SAT_GODKJENN_FH_EGEN");

                entity.Property(e => e.SatPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAT_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaSbhSetting>(entity =>
            {
                entity.HasKey(e => e.SbhInitialer)
                    .IsClustered(false);

                entity.ToTable("FA_SBH_SETTING");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhUsenativespellcheck).HasColumnName("SBH_USENATIVESPELLCHECK");

                entity.Property(e => e.SbsSettinger)
                    .HasColumnType("image")
                    .HasColumnName("SBS_SETTINGER");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithOne(p => p.FaSbhSetting)
                    .HasForeignKey<FaSbhSetting>(d => d.SbhInitialer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SBH_S_SAKSBEH_S_FA_SAKSB");
            });

            modelBuilder.Entity<FaSetting>(entity =>
            {
                entity.HasKey(e => e.SetIdent)
                    .IsClustered(false);

                entity.ToTable("FA_SETTING");

                entity.Property(e => e.SetIdent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SET_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.SetSettinger)
                    .HasColumnType("image")
                    .HasColumnName("SET_SETTINGER");
            });

            modelBuilder.Entity<FaSoeker>(entity =>
            {
                entity.HasKey(e => e.SokLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_SOEKER");

                entity.HasIndex(e => e.PnrPostnr, "FK_FA_SOEKER1")
                    .HasFillFactor(80);

                entity.Property(e => e.SokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SOK_LOEPENR");

                entity.Property(e => e.PnrPostnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("PNR_POSTNR")
                    .IsFixedLength();

                entity.Property(e => e.SokAdresse)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("SOK_ADRESSE");

                entity.Property(e => e.SokEtternavn1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SOK_ETTERNAVN1");

                entity.Property(e => e.SokEtternavn2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SOK_ETTERNAVN2");

                entity.Property(e => e.SokFoedselsdato1)
                    .HasColumnType("datetime")
                    .HasColumnName("SOK_FOEDSELSDATO1");

                entity.Property(e => e.SokFoedselsdato2)
                    .HasColumnType("datetime")
                    .HasColumnName("SOK_FOEDSELSDATO2");

                entity.Property(e => e.SokFornavn1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SOK_FORNAVN1");

                entity.Property(e => e.SokFornavn2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SOK_FORNAVN2");

                entity.Property(e => e.SokMerknad)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SOK_MERKNAD");

                entity.Property(e => e.SokPersonnr1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SOK_PERSONNR1");

                entity.Property(e => e.SokPersonnr2)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SOK_PERSONNR2");

                entity.Property(e => e.SokTelefonarbeid1)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SOK_TELEFONARBEID1")
                    .IsFixedLength();

                entity.Property(e => e.SokTelefonarbeid2)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SOK_TELEFONARBEID2")
                    .IsFixedLength();

                entity.Property(e => e.SokTelefonmobil1)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SOK_TELEFONMOBIL1")
                    .IsFixedLength();

                entity.Property(e => e.SokTelefonmobil2)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SOK_TELEFONMOBIL2")
                    .IsFixedLength();

                entity.Property(e => e.SokTelefonprivat)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("SOK_TELEFONPRIVAT")
                    .IsFixedLength();

                entity.HasOne(d => d.PnrPostnrNavigation)
                    .WithMany(p => p.FaSoekers)
                    .HasForeignKey(d => d.PnrPostnr)
                    .HasConstraintName("FK_FA_SOEKE_POSTADRES_FA_POSTA");
            });

            modelBuilder.Entity<FaSosialeavgifter>(entity =>
            {
                entity.HasKey(e => e.SouIdent)
                    .IsClustered(false);

                entity.ToTable("FA_SOSIALEAVGIFTER");

                entity.Property(e => e.SouIdent)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SOU_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.SouBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SOU_BESKRIVELSE");

                entity.Property(e => e.SouProsentsats)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("SOU_PROSENTSATS");
            });

            modelBuilder.Entity<FaSsbStatistikk>(entity =>
            {
                entity.HasKey(e => e.SsbId);

                entity.ToTable("FA_SSB_STATISTIKK");

                entity.Property(e => e.SsbId).HasColumnName("SSB_ID");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.SbhInitialer)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.Property(e => e.SsbActiveWoActivity)
                    .HasColumnType("image")
                    .HasColumnName("SSB_ACTIVE_WO_ACTIVITY");

                entity.Property(e => e.SsbCanBeFinalized)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SSB_CAN_BE_FINALIZED");

                entity.Property(e => e.SsbErrormessages)
                    .HasColumnType("image")
                    .HasColumnName("SSB_ERRORMESSAGES");

                entity.Property(e => e.SsbFinal)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SSB_FINAL");

                entity.Property(e => e.SsbIsDownloaded)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SSB_IS_DOWNLOADED");

                entity.Property(e => e.SsbOrderdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SSB_ORDERDATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SsbParameters)
                    .IsRequired()
                    .HasColumnType("image")
                    .HasColumnName("SSB_PARAMETERS");

                entity.Property(e => e.SsbResultxml)
                    .HasColumnType("image")
                    .HasColumnName("SSB_RESULTXML");

                entity.Property(e => e.SsbValidationerrors)
                    .HasColumnType("image")
                    .HasColumnName("SSB_VALIDATIONERRORS");

                entity.Property(e => e.SsbYear).HasColumnName("SSB_YEAR");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaSsbStatistikks)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_SSB_STATISTIKK__FA_DOKUM");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaSsbStatistikks)
                    .HasForeignKey(d => d.KomKommunenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SSB_STATISTIKK_KOMMUNE");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaSsbStatistikks)
                    .HasForeignKey(d => d.SbhInitialer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_SSB_STATISTIKK_FA_SAKSB");
            });

            modelBuilder.Entity<FaSsbbegrunnelser>(entity =>
            {
                entity.HasKey(e => e.SbbIdent)
                    .IsClustered(false);

                entity.ToTable("FA_SSBBEGRUNNELSER");

                entity.Property(e => e.SbbIdent)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SBB_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.SbbBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("SBB_BESKRIVELSE");

                entity.Property(e => e.SbbKode)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("SBB_KODE");

                entity.Property(e => e.SbbPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SBB_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaStatFylkesmann1>(entity =>
            {
                entity.HasKey(e => new { e.Sf1PeriodeFradato, e.Sf1Punkt, e.DisDistriktskode, e.KliLoepenr, e.Sf1Tall })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_FYLKESMANN1");

                entity.Property(e => e.Sf1PeriodeFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("SF1_PERIODE_FRADATO");

                entity.Property(e => e.Sf1Punkt)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("SF1_PUNKT");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.Sf1Tall)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF1_TALL");

                entity.Property(e => e.Sf1Gsaaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SF1_GSAAAR");

                entity.Property(e => e.Sf1Gsajournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF1_GSAJOURNALNR");

                entity.Property(e => e.Sf1Meldingsnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF1_MELDINGSNR");

                entity.Property(e => e.Sf1Tiltaksnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF1_TILTAKSNR");
            });

            modelBuilder.Entity<FaStatFylkesmann2sum>(entity =>
            {
                entity.HasKey(e => new { e.Sf2PeriodeFradato, e.DisDistriktskode })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_FYLKESMANN2SUM");

                entity.Property(e => e.Sf2PeriodeFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("SF2_PERIODE_FRADATO");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.Sf2Adresse1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SF2_ADRESSE1");

                entity.Property(e => e.Sf2Adresse2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SF2_ADRESSE2");

                entity.Property(e => e.Sf2BvtjenesteTilsyn)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SF2_BVTJENESTE_TILSYN")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Sf2Epost)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("SF2_EPOST");

                entity.Property(e => e.Sf2Etat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SF2_ETAT");

                entity.Property(e => e.Sf2Fylke)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SF2_FYLKE");

                entity.Property(e => e.Sf2Ikkommuner)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("SF2_IKKOMMUNER");

                entity.Property(e => e.Sf2Iksamarbeid)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SF2_IKSAMARBEID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sf2Kommune)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SF2_KOMMUNE");

                entity.Property(e => e.Sf2Kommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SF2_KOMMUNENR");

                entity.Property(e => e.Sf2Kontaktperson)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SF2_KONTAKTPERSON");

                entity.Property(e => e.Sf2P11A)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_A");

                entity.Property(e => e.Sf2P11B)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_B");

                entity.Property(e => e.Sf2P11C)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_C");

                entity.Property(e => e.Sf2P11D)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_D");

                entity.Property(e => e.Sf2P11E)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_E");

                entity.Property(e => e.Sf2P11F)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_F");

                entity.Property(e => e.Sf2P11G)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_G");

                entity.Property(e => e.Sf2P11H)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SF2_P1_1_H");

                entity.Property(e => e.Sf2P2A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_A");

                entity.Property(e => e.Sf2P2B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_B");

                entity.Property(e => e.Sf2P2C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_C");

                entity.Property(e => e.Sf2P2D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_D");

                entity.Property(e => e.Sf2P2E)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_E");

                entity.Property(e => e.Sf2P2E2Overfrist)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_E2_OVERFRIST");

                entity.Property(e => e.Sf2P2E3Prosentoverfrist)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("SF2_P2_E3_PROSENTOVERFRIST");

                entity.Property(e => e.Sf2P2EHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_E_HERAV");

                entity.Property(e => e.Sf2P2F)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_F");

                entity.Property(e => e.Sf2P2G)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_G");

                entity.Property(e => e.Sf2P2GHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_G_HERAV");

                entity.Property(e => e.Sf2P2H)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_H");

                entity.Property(e => e.Sf2P2I)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_I");

                entity.Property(e => e.Sf2P2J)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P2_J");

                entity.Property(e => e.Sf2P3)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3");

                entity.Property(e => e.Sf2P31)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_1");

                entity.Property(e => e.Sf2P3A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_A");

                entity.Property(e => e.Sf2P3AntallUtvidet)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_ANTALL_UTVIDET");

                entity.Property(e => e.Sf2P3B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_B");

                entity.Property(e => e.Sf2P3C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_C");

                entity.Property(e => e.Sf2P3D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_D");

                entity.Property(e => e.Sf2P3E)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_E");

                entity.Property(e => e.Sf2P3F)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_F");

                entity.Property(e => e.Sf2P3G)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P3_G");

                entity.Property(e => e.Sf2P4A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A");

                entity.Property(e => e.Sf2P4A442)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_2");

                entity.Property(e => e.Sf2P4A442Herav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_2_HERAV");

                entity.Property(e => e.Sf2P4A445)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_5");

                entity.Property(e => e.Sf2P4A445Herav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_5_HERAV");

                entity.Property(e => e.Sf2P4A446)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_6");

                entity.Property(e => e.Sf2P4A446Herav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_6_HERAV");

                entity.Property(e => e.Sf2P4A44Ttp)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_TTP");

                entity.Property(e => e.Sf2P4A44TtpHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_4_4_TTP_HERAV");

                entity.Property(e => e.Sf2P4AHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_HERAV");

                entity.Property(e => e.Sf2P4ATtp)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_A_TTP");

                entity.Property(e => e.Sf2P4B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_B");

                entity.Property(e => e.Sf2P4BHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_B_HERAV");

                entity.Property(e => e.Sf2P4C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_C");

                entity.Property(e => e.Sf2P4CHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_C_HERAV");

                entity.Property(e => e.Sf2P4D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D");

                entity.Property(e => e.Sf2P4D424)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D_4_24");

                entity.Property(e => e.Sf2P4D426)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D_4_26");

                entity.Property(e => e.Sf2P4D427)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D_4_27");

                entity.Property(e => e.Sf2P4DFosterhjem)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D_FOSTERHJEM");

                entity.Property(e => e.Sf2P4DHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D_HERAV");

                entity.Property(e => e.Sf2P4DInstitusjon)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_D_INSTITUSJON");

                entity.Property(e => e.Sf2P4E)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_E");

                entity.Property(e => e.Sf2P4E2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_E2");

                entity.Property(e => e.Sf2P4E2Halvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_E2_HALVAAR");

                entity.Property(e => e.Sf2P4EHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_E_HALVAAR");

                entity.Property(e => e.Sf2P4F)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_F");

                entity.Property(e => e.Sf2P4FHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_F_HALVAAR");

                entity.Property(e => e.Sf2P4G)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_G");

                entity.Property(e => e.Sf2P4GHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_G_HALVAAR");

                entity.Property(e => e.Sf2P4H)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_H");

                entity.Property(e => e.Sf2P4HHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_H_HALVAAR");

                entity.Property(e => e.Sf2P4I)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_I");

                entity.Property(e => e.Sf2P4IHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_I_HALVAAR");

                entity.Property(e => e.Sf2P4J)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_J");

                entity.Property(e => e.Sf2P4JHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_J_HALVAAR");

                entity.Property(e => e.Sf2P4K)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_K");

                entity.Property(e => e.Sf2P4KHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_K_HALVAAR");

                entity.Property(e => e.Sf2P4L)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_L");

                entity.Property(e => e.Sf2P4M)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_M");

                entity.Property(e => e.Sf2P4MHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_M_HALVAAR");

                entity.Property(e => e.Sf2P4N)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_N");

                entity.Property(e => e.Sf2P4NHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P4_N_HALVAAR");

                entity.Property(e => e.Sf2P51)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_1");

                entity.Property(e => e.Sf2P51U18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_1_U18");

                entity.Property(e => e.Sf2P52)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2");

                entity.Property(e => e.Sf2P521)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_1");

                entity.Property(e => e.Sf2P522)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_2");

                entity.Property(e => e.Sf2P522Oppf)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_2_OPPF");

                entity.Property(e => e.Sf2P523)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_3");

                entity.Property(e => e.Sf2P524)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_4");

                entity.Property(e => e.Sf2P524Oppf)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_4_OPPF");

                entity.Property(e => e.Sf2P52Tilsynsfoerer)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_2_TILSYNSFOERER");

                entity.Property(e => e.Sf2P5FosterhjAnnenkom)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_FOSTERHJ_ANNENKOM");

                entity.Property(e => e.Sf2P5FosterhjEgenkom)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_FOSTERHJ_EGENKOM");

                entity.Property(e => e.Sf2P5OppfKtrlAnsv)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_OPPF_KTRL_ANSV");

                entity.Property(e => e.Sf2P5OppfKtrlAnsvOvr1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_OPPF_KTRL_ANSV_OVR_1");

                entity.Property(e => e.Sf2P5OppfKtrlAnsvUndr1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P5_OPPF_KTRL_ANSV_UNDR_1");

                entity.Property(e => e.Sf2P618SisteHalvaar)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P6_18_SISTE_HALVAAR");

                entity.Property(e => e.Sf2P6HeravAnnetOenske)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P6_HERAV_ANNET_OENSKE");

                entity.Property(e => e.Sf2P6HeravAvslag)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P6_HERAV_AVSLAG");

                entity.Property(e => e.Sf2P6HeravEgetOenske)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P6_HERAV_EGET_OENSKE");

                entity.Property(e => e.Sf2P7A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_A");

                entity.Property(e => e.Sf2P7B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_B");

                entity.Property(e => e.Sf2P7Bortfalt4132)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_BORTFALT_4_13_2");

                entity.Property(e => e.Sf2P7Bortfalt4252)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_BORTFALT_4_25_2");

                entity.Property(e => e.Sf2P7Bortfalt462)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_BORTFALT_4_6_2");

                entity.Property(e => e.Sf2P7C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_C");

                entity.Property(e => e.Sf2P7CHerav)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_C_HERAV");

                entity.Property(e => e.Sf2P7D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF2_P7_D");

                entity.Property(e => e.Sf2P8A)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SF2_P8_A")
                    .IsFixedLength();

                entity.Property(e => e.Sf2P8B)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SF2_P8_B")
                    .IsFixedLength();

                entity.Property(e => e.Sf2P8C)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SF2_P8_C")
                    .IsFixedLength();

                entity.Property(e => e.Sf2P8Kommentar)
                    .HasColumnType("text")
                    .HasColumnName("SF2_P8_KOMMENTAR");

                entity.Property(e => e.Sf2Postnr)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("SF2_POSTNR");

                entity.Property(e => e.Sf2Poststed)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SF2_POSTSTED");

                entity.Property(e => e.Sf2Tilsynsansvar)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("SF2_TILSYNSANSVAR");

                entity.Property(e => e.Sf2Tlf)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SF2_TLF");
            });

            modelBuilder.Entity<FaStatFylkesmannK2Sum>(entity =>
            {
                entity.HasKey(e => new { e.Sfk2PeriodeFradato, e.DisDistriktskode, e.Sfk2Skjemanr })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_FYLKESMANN_K2_SUM");

                entity.Property(e => e.Sfk2PeriodeFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_PERIODE_FRADATO");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2Skjemanr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK2_SKJEMANR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.Sfk2P1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK2_P1");

                entity.Property(e => e.Sfk2P10)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P10");

                entity.Property(e => e.Sfk2P11A)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P11_a");

                entity.Property(e => e.Sfk2P11B)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P11_b")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P12)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P12");

                entity.Property(e => e.Sfk2P12Loepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK2_P12_loepenr");

                entity.Property(e => e.Sfk2P13Kff1A)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_kff1_a");

                entity.Property(e => e.Sfk2P13Kff1B)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P13_kff1_b");

                entity.Property(e => e.Sfk2P13Kft2A)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_kft2_a");

                entity.Property(e => e.Sfk2P13Kft2B)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P13_kft2_b");

                entity.Property(e => e.Sfk2P13M1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_m1")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13M2)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_m2")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13M3)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_m3")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13M4)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_m4");

                entity.Property(e => e.Sfk2P13S1)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_s1");

                entity.Property(e => e.Sfk2P13S2)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_S2");

                entity.Property(e => e.Sfk2P13S3)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P13_S3");

                entity.Property(e => e.Sfk2P13T1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_t1")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13T2)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_t2")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13T3)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_t3")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13T4)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_t4");

                entity.Property(e => e.Sfk2P13U1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_u1")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13U2)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_u2")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13U3)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_u3")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P13U4)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_u4");

                entity.Property(e => e.Sfk2P13U56mnd)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P13_u5_6mnd");

                entity.Property(e => e.Sfk2P2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK2_P2");

                entity.Property(e => e.Sfk2P3)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK2_P3");

                entity.Property(e => e.Sfk2P4)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P4");

                entity.Property(e => e.Sfk2P5)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P5");

                entity.Property(e => e.Sfk2P6)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P6");

                entity.Property(e => e.Sfk2P7)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P7");

                entity.Property(e => e.Sfk2P8)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P8");

                entity.Property(e => e.Sfk2P9A)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P9_a")
                    .IsFixedLength();

                entity.Property(e => e.Sfk2P9B)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK2_P9_b");

                entity.Property(e => e.Sfk2P9C)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("SFK2_P9_c")
                    .IsFixedLength();

                entity.Property(e => e.Sfk3OversittM1120)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_M11_20");

                entity.Property(e => e.Sfk3OversittM15)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_M1_5");

                entity.Property(e => e.Sfk3OversittM20)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_M20_");

                entity.Property(e => e.Sfk3OversittM610)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_M6_10");

                entity.Property(e => e.Sfk3OversittMTot)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK3_OVERSITT_M_TOT");

                entity.Property(e => e.Sfk3OversittU15)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_U1_5");

                entity.Property(e => e.Sfk3OversittU1630)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_U16_30");

                entity.Property(e => e.Sfk3OversittU3160)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_U31_60");

                entity.Property(e => e.Sfk3OversittU615)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_U6_15");

                entity.Property(e => e.Sfk3OversittU6190)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_U61_90");

                entity.Property(e => e.Sfk3OversittU90)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK3_OVERSITT_U90_");

                entity.Property(e => e.Sfk3OversittUTot)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK3_OVERSITT_U_TOT");

                entity.Property(e => e.Sfk3TotAntall)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK3_TOT_ANTALL");

                entity.Property(e => e.Sfk3UtvidetUstid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK3_UTVIDET_USTID");
            });

            modelBuilder.Entity<FaStatFylkesmannKtrskjema>(entity =>
            {
                entity.HasKey(e => new { e.SfkPeriodeFradato, e.DisDistriktskode, e.SfkSkjemanr })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_FYLKESMANN_KTRSKJEMA");

                entity.Property(e => e.SfkPeriodeFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_PERIODE_FRADATO");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.SfkSkjemanr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK_SKJEMANR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SfkOversittM1120)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_M11_20");

                entity.Property(e => e.SfkOversittM15)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_M1_5");

                entity.Property(e => e.SfkOversittM20)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_M20_");

                entity.Property(e => e.SfkOversittM610)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_M6_10");

                entity.Property(e => e.SfkOversittMTot)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK_OVERSITT_M_TOT");

                entity.Property(e => e.SfkOversittU15)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_U1_5");

                entity.Property(e => e.SfkOversittU1630)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_U16_30");

                entity.Property(e => e.SfkOversittU3160)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_U31_60");

                entity.Property(e => e.SfkOversittU615)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_U6_15");

                entity.Property(e => e.SfkOversittU6190)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_U61_90");

                entity.Property(e => e.SfkOversittU90)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SFK_OVERSITT_U90_");

                entity.Property(e => e.SfkOversittUTot)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK_OVERSITT_U_TOT");

                entity.Property(e => e.SfkP10UndFristutvidelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P10_UND_FRISTUTVIDELSE");

                entity.Property(e => e.SfkP11UndFristoversittelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P11_UND_FRISTOVERSITTELSE");

                entity.Property(e => e.SfkP12Henlagt1)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P12_HENLAGT1");

                entity.Property(e => e.SfkP12Henlagt2)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P12_HENLAGT2");

                entity.Property(e => e.SfkP12Henlagt3)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P12_HENLAGT3");

                entity.Property(e => e.SfkP3MelMottattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_P3_MEL_MOTTATTDATO");

                entity.Property(e => e.SfkP4MelKonkludertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_P4_MEL_KONKLUDERTDATO");

                entity.Property(e => e.SfkP4MelKonklusjon)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P4_MEL_KONKLUSJON")
                    .IsFixedLength();

                entity.Property(e => e.SfkP5UndIverksattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_P5_UND_IVERKSATTDATO");

                entity.Property(e => e.SfkP6UndHenlagtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_P6_UND_HENLAGTDATO");

                entity.Property(e => e.SfkP7SakAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_P7_SAK_AVGJORTDATO");

                entity.Property(e => e.SfkP8SakSendtfylkedato)
                    .HasColumnType("datetime")
                    .HasColumnName("SFK_P8_SAK_SENDTFYLKEDATO");

                entity.Property(e => e.SfkP9MelFristoversittelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SFK_P9_MEL_FRISTOVERSITTELSE");

                entity.Property(e => e.SfkTotAntall)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK_TOT_ANTALL");

                entity.Property(e => e.SfkUtvidetUstid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SFK_UTVIDET_USTID");
            });

            modelBuilder.Entity<FaStatSsb1>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.Ss1Periode, e.DisDistriktskode })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_SSB1");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_STAT_SSB1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KomKommunenr, "FK_FA_STAT_SSB2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DisDistriktskode, "IX_FA_STAT_SSB13")
                    .HasFillFactor(80);

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.Ss1Periode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SS1_PERIODE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.Ss1Antallundersokelser)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SS1_ANTALLUNDERSOKELSER");

                entity.Property(e => e.Ss1Barnetsstatsborgerskap)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_BARNETSSTATSBORGERSKAP")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Farsfodeland)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SS1_FARSFODELAND");

                entity.Property(e => e.Ss1Fodselsnr)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SS1_FODSELSNR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Fratattforeldreansvar)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_FRATATTFORELDREANSVAR");

                entity.Property(e => e.Ss1Grunnlagforvedtaket01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET01");

                entity.Property(e => e.Ss1Grunnlagforvedtaket02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET02");

                entity.Property(e => e.Ss1Grunnlagforvedtaket03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET03");

                entity.Property(e => e.Ss1Grunnlagforvedtaket04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET04");

                entity.Property(e => e.Ss1Grunnlagforvedtaket05)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET05");

                entity.Property(e => e.Ss1Grunnlagforvedtaket06)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET06");

                entity.Property(e => e.Ss1Grunnlagforvedtaket07)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET07");

                entity.Property(e => e.Ss1Grunnlagforvedtaket08)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET08");

                entity.Property(e => e.Ss1Grunnlagforvedtaket09)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET09");

                entity.Property(e => e.Ss1Grunnlagforvedtaket10)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET10");

                entity.Property(e => e.Ss1Grunnlagforvedtaket11)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET11");

                entity.Property(e => e.Ss1Grunnlagforvedtaket12)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET12");

                entity.Property(e => e.Ss1Grunnlagforvedtaket13)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_GRUNNLAGFORVEDTAKET13");

                entity.Property(e => e.Ss1Morsfodeland)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SS1_MORSFODELAND");

                entity.Property(e => e.Ss1MynVedtakstype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_MYN_VEDTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Resultathenlagt)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_RESULTATHENLAGT")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Resultathenlagtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SS1_RESULTATHENLAGTDATO");

                entity.Property(e => e.Ss1Resultatikkeavsluttet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_RESULTATIKKEAVSLUTTET");

                entity.Property(e => e.Ss1Resultatvedtak)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_RESULTATVEDTAK")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Resultatvedtakdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SS1_RESULTATVEDTAKDATO");

                entity.Property(e => e.Ss1SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SS1_SAK_AAR");

                entity.Property(e => e.Ss1SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SS1_SAK_JOURNALNR");

                entity.Property(e => e.Ss1Sakenmeldtav01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV01");

                entity.Property(e => e.Ss1Sakenmeldtav02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV02");

                entity.Property(e => e.Ss1Sakenmeldtav03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV03");

                entity.Property(e => e.Ss1Sakenmeldtav04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV04");

                entity.Property(e => e.Ss1Sakenmeldtav05)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV05");

                entity.Property(e => e.Ss1Sakenmeldtav06)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV06");

                entity.Property(e => e.Ss1Sakenmeldtav07)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV07");

                entity.Property(e => e.Ss1Sakenmeldtav08)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV08");

                entity.Property(e => e.Ss1Sakenmeldtav09)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV09");

                entity.Property(e => e.Ss1Sakenmeldtav10)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV10");

                entity.Property(e => e.Ss1Sakenmeldtav11)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV11");

                entity.Property(e => e.Ss1Sakenmeldtav12)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV12");

                entity.Property(e => e.Ss1Sakenmeldtav13)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV13");

                entity.Property(e => e.Ss1Sakenmeldtav14)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV14");

                entity.Property(e => e.Ss1Sakenmeldtav15)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV15");

                entity.Property(e => e.Ss1Sakenmeldtav16)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV16");

                entity.Property(e => e.Ss1Sakenmeldtav17)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV17");

                entity.Property(e => e.Ss1Sakenmeldtav18)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV18");

                entity.Property(e => e.Ss1Sakenmeldtav19)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV19");

                entity.Property(e => e.Ss1Sakenmeldtav20)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKENMELDTAV20");

                entity.Property(e => e.Ss1Saksinnhold01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKSINNHOLD01");

                entity.Property(e => e.Ss1Saksinnhold02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKSINNHOLD02");

                entity.Property(e => e.Ss1Saksinnhold03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKSINNHOLD03");

                entity.Property(e => e.Ss1Saksinnhold04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS1_SAKSINNHOLD04");

                entity.Property(e => e.Ss1Sluttdatoundersokelse)
                    .HasColumnType("datetime")
                    .HasColumnName("SS1_SLUTTDATOUNDERSOKELSE");

                entity.Property(e => e.Ss1Startdatoundersokelse)
                    .HasColumnType("datetime")
                    .HasColumnName("SS1_STARTDATOUNDERSOKELSE");

                entity.Property(e => e.Ss1Tidligeretiltak)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TIDLIGERETILTAK")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak01typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK01TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak02typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK02TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak03typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK03TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak04typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK04TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak05typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK05TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak06typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK06TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak07typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK07TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak08typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK08TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak09typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK09TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak10typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK10TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak11typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK11TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak12typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK12TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak13typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK13TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak14typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK14TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak15typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK15TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak16typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK16TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak17typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK17TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak18typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK18TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak19typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK19TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak20typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK20TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak21typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK21TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak22typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK22TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23tidlaar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23TIDLAAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Tiltak23typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TILTAK23TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Typeforstetiltak)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS1_TYPEFORSTETILTAK")
                    .IsFixedLength();

                entity.Property(e => e.Ss1Undersokelseintlopenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SS1_UNDERSOKELSEINTLOPENR");

                entity.Property(e => e.Ss1Viktigstegrunnlag)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SS1_VIKTIGSTEGRUNNLAG")
                    .IsFixedLength();

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaStatSsb1s)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_STAT__KLIENT_SS_FA_KLIEN");

                entity.HasOne(d => d.KomKommunenrNavigation)
                    .WithMany(p => p.FaStatSsb1s)
                    .HasForeignKey(d => d.KomKommunenr)
                    .HasConstraintName("FK_FA_STAT__KOMMNUNE__FA_KOMMU");
            });

            modelBuilder.Entity<FaStatSsb2>(entity =>
            {
                entity.HasKey(e => new { e.KliLoepenr, e.Ss1Periode, e.DisDistriktskode })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_SSB2");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.Ss1Periode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("SS1_PERIODE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Avsluttet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_AVSLUTTET");

                entity.Property(e => e.Ss2Barnetborhos)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("SS2_BARNETBORHOS")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Grunnlagforvedtaket14)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_GRUNNLAGFORVEDTAKET14");

                entity.Property(e => e.Ss2Grunnlagforvedtaket15)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_GRUNNLAGFORVEDTAKET15");

                entity.Property(e => e.Ss2Grunnlagforvedtaket16)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_GRUNNLAGFORVEDTAKET16");

                entity.Property(e => e.Ss2Grunnlagforvedtaket17)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_GRUNNLAGFORVEDTAKET17");

                entity.Property(e => e.Ss2Hovedgrunnavsluttet)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_HOVEDGRUNNAVSLUTTET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Plassering)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_PLASSERING")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Plassertannenkommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SS2_PLASSERTANNENKOMMUNENR");

                entity.Property(e => e.Ss2Punkt141)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_1");

                entity.Property(e => e.Ss2Punkt1410)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_10");

                entity.Property(e => e.Ss2Punkt142)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_2");

                entity.Property(e => e.Ss2Punkt143)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_3");

                entity.Property(e => e.Ss2Punkt144)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_4");

                entity.Property(e => e.Ss2Punkt145)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_5");

                entity.Property(e => e.Ss2Punkt146)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_6");

                entity.Property(e => e.Ss2Punkt147)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_7");

                entity.Property(e => e.Ss2Punkt148)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_8");

                entity.Property(e => e.Ss2Punkt149)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_PUNKT14_9");

                entity.Property(e => e.Ss2Tiltak01tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK01TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak01tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK01TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak02tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK02TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak02tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK02TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak03tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK03TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak03tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK03TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak04tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK04TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak04tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK04TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak05tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK05TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak05tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK05TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak06tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK06TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak06tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK06TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak07tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK07TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak07tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK07TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak08tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK08TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak08tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK08TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak09tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK09TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak09tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK09TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak10tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK10TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak10tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK10TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak11tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK11TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak11tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK11TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak12tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK12TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak12tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK12TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak13tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK13TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak13tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK13TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak14tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK14TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak14tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK14TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak15tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK15TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak15tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK15TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak16tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK16TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak16tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK16TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak17tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK17TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak17tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK17TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak18tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK18TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak18tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK18TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak19tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK19TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak19tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK19TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak20tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK20TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak20tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK20TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak21tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK21TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak21tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK21TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak22tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK22TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak22tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK22TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak23tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK23TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak23tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK23TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak24antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak24lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak24lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak24tidlar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24TIDLAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak24tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak24tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK24TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak24tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK24TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak24typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak24typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK24TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25tidlar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25TIDLAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK25TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak25tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK25TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak25typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak25typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK25TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26tidlar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26TIDLAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK26TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak26tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK26TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak26typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak26typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK26TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27tidlar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27TIDLAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK27TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak27tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK27TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak27typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak27typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK27TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28antmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28ANTMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28lovgrunnlag1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28LOVGRUNNLAG1")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28lovgrunnlag2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28LOVGRUNNLAG2")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28tidlar)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28TIDLAR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28tidlmnd)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28TIDLMND")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28tiltakihjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK28TILTAKIHJEM");

                entity.Property(e => e.Ss2Tiltak28tiltakuhjem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAK28TILTAKUHJEM");

                entity.Property(e => e.Ss2Tiltak28typetiltakh)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28TYPETILTAKH")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltak28typetiltako)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_TILTAK28TYPETILTAKO")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Tiltakh01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH01");

                entity.Property(e => e.Ss2Tiltakh02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH02");

                entity.Property(e => e.Ss2Tiltakh03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH03");

                entity.Property(e => e.Ss2Tiltakh04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH04");

                entity.Property(e => e.Ss2Tiltakh05)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH05");

                entity.Property(e => e.Ss2Tiltakh06)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH06");

                entity.Property(e => e.Ss2Tiltakh07)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH07");

                entity.Property(e => e.Ss2Tiltakh08)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH08");

                entity.Property(e => e.Ss2Tiltakh09)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH09");

                entity.Property(e => e.Ss2Tiltakh10)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH10");

                entity.Property(e => e.Ss2Tiltakh11)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH11");

                entity.Property(e => e.Ss2Tiltakh12)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH12");

                entity.Property(e => e.Ss2Tiltakh13)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH13");

                entity.Property(e => e.Ss2Tiltakh14)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH14");

                entity.Property(e => e.Ss2Tiltakh15)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH15");

                entity.Property(e => e.Ss2Tiltakh16)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH16");

                entity.Property(e => e.Ss2Tiltakh17)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH17");

                entity.Property(e => e.Ss2Tiltakh18)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH18");

                entity.Property(e => e.Ss2Tiltakh19)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH19");

                entity.Property(e => e.Ss2Tiltakh20)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH20");

                entity.Property(e => e.Ss2Tiltakh21)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH21");

                entity.Property(e => e.Ss2Tiltakh22)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH22");

                entity.Property(e => e.Ss2Tiltakh23)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH23");

                entity.Property(e => e.Ss2Tiltakh24)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH24");

                entity.Property(e => e.Ss2Tiltakh25)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH25");

                entity.Property(e => e.Ss2Tiltakh26)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH26");

                entity.Property(e => e.Ss2Tiltakh27)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH27");

                entity.Property(e => e.Ss2Tiltakh28)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKH28");

                entity.Property(e => e.Ss2Tiltakih01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH01");

                entity.Property(e => e.Ss2Tiltakih02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH02");

                entity.Property(e => e.Ss2Tiltakih03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH03");

                entity.Property(e => e.Ss2Tiltakih04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH04");

                entity.Property(e => e.Ss2Tiltakih05)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH05");

                entity.Property(e => e.Ss2Tiltakih06)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH06");

                entity.Property(e => e.Ss2Tiltakih07)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH07");

                entity.Property(e => e.Ss2Tiltakih08)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH08");

                entity.Property(e => e.Ss2Tiltakih09)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH09");

                entity.Property(e => e.Ss2Tiltakih10)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH10");

                entity.Property(e => e.Ss2Tiltakih11)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH11");

                entity.Property(e => e.Ss2Tiltakih12)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH12");

                entity.Property(e => e.Ss2Tiltakih13)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH13");

                entity.Property(e => e.Ss2Tiltakih14)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH14");

                entity.Property(e => e.Ss2Tiltakih15)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH15");

                entity.Property(e => e.Ss2Tiltakih16)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH16");

                entity.Property(e => e.Ss2Tiltakih17)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH17");

                entity.Property(e => e.Ss2Tiltakih18)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH18");

                entity.Property(e => e.Ss2Tiltakih19)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH19");

                entity.Property(e => e.Ss2Tiltakih20)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH20");

                entity.Property(e => e.Ss2Tiltakih21)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH21");

                entity.Property(e => e.Ss2Tiltakih22)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH22");

                entity.Property(e => e.Ss2Tiltakih23)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH23");

                entity.Property(e => e.Ss2Tiltakih24)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH24");

                entity.Property(e => e.Ss2Tiltakih25)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH25");

                entity.Property(e => e.Ss2Tiltakih26)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH26");

                entity.Property(e => e.Ss2Tiltakih27)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH27");

                entity.Property(e => e.Ss2Tiltakih28)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKIH28");

                entity.Property(e => e.Ss2Tiltako01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO01");

                entity.Property(e => e.Ss2Tiltako02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO02");

                entity.Property(e => e.Ss2Tiltako03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO03");

                entity.Property(e => e.Ss2Tiltako04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO04");

                entity.Property(e => e.Ss2Tiltako05)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO05");

                entity.Property(e => e.Ss2Tiltako06)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO06");

                entity.Property(e => e.Ss2Tiltako07)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO07");

                entity.Property(e => e.Ss2Tiltako08)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO08");

                entity.Property(e => e.Ss2Tiltako09)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO09");

                entity.Property(e => e.Ss2Tiltako10)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO10");

                entity.Property(e => e.Ss2Tiltako11)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO11");

                entity.Property(e => e.Ss2Tiltako12)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO12");

                entity.Property(e => e.Ss2Tiltako13)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO13");

                entity.Property(e => e.Ss2Tiltako14)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO14");

                entity.Property(e => e.Ss2Tiltako15)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO15");

                entity.Property(e => e.Ss2Tiltako16)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO16");

                entity.Property(e => e.Ss2Tiltako17)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO17");

                entity.Property(e => e.Ss2Tiltako18)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO18");

                entity.Property(e => e.Ss2Tiltako19)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO19");

                entity.Property(e => e.Ss2Tiltako20)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO20");

                entity.Property(e => e.Ss2Tiltako21)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO21");

                entity.Property(e => e.Ss2Tiltako22)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO22");

                entity.Property(e => e.Ss2Tiltako23)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO23");

                entity.Property(e => e.Ss2Tiltako24)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO24");

                entity.Property(e => e.Ss2Tiltako25)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO25");

                entity.Property(e => e.Ss2Tiltako26)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO26");

                entity.Property(e => e.Ss2Tiltako27)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO27");

                entity.Property(e => e.Ss2Tiltako28)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKO28");

                entity.Property(e => e.Ss2Tiltaksplan)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKSPLAN");

                entity.Property(e => e.Ss2Tiltakuh01)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH01");

                entity.Property(e => e.Ss2Tiltakuh02)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH02");

                entity.Property(e => e.Ss2Tiltakuh03)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH03");

                entity.Property(e => e.Ss2Tiltakuh04)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH04");

                entity.Property(e => e.Ss2Tiltakuh05)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH05");

                entity.Property(e => e.Ss2Tiltakuh06)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH06");

                entity.Property(e => e.Ss2Tiltakuh07)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH07");

                entity.Property(e => e.Ss2Tiltakuh08)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH08");

                entity.Property(e => e.Ss2Tiltakuh09)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH09");

                entity.Property(e => e.Ss2Tiltakuh10)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH10");

                entity.Property(e => e.Ss2Tiltakuh11)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH11");

                entity.Property(e => e.Ss2Tiltakuh12)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH12");

                entity.Property(e => e.Ss2Tiltakuh13)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH13");

                entity.Property(e => e.Ss2Tiltakuh14)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH14");

                entity.Property(e => e.Ss2Tiltakuh15)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH15");

                entity.Property(e => e.Ss2Tiltakuh16)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH16");

                entity.Property(e => e.Ss2Tiltakuh17)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH17");

                entity.Property(e => e.Ss2Tiltakuh18)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH18");

                entity.Property(e => e.Ss2Tiltakuh19)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH19");

                entity.Property(e => e.Ss2Tiltakuh20)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH20");

                entity.Property(e => e.Ss2Tiltakuh21)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH21");

                entity.Property(e => e.Ss2Tiltakuh22)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH22");

                entity.Property(e => e.Ss2Tiltakuh23)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH23");

                entity.Property(e => e.Ss2Tiltakuh24)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH24");

                entity.Property(e => e.Ss2Tiltakuh25)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH25");

                entity.Property(e => e.Ss2Tiltakuh26)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH26");

                entity.Property(e => e.Ss2Tiltakuh27)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH27");

                entity.Property(e => e.Ss2Tiltakuh28)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SS2_TILTAKUH28");

                entity.Property(e => e.Ss2UtilsiktetFlytting)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_UTILSIKTET_FLYTTING")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak1Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK1_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak1Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK1_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak1Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK1_VARIGHET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak2Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK2_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak2Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK2_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak2Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK2_VARIGHET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak3Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK3_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak3Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK3_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak3Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK3_VARIGHET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak4Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK4_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak4Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK4_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak4Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK4_VARIGHET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak5Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK5_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak5Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK5_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak5Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK5_VARIGHET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak6Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK6_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak6Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK6_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak6Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK6_VARIGHET")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak7Nr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK7_NR")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak7Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK7_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Ss2Vedtak7Varighet)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SS2_VEDTAK7_VARIGHET")
                    .IsFixedLength();

                entity.HasOne(d => d.FaStatSsb1)
                    .WithOne(p => p.FaStatSsb2)
                    .HasForeignKey<FaStatSsb2>(d => new { d.KliLoepenr, d.Ss1Periode, d.DisDistriktskode })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_STAT__SSBSTAT1__FA_STAT_");
            });

            modelBuilder.Entity<FaStatTabell22>(entity =>
            {
                entity.HasKey(e => new { e.D22PeriodeStartdato, e.D22PeriodeSluttdato, e.KliLoepenr, e.D22Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL22");

                entity.HasIndex(e => e.D22Distrikter, "IX_FA_STAT_TABELL22_DISTR");

                entity.Property(e => e.D22PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D22_PERIODE_STARTDATO");

                entity.Property(e => e.D22PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D22_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D22Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D22_DISTRIKTER");

                entity.Property(e => e.D22Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D22_DISTRIKTSNAVN");

                entity.Property(e => e.D22Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D22_KLIENTGRUPPER");

                entity.Property(e => e.D22Klientnavn)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("D22_KLIENTNAVN");

                entity.Property(e => e.D22Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D22_SAKSBEHANDLERE");

                entity.Property(e => e.D22Tall1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL1");

                entity.Property(e => e.D22Tall11)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL11");

                entity.Property(e => e.D22Tall2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL2");

                entity.Property(e => e.D22Tall4)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL4");

                entity.Property(e => e.D22Tall5)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL5");

                entity.Property(e => e.D22Tall6)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL6");

                entity.Property(e => e.D22Tall8)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL8");

                entity.Property(e => e.D22Tall9)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D22_TALL9");
            });

            modelBuilder.Entity<FaStatTabell22sum>(entity =>
            {
                entity.HasKey(e => new { e.S22PeriodeStartdato, e.S22PeriodeSluttdato, e.S22Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL22SUM");

                entity.HasIndex(e => e.S22Distrikter, "IX_FA_STAT_TABELL22SUM_DISTR");

                entity.Property(e => e.S22PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S22_PERIODE_STARTDATO");

                entity.Property(e => e.S22PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S22_PERIODE_SLUTTDATO");

                entity.Property(e => e.S22Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S22_DISTRIKTER");

                entity.Property(e => e.S22Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S22_DISTRIKTSNAVN");

                entity.Property(e => e.S22Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S22_KLIENTGRUPPER");

                entity.Property(e => e.S22Punkt1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT1");

                entity.Property(e => e.S22Punkt11)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT11");

                entity.Property(e => e.S22Punkt2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT2");

                entity.Property(e => e.S22Punkt4)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT4");

                entity.Property(e => e.S22Punkt5)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT5");

                entity.Property(e => e.S22Punkt6)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT6");

                entity.Property(e => e.S22Punkt8)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT8");

                entity.Property(e => e.S22Punkt9)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S22_PUNKT9");

                entity.Property(e => e.S22Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S22_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell23>(entity =>
            {
                entity.HasKey(e => new { e.D23PeriodeStartdato, e.D23PeriodeSluttdato, e.KliLoepenr, e.D23Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL23");

                entity.HasIndex(e => e.D23Distrikter, "IX_FA_STAT_TABELL23_DISTR");

                entity.Property(e => e.D23PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D23_PERIODE_STARTDATO");

                entity.Property(e => e.D23PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D23_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D23Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D23_DISTRIKTER");

                entity.Property(e => e.D23Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D23_DISTRIKTSNAVN");

                entity.Property(e => e.D23Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D23_KLIENTGRUPPER");

                entity.Property(e => e.D23Punkt1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT1");

                entity.Property(e => e.D23Punkt10)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT10");

                entity.Property(e => e.D23Punkt11)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT11");

                entity.Property(e => e.D23Punkt12)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT12");

                entity.Property(e => e.D23Punkt13)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT13");

                entity.Property(e => e.D23Punkt131)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT13_1");

                entity.Property(e => e.D23Punkt132)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT13_2");

                entity.Property(e => e.D23Punkt14)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT14");

                entity.Property(e => e.D23Punkt15)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT15");

                entity.Property(e => e.D23Punkt16)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT16");

                entity.Property(e => e.D23Punkt161)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT16_1");

                entity.Property(e => e.D23Punkt162)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT16_2");

                entity.Property(e => e.D23Punkt18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT18");

                entity.Property(e => e.D23Punkt181)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT18_1");

                entity.Property(e => e.D23Punkt19)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT19");

                entity.Property(e => e.D23Punkt2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT2");

                entity.Property(e => e.D23Punkt3)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT3");

                entity.Property(e => e.D23Punkt4)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT4");

                entity.Property(e => e.D23Punkt41)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT4_1");

                entity.Property(e => e.D23Punkt5)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT5");

                entity.Property(e => e.D23Punkt51)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT5_1");

                entity.Property(e => e.D23Punkt52)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT5_2");

                entity.Property(e => e.D23Punkt53)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT5_3");

                entity.Property(e => e.D23Punkt54)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT5_4");

                entity.Property(e => e.D23Punkt6)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT6");

                entity.Property(e => e.D23Punkt7)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT7");

                entity.Property(e => e.D23Punkt8)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT8");

                entity.Property(e => e.D23Punkt9)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D23_PUNKT9");

                entity.Property(e => e.D23Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D23_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell23G>(entity =>
            {
                entity.HasKey(e => new { e.G23PeriodeStartdato, e.G23PeriodeSluttdato, e.KliLoepenr, e.G23Distrikter, e.G23Punkt, e.G23Teller })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL23_G");

                entity.HasIndex(e => e.G23Distrikter, "IX_FA_STAT_TABELL22_G_DISTR");

                entity.Property(e => e.G23PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G23_PERIODE_STARTDATO");

                entity.Property(e => e.G23PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G23_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.G23Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G23_DISTRIKTER");

                entity.Property(e => e.G23Punkt)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("G23_PUNKT")
                    .IsFixedLength();

                entity.Property(e => e.G23Teller)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G23_TELLER");

                entity.Property(e => e.G23Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("G23_DISTRIKTSNAVN");

                entity.Property(e => e.G23Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G23_KLIENTGRUPPER");

                entity.Property(e => e.G23Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G23_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell23sum>(entity =>
            {
                entity.HasKey(e => new { e.S23PeriodeStartdato, e.S23PeriodeSluttdato, e.S23Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL23SUM");

                entity.HasIndex(e => e.S23Distrikter, "IX_FA_STAT_TABELL23SUM_DISTR");

                entity.Property(e => e.S23PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S23_PERIODE_STARTDATO");

                entity.Property(e => e.S23PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S23_PERIODE_SLUTTDATO");

                entity.Property(e => e.S23Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S23_DISTRIKTER");

                entity.Property(e => e.S23Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S23_DISTRIKTSNAVN");

                entity.Property(e => e.S23Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S23_KLIENTGRUPPER");

                entity.Property(e => e.S23Punkt1)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT1");

                entity.Property(e => e.S23Punkt10)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT10");

                entity.Property(e => e.S23Punkt11)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT11");

                entity.Property(e => e.S23Punkt12)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT12");

                entity.Property(e => e.S23Punkt13)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT13");

                entity.Property(e => e.S23Punkt131)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT13_1");

                entity.Property(e => e.S23Punkt132)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT13_2");

                entity.Property(e => e.S23Punkt14)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT14");

                entity.Property(e => e.S23Punkt15)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT15");

                entity.Property(e => e.S23Punkt16)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT16");

                entity.Property(e => e.S23Punkt161)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT16_1");

                entity.Property(e => e.S23Punkt162)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT16_2");

                entity.Property(e => e.S23Punkt17)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT17");

                entity.Property(e => e.S23Punkt18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT18");

                entity.Property(e => e.S23Punkt181)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT18_1");

                entity.Property(e => e.S23Punkt19)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT19");

                entity.Property(e => e.S23Punkt2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT2");

                entity.Property(e => e.S23Punkt3)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT3");

                entity.Property(e => e.S23Punkt4)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT4");

                entity.Property(e => e.S23Punkt41)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT4_1");

                entity.Property(e => e.S23Punkt5)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT5");

                entity.Property(e => e.S23Punkt51)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT5_1");

                entity.Property(e => e.S23Punkt52)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT5_2");

                entity.Property(e => e.S23Punkt53)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT5_3");

                entity.Property(e => e.S23Punkt54)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT5_4");

                entity.Property(e => e.S23Punkt6)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT6");

                entity.Property(e => e.S23Punkt7)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT7");

                entity.Property(e => e.S23Punkt8)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT8");

                entity.Property(e => e.S23Punkt9)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S23_PUNKT9");

                entity.Property(e => e.S23Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S23_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell241>(entity =>
            {
                entity.HasKey(e => new { e.D241PeriodeStartdato, e.D241PeriodeSluttdato, e.KliLoepenr, e.D241Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL241");

                entity.HasIndex(e => e.D241Distrikter, "IX_FA_STAT_TABELL241_DISTR");

                entity.Property(e => e.D241PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D241_PERIODE_STARTDATO");

                entity.Property(e => e.D241PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D241_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D241Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D241_DISTRIKTER");

                entity.Property(e => e.D241Dato18aar)
                    .HasColumnType("datetime")
                    .HasColumnName("D241_DATO18AAR");

                entity.Property(e => e.D241Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D241_DISTRIKTSNAVN");

                entity.Property(e => e.D241Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D241_KLIENTGRUPPER");

                entity.Property(e => e.D241Punkt111Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_1_1_FLY");

                entity.Property(e => e.D241Punkt111Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_1_1_HEL");

                entity.Property(e => e.D241Punkt111Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_1_1_PR");

                entity.Property(e => e.D241Punkt11Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_1_FLY");

                entity.Property(e => e.D241Punkt11Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_1_HEL");

                entity.Property(e => e.D241Punkt11Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_1_PR");

                entity.Property(e => e.D241Punkt12Sum1Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT12_SUM_1_FLY");

                entity.Property(e => e.D241Punkt12Sum1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT12_SUM_1_HEL");

                entity.Property(e => e.D241Punkt12Sum1Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT12_SUM_1_PR");

                entity.Property(e => e.D241Punkt12SumFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT12_SUM_FLY");

                entity.Property(e => e.D241Punkt12SumHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT12_SUM_HEL");

                entity.Property(e => e.D241Punkt12SumPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT12_SUM_PR");

                entity.Property(e => e.D241Punkt1Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_FLY");

                entity.Property(e => e.D241Punkt1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_HEL");

                entity.Property(e => e.D241Punkt1Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT1_PR");

                entity.Property(e => e.D241Punkt21Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_1_FLY");

                entity.Property(e => e.D241Punkt21Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_1_HEL");

                entity.Property(e => e.D241Punkt21Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_1_PR");

                entity.Property(e => e.D241Punkt221Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_2_1_PR");

                entity.Property(e => e.D241Punkt222Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_2_2_PR");

                entity.Property(e => e.D241Punkt223Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_2_3_PR");

                entity.Property(e => e.D241Punkt224Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_2_4_PR");

                entity.Property(e => e.D241Punkt22Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_2_PR");

                entity.Property(e => e.D241Punkt2Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_FLY");

                entity.Property(e => e.D241Punkt2Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_HEL");

                entity.Property(e => e.D241Punkt2Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT2_PR");

                entity.Property(e => e.D241Punkt3FylktiltakFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT3_FYLKTILTAK_FLY");

                entity.Property(e => e.D241Punkt3FylktiltakHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT3_FYLKTILTAK_HEL");

                entity.Property(e => e.D241Punkt3FylktiltakPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT3_FYLKTILTAK_PR");

                entity.Property(e => e.D241Punkt4HjptiltakFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT4_HJPTILTAK_FLY");

                entity.Property(e => e.D241Punkt4HjptiltakHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT4_HJPTILTAK_HEL");

                entity.Property(e => e.D241Punkt4HjptiltakPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT4_HJPTILTAK_PR");

                entity.Property(e => e.D241Punkt5AkuttFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT5_AKUTT_FLY");

                entity.Property(e => e.D241Punkt5AkuttHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT5_AKUTT_HEL");

                entity.Property(e => e.D241Punkt5AkuttPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT5_AKUTT_PR");

                entity.Property(e => e.D241Punkt6Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT6_HEL");

                entity.Property(e => e.D241Punkt6Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241_PUNKT6_PR");

                entity.Property(e => e.D241Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D241_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell241G>(entity =>
            {
                entity.HasKey(e => new { e.G241PeriodeStartdato, e.G241PeriodeSluttdato, e.KliLoepenr, e.G241Distrikter, e.G241Punkt, e.G241Teller })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL241_G");

                entity.HasIndex(e => e.G241Distrikter, "IX_FA_STAT_TABELL241_G_DISTR");

                entity.Property(e => e.G241PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G241_PERIODE_STARTDATO");

                entity.Property(e => e.G241PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G241_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.G241Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G241_DISTRIKTER");

                entity.Property(e => e.G241Punkt)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("G241_PUNKT")
                    .IsFixedLength();

                entity.Property(e => e.G241Teller)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G241_TELLER");

                entity.Property(e => e.G241Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("G241_DISTRIKTSNAVN");

                entity.Property(e => e.G241Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G241_KLIENTGRUPPER");

                entity.Property(e => e.G241Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G241_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell241b>(entity =>
            {
                entity.HasKey(e => new { e.D241bPeriodeStartdato, e.D241bPeriodeSluttdato, e.KliLoepenr, e.D241bDistrikter, e.D241bPunkt, e.D241bGyldigPlan })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL241B");

                entity.HasIndex(e => e.D241bDistrikter, "IX_FA_STAT_TABELL241B_DISTR");

                entity.Property(e => e.D241bPeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D241B_PERIODE_STARTDATO");

                entity.Property(e => e.D241bPeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D241B_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D241bDistrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D241B_DISTRIKTER");

                entity.Property(e => e.D241bPunkt)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("D241B_PUNKT");

                entity.Property(e => e.D241bGyldigPlan)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("D241B_GYLDIG_PLAN");

                entity.Property(e => e.D241bDistriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D241B_DISTRIKTSNAVN");

                entity.Property(e => e.D241bKlientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D241B_KLIENTGRUPPER");

                entity.Property(e => e.D241bLovHovedparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("D241B_LOV_HOVEDPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.D241bRapportHovedpunkt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("D241B_RAPPORT_HOVEDPUNKT");

                entity.Property(e => e.D241bSaksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D241B_SAKSBEHANDLERE");

                entity.Property(e => e.D241bTiltaksnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D241B_TILTAKSNR");
            });

            modelBuilder.Entity<FaStatTabell241bRapp>(entity =>
            {
                entity.HasKey(e => new { e.R241bPeriodeStartdato, e.R241bPeriodeSluttdato, e.KliLoepenr, e.R241bDistrikter, e.R241bPunkt })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL241B_RAPP");

                entity.HasIndex(e => e.R241bDistrikter, "IX_FA_STAT_TABELL241B_RAPP_DISTR");

                entity.Property(e => e.R241bPeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("R241B_PERIODE_STARTDATO");

                entity.Property(e => e.R241bPeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("R241B_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.R241bDistrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("R241B_DISTRIKTER");

                entity.Property(e => e.R241bPunkt)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("R241B_PUNKT");

                entity.Property(e => e.R241bDistriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("R241B_DISTRIKTSNAVN");

                entity.Property(e => e.R241bGyldigPlan)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("R241B_GYLDIG_PLAN")
                    .IsFixedLength();

                entity.Property(e => e.R241bKlientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("R241B_KLIENTGRUPPER");

                entity.Property(e => e.R241bLovHovedparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("R241B_LOV_HOVEDPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.R241bRapportHovedpunkt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("R241B_RAPPORT_HOVEDPUNKT");

                entity.Property(e => e.R241bSaksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("R241B_SAKSBEHANDLERE");

                entity.Property(e => e.R241bTiltaksnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("R241B_TILTAKSNR");
            });

            modelBuilder.Entity<FaStatTabell241bSum>(entity =>
            {
                entity.HasKey(e => new { e.S241bPeriodeStartdato, e.S241bPeriodeSluttdato, e.S241bDistrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL241B_SUM");

                entity.HasIndex(e => e.S241bDistrikter, "IX_FA_STAT_TABELL241B_SUM_DISTR");

                entity.Property(e => e.S241bPeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S241B_PERIODE_STARTDATO");

                entity.Property(e => e.S241bPeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S241B_PERIODE_SLUTTDATO");

                entity.Property(e => e.S241bDistrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S241B_DISTRIKTER");

                entity.Property(e => e.S241b1aHeravTtp)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241B_1A_HERAV_TTP");

                entity.Property(e => e.S241b1aHjelp)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241B_1A_HJELP");

                entity.Property(e => e.S241b2bAndelMTtp)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("S241B_2B_ANDEL_M_TTP");

                entity.Property(e => e.S241b3cHeravOplan)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241B_3C_HERAV_OPLAN");

                entity.Property(e => e.S241b3cOmsorg)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241B_3C_OMSORG");

                entity.Property(e => e.S241b4dAndelMOplan)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("S241B_4D_ANDEL_M_OPLAN");

                entity.Property(e => e.S241bDistriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S241B_DISTRIKTSNAVN");

                entity.Property(e => e.S241bKlientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S241B_KLIENTGRUPPER");

                entity.Property(e => e.S241bSaksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S241B_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell241sum>(entity =>
            {
                entity.HasKey(e => new { e.S241PeriodeStartdato, e.S241PeriodeSluttdato, e.S241Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL241SUM");

                entity.HasIndex(e => e.S241Distrikter, "IX_FA_STAT_TABELL241SUM_DISTR");

                entity.Property(e => e.S241PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S241_PERIODE_STARTDATO");

                entity.Property(e => e.S241PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S241_PERIODE_SLUTTDATO");

                entity.Property(e => e.S241Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S241_DISTRIKTER");

                entity.Property(e => e.S241Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S241_DISTRIKTSNAVN");

                entity.Property(e => e.S241Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S241_KLIENTGRUPPER");

                entity.Property(e => e.S241Punkt111Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_1_1_FLY");

                entity.Property(e => e.S241Punkt111Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_1_1_HEL");

                entity.Property(e => e.S241Punkt111Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_1_1_PR");

                entity.Property(e => e.S241Punkt11Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_1_FLY");

                entity.Property(e => e.S241Punkt11Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_1_HEL");

                entity.Property(e => e.S241Punkt11Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_1_PR");

                entity.Property(e => e.S241Punkt12Sum1Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT12_SUM_1_FLY");

                entity.Property(e => e.S241Punkt12Sum1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT12_SUM_1_HEL");

                entity.Property(e => e.S241Punkt12Sum1Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT12_SUM_1_PR");

                entity.Property(e => e.S241Punkt12SumFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT12_SUM_FLY");

                entity.Property(e => e.S241Punkt12SumHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT12_SUM_HEL");

                entity.Property(e => e.S241Punkt12SumPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT12_SUM_PR");

                entity.Property(e => e.S241Punkt1Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_FLY");

                entity.Property(e => e.S241Punkt1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_HEL");

                entity.Property(e => e.S241Punkt1Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT1_PR");

                entity.Property(e => e.S241Punkt21Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_1_FLY");

                entity.Property(e => e.S241Punkt21Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_1_HEL");

                entity.Property(e => e.S241Punkt21Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_1_PR");

                entity.Property(e => e.S241Punkt221Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_2_1_PR");

                entity.Property(e => e.S241Punkt222Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_2_2_PR");

                entity.Property(e => e.S241Punkt223Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_2_3_PR");

                entity.Property(e => e.S241Punkt224Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_2_4_PR");

                entity.Property(e => e.S241Punkt22Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_2_PR");

                entity.Property(e => e.S241Punkt2Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_FLY");

                entity.Property(e => e.S241Punkt2Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_HEL");

                entity.Property(e => e.S241Punkt2Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT2_PR");

                entity.Property(e => e.S241Punkt3FylktiltakFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT3_FYLKTILTAK_FLY");

                entity.Property(e => e.S241Punkt3FylktiltakHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT3_FYLKTILTAK_HEL");

                entity.Property(e => e.S241Punkt3FylktiltakPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT3_FYLKTILTAK_PR");

                entity.Property(e => e.S241Punkt4HjptiltakFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT4_HJPTILTAK_FLY");

                entity.Property(e => e.S241Punkt4HjptiltakHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT4_HJPTILTAK_HEL");

                entity.Property(e => e.S241Punkt4HjptiltakPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT4_HJPTILTAK_PR");

                entity.Property(e => e.S241Punkt5AkuttFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT5_AKUTT_FLY");

                entity.Property(e => e.S241Punkt5AkuttHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT5_AKUTT_HEL");

                entity.Property(e => e.S241Punkt5AkuttPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT5_AKUTT_PR");

                entity.Property(e => e.S241Punkt6Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT6_HEL");

                entity.Property(e => e.S241Punkt6Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S241_PUNKT6_PR");

                entity.Property(e => e.S241Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S241_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell242>(entity =>
            {
                entity.HasKey(e => new { e.D242PeriodeStartdato, e.D242PeriodeSluttdato, e.KliLoepenr, e.D242Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL242");

                entity.HasIndex(e => e.D242Distrikter, "IX_FA_STAT_TABELL242_DISTR");

                entity.Property(e => e.D242PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D242_PERIODE_STARTDATO");

                entity.Property(e => e.D242PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D242_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D242Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D242_DISTRIKTER");

                entity.Property(e => e.D242Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D242_DISTRIKTSNAVN");

                entity.Property(e => e.D242Innvandrer)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("D242_INNVANDRER");

                entity.Property(e => e.D242Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D242_KLIENTGRUPPER");

                entity.Property(e => e.D242Punkt111A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_A");

                entity.Property(e => e.D242Punkt111B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_B");

                entity.Property(e => e.D242Punkt111C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_C");

                entity.Property(e => e.D242Punkt111D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_D");

                entity.Property(e => e.D242Punkt111Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_FLY");

                entity.Property(e => e.D242Punkt111Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_HEL");

                entity.Property(e => e.D242Punkt111Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1_PR");

                entity.Property(e => e.D242Punkt111iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_A");

                entity.Property(e => e.D242Punkt111iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_B");

                entity.Property(e => e.D242Punkt111iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_C");

                entity.Property(e => e.D242Punkt111iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_D");

                entity.Property(e => e.D242Punkt111iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_FLY");

                entity.Property(e => e.D242Punkt111iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_HEL");

                entity.Property(e => e.D242Punkt111iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_1I_PR");

                entity.Property(e => e.D242Punkt112A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_A");

                entity.Property(e => e.D242Punkt112B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_B");

                entity.Property(e => e.D242Punkt112C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_C");

                entity.Property(e => e.D242Punkt112D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_D");

                entity.Property(e => e.D242Punkt112Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_FLY");

                entity.Property(e => e.D242Punkt112Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_HEL");

                entity.Property(e => e.D242Punkt112Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2_PR");

                entity.Property(e => e.D242Punkt112iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_A");

                entity.Property(e => e.D242Punkt112iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_B");

                entity.Property(e => e.D242Punkt112iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_C");

                entity.Property(e => e.D242Punkt112iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_D");

                entity.Property(e => e.D242Punkt112iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_FLY");

                entity.Property(e => e.D242Punkt112iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_HEL");

                entity.Property(e => e.D242Punkt112iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_2I_PR");

                entity.Property(e => e.D242Punkt11A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_A");

                entity.Property(e => e.D242Punkt11B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_B");

                entity.Property(e => e.D242Punkt11C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_C");

                entity.Property(e => e.D242Punkt11D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_D");

                entity.Property(e => e.D242Punkt11Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_FLY");

                entity.Property(e => e.D242Punkt11Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_HEL");

                entity.Property(e => e.D242Punkt11Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1_PR");

                entity.Property(e => e.D242Punkt11iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_A");

                entity.Property(e => e.D242Punkt11iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_B");

                entity.Property(e => e.D242Punkt11iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_C");

                entity.Property(e => e.D242Punkt11iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_D");

                entity.Property(e => e.D242Punkt11iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_FLY");

                entity.Property(e => e.D242Punkt11iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_HEL");

                entity.Property(e => e.D242Punkt11iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_1I_PR");

                entity.Property(e => e.D242Punkt12A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_A");

                entity.Property(e => e.D242Punkt12B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_B");

                entity.Property(e => e.D242Punkt12C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_C");

                entity.Property(e => e.D242Punkt12D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_D");

                entity.Property(e => e.D242Punkt12Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_FLY");

                entity.Property(e => e.D242Punkt12Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_HEL");

                entity.Property(e => e.D242Punkt12Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2_PR");

                entity.Property(e => e.D242Punkt12iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_A");

                entity.Property(e => e.D242Punkt12iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_B");

                entity.Property(e => e.D242Punkt12iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_C");

                entity.Property(e => e.D242Punkt12iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_D");

                entity.Property(e => e.D242Punkt12iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_FLY");

                entity.Property(e => e.D242Punkt12iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_HEL");

                entity.Property(e => e.D242Punkt12iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_2I_PR");

                entity.Property(e => e.D242Punkt13A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_A");

                entity.Property(e => e.D242Punkt13B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_B");

                entity.Property(e => e.D242Punkt13C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_C");

                entity.Property(e => e.D242Punkt13D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_D");

                entity.Property(e => e.D242Punkt13Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_FLY");

                entity.Property(e => e.D242Punkt13Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_HEL");

                entity.Property(e => e.D242Punkt13Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3_PR");

                entity.Property(e => e.D242Punkt13iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_A");

                entity.Property(e => e.D242Punkt13iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_B");

                entity.Property(e => e.D242Punkt13iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_C");

                entity.Property(e => e.D242Punkt13iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_D");

                entity.Property(e => e.D242Punkt13iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_FLY");

                entity.Property(e => e.D242Punkt13iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_HEL");

                entity.Property(e => e.D242Punkt13iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_3I_PR");

                entity.Property(e => e.D242Punkt14A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_A");

                entity.Property(e => e.D242Punkt14B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_B");

                entity.Property(e => e.D242Punkt14C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_C");

                entity.Property(e => e.D242Punkt14D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_D");

                entity.Property(e => e.D242Punkt14Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_FLY");

                entity.Property(e => e.D242Punkt14Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_HEL");

                entity.Property(e => e.D242Punkt14Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4_PR");

                entity.Property(e => e.D242Punkt14iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_A");

                entity.Property(e => e.D242Punkt14iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_B");

                entity.Property(e => e.D242Punkt14iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_C");

                entity.Property(e => e.D242Punkt14iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_D");

                entity.Property(e => e.D242Punkt14iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_FLY");

                entity.Property(e => e.D242Punkt14iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_HEL");

                entity.Property(e => e.D242Punkt14iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_4I_PR");

                entity.Property(e => e.D242Punkt15A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_A");

                entity.Property(e => e.D242Punkt15B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_B");

                entity.Property(e => e.D242Punkt15C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_C");

                entity.Property(e => e.D242Punkt15D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_D");

                entity.Property(e => e.D242Punkt15Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_FLY");

                entity.Property(e => e.D242Punkt15Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_HEL");

                entity.Property(e => e.D242Punkt15Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5_PR");

                entity.Property(e => e.D242Punkt15iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_A");

                entity.Property(e => e.D242Punkt15iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_B");

                entity.Property(e => e.D242Punkt15iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_C");

                entity.Property(e => e.D242Punkt15iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_D");

                entity.Property(e => e.D242Punkt15iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_FLY");

                entity.Property(e => e.D242Punkt15iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_HEL");

                entity.Property(e => e.D242Punkt15iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_5I_PR");

                entity.Property(e => e.D242Punkt16A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_A");

                entity.Property(e => e.D242Punkt16B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_B");

                entity.Property(e => e.D242Punkt16C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_C");

                entity.Property(e => e.D242Punkt16D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_D");

                entity.Property(e => e.D242Punkt16Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_FLY");

                entity.Property(e => e.D242Punkt16Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_HEL");

                entity.Property(e => e.D242Punkt16Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6_PR");

                entity.Property(e => e.D242Punkt16iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_A");

                entity.Property(e => e.D242Punkt16iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_B");

                entity.Property(e => e.D242Punkt16iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_C");

                entity.Property(e => e.D242Punkt16iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_D");

                entity.Property(e => e.D242Punkt16iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_FLY");

                entity.Property(e => e.D242Punkt16iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_HEL");

                entity.Property(e => e.D242Punkt16iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_6I_PR");

                entity.Property(e => e.D242Punkt1A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_A");

                entity.Property(e => e.D242Punkt1B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_B");

                entity.Property(e => e.D242Punkt1C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_C");

                entity.Property(e => e.D242Punkt1D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_D");

                entity.Property(e => e.D242Punkt1Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_FLY");

                entity.Property(e => e.D242Punkt1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_HEL");

                entity.Property(e => e.D242Punkt1Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1_PR");

                entity.Property(e => e.D242Punkt1aFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1A_FLY");

                entity.Property(e => e.D242Punkt1aHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1A_HEL");

                entity.Property(e => e.D242Punkt1aPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242_PUNKT1A_PR");

                entity.Property(e => e.D242Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D242_SAKSBEHANDLERE");

                entity.Property(e => e.D242bPunkt11Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242B_PUNKT1_1_FLY");

                entity.Property(e => e.D242bPunkt11Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242B_PUNKT1_1_HEL");

                entity.Property(e => e.D242bPunkt1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D242B_PUNKT1_HEL");
            });

            modelBuilder.Entity<FaStatTabell242G>(entity =>
            {
                entity.HasKey(e => new { e.G242PeriodeStartdato, e.G242PeriodeSluttdato, e.KliLoepenr, e.G242Distrikter, e.G242Punkt, e.G242Teller })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL242_G");

                entity.HasIndex(e => e.G242Distrikter, "IX_FA_STAT_TABELL242_G_DISTR");

                entity.Property(e => e.G242PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G242_PERIODE_STARTDATO");

                entity.Property(e => e.G242PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G242_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.G242Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G242_DISTRIKTER");

                entity.Property(e => e.G242Punkt)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("G242_PUNKT")
                    .IsFixedLength();

                entity.Property(e => e.G242Teller)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G242_TELLER");

                entity.Property(e => e.G242Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("G242_DISTRIKTSNAVN");

                entity.Property(e => e.G242Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G242_KLIENTGRUPPER");

                entity.Property(e => e.G242Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G242_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell242Ttperioder>(entity =>
            {
                entity.HasKey(e => new { e.P242PeriodeStartdato, e.P242PeriodeSluttdato, e.KliLoepenr, e.P242Distrikter, e.P242Loepenr })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL242_TTPERIODER");

                entity.HasIndex(e => e.P242Distrikter, "IX_FA_STAT_TABELL242_TTPER_DISTR");

                entity.Property(e => e.P242PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("P242_PERIODE_STARTDATO");

                entity.Property(e => e.P242PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("P242_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.P242Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("P242_DISTRIKTER");

                entity.Property(e => e.P242Loepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("P242_LOEPENR");

                entity.Property(e => e.LovHovedparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_HOVEDPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.P242AntallDoegn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("P242_ANTALL_DOEGN");

                entity.Property(e => e.P242Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("P242_DISTRIKTSNAVN");

                entity.Property(e => e.P242Fradato)
                    .HasColumnType("datetime")
                    .HasColumnName("P242_FRADATO");

                entity.Property(e => e.P242Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("P242_KLIENTGRUPPER");

                entity.Property(e => e.P242Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("P242_SAKSBEHANDLERE");

                entity.Property(e => e.P242Tildato)
                    .HasColumnType("datetime")
                    .HasColumnName("P242_TILDATO");

                entity.Property(e => e.TilKategori)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIL_KATEGORI")
                    .IsFixedLength();

                entity.Property(e => e.TilUtenforhjemmet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_UTENFORHJEMMET");

                entity.Property(e => e.TttSsbkode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TTT_SSBKODE")
                    .IsFixedLength();

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TTT_TILTAKSTYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaStatTabell242sum>(entity =>
            {
                entity.HasKey(e => new { e.S242PeriodeStartdato, e.S242PeriodeSluttdato, e.S242Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL242SUM");

                entity.HasIndex(e => e.S242Distrikter, "IX_FA_STAT_TABELL242SUM_DISTR");

                entity.Property(e => e.S242PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S242_PERIODE_STARTDATO");

                entity.Property(e => e.S242PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S242_PERIODE_SLUTTDATO");

                entity.Property(e => e.S242Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S242_DISTRIKTER");

                entity.Property(e => e.S242Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S242_DISTRIKTSNAVN");

                entity.Property(e => e.S242Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S242_KLIENTGRUPPER");

                entity.Property(e => e.S242Punkt111A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_A");

                entity.Property(e => e.S242Punkt111B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_B");

                entity.Property(e => e.S242Punkt111C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_C");

                entity.Property(e => e.S242Punkt111D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_D");

                entity.Property(e => e.S242Punkt111Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_DGN");

                entity.Property(e => e.S242Punkt111Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_FLY");

                entity.Property(e => e.S242Punkt111Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_HEL");

                entity.Property(e => e.S242Punkt111Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1_PR");

                entity.Property(e => e.S242Punkt111iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_A");

                entity.Property(e => e.S242Punkt111iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_B");

                entity.Property(e => e.S242Punkt111iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_C");

                entity.Property(e => e.S242Punkt111iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_D");

                entity.Property(e => e.S242Punkt111iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_FLY");

                entity.Property(e => e.S242Punkt111iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_HEL");

                entity.Property(e => e.S242Punkt111iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_1I_PR");

                entity.Property(e => e.S242Punkt112A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_A");

                entity.Property(e => e.S242Punkt112B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_B");

                entity.Property(e => e.S242Punkt112C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_C");

                entity.Property(e => e.S242Punkt112D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_D");

                entity.Property(e => e.S242Punkt112Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_DGN");

                entity.Property(e => e.S242Punkt112Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_FLY");

                entity.Property(e => e.S242Punkt112Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_HEL");

                entity.Property(e => e.S242Punkt112Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2_PR");

                entity.Property(e => e.S242Punkt112iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_A");

                entity.Property(e => e.S242Punkt112iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_B");

                entity.Property(e => e.S242Punkt112iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_C");

                entity.Property(e => e.S242Punkt112iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_D");

                entity.Property(e => e.S242Punkt112iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_FLY");

                entity.Property(e => e.S242Punkt112iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_HEL");

                entity.Property(e => e.S242Punkt112iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_2I_PR");

                entity.Property(e => e.S242Punkt11A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_A");

                entity.Property(e => e.S242Punkt11B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_B");

                entity.Property(e => e.S242Punkt11C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_C");

                entity.Property(e => e.S242Punkt11D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_D");

                entity.Property(e => e.S242Punkt11Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_FLY");

                entity.Property(e => e.S242Punkt11Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_HEL");

                entity.Property(e => e.S242Punkt11Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1_PR");

                entity.Property(e => e.S242Punkt11iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_A");

                entity.Property(e => e.S242Punkt11iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_B");

                entity.Property(e => e.S242Punkt11iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_C");

                entity.Property(e => e.S242Punkt11iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_D");

                entity.Property(e => e.S242Punkt11iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_FLY");

                entity.Property(e => e.S242Punkt11iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_HEL");

                entity.Property(e => e.S242Punkt11iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_1I_PR");

                entity.Property(e => e.S242Punkt12A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_A");

                entity.Property(e => e.S242Punkt12B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_B");

                entity.Property(e => e.S242Punkt12C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_C");

                entity.Property(e => e.S242Punkt12D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_D");

                entity.Property(e => e.S242Punkt12Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_DGN");

                entity.Property(e => e.S242Punkt12Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_FLY");

                entity.Property(e => e.S242Punkt12Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_HEL");

                entity.Property(e => e.S242Punkt12Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2_PR");

                entity.Property(e => e.S242Punkt12iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_A");

                entity.Property(e => e.S242Punkt12iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_B");

                entity.Property(e => e.S242Punkt12iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_C");

                entity.Property(e => e.S242Punkt12iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_D");

                entity.Property(e => e.S242Punkt12iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_FLY");

                entity.Property(e => e.S242Punkt12iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_HEL");

                entity.Property(e => e.S242Punkt12iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_2I_PR");

                entity.Property(e => e.S242Punkt13A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_A");

                entity.Property(e => e.S242Punkt13B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_B");

                entity.Property(e => e.S242Punkt13C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_C");

                entity.Property(e => e.S242Punkt13D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_D");

                entity.Property(e => e.S242Punkt13Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_DGN");

                entity.Property(e => e.S242Punkt13Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_FLY");

                entity.Property(e => e.S242Punkt13Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_HEL");

                entity.Property(e => e.S242Punkt13Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3_PR");

                entity.Property(e => e.S242Punkt13iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_A");

                entity.Property(e => e.S242Punkt13iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_B");

                entity.Property(e => e.S242Punkt13iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_C");

                entity.Property(e => e.S242Punkt13iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_D");

                entity.Property(e => e.S242Punkt13iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_FLY");

                entity.Property(e => e.S242Punkt13iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_HEL");

                entity.Property(e => e.S242Punkt13iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_3I_PR");

                entity.Property(e => e.S242Punkt14A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_A");

                entity.Property(e => e.S242Punkt14B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_B");

                entity.Property(e => e.S242Punkt14C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_C");

                entity.Property(e => e.S242Punkt14D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_D");

                entity.Property(e => e.S242Punkt14Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_DGN");

                entity.Property(e => e.S242Punkt14Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_FLY");

                entity.Property(e => e.S242Punkt14Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_HEL");

                entity.Property(e => e.S242Punkt14Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4_PR");

                entity.Property(e => e.S242Punkt14iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_A");

                entity.Property(e => e.S242Punkt14iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_B");

                entity.Property(e => e.S242Punkt14iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_C");

                entity.Property(e => e.S242Punkt14iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_D");

                entity.Property(e => e.S242Punkt14iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_FLY");

                entity.Property(e => e.S242Punkt14iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_HEL");

                entity.Property(e => e.S242Punkt14iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_4I_PR");

                entity.Property(e => e.S242Punkt15A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_A");

                entity.Property(e => e.S242Punkt15B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_B");

                entity.Property(e => e.S242Punkt15C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_C");

                entity.Property(e => e.S242Punkt15D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_D");

                entity.Property(e => e.S242Punkt15Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_DGN");

                entity.Property(e => e.S242Punkt15Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_FLY");

                entity.Property(e => e.S242Punkt15Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_HEL");

                entity.Property(e => e.S242Punkt15Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5_PR");

                entity.Property(e => e.S242Punkt15iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_A");

                entity.Property(e => e.S242Punkt15iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_B");

                entity.Property(e => e.S242Punkt15iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_C");

                entity.Property(e => e.S242Punkt15iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_D");

                entity.Property(e => e.S242Punkt15iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_FLY");

                entity.Property(e => e.S242Punkt15iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_HEL");

                entity.Property(e => e.S242Punkt15iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_5I_PR");

                entity.Property(e => e.S242Punkt16A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_A");

                entity.Property(e => e.S242Punkt16B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_B");

                entity.Property(e => e.S242Punkt16C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_C");

                entity.Property(e => e.S242Punkt16D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_D");

                entity.Property(e => e.S242Punkt16Dgn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_DGN");

                entity.Property(e => e.S242Punkt16Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_FLY");

                entity.Property(e => e.S242Punkt16Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_HEL");

                entity.Property(e => e.S242Punkt16Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6_PR");

                entity.Property(e => e.S242Punkt16iA)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_A");

                entity.Property(e => e.S242Punkt16iB)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_B");

                entity.Property(e => e.S242Punkt16iC)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_C");

                entity.Property(e => e.S242Punkt16iD)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_D");

                entity.Property(e => e.S242Punkt16iFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_FLY");

                entity.Property(e => e.S242Punkt16iHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_HEL");

                entity.Property(e => e.S242Punkt16iPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_6I_PR");

                entity.Property(e => e.S242Punkt1A)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_A");

                entity.Property(e => e.S242Punkt1B)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_B");

                entity.Property(e => e.S242Punkt1C)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_C");

                entity.Property(e => e.S242Punkt1D)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_D");

                entity.Property(e => e.S242Punkt1Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_FLY");

                entity.Property(e => e.S242Punkt1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_HEL");

                entity.Property(e => e.S242Punkt1Pr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1_PR");

                entity.Property(e => e.S242Punkt1aFly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1A_FLY");

                entity.Property(e => e.S242Punkt1aHel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1A_HEL");

                entity.Property(e => e.S242Punkt1aPr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242_PUNKT1A_PR");

                entity.Property(e => e.S242Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S242_SAKSBEHANDLERE");

                entity.Property(e => e.S242bPunkt11Fly)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242B_PUNKT1_1_FLY");

                entity.Property(e => e.S242bPunkt11Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242B_PUNKT1_1_HEL");

                entity.Property(e => e.S242bPunkt1Hel)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S242B_PUNKT1_HEL");
            });

            modelBuilder.Entity<FaStatTabell243>(entity =>
            {
                entity.HasKey(e => new { e.D243PeriodeStartdato, e.D243PeriodeSluttdato, e.KliLoepenr, e.D243Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL243");

                entity.HasIndex(e => e.D243Distrikter, "IX_FA_STAT_TABELL243_DISTR");

                entity.Property(e => e.D243PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D243_PERIODE_STARTDATO");

                entity.Property(e => e.D243PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D243_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D243Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D243_DISTRIKTER");

                entity.Property(e => e.D243AntOppfoelgbesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_ANT_OPPFOELGBESOEK");

                entity.Property(e => e.D243Antbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D243_ANTBESOEK");

                entity.Property(e => e.D243AntmndFh)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_ANTMND_FH");

                entity.Property(e => e.D243Dato18aar)
                    .HasColumnType("datetime")
                    .HasColumnName("D243_DATO18AAR");

                entity.Property(e => e.D243Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D243_DISTRIKTSNAVN");

                entity.Property(e => e.D243Fh12Sum)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_FH12_SUM");

                entity.Property(e => e.D243Fh1U18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_FH1_U18");

                entity.Property(e => e.D243Fh2O18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_FH2_O18");

                entity.Property(e => e.D243Fh3)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_FH3");

                entity.Property(e => e.D243Fh4)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_FH4");

                entity.Property(e => e.D243Fh5Ubesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D243_FH5_UBESOEK");

                entity.Property(e => e.D243Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D243_KLIENTGRUPPER");

                entity.Property(e => e.D243KravOppfoelgbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D243_KRAV_OPPFOELGBESOEK");

                entity.Property(e => e.D243KravTilsynsbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D243_KRAV_TILSYNSBESOEK");

                entity.Property(e => e.D243Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D243_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell243G>(entity =>
            {
                entity.HasKey(e => new { e.G243PeriodeStartdato, e.G243PeriodeSluttdato, e.KliLoepenr, e.G243Distrikter, e.G243Punkt, e.G243Teller })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL243_G");

                entity.HasIndex(e => e.G243Distrikter, "IX_FA_STAT_TABELL243_G_DISTR");

                entity.Property(e => e.G243PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G243_PERIODE_STARTDATO");

                entity.Property(e => e.G243PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G243_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.G243Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G243_DISTRIKTER");

                entity.Property(e => e.G243Punkt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("G243_PUNKT")
                    .IsFixedLength();

                entity.Property(e => e.G243Teller)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G243_TELLER");

                entity.Property(e => e.G243Antbesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G243_ANTBESOEK");

                entity.Property(e => e.G243AntmndFh)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G243_ANTMND_FH");

                entity.Property(e => e.G243Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("G243_DISTRIKTSNAVN");

                entity.Property(e => e.G243Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G243_KLIENTGRUPPER");

                entity.Property(e => e.G243Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G243_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell243sum>(entity =>
            {
                entity.HasKey(e => new { e.S243PeriodeStartdato, e.S243PeriodeSluttdato, e.S243Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL243SUM");

                entity.HasIndex(e => e.S243Distrikter, "IX_FA_STAT_TABELL243SUM_DISTR");

                entity.Property(e => e.S243PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S243_PERIODE_STARTDATO");

                entity.Property(e => e.S243PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S243_PERIODE_SLUTTDATO");

                entity.Property(e => e.S243Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S243_DISTRIKTER");

                entity.Property(e => e.S243Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S243_DISTRIKTSNAVN");

                entity.Property(e => e.S243Fh12Sum)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S243_FH12_SUM");

                entity.Property(e => e.S243Fh1U18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S243_FH1_U18");

                entity.Property(e => e.S243Fh2O18)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S243_FH2_O18");

                entity.Property(e => e.S243Fh3)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S243_FH3");

                entity.Property(e => e.S243Fh4)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S243_FH4");

                entity.Property(e => e.S243Fh5Ubesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S243_FH5_UBESOEK");

                entity.Property(e => e.S243Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S243_KLIENTGRUPPER");

                entity.Property(e => e.S243Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S243_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell25>(entity =>
            {
                entity.HasKey(e => new { e.D25PeriodeStartdato, e.D25PeriodeSluttdato, e.KliLoepenr, e.D25Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL25");

                entity.HasIndex(e => e.D25Distrikter, "IX_FA_STAT_TABELL25_DISTR");

                entity.Property(e => e.D25PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D25_PERIODE_STARTDATO");

                entity.Property(e => e.D25PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D25_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D25Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D25_DISTRIKTER");

                entity.Property(e => e.D25AntOppfoelgbesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D25_ANT_OPPFOELGBESOEK");

                entity.Property(e => e.D25Antbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_ANTBESOEK");

                entity.Property(e => e.D25AntmndFh)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_ANTMND_FH");

                entity.Property(e => e.D25Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D25_DISTRIKTSNAVN");

                entity.Property(e => e.D25Egenbydel)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_EGENBYDEL");

                entity.Property(e => e.D25Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D25_KLIENTGRUPPER");

                entity.Property(e => e.D25KravOppfoelgbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_KRAV_OPPFOELGBESOEK");

                entity.Property(e => e.D25KravTilsynsbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_KRAV_TILSYNSBESOEK");

                entity.Property(e => e.D25Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D25_SAKSBEHANDLERE");

                entity.Property(e => e.D25StatistikkOppfoelgbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_STATISTIKK_OPPFOELGBESOEK");

                entity.Property(e => e.D25Statistikkbesoek)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("D25_STATISTIKKBESOEK");
            });

            modelBuilder.Entity<FaStatTabell25sum>(entity =>
            {
                entity.HasKey(e => new { e.S25PeriodeStartdato, e.S25PeriodeSluttdato, e.S25Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL25SUM");

                entity.HasIndex(e => e.S25Distrikter, "IX_FA_STAT_TABELL25SUM_DISTR");

                entity.Property(e => e.S25PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S25_PERIODE_STARTDATO");

                entity.Property(e => e.S25PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S25_PERIODE_SLUTTDATO");

                entity.Property(e => e.S25Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S25_DISTRIKTER");

                entity.Property(e => e.S25Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S25_DISTRIKTSNAVN");

                entity.Property(e => e.S25Fh1PlassertEgen)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S25_FH1_PLASSERT_EGEN");

                entity.Property(e => e.S25Fh1SnittBesoek)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("S25_FH1_SNITT_BESOEK");

                entity.Property(e => e.S25Fh1SumBesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S25_FH1_SUM_BESOEK");

                entity.Property(e => e.S25Fh2PlassertAndre)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S25_FH2_PLASSERT_ANDRE");

                entity.Property(e => e.S25Fh2SnittBesoek)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("S25_FH2_SNITT_BESOEK");

                entity.Property(e => e.S25Fh2SumBesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S25_FH2_SUM_BESOEK");

                entity.Property(e => e.S25Fh3SnittOppfoelgbesoek)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("S25_FH3_SNITT_OPPFOELGBESOEK");

                entity.Property(e => e.S25Fh3SumOppfoelgbesoek)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S25_FH3_SUM_OPPFOELGBESOEK");

                entity.Property(e => e.S25Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S25_KLIENTGRUPPER");

                entity.Property(e => e.S25Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S25_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell26>(entity =>
            {
                entity.HasKey(e => new { e.D26PeriodeStartdato, e.D26PeriodeSluttdato, e.KliLoepenr, e.D26Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL26");

                entity.HasIndex(e => e.D26Distrikter, "IX_FA_STAT_TABELL26_DISTR");

                entity.Property(e => e.D26PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D26_PERIODE_STARTDATO");

                entity.Property(e => e.D26PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("D26_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.D26Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D26_DISTRIKTER");

                entity.Property(e => e.D26Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("D26_DISTRIKTSNAVN");

                entity.Property(e => e.D26Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D26_KLIENTGRUPPER");

                entity.Property(e => e.D26Punkt11fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_1FN");

                entity.Property(e => e.D26Punkt11lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_1LR");

                entity.Property(e => e.D26Punkt11tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_1TR");

                entity.Property(e => e.D26Punkt12fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_2FN");

                entity.Property(e => e.D26Punkt12lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_2LR");

                entity.Property(e => e.D26Punkt12tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_2TR");

                entity.Property(e => e.D26Punkt13fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_3FN");

                entity.Property(e => e.D26Punkt13lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_3LR");

                entity.Property(e => e.D26Punkt13tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_3TR");

                entity.Property(e => e.D26Punkt14fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_4FN");

                entity.Property(e => e.D26Punkt14lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_4LR");

                entity.Property(e => e.D26Punkt14tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_4TR");

                entity.Property(e => e.D26Punkt15fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_5FN");

                entity.Property(e => e.D26Punkt15lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_5LR");

                entity.Property(e => e.D26Punkt15tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_5TR");

                entity.Property(e => e.D26Punkt1Fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_FN");

                entity.Property(e => e.D26Punkt1Lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_LR");

                entity.Property(e => e.D26Punkt1Sumfn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_SUMFN");

                entity.Property(e => e.D26Punkt1Sumlr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_SUMLR");

                entity.Property(e => e.D26Punkt1Sumtr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_SUMTR");

                entity.Property(e => e.D26Punkt1Tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT1_TR");

                entity.Property(e => e.D26Punkt2Fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT2_FN");

                entity.Property(e => e.D26Punkt2Lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT2_LR");

                entity.Property(e => e.D26Punkt2Tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("D26_PUNKT2_TR");

                entity.Property(e => e.D26Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("D26_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell26G>(entity =>
            {
                entity.HasKey(e => new { e.G26PeriodeStartdato, e.G26PeriodeSluttdato, e.KliLoepenr, e.G26Distrikter, e.G26Punkt, e.G26Teller })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL26_G");

                entity.HasIndex(e => e.G26Distrikter, "IX_FA_STAT_TABELL26_G_DISTR");

                entity.Property(e => e.G26PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G26_PERIODE_STARTDATO");

                entity.Property(e => e.G26PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("G26_PERIODE_SLUTTDATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.G26Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G26_DISTRIKTER");

                entity.Property(e => e.G26Punkt)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("G26_PUNKT")
                    .IsFixedLength();

                entity.Property(e => e.G26Teller)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("G26_TELLER");

                entity.Property(e => e.G26Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("G26_DISTRIKTSNAVN");

                entity.Property(e => e.G26Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G26_KLIENTGRUPPER");

                entity.Property(e => e.G26Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("G26_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatTabell26sum>(entity =>
            {
                entity.HasKey(e => new { e.S26PeriodeStartdato, e.S26PeriodeSluttdato, e.S26Distrikter })
                    .IsClustered(false);

                entity.ToTable("FA_STAT_TABELL26SUM");

                entity.HasIndex(e => e.S26Distrikter, "IX_FA_STAT_TABELL26SUM_DISTR");

                entity.Property(e => e.S26PeriodeStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S26_PERIODE_STARTDATO");

                entity.Property(e => e.S26PeriodeSluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("S26_PERIODE_SLUTTDATO");

                entity.Property(e => e.S26Distrikter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S26_DISTRIKTER");

                entity.Property(e => e.S26Distriktsnavn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("S26_DISTRIKTSNAVN");

                entity.Property(e => e.S26Klientgrupper)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S26_KLIENTGRUPPER");

                entity.Property(e => e.S26Punkt11fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_1FN");

                entity.Property(e => e.S26Punkt11lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_1LR");

                entity.Property(e => e.S26Punkt11tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_1TR");

                entity.Property(e => e.S26Punkt12fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_2FN");

                entity.Property(e => e.S26Punkt12lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_2LR");

                entity.Property(e => e.S26Punkt12tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_2TR");

                entity.Property(e => e.S26Punkt13fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_3FN");

                entity.Property(e => e.S26Punkt13lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_3LR");

                entity.Property(e => e.S26Punkt13tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_3TR");

                entity.Property(e => e.S26Punkt14fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_4FN");

                entity.Property(e => e.S26Punkt14lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_4LR");

                entity.Property(e => e.S26Punkt14tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_4TR");

                entity.Property(e => e.S26Punkt15fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_5FN");

                entity.Property(e => e.S26Punkt15lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_5LR");

                entity.Property(e => e.S26Punkt15tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_5TR");

                entity.Property(e => e.S26Punkt1Fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_FN");

                entity.Property(e => e.S26Punkt1Lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_LR");

                entity.Property(e => e.S26Punkt1Sumfn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_SUMFN");

                entity.Property(e => e.S26Punkt1Sumlr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_SUMLR");

                entity.Property(e => e.S26Punkt1Sumtr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_SUMTR");

                entity.Property(e => e.S26Punkt1Tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT1_TR");

                entity.Property(e => e.S26Punkt2Fn)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT2_FN");

                entity.Property(e => e.S26Punkt2Lr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT2_LR");

                entity.Property(e => e.S26Punkt2Tr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("S26_PUNKT2_TR");

                entity.Property(e => e.S26Saksbehandlere)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("S26_SAKSBEHANDLERE");
            });

            modelBuilder.Entity<FaStatsborgerskapkonto>(entity =>
            {
                entity.HasKey(e => e.SbkIdent)
                    .IsClustered(false);

                entity.ToTable("FA_STATSBORGERSKAPKONTO");

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonummer }, "FK_FA_STATSBORGERSKAPKONTO1")
                    .HasFillFactor(80);

                entity.Property(e => e.SbkIdent)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SBK_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaStatsborgerskapkontos)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonummer })
                    .HasConstraintName("FK_FA_STATS_KONTOER_S_FA_KONTO");
            });

            modelBuilder.Entity<FaSvarinnLogg>(entity =>
            {
                entity.HasKey(e => e.SvinId)
                    .HasName("PK_FA_SVARINN")
                    .IsClustered(false);

                entity.ToTable("FA_SVARINN_LOGG");

                entity.Property(e => e.SvinId)
                    .ValueGeneratedNever()
                    .HasColumnName("SVIN_ID");

                entity.Property(e => e.SvinError)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("SVIN_ERROR");
            });

            modelBuilder.Entity<FaTekstmaler>(entity =>
            {
                entity.HasKey(e => new { e.TmaMaltype, e.TmaNavn, e.TmaCkeditor })
                    .IsClustered(false);

                entity.ToTable("FA_TEKSTMALER");

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_TEKSTMALER1")
                    .HasFillFactor(80);

                entity.Property(e => e.TmaMaltype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TMA_MALTYPE")
                    .IsFixedLength();

                entity.Property(e => e.TmaNavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TMA_NAVN");

                entity.Property(e => e.TmaCkeditor)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TMA_CKEDITOR");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.TmaBasedontemplateid).HasColumnName("TMA_BASEDONTEMPLATEID");

                entity.Property(e => e.TmaBasetype).HasColumnName("TMA_BASETYPE");

                entity.Property(e => e.TmaBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TMA_BESKRIVELSE");

                entity.Property(e => e.TmaFooterpagesetting).HasColumnName("TMA_FOOTERPAGESETTING");

                entity.Property(e => e.TmaFootertemplateid).HasColumnName("TMA_FOOTERTEMPLATEID");

                entity.Property(e => e.TmaGruppe)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TMA_GRUPPE")
                    .IsFixedLength();

                entity.Property(e => e.TmaHeaderpagesetting).HasColumnName("TMA_HEADERPAGESETTING");

                entity.Property(e => e.TmaHeadertemplateid).HasColumnName("TMA_HEADERTEMPLATEID");

                entity.Property(e => e.TmaId).HasColumnName("TMA_ID");

                entity.Property(e => e.TmaPassivisertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("TMA_PASSIVISERTAV");

                entity.Property(e => e.TmaPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TMA_PASSIVISERTDATO");

                entity.Property(e => e.TmaSecondfooteremplateid).HasColumnName("TMA_SECONDFOOTEREMPLATEID");

                entity.Property(e => e.TmaSecondfooterpagesetting).HasColumnName("TMA_SECONDFOOTERPAGESETTING");

                entity.Property(e => e.TmaSecondheaderpagesetting).HasColumnName("TMA_SECONDHEADERPAGESETTING");

                entity.Property(e => e.TmaSecondheadertemplateid).HasColumnName("TMA_SECONDHEADERTEMPLATEID");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaTekstmalers)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_TEKST_DOKUMENT__FA_DOKUM");
            });

            modelBuilder.Entity<FaTekstmaltyper>(entity =>
            {
                entity.HasKey(e => new { e.TmtGruppe, e.TmtType })
                    .IsClustered(false);

                entity.ToTable("FA_TEKSTMALTYPER");

                entity.Property(e => e.TmtGruppe)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TMT_GRUPPE")
                    .IsFixedLength();

                entity.Property(e => e.TmtType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TMT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.TmtGruppebeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TMT_GRUPPEBESKRIVELSE");

                entity.Property(e => e.TmtPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TMT_PASSIVISERTDATO");

                entity.Property(e => e.TmtTypebeskrivelse)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("TMT_TYPEBESKRIVELSE");
            });

            modelBuilder.Entity<FaTeller>(entity =>
            {
                entity.HasKey(e => e.TelTeller)
                    .IsClustered(false);

                entity.ToTable("FA_TELLER");

                entity.HasIndex(e => e.DisDistriktskode, "FK_FA_TELLER")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.TelType, e.DisDistriktskode }, "IX_FA_TELLER2")
                    .IsUnique()
                    .HasFillFactor(80);

                entity.Property(e => e.TelTeller)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("TEL_TELLER");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.TelBeskrivelse)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("TEL_BESKRIVELSE");

                entity.Property(e => e.TelSistbruktenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TEL_SISTBRUKTENR");

                entity.Property(e => e.TelType)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("TEL_TYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.DisDistriktskodeNavigation)
                    .WithMany(p => p.FaTellers)
                    .HasForeignKey(d => d.DisDistriktskode)
                    .HasConstraintName("FK_FA_TELLER_DISTRIKT");
            });

            modelBuilder.Entity<FaTilgangsgrVindu>(entity =>
            {
                entity.HasKey(e => new { e.VinUtvnavn, e.TggIdent })
                    .IsClustered(false);

                entity.ToTable("FA_TILGANGSGR_VINDU");

                entity.HasIndex(e => e.TggIdent, "FK_FA_TILGANGSGR_VINDU1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.VinUtvnavn, "FK_FA_TILGANGSGR_VINDU2")
                    .HasFillFactor(80);

                entity.Property(e => e.VinUtvnavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VIN_UTVNAVN");

                entity.Property(e => e.TggIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TGG_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.TgvRettighet)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TGV_RETTIGHET")
                    .IsFixedLength();

                entity.HasOne(d => d.TggIdentNavigation)
                    .WithMany(p => p.FaTilgangsgrVindus)
                    .HasForeignKey(d => d.TggIdent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILGA_TILGANGS_FA_TILG1");

                entity.HasOne(d => d.VinUtvnavnNavigation)
                    .WithMany(p => p.FaTilgangsgrVindus)
                    .HasForeignKey(d => d.VinUtvnavn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILGA_TILGANGS_FA_VIND2");
            });

            modelBuilder.Entity<FaTilgangsgrupper>(entity =>
            {
                entity.HasKey(e => e.TggIdent)
                    .IsClustered(false);

                entity.ToTable("FA_TILGANGSGRUPPER");

                entity.Property(e => e.TggIdent)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TGG_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.TggAktiverAvvikslogg)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TGG_AKTIVER_AVVIKSLOGG");

                entity.Property(e => e.TggBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TGG_BESKRIVELSE");

                entity.HasMany(d => d.SbhInitialers)
                    .WithMany(p => p.TggIdents)
                    .UsingEntity<Dictionary<string, object>>(
                        "FaTilgangsgrSaksbeh",
                        l => l.HasOne<FaSaksbehandlere>().WithMany().HasForeignKey("SbhInitialer").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_TILGA_TILGANGSG_FA_SAKSB"),
                        r => r.HasOne<FaTilgangsgrupper>().WithMany().HasForeignKey("TggIdent").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FA_TILGA_TILGANGSG_FA_TILGA"),
                        j =>
                        {
                            j.HasKey("TggIdent", "SbhInitialer").IsClustered(false);

                            j.ToTable("FA_TILGANGSGR_SAKSBEH");

                            j.HasIndex(new[] { "TggIdent" }, "FK_TILGANGGR_SAKSBEH1").HasFillFactor(80);

                            j.HasIndex(new[] { "SbhInitialer" }, "FK_TILGANGGR_SAKSBEH2");

                            j.IndexerProperty<string>("TggIdent").HasMaxLength(3).IsUnicode(false).HasColumnName("TGG_IDENT").IsFixedLength();

                            j.IndexerProperty<string>("SbhInitialer").HasMaxLength(8).IsUnicode(false).HasColumnName("SBH_INITIALER");
                        });
            });

            modelBuilder.Entity<FaTilstand>(entity =>
            {
                entity.HasKey(e => e.TsaLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_TILSTAND");

                entity.Property(e => e.TsaLoepenr)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("TSA_LOEPENR");

                entity.Property(e => e.TsaEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TSA_ENDRETDATO");

                entity.Property(e => e.TsaKliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TSA_KLI_LOEPENR");

                entity.Property(e => e.TsaSbhInitialer)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TSA_SBH_INITIALER")
                    .IsFixedLength();

                entity.Property(e => e.TsaTilstand)
                    .HasColumnType("ntext")
                    .HasColumnName("TSA_TILSTAND");

                entity.Property(e => e.TsaType)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TSA_TYPE")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<FaTiltak>(entity =>
            {
                entity.HasKey(e => e.TilLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAK");

                entity.HasIndex(e => e.TttTiltakstype, "FK_FA_TILTAK1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer2, "FK_FA_TILTAK10");

                entity.HasIndex(e => new { e.FroType, e.FroKode1 }, "FK_FA_TILTAK11")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode2 }, "FK_FA_TILTAK12")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode3 }, "FK_FA_TILTAK13")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_TILTAK14");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_TILTAK15");

                entity.HasIndex(e => e.KttTiltakskode, "FK_FA_TILTAK16")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.TttTiltakstype, e.FttFriTiltakstype }, "FK_FA_TILTAK17")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_TILTAK2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.TtpLoepenr, "FK_FA_TILTAK3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_TILTAK4");

                entity.HasIndex(e => new { e.SakAar, e.SakJournalnr }, "FK_FA_TILTAK5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_TILTAK6")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LovHovedParagraf, "FK_FA_TILTAK7")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LovJmfParagraf1, "FK_FA_TILTAK8")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.LovJmfParagraf2, "FK_FA_TILTAK9")
                    .HasFillFactor(80);

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.FroKode1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE1")
                    .IsFixedLength();

                entity.Property(e => e.FroKode2)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE2")
                    .IsFixedLength();

                entity.Property(e => e.FroKode3)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE3")
                    .IsFixedLength();

                entity.Property(e => e.FroType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FRO_TYPE")
                    .HasDefaultValueSql("('T')")
                    .IsFixedLength();

                entity.Property(e => e.FttFriTiltakstype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FTT_FRI_TILTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KttTiltakskode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTT_TILTAKSKODE")
                    .IsFixedLength();

                entity.Property(e => e.LovHovedParagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_hovedPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfParagraf1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_jmfPARAGRAF1")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfParagraf2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_jmfPARAGRAF2")
                    .IsFixedLength();

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhInitialer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer2");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.Property(e => e.TilAvsluttetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_AVSLUTTETDATO");

                entity.Property(e => e.TilBortfalt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_BORTFALT");

                entity.Property(e => e.TilBortfaltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_BORTFALTDATO");

                entity.Property(e => e.TilDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_DOKUMENTNR");

                entity.Property(e => e.TilEgenbetvurdert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_EGENBETVURDERT");

                entity.Property(e => e.TilEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_ENDRETDATO");

                entity.Property(e => e.TilEttervern)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIL_ETTERVERN")
                    .IsFixedLength();

                entity.Property(e => e.TilFlyttet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_FLYTTET");

                entity.Property(e => e.TilFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_FRADATO");

                entity.Property(e => e.TilFratattforeldreansvar)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_FRATATTFORELDREANSVAR");

                entity.Property(e => e.TilGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIL_GMLREFERANSE");

                entity.Property(e => e.TilHovedgrunnavsluttet)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIL_HOVEDGRUNNAVSLUTTET")
                    .IsFixedLength();

                entity.Property(e => e.TilHovedtiltak)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_HOVEDTILTAK");

                entity.Property(e => e.TilHuskeliste)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_HUSKELISTE");

                entity.Property(e => e.TilIverksattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_IVERKSATTDATO");

                entity.Property(e => e.TilKategori)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TIL_KATEGORI")
                    .IsFixedLength();

                entity.Property(e => e.TilKommentar)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("TIL_KOMMENTAR");

                entity.Property(e => e.TilMerknadfristoversittelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("TIL_MERKNADFRISTOVERSITTELSE");

                entity.Property(e => e.TilOmsorgsovertakelse)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_OMSORGSOVERTAKELSE");

                entity.Property(e => e.TilPresisering)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIL_PRESISERING");

                entity.Property(e => e.TilRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_REGISTRERTDATO");

                entity.Property(e => e.TilTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_TILDATO");

                entity.Property(e => e.TilTiltaksmaal)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("TIL_TILTAKSMAAL");

                entity.Property(e => e.TilTiltakstypePres)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIL_TILTAKSTYPE_PRES");

                entity.Property(e => e.TilUtenforhjemmet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_UTENFORHJEMMET");

                entity.Property(e => e.TilUtilsiktet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TIL_UTILSIKTET");

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TTT_TILTAKSTYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaTiltaks)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_TILTA_DOKUMENT__FA_DOKUM");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaTiltaks)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILTA_KLIENT_TI_FA_KLIEN");

                entity.HasOne(d => d.KttTiltakskodeNavigation)
                    .WithMany(p => p.FaTiltaks)
                    .HasForeignKey(d => d.KttTiltakskode)
                    .HasConstraintName("FK_FA_TILTA_KTILTAKST_FA_KONTO");

                entity.HasOne(d => d.LovHovedParagrafNavigation)
                    .WithMany(p => p.FaTiltakLovHovedParagrafNavigations)
                    .HasForeignKey(d => d.LovHovedParagraf)
                    .HasConstraintName("FK_FA_TILTA_LOVTEKST__FA_LOVTE");

                entity.HasOne(d => d.LovJmfParagraf1Navigation)
                    .WithMany(p => p.FaTiltakLovJmfParagraf1Navigations)
                    .HasForeignKey(d => d.LovJmfParagraf1)
                    .HasConstraintName("FK_FA_TILTA_LOVTEKST__FA_LOVT3");

                entity.HasOne(d => d.LovJmfParagraf2Navigation)
                    .WithMany(p => p.FaTiltakLovJmfParagraf2Navigations)
                    .HasForeignKey(d => d.LovJmfParagraf2)
                    .HasConstraintName("FK_FA_TILTA_LOVTEKST__FA_LOVT2");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaTiltakSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS8");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaTiltakSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKSB");

                entity.HasOne(d => d.SbhInitialer2Navigation)
                    .WithMany(p => p.FaTiltakSbhInitialer2Navigations)
                    .HasForeignKey(d => d.SbhInitialer2)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS6");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaTiltakSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS7");

                entity.HasOne(d => d.TtpLoepenrNavigation)
                    .WithMany(p => p.FaTiltaks)
                    .HasForeignKey(d => d.TtpLoepenr)
                    .HasConstraintName("FK_FA_TILTA_TILTAKSPL_FA_TILTA");

                entity.HasOne(d => d.TttTiltakstypeNavigation)
                    .WithMany(p => p.FaTiltaks)
                    .HasForeignKey(d => d.TttTiltakstype)
                    .HasConstraintName("FK_FA_TILTA_TILTAKSTY_FA_TILTA");

                entity.HasOne(d => d.Fro)
                    .WithMany(p => p.FaTiltakFros)
                    .HasForeignKey(d => new { d.FroType, d.FroKode1 })
                    .HasConstraintName("FK_FA_TILTA_FRISTOVER_FA_FRIS3");

                entity.HasOne(d => d.FroNavigation)
                    .WithMany(p => p.FaTiltakFroNavigations)
                    .HasForeignKey(d => new { d.FroType, d.FroKode2 })
                    .HasConstraintName("FK_FA_TILTA_FRISTOVER_FA_FRIS2");

                entity.HasOne(d => d.Fro1)
                    .WithMany(p => p.FaTiltakFro1s)
                    .HasForeignKey(d => new { d.FroType, d.FroKode3 })
                    .HasConstraintName("FK_FA_TILTA_FRISTOVER_FA_FRIST");

                entity.HasOne(d => d.Sak)
                    .WithMany(p => p.FaTiltaks)
                    .HasForeignKey(d => new { d.SakAar, d.SakJournalnr })
                    .HasConstraintName("FK_FA_TILTA_SAK_TILTA_FA_SAKSJ");
            });

            modelBuilder.Entity<FaTiltakGmltype>(entity =>
            {
                entity.HasKey(e => e.TilLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAK_GMLTYPE");

                entity.HasIndex(e => e.TgtTiltakstype, "FK_FA_TILTAK_GMLTYPE1")
                    .HasFillFactor(80);

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.TgtBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("TGT_BESKRIVELSE");

                entity.Property(e => e.TgtTiltakstype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TGT_TILTAKSTYPE")
                    .IsFixedLength();

                entity.HasOne(d => d.TilLoepenrNavigation)
                    .WithOne(p => p.FaTiltakGmltype)
                    .HasForeignKey<FaTiltakGmltype>(d => d.TilLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TILTAK_GMLTYPE_TILTAK");
            });

            modelBuilder.Entity<FaTiltakUiKonto>(entity =>
            {
                entity.HasKey(e => e.UikIdent)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAK_UI_KONTO");

                entity.HasIndex(e => new { e.KtpNoekkel, e.KtnKontonummer }, "FK_FA_TILTAK_UI_KONTO1")
                    .HasFillFactor(80);

                entity.Property(e => e.UikIdent)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UIK_IDENT")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("ktn_kontonummer")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL")
                    .IsFixedLength();

                entity.HasOne(d => d.Kt)
                    .WithMany(p => p.FaTiltakUiKontos)
                    .HasForeignKey(d => new { d.KtpNoekkel, d.KtnKontonummer })
                    .HasConstraintName("FK_FA_TILTA_KONTOER_T_FA_KONTO");
            });

            modelBuilder.Entity<FaTiltaksevaluering>(entity =>
            {
                entity.HasKey(e => e.TevLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAKSEVALUERING");

                entity.HasIndex(e => e.TilLoepenr, "FK_FA_TILTAKSEVALUERING1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_TILTAKSEVALUERING2");

                entity.Property(e => e.TevLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TEV_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.TevEvalueringskode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TEV_EVALUERINGSKODE")
                    .IsFixedLength();

                entity.Property(e => e.TevFraklokken)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("TEV_FRAKLOKKEN");

                entity.Property(e => e.TevKommentar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TEV_KOMMENTAR");

                entity.Property(e => e.TevPlanlagtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TEV_PLANLAGTDATO");

                entity.Property(e => e.TevSted)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("TEV_STED");

                entity.Property(e => e.TevTilklokken)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("TEV_TILKLOKKEN");

                entity.Property(e => e.TevTilstede)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("TEV_TILSTEDE");

                entity.Property(e => e.TevUtfoertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TEV_UTFOERTDATO");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaTiltaksevaluerings)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS5");

                entity.HasOne(d => d.TilLoepenrNavigation)
                    .WithMany(p => p.FaTiltaksevaluerings)
                    .HasForeignKey(d => d.TilLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILTA_TILTAK_TI_FA_TILT2");
            });

            modelBuilder.Entity<FaTiltakslinjer>(entity =>
            {
                entity.HasKey(e => e.TtlLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAKSLINJER");

                entity.HasIndex(e => e.TilLoepenr, "FK_FA_TILTAKSLINJER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_TILTAKSLINJER2");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_TILTAKSLINJER3")
                    .HasFillFactor(80);

                entity.Property(e => e.TtlLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTL_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.TtlBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("TTL_BESKRIVELSE");

                entity.Property(e => e.TtlEksternansvarlig)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TTL_EKSTERNANSVARLIG");

                entity.Property(e => e.TtlFrist)
                    .HasColumnType("datetime")
                    .HasColumnName("TTL_FRIST");

                entity.Property(e => e.TtlInternt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TTL_INTERNT");

                entity.Property(e => e.TtlOppfyltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTL_OPPFYLTDATO");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaTiltakslinjers)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILTA_KLIENT_TI_FA_KLIE3");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaTiltakslinjers)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS4");

                entity.HasOne(d => d.TilLoepenrNavigation)
                    .WithMany(p => p.FaTiltakslinjers)
                    .HasForeignKey(d => d.TilLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILTA_TILTAK_TI_FA_TILTA");
            });

            modelBuilder.Entity<FaTiltaksplan>(entity =>
            {
                entity.HasKey(e => e.TtpLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAKSPLAN");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_TILTAKSPLAN1");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_TILTAKSPLAN2");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_TILTAKSPLAN3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_TILTAKSPLAN4")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.PtyPlankode, "FK_FA_TILTAKSPLAN5")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_TILTAKSPLAN6");

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");

                entity.Property(e => e.ArkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DATO");

                entity.Property(e => e.ArkSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_SJEKK_I_VSA");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.ArkTiltakSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_TILTAK_SYSTEMID");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PtyPlankode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANKODE")
                    .IsFixedLength();

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.Property(e => e.TtpAvsluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTP_AVSLUTTDATO");

                entity.Property(e => e.TtpBegrSlettet)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("TTP_BEGR_SLETTET");

                entity.Property(e => e.TtpDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_DOKUMENTNR");

                entity.Property(e => e.TtpEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTP_ENDRETDATO");

                entity.Property(e => e.TtpEttdokument)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TTP_ETTDOKUMENT")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TtpFerdigdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTP_FERDIGDATO");

                entity.Property(e => e.TtpFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTP_FRADATO");

                entity.Property(e => e.TtpHovedmaal)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TTP_HOVEDMAAL");

                entity.Property(e => e.TtpNote)
                    .HasMaxLength(4000)
                    .HasColumnName("TTP_NOTE");

                entity.Property(e => e.TtpRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTP_REGISTRERTDATO");

                entity.Property(e => e.TtpSlettet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("TTP_SLETTET");

                entity.Property(e => e.TtpTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTP_TILDATO");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaTiltaksplans)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_TILTA_DOKUMENT__FA_DOKU2");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaTiltaksplans)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILTA_KLIENT_TI_FA_KLIE2");

                entity.HasOne(d => d.PtyPlankodeNavigation)
                    .WithMany(p => p.FaTiltaksplans)
                    .HasForeignKey(d => d.PtyPlankode)
                    .HasConstraintName("FK_FA_TILTAKSPLAN_PLANTYPE");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaTiltaksplanSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS2");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaTiltaksplanSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_TILTAKSPLAN_SAKSBEHANDL");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaTiltaksplanSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_T_FA_SAKS3");
            });

            modelBuilder.Entity<FaTiltaksplanevalueringer>(entity =>
            {
                entity.HasKey(e => new { e.TtpLoepenr, e.EvaLoepenr })
                    .IsClustered(false);

                entity.ToTable("FA_TILTAKSPLANEVALUERINGER");

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_TILTAKSPLANEVALUERINGER");

                entity.HasIndex(e => e.TtpLoepenr, "FK_FA_TILTAKSPLANEVALUERINGER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_TILTAKSPLANEVALUERINGER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_TILTAKSPLANEVALUERINGER4");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_TILTAKSPLANEVALUERINGER5");

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");

                entity.Property(e => e.EvaLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("EVA_LOEPENR");

                entity.Property(e => e.ArkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DATO");

                entity.Property(e => e.ArkSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_SJEKK_I_VSA");

                entity.Property(e => e.ArkStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_STOPP");

                entity.Property(e => e.ArkTiltakevaSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_TILTAKEVA_SYSTEMID");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.EvaBegrSlettet)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("EVA_BEGR_SLETTET");

                entity.Property(e => e.EvaDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("EVA_DOKUMENTNR");

                entity.Property(e => e.EvaEmne)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("EVA_EMNE");

                entity.Property(e => e.EvaEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("EVA_ENDRETDATO");

                entity.Property(e => e.EvaFerdigdato)
                    .HasColumnType("datetime")
                    .HasColumnName("EVA_FERDIGDATO");

                entity.Property(e => e.EvaFraklokken)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EVA_FRAKLOKKEN");

                entity.Property(e => e.EvaKommentar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EVA_KOMMENTAR");

                entity.Property(e => e.EvaNotat)
                    .HasMaxLength(4000)
                    .HasColumnName("EVA_NOTAT");

                entity.Property(e => e.EvaPlanlagtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("EVA_PLANLAGTDATO");

                entity.Property(e => e.EvaRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("EVA_REGISTRERTDATO");

                entity.Property(e => e.EvaSlettet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("EVA_SLETTET");

                entity.Property(e => e.EvaSted)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EVA_STED");

                entity.Property(e => e.EvaTilklokken)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("EVA_TILKLOKKEN");

                entity.Property(e => e.EvaTilstede)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("EVA_TILSTEDE");

                entity.Property(e => e.EvaUtfoertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("EVA_UTFOERTDATO");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaTiltaksplanevalueringers)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_TILTA_DOKUMENT__FA_DOKU3");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaTiltaksplanevalueringerSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_TILTA_SBH_TTPEV_FA_SAKSB");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaTiltaksplanevalueringerSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_TILTA_SAKSBEH_E_FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaTiltaksplanevalueringerSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_TTPEVALUERING_REGAV");

                entity.HasOne(d => d.TtpLoepenrNavigation)
                    .WithMany(p => p.FaTiltaksplanevalueringers)
                    .HasForeignKey(d => d.TtpLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_TILTA_TILTAKSPL_FA_TILT3");
            });

            modelBuilder.Entity<FaTiltakstyper>(entity =>
            {
                entity.HasKey(e => e.TttTiltakstype)
                    .IsClustered(false);

                entity.ToTable("FA_TILTAKSTYPER");

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TTT_TILTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.TttBeskrivelse)
                    .HasMaxLength(300)
                    .HasColumnName("TTT_BESKRIVELSE");

                entity.Property(e => e.TttKategori)
                    .HasMaxLength(200)
                    .HasColumnName("TTT_KATEGORI");

                entity.Property(e => e.TttPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTT_PASSIVISERTDATO");

                entity.Property(e => e.TttSsbkode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TTT_SSBKODE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaTmpOmberegning>(entity =>
            {
                entity.HasKey(e => e.TmpEnpLoepenr)
                    .HasName("PK_TMP_OMBEREGNING");

                entity.ToTable("FA_TMP_OMBEREGNING");

                entity.Property(e => e.TmpEnpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TMP_ENP_LOEPENR");

                entity.Property(e => e.TmpEngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TMP_ENG_LOEPENR");

                entity.Property(e => e.TmpEnpNewsum)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("TMP_ENP_NEWSUM");

                entity.Property(e => e.TmpEnpOldsum)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("TMP_ENP_OLDSUM");
            });

            modelBuilder.Entity<FaTtkoder>(entity =>
            {
                entity.HasKey(e => e.TtkKode)
                    .IsClustered(false);

                entity.ToTable("FA_TTKODER");

                entity.Property(e => e.TtkKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TTK_KODE")
                    .IsFixedLength();

                entity.Property(e => e.TtkBeskrivelse)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("TTK_BESKRIVELSE");

                entity.Property(e => e.TtkPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TTK_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaUndersoekelser>(entity =>
            {
                entity.HasKey(e => e.MelLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_UNDERSOEKELSER");

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_UNDERSOEKELSER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosSendtmelderAar, e.PosSendtmelderLoepenr }, "FK_FA_UNDERSOEKELSER10")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhGodkjennSluttrapport, "FK_FA_UNDERSOEKELSER16");

                entity.HasIndex(e => new { e.FroType, e.FroKode1 }, "FK_FA_UNDERSOEKELSER2")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode2 }, "FK_FA_UNDERSOEKELSER3")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.FroType, e.FroKode3 }, "FK_FA_UNDERSOEKELSER4")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_UNDERSOEKELSER5");

                entity.HasIndex(e => e.SbhInitialer2, "FK_FA_UNDERSOEKELSER6");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_UNDERSOEKELSER7");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_UNDERSOEKELSER8");

                entity.HasIndex(e => e.DokUplannr, "FK_FA_UNDERSOEKELSER9")
                    .HasFillFactor(80);

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.ArkDoksrapportDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DOKSRAPPORT_DATO");

                entity.Property(e => e.ArkDoksrapportSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOKSRAPPORT_SJEKK_I_VSA");

                entity.Property(e => e.ArkDoksrapportStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOKSRAPPORT_STOPP");

                entity.Property(e => e.ArkDoksrapportSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_DOKSRAPPORT_SYSTEMID");

                entity.Property(e => e.ArkDokuplanDato)
                    .HasColumnType("datetime")
                    .HasColumnName("ARK_DOKUPLAN_DATO");

                entity.Property(e => e.ArkDokuplanSjekkIVsa)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOKUPLAN_SJEKK_I_VSA");

                entity.Property(e => e.ArkDokuplanStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOKUPLAN_STOPP");

                entity.Property(e => e.ArkDokuplanSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_DOKUPLAN_SYSTEMID");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.DokUplannr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_UPLANNR");

                entity.Property(e => e.FroKode1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE1")
                    .IsFixedLength();

                entity.Property(e => e.FroKode2)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE2")
                    .IsFixedLength();

                entity.Property(e => e.FroKode3)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FRO_KODE3")
                    .IsFixedLength();

                entity.Property(e => e.FroType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FRO_TYPE")
                    .HasDefaultValueSql("('U')")
                    .IsFixedLength();

                entity.Property(e => e.PosSendtmelderAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_SENDTMELDER_AAR");

                entity.Property(e => e.PosSendtmelderLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_SENDTMELDER_LOEPENR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_ENDRETAV");

                entity.Property(e => e.SbhGodkjennSluttrapport)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_GODKJENN_SLUTTRAPPORT");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.Property(e => e.SbhInitialer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER2");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.Property(e => e.Und6mndbegrunnelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("UND_6MNDBEGRUNNELSE");

                entity.Property(e => e.UndBehandlingstid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UND_BEHANDLINGSTID")
                    .IsFixedLength();

                entity.Property(e => e.UndDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UND_DOKUMENTNR");

                entity.Property(e => e.UndDokumentnruplan)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UND_DOKUMENTNRUPLAN");

                entity.Property(e => e.UndEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_ENDRETDATO");

                entity.Property(e => e.UndFerdigdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_FERDIGDATO");

                entity.Property(e => e.UndFerdigdatoUplan)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_FERDIGDATO_UPLAN");

                entity.Property(e => e.UndFristdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_FRISTDATO");

                entity.Property(e => e.UndGjenapnes6mnd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_GJENAPNES6MND");

                entity.Property(e => e.UndGjenapnet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_GJENAPNET");

                entity.Property(e => e.UndGmlreferanse)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UND_GMLREFERANSE");

                entity.Property(e => e.UndHenlagtdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_HENLAGTDATO");

                entity.Property(e => e.UndInnhAndreBarnSitu)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_ANDRE_BARN_SITU");

                entity.Property(e => e.UndInnhAndreForeFami)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_ANDRE_FORE_FAMI");

                entity.Property(e => e.UndInnhBarnAdferdKrim)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_ADFERD_KRIM");

                entity.Property(e => e.UndInnhBarnAdferd)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_ADFERD");

                entity.Property(e => e.UndInnhBarnForeKonflikt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_FORE_KONFLIKT");

                entity.Property(e => e.UndInnhBarnKrim)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_KRIM");

                entity.Property(e => e.UndInnhBarnMennHandel)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_MENN_HANDEL");

                entity.Property(e => e.UndInnhForeManglBeskyt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_MANGL_BESKYT");

                entity.Property(e => e.UndInnhForeManglStimu)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_MANGL_STIMU");

                entity.Property(e => e.UndInnhForeOppfoelgBehov)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_OPPFOELG_BEHOV");

                entity.Property(e => e.UndInnhForeTilgjeng)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_TILGJENG");

                entity.Property(e => e.UndInnhBarnFysiskMish)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_FYSISK_MISH");

                entity.Property(e => e.UndInnhBarnMangOmsorgp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_MANG_OMSORGP");

                entity.Property(e => e.UndInnhBarnNedsFunk)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_NEDS_FUNK");

                entity.Property(e => e.UndInnhBarnPsykProb)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_PSYK_PROB");

                entity.Property(e => e.UndInnhBarnPsykiskMish)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_PSYKISK_MISH");

                entity.Property(e => e.UndInnhBarnRelasvansker)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_RELASVANSKER");

                entity.Property(e => e.UndInnhBarnRusmisbruk)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_RUSMISBRUK");

                entity.Property(e => e.UndInnhBarnSeksuOvergr)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_SEKSU_OVERGR");

                entity.Property(e => e.UndInnhBarnVansjotsel)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_BARN_VANSJOTSEL");

                entity.Property(e => e.UndInnhForeKriminalitet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_KRIMINALITET");

                entity.Property(e => e.UndInnhForeManglerFerdigh)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_MANGLER_FERDIGH");

                entity.Property(e => e.UndInnhForePsykiskProblem)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_PSYKISK_PROBLEM");

                entity.Property(e => e.UndInnhForeRusmisbruk)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_RUSMISBRUK");

                entity.Property(e => e.UndInnhForeSomatiskSykdom)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_FORE_SOMATISK_SYKDOM");

                entity.Property(e => e.UndInnhKonfliktHjemme)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_KONFLIKT_HJEMME");

                entity.Property(e => e.UndInnhPresBarnet)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UND_INNH_PRES_BARNET");

                entity.Property(e => e.UndInnhPresFamilie)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UND_INNH_PRES_FAMILIE");

                entity.Property(e => e.UndInnhVoldHjemme)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_INNH_VOLD_HJEMME");

                entity.Property(e => e.UndKonklusjon)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UND_KONKLUSJON")
                    .IsFixedLength();

                entity.Property(e => e.UndKonklusjonKategori)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("UND_KONKLUSJON_KATEGORI")
                    .HasDefaultValueSql("('102 ')")
                    .IsFixedLength();

                entity.Property(e => e.UndKonklusjontekst)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UND_KONKLUSJONTEKST");

                entity.Property(e => e.UndMeldtnykommune)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UND_MELDTNYKOMMUNE");

                entity.Property(e => e.UndMerknadfristoversittelse)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("UND_MERKNADFRISTOVERSITTELSE");

                entity.Property(e => e.UndRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_REGISTRERTDATO");

                entity.Property(e => e.UndStartdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UND_STARTDATO");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaUndersoekelserDokLoepenrNavigations)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_FA_UNDER_DOKUMENT__FA_DOKUM");

                entity.HasOne(d => d.DokUplannrNavigation)
                    .WithMany(p => p.FaUndersoekelserDokUplannrNavigations)
                    .HasForeignKey(d => d.DokUplannr)
                    .HasConstraintName("FK_FA_UNDER_DOKUMENT__FA_DOKU2");

                entity.HasOne(d => d.MelLoepenrNavigation)
                    .WithOne(p => p.FaUndersoekelser)
                    .HasForeignKey<FaUndersoekelser>(d => d.MelLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_UNDER_MELDING_U_FA_MELDI");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaUndersoekelserSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_UNDER_SAKSBEH_U_FA_SAK3");

                entity.HasOne(d => d.SbhGodkjennSluttrapportNavigation)
                    .WithMany(p => p.FaUndersoekelserSbhGodkjennSluttrapportNavigations)
                    .HasForeignKey(d => d.SbhGodkjennSluttrapport)
                    .HasConstraintName("FK_FA_UNDERSOEKELSER_SBH_GODKJENN_SLUTTRAPPORT");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaUndersoekelserSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_UNDER_SAKSBEH_U_FA_SAKS1");

                entity.HasOne(d => d.SbhInitialer2Navigation)
                    .WithMany(p => p.FaUndersoekelserSbhInitialer2Navigations)
                    .HasForeignKey(d => d.SbhInitialer2)
                    .HasConstraintName("FK_FA_UNDER_SAKSBEH_U_FA_SAK4");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaUndersoekelserSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_UNDER_SAKSBEH_U_FA_SAK2");

                entity.HasOne(d => d.Fro)
                    .WithMany(p => p.FaUndersoekelserFros)
                    .HasForeignKey(d => new { d.FroType, d.FroKode1 })
                    .HasConstraintName("FK_FA_UNDER_FRISTOVER_FA_FRIST");

                entity.HasOne(d => d.FroNavigation)
                    .WithMany(p => p.FaUndersoekelserFroNavigations)
                    .HasForeignKey(d => new { d.FroType, d.FroKode2 })
                    .HasConstraintName("FK_FA_UNDER_FRISTOVER_FA_FRIS2");

                entity.HasOne(d => d.Fro1)
                    .WithMany(p => p.FaUndersoekelserFro1s)
                    .HasForeignKey(d => new { d.FroType, d.FroKode3 })
                    .HasConstraintName("FK_FA_UNDER_FRISTOVER_FA_FRIS3");

                entity.HasOne(d => d.PosSendtmelder)
                    .WithMany(p => p.FaUndersoekelsers)
                    .HasForeignKey(d => new { d.PosSendtmelderAar, d.PosSendtmelderLoepenr })
                    .HasConstraintName("FK_FA_UNDER_POSTJOURN_FA_POSTJ");
            });

            modelBuilder.Entity<FaUndersoekelserSlettet>(entity =>
            {
                entity.HasKey(e => e.UnsLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_UNDERSOEKELSER_SLETTET");

                entity.HasIndex(e => e.KliLoepenr, "FK_FA_UNDERSOEKELSER_SLETTET")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_UNDERSOEKELSER_SLETTET2");

                entity.Property(e => e.UnsLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UNS_LOEPENR");

                entity.Property(e => e.ArkDoksrapportStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOKSRAPPORT_STOPP");

                entity.Property(e => e.ArkDoksrapportSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_DOKSRAPPORT_SYSTEMID");

                entity.Property(e => e.ArkDokuplanStopp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("ARK_DOKUPLAN_STOPP");

                entity.Property(e => e.ArkDokuplanSystemid)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ARK_DOKUPLAN_SYSTEMID");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_REGISTRERTAV");

                entity.Property(e => e.UndDokumentnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UND_DOKUMENTNR");

                entity.Property(e => e.UndDokumentnruplan)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UND_DOKUMENTNRUPLAN");

                entity.Property(e => e.UnsBegrSlettet)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("UNS_BEGR_SLETTET");

                entity.Property(e => e.UnsRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UNS_REGISTRERTDATO");

                entity.HasOne(d => d.KliLoepenrNavigation)
                    .WithMany(p => p.FaUndersoekelserSlettets)
                    .HasForeignKey(d => d.KliLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_UNDER_KLIENT_UN_FA_KLIEN");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaUndersoekelserSlettets)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_UNDER_SBH_REGIS_FA_SAKSB");
            });

            modelBuilder.Entity<FaUndersoekelseslinjer>(entity =>
            {
                entity.HasKey(e => e.UnpLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_UNDERSOEKELSESLINJER");

                entity.HasIndex(e => e.MelLoepenr, "FK_FA_UNDERSOEKELSESLINJER1")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_UNDERSOEKELSESLINJER2");

                entity.HasIndex(e => e.SbhInitialer2, "FK_FA_UNDERSOEKELSESLINJER3");

                entity.Property(e => e.UnpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UNP_LOEPENR");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhInitialer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer2");

                entity.Property(e => e.UnpBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("UNP_BESKRIVELSE");

                entity.Property(e => e.UnpDato)
                    .HasColumnType("datetime")
                    .HasColumnName("UNP_DATO");

                entity.Property(e => e.UnpEkstern)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("UNP_EKSTERN");

                entity.Property(e => e.UnpFrist)
                    .HasColumnType("datetime")
                    .HasColumnName("UNP_FRIST");

                entity.Property(e => e.UnpInternaktivitet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UNP_INTERNAKTIVITET");

                entity.Property(e => e.UnpMerknad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UNP_MERKNAD");

                entity.Property(e => e.UnpOppfyltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("UNP_OPPFYLTDATO");

                entity.Property(e => e.UnpTilhuskeliste)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UNP_TILHUSKELISTE");

                entity.HasOne(d => d.MelLoepenrNavigation)
                    .WithMany(p => p.FaUndersoekelseslinjers)
                    .HasForeignKey(d => d.MelLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_UNDER_UNDERSOK__FA_UNDER");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaUndersoekelseslinjerSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_UNDER_SAKSBEH_U_FA_SAKS3");

                entity.HasOne(d => d.SbhInitialer2Navigation)
                    .WithMany(p => p.FaUndersoekelseslinjerSbhInitialer2Navigations)
                    .HasForeignKey(d => d.SbhInitialer2)
                    .HasConstraintName("FK_FA_UNDER_SAKSBEH_U_FA_SAKS2");
            });

            modelBuilder.Entity<FaUtbetalingsvilkaar>(entity =>
            {
                entity.HasKey(e => e.VikType)
                    .IsClustered(false);

                entity.ToTable("FA_UTBETALINGSVILKAAR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.VikBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("VIK_BESKRIVELSE");
            });

            modelBuilder.Entity<FaVAktiveTiltak>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_AKTIVE_TILTAK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.LovHovedparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_HOVEDPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfparagraf1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JMFPARAGRAF1")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfparagraf2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JMFPARAGRAF2")
                    .IsFixedLength();

                entity.Property(e => e.PlukkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLUKK_DATO");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVAktiveTiltaksplaner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_AKTIVE_TILTAKSPLANER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PtyPlankode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANKODE")
                    .IsFixedLength();

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");
            });

            modelBuilder.Entity<FaVAlleAktiveTiltaksplaner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_ALLE_AKTIVE_TILTAKSPLANER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PtyPlankode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANKODE")
                    .IsFixedLength();

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");
            });

            modelBuilder.Entity<FaVAvvikslogg>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_AVVIKSLOGG");

                entity.Property(e => e.Arbeidstid).HasColumnName("ARBEIDSTID");

                entity.Property(e => e.Dato)
                    .HasColumnType("datetime")
                    .HasColumnName("DATO");

                entity.Property(e => e.Dato2)
                    .HasColumnType("datetime")
                    .HasColumnName("DATO2");

                entity.Property(e => e.Detaljer)
                    .IsUnicode(false)
                    .HasColumnName("DETALJER");

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("GUID");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.Id2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ID2");

                entity.Property(e => e.Idtekst)
                    .HasMaxLength(72)
                    .IsUnicode(false)
                    .HasColumnName("IDTEKST");

                entity.Property(e => e.Klokkeslett)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("KLOKKESLETT");

                entity.Property(e => e.Klokkeslett2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("KLOKKESLETT2");

                entity.Property(e => e.Loggtidspunkt)
                    .HasColumnType("datetime")
                    .HasColumnName("LOGGTIDSPUNKT");

                entity.Property(e => e.Loggtidspunkt2)
                    .HasColumnType("datetime")
                    .HasColumnName("LOGGTIDSPUNKT2");

                entity.Property(e => e.Operatoerinitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("OPERATOERINITIALER");

                entity.Property(e => e.Operatoernavn)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("OPERATOERNAVN");

                entity.Property(e => e.Oppslagstype)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OPPSLAGSTYPE");

                entity.Property(e => e.Oppslagstypetekst)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .HasColumnName("OPPSLAGSTYPETEKST");

                entity.Property(e => e.Referanse)
                    .IsUnicode(false)
                    .HasColumnName("REFERANSE");

                entity.Property(e => e.Sbh1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH1");

                entity.Property(e => e.Sbh2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH2");
            });

            modelBuilder.Entity<FaVBudsjettforbruk>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_BUDSJETTFORBRUK");

                entity.Property(e => e.AarBudsjettBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("AAR_BUDSJETT_BELOEP");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.Property(e => e.MndBudsjettBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("MND_BUDSJETT_BELOEP");

                entity.Property(e => e.MndForbrukBeloep)
                    .HasColumnType("numeric(38, 2)")
                    .HasColumnName("MND_FORBRUK_BELOEP");

                entity.Property(e => e.Periodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("PERIODEAAR");

                entity.Property(e => e.Periodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("PERIODEMND");
            });

            modelBuilder.Entity<FaVCrAapnemeldinger>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AAPNEMELDINGER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mel_loepenr");
            });

            modelBuilder.Entity<FaVCrAapneu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AAPNEUS");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mel_loepenr");
            });

            modelBuilder.Entity<FaVCrAktiveOmsorgsplaner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AKTIVE_OMSORGSPLANER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PtyPlankode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANKODE")
                    .IsFixedLength();

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");
            });

            modelBuilder.Entity<FaVCrAktiveOmsorgstiltak>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AKTIVE_OMSORGSTILTAK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.LovHovedparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_HOVEDPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfparagraf1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JMFPARAGRAF1")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfparagraf2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JMFPARAGRAF2")
                    .IsFixedLength();

                entity.Property(e => e.PlukkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLUKK_DATO");

                entity.Property(e => e.SakSlutningdato)
                    .HasColumnType("datetime")
                    .HasColumnName("SAK_SLUTNINGDATO");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVCrAktiveTiltak>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AKTIVE_TILTAK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.LovHovedparagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_HOVEDPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfparagraf1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JMFPARAGRAF1")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfparagraf2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_JMFPARAGRAF2")
                    .IsFixedLength();

                entity.Property(e => e.PlukkDato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLUKK_DATO");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVCrAktiveTiltaksplaner>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AKTIVE_TILTAKSPLANER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.PtyPlankode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PTY_PLANKODE")
                    .IsFixedLength();

                entity.Property(e => e.TtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TTP_LOEPENR");
            });

            modelBuilder.Entity<FaVCrAvsattemidler>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_AVSATTEMIDLER");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.EnpOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_OPPGAVER");

                entity.Property(e => e.EnpPlantype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENP_PLANTYPE");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_ANVISTAAR");

                entity.Property(e => e.LinjeAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_ANVISTDATO");

                entity.Property(e => e.LinjeAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_ANVISTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_ANVISTNR");

                entity.Property(e => e.LinjeBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_BELOEP");

                entity.Property(e => e.LinjeBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BETALINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BILAGSSERIE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_FORFALLSDATO");

                entity.Property(e => e.LinjeKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_KONTONRMOTTAKER")
                    .IsFixedLength();

                entity.Property(e => e.LinjeLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_LOEPENR");

                entity.Property(e => e.LinjeMelding)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_MELDING");

                entity.Property(e => e.LinjeNyettertbf)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_NYETTERTBF");

                entity.Property(e => e.LinjePeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_PERIODEAAR");

                entity.Property(e => e.LinjePeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("LINJE_PERIODEMND");

                entity.Property(e => e.LinjePlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_PLANLAGTBELOEP");

                entity.Property(e => e.LinjeRefnr)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_REFNR");

                entity.Property(e => e.LinjeSbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_SBH_ANVISTAV");

                entity.Property(e => e.LinjeSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_SKREVET");

                entity.Property(e => e.LinjeTbfbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TBFBELOEP");

                entity.Property(e => e.LinjeTbfompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_TBFOMPOSTERT_LOEPENR");

                entity.Property(e => e.LinjeTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TILBAKEFOERTBELOEP");

                entity.Property(e => e.LinjeVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_VILKAAROPPFYLT");

                entity.Property(e => e.Loennsart)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOENNSART");

                entity.Property(e => e.PlanAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_AVGJORTDATO");

                entity.Property(e => e.PlanBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_BESKRIVELSE");

                entity.Property(e => e.PlanFormaal)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_FORMAAL");

                entity.Property(e => e.PlanKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_KLARGJORTDATO");

                entity.Property(e => e.PlanLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PLAN_LOEPENR");

                entity.Property(e => e.PlanStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_STATUS");

                entity.Property(e => e.PlanStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_STOPPETDATO");

                entity.Property(e => e.PlanVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETFRA");

                entity.Property(e => e.PlanVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETTIL");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_AVGJORTAV_INITIALER");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_KLARGJORTAV_INITIALER");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_STOPPETAV_INITIALER");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.UtbAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTB_AARSAKER");

                entity.Property(e => e.UtbAnvisttilklient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVISTTILKLIENT");

                entity.Property(e => e.UtbBalanse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BALANSE")
                    .IsFixedLength();

                entity.Property(e => e.UtbRekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_REK_AAR");

                entity.Property(e => e.UtbRekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_REK_LOEPENR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVCrForbMedAktEngavt>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_FORB_MED_AKT_ENGAVT");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");
            });

            modelBuilder.Entity<FaVCrFosterhjem>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_FOSTERHJEM");

                entity.Property(e => e.ForEtternavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("for_etternavn");

                entity.Property(e => e.ForFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("for_fornavn");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("for_loepenr");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.KtkFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("ktk_fradato");

                entity.Property(e => e.KtkRolle)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ktk_rolle")
                    .IsFixedLength();

                entity.Property(e => e.KtkTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("ktk_tildato");
            });

            modelBuilder.Entity<FaVCrHuskeliste>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_HUSKELISTE");

                entity.Property(e => e.Aar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("AAR");

                entity.Property(e => e.Emne)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("EMNE");

                entity.Property(e => e.Ferdigdato)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FERDIGDATO");

                entity.Property(e => e.Frist)
                    .HasColumnType("datetime")
                    .HasColumnName("FRIST");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.Klientnavn)
                    .IsRequired()
                    .HasMaxLength(61)
                    .IsUnicode(false)
                    .HasColumnName("KLIENTNAVN");

                entity.Property(e => e.Loepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LOEPENR");

                entity.Property(e => e.Loepenr2)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LOEPENR2");

                entity.Property(e => e.Oppfoelging)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("OPPFOELGING");

                entity.Property(e => e.Oppfyltdato)
                    .HasColumnType("datetime")
                    .HasColumnName("OPPFYLTDATO");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");

                entity.Property(e => e.SbhInitialer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER2");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<FaVCrKliUtenOmsorgsplan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_KLI_UTEN_OMSORGSPLAN");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");
            });

            modelBuilder.Entity<FaVCrKliUtenTiltaksplan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_KLI_UTEN_TILTAKSPLAN");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");
            });

            modelBuilder.Entity<FaVCrKliYtelser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_KLI_YTELSER");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.EnpOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_OPPGAVER");

                entity.Property(e => e.EnpPlantype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENP_PLANTYPE");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER2");

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER3");

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER4");

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER5");

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER6");

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1");

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2");

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3");

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4");

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5");

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6");

                entity.Property(e => e.LinjeAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_ANVISTAAR");

                entity.Property(e => e.LinjeAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_ANVISTDATO");

                entity.Property(e => e.LinjeAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_ANVISTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_ANVISTNR");

                entity.Property(e => e.LinjeBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_BELOEP");

                entity.Property(e => e.LinjeBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BETALINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BILAGSSERIE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_FORFALLSDATO");

                entity.Property(e => e.LinjeKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_KONTONRMOTTAKER")
                    .IsFixedLength();

                entity.Property(e => e.LinjeLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_LOEPENR");

                entity.Property(e => e.LinjePeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_PERIODEAAR");

                entity.Property(e => e.LinjePeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("LINJE_PERIODEMND");

                entity.Property(e => e.LinjePlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_PLANLAGTBELOEP");

                entity.Property(e => e.LinjeSbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_SBH_ANVISTAV");

                entity.Property(e => e.LinjeSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_SKREVET");

                entity.Property(e => e.LinjeTbfbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TBFBELOEP");

                entity.Property(e => e.LinjeTbfompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_TBFOMPOSTERT_LOEPENR");

                entity.Property(e => e.LinjeTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TILBAKEFOERTBELOEP");

                entity.Property(e => e.LinjeVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_VILKAAROPPFYLT");

                entity.Property(e => e.PlanAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_AVGJORTDATO");

                entity.Property(e => e.PlanBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_BESKRIVELSE");

                entity.Property(e => e.PlanFormaal)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_FORMAAL");

                entity.Property(e => e.PlanKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_KLARGJORTDATO");

                entity.Property(e => e.PlanLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PLAN_LOEPENR");

                entity.Property(e => e.PlanStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_STATUS");

                entity.Property(e => e.PlanStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_STOPPETDATO");

                entity.Property(e => e.PlanVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETFRA");

                entity.Property(e => e.PlanVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETTIL");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_AVGJORTAV_INITIALER");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_KLARGJORTAV_INITIALER");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_STOPPETAV_INITIALER");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.UtbAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTB_AARSAKER");

                entity.Property(e => e.UtbAnvisttilklient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVISTTILKLIENT");

                entity.Property(e => e.UtbBalanse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BALANSE")
                    .IsFixedLength();

                entity.Property(e => e.UtbRekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_REK_AAR");

                entity.Property(e => e.UtbRekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_REK_LOEPENR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVCrKlienter>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_klienter");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KliEtternavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_ETTERNAVN");

                entity.Property(e => e.KliFoedselsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_FOEDSELSDATO");

                entity.Property(e => e.KliFornavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FORNAVN");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KliPersonnr)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("KLI_PERSONNR");

                entity.Property(e => e.KplFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KPL_FRADATO");

                entity.Property(e => e.KplPlasseringbor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KPL_PLASSERINGBOR");

                entity.Property(e => e.KplTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KPL_TILDATO");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");
            });

            modelBuilder.Entity<FaVCrKlientplassering>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_KLIENTPLASSERING");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.KomKommunenr)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("KOM_KOMMUNENR");

                entity.Property(e => e.KplBorhos)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("KPL_BORHOS")
                    .IsFixedLength();

                entity.Property(e => e.KplFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("kpl_fradato");

                entity.Property(e => e.KplPlasseringbor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KPL_PLASSERINGBOR")
                    .IsFixedLength();

                entity.Property(e => e.KplPlassert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("kpl_plassert");

                entity.Property(e => e.KplTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("kpl_tildato");
            });

            modelBuilder.Entity<FaVCrMangelfullbetaling>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_MANGELFULLBETALING");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.UtbAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("utb_anvistdato");

                entity.Property(e => e.UtbBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("utb_beloep");

                entity.Property(e => e.UtbBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("utb_betalingstype")
                    .IsFixedLength();

                entity.Property(e => e.UtbForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("utb_forfallsdato");

                entity.Property(e => e.UtbLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("utb_Loepenr");

                entity.Property(e => e.UtbUBetalingsmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("utb_u_betalingsmaate")
                    .IsFixedLength();

                entity.Property(e => e.UtpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("utp_loepenr");
            });

            modelBuilder.Entity<FaVCrMangelfullenglinje>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_MANGELFULLENGLINJE");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.EnlAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("enl_anvistdato");

                entity.Property(e => e.EnlBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("enl_beloep");

                entity.Property(e => e.EnlBetalingstype)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("enl_betalingstype")
                    .IsFixedLength();

                entity.Property(e => e.EnlForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("enl_forfallsdato");

                entity.Property(e => e.EnlLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("enl_Loepenr");

                entity.Property(e => e.EnpLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("enp_loepenr");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");
            });

            modelBuilder.Entity<FaVCrManglerFosterforeldre>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_MANGLER_FOSTERFORELDRE");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KliEtternavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_ETTERNAVN");

                entity.Property(e => e.KliFornavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FORNAVN");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_INITIALER");
            });

            modelBuilder.Entity<FaVCrNullIverksatteTiltak>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_NULL_IVERKSATTE_TILTAK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SakStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("SAK_STATUS")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVCrPlasseringer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_PLASSERINGER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");
            });

            modelBuilder.Entity<FaVCrSaker>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_SAKER");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Konklusjon)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("konklusjon");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mel_loepenr");

                entity.Property(e => e.Sbh2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh2");

                entity.Property(e => e.Sluttdato)
                    .HasColumnType("datetime")
                    .HasColumnName("sluttdato");

                entity.Property(e => e.Startdato)
                    .HasColumnType("datetime")
                    .HasColumnName("startdato");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(360)
                    .HasColumnName("status");

                entity.Property(e => e.Tabell)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tabell");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("til_loepenr");

                entity.Property(e => e.UmelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("umel_loepenr");
            });

            modelBuilder.Entity<FaVCrStatGrunnlag5>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_STAT_GRUNNLAG5");

                entity.Property(e => e.Etternavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ETTERNAVN");

                entity.Property(e => e.Fornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("FORNAVN");

                entity.Property(e => e.FraDato)
                    .HasColumnType("datetime")
                    .HasColumnName("FRA_DATO");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtkFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTK_FRADATO");

                entity.Property(e => e.KtkTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("KTK_TILDATO");

                entity.Property(e => e.Punkt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("punkt");
            });

            modelBuilder.Entity<FaVCrStatGrunnlag5Kobling>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_STAT_GRUNNLAG5_KOBLING");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Nr).HasColumnName("nr");

                entity.Property(e => e.Punkt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("punkt");

                entity.Property(e => e.Sf1Gsaaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SF1_GSAAAR");

                entity.Property(e => e.Sf1Gsajournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SF1_GSAJOURNALNR");

                entity.Property(e => e.Sf1PeriodeFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("SF1_PERIODE_FRADATO");

                entity.Property(e => e.Sf1Punkt)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("sf1_punkt");
            });

            modelBuilder.Entity<FaVCrTilsynsfoerer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_TILSYNSFOERER");

                entity.Property(e => e.ForEtternavn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("for_etternavn");

                entity.Property(e => e.ForFornavn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("for_fornavn");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("for_loepenr");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.KtkFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("ktk_fradato");

                entity.Property(e => e.KtkRolle)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ktk_rolle")
                    .IsFixedLength();

                entity.Property(e => e.KtkTildato)
                    .HasColumnType("datetime")
                    .HasColumnName("ktk_tildato");
            });

            modelBuilder.Entity<FaVCrUstilflerevedtak>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_USTILFLEREVEDTAK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mel_loepenr");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("sak_aar");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("sak_journalnr");
            });

            modelBuilder.Entity<FaVCrUtenOmsorgsplan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_UTEN_OMSORGSPLAN");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVCrUtenTiltaksplan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_UTEN_TILTAKSPLAN");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVCrVarSnittInstitusjon>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_VAR_SNITT_INSTITUSJON");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KliAvsluttetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("KLI_AVSLUTTETDATO");

                entity.Property(e => e.KliEtternavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_ETTERNAVN");

                entity.Property(e => e.KliFornavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KLI_FORNAVN");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KodKorttekst)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("KOD_KORTTEKST");

                entity.Property(e => e.LovHovedParagraf)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_hovedPARAGRAF")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfParagraf1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_jmfPARAGRAF1")
                    .IsFixedLength();

                entity.Property(e => e.LovJmfParagraf2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LOV_jmfPARAGRAF2")
                    .IsFixedLength();

                entity.Property(e => e.TilAvsluttetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_AVSLUTTETDATO");

                entity.Property(e => e.TilIverksattdato)
                    .HasColumnType("datetime")
                    .HasColumnName("TIL_IVERKSATTDATO");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.TttBeskrivelse)
                    .HasMaxLength(300)
                    .HasColumnName("TTT_BESKRIVELSE");

                entity.Property(e => e.TttSsbkode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ttt_ssbkode")
                    .IsFixedLength();

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TTT_TILTAKSTYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVCrYtelser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_CR_YTELSER");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.EnpOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_OPPGAVER");

                entity.Property(e => e.EnpPlantype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENP_PLANTYPE");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_ANVISTAAR");

                entity.Property(e => e.LinjeAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_ANVISTDATO");

                entity.Property(e => e.LinjeAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_ANVISTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_ANVISTNR");

                entity.Property(e => e.LinjeBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_BELOEP");

                entity.Property(e => e.LinjeBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BETALINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BILAGSSERIE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_FORFALLSDATO");

                entity.Property(e => e.LinjeKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_KONTONRMOTTAKER")
                    .IsFixedLength();

                entity.Property(e => e.LinjeLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_LOEPENR");

                entity.Property(e => e.LinjeMelding)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_MELDING");

                entity.Property(e => e.LinjeNyettertbf)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_NYETTERTBF");

                entity.Property(e => e.LinjePeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_PERIODEAAR");

                entity.Property(e => e.LinjePeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("LINJE_PERIODEMND");

                entity.Property(e => e.LinjePlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_PLANLAGTBELOEP");

                entity.Property(e => e.LinjeRefnr)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_REFNR");

                entity.Property(e => e.LinjeSbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_SBH_ANVISTAV");

                entity.Property(e => e.LinjeSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_SKREVET");

                entity.Property(e => e.LinjeTbfbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TBFBELOEP");

                entity.Property(e => e.LinjeTbfompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_TBFOMPOSTERT_LOEPENR");

                entity.Property(e => e.LinjeTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TILBAKEFOERTBELOEP");

                entity.Property(e => e.LinjeVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_VILKAAROPPFYLT");

                entity.Property(e => e.PlanAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_AVGJORTDATO");

                entity.Property(e => e.PlanBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_BESKRIVELSE");

                entity.Property(e => e.PlanFormaal)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_FORMAAL");

                entity.Property(e => e.PlanKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_KLARGJORTDATO");

                entity.Property(e => e.PlanLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PLAN_LOEPENR");

                entity.Property(e => e.PlanStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_STATUS");

                entity.Property(e => e.PlanStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_STOPPETDATO");

                entity.Property(e => e.PlanVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETFRA");

                entity.Property(e => e.PlanVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETTIL");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_AVGJORTAV_INITIALER");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_KLARGJORTAV_INITIALER");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_STOPPETAV_INITIALER");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.UtbAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTB_AARSAKER");

                entity.Property(e => e.UtbAnvisttilklient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVISTTILKLIENT");

                entity.Property(e => e.UtbBalanse)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("UTB_BALANSE")
                    .IsFixedLength();

                entity.Property(e => e.UtbRekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_REK_AAR");

                entity.Property(e => e.UtbRekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_REK_LOEPENR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVGodkjenning>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_GODKJENNING");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE");

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.EnlArbavgferiepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_ARBAVGFERIEPENGER");

                entity.Property(e => e.EnlArbgiveravgift)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_ARBGIVERAVGIFT");

                entity.Property(e => e.EnlBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENL_BELOEP");

                entity.Property(e => e.EnlFeriepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_FERIEPENGER");

                entity.Property(e => e.EnpOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_OPPGAVER");

                entity.Property(e => e.EnpPlantype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENP_PLANTYPE");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER1");

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER2");

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER3");

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER4");

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER5");

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER6");

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1");

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2");

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3");

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4");

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5");

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6");

                entity.Property(e => e.LinjeAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_ANVISTAAR");

                entity.Property(e => e.LinjeAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_ANVISTDATO");

                entity.Property(e => e.LinjeAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_ANVISTMAATE");

                entity.Property(e => e.LinjeAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_ANVISTNR");

                entity.Property(e => e.LinjeBeloep)
                    .HasColumnType("numeric(13, 2)")
                    .HasColumnName("LINJE_BELOEP");

                entity.Property(e => e.LinjeBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BETALINGSTYPE");

                entity.Property(e => e.LinjeBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BILAGSSERIE");

                entity.Property(e => e.LinjeForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_FORFALLSDATO");

                entity.Property(e => e.LinjeKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_KONTONRMOTTAKER");

                entity.Property(e => e.LinjeLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_LOEPENR");

                entity.Property(e => e.LinjePeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_PERIODEAAR");

                entity.Property(e => e.LinjePeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("LINJE_PERIODEMND");

                entity.Property(e => e.LinjePlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_PLANLAGTBELOEP");

                entity.Property(e => e.LinjeSbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_SBH_ANVISTAV");

                entity.Property(e => e.LinjeSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_SKREVET");

                entity.Property(e => e.LinjeTbfbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TBFBELOEP");

                entity.Property(e => e.LinjeTbfompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_TBFOMPOSTERT_LOEPENR");

                entity.Property(e => e.LinjeTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TILBAKEFOERTBELOEP");

                entity.Property(e => e.LinjeVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_VILKAAROPPFYLT");

                entity.Property(e => e.PlanAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_AVGJORTDATO");

                entity.Property(e => e.PlanBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_BESKRIVELSE");

                entity.Property(e => e.PlanFormaal)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_FORMAAL");

                entity.Property(e => e.PlanKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_KLARGJORTDATO");

                entity.Property(e => e.PlanLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PLAN_LOEPENR");

                entity.Property(e => e.PlanStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.PlanStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_STOPPETDATO");

                entity.Property(e => e.PlanVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETFRA");

                entity.Property(e => e.PlanVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETTIL");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_AVGJORTAV_INITIALER");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_KLARGJORTAV_INITIALER");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_STOPPETAV_INITIALER");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.UtbAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTB_AARSAKER");

                entity.Property(e => e.UtbAnvisttilklient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVISTTILKLIENT");

                entity.Property(e => e.UtbRekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_REK_AAR");

                entity.Property(e => e.UtbRekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_REK_LOEPENR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE");
            });

            modelBuilder.Entity<FaVKlientHovedtiltak>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_KLIENT_HOVEDTILTAK");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.TttTiltakstype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ttt_tiltakstype")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVKlimedsperretadresse>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_KLIMEDSPERRETADRESSE");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");
            });

            modelBuilder.Entity<FaVStatFmkontroll>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_FMKONTROLL");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.FmkFradato)
                    .HasColumnType("datetime")
                    .HasColumnName("fmk_fradato");

                entity.Property(e => e.FmkSkjemanr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("fmk_skjemanr");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Linje1)
                    .HasMaxLength(65)
                    .IsUnicode(false)
                    .HasColumnName("linje1");

                entity.Property(e => e.Linje2)
                    .HasMaxLength(213)
                    .IsUnicode(false)
                    .HasColumnName("linje2");

                entity.Property(e => e.Linje3)
                    .HasMaxLength(612)
                    .IsUnicode(false)
                    .HasColumnName("linje3");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mel_loepenr");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("sak_aar");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("sak_journalnr");
            });

            modelBuilder.Entity<FaVStatKontrollskjema>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_KONTROLLSKJEMA");

                entity.Property(e => e.DatoSkjema)
                    .HasColumnType("datetime")
                    .HasColumnName("DATO_SKJEMA");

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("MEL_LOEPENR");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");
            });

            modelBuilder.Entity<FaVStatPaaklaget>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_PAAKLAGET");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.MelLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("mel_loepenr");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("sak_aar");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("sak_journalnr");

                entity.Property(e => e.SakNyttAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("sak_nytt_aar");

                entity.Property(e => e.SakNyttJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("sak_nytt_journalnr");
            });

            modelBuilder.Entity<FaVStatSsb1>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_SSB1");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Linje1)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("linje1");

                entity.Property(e => e.Linje2)
                    .HasMaxLength(95)
                    .IsUnicode(false)
                    .HasColumnName("linje2");

                entity.Property(e => e.Ss1Periode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ss1_periode")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVStatSsb2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_SSB2");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Linje3)
                    .HasMaxLength(553)
                    .IsUnicode(false)
                    .HasColumnName("linje3");

                entity.Property(e => e.Ss1Periode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ss1_periode")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVStatSsb3>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_SSB3");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Linje4)
                    .HasMaxLength(212)
                    .IsUnicode(false)
                    .HasColumnName("linje4");

                entity.Property(e => e.Ss1Periode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ss1_periode")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVStatSsb4>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_STAT_SSB4");

                entity.Property(e => e.DisDistriktskode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("dis_distriktskode")
                    .IsFixedLength();

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("kli_loepenr");

                entity.Property(e => e.Linje5)
                    .HasMaxLength(123)
                    .IsUnicode(false)
                    .HasColumnName("linje5");

                entity.Property(e => e.Ss1Periode)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ss1_periode")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVTiltaksrang>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_TILTAKSRANG");

                entity.Property(e => e.Rang).HasColumnName("RANG");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVTiltaksstatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_TILTAKSSTATUS");

                entity.Property(e => e.Img)
                    .IsRequired()
                    .HasMaxLength(106)
                    .IsUnicode(false)
                    .HasColumnName("IMG");

                entity.Property(e => e.Rang).HasColumnName("RANG");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVUtenTiltaksplan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_UTEN_TILTAKSPLAN");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");
            });

            modelBuilder.Entity<FaVYtelser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("FA_V_YTELSER");

                entity.Property(e => e.BktKode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BKT_KODE")
                    .IsFixedLength();

                entity.Property(e => e.BktType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BKT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.DisDistriktskode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DIS_DISTRIKTSKODE")
                    .IsFixedLength();

                entity.Property(e => e.EngLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("ENG_LOEPENR");

                entity.Property(e => e.EnlArbavgferiepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_ARBAVGFERIEPENGER");

                entity.Property(e => e.EnlArbgiveravgift)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_ARBGIVERAVGIFT");

                entity.Property(e => e.EnlBeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("ENL_BELOEP");

                entity.Property(e => e.EnlFeriepenger)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("ENL_FERIEPENGER");

                entity.Property(e => e.EnpOppgaver)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ENP_OPPGAVER");

                entity.Property(e => e.EnpPlantype)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ENP_PLANTYPE");

                entity.Property(e => e.ForLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("FOR_LOEPENR");

                entity.Property(e => e.KliLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("KLI_LOEPENR");

                entity.Property(e => e.KtnKontonummer1)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER1")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer2)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER2")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer3)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER3")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer4)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER4")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer5)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER5")
                    .IsFixedLength();

                entity.Property(e => e.KtnKontonummer6)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("KTN_KONTONUMMER6")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel1)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL1")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel2)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL2")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel3)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL3")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel4)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL4")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel5)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL5")
                    .IsFixedLength();

                entity.Property(e => e.KtpNoekkel6)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("KTP_NOEKKEL6")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_ANVISTAAR");

                entity.Property(e => e.LinjeAnvistdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_ANVISTDATO");

                entity.Property(e => e.LinjeAnvistmaate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_ANVISTMAATE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeAnvistnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_ANVISTNR");

                entity.Property(e => e.LinjeBeloep)
                    .HasColumnType("numeric(13, 2)")
                    .HasColumnName("LINJE_BELOEP");

                entity.Property(e => e.LinjeBetalingstype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BETALINGSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeBettypeLoepenr)
                    .HasMaxLength(31)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BETTYPE_LOEPENR");

                entity.Property(e => e.LinjeBilagsserie)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_BILAGSSERIE")
                    .IsFixedLength();

                entity.Property(e => e.LinjeForfallsdato)
                    .HasColumnType("datetime")
                    .HasColumnName("LINJE_FORFALLSDATO");

                entity.Property(e => e.LinjeKontonrmottaker)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_KONTONRMOTTAKER")
                    .IsFixedLength();

                entity.Property(e => e.LinjeLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_LOEPENR");

                entity.Property(e => e.LinjePeriodeaar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("LINJE_PERIODEAAR");

                entity.Property(e => e.LinjePeriodemnd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("LINJE_PERIODEMND");

                entity.Property(e => e.LinjePlanlagtbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_PLANLAGTBELOEP");

                entity.Property(e => e.LinjeSbhAnvistav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LINJE_SBH_ANVISTAV");

                entity.Property(e => e.LinjeSkrevet)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_SKREVET");

                entity.Property(e => e.LinjeTbfbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TBFBELOEP");

                entity.Property(e => e.LinjeTbfompostertLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("LINJE_TBFOMPOSTERT_LOEPENR");

                entity.Property(e => e.LinjeTilbakefoertbeloep)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("LINJE_TILBAKEFOERTBELOEP");

                entity.Property(e => e.LinjeVilkaaroppfylt)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("LINJE_VILKAAROPPFYLT");

                entity.Property(e => e.PlanAvgjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_AVGJORTDATO");

                entity.Property(e => e.PlanBeskrivelse)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_BESKRIVELSE");

                entity.Property(e => e.PlanFormaal)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_FORMAAL");

                entity.Property(e => e.PlanKlargjortdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_KLARGJORTDATO");

                entity.Property(e => e.PlanLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("PLAN_LOEPENR");

                entity.Property(e => e.PlanStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("PLAN_STATUS");

                entity.Property(e => e.PlanStoppetdato)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_STOPPETDATO");

                entity.Property(e => e.PlanVarighetfra)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETFRA");

                entity.Property(e => e.PlanVarighettil)
                    .HasColumnType("datetime")
                    .HasColumnName("PLAN_VARIGHETTIL");

                entity.Property(e => e.SakAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("SAK_AAR");

                entity.Property(e => e.SakJournalnr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("SAK_JOURNALNR");

                entity.Property(e => e.SbhAvgjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_AVGJORTAV_INITIALER");

                entity.Property(e => e.SbhKlargjortavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_KLARGJORTAV_INITIALER");

                entity.Property(e => e.SbhStoppetavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_STOPPETAV_INITIALER");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.UtbAarsaker)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("UTB_AARSAKER");

                entity.Property(e => e.UtbAnvisttilklient)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("UTB_ANVISTTILKLIENT");

                entity.Property(e => e.UtbRekAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("UTB_REK_AAR");

                entity.Property(e => e.UtbRekLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("UTB_REK_LOEPENR");

                entity.Property(e => e.VikType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VIK_TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<FaVedlegg>(entity =>
            {
                entity.HasKey(e => e.VedLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_VEDLEGG");

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_VEDLEGG");

                entity.HasIndex(e => e.DokLoepenr, "FK_FA_VEDLEGG2");

                entity.HasIndex(e => e.VedSubject, "IX_FA_VEDLEGG_SUBJECT");

                entity.Property(e => e.VedLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("VED_LOEPENR");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.VedMetaData)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("VED_META_DATA");

                entity.Property(e => e.VedSubject)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VED_SUBJECT");

                entity.Property(e => e.VedSvarinnRef).HasColumnName("VED_SVARINN_REF");

                entity.Property(e => e.VedVedleggDato)
                    .HasColumnType("datetime")
                    .HasColumnName("VED_VEDLEGG_DATO");

                entity.HasOne(d => d.DokLoepenrNavigation)
                    .WithMany(p => p.FaVedleggs)
                    .HasForeignKey(d => d.DokLoepenr)
                    .HasConstraintName("FK_VEDLEGG_DOKUMENT");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaVedleggs)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_POSTJOURNAL_VEDLEGG");
            });

            modelBuilder.Entity<FaVedtaksmyndighet>(entity =>
            {
                entity.HasKey(e => e.MynVedtakstype)
                    .IsClustered(false);

                entity.ToTable("FA_VEDTAKSMYNDIGHET");

                entity.Property(e => e.MynVedtakstype)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MYN_VEDTAKSTYPE")
                    .IsFixedLength();

                entity.Property(e => e.MynMyndenavn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MYN_MYNDENAVN");

                entity.Property(e => e.MynMyndighetsnivaa)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MYN_MYNDIGHETSNIVAA")
                    .IsFixedLength();

                entity.Property(e => e.MynPassivisertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("MYN_PASSIVISERTDATO");
            });

            modelBuilder.Entity<FaVindu>(entity =>
            {
                entity.HasKey(e => e.VinUtvnavn)
                    .IsClustered(false);

                entity.ToTable("FA_VINDU");

                entity.Property(e => e.VinUtvnavn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VIN_UTVNAVN");

                entity.Property(e => e.VinBeskrivelse)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("VIN_BESKRIVELSE");

                entity.Property(e => e.VinUtveventid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("VIN_UTVEVENTID");
            });

            modelBuilder.Entity<FaVurdegenbet>(entity =>
            {
                entity.HasKey(e => e.VurLoepenr)
                    .IsClustered(false);

                entity.ToTable("FA_VURDEGENBET");

                entity.HasIndex(e => e.TilLoepenr, "FK_FA_VURDEGENBET1")
                    .HasFillFactor(80);

                entity.HasIndex(e => new { e.PosAar, e.PosLoepenr }, "FK_FA_VURDEGENBET2")
                    .HasFillFactor(80);

                entity.HasIndex(e => e.SbhInitialer, "FK_FA_VURDEGENBET3");

                entity.HasIndex(e => e.SbhRegistrertav, "FK_FA_VURDEGENBET4");

                entity.HasIndex(e => e.SbhEndretav, "FK_FA_VURDEGENBET5");

                entity.Property(e => e.VurLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("VUR_LOEPENR");

                entity.Property(e => e.PosAar)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("POS_AAR");

                entity.Property(e => e.PosLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("POS_LOEPENR");

                entity.Property(e => e.SbhEndretav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_endretav");

                entity.Property(e => e.SbhInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_initialer");

                entity.Property(e => e.SbhRegistrertav)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("sbh_registrertav");

                entity.Property(e => e.TilLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TIL_LOEPENR");

                entity.Property(e => e.VurAntBarn)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("VUR_ANT_BARN");

                entity.Property(e => e.VurAntVoksne)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("VUR_ANT_VOKSNE");

                entity.Property(e => e.VurDato)
                    .HasColumnType("datetime")
                    .HasColumnName("VUR_DATO");

                entity.Property(e => e.VurEndretdato)
                    .HasColumnType("datetime")
                    .HasColumnName("VUR_ENDRETDATO");

                entity.Property(e => e.VurFormue)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("VUR_FORMUE");

                entity.Property(e => e.VurHvem)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("VUR_HVEM")
                    .IsFixedLength();

                entity.Property(e => e.VurInntekt)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("VUR_INNTEKT");

                entity.Property(e => e.VurKonklusjon)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("VUR_KONKLUSJON");

                entity.Property(e => e.VurNorm)
                    .HasColumnType("numeric(11, 2)")
                    .HasColumnName("VUR_NORM");

                entity.Property(e => e.VurRegistrertdato)
                    .HasColumnType("datetime")
                    .HasColumnName("VUR_REGISTRERTDATO");

                entity.Property(e => e.VurVurdering)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("VUR_VURDERING");

                entity.HasOne(d => d.SbhEndretavNavigation)
                    .WithMany(p => p.FaVurdegenbetSbhEndretavNavigations)
                    .HasForeignKey(d => d.SbhEndretav)
                    .HasConstraintName("FK_FA_VURDE_SAKSBEH1__FA_SAKSB");

                entity.HasOne(d => d.SbhInitialerNavigation)
                    .WithMany(p => p.FaVurdegenbetSbhInitialerNavigations)
                    .HasForeignKey(d => d.SbhInitialer)
                    .HasConstraintName("FK_FA_VURDE_SAKSBEH3__FA_SAKSB");

                entity.HasOne(d => d.SbhRegistrertavNavigation)
                    .WithMany(p => p.FaVurdegenbetSbhRegistrertavNavigations)
                    .HasForeignKey(d => d.SbhRegistrertav)
                    .HasConstraintName("FK_FA_VURDE_SAKSBEH2__FA_SAKSB");

                entity.HasOne(d => d.TilLoepenrNavigation)
                    .WithMany(p => p.FaVurdegenbets)
                    .HasForeignKey(d => d.TilLoepenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FA_VURDE_TILTAK_VU_FA_TILTA");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.FaVurdegenbets)
                    .HasForeignKey(d => new { d.PosAar, d.PosLoepenr })
                    .HasConstraintName("FK_FA_VURDE_POSTJOURN_FA_POSTJ");
            });

            modelBuilder.Entity<GenVDokumenter>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("gen_v_dokumenter");

                entity.Property(e => e.DokDokument)
                    .HasColumnType("image")
                    .HasColumnName("DOK_DOKUMENT");

                entity.Property(e => e.DokLaast)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("DOK_LAAST");

                entity.Property(e => e.DokLoepenr)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DOK_LOEPENR");

                entity.Property(e => e.DokProdusert)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("DOK_PRODUSERT");

                entity.Property(e => e.DokType)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("DOK_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.SbhUtsjekketavInitialer)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SBH_UTSJEKKETAV_INITIALER");
            });

            modelBuilder.Entity<KonvNumRow>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TableName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Table_name");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Msgid)
                    .IsClustered(false);

                entity.ToTable("MESSAGES");

                entity.Property(e => e.Msgid)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MSGID");

                entity.Property(e => e.Msgbutton)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("MSGBUTTON")
                    .IsFixedLength();

                entity.Property(e => e.Msgdefaultbutton)
                    .HasColumnType("numeric(28, 0)")
                    .HasColumnName("MSGDEFAULTBUTTON");

                entity.Property(e => e.Msgheight)
                    .HasColumnType("numeric(28, 0)")
                    .HasColumnName("MSGHEIGHT");

                entity.Property(e => e.Msgicon)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("MSGICON")
                    .IsFixedLength();

                entity.Property(e => e.Msgprint)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MSGPRINT")
                    .IsFixedLength();

                entity.Property(e => e.Msgseverity)
                    .HasColumnType("numeric(28, 0)")
                    .HasColumnName("MSGSEVERITY");

                entity.Property(e => e.Msgtext)
                    .IsRequired()
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("MSGTEXT");

                entity.Property(e => e.Msgtitle)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("MSGTITLE");

                entity.Property(e => e.Msguserinput)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MSGUSERINPUT")
                    .IsFixedLength();

                entity.Property(e => e.Msgwidth)
                    .HasColumnType("numeric(28, 0)")
                    .HasColumnName("MSGWIDTH");
            });

            modelBuilder.Entity<TmpFamiliaKommune>(entity =>
            {
                entity.HasKey(e => e.Gml)
                    .IsClustered(false);

                entity.ToTable("tmp_familia_kommune");

                entity.Property(e => e.Gml)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("gml");

                entity.Property(e => e.Navn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NAVN");

                entity.Property(e => e.Ny)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("ny");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
