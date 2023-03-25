using System;
using System.Collections.Generic;

namespace UttrekkFamilia.Modulus
{
    public class Document
    {
        public Document()
        {
            aktivitetIdListe = new List<string>();
        }

        public string dokumentId { get; set; }
        public string filId { get; set; }
        public string sakId { get; set; }
        public List<string> aktivitetIdListe { get; set; }
        public string tittel { get; set; }
        public DateTime? journalDato { get; set; }
        public bool? ferdigstilt { get; set; }
        public string filFormat { get; set; }
        public string opprettetAvId { get; set; }
        public string merknadInnsyn { get; set; }
    }

    public class DocumentToInclude
    {
        public DocumentToInclude()
        {
            aktivitetIdListe = new List<string>();
        }

        public decimal dokLoepenr { get; set; }
        public string sakId { get; set; }
        public List<string> aktivitetIdListe { get; set; }
        public string tittel { get; set; }
        public DateTime? journalDato { get; set; }
        public string opprettetAvId { get; set; }
        public string merknadInnsyn { get; set; }
    }
    public class TextDocumentToInclude
    {
        public TextDocumentToInclude()
        {
            aktivitetIdListe = new List<string>();
        }

        public decimal dokLoepenr { get; set; }
        public string sakId { get; set; }
        public List<string> aktivitetIdListe { get; set; }
        public string tittel { get; set; }
        public DateTime? journalDato { get; set; }
        public string opprettetAvId { get; set; }
        public string innhold { get; set; }
        public string beskrivelse { get; set; }
        public DateTime datonotat { get; set; }
        public string forNavn { get; set; }
        public string etterNavn { get; set; }
        public DateTime? foedselsdato { get; set; }
        public string merknadInnsyn { get; set; }
    }
    public class HtmlDocumentToInclude
    {
        public HtmlDocumentToInclude()
        {
            aktivitetIdListe = new List<string>();
        }

        public string dokLoepenr { get; set; }
        public string sakId { get; set; }
        public List<string> aktivitetIdListe { get; set; }
        public string tittel { get; set; }
        public DateTime? journalDato { get; set; }
        public string opprettetAvId { get; set; }
        public string innhold { get; set; }
    }
}
