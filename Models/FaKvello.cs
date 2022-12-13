using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Models
{
    public partial class FaKvello
    {
        public FaKvello()
        {
            FaKvelloAnsvarligs = new HashSet<FaKvelloAnsvarlig>();
            FaKvelloBeskFaktorers = new HashSet<FaKvelloBeskFaktorer>();
            FaKvelloPersoners = new HashSet<FaKvelloPersoner>();
            FaKvelloRiskFaktorers = new HashSet<FaKvelloRiskFaktorer>();
            FaKvelloTiltaks = new HashSet<FaKvelloTiltak>();
        }

        public decimal KveLoepenr { get; set; }
        public decimal KliLoepenr { get; set; }
        public string SbhRegistrertav { get; set; }
        public string SbhEndretav { get; set; }
        public DateTime KveFradato { get; set; }
        public DateTime? KveTildato { get; set; }
        public string KveInfoKilde { get; set; }
        public decimal KveBoligType { get; set; }
        public string KveBoligBeskrivelse { get; set; }
        public decimal KveOekOrdarb { get; set; }
        public decimal KveOekArbtiltak { get; set; }
        public decimal KveOekArbsoek { get; set; }
        public decimal KveOekUnderutd { get; set; }
        public decimal KveOekYtelsenav { get; set; }
        public decimal KveOekAnnet { get; set; }
        public string KveOekBeskrivelse { get; set; }
        public decimal KvePersonerLaast { get; set; }
        public decimal KveAnsvarligLaast { get; set; }
        public decimal KvePeriodeLaast { get; set; }
        public decimal KveInfoLaast { get; set; }
        public decimal KveBoligLaast { get; set; }
        public decimal KveOekonomiLaast { get; set; }
        public string KveBarnKompetanse { get; set; }
        public string KveBarnPsykisk { get; set; }
        public string KveBarnSomatisk { get; set; }
        public string KveBarnAktiviteter { get; set; }
        public string KveBarnSelvrapport { get; set; }
        public decimal KveBarnKompetanseScore { get; set; }
        public decimal KveBarnFramtoningScore { get; set; }
        public decimal KveBarnAktiviteterScore { get; set; }
        public decimal KveBarnSelvrapportScore { get; set; }
        public decimal KveBarnKompetanseLaast { get; set; }
        public decimal KveBarnHelseLaast { get; set; }
        public decimal KveBarnFramtoningLaast { get; set; }
        public decimal KveBarnAktiviteterLaast { get; set; }
        public decimal KveBarnSelvrapportLaast { get; set; }
        public string KveForeldreMorPsy { get; set; }
        public string KveForeldreMorSom { get; set; }
        public string KveForeldreFarPsy { get; set; }
        public string KveForeldreFarSom { get; set; }
        public string KveForeldreMorFramt { get; set; }
        public string KveForeldreFarFramt { get; set; }
        public string KveForeldreMorForst { get; set; }
        public string KveForeldreFarForst { get; set; }
        public decimal KveForeldreMorFramtScore { get; set; }
        public decimal KveForeldreFarFramtScore { get; set; }
        public decimal KveForeldreMorForstScore { get; set; }
        public decimal KveForeldreFarForstScore { get; set; }
        public decimal KveForeldreMorPsyLaast { get; set; }
        public decimal KveForeldreMorSomLaast { get; set; }
        public decimal KveForeldreFarPsyLaast { get; set; }
        public decimal KveForeldreFarSomLaast { get; set; }
        public decimal KveForeldreMorFramtLaast { get; set; }
        public decimal KveForeldreFarFramtLaast { get; set; }
        public decimal KveForeldreMorForstLaast { get; set; }
        public decimal KveForeldreFarForstLaast { get; set; }
        public string KveSamsGenFamilie { get; set; }
        public string KveSamsOmsorgSensitiv { get; set; }
        public string KveSamsReaksjonMor { get; set; }
        public string KveSamsReaksjonFar { get; set; }
        public string KveSamsInvolverMor { get; set; }
        public string KveSamsInvolverFar { get; set; }
        public string KveSamsRutiner { get; set; }
        public string KveSamsGrenser { get; set; }
        public string KveSamsTilsyn { get; set; }
        public decimal KveSamsGenFamilieScore { get; set; }
        public decimal KveSamsOmsorgSensScore { get; set; }
        public decimal KveSamsReaksjonMorScore { get; set; }
        public decimal KveSamsReaksjonFarScore { get; set; }
        public decimal KveSamsInvolverMorScore { get; set; }
        public decimal KveSamsInvolverFarScore { get; set; }
        public decimal KveSamsRutinerScore { get; set; }
        public decimal KveSamsGrenserScore { get; set; }
        public decimal KveSamsTilsynScore { get; set; }
        public decimal KveSamsGenFamilieLaast { get; set; }
        public decimal KveSamsOmSensitivLaast { get; set; }
        public decimal KveSamsReaksjonLaast { get; set; }
        public decimal KveSamsRutinerLaast { get; set; }
        public decimal KveSamsGrenserLaast { get; set; }
        public decimal KveSamsTilsynLaast { get; set; }
        public string KveFamilieVold { get; set; }
        public string KveFamilieRus { get; set; }
        public string KveFamilieOvergrep { get; set; }
        public string KveFamilieKriminalitet { get; set; }
        public decimal KveFamilieVoldLaast { get; set; }
        public decimal KveFamilieRusLaast { get; set; }
        public decimal KveFamilieOvergrepLaast { get; set; }
        public decimal KveFamilieKrimLaast { get; set; }
        public string KveOppsumeringBeskrivelse { get; set; }
        public decimal KveOppsumeringBeskrLaast { get; set; }
        public decimal KveOppsumeringScoreLaast { get; set; }
        public DateTime? KveEndretdato { get; set; }
        public DateTime KveRegistrertdato { get; set; }
        public string KveFaktorStress { get; set; }
        public decimal KveFaktorStressLaast { get; set; }
        public decimal KveFaktorStressScore { get; set; }
        public decimal KveFaktorRiskLaast { get; set; }
        public decimal KveFaktorBeskLaast { get; set; }
        public decimal KveTilGjennomfoertLaast { get; set; }
        public decimal KveTilAvbruttLaast { get; set; }
        public decimal KveTilAvslaattLaast { get; set; }
        public decimal KveTilPrivatLaast { get; set; }
        public decimal? PosAar { get; set; }
        public decimal? PosLoepenr { get; set; }
        public decimal KveSamsReaksjonLaast1 { get; set; }
        public decimal KveSamsReaksjonLaast2 { get; set; }
        public decimal KveSamsReaksjonLaast3 { get; set; }
        public decimal KveBarnHelseLaast1 { get; set; }
        public DateTime? KveFinishDate { get; set; }
        public decimal KveFamilieVoldScore { get; set; }
        public decimal KveFamilieRusScore { get; set; }
        public decimal KveFamilieOvergrepScore { get; set; }
        public decimal KveFamilieKrimScore { get; set; }
        public decimal KveBackgroundEconomyScore { get; set; }
        public decimal KveBackgroundHousingScore { get; set; }
        public decimal? KveTilAktivLaast { get; set; }
        public string KveSamsTxGeneralFather { get; set; }
        public string KveSamsTxSensitivFather { get; set; }
        public string KveSamsTxRoutesFather { get; set; }
        public string KveSamsTxImplFather { get; set; }
        public string KveSamsTxAttenFather { get; set; }
        public decimal KveSamsGeneralFather { get; set; }
        public decimal KveSamsSensitivityFather { get; set; }
        public decimal KveSamsRoutesFather { get; set; }
        public decimal KveSamsImplFather { get; set; }
        public decimal KveSamsAttenAwayFather { get; set; }
        public decimal KveSamsScGeneralFather { get; set; }
        public decimal KveSamsScSensitivFather { get; set; }
        public decimal KveSamsScRoutesFather { get; set; }
        public decimal KveSamsScImplFather { get; set; }
        public decimal KveSamsScAttenFather { get; set; }
        public string KveBackgroundFamily { get; set; }
        public decimal KveBackgroundLockFamily { get; set; }
        public string KveFaktorVurdering { get; set; }
        public decimal KveNewVersion { get; set; }

        public virtual FaKlient KliLoepenrNavigation { get; set; }
        public virtual FaPostjournal Pos { get; set; }
        public virtual FaSaksbehandlere SbhEndretavNavigation { get; set; }
        public virtual FaSaksbehandlere SbhRegistrertavNavigation { get; set; }
        public virtual ICollection<FaKvelloAnsvarlig> FaKvelloAnsvarligs { get; set; }
        public virtual ICollection<FaKvelloBeskFaktorer> FaKvelloBeskFaktorers { get; set; }
        public virtual ICollection<FaKvelloPersoner> FaKvelloPersoners { get; set; }
        public virtual ICollection<FaKvelloRiskFaktorer> FaKvelloRiskFaktorers { get; set; }
        public virtual ICollection<FaKvelloTiltak> FaKvelloTiltaks { get; set; }
    }
}
