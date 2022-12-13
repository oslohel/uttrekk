using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Plan
    {
        public Plan()
        {
            tiltakListe = new List<string>();
            delmalListe = new List<DelmalPlan>();
            evalueringListe = new List<PlanEvaluering>();
        }

        public string planId { get; set; }
        public string sakId { get; set; }
        public string planType { get; set; }
        public string undersokelseId { get; set; }
        public string lovhjemmel { get; set; }
        public string situasjonsbeskrivelse { get; set; }
        public DateTime? gyldigFraDato { get; set; }
        public DateTime? gyldigTilDato { get; set; }
        public DateTime? avsluttetDato { get; set; }
        public DateTime? stoppetDato { get; set; }
        public string stoppetBeskrivelse { get; set; }
        public DateTime? nesteEvalueringDato { get; set; }
        public List<string> tiltakListe { get; set; }
        public string planStatus { get; set; }
        public string hovedmaletBarnevernstjenestensTiltak { get; set; }
        public string tegnPaMaloppnaelse { get; set; }
        public List<DelmalPlan> delmalListe { get; set; }
        public List<PlanEvaluering> evalueringListe { get; set; }
        public string varighetOgTilbakeforing { get; set; }
        public string plasseringsted { get; set; }
        public string intensjonForKontaktMedFamilie { get; set; }
        public string barnetsBehovOverTid { get; set; }
        public string bostedOgVarighet { get; set; }
        public string skolegangDagtilbud { get; set; }
        public string økonomi { get; set; }
        public string tjenesterHjelpeapparatet { get; set; }
        public string planForFlytting { get; set; }
        public string nettverk { get; set; }
        public string tidsperspektiv { get; set; }
    }

    public class PlanEvaluering
    {
        public PlanEvaluering()
        {
        }

        public string status { get; set; }
        public DateTime? planlagtEvalueringsDato { get; set; }
        public DateTime? utfortEvalueringsDato { get; set; }
        public DateTime? begrunnelseEndringPlanlagtEvalueringsDato { get; set; }
        public string evaluering { get; set; }
        public string evalueringsResultat { get; set; }
        public string barnetsSynspunkt { get; set; }
        public string foreldresSynspunkt { get; set; }
    }

    public class DelmalPlan
    {
        public DelmalPlan()
        {
        }

        public string tittel { get; set; }
        public string tegnPaMaloppnaelse { get; set; }
        public string ansvarligId { get; set; }
        public DateTime? gyldigFraDato { get; set; }
        public DateTime? gyldigTilDato { get; set; }
        public string tiltakId { get; set; }
        public string status { get; set; }
    }
}