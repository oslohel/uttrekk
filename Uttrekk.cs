#region Usings
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UttrekkFamilia.Models;
using UttrekkFamilia.ModelsBVV;
using UttrekkFamilia.Modulus;
using File = System.IO.File;
#endregion

namespace UttrekkFamilia
{
    internal class Uttrekk
    {
        #region Members
        private readonly DateTime LastDateNoMigration = new(1997, 12, 31);
        private readonly DateTime FromDateMigrationTilsyn = new(2004, 12, 31);
        private readonly DateTime FirstInYearOfMigration = new(2023, 1, 1);
        private readonly DateTime FirstInYearOfNewLaw = new(2023, 1, 1);
        private readonly DateTime FirstDateOfMigrationMeldingerUtenSak = new(2018, 1, 1);
        private string ConnectionStringFamilia = "";
        private readonly string ConnectionStringSokrates = "";
        private readonly string ConnectionStringVFB = "";
        private readonly string ConnectionStringMigrering = "";
        private readonly string MainDBServer = "";
        private const string SchemaSokrates = "SOKRATES";
        private readonly string OutputFolderName = "";
        private string Bydelsforkortelse = "";
        private readonly bool OnlyWriteDocumentFiles;
        private readonly bool OnlyActiveCases;
        private readonly bool OnlyPassiveCases;
        private readonly int AntallFilerPerZip;
        private readonly Mappings mappings;
        private readonly string BVVLeder = "18";
        private readonly int MaxAntallEntiteterPerFil = 1000;
        private readonly string MigreringsdbPostfix = "";
        #endregion

        #region Contructors
        public Uttrekk(string connSokrates, string mainDBServer, string extraDBServer, string outputFolderName, string bydelsidentifikator, bool useSokrates, bool onlyWriteDocumentFiles, int antallFilerPerZip, bool onlyActiveCases, bool onlyPassiveCases, bool production)
        {
            mappings = new Mappings(useSokrates);
            ConnectionStringFamilia = mappings.GetConnectionstring(bydelsidentifikator, mainDBServer);
            ConnectionStringVFB = mappings.GetConnectionstring("BVV", extraDBServer);
            ConnectionStringMigrering = mappings.GetConnectionstring("MIG", extraDBServer);
            ConnectionStringSokrates = connSokrates;
            MainDBServer = mainDBServer;
            OutputFolderName = outputFolderName;
            Bydelsforkortelse = bydelsidentifikator;
            OnlyWriteDocumentFiles = onlyWriteDocumentFiles;
            OnlyActiveCases = onlyActiveCases;
            OnlyPassiveCases = onlyPassiveCases;
            if (OnlyActiveCases && OnlyPassiveCases)
            {
                OnlyActiveCases = false;
                OnlyPassiveCases = false;
            }
            AntallFilerPerZip = antallFilerPerZip;
            if (production)
            {
                MigreringsdbPostfix = "PROD";
            }
        }
        #endregion

        #region Folders
        public void CreateAllfolders(bool onlyWriteDocumentFiles = false)
        {
            if (onlyWriteDocumentFiles)
            {
                CreateFolderIfNotExist("filer");
            }
            else
            {
                CreateFolderIfNotExist("saker");
                CreateFolderIfNotExist("innbygger");
                CreateFolderIfNotExist("organisasjon");
                CreateFolderIfNotExist("barnetsNettverk");
                CreateFolderIfNotExist("melding");
                CreateFolderIfNotExist("henvendelse");
                CreateFolderIfNotExist("undersokelser");
                CreateFolderIfNotExist("avdelingsmapping");
                CreateFolderIfNotExist("brukere");
                CreateFolderIfNotExist("vedtak");
                CreateFolderIfNotExist("tiltak");
                CreateFolderIfNotExist("plan");
                CreateFolderIfNotExist("aktiviteter");
                CreateFolderIfNotExist("dokumenter");
                CreateFolderIfNotExist("filer");
            }
        }
        #endregion

        #region Sokrates
        public async Task<string> ExtractSokratesAsync(BackgroundWorker worker, bool showResult = false)
        {
            OracleConnection connection = new(ConnectionStringSokrates);
            OracleDataReader reader = null;
            try
            {
                worker.ReportProgress(0, $"Innhenter informasjon barnevernssaker for {Bydelsforkortelse} fra Sokrates...");
                mappings.ResetSokratesList();
                List<FaKlient> rawData;
                int totalAntall = 0;
                int antallInnflyttet = 0;
                int antallUtflyttet = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaKlients
                        .OrderBy(o => o.KliLoepenr)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }

                int antall = 0;
                connection.Open();

                foreach (var klient in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Innhenter informasjon barnevernssaker for {Bydelsforkortelse} fra Sokrates ({antall} av {totalAntall})...");
                    }
                    Sokrates sokrates = new()
                    {
                        kliLoepenr = klient.KliLoepenr,
                        fodselsnummer = klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr
                    };
                    if (klient.KliPersonnr.GetValueOrDefault() != 99999 && klient.KliPersonnr.GetValueOrDefault() != 00100 && klient.KliPersonnr.GetValueOrDefault() != 00200)
                    {
                        OracleCommand command = new($"Select Client.Office_id, Client.id from {SchemaSokrates}.Person, {SchemaSokrates}.Client Where Person.Id = Client.Person_id And Person.Personalnumber = '{sokrates.fodselsnummer}'", connection)
                        {
                            CommandType = System.Data.CommandType.Text
                        };
                        int officeId = 0;
                        int clientId = 0;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            officeId = reader.GetInt32(0);
                            clientId = reader.GetInt32(1);
                        }
                        sokrates.eierBydel = mappings.IsKontoretDenneBydel(officeId, Bydelsforkortelse);

                        if (sokrates.eierBydel)
                        {
                            command = new OracleCommand($"Select Office_Id_from, Office_Id_to, DTG from {SchemaSokrates}.transfer_history Where Transfer_Code_id = 12 And Client_id = {clientId} Order by Id", connection)
                            {
                                CommandType = System.Data.CommandType.Text
                            };
                            DateTime? lastTransferDate = null;
                            bool tidligereBydelerFinnes = false;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                int fromOffice = reader.GetInt32(0);
                                int toOffice = reader.GetInt32(1);
                                if (mappings.GetBydelFraOffice(fromOffice) == mappings.GetBydelFraOffice(toOffice))
                                {
                                    continue;
                                }
                                OracleDate transferDate = reader.GetOracleDate(2);
                                TidligereAvdeling tidligereBydel = new()
                                {
                                    avdelingId = mappings.GetBydelFraOffice(fromOffice),
                                    tilDato = (DateTime)transferDate
                                };
                                if (lastTransferDate.HasValue)
                                {
                                    tidligereBydel.fraDato = lastTransferDate.Value;
                                }
                                lastTransferDate = (DateTime)transferDate;

                                if (klient.KliFoedselsdato.HasValue && klient.KliPersonnr.HasValue && !string.IsNullOrEmpty(tidligereBydel.avdelingId))
                                {
                                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(tidligereBydel.avdelingId, MainDBServer)))
                                    {
                                        FaKlient klientTidligereBydel = await context.FaKlients.Where(k => k.KliFoedselsdato == klient.KliFoedselsdato.Value && k.KliPersonnr == klient.KliPersonnr.Value).FirstOrDefaultAsync();
                                        if (klientTidligereBydel != null)
                                        {
                                            tidligereBydel.avdelingId = GetEnhetskode(klientTidligereBydel.DisDistriktskode, tidligereBydel.avdelingId);
                                            sokrates.tidligereAvdelinger.Add(tidligereBydel);
                                            tidligereBydelerFinnes = true;
                                        }
                                    }
                                }
                            }
                            if (tidligereBydelerFinnes)
                            {
                                antallInnflyttet += 1;
                            }
                        }
                        else
                        {
                            antallUtflyttet += 1;
                        }
                    }
                    else
                    {
                        sokrates.eierBydel = true;
                    }
                    mappings.AddKlient(sokrates);
                }
                if (showResult)
                {
                    MessageBox.Show($"Totalt {totalAntall} stk., hvorav {antallUtflyttet} stk utflyttet og {antallInnflyttet} innflyttet", $"Oversikt barnevernssaker {Bydelsforkortelse}", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return $"Antall innflyttede i Sokrates: {antallInnflyttet}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        #endregion

        #region Information Familia
        public async Task GetInformationFamiliaAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Innhenter informasjon fra Familia...");

                string information = "INFORMASJON " + Bydelsforkortelse + Environment.NewLine + Environment.NewLine;
                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    int count = await context.FaMeldingers.Where(m => m.MelMeldingstype != "UGR").CountAsync();
                    information += $"{count,10}: Antall meldinger totalt (uten UGR)";
                    count = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall dokumenter";

                    SqlConnection connection = new(ConnectionStringFamilia);
                    SqlDataReader reader = null;

                    try
                    {
                        connection.Open();
                        SqlCommand command = new($"Select Dok_Mimetype, Count(*) From FA_DOKUMENTER Where DOK_PRODUSERT = 1 AND DOK_DOKUMENT Is Not Null Group by Dok_Mimetype", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string mimeType = "Ikke angitt";
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                mimeType = reader.GetString(0);
                            }
                            int numberOfDocuments = reader.GetInt32(1);
                            information += Environment.NewLine + $"{numberOfDocuments,10}: Antall dokumenter med av typen {mimeType}";
                        }
                        reader.Close();
                        command = new($"Select Sum(CAST (Datalength(Dok_Dokument) as BigInt)) From FA_DOKUMENTER Where DOK_PRODUSERT = 1 AND DOK_DOKUMENT Is Not Null", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                Int64 size = reader.GetInt64(0);
                                decimal sizeDecimal = decimal.Divide(size, 1024);
                                sizeDecimal = decimal.Divide(sizeDecimal, 1024);
                                sizeDecimal = decimal.Divide(sizeDecimal, 1024);
                                information += Environment.NewLine + $"{sizeDecimal,10:0.0}: Totalt GB med dokumenter";
                            }
                            else
                            {
                                information += Environment.NewLine + $"{0,10}: Totalt GB med dokumenter";
                            }
                        }
                        else
                        {
                            information += Environment.NewLine + $"{0,10}: Totalt GB med dokumenter";
                        }
                        reader.Close();
                        command = new($"Select Top 5 CAST (Datalength(Dok_Dokument) as BigInt) From FA_DOKUMENTER Where DOK_PRODUSERT = 1 AND DOK_DOKUMENT Is Not Null Order By Datalength(Dok_Dokument) Desc", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        int counter = 1;
                        while (reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                Int64 size = reader.GetInt64(0);
                                decimal sizeDecimal = decimal.Divide(size, 1024);
                                sizeDecimal = decimal.Divide(sizeDecimal, 1024);
                                information += Environment.NewLine + $"{sizeDecimal,10:0.0}: Antall MB for {counter}.største dokument";
                                counter += 1;
                            }
                        }
                        reader.Close();
                        command = new($"Select POS_BREVTYPE, Count(*) From FA_POSTJOURNAL Where POS_UNNDRATTINNSYN_IS = 0 Group by POS_BREVTYPE Order by POS_BREVTYPE", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string brevType = "Ikke angitt";
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                brevType = reader.GetString(0);
                            }
                            int numberOfDocuments = reader.GetInt32(1);
                            information += Environment.NewLine + $"{numberOfDocuments,10}: Antall postjournaler med brevtypen {brevType}";
                        }
                        reader.Close();
                        command = new($"Select POS_BREVTYPE, Count(*) From FA_POSTJOURNAL Where POS_UNNDRATTINNSYN_IS = 0 And Pos_Ferdigdato Is Null Group by POS_BREVTYPE Order by POS_BREVTYPE", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string brevType = "Ikke angitt";
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                brevType = reader.GetString(0);
                            }
                            int numberOfDocuments = reader.GetInt32(1);
                            information += Environment.NewLine + $"{numberOfDocuments,10}: Antall postjournaler med brevtypen {brevType} hvor ferdigdato ikke er satt";
                        }
                        reader.Close();
                        command = new($"Select POS_BREVTYPE, Count(*) From FA_POSTJOURNAL Where POS_UNNDRATTINNSYN_IS = 0 And Pos_Ferdigdato Is Null And Pos_Registrertdato < CAST('20220101 00:00:00.000' AS DATETIME) Group by POS_BREVTYPE Order by POS_BREVTYPE", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string brevType = "Ikke angitt";
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                brevType = reader.GetString(0);
                            }
                            int numberOfDocuments = reader.GetInt32(1);
                            information += Environment.NewLine + $"{numberOfDocuments,10}: Antall postjournaler med brevtypen {brevType} hvor ferdigdato ikke er satt som er eldre enn 2022";
                        }
                        reader.Close();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    count = await context.FaPostjournals.Where(j => j.PosUnndrattinnsynIs == 0 && j.PosBrevtype == "VE" && !j.PosFerdigdato.HasValue).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall postjournaler av typen VE hvor ferdigdato ikke er satt";
                    count = await context.FaPostjournals.Where(j => j.PosUnndrattinnsynIs == 0 && j.PosBrevtype == "VE" && !j.PosFerdigdato.HasValue && j.PosRegistrertdato.HasValue && j.PosRegistrertdato.Value.Year == 2022).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall postjournaler av typen VE hvor ferdigdato ikke er satt registert i 2022";
                    count = await context.FaPostjournals.Where(j => j.PosUnndrattinnsynIs == 0 && j.PosBrevtype == "VE" && !j.PosFerdigdato.HasValue && j.PosRegistrertdato.HasValue && j.PosRegistrertdato.Value.Year == 2021).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall postjournaler av typen VE hvor ferdigdato ikke er satt registert i 2021";
                    count = await context.FaPostjournals.Where(j => j.PosUnndrattinnsynIs == 0 && j.PosBrevtype == "VE" && !j.PosFerdigdato.HasValue && j.PosRegistrertdato.HasValue && j.PosRegistrertdato.Value.Year < 2021).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall postjournaler av typen VE hvor ferdigdato ikke er satt registert før 2021";

                    count = await context.FaSaksbehandleres.CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall saksbehandlere";
                    count = await context.FaSaksbehandleres.Where(s => !string.IsNullOrEmpty(s.SbhLoennsnr)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall saksbehandlere med utfylt lønnsnummer (SBH_LOENNSNR)";

                    count = await context.FaSaksjournals.Where(s => string.IsNullOrEmpty(s.LovHovedParagraf) && (!string.IsNullOrEmpty(s.LovJmfParagraf1) || !string.IsNullOrEmpty(s.LovJmfParagraf2))).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall saksjournaler med utfylt lovparagraf 1 eller 2, men ikke hovedparagraf";
                    count = await context.FaSaksjournals.Where(s => string.IsNullOrEmpty(s.LovJmfParagraf1) && !string.IsNullOrEmpty(s.LovJmfParagraf2)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall saksjournaler med utfylt lovparagraf 2, men ikke lovparagraf 1";

                    count = await context.FaKlients.Where(k => k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Totalt antall saker (uten saker med beskyttet tilsyn)";
                    count = await context.FaKlients.Where(k => !k.KliFoedselsdato.HasValue && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter uten fødselsdato";
                    count = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && k.KliFoedselsdato > LastDateNoMigration && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter født etter {LastDateNoMigration:dd.MM.yyyy}";
                    count = await context.FaKlients.Where(k => (k.KliFoedselsdato.HasValue && k.KliFraannenkommune == 0 && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue))).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter født etter {LastDateNoMigration:dd.MM.yyyy} eller med åpen sak";
                    count = await context.FaKlients.Where(k => !k.KliAvsluttetdato.HasValue && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter med åpen sak";
                    count = await context.FaKlients.Where(k => k.KliAvsluttetdato.HasValue && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter med avsluttet sak";
                    count = await context.FaKlients.Where(k => !k.KliAvsluttetdato.HasValue && k.KliFraannenkommune == 1).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter med beskyttet tilsyn og åpen sak";

                    count = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && k.KliFraannenkommune == 0 && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue) && string.IsNullOrEmpty(k.SbhInitialer)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter som skal migreres som ikke har hovedsaksbehandler (FA_Klient.sbh_Initialer)";
                    count = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && k.KliFraannenkommune == 0 && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue) && string.IsNullOrEmpty(k.SbhInitialer) && !string.IsNullOrEmpty(k.SbhInitialer2)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall klienter som skal migreres som ikke har hovedsaksbehandler (FA_Klient.sbh_Initialer), men har sekundærbehandler (FA_Klient.sbh_Initialer2)";

                    count = await context.FaMeldingers.Where(m => m.MelMeldingstype != "UGR" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall meldinger knyttet til saker som skal migreres";
                    count = await context.FaMeldingers.Where(m => m.MelMeldingstype != "UGR" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue) && !m.MelAvsluttetgjennomgang.HasValue).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall meldinger knyttet til saker som skal migreres hvor behandling av melding ikke er avsluttet";
                    count = await context.FaMeldingers.Where(m => m.MelMeldingstype != "UGR" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue) && (m.MelHenlagtPgaUtenforbvl == 0 && m.MelHenlagtAnnenInstans == 0 && m.MelHenlagtTilAnnenBv == 0 && m.MelKonklusjon == "H")).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall meldinger knyttet til saker som skal migreres som er henlagt H men som ikke har henlagtkode";

                    count = await context.FaMeldingers.Where(m => m.MelMeldingstype != "UGR" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue) && ((m.MelMeldtBarnet == 1 || m.MelMeldtForeldre == 1 || m.MelMeldtFamilie == 1 || m.MelMeldtNaboer == 1 || m.MelMeldtAndre == 1) && (m.MelMeldtSosialkt == 1 || m.MelMeldtBarnevt == 1 || m.MelMeldtBarnevvakt == 1 || m.MelMeldtBarnehage == 1 || m.MelMeldtHelsestasjon == 1 || m.MelMeldtLege == 1 || m.MelMeldtSkole == 1 || m.MelMeldtPedPpt == 1 || m.MelMeldtPoliti == 1 || m.MelMeldtBup == 1 || m.MelMeldtOffentlig == 1 || m.MelMeldtPsykiskHelseBarn == 1 || m.MelMeldtPsykiskHelseVoksne == 1 || m.MelMeldtUdi == 1 || m.MelMeldtKrisesenter == 1 || m.MelMeldtFrivillige == 1 || m.MelMeldtUtekontakt == 1 || m.MelMeldtFamilievernkontor == 1 || m.MelMeldtTjenesteInstans == 1))).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall meldinger knyttet til saker som skal migreres som har både privat og offentlig type melder";

                    count = await context.FaMeldingers.Where(m => m.MelMeldingstype != "UGR" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue) && (m.MelKonklusjon == "H" && (m.MelHenlagtTilAnnenBv == 0 && m.MelHenlagtAnnenInstans == 0 && m.MelHenlagtPgaUtenforbvl == 0))).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall meldinger knyttet til saker som skal migreres som er henlagt, men som ikke har henlagtkode";

                    count = await context.FaUndersoekelsers.Where(m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.MelLoepenrNavigation.KliLoepenrNavigation.KliAvsluttetdato.HasValue)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall undersøkelser knyttet til saker som skal migreres";

                    count = await context.FaUndersoekelsers.Where(m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.MelLoepenrNavigation.KliLoepenrNavigation.KliAvsluttetdato.HasValue) && !m.UndFristdato.HasValue).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall undersøkelser knyttet til saker som skal migreres som ikke har fristdato";

                    count = await context.FaTiltaksplans.Where(m => m.TtpSlettet == 0 && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall planer knyttet til saker som skal migreres";
                    count = await context.FaTiltaksplans.Where(m => m.TtpSlettet == 1).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall slettede planer";

                    count = await context.FaTiltaks.Where(m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall tiltak knyttet til saker som skal migreres";

                    count = await context.FaTiltaks.Where(m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue) && (m.TilTildato.HasValue && m.TilTildato.Value < m.TilFradato)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall tiltak knyttet til saker som skal migreres hvor Tildato er tidligere enn Fradato";

                    count = await context.FaForbindelsers.Where(f => string.IsNullOrEmpty(f.ForFoedselsnummer) && !f.ForDnummer.HasValue && string.IsNullOrEmpty(f.ForOrganisasjonsnr) && string.IsNullOrEmpty(f.ForLeverandoernr)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall forbindelser uten fødselsnummer, d-nummer, org.nummer eller leverandørnummer";
                    count = await context.FaForbindelsers.Where(f => !string.IsNullOrEmpty(f.ForOrganisasjonsnr)).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall forbindelser med org.nummer";
                    count = await context.FaForbindelsers.Where(f => !string.IsNullOrEmpty(f.ForFoedselsnummer) || f.ForDnummer.HasValue).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall forbindelser med fødselsnummer eller D-nummer";
                    count = await context.FaForbindelsers.Where(f => !string.IsNullOrEmpty(f.ForFoedselsnummer) && f.ForFoedselsnummer.Length != 11).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall forbindelser med fødselsnummer som ikke er 11 tegn";
                    List<FaForbindelser> forbindelser = await context.FaForbindelsers.Where(f => !string.IsNullOrEmpty(f.ForFoedselsnummer)).ToListAsync();
                    count = forbindelser.Where(f => !IsDigitsOnly(f.ForFoedselsnummer)).Count();
                    information += Environment.NewLine + $"{count,10}: Antall forbindelser med fødselsnummer som ikke kun er siffer";

                    count = await context.FaKlients.Where(k => k.KliPersonnr.GetValueOrDefault() == 99999 && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall ufødte barn (personnr 99999)";
                    count = await context.FaKlients.Where(k => k.KliPersonnr.GetValueOrDefault() == 00100 && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall ukjente gutter (personnr 00100)";
                    count = await context.FaKlients.Where(k => k.KliPersonnr.GetValueOrDefault() == 00200 && k.KliFraannenkommune == 0).CountAsync();
                    information += Environment.NewLine + $"{count,10}: Antall ukjente jenter (personnr 00200)" + Environment.NewLine + Environment.NewLine;
                }
                worker.ReportProgress(0, "Teller antall rader alle tabeller...");

                information += GetInfoTabellerMSSQL(ConnectionStringFamilia);
                string fileName = $"{OutputFolderName}{Bydelsforkortelse}_{DateTime.Now:yyyyMMdd_HHmm_}InformasjonFamilia.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Informasjon lagret i filen {fileName}");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task GetOneFileFamiliaAsync(BackgroundWorker worker)
        {
            try
            {
                CreateFolderIfNotExist("filer");

                worker.ReportProgress(0, "Starter uthenting dokumentfiler...");
                string folderName = $"{OutputFolderName}";
                string fileName = $"{OutputFolderName}Doks.txt";

                if (!File.Exists(fileName))
                {
                    MessageBox.Show($"Fil {fileName} eksisterer ikke.", "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                IEnumerable<string> lines = File.ReadLines(fileName);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        decimal dokLoepenr = Convert.ToDecimal(line);
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            dokument = await context.FaDokumenters.Where(p => p.DokLoepenr == dokLoepenr).FirstOrDefaultAsync();
                            if (dokument == null)
                            {
                                continue;
                            }
                        }
                        SqlConnection connection = new(ConnectionStringFamilia);
                        SqlDataReader reader = null;
                        try
                        {
                            connection.Open();
                            string fileExtension = ".doc";
                            string dokMimetype = dokument.DokMimetype?.Trim();
                            if (!string.IsNullOrEmpty(dokMimetype))
                            {
                                dokMimetype = dokMimetype.ToLower();
                                if (dokMimetype == "application/pdf")
                                {
                                    fileExtension = ".pdf";
                                }
                                else
                                {
                                    if (dokMimetype == "text/html")
                                    {
                                        fileExtension = ".html";
                                    }
                                }
                            }
                            SqlCommand command = new($"Select Dok_Dokument From FA_DOKUMENTER Where DOK_Loepenr={dokLoepenr}", connection)
                            {
                                CommandTimeout = 300
                            };
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + dokLoepenr.ToString() + "__" + Bydelsforkortelse + fileExtension, (byte[])reader["Dok_Dokument"]);
                            }
                            reader.Close();
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                worker.ReportProgress(0, $"Uthenting dokumentfiler ferdig.");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Information Visma Flyt Barnevernvakt
        public async Task GetInformationBVVAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Innhenter informasjon fra Visma Flyt Barnevernvakt...");
                string information = "INFORMASJON VISMA FLYT BARNEVERNVAKT" + Environment.NewLine + Environment.NewLine;
                worker.ReportProgress(0, "Teller antall rader alle tabeller...");
                information += GetInfoTabellerMSSQL(ConnectionStringVFB);
                string fileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}InformasjonVFB.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Informasjon lagret i filen {fileName}");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Information Entiteter
        public async Task GetInformationAntallEntiteterAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Teller entiteter pr bydel...");
                string information = "";
                foreach (var bydel in mappings.GetAlleBydeler())
                {
                    worker.ReportProgress(0, $"Teller {bydel}...");
                    Bydelsforkortelse = bydel;
                    ConnectionStringFamilia = mappings.GetConnectionstring(Bydelsforkortelse, MainDBServer);
                    await ExtractSokratesAsync(worker, false);
                    worker.ReportProgress(0, $"Teller {bydel}...");
                    int antallSaker = 0;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        List<FaKlient> klienter = await context.FaKlients
                            .Where(KlientFilter())
                            .OrderBy(o => o.KliLoepenr)
                            .Where(k => k.KliFraannenkommune == 0)
                            .ToListAsync();
                        foreach (var klient in klienter)
                        {
                            if (mappings.IsOwner(klient.KliLoepenr))
                            {
                                antallSaker += 1;
                            }
                        }

                        List<FaMeldinger> meldinger = meldinger = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak).ToListAsync();
                        int antallUtenSak = meldinger.Count;
                        foreach (var melding in meldinger)
                        {
                            if (string.IsNullOrEmpty(melding.MelGmlreferanse) && melding.MelPersonnr.HasValue && melding.MelFoedselsdato.HasValue && melding.MelPersonnr.GetValueOrDefault() != 99999 && melding.MelPersonnr.GetValueOrDefault() != 00100 && melding.MelPersonnr.GetValueOrDefault() != 00200)
                            {
                                int antallOrdinæreSaker = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue)).Where(k => k.KliFraannenkommune == 0 && k.KliFoedselsdato == melding.MelFoedselsdato && k.KliPersonnr == melding.MelPersonnr).CountAsync();
                                if (antallOrdinæreSaker > 0)
                                {
                                    antallUtenSak -= 1;
                                }
                                else
                                {
                                    FaMeldinger firstMelding = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak && m.MelFoedselsdato == melding.MelFoedselsdato && m.MelPersonnr == melding.MelPersonnr).OrderByDescending(m => m.MelLoepenr).FirstOrDefaultAsync();
                                    if (firstMelding != null)
                                    {
                                        if (melding.MelLoepenr != firstMelding.MelLoepenr)
                                        {
                                            antallUtenSak -= 1;
                                        }
                                    }
                                }
                            }
                        }
                        antallSaker += antallUtenSak;

                        List<FaKlient> tilsynssaker = await context.FaKlients
                            .Where(KlientFilter())
                            .Where(m => m.KliFraannenkommune == 1 && !m.KliAvsluttetdato.HasValue && m.KliFoedselsdato.HasValue && m.KliFoedselsdato > FromDateMigrationTilsyn)
                            .OrderBy(o => o.KliLoepenr)
                            .ToListAsync();
                        antallSaker += tilsynssaker.Count;

                        List<FaMedarbeidere> medarbeidere = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue)).ToListAsync();
                        int antallMedarbeidere = medarbeidere.Count;

                        foreach (var medarbeider in medarbeidere)
                        {
                            int numberOfActiveContracts = await context.FaEngasjementsavtales.Where(e => e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                                && (e.EngTildato >= FirstInYearOfMigration)).CountAsync();
                            if (numberOfActiveContracts == 0)
                            {
                                antallMedarbeidere -= 1;
                            }
                        }
                        antallSaker += antallMedarbeidere;

                        int antallInnbyggere = 0;

                        List<FaKlient> klienterInnbyggereBarn = await context.FaKlients
                            .Where(KlientFilter())
                            .Where(KlientKunGyldigeTilsyn())
                            .OrderBy(o => o.KliLoepenr)
                            .ToListAsync();
                        foreach (var klient in klienterInnbyggereBarn)
                        {
                            if (mappings.IsOwner(klient.KliLoepenr) || klient.KliFraannenkommune == 1)
                            {
                                antallInnbyggere += 1;
                            }
                        }
                        antallInnbyggere += antallUtenSak;

                        List<FaForbindelser> rawDataMedarbeidere;
                        List<FaForbindelser> rawDataKlienttilknytninger;
                        rawDataMedarbeidere = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue)).Select(m => m.ForLoepenrNavigation).Distinct().ToListAsync();
                        var rollerInkludert = new string[] { "MOR", "FAR", "SØS", "FMO", "FFA", "FAM", "VRG", "BRH", "BSH", "FSA" };
                        rawDataKlienttilknytninger = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation)
                            .Where(KlientTilknytningFilter())
                            .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                            .Where(f => (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue) || rollerInkludert.Contains(f.KtkRolle))
                            .Select(m => m.ForLoepenrNavigation)
                            .Distinct()
                            .ToListAsync();
                        List<FaForbindelser> forbindelser = new(rawDataKlienttilknytninger);
                        forbindelser.AddRange(rawDataMedarbeidere);
                        forbindelser = forbindelser.Distinct().ToList();

                        antallInnbyggere += forbindelser.Count;

                        int antallDokumenter = 0;
                        SqlConnection connection = new(ConnectionStringFamilia);
                        SqlDataReader reader = null;

                        try
                        {
                            connection.Open();
                            SqlCommand command = new($"Select Count(*) From FA_DOKUMENTER Where DOK_PRODUSERT = 1 AND DOK_DOKUMENT Is Not Null", connection)
                            {
                                CommandTimeout = 300
                            };
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                antallDokumenter = reader.GetInt32(0);
                            }
                            reader.Close();
                        }
                        finally
                        {
                            connection.Close();
                        }
                        information += $"{bydel}: Saker: {antallSaker} Innbyggere (inkl barn): {antallInnbyggere} Dokumenter (ca.): {antallDokumenter}" + Environment.NewLine;
                    }
                }
                string fileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}AntallEntiteterPrBydel.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Telling entiteter pr bydel lagret i filen {fileName}");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                if (ex.InnerException != null)
                {
                    message += $" Inner: {ex.InnerException.Message}";
                }
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Finnes sak i Familia?
        public async Task ExistsInFamiliaAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Sjekker om personer har sak i Familia...");
                string information = "";

                DateTime FirstDayInYear = new(2023, 01, 01);
                string fileName = $"{OutputFolderName}Tilsjekk.txt";
                IEnumerable<string> tochecklist = File.ReadLines(fileName);

                int totalt = tochecklist.Count();
                int index = 0;
                foreach (string tocheck in tochecklist)
                {
                    index += 1;
                    if (!string.IsNullOrEmpty(tocheck))
                    {
                        worker.ReportProgress(0, $"Sjekker {index} av {totalt}...");
                        int year = Convert.ToInt32(tocheck.Substring(4, 2));

                        if (year < 24)
                        {
                            year += 2000;
                        }
                        else
                        {
                            year += 1900;
                        }
                        DateTime fodselsDato = new(year, Convert.ToInt32(tocheck.Substring(2, 2)), Convert.ToInt32(tocheck[..2]));
                        decimal personNummer = Convert.ToDecimal(tocheck[6..]);
                        bool funnetSak = false;
                        foreach (var bydel in mappings.GetAlleBydeler())
                        {
                            Bydelsforkortelse = bydel;
                            ConnectionStringFamilia = mappings.GetConnectionstring(Bydelsforkortelse, MainDBServer);

                            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                            {
                                FaKlient klient = await context.FaKlients
                                    .Where(k => (k.KliFraannenkommune == 0 && k.KliFoedselsdato.HasValue && 
                                    (k.KliFoedselsdato == fodselsDato && k.KliPersonnr == personNummer)) &&
                                    (!k.KliAvsluttetdato.HasValue || k.KliAvsluttetdato >= FirstDayInYear))
                                    .FirstOrDefaultAsync();
                                if (klient != null)
                                {
                                    funnetSak = true;
                                    int antallUndersøkelser = await context.FaUndersoekelsers.Include(d=> d.MelLoepenrNavigation).Where(u => u.MelLoepenrNavigation.KliLoepenr == klient.KliLoepenr).CountAsync();
                                    int antallUndersøkelser2023 = await context.FaUndersoekelsers.Include(d => d.MelLoepenrNavigation).Where(u => (u.UndStartdato.HasValue && u.UndStartdato >= FirstDayInYear) && u.MelLoepenrNavigation.KliLoepenr == klient.KliLoepenr).CountAsync();
                                    string iEllerUtenforHjemmet = "I";
                                    FaTiltak tiltak = await context.FaTiltaks.Where(t => t.KliLoepenr == klient.KliLoepenr).OrderByDescending(o => o.TilIverksattdato).FirstOrDefaultAsync();
                                    if (tiltak != null && tiltak.TilUtenforhjemmet == 1)
                                    {
                                        iEllerUtenforHjemmet = "U";
                                    }
                                    information += $"{tocheck}: Sak i {bydel} I/U: {iEllerUtenforHjemmet} Undersøkelser: {antallUndersøkelser} US i 2023: {antallUndersøkelser2023}" + Environment.NewLine;
                                    break;
                                }
                            }
                        }
                        if (!funnetSak)
                        {
                            information += $"{tocheck}: Sak ikke funnet" + Environment.NewLine;
                        }
                    }
                }
                string resultFileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}StatusSakFamilia.txt";
                await File.WriteAllTextAsync(resultFileName, information);
                worker.ReportProgress(0, $"Ferdig sjekk om personer har sak i Familia, lagret i filen {resultFileName}");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                if (ex.InnerException != null)
                {
                    message += $" Inner: {ex.InnerException.Message}";
                }
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Information Sokrates
        public async Task GetInformationSokratesAsync(BackgroundWorker worker)
        {
            OracleConnection connection = new(ConnectionStringSokrates);
            OracleDataReader reader = null;
            try
            {
                string originalBydelsforkortelse = Bydelsforkortelse;
                worker.ReportProgress(0, "Innhenter informasjon fra Sokrates...");
                string information = "INFORMASJON SOKRATES" + Environment.NewLine + Environment.NewLine;
                worker.ReportProgress(0, "Teller antall rader alle tabeller i Sokrates...");
                information += GetInfoTabellerOracle(ConnectionStringSokrates, SchemaSokrates);
                string fileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}InformasjonSokrates.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Informasjon lagret i filen {fileName}");

                worker.ReportProgress(0, "Sjekker hvilke Familia-saker som ikke finnes i Sokrates...");
                information = "";
                connection.Open();
                foreach (var bydel in mappings.GetAlleBydeler())
                {
                    Bydelsforkortelse = bydel;
                    ConnectionStringFamilia = mappings.GetConnectionstring(Bydelsforkortelse, MainDBServer);
                    List<FaKlient> rawData;
                    int totalAntall = 0;

                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        rawData = await context.FaKlients
                            .Where(KlientFilter())
                            .OrderBy(o => o.KliLoepenr)
                            .Where(k => k.KliFraannenkommune == 0)
                            .ToListAsync();
                        totalAntall = rawData.Count;
                    }
                    int antall = 0;

                    foreach (var klient in rawData)
                    {
                        antall += 1;
                        if (antall % 10 == 0)
                        {
                            worker.ReportProgress(0, $"Sjekker barnevernsaker i {bydel} ({antall} av {totalAntall})...");
                        }
                        string fodselsnummer = klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr;
                        OracleCommand command = new($"Select Client.id from {SchemaSokrates}.Person, {SchemaSokrates}.Client Where Person.Id = Client.Person_id And Person.Personalnumber = '{fodselsnummer}'", connection)
                        {
                            CommandType = System.Data.CommandType.Text
                        };
                        int clientId = 0;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            clientId = reader.GetInt32(0);
                        }
                        if (clientId == 0)
                        {
                            information += $"{bydel}: Klient: {klient.KliLoepenr} Fødselsnummer: {fodselsnummer}" + Environment.NewLine;
                        }
                    }
                }
                fileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}FamiliaIkkeSokrates.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Sjekk hvilke Familia-saker som ikke finnes i Sokrates lagret i filen {fileName}");

                Bydelsforkortelse = originalBydelsforkortelse;
                ConnectionStringFamilia = mappings.GetConnectionstring(Bydelsforkortelse, MainDBServer);
                await ExtractSokratesAsync(worker, false);
                worker.ReportProgress(0, "Sjekker hvilke Familia-saker som har flyttet fra bydelen i Sokrates...");
                information = $"Utflyttet fra bydel {Bydelsforkortelse}" + Environment.NewLine + Environment.NewLine;
                ConnectionStringFamilia = mappings.GetConnectionstring(Bydelsforkortelse, MainDBServer);
                List<FaKlient> klienter;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    klienter = await context.FaKlients
                        .Where(KlientFilter())
                        .OrderBy(o => o.KliLoepenr)
                        .Where(k => k.KliFraannenkommune == 0)
                        .ToListAsync();
                }
                foreach (var klient in klienter)
                {
                    if (!mappings.IsOwner(klient.KliLoepenr))
                    {
                        information += $"{klient.KliLoepenr}" + Environment.NewLine;
                    }
                }
                fileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}FamiliaUtflyttetSokrates.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Sjekk hvilke Familia-saker som har flyttet fra bydelen i Sokrates lagret i filen {fileName}");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                if (ex.InnerException != null)
                {
                    message += $" Inner: {ex.InnerException.Message}";
                }
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

        }
        #endregion

        #region Uttrekk Sokrates
        public async Task SokratesUttrekk(BackgroundWorker worker)
        {
            OracleConnection connection = new(ConnectionStringSokrates);
            OracleDataReader reader = null;
            OracleDataReader historyReader = null;
            string information = "";
            try
            {
                worker.ReportProgress(0, $"Uttrekk informasjon fra Sokrates starter...");
                int antall = 0;
                connection.Open();

                OracleCommand command = new($"Select Client.Office_id, Client.id, Firstname, Lastname, Address, PersonalNumber from {SchemaSokrates}.Person, {SchemaSokrates}.Client Where Person.Id = Client.Person_id", connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Hentet {antall} fra Sokrates...");
                    }
                    int officeId = reader.GetInt32(0);
                    int clientId = reader.GetInt32(1);
                    string firstName = "";
                    if (!reader.IsDBNull(2))
                    {
                        firstName = reader.GetString(2);
                    }
                    string lastName = "";
                    if (!reader.IsDBNull(3))
                    {
                        lastName = reader.GetString(3);
                    }
                    string address = "";
                    if (!reader.IsDBNull(4))
                    {
                        address = reader.GetString(4);
                    }
                    string fødselsNummer = "";
                    if (!reader.IsDBNull(5))
                    {
                        fødselsNummer = reader.GetString(5);
                    }
                    string history = "";

                    OracleCommand historyCommand = new($"Select Office_Id_from, Office_Id_to, DTG from {SchemaSokrates}.transfer_history Where Transfer_Code_id = 12 And Client_id = {clientId} Order by Id", connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    historyReader = historyCommand.ExecuteReader();
                    while (historyReader.Read())
                    {
                        int fromOffice = historyReader.GetInt32(0);
                        int toOffice = historyReader.GetInt32(1);
                        DateTime tilDato = (DateTime)historyReader.GetOracleDate(2);
                        if (!string.IsNullOrEmpty(history))
                        {
                            history += " ";
                        }
                        history += $"{mappings.GetBydelFraOffice(fromOffice)}->{mappings.GetBydelFraOffice(toOffice)} ({tilDato:yyyy-MM-dd})";
                    }
                    information += $"{fødselsNummer}|{mappings.GetBydelFraOffice(officeId)}|{firstName}|{lastName}|{address}|{history}" + Environment.NewLine;
                }
                string resultFileName = $"{OutputFolderName}UttrekkSokrates.txt";
                await File.WriteAllTextAsync(resultFileName, information);
                worker.ReportProgress(0, $"Ferdig uttrekk fra Sokrates, lagret i filen {resultFileName}");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (historyReader != null && !historyReader.IsClosed)
                {
                    historyReader.Close();
                }
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        #endregion

        #region Information JSON
        public void GetJsonInnholdAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter telling innhold i JSON-filene...");

                string result = $"Innhold i JSON-filene under folder {OutputFolderName}:" + Environment.NewLine;
                var folders = from dir in Directory.EnumerateDirectories(OutputFolderName) select dir;

                foreach (string folder in folders)
                {
                    string[] filer = Directory.GetFiles(folder, "*.json", SearchOption.TopDirectoryOnly).ToArray();
                    int count = 0;
                    foreach (string fil in filer)
                    {
                        string jsonString = File.ReadAllText(fil);
                        using (JsonDocument document = JsonDocument.Parse(jsonString))
                        {
                            JsonElement root = document.RootElement;
                            if (root.ValueKind == JsonValueKind.Array)
                            {
                                count += root.GetArrayLength();
                            }
                        }
                    }
                    result += Environment.NewLine + $"{folder}: {count}" ;
                }
                File.WriteAllText($"{OutputFolderName}JSON_{DateTime.Now:yyyyMMdd_HHmm_}.txt", result);
                worker.ReportProgress(0, "Ferdig telling innhold i JSON-filene...");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Organisasjonsnummer sjekk
        public async Task DoOrgNoSjekkAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Start sjekk organisasjonsnummer...");
                string result = "";
                int index = 0;
                string fileName = $"{OutputFolderName}OrgNo.txt";

                IEnumerable<string> lines = File.ReadLines(fileName);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        index += 1;
                        worker.ReportProgress(0, $"Sjekk organisasjonsnummer {index}");

                        using (HttpClient httpClient = new())
                        {
                            try
                            {
                                string apiUrl = $"https://data.brreg.no/enhetsregisteret/api/enheter/{line}";
                                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                                if (response.IsSuccessStatusCode)
                                {
                                    string responseBody = await response.Content.ReadAsStringAsync();
                                    OrganisasjonEnhetsregisteret organisasjonEnhetsregisteret = JsonSerializer.Deserialize<OrganisasjonEnhetsregisteret>(responseBody);

                                    if (organisasjonEnhetsregisteret.slettedato.HasValue)
                                    {
                                        result += $"SLETTET;NEI;;{organisasjonEnhetsregisteret.organisasjonsnummer};{organisasjonEnhetsregisteret.navn}" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        int antallUnderenheter = 0;
                                        string apiUnderenheterUrl = $"https://data.brreg.no/enhetsregisteret/api/underenheter?overordnetEnhet={line}";
                                        HttpResponseMessage responseUnderenheter = await httpClient.GetAsync(apiUnderenheterUrl);

                                        if (responseUnderenheter.IsSuccessStatusCode)
                                        {
                                            string responseBodyUnderenheter = await responseUnderenheter.Content.ReadAsStringAsync();
                                            UnderOrganisasjoner underenheter = JsonSerializer.Deserialize<UnderOrganisasjoner>(responseBodyUnderenheter);
                                            if (underenheter._embedded != null)
                                            {
                                                antallUnderenheter = underenheter._embedded.underenheter.Count;
                                            }
                                        }
                                        if (antallUnderenheter == 0)
                                        {
                                            result += $"AKTIV;NEI;;{organisasjonEnhetsregisteret.organisasjonsnummer};{organisasjonEnhetsregisteret.navn}" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            result += $"AKTIV;JA;https://w2.brreg.no/enhet/sok/underenh.jsp?orgnr={line};{organisasjonEnhetsregisteret.organisasjonsnummer};{organisasjonEnhetsregisteret.navn}" + Environment.NewLine;
                                        }
                                    }
                                }
                                else
                                {
                                    string apiUnderenheterUrl = $"https://data.brreg.no/enhetsregisteret/api/underenheter?organisasjonsnummer={line}";
                                    HttpResponseMessage responseUnderenhet = await httpClient.GetAsync(apiUnderenheterUrl);

                                    if (responseUnderenhet.IsSuccessStatusCode)
                                    {
                                        string responseBodyUnderenhet = await responseUnderenhet.Content.ReadAsStringAsync();
                                        UnderOrganisasjoner underenhet = JsonSerializer.Deserialize<UnderOrganisasjoner>(responseBodyUnderenhet);
                                        if (underenhet._embedded != null)
                                        {
                                            if (underenhet._embedded.underenheter[0].slettedato.HasValue)
                                            {
                                                result += $"SLETTET;NEI;;{line};{underenhet._embedded.underenheter[0].navn}" + Environment.NewLine;
                                            }
                                            else
                                            {
                                                result += $"AKTIV;NEI;;{line};{underenhet._embedded.underenheter[0].navn}" + Environment.NewLine;
                                            }
                                        }
                                        else
                                        {
                                            result += $"AKTIV;NEI;;{line};IKKE FUNNET" + Environment.NewLine;

                                        }
                                    }
                                    else
                                    {
                                        result += $"AKTIV;NEI;{line};;IKKE FUNNET" + Environment.NewLine;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                    }
                }
                File.WriteAllText($"{OutputFolderName}Orgno_{DateTime.Now:yyyyMMdd_HHmm_}.txt", result);
                worker.ReportProgress(0, "Ferdig sjekk organisasjonsnummer...");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public class OrganisasjonEnhetsregisteret
        {
            public string organisasjonsnummer { get; set; }
            public string navn { get; set; }
            public DateOnly? slettedato { get; set; }
        }
        public class UnderOrganisasjoner
        {
            public UnderOrganisasjonerEnhetsregisteret _embedded { get; set; }
        }
        public class UnderOrganisasjonerEnhetsregisteret
        {
            public List<OrganisasjonEnhetsregisteret> underenheter { get; set; }
        }
        #endregion

        #region Translate Between Famila And ModulusBarn
        public void DoTranslateBetweenFamilaAndModulusBarnAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Start mapping saksnummer i Modulus Barn og Id i Familia...");
                string result = "";
                string fileNameRegister = $"{OutputFolderName}Register.txt";
                string fileNameToTranslate = $"{OutputFolderName}Oversette.txt";

                IEnumerable<string> linesRegister = File.ReadLines(fileNameRegister);
                IEnumerable<string> lines = File.ReadLines(fileNameToTranslate);

                NameValueCollection mappings = [];

                int index = 0;
                foreach (string line in linesRegister)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        index += 1;
                        worker.ReportProgress(0, $"Leser inn mapping {index}");
                        string[] verdier = line.Split("|");
                        mappings.Add(verdier[0], verdier[1]);
                    }
                }

                index = 0;
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        index += 1;
                        worker.ReportProgress(0, $"Oversetter {index}");
                        result += line + "|" + mappings[line] + Environment.NewLine;
                    }
                }

                File.WriteAllText($"{OutputFolderName}Mapping_{DateTime.Now:yyyyMMdd_HHmm_}.txt", result);
                worker.ReportProgress(0, "Ferdig oversetting...");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Zip
        public void DoZip(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter zipping av dokumenter...");
                int antallFilerZippet = 0;
                string folderName = $"{OutputFolderName}filer";
                string[] filer = Directory.GetFiles(folderName, "*.*", SearchOption.TopDirectoryOnly).Where(file => !file.ToLower().Contains(".zip")).ToArray();

                if (filer != null && filer.Length > 0)
                {
                    int antallFilerLagtTilDenneZip = 0;
                    ZipArchive zip = null;
                    foreach (string fil in filer)
                    {
                        if (antallFilerZippet % 10 == 0)
                        {
                            worker.ReportProgress(0, $"Zipper filer ({antallFilerZippet} av {filer.Length})...");
                        }

                        if (antallFilerLagtTilDenneZip == 0)
                        {
                            zip = ZipFile.Open($"{OutputFolderName}\\filer\\{Guid.NewGuid().ToString().Replace('-', '_')}.zip", ZipArchiveMode.Create);
                        }
                        zip.CreateEntryFromFile(fil, fil[(fil.LastIndexOf('\\') + 1)..], CompressionLevel.SmallestSize);
                        File.Delete(fil);
                        antallFilerLagtTilDenneZip += 1;
                        antallFilerZippet += 1;
                        if (antallFilerLagtTilDenneZip == AntallFilerPerZip || filer.Length == antallFilerZippet)
                        {
                            zip.Dispose();
                            antallFilerLagtTilDenneZip = 0;
                        }
                    }
                    worker.ReportProgress(0, $"Zippet {filer.Length} dokumenter ferdig.");
                }
                else
                {
                    worker.ReportProgress(0, $"Ingen dokumenter funnet for zipping.");
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Replace in files
        public void DoReplaceInFiles(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter søk/ersatt av dokumenter...");
                int antallFiler = 0;
                string folderName = $"{OutputFolderName}";
                string[] filer = Directory.GetFiles(folderName, "*.json", SearchOption.AllDirectories).ToArray();
                string fileName = $"{OutputFolderName}Ids.txt";

                if (!File.Exists(fileName))
                {
                    MessageBox.Show($"Fil {fileName} eksisterer ikke.", "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                IEnumerable<string> lines = File.ReadLines(fileName);

                if (filer != null && filer.Length > 0)
                {
                    foreach (string fil in filer)
                    {
                        if (antallFiler % 10 == 0)
                        {
                            worker.ReportProgress(0, $"Søk/erstatter filer ({antallFiler} av {filer.Length})...");
                        }

                        string fileContents = File.ReadAllText(fil);

                        foreach (string line in lines)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                string replaceString = "";

                                int pos = line.IndexOf("__");
                                if (pos >= 0)
                                {
                                    replaceString = line[..pos] + "X__" + line[(pos + 2)..];
                                }
                                fileContents = fileContents.Replace(line, replaceString);
                            }
                        }
                        File.WriteAllText(fil, fileContents);
                        antallFiler += 1;
                    }
                }
                worker.ReportProgress(0, $"Erstattet {filer.Length} filer ferdig.");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Aktor Id oversettelse
        public void DoAktorIds(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter oversettelse aktor Id-er...");
                string folderName = $"{OutputFolderName}";
                string fileName = $"{OutputFolderName}BasisIds.txt";
                string resultsFileName = $"{OutputFolderName}ResultBasisIds.txt";
                string results = "";

                if (!File.Exists(fileName))
                {
                    MessageBox.Show($"Fil {fileName} eksisterer ikke.", "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                IEnumerable<string> lines = File.ReadLines(fileName);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        string aktorId = GetStringFromByteArray(line);
                        results += $"{line}   {aktorId}{Environment.NewLine}";
                    }
                }
                File.WriteAllText(resultsFileName, results);
                worker.ReportProgress(0, $"Oversettelse aktor Id-er ferdig.");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Visma Flyt Barnevernvakt
        public async Task<string> GetBVVAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk Visma Flyt Barnevernvakt...");
                int antall = await GetBVVAvdelingsmappingAsync(worker);
                antall += await GetBVVBrukereAsync(worker);
                antall += await GetBVVBarnevernvaktsakerAsync(worker);
                antall += await GetBVVInnbyggereBarnAsync(worker);
                antall += await GetBVVInnbyggereAsync(worker);
                antall += await GetBVVBarnetsNettverkBarnAsync(worker);
                antall += await GetBVVBarnetsNettverkFamilieAsync(worker);
                antall += await GetBVVBarnetsNettverkAsync(worker);
                antall += await GetBVVHenvendelserAsync(worker);
                antall += await GetBVVPostAsync(worker);
                antall += await GetBVVJournalerAsync(worker);
                return $"Antall entiteter Visma Flyt Barnevernvakt: {antall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBVVAvdelingsmappingAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk Visma Flyt Barnevernvakt avdelingsmappinger...");

                List<Avdelingsmapping> avdelingsmappinger = new();
                int antall = 1;

                Avdelingsmapping avdelingsmapping = new()
                {
                    avdelingId = "BVV1",
                    enhetskodeModulusBarn = "BVV1"
                };
                avdelingsmappinger.Add(avdelingsmapping);
                await WriteFileAsync(avdelingsmappinger, GetJsonFileName("avdelingsmapping", "BVVAvdelingsmapping"));
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBVVBrukereAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk Visma Flyt Barnevernvakt saksbehandlere...");
                List<EmployeeEmployee> rawData;
                int totalAntall = 0;

                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    rawData = await context.EmployeeEmployees.Where(b => !b.Email.Contains("system.") && !b.Email.Contains("visma.")).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Bruker> brukere = new();
                int antall = 0;
                foreach (var saksbehandler in rawData)
                {
                    antall += 1;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt saksbehandlere ({antall} av {totalAntall})...");
                    }
                    Bruker bruker = new()
                    {
                        brukerId = AddBydel(saksbehandler.EmployeeEmployeeId.ToString())
                    };
                    SqlConnection connection = new(ConnectionStringMigrering);
                    SqlDataReader reader = null;
                    try
                    {
                        string initialer = saksbehandler.EmployeeEmployeeId.ToString().Trim().ToUpper();
                        connection.Open();
                        SqlCommand command = new($"Select Fornavn,Etternavn,Epost From Brukere Where Upper(Virksomhet)='{Bydelsforkortelse}' And Upper(FamiliaID)='{initialer}'", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (!string.IsNullOrEmpty(reader.GetString(0)))
                                {
                                    bruker.fulltNavn = reader.GetString(0).Trim();
                                }
                                if (!string.IsNullOrEmpty(reader.GetString(1)))
                                {
                                    if (!string.IsNullOrEmpty(bruker.fulltNavn))
                                    {
                                        bruker.fulltNavn += " ";
                                    }
                                    bruker.fulltNavn += reader.GetString(1).Trim();
                                }
                                bruker.email = reader.GetString(2);
                                bruker.brukerNokkelModulusBarn = bruker.email;
                                bruker.brukerId = bruker.email;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(saksbehandler.FirstName))
                            {
                                bruker.fulltNavn = saksbehandler.FirstName?.Trim();
                            }
                            if (!string.IsNullOrEmpty(saksbehandler.MiddleName))
                            {
                                if (!string.IsNullOrEmpty(bruker.fulltNavn))
                                {
                                    bruker.fulltNavn += " ";
                                }
                                bruker.fulltNavn += saksbehandler.MiddleName?.Trim();
                            }
                            if (!string.IsNullOrEmpty(saksbehandler.LastName))
                            {
                                if (!string.IsNullOrEmpty(bruker.fulltNavn))
                                {
                                    bruker.fulltNavn += " ";
                                }
                                bruker.fulltNavn += saksbehandler.LastName?.Trim();
                            }
                            bruker.email = saksbehandler.Email?.Trim();
                            bruker.brukerNokkelModulusBarn = bruker.brukerId;
                        }
                        reader.Close();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    if (!string.IsNullOrEmpty(saksbehandler.FirstName))
                    {
                        bruker.fulltNavn = saksbehandler.FirstName?.Trim();
                    }
                    if (!string.IsNullOrEmpty(saksbehandler.MiddleName))
                    {
                        if (!string.IsNullOrEmpty(bruker.fulltNavn))
                        {
                            bruker.fulltNavn += " ";
                        }
                        bruker.fulltNavn += saksbehandler.MiddleName?.Trim();
                    }
                    if (!string.IsNullOrEmpty(saksbehandler.LastName))
                    {
                        if (!string.IsNullOrEmpty(bruker.fulltNavn))
                        {
                            bruker.fulltNavn += " ";
                        }
                        bruker.fulltNavn += saksbehandler.LastName?.Trim();
                    }
                    bruker.enhetskodeModulusBarnListe.Add("BVV1");
                    brukere.Add(bruker);
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Bruker> brukereDistinct = brukere.GroupBy(c => c.brukerId).Select(s => s.First()).ToList();
                int migrertAntall = brukereDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<Bruker> brukerePart = brukereDistinct.OrderBy(o => o.brukerId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(brukerePart, GetJsonFileName("brukere", $"BVVBrukere{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBVVBarnevernvaktsakerAsync(BackgroundWorker worker)
        {
            try
            {
                int antall = 0;
                List<CaseCase> rawData;
                List<Aktivitet> aktiviteter = new();
                int totalAntall = 0;

                await FillBVVUserNameCacheAsync();

                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    rawData = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).OrderBy(k => k.ClientId).ThenBy(k => k.CaseCaseId).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Sak> saker = new();

                int? lastClientId = 0;

                foreach (var caseCase in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnevernvaktsaker ({antall} av {totalAntall})...");
                    }
                    if (caseCase.ClientId != lastClientId)
                    {
                        Sak sak = new()
                        {
                            sakId = AddBydel(caseCase.Number, "SAK"),
                            aktorId = GetActorId(await GetBVVPersonFromClientAsync(caseCase.ClientId)),
                            avdelingId = "BVV1",
                            arbeidsbelastning = "LAV",
                            sakstype = "BARNEVERNSVAKT"
                        };
                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            DateTime startDato = DateTime.MaxValue;
                            DateTime? sluttDato = null;

                            List<CaseCase> casesThisClient = await context.CaseCases.Where(k => k.ClientId == caseCase.ClientId && k.Type != 2 && k.Type != 3).ToListAsync();
                            foreach (var caseThisClient in casesThisClient)
                            {
                                if (startDato > caseThisClient.CreatedDate)
                                {
                                    startDato = caseThisClient.CreatedDate;
                                }
                                if (sluttDato.HasValue)
                                {
                                    if (sluttDato < caseCase.StatusClosedDate)
                                    {
                                        sluttDato = caseCase.StatusClosedDate;
                                    }
                                }
                                else
                                {
                                    sluttDato = caseCase.StatusClosedDate;
                                }
                            }
                            sak.startDato = startDato;
                            sak.sluttDato = sluttDato;

                            EnquiryEnquiry vfbHenvendelse = await context.EnquiryEnquiries.Where(e => e.ClientId == caseCase.ClientId).OrderBy(k => k.FinishedDate).FirstOrDefaultAsync();
                            if (vfbHenvendelse != null && vfbHenvendelse.FinishedDate.HasValue && vfbHenvendelse.FinishedDate < sak.startDato)
                            {
                                sak.startDato = vfbHenvendelse.FinishedDate;
                            }
                            EnquiryEnquiry vfbHenvendelseReportedDate = await context.EnquiryEnquiries.Where(e => e.ClientId == caseCase.ClientId && !e.FinishedDate.HasValue).OrderBy(k => k.ReportedDate).FirstOrDefaultAsync();
                            if (vfbHenvendelseReportedDate != null && vfbHenvendelseReportedDate.ReportedDate < sak.startDato)
                            {
                                sak.startDato = vfbHenvendelseReportedDate.ReportedDate;
                            }
                            JournalJournal journal = await context.JournalJournals.Where(e => e.ClientId == caseCase.ClientId).OrderBy(k => k.CreatedDate).FirstOrDefaultAsync();
                            if (journal != null && journal.CreatedDate < sak.startDato)
                            {
                                sak.startDato = journal.CreatedDate;
                            }
                            CorrespondenceCorrespondence correspondence = await context.CorrespondenceCorrespondences.Where(e => e.ClientId == caseCase.ClientId).OrderBy(k => k.CreatedDate).FirstOrDefaultAsync();
                            if (correspondence != null && correspondence.CreatedDate < sak.startDato)
                            {
                                sak.startDato = correspondence.CreatedDate;
                            }
                            CorrespondenceCorrespondence correspondenceDate = await context.CorrespondenceCorrespondences.Where(e => e.ClientId == caseCase.ClientId).OrderBy(k => k.CorrespondenceDate).FirstOrDefaultAsync();
                            if (correspondence != null && correspondenceDate.CorrespondenceDate < sak.startDato)
                            {
                                sak.startDato = correspondenceDate.CorrespondenceDate;
                            }
                        }

                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            CaseCase newestCase = await context.CaseCases.Where(k => k.ClientId == caseCase.ClientId && k.Type != 2 && k.Type != 3).OrderByDescending(k => k.CaseCaseId).FirstOrDefaultAsync();

                            if (newestCase.OwnedBy.HasValue)
                            {
                                sak.saksbehandlerId = GetBrukerId(newestCase.OwnedBy.ToString());
                            }
                            else
                            {
                                sak.saksbehandlerId = GetBrukerId(BVVLeder);
                            }
                            List<CaseCasecaseworker> caseWorkers = await context.CaseCasecaseworkers.Where(k => k.CaseId == newestCase.CaseCaseId).ToListAsync();
                            foreach (var caseWorker in caseWorkers)
                            {
                                sak.sekunderSaksbehandlerId.Add(GetBrukerId(caseWorker.CaseworkerId.ToString()));
                            }
                        }
                        if (string.IsNullOrEmpty(sak.saksbehandlerId))
                        {
                            sak.saksbehandlerId = GetBrukerId(BVVLeder);
                        }
                        if (!caseCase.StatusClosedDate.HasValue)
                        {
                            sak.status = "ÅPEN";
                        }
                        else
                        {
                            sak.status = "LUKKET";
                        }
                        Aktivitet aktivitet = new()
                        {
                            aktivitetId = AddBydel(sak.sakId, "LOG"),
                            sakId = sak.sakId,
                            aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)",
                            aktivitetsUnderType = "INGEN",
                            status = "UTFØRT",
                            hendelsesdato = DateTime.Now,
                            saksbehandlerId = GetBrukerId(BVVLeder),
                            tittel = "Logg",
                            utfortAvId = GetBrukerId(BVVLeder),
                            utfortDato = DateTime.Now,
                            notat = "Se dokument"
                        };
                        List<HtmlDocumentToInclude> htmlDocumentsIncluded = new();
                        HtmlDocumentToInclude htmlDocumentToInclude = new()
                        {
                            dokLoepenr = aktivitet.sakId + "LOGG",
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            innhold = await GetBVVLoggAsync(caseCase.ClientId)
                        };
                        htmlDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                        await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, $"Logg{Guid.NewGuid().ToString().Replace('-', '_')}", "Log", false);
                        aktiviteter.Add(aktivitet);
                        saker.Add(sak);
                        lastClientId = caseCase.ClientId;
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int migrertAntall = aktiviteterDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"BVVLogger{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = saker.Count;
                while (migrertAntall > toSkip)
                {
                    List<Sak> sakerPart = saker.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(sakerPart, GetJsonFileName("saker", $"BVVBarnevernvaktsaker{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<string> GetBVVLoggAsync(int? clientId)
        {
            StringBuilder logg = new();

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                List<ActivitylogActivitylog> activitylogs = null;
                List<CaseCase> casesThisClient = await context.CaseCases.Where(k => k.ClientId == clientId && k.Type != 2 && k.Type != 3).ToListAsync();
                foreach (var caseThisClient in casesThisClient)
                {
                    if (activitylogs == null)
                    {
                        activitylogs = await context.ActivitylogActivitylogs.Where(k => k.CaseId == caseThisClient.CaseCaseId).ToListAsync();
                    }
                    else
                    {
                        activitylogs.AddRange(await context.ActivitylogActivitylogs.Where(k => k.CaseId == caseThisClient.CaseCaseId).ToListAsync());
                    }
                }
                activitylogs = activitylogs.OrderByDescending(o => o.CreatedDate).Take(2000).ToList();

                foreach (var activitylog in activitylogs)
                {
                    logg.Append($"<p><b>Dato:&nbsp;</b>{activitylog.CreatedDate.ToLocalTime().ToString("g")}<b>&nbsp;Av:&nbsp;</b>{mappings.GetBVVUsername(activitylog.CreatedBy)}");
                    logg.Append($"<b>&nbsp;({GetBVVOperation(activitylog.OperationType)})</b>");
                    if (!string.IsNullOrEmpty(activitylog.OperationDetails))
                    {
                        logg.Append($"<b>&nbsp;Detaljer:&nbsp;</b>{activitylog.OperationDetails}");
                    }
                    if (!string.IsNullOrEmpty(activitylog.EntityName) && activitylog.EntityName != "-")
                    {
                        logg.Append($"<b>&nbsp;Entitet:&nbsp;</b>{activitylog.EntityName}");
                    }
                    if (!string.IsNullOrEmpty(activitylog.OldValueJson) && activitylog.OldValueJson != "null")
                    {
                        logg.Append($"<b>&nbsp;Før:&nbsp;</b>{activitylog.OldValueJson}");
                    }
                    if (!string.IsNullOrEmpty(activitylog.NewValueJson) && activitylog.NewValueJson != "null")
                    {
                        logg.Append($"<b>&nbsp;Etter:&nbsp;</b>{activitylog.NewValueJson}");
                    }
                    logg.Append("</p>");
                }
            }
            return logg.ToString();
        }
        private static string GetBVVOperation(int operationType)
        {
            string operation = "";
            switch (operationType)
            {
                case 0:
                    operation = "Lest";
                    break;
                case 1:
                    operation = "Opprettet";
                    break;
                case 2:
                    operation = "Oppdatert";
                    break;
                case 3:
                    operation = "Slettet";
                    break;
                case 8:
                    operation = "Flyttet";
                    break;
                case 9:
                    operation = "Vedlegg";
                    break;
                case 11:
                    operation = "Nedlasting";
                    break;
                case 12:
                    operation = "Utskrift";
                    break;
            }
            return operation;
        }
        private async Task FillBVVUserNameCacheAsync()
        {
            string userName = "";
            mappings.ClearBVVUsernameCache();
            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                List<EmployeeEmployee> users = await context.EmployeeEmployees.ToListAsync();
                foreach (var user in users)
                {
                    if (!string.IsNullOrEmpty(user.FirstName))
                    {
                        userName = user.FirstName?.Trim();
                    }
                    if (!string.IsNullOrEmpty(user.MiddleName))
                    {
                        if (!string.IsNullOrEmpty(userName))
                        {
                            userName += " ";
                        }
                        userName += user.MiddleName?.Trim();
                    }
                    if (!string.IsNullOrEmpty(user.LastName))
                    {
                        if (!string.IsNullOrEmpty(userName))
                        {
                            userName += " ";
                        }
                        userName += user.LastName?.Trim();
                    }
                    mappings.AddBVVUsernameCache(user.EmployeeEmployeeId, userName);
                }
            }
        }
        private async Task<PersonPerson> GetBVVPersonFromClientAsync(int? clientId)
        {
            PersonPerson person = null;
            if (clientId.HasValue)
            {
                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    ClientClient client = await context.ClientClients.Where(c => c.ClientClientId == clientId.Value).FirstOrDefaultAsync();
                    if (client != null)
                    {
                        person = await context.PersonPeople.Where(c => c.PersonPersonId == client.PersonId).FirstOrDefaultAsync();
                    }
                }
            }
            return person;
        }
        private async Task<int> GetBVVInnbyggereBarnAsync(BackgroundWorker worker)
        {
            int antall = 0;
            List<PersonPerson> rawData;
            int totalAntall = 0;

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                rawData = await context.PersonPeople.ToListAsync();
                totalAntall = rawData.Count;
            }

            List<Innbygger> innbyggere = new();

            foreach (var person in rawData)
            {
                antall += 1;
                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt innbyggere - barn ({antall} av {totalAntall})...");
                }
                bool included = false;
                int currentClientId = 0;

                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    ClientClient client = await context.ClientClients.Where(c => c.PersonId == person.PersonPersonId).FirstOrDefaultAsync();
                    if (client is not null)
                    {
                        currentClientId = client.ClientClientId;
                        CaseCase caseCase = await context.CaseCases.Where(k => k.ClientId == currentClientId && k.Type != 2 && k.Type != 3).FirstOrDefaultAsync();
                        if (caseCase is not null)
                        {
                            included = true;
                        }
                    }
                }
                if (!included)
                {
                    continue;
                }
                Innbygger innbygger = new()
                {
                    actorId = GetActorId(person),
                    registrertDato = person.StartDate,
                    etternavn = person.LastName?.Trim(),
                    dufNummer = person.Dufnumber,
                    sivilstand = "UOPPGITT",
                    fodselDato = person.Birthdate,
                    fodselsnummer = person.BirthNumber,
                    potensiellOppdragstaker = false,
                    oppdragstaker = false,
                    ikkeAktuellForOppdrag = true,
                    beskrivelse = person.Notes,
                    deaktiver = false
                };
                if (innbygger.fodselsnummer != null)
                {
                    if (innbygger.fodselsnummer.Trim() == "")
                    {
                        innbygger.fodselsnummer = null;
                    }
                }
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    innbygger.fornavn = person.FirstName?.Trim();
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    if (!string.IsNullOrEmpty(innbygger.fornavn))
                    {
                        innbygger.fornavn += " ";
                    }
                    innbygger.fornavn += person.MiddleName?.Trim();
                }
                if (!string.IsNullOrEmpty(innbygger.dufNummer))
                {
                    innbygger.dufNavn = innbygger.fornavn;
                    if (!string.IsNullOrEmpty(innbygger.etternavn))
                    {
                        if (!string.IsNullOrEmpty(innbygger.dufNavn))
                        {
                            innbygger.dufNavn += " ";
                        }
                        innbygger.dufNavn += innbygger.etternavn?.Trim();
                    }
                }
                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    PersonPersoncitizenshipnationality nasjonalitet = await context.PersonPersoncitizenshipnationalities.Where(c => c.PersonId == person.PersonPersonId).FirstOrDefaultAsync();
                    if (nasjonalitet is not null)
                    {
                        innbygger.statsborger = await GetBVVBaseregistryValueAsync(nasjonalitet.CitizenshipNationalityRegistryId);
                    }
                }
                innbygger.morsmal = await GetBVVBaseregistryValueAsync(person.LanguageRegistryId);
                if (person.Gender == 0)
                {
                    innbygger.kjonn = "MANN";
                }
                else
                {
                    if (person.Gender == 1)
                    {
                        innbygger.kjonn = "KVINNE";
                    }
                }
                if (string.IsNullOrEmpty(innbygger.kjonn) && !string.IsNullOrEmpty(innbygger.fodselsnummer))
                {
                    int kjonnsTall = int.Parse(innbygger.fodselsnummer[8].ToString());
                    if (kjonnsTall % 2 == 0)
                    {
                        innbygger.kjonn = "KVINNE";
                    }
                    else
                    {
                        innbygger.kjonn = "MANN";
                    }
                }
                if (string.IsNullOrEmpty(innbygger.kjonn))
                {
                    var random = new Random();
                    int number = random.Next(-1, 1);
                    if (number > 0)
                    {
                        innbygger.kjonn = "KVINNE";
                    }
                    else
                    {
                        innbygger.kjonn = "MANN";
                    }
                }
                bool hovetelefonBestemt = false;
                if (!string.IsNullOrEmpty(person.Phone))
                {
                    AktørTelefonnummer aktørTelefonnummerMobil = new()
                    {
                        telefonnummerType = "PRIVAT",
                        telefonnummer = GetTelefonnummer(person.PhonePrefix?.Trim() + person.Phone?.Trim()),
                        hovedtelefon = true
                    };
                    hovetelefonBestemt = true;
                    innbygger.telefonnummer.Add(aktørTelefonnummerMobil);
                }
                if (!string.IsNullOrEmpty(person.SecondaryPhone))
                {
                    AktørTelefonnummer aktørTelefonnummerPrivat = new()
                    {
                        telefonnummerType = "ANNET",
                        telefonnummer = GetTelefonnummer(person.SecondaryPhonePrefix?.Trim() + person.SecondaryPhone?.Trim())
                    };
                    if (!hovetelefonBestemt)
                    {
                        aktørTelefonnummerPrivat.hovedtelefon = true;
                    }
                    innbygger.telefonnummer.Add(aktørTelefonnummerPrivat);
                }
                if (!string.IsNullOrEmpty(person.Address) || person.PostCodeRegistryId.HasValue || person.MunicipalityRegistryId.HasValue)
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(person.PersonPersonId.ToString(), "1"),
                        adresseType = "BOSTEDSADRESSE",
                        linje1 = person.Address?.Trim(),
                        postnummer = await GetBVVBaseregistryValueAsync(person.PostCodeRegistryId, true),
                        poststed = await GetBVVBaseregistryValueAsync(person.MunicipalityRegistryId),
                        hovedadresse = false
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    innbygger.adresse.Add(adresse);
                }
                if (!string.IsNullOrEmpty(person.VisitingAddress) || person.VisitingAddressPostCodeRegistryId.HasValue || person.VisitingAddressMunicipalityRegistryId.HasValue)
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(person.PersonPersonId.ToString(), "2"),
                        adresseType = "OPPHOLDSADRESSE",
                        linje1 = person.VisitingAddress?.Trim(),
                        postnummer = await GetBVVBaseregistryValueAsync(person.VisitingAddressPostCodeRegistryId, true),
                        poststed = await GetBVVBaseregistryValueAsync(person.VisitingAddressMunicipalityRegistryId)
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    innbygger.adresse.Add(adresse);
                }
                if (person.IsSecretAddress || person.IsHighlyRestricted || person.IsSecretVisitingAddress)
                {
                    innbygger.adressesperre = "SKJULT_ADRESSE";
                }
                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    ClientClientclientgroup clientgrouplink = await context.ClientClientclientgroups.Where(c => c.ClientId == currentClientId && c.ClientClientClientGroupId == 4).FirstOrDefaultAsync();
                    if (clientgrouplink is not null)
                    {
                        innbygger.ensligMindreaarig = true;
                    }
                }
                if (!string.IsNullOrEmpty(person.Hnumber))
                {
                    innbygger.fodselsnummer = null;
                    innbygger.dufNummer = null;
                    innbygger.dufNavn = null;
                }
                innbyggere.Add(innbygger);
            }
            int toSkip = 0;
            int fileNumber = 1;
            List<Innbygger> innbyggerDistinct = innbyggere.GroupBy(c => c.actorId).Select(s => s.First()).ToList();
            int migrertAntall = innbyggerDistinct.Count;
            while (migrertAntall > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggerDistinct.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(innbyggerePart, GetJsonFileName("innbygger", $"BVVInnbyggereBarn{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return antall;
        }
        private async Task<int> GetBVVInnbyggereAsync(BackgroundWorker worker)
        {
            int antall = 0;
            List<PersonPerson> rawData;
            int totalAntall = 0;

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                rawData = await context.PersonPeople.Where(i => string.IsNullOrEmpty(i.Hnumber)).ToListAsync();
                totalAntall = rawData.Count;
            }

            List<Innbygger> innbyggere = new();

            foreach (var person in rawData)
            {
                antall += 1;
                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt innbyggere ({antall} av {totalAntall})...");
                }
                bool included = false;
                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    ClientClient client = await context.ClientClients.Where(c => c.PersonId == person.PersonPersonId).FirstOrDefaultAsync();
                    if (client != null)
                    {
                        CaseCase caseCase = await context.CaseCases.Where(k => k.ClientId == client.ClientClientId && k.Type != 2 && k.Type != 3).FirstOrDefaultAsync();
                        if (caseCase != null)
                        {
                            continue;
                        }
                    }
                    List<PersonPersonrole> relations = await context.PersonPersonroles.Where(k => k.PersonId == person.PersonPersonId).ToListAsync();
                    foreach (var relation in relations)
                    {
                        List<ClientClient> clientConnections = await context.ClientClients.Where(k => k.PersonId == relation.RelatedPersonId).ToListAsync();
                        foreach (var clientConnection in clientConnections)
                        {
                            CaseCase caseCase = await context.CaseCases.Where(k => k.ClientId == clientConnection.ClientClientId && k.Type != 2 && k.Type != 3).FirstOrDefaultAsync();
                            if (caseCase is not null)
                            {
                                included = true;
                                break;
                            }
                        }
                        if (included)
                        {
                            break;
                        }
                    }
                    if (!included)
                    {
                        PersonNetworkpersonrole personNetworkpersonroles = await context.PersonNetworkpersonroles.Where(c => c.PersonId == person.PersonPersonId).FirstOrDefaultAsync();
                        if (personNetworkpersonroles is not null)
                        {
                            included = true;
                        }
                    }
                }
                if (!included)
                {
                    continue;
                }
                Innbygger innbygger = new()
                {
                    actorId = GetActorId(person),
                    registrertDato = person.StartDate,
                    etternavn = person.LastName?.Trim(),
                    dufNummer = person.Dufnumber,
                    sivilstand = "UOPPGITT",
                    fodselDato = person.Birthdate,
                    fodselsnummer = person.BirthNumber,
                    potensiellOppdragstaker = false,
                    oppdragstaker = false,
                    ikkeAktuellForOppdrag = true,
                    beskrivelse = person.Notes,
                    deaktiver = false
                };
                if (innbygger.fodselsnummer != null)
                {
                    if (innbygger.fodselsnummer.Trim() == "")
                    {
                        innbygger.fodselsnummer = null;
                    }
                }
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    innbygger.fornavn = person.FirstName?.Trim();
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    if (!string.IsNullOrEmpty(innbygger.fornavn))
                    {
                        innbygger.fornavn += " ";
                    }
                    innbygger.fornavn += person.MiddleName?.Trim();
                }
                if (!string.IsNullOrEmpty(innbygger.dufNummer))
                {
                    innbygger.dufNavn = innbygger.fornavn;
                    if (!string.IsNullOrEmpty(innbygger.etternavn))
                    {
                        if (!string.IsNullOrEmpty(innbygger.dufNavn))
                        {
                            innbygger.dufNavn += " ";
                        }
                        innbygger.dufNavn += innbygger.etternavn?.Trim();
                    }
                }
                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    PersonPersoncitizenshipnationality nasjonalitet = await context.PersonPersoncitizenshipnationalities.Where(c => c.PersonId == person.PersonPersonId).FirstOrDefaultAsync();
                    if (nasjonalitet is not null)
                    {
                        innbygger.statsborger = await GetBVVBaseregistryValueAsync(nasjonalitet.CitizenshipNationalityRegistryId);
                    }
                }
                innbygger.morsmal = await GetBVVBaseregistryValueAsync(person.LanguageRegistryId);
                if (person.Gender == 0)
                {
                    innbygger.kjonn = "MANN";
                }
                else
                {
                    if (person.Gender == 1)
                    {
                        innbygger.kjonn = "KVINNE";
                    }
                }
                bool hovetelefonBestemt = false;
                if (!string.IsNullOrEmpty(person.Phone))
                {
                    AktørTelefonnummer aktørTelefonnummerMobil = new()
                    {
                        telefonnummerType = "PRIVAT",
                        telefonnummer = GetTelefonnummer(person.PhonePrefix?.Trim() + person.Phone?.Trim()),
                        hovedtelefon = true
                    };
                    hovetelefonBestemt = true;
                    innbygger.telefonnummer.Add(aktørTelefonnummerMobil);
                }
                if (!string.IsNullOrEmpty(person.SecondaryPhone))
                {
                    AktørTelefonnummer aktørTelefonnummerPrivat = new()
                    {
                        telefonnummerType = "ANNET",
                        telefonnummer = GetTelefonnummer(person.SecondaryPhonePrefix?.Trim() + person.SecondaryPhone?.Trim())
                    };
                    if (!hovetelefonBestemt)
                    {
                        aktørTelefonnummerPrivat.hovedtelefon = true;
                    }
                    innbygger.telefonnummer.Add(aktørTelefonnummerPrivat);
                }
                if (!string.IsNullOrEmpty(person.Address) || person.PostCodeRegistryId.HasValue || person.MunicipalityRegistryId.HasValue)
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(person.PersonPersonId.ToString(), "FOR__1"),
                        adresseType = "BOSTEDSADRESSE",
                        linje1 = person.Address?.Trim(),
                        postnummer = await GetBVVBaseregistryValueAsync(person.PostCodeRegistryId, true),
                        poststed = await GetBVVBaseregistryValueAsync(person.MunicipalityRegistryId),
                        hovedadresse = false
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    innbygger.adresse.Add(adresse);
                }
                if (!string.IsNullOrEmpty(person.VisitingAddress) || person.VisitingAddressPostCodeRegistryId.HasValue || person.VisitingAddressMunicipalityRegistryId.HasValue)
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(person.PersonPersonId.ToString(), "FOR__2"),
                        adresseType = "OPPHOLDSADRESSE",
                        linje1 = person.VisitingAddress?.Trim(),
                        postnummer = await GetBVVBaseregistryValueAsync(person.VisitingAddressPostCodeRegistryId, true),
                        poststed = await GetBVVBaseregistryValueAsync(person.VisitingAddressMunicipalityRegistryId)
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    innbygger.adresse.Add(adresse);
                }
                if (person.IsSecretAddress || person.IsHighlyRestricted || person.IsSecretVisitingAddress)
                {
                    innbygger.adressesperre = "SKJULT_ADRESSE";
                }
                if (string.IsNullOrEmpty(person.BirthNumber) && string.IsNullOrEmpty(person.Dufnumber))
                {
                    innbygger.ukjentPerson = true;
                }
                innbyggere.Add(innbygger);
            }
            int toSkip = 0;
            int fileNumber = 1;
            List<Innbygger> innbyggerDistinct = innbyggere.GroupBy(c => c.actorId).Select(s => s.First()).ToList();
            int migrertAntall = innbyggerDistinct.Count;
            while (migrertAntall > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggerDistinct.OrderBy(v => v.actorId).OrderBy(w => w.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(innbyggerePart, GetJsonFileName("innbygger", $"BVVInnbyggere{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return antall;
        }
        private async Task<int> GetBVVBarnetsNettverkBarnAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk Visma Flyt Barnevernvakt barnets nettverk - barnet...");
                List<CaseCase> rawData;
                int totalAntall = 0;

                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    rawData = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).OrderBy(k => k.ClientId).ThenBy(k => k.CaseCaseId).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<BarnetsNettverk> barnetsNettverk = new();
                int antall = 0;
                int? lastClientId = 0;

                foreach (var caseCase in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnets nettverk - barnet ({antall} av {totalAntall})...");
                    }
                    if (lastClientId != caseCase.ClientId)
                    {
                        BarnetsNettverk forbindelse = new()
                        {
                            sakId = AddBydel(caseCase.Number, "SAK"),
                            actorId = GetActorId(await GetBVVPersonFromClientAsync(caseCase.ClientId)),
                            relasjonTilSak = "HOVEDPERSON",
                            rolle = "HOVEDPERSON",
                            foresatt = false
                        };
                        barnetsNettverk.Add(forbindelse);
                        lastClientId = caseCase.ClientId;
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<BarnetsNettverk> barnetsNettverkDistinct = barnetsNettverk.GroupBy(c => new { c.sakId, c.actorId }).Select(s => s.First()).ToList();
                int migrertAntall = barnetsNettverkDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<BarnetsNettverk> barnetsNettverkPart = barnetsNettverkDistinct.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(barnetsNettverkPart, GetJsonFileName("barnetsNettverk", $"BVVBarnetsNettverkBarn{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBVVBarnetsNettverkFamilieAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk Visma Flyt Barnevernvakt barnets nettverk - familie...");
                List<CaseCase> rawCases;
                int totalAntall = 0;

                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    rawCases = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).OrderBy(k => k.ClientId).ThenBy(k => k.CaseCaseId).ToListAsync();
                    totalAntall = rawCases.Count;
                }
                List<BarnetsNettverk> barnetsNettverk = new();
                int antall = 0;
                int? lastClientId = 0;

                foreach (var caseCase in rawCases)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnets nettverk - familie ({antall} av {totalAntall})...");
                    }
                    if (lastClientId != caseCase.ClientId)
                    {
                        int childrenPersonId = (await GetBVVPersonFromClientAsync(caseCase.ClientId)).PersonPersonId;

                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            List<PersonPersonrole> rawRelations = await context.PersonPersonroles.Where(k => k.RelatedPersonId == childrenPersonId).ToListAsync();
                            foreach (var relation in rawRelations)
                            {
                                PersonPerson person = await context.PersonPeople.Where(i => i.PersonPersonId == relation.PersonId && string.IsNullOrEmpty(i.Hnumber)).FirstOrDefaultAsync();
                                if (person != null)
                                {
                                    BarnetsNettverk forbindelse = new()
                                    {
                                        sakId = AddBydel(caseCase.Number, "SAK"),
                                        actorId = await GetActorIdAsync(relation.PersonId),
                                        relasjonTilSak = Mappings.GetBVVRelasjonSak(relation.FamilyroleRegistryId),
                                        rolle = Mappings.GetBVVRolleSak(relation.FamilyroleRegistryId),
                                        kommentar = relation.Notes
                                    };
                                    if (relation.HasParentalRights.HasValue && relation.HasParentalRights.Value)
                                    {
                                        forbindelse.foresatt = true;
                                    }
                                    barnetsNettverk.Add(forbindelse);
                                }
                            }
                        }
                        lastClientId = caseCase.ClientId;
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<BarnetsNettverk> barnetsNettverkDistinct = barnetsNettverk.GroupBy(c => new { c.sakId, c.actorId }).Select(s => s.First()).ToList();
                int migrertAntall = barnetsNettverkDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<BarnetsNettverk> barnetsNettverkDistinctPart = barnetsNettverkDistinct.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(barnetsNettverkDistinctPart, GetJsonFileName("barnetsNettverk", $"BVVBarnetsNettverkFamilie{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBVVBarnetsNettverkAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk Visma Flyt Barnevernvakt barnets nettverk...");
                List<CaseCase> rawCases;
                int totalAntall = 0;

                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    rawCases = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).OrderBy(k => k.ClientId).ThenBy(k => k.CaseCaseId).ToListAsync();
                    totalAntall = rawCases.Count;
                }
                List<BarnetsNettverk> barnetsNettverk = new();
                int antall = 0;
                int? lastClientId = 0;

                foreach (var caseCase in rawCases)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnets nettverk ({antall} av {totalAntall})...");
                    }
                    if (lastClientId != caseCase.ClientId)
                    {
                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            List<PersonNetworkpersonrole> rawRelations = await context.PersonNetworkpersonroles.Where(k => k.RelatedClientId == caseCase.ClientId).ToListAsync();
                            foreach (var relation in rawRelations)
                            {
                                PersonPerson person = await context.PersonPeople.Where(i => i.PersonPersonId == relation.PersonId && string.IsNullOrEmpty(i.Hnumber)).FirstOrDefaultAsync();
                                if (person != null)
                                {
                                    BarnetsNettverk forbindelse = new()
                                    {
                                        sakId = AddBydel(caseCase.Number, "SAK"),
                                        actorId = await GetActorIdAsync(relation.PersonId),
                                        relasjonTilSak = Mappings.GetBVVRelasjonSak(relation.RoleRegistryId),
                                        rolle = Mappings.GetBVVRolleSak(relation.RoleRegistryId),
                                        foresatt = false,
                                        kommentar = relation.Notes
                                    };
                                    barnetsNettverk.Add(forbindelse);
                                }
                            }
                        }
                        lastClientId = caseCase.ClientId;
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<BarnetsNettverk> barnetsNettverkDistinct = barnetsNettverk.GroupBy(c => new { c.sakId, c.actorId }).Select(s => s.First()).ToList();
                int migrertAntall = barnetsNettverkDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<BarnetsNettverk> barnetsNettverkPart = barnetsNettverkDistinct.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(barnetsNettverkPart, GetJsonFileName("barnetsNettverk", $"BVVBarnetsNettverk{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBVVHenvendelserAsync(BackgroundWorker worker)
        {
            List<EnquiryDocument> rawEnquiryDocuments;
            List<Document> documents = new();
            List<HtmlDocumentToInclude> htmlDocumentsIncluded = new();

            int totalAntall = 0;

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                rawEnquiryDocuments = await context.EnquiryDocuments.ToListAsync();
                totalAntall = rawEnquiryDocuments.Count;
            }
            List<Henvendelse> henvendelser = new();
            int antall = 0;
            SqlConnection connection = new(ConnectionStringVFB);
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                foreach (var enquiryDocument in rawEnquiryDocuments)
                {
                    antall += 1;
                    int? caseId = null;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt henvendelser ({antall} av {totalAntall})...");
                    }
                    EnquiryEnquiry vfbHenvendelse;
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        vfbHenvendelse = await context.EnquiryEnquiries.Where(e => e.EnquiryEnquiryId == enquiryDocument.EnquiryId).FirstOrDefaultAsync();
                        if (!vfbHenvendelse.CaseId.HasValue)
                        {
                            if (!vfbHenvendelse.ClientId.HasValue)
                            {
                                continue;
                            }
                            CaseCase singleCase = await context.CaseCases.Where(e => e.ClientId == vfbHenvendelse.ClientId && e.Type == 1).OrderBy(k => k.CaseCaseId).FirstOrDefaultAsync();
                            if (singleCase == null)
                            {
                                continue;
                            }
                            caseId = singleCase.CaseCaseId;
                        }
                        else
                        {
                            int numberOfCases = await context.CaseCases.Where(e => e.CaseCaseId == vfbHenvendelse.CaseId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            caseId = vfbHenvendelse.CaseId;
                        }
                    }
                    Henvendelse henvendelse = new()
                    {
                        // Skal stå blank: henvendelseKommuneBydel, henvendelseMelderPartId
                        aktivitetId = AddBydel(enquiryDocument.EnquiryDocumentId.ToString(), "HEN"),
                        status = "UTFØRT",
                        utfortAvId = GetBrukerId(vfbHenvendelse.CreatedBy.ToString()),
                        henvendelsesDato = vfbHenvendelse.FinishedDate,
                        henvendelseInnhold = "Se dokument",
                        henvendelseKreverUmiddelbarInngripen = false,
                        henvendelseKommunenummer = "9999",
                        henvendelseMottaksmåte = "06_HENVENDELSE_MOTTAKSMÅTE_UKJENT",
                        henvendelseMelderPartErAnonym = true
                    };

                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        EnquiryEnquiry enquiry = await context.EnquiryEnquiries.Where(k => k.EnquiryEnquiryId == enquiryDocument.EnquiryId).FirstOrDefaultAsync();
                        if (enquiry != null)
                        {
                            henvendelse.henvendelseMelderType = mappings.GetBVVMelder(enquiry.ReporterTypeRegistryId);
                            CaseCase caseCase = await context.CaseCases.Where(k => k.CaseCaseId == enquiry.CaseId).FirstOrDefaultAsync();
                            if (caseCase != null)
                            {
                                henvendelse.henvendelseKategori = mappings.GetBVVHovedkategori(caseCase.MainCategoryRegistryId);
                                henvendelse.aktivitetsUndertype = mappings.GetBVVTypeBarnevernsvaktsak(caseCase.PersoncaseTypeRegistryId);
                            }
                        }
                        if (string.IsNullOrEmpty(henvendelse.henvendelseMelderType))
                        {
                            henvendelse.henvendelseMelderType = "31_HENVENDELSE_MELDERTYPE_ANDRE";
                        }
                        if (string.IsNullOrEmpty(henvendelse.henvendelseKategori))
                        {
                            henvendelse.henvendelseKategori = "24_ANNET";
                        }
                    }
                    if (!henvendelse.henvendelsesDato.HasValue)
                    {
                        henvendelse.henvendelsesDato = vfbHenvendelse.ReportedDate;
                    }
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        CaseCase rawCase = await context.CaseCases.Where(e => e.CaseCaseId == caseId).FirstOrDefaultAsync();
                        henvendelse.sakId = AddBydel(await GetBVVFirstCaseNumberAsync(rawCase.ClientId), "SAK");
                    }
                    SqlCommand command = new($"Select FileDataBlob From Enquiry_Document Where Enquiry_DocumentId={enquiryDocument.EnquiryDocumentId} And FileDataBlob Is Not Null", connection)
                    {
                        CommandTimeout = 300
                    };
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Document document = new()
                        {
                            dokumentId = henvendelse.aktivitetId,
                            filId = henvendelse.aktivitetId,
                            ferdigstilt = true,
                            opprettetAvId = henvendelse.utfortAvId,
                            sakId = henvendelse.sakId,
                            tittel = vfbHenvendelse.Subject,
                            journalDato = henvendelse.henvendelsesDato,
                            filFormat = "PDF"
                        };
                        document.aktivitetIdListe.Add(henvendelse.aktivitetId);
                        document.filId += ".pdf";
                        documents.Add(document);

                        SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                        SqlDataReader readerMigrering = null;
                        try
                        {
                            connectionMigrering.Open();
                            bool fileAlreadyWritten = false;
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{document.filId}'", connectionMigrering)
                            {
                                CommandTimeout = 300
                            };
                            readerMigrering = commandMigrering.ExecuteReader();
                            while (readerMigrering.Read())
                            {
                                if (readerMigrering.GetInt32(0) > 0)
                                {
                                    fileAlreadyWritten = true;
                                }
                            }
                            readerMigrering.Close();

                            if (!fileAlreadyWritten)
                            {
                                while (reader.Read())
                                {
                                    await File.WriteAllBytesAsync(OutputFolderName + $"filer\\" + document.filId, (byte[])reader["FileDataBlob"]);
                                }
                                commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                {
                                    CommandTimeout = 300
                                };
                                commandMigrering.ExecuteNonQuery();
                            }
                        }
                        finally
                        {
                            connectionMigrering.Close();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(enquiryDocument.DocumentText))
                        {
                            HtmlDocumentToInclude htmlDocumentToInclude = new()
                            {
                                dokLoepenr = enquiryDocument.EnquiryDocumentId.ToString(),
                                sakId = henvendelse.sakId,
                                tittel = vfbHenvendelse.Subject,
                                journalDato = henvendelse.henvendelsesDato,
                                opprettetAvId = henvendelse.utfortAvId,
                                innhold = enquiryDocument.DocumentText
                            };
                            htmlDocumentToInclude.aktivitetIdListe.Add(henvendelse.aktivitetId);
                            htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                        }
                    }
                    reader.Close();
                    henvendelser.Add(henvendelse);
                }
            }
            finally
            {
                connection.Close();
            }
            await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, "Henvendelser", "Hen");
            int toSkip = 0;
            int fileNumber = 1;
            List<Document> documentsDistinct = documents.GroupBy(c => new { c.dokumentId, c.sakId }).Select(s => s.First()).ToList();
            int migrertAntall = documentsDistinct.Count;
            while (migrertAntall > toSkip)
            {
                List<Document> documentsPart = documentsDistinct.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"DokumenterBVVHen{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            toSkip = 0;
            fileNumber = 1;
            migrertAntall = henvendelser.Count;
            while (migrertAntall > toSkip)
            {
                List<Henvendelse> henvendelsePart = henvendelser.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(henvendelsePart, GetJsonFileName("henvendelse", $"BVVHenvendelser{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return antall;
        }
        private async Task<int> GetBVVJournalerAsync(BackgroundWorker worker)
        {
            List<JournalDocument> rawJournalDocuments;
            List<Document> documents = new();
            List<HtmlDocumentToInclude> htmlDocumentsIncluded = new();

            int totalAntall = 0;

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                rawJournalDocuments = await context.JournalDocuments.ToListAsync();
                totalAntall = rawJournalDocuments.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            SqlConnection connection = new(ConnectionStringVFB);
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                foreach (var journalDocument in rawJournalDocuments)
                {
                    antall += 1;
                    int? caseId = null;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt journaler ({antall} av {totalAntall})...");
                    }
                    JournalJournal journal;
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        journal = await context.JournalJournals.Where(e => e.JournalJournalId == journalDocument.JournalId).FirstOrDefaultAsync();
                        if (!journal.CaseId.HasValue)
                        {
                            if (!journal.ClientId.HasValue)
                            {
                                continue;
                            }
                            CaseCase singleCase = await context.CaseCases.Where(e => e.ClientId == journal.ClientId && e.Type == 1).OrderBy(k => k.CaseCaseId).FirstOrDefaultAsync();
                            if (singleCase == null)
                            {
                                continue;
                            }
                            caseId = singleCase.CaseCaseId;
                        }
                        else
                        {
                            int numberOfCases = await context.CaseCases.Where(e => e.CaseCaseId == journal.CaseId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            caseId = journal.CaseId;
                        }
                    }
                    Aktivitet aktivitet = new()
                    {
                        aktivitetId = AddBydel(journalDocument.JournalDocumentId.ToString(), "JOU"),
                        aktivitetsType = "BARNEVERNSVAKT",
                        aktivitetsUnderType = mappings.GetBVVJournalCategory(journal.JournalCategoryRegistryId),
                        status = "UTFØRT",
                        hendelsesdato = journal.CreatedDate,
                        saksbehandlerId = GetBrukerId(journal.OwnedBy.ToString()),
                        tittel = journal.Title,
                        utfortAvId = GetBrukerId(journal.OwnedBy.ToString()),
                        utfortDato = journal.FinishedDate,
                        notat = "Se dokument"
                    };
                    if (!aktivitet.utfortDato.HasValue)
                    {
                        aktivitet.utfortDato = aktivitet.hendelsesdato;
                    }
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        CaseCase rawCase = await context.CaseCases.Where(e => e.CaseCaseId == caseId).FirstOrDefaultAsync();
                        aktivitet.sakId = AddBydel(await GetBVVFirstCaseNumberAsync(rawCase.ClientId), "SAK");
                    }
                    SqlCommand command = new($"Select FileDataBlob From journal_document Where Journal_DocumentId={journalDocument.JournalDocumentId} And FileDataBlob Is Not Null", connection)
                    {
                        CommandTimeout = 300
                    };
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Document document = new()
                        {
                            dokumentId = aktivitet.aktivitetId,
                            filId = aktivitet.aktivitetId,
                            ferdigstilt = true,
                            opprettetAvId = aktivitet.utfortAvId,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            filFormat = "PDF"
                        };
                        document.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        document.filId += ".pdf";
                        documents.Add(document);

                        SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                        SqlDataReader readerMigrering = null;
                        try
                        {
                            connectionMigrering.Open();
                            bool fileAlreadyWritten = false;
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{document.filId}'", connectionMigrering)
                            {
                                CommandTimeout = 300
                            };
                            readerMigrering = commandMigrering.ExecuteReader();
                            while (readerMigrering.Read())
                            {
                                if (readerMigrering.GetInt32(0) > 0)
                                {
                                    fileAlreadyWritten = true;
                                }
                            }
                            readerMigrering.Close();

                            if (!fileAlreadyWritten)
                            {
                                while (reader.Read())
                                {
                                    await File.WriteAllBytesAsync(OutputFolderName + $"filer\\" + document.filId, (byte[])reader["FileDataBlob"]);
                                }
                                commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                {
                                    CommandTimeout = 300
                                };
                                commandMigrering.ExecuteNonQuery();
                            }
                        }
                        finally
                        {
                            connectionMigrering.Close();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(journalDocument.DocumentText))
                        {
                            HtmlDocumentToInclude htmlDocumentToInclude = new()
                            {
                                dokLoepenr = journalDocument.JournalDocumentId.ToString(),
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato,
                                opprettetAvId = aktivitet.utfortAvId,
                                innhold = journalDocument.DocumentText
                            };
                            htmlDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                            htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                        }
                    }
                    reader.Close();
                    aktiviteter.Add(aktivitet);
                }
            }
            finally
            {
                connection.Close();
            }
            await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, "Journaler", "Jou");
            int toSkip = 0;
            int fileNumber = 1;
            List<Document> documentsDistinct = documents.GroupBy(c => new { c.dokumentId, c.sakId }).Select(s => s.First()).ToList();
            int migrertAntall = documentsDistinct.Count;
            while (migrertAntall > toSkip)
            {
                List<Document> documentsPart = documentsDistinct.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"DokumenterBVVJou{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            toSkip = 0;
            fileNumber = 1;
            migrertAntall = aktiviteter.Count;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"BVVJournaler{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return antall;
        }
        private async Task<int> GetBVVPostAsync(BackgroundWorker worker)
        {
            List<CorrespondenceDocument> rawCorrespondenceDocuments;
            List<Document> documents = new();
            List<HtmlDocumentToInclude> htmlDocumentsIncluded = new();

            int totalAntall = 0;

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                rawCorrespondenceDocuments = await context.CorrespondenceDocuments.ToListAsync();
                totalAntall = rawCorrespondenceDocuments.Count;
            }
            List<Aktivitet> aktiviteter = new();
            List<Vedtak> vedtaksListe = new();
            int antall = 0;
            SqlConnection connection = new(ConnectionStringVFB);
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                foreach (var correspondenceDocument in rawCorrespondenceDocuments)
                {
                    antall += 1;
                    int? caseId = null;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt post ({antall} av {totalAntall})...");
                    }
                    CorrespondenceCorrespondence correspondence;
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        correspondence = await context.CorrespondenceCorrespondences.Where(e => e.CorrespondenceCorrespondenceId == correspondenceDocument.CorrespondenceId).FirstOrDefaultAsync();
                        if (!correspondence.CaseId.HasValue)
                        {
                            if (!correspondence.ClientId.HasValue)
                            {
                                continue;
                            }
                            CaseCase singleCase = await context.CaseCases.Where(e => e.ClientId == correspondence.ClientId && e.Type == 1).OrderBy(k => k.CaseCaseId).FirstOrDefaultAsync();
                            if (singleCase == null)
                            {
                                continue;
                            }
                            caseId = singleCase.CaseCaseId;
                        }
                        else
                        {
                            int numberOfCases = await context.CaseCases.Where(e => e.CaseCaseId == correspondence.CaseId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            caseId = correspondence.CaseId;
                        }
                    }
                    string aktivitetsUnderType = mappings.GetBVVCorrespondenceCategory(correspondence.CorrespondenceCategoryRegistryId);

                    if (aktivitetsUnderType == "VEDTAK_AKUTT")
                    {
                        Vedtak vedtak = new()
                        {
                            aktivitetId = AddBydel(correspondenceDocument.CorrespondenceDocumentId.ToString(), "VED"),
                            aktivitetsUndertype = "BARNEVERNSVAKT_VEDTAK_AKUTT",
                            status = "UTFØRT",
                            tittel = correspondence.Title,
                            vedtakFraBarnevernsvakt = true,
                            vedtaksdato = correspondence.CreatedDate,
                            startdato = correspondence.CreatedDate,
                            avsluttetStatusDato = correspondence.FinishedDate,
                            saksbehandlerId = GetBrukerId(correspondence.OwnedBy.ToString()),
                            godkjentAvSaksbehandlerId = GetBrukerId(correspondence.OwnedBy.ToString()),
                            godkjentStatusDato = correspondence.FinishedDate,
                            barnetsMedvirkning = "Se dokument",
                            bakgrunnsopplysninger = "Se dokument",
                            vedtak = "Se dokument",
                            begrunnelse = "Se dokument",
                        };
                        if (!vedtak.avsluttetStatusDato.HasValue)
                        {
                            vedtak.avsluttetStatusDato = correspondence.CreatedDate;
                        }
                        if (!vedtak.godkjentStatusDato.HasValue)
                        {
                            vedtak.godkjentStatusDato = correspondence.CreatedDate;
                        }
                        if (vedtak.startdato >= FirstInYearOfNewLaw)
                        {
                            vedtak.lovhjemmel = "Bvl._§_4-1";
                        }
                        else
                        {
                            vedtak.lovhjemmel = "Bvl._§_4-6._1.ledd_(gammel_lov)";
                        }

                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            CaseCase rawCase = await context.CaseCases.Where(e => e.CaseCaseId == caseId).FirstOrDefaultAsync();
                            vedtak.sakId = AddBydel(await GetBVVFirstCaseNumberAsync(rawCase.ClientId), "SAK");
                        }
                        SqlCommand command = new($"Select FileDataBlob From Correspondence_Document Where Correspondence_DocumentId={correspondenceDocument.CorrespondenceDocumentId} And FileDataBlob Is Not Null", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Document document = new()
                            {
                                dokumentId = vedtak.aktivitetId,
                                filId = vedtak.aktivitetId,
                                ferdigstilt = true,
                                opprettetAvId = vedtak.saksbehandlerId,
                                sakId = vedtak.sakId,
                                tittel = vedtak.tittel,
                                journalDato = vedtak.vedtaksdato,
                                filFormat = "PDF"
                            };
                            document.aktivitetIdListe.Add(vedtak.aktivitetId);
                            document.filId += ".pdf";
                            documents.Add(document);

                            SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                            SqlDataReader readerMigrering = null;
                            try
                            {
                                connectionMigrering.Open();
                                bool fileAlreadyWritten = false;
                                SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{document.filId}'", connectionMigrering)
                                {
                                    CommandTimeout = 300
                                };
                                readerMigrering = commandMigrering.ExecuteReader();
                                while (readerMigrering.Read())
                                {
                                    if (readerMigrering.GetInt32(0) > 0)
                                    {
                                        fileAlreadyWritten = true;
                                    }
                                }
                                readerMigrering.Close();

                                if (!fileAlreadyWritten)
                                {
                                    while (reader.Read())
                                    {
                                        await File.WriteAllBytesAsync(OutputFolderName + $"filer\\" + document.filId, (byte[])reader["FileDataBlob"]);
                                    }
                                    commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                    {
                                        CommandTimeout = 300
                                    };
                                    commandMigrering.ExecuteNonQuery();
                                }
                            }
                            finally
                            {
                                connectionMigrering.Close();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(correspondenceDocument.DocumentText))
                            {
                                HtmlDocumentToInclude htmlDocumentToInclude = new()
                                {
                                    dokLoepenr = correspondenceDocument.CorrespondenceDocumentId.ToString() + "COR",
                                    sakId = vedtak.sakId,
                                    tittel = vedtak.tittel,
                                    journalDato = vedtak.vedtaksdato,
                                    opprettetAvId = vedtak.saksbehandlerId,
                                    innhold = correspondenceDocument.DocumentText
                                };
                                htmlDocumentToInclude.aktivitetIdListe.Add(vedtak.aktivitetId);
                                htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                            }
                        }
                        List<CorrespondenceCorrespondenceattachment> attachments;
                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            attachments = await context.CorrespondenceCorrespondenceattachments.Where(e => e.CorrespondenceId == correspondence.CorrespondenceCorrespondenceId).ToListAsync();
                        }
                        int vedleggIndeks = 0;
                        foreach (var attachment in attachments)
                        {
                            vedleggIndeks += 1;
                            if (attachment.FileDataBlob != null)
                            {
                                Document attachmentDocument = new()
                                {
                                    dokumentId = vedtak.aktivitetId + vedleggIndeks,
                                    filId = vedtak.aktivitetId + vedleggIndeks,
                                    ferdigstilt = true,
                                    opprettetAvId = vedtak.saksbehandlerId,
                                    sakId = vedtak.sakId,
                                    tittel = $"{vedtak.tittel} (vedlegg {vedleggIndeks})",
                                    journalDato = vedtak.vedtaksdato,
                                    filFormat = "PDF"
                                };
                                attachmentDocument.aktivitetIdListe.Add(vedtak.aktivitetId);
                                attachmentDocument.filId += ".pdf";
                                documents.Add(attachmentDocument);

                                SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                                SqlDataReader readerMigrering = null;
                                try
                                {
                                    connectionMigrering.Open();
                                    bool fileAlreadyWritten = false;
                                    SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{attachmentDocument.filId}'", connectionMigrering)
                                    {
                                        CommandTimeout = 300
                                    };
                                    readerMigrering = commandMigrering.ExecuteReader();
                                    while (readerMigrering.Read())
                                    {
                                        if (readerMigrering.GetInt32(0) > 0)
                                        {
                                            fileAlreadyWritten = true;
                                        }
                                    }
                                    readerMigrering.Close();

                                    if (!fileAlreadyWritten)
                                    {
                                        await File.WriteAllBytesAsync(OutputFolderName + $"filer\\" + attachmentDocument.filId, attachment.FileDataBlob);
                                        commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{attachmentDocument.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                        {
                                            CommandTimeout = 300
                                        };
                                        commandMigrering.ExecuteNonQuery();
                                    }
                                }
                                finally
                                {
                                    connectionMigrering.Close();
                                }
                            }
                            else
                            {
                                HtmlDocumentToInclude htmlDocumentToInclude = new()
                                {
                                    dokLoepenr = attachment.Id.ToString() + "CORATT",
                                    sakId = vedtak.sakId,
                                    tittel = $"{vedtak.tittel} (vedlegg {vedleggIndeks})",
                                    journalDato = vedtak.vedtaksdato,
                                    opprettetAvId = vedtak.saksbehandlerId,
                                    innhold = $"<p>{attachment.Name}</p>"
                                };
                                htmlDocumentToInclude.aktivitetIdListe.Add(vedtak.aktivitetId);
                                htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                            }
                        }
                        reader.Close();
                    }
                    else
                    {
                        Aktivitet aktivitet = new()
                        {
                            aktivitetId = AddBydel(correspondenceDocument.CorrespondenceDocumentId.ToString(), "POS"),
                            aktivitetsType = "BARNEVERNSVAKT",
                            aktivitetsUnderType = aktivitetsUnderType,
                            status = "UTFØRT",
                            hendelsesdato = correspondence.CorrespondenceDate,
                            saksbehandlerId = GetBrukerId(correspondence.OwnedBy.ToString()),
                            tittel = correspondence.Title,
                            utfortAvId = GetBrukerId(correspondence.OwnedBy.ToString()),
                            utfortDato = correspondence.FinishedDate,
                            notat = "Se dokument"
                        };
                        if (!aktivitet.utfortDato.HasValue)
                        {
                            aktivitet.utfortDato = aktivitet.hendelsesdato;
                        }
                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            CaseCase rawCase = await context.CaseCases.Where(e => e.CaseCaseId == caseId).FirstOrDefaultAsync();
                            aktivitet.sakId = AddBydel(await GetBVVFirstCaseNumberAsync(rawCase.ClientId), "SAK");
                        }
                        SqlCommand command = new($"Select FileDataBlob From Correspondence_Document Where Correspondence_DocumentId={correspondenceDocument.CorrespondenceDocumentId} And FileDataBlob Is Not Null", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            Document document = new()
                            {
                                dokumentId = aktivitet.aktivitetId,
                                filId = aktivitet.aktivitetId,
                                ferdigstilt = true,
                                opprettetAvId = aktivitet.utfortAvId,
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato,
                                filFormat = "PDF"
                            };
                            document.aktivitetIdListe.Add(aktivitet.aktivitetId);
                            document.filId += ".pdf";
                            documents.Add(document);

                            SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                            SqlDataReader readerMigrering = null;
                            try
                            {
                                connectionMigrering.Open();
                                bool fileAlreadyWritten = false;
                                SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{document.filId}'", connectionMigrering)
                                {
                                    CommandTimeout = 300
                                };
                                readerMigrering = commandMigrering.ExecuteReader();
                                while (readerMigrering.Read())
                                {
                                    if (readerMigrering.GetInt32(0) > 0)
                                    {
                                        fileAlreadyWritten = true;
                                    }
                                }
                                readerMigrering.Close();

                                if (!fileAlreadyWritten)
                                {
                                    while (reader.Read())
                                    {
                                        await File.WriteAllBytesAsync(OutputFolderName + $"filer\\" + document.filId, (byte[])reader["FileDataBlob"]);
                                    }
                                    commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                    {
                                        CommandTimeout = 300
                                    };
                                    commandMigrering.ExecuteNonQuery();
                                }
                            }
                            finally
                            {
                                connectionMigrering.Close();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(correspondenceDocument.DocumentText))
                            {
                                HtmlDocumentToInclude htmlDocumentToInclude = new()
                                {
                                    dokLoepenr = correspondenceDocument.CorrespondenceDocumentId.ToString() + "COR",
                                    sakId = aktivitet.sakId,
                                    tittel = aktivitet.tittel,
                                    journalDato = aktivitet.utfortDato,
                                    opprettetAvId = aktivitet.utfortAvId,
                                    innhold = correspondenceDocument.DocumentText
                                };
                                htmlDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                                htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                            }
                        }
                        List<CorrespondenceCorrespondenceattachment> attachments;
                        using (var context = new BVVDBContext(ConnectionStringVFB))
                        {
                            attachments = await context.CorrespondenceCorrespondenceattachments.Where(e => e.CorrespondenceId == correspondence.CorrespondenceCorrespondenceId).ToListAsync();
                        }
                        int vedleggIndeks = 0;
                        foreach (var attachment in attachments)
                        {
                            vedleggIndeks += 1;
                            if (attachment.FileDataBlob != null)
                            {
                                Document attachmentDocument = new()
                                {
                                    dokumentId = aktivitet.aktivitetId + vedleggIndeks,
                                    filId = aktivitet.aktivitetId + vedleggIndeks,
                                    ferdigstilt = true,
                                    opprettetAvId = aktivitet.utfortAvId,
                                    sakId = aktivitet.sakId,
                                    tittel = $"{aktivitet.tittel} (vedlegg {vedleggIndeks})",
                                    journalDato = aktivitet.utfortDato,
                                    filFormat = "PDF"
                                };
                                attachmentDocument.aktivitetIdListe.Add(aktivitet.aktivitetId);
                                attachmentDocument.filId += ".pdf";
                                documents.Add(attachmentDocument);

                                SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                                SqlDataReader readerMigrering = null;
                                try
                                {
                                    connectionMigrering.Open();
                                    bool fileAlreadyWritten = false;
                                    SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{attachmentDocument.filId}'", connectionMigrering)
                                    {
                                        CommandTimeout = 300
                                    };
                                    readerMigrering = commandMigrering.ExecuteReader();
                                    while (readerMigrering.Read())
                                    {
                                        if (readerMigrering.GetInt32(0) > 0)
                                        {
                                            fileAlreadyWritten = true;
                                        }
                                    }
                                    readerMigrering.Close();

                                    if (!fileAlreadyWritten)
                                    {
                                        await File.WriteAllBytesAsync(OutputFolderName + $"filer\\" + attachmentDocument.filId, attachment.FileDataBlob);
                                        commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{attachmentDocument.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                        {
                                            CommandTimeout = 300
                                        };
                                        commandMigrering.ExecuteNonQuery();
                                    }
                                }
                                finally
                                {
                                    connectionMigrering.Close();
                                }
                            }
                            else
                            {
                                HtmlDocumentToInclude htmlDocumentToInclude = new()
                                {
                                    dokLoepenr = attachment.Id.ToString() + "CORATT",
                                    sakId = aktivitet.sakId,
                                    tittel = $"{aktivitet.tittel} (vedlegg {vedleggIndeks})",
                                    journalDato = aktivitet.utfortDato,
                                    opprettetAvId = aktivitet.utfortAvId,
                                    innhold = $"<p>{attachment.Name}</p>"
                                };
                                htmlDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                                htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                            }
                        }
                        reader.Close();
                        aktiviteter.Add(aktivitet);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, "Post", "Pos");
            int toSkip = 0;
            int fileNumber = 1;
            List<Document> documentsDistinct = documents.GroupBy(c => new { c.dokumentId, c.sakId }).Select(s => s.First()).ToList();
            int migrertAntall = documentsDistinct.Count;
            while (migrertAntall > toSkip)
            {
                List<Document> documentsPart = documentsDistinct.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"DokumenterBVVPos{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            toSkip = 0;
            fileNumber = 1;
            migrertAntall = aktiviteter.Count;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"BVVPost{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            toSkip = 0;
            fileNumber = 1;
            migrertAntall = vedtaksListe.Count;
            while (migrertAntall > toSkip)
            {
                List<Vedtak> vedtaksPart = vedtaksListe.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(vedtaksPart, GetJsonFileName("vedtak", $"BVVVedtak{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return antall;
        }
        private async Task<string> GetBVVFirstCaseNumberAsync(int? client)
        {
            string caseNumber = "";

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                CaseCase caseFirst = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3 && k.ClientId == client).OrderBy(k => k.CaseCaseId).FirstOrDefaultAsync();

                if (caseFirst != null)
                {
                    caseNumber = caseFirst.Number;
                }
            }
            return caseNumber;
        }
        private async Task<string> GetBVVBaseregistryValueAsync(int? registryId, bool getRegistryValueCode = false)
        {
            string value = "";

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                BaseregistryBaseregistry registry = await context.BaseregistryBaseregistries.Where(k => k.BaseRegistryBaseRegistryId == registryId).FirstOrDefaultAsync();
                if (registry != null)
                {
                    if (getRegistryValueCode)
                    {
                        value = registry.RegistryValueCode;
                    }
                    else
                    {
                        value = registry.RegistryValue;
                    }
                }
            }
            return value;
        }
        #endregion

        #region Saker
        public async Task<string> GetSakerAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk saker...");
                string statusText = $"Antall barnevernsaker: {await GetBarnevernsakerAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall barnevernsaker uten sak: {await GetBarnevernsakerUtenSakAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall tilsynssaker: {await GetTilsynssakerAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall oppdragstakersaker: {await GetOppdragstakersakerAsync(worker)}" + Environment.NewLine;
                return statusText;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBarnevernsakerAsync(BackgroundWorker worker)
        {
            try
            {
                int antall = 0;
                int migrertAntall = 0;
                List<FaKlient> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaKlients
                        .Where(KlientFilter())
                        .OrderBy(o => o.KliLoepenr)
                        .Where(k => k.KliFraannenkommune == 0)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Sak> saker = new();

                foreach (var klient in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk barnevernsaker ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(klient.KliLoepenr))
                    {
                        continue;
                    }
                    Sak sak = new()
                    {
                        sakId = GetSakId(klient.KliLoepenr.ToString()),
                        avdelingId = GetEnhetskode(klient.DisDistriktskode),
                        startDato = klient.KliRegistrertdato,
                        sluttDato = klient.KliAvsluttetdato,
                        merknad = klient.KliMerknader?.Trim(),
                        arbeidsbelastning = "LAV",
                        sakstype = "BARNEVERNSSAK"
                    };
                    if (sak.sluttDato.HasValue && sak.startDato.HasValue && sak.sluttDato.Value < sak.startDato.Value)
                    {
                        sak.sluttDato = sak.startDato;
                    };
                    sak.aktorId = GetActorId(klient);
                    if (!string.IsNullOrEmpty(klient.KliGmlreferanse))
                    {
                        sak.konfidensialitet = "VIP";
                    }
                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        sak.tidligereAvdelingListe = mappings.GetTidligereBydeler(klient.KliLoepenr);
                    }
                    if (!string.IsNullOrEmpty(klient.SbhInitialer))
                    {
                        sak.saksbehandlerId = GetBrukerId(klient.SbhInitialer);
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.sekunderSaksbehandlerId.Add(GetBrukerId(klient.SbhInitialer2));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.saksbehandlerId = GetBrukerId(klient.SbhInitialer2);
                        }
                    }
                    if (string.IsNullOrEmpty(sak.saksbehandlerId))
                    {
                        sak.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                    }
                    if (!klient.KliAvsluttetdato.HasValue)
                    {
                        sak.status = "ÅPEN";
                    }
                    else
                    {
                        sak.status = "LUKKET";
                    }
                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        await GetDataTidligereBydelerAsync(worker, klient.KliFoedselsdato.Value, klient.KliPersonnr.Value, sak);
                    }
                    await GetMeldingerUtenSakForKlientAsync(worker, klient.KliLoepenr, sak, klient.KliFoedselsdato.Value, klient.KliPersonnr.Value);
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        List<FaMeldinger> meldinger = await context.FaMeldingers
                            .Where(m => m.KliLoepenr == klient.KliLoepenr && m.MelAvsluttetgjennomgang.HasValue && m.MelMeldingstype != "UGR")
                            .ToListAsync();
                        foreach (FaMeldinger melding in meldinger)
                        {
                            if (melding.MelMottattdato < sak.startDato)
                            {
                                sak.startDato = melding.MelMottattdato;
                            }
                        }
                    }
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaPostjournal postjournal = await context.FaPostjournals.Where(p => p.KliLoepenr == klient.KliLoepenr).OrderBy(o => o.PosDato).FirstOrDefaultAsync();
                        if (postjournal != null && postjournal.PosDato.Year > 1997 && sak.startDato > postjournal.PosDato)
                        {
                            sak.startDato = postjournal.PosDato;
                        }
                    }
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaJournal journal  = await context.FaJournals.Where(m => m.JouFerdigdato != null && m.KliLoepenr == klient.KliLoepenr).OrderBy(o => o.JouDatonotat).FirstOrDefaultAsync();
                        if (journal != null && journal.JouDatonotat.Year > 1997 && sak.startDato > journal.JouDatonotat)
                        {
                            sak.startDato = journal.JouDatonotat;
                        }
                    }
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaTiltaksplan tiltaksplan = await context.FaTiltaksplans.Where(m => m.TtpSlettet == 0 && m.TtpFerdigdato.HasValue && m.KliLoepenr == klient.KliLoepenr).OrderBy(o => o.TtpFerdigdato).FirstOrDefaultAsync();
                        if (tiltaksplan != null && tiltaksplan.TtpFerdigdato.HasValue && tiltaksplan.TtpFerdigdato.Value.Year > 1997 && sak.startDato > tiltaksplan.TtpFerdigdato)
                        {
                            sak.startDato = tiltaksplan.TtpFerdigdato;
                        }
                    }
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaTiltaksplanevalueringer evaluering = await context.FaTiltaksplanevalueringers.Where(m => m.EvaUtfoertdato.HasValue && m.TtpLoepenrNavigation.KliLoepenr == klient.KliLoepenr).OrderBy(o => o.EvaUtfoertdato).FirstOrDefaultAsync();
                        if (evaluering != null && evaluering.EvaUtfoertdato.HasValue && evaluering.EvaUtfoertdato.Value.Year > 1997 && sak.startDato > evaluering.EvaUtfoertdato)
                        {
                            sak.startDato = evaluering.EvaUtfoertdato;
                        }
                    }
                    saker.Add(sak);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Sak> sakerDistinct = saker.GroupBy(c => c.sakId).Select(s => s.First()).ToList();
                int antallEntiteter = sakerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = sakerDistinct.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(sakerPart, GetJsonFileName("saker", $"Barnevernsaker{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return migrertAntall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetBarnevernsakerUtenSakAsync(BackgroundWorker worker)
        {
            try
            {
                int antall = 0;
                List<FaMeldinger> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak).ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Sak> saker = new();

                foreach (var melding in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk barnevernsaker uten sak ({antall} av {totalAntall})...");
                    }
                    if (string.IsNullOrEmpty(melding.MelGmlreferanse) && melding.MelPersonnr.HasValue && melding.MelFoedselsdato.HasValue && melding.MelPersonnr.GetValueOrDefault() != 99999 && melding.MelPersonnr.GetValueOrDefault() != 00100 && melding.MelPersonnr.GetValueOrDefault() != 00200)
                    {
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            int antallOrdinæreSaker = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue)).Where(k => k.KliFraannenkommune == 0 && k.KliFoedselsdato == melding.MelFoedselsdato && k.KliPersonnr == melding.MelPersonnr).CountAsync();
                            if (antallOrdinæreSaker > 0)
                            {
                                continue;
                            }
                            else
                            {
                                FaMeldinger firstMelding = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak && m.MelFoedselsdato == melding.MelFoedselsdato && m.MelPersonnr == melding.MelPersonnr).OrderByDescending(m => m.MelLoepenr).FirstOrDefaultAsync();
                                if (firstMelding != null)
                                {
                                    if (melding.MelLoepenr != firstMelding.MelLoepenr)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    };
                    Sak sak = new()
                    {
                        sakId = GetSakId(melding.MelLoepenr.ToString() + "__MUS"),
                        avdelingId = GetEnhetskode(melding.DisDistriktskode),
                        aktorId = AddBydel(melding.MelLoepenr.ToString()),
                        startDato = melding.MelMottattdato,
                        sluttDato = melding.MelAvsluttetgjennomgang,
                        arbeidsbelastning = "LAV",
                        status = "LUKKET",
                        sakstype = "BARNEVERNSSAK"
                    };
                    if (melding.MelPersonnr.HasValue && melding.MelFoedselsdato.HasValue && melding.MelPersonnr.GetValueOrDefault() != 99999 && melding.MelPersonnr.GetValueOrDefault() != 00100 && melding.MelPersonnr.GetValueOrDefault() != 00200)
                    {
                        if (string.IsNullOrEmpty(melding.MelGmlreferanse))
                        {
                            sak.aktorId = GetUnikActorId(null, melding.MelFoedselsdato.Value.ToString("ddMMyy") + melding.MelPersonnr, null, null);
                        }
                    }
                    if (sak.sluttDato.HasValue && sak.startDato.HasValue && sak.sluttDato.Value < sak.startDato.Value)
                    {
                        sak.sluttDato = sak.startDato;
                    };
                    if (!string.IsNullOrEmpty(melding.SbhInitialer))
                    {
                        sak.saksbehandlerId = GetBrukerId(melding.SbhInitialer);
                    }
                    else
                    {
                        sak.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                    }
                    saker.Add(sak);
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Sak> sakerDistinct = saker.GroupBy(c => c.sakId).Select(s => s.First()).ToList();
                int antallEntiteter = sakerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = sakerDistinct.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(sakerPart, GetJsonFileName("saker", $"BarnevernsakerUtenSak{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetTilsynssakerAsync(BackgroundWorker worker)
        {
            try
            {
                int antall = 0;
                List<FaKlient> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaKlients
                        .Where(KlientFilter())
                        .Where(m => m.KliFraannenkommune == 1 && !m.KliAvsluttetdato.HasValue && m.KliFoedselsdato.HasValue && m.KliFoedselsdato > FromDateMigrationTilsyn)
                        .OrderBy(o => o.KliLoepenr)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Sak> saker = new();

                foreach (var klient in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk tilsynssaker ({antall} av {totalAntall})...");
                    }
                    Sak sak = new()
                    {
                        sakId = GetSakId(klient.KliLoepenr.ToString()),
                        avdelingId = GetEnhetskode(klient.DisDistriktskode),
                        startDato = klient.KliRegistrertdato,
                        sluttDato = klient.KliAvsluttetdato,
                        merknad = klient.KliMerknader?.Trim(),
                        arbeidsbelastning = "LAV"
                    };
                    if (sak.sluttDato.HasValue && sak.startDato.HasValue && sak.sluttDato.Value < sak.startDato.Value)
                    {
                        sak.sluttDato = sak.startDato;
                    };
                    sak.aktorId = GetActorId(klient);
                    if (!string.IsNullOrEmpty(klient.KliGmlreferanse))
                    {
                        sak.konfidensialitet = "VIP";
                    }
                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        sak.tidligereAvdelingListe = mappings.GetTidligereBydeler(klient.KliLoepenr);
                    }
                    if (!string.IsNullOrEmpty(klient.SbhInitialer))
                    {
                        sak.saksbehandlerId = GetBrukerId(klient.SbhInitialer);
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.sekunderSaksbehandlerId.Add(GetBrukerId(klient.SbhInitialer2));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.saksbehandlerId = GetBrukerId(klient.SbhInitialer2);
                        }
                    }
                    if (string.IsNullOrEmpty(sak.saksbehandlerId))
                    {
                        sak.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                    }
                    if (!klient.KliAvsluttetdato.HasValue)
                    {
                        sak.status = "ÅPEN";
                    }
                    else
                    {
                        sak.status = "LUKKET";
                    }
                    sak.sakstype = "TILSYNSSAK";
                    saker.Add(sak);
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Sak> sakerDistinct = saker.GroupBy(c => c.sakId).Select(s => s.First()).ToList();
                int antallEntiteter = sakerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = sakerDistinct.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(sakerPart, GetJsonFileName("saker", $"Tilsynssaker{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return antall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetOppdragstakersakerAsync(BackgroundWorker worker)
        {
            try
            {
                int antall = 0;
                int migrertAntall = 0;
                List<FaMedarbeidere> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue)).ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Sak> saker = new();

                foreach (var medarbeider in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk oppdragstakersaker ({antall} av {totalAntall})...");
                    }
                    int numberOfActiveContracts = 0;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        numberOfActiveContracts = await context.FaEngasjementsavtales.Where(e => e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                            && medarbeider.ForLoepenr == e.ForLoepenr
                            && (e.EngTildato >= FirstInYearOfMigration)).CountAsync();
                    }
                    if (numberOfActiveContracts == 0)
                    {
                        continue;
                    }
                    Sak sak = new()
                    {
                        avdelingId = "SuppliersAndContractorsTeam",
                        aktorId = GetActorId(medarbeider.ForLoepenrNavigation, false),
                        startDato = medarbeider.MedBegyntdato,
                        status = "ÅPEN",
                        arbeidsbelastning = "LAV",
                        sakstype = "OPPDRAGSTAKER",
                        saksbehandlerId = GetBrukerId(mappings.GetHovedkontorfagligBydel(Bydelsforkortelse))
                    };
                    sak.sakId = $"{sak.aktorId}__OPP";

                    bool caseAlreadyWritten = false;

                    SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                    SqlDataReader readerMigrering = null;
                    try
                    {
                        connectionMigrering.Open();

                        SqlCommand commandMigrering = new($"Select Count(*) From Oppdragstakere{MigreringsdbPostfix} Where SakId='{sak.sakId}'", connectionMigrering)
                        {
                            CommandTimeout = 300
                        };
                        readerMigrering = commandMigrering.ExecuteReader();
                        while (readerMigrering.Read())
                        {
                            if (readerMigrering.GetInt32(0) > 0)
                            {
                                caseAlreadyWritten = true;
                            }
                        }
                        readerMigrering.Close();

                        if (!caseAlreadyWritten)
                        {
                            commandMigrering = new($"Update Oppdragstakere{MigreringsdbPostfix} Set SakId='{sak.sakId}', Dato=GETDATE() Where AktorId='{sak.aktorId}'", connectionMigrering)
                            {
                                CommandTimeout = 300
                            };
                            commandMigrering.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        connectionMigrering.Close();
                    }
                    if (caseAlreadyWritten)
                    {
                        continue;
                    }
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaEngasjementsavtale engasjementsavtale = await context.FaEngasjementsavtales.Where(e => e.DokLoepenr.HasValue && e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                              && (e.EngTildato >= FirstInYearOfMigration)).OrderBy(o => o.EngFradato).FirstOrDefaultAsync();
                        if (engasjementsavtale != null && engasjementsavtale.EngFradato < sak.startDato)
                        {
                            sak.startDato = engasjementsavtale.EngFradato;
                        }
                    }
                    saker.Add(sak);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Sak> sakerDistinct = saker.GroupBy(c => c.sakId).Select(s => s.First()).ToList();
                int antallEntiteter = sakerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = sakerDistinct.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(sakerPart, GetJsonFileName("saker", $"Oppdragstakersaker{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return migrertAntall;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Lokal Oppdragstakersak
        public async Task<string> LokalOppdragstakersak(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk lokale oppdragstakersaker...");
                await ExtractSokratesAsync(worker, false);

                int antall = 0;
                int migrertAntall = 0;
                List<FaMedarbeidere> rawData;
                int totalAntall = 0;
                List<DocumentToInclude> documentsIncluded = new();
                List<Aktivitet> oppdragstakeravtaleAktiviteter = new();

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue)).ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Sak> saker = new();

                foreach (var medarbeider in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk lokale oppdragstakersaker ({antall} av {totalAntall})...");
                    }
                    int numberOfActiveContracts = 0;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        numberOfActiveContracts = await context.FaEngasjementsavtales.Where(e => e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                            && medarbeider.ForLoepenr == e.ForLoepenr
                            && (e.EngTildato >= FirstInYearOfMigration)).CountAsync();
                    }
                    if (numberOfActiveContracts == 0)
                    {
                        continue;
                    }
                    Sak sak = new()
                    {
                        //TODO: Avtaler - Skal hver bydel sin mottaksavdeling være eier?
                        avdelingId = "SuppliersAndContractorsTeam",
                        aktorId = GetActorId(medarbeider.ForLoepenrNavigation, false),
                        startDato = medarbeider.MedBegyntdato,
                        //TODO: Avtaler - Skal noen settes til LUKKET?
                        status = "ÅPEN",
                        arbeidsbelastning = "LAV",
                        //TODO: Avtaler - Navn på sakstype?
                        sakstype = "OPPDRAGSTAKER",
                        //TODO: Avtaler - Få navn på hovedbruker i hver mottaksavdeling?
                        saksbehandlerId = GetBrukerId(mappings.GetHovedkontorfagligBydel(Bydelsforkortelse))
                    };
                    sak.sakId = $"{sak.aktorId}__OPP";

                    bool caseAlreadyWritten = false;

                    SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                    SqlDataReader readerMigrering = null;
                    try
                    {
                        connectionMigrering.Open();

                        SqlCommand commandMigrering = new($"Select Count(*) From Oppdragstakere{MigreringsdbPostfix} Where SakId='{sak.sakId}'", connectionMigrering)
                        {
                            CommandTimeout = 300
                        };
                        readerMigrering = commandMigrering.ExecuteReader();
                        while (readerMigrering.Read())
                        {
                            if (readerMigrering.GetInt32(0) > 0)
                            {
                                caseAlreadyWritten = true;
                            }
                        }
                        readerMigrering.Close();

                        if (!caseAlreadyWritten)
                        {
                            MessageBox.Show($"Fant ikke {sak.aktorId} i Migrering.Oppdragstakere{MigreringsdbPostfix}","OBS",MessageBoxButton.OK,MessageBoxImage.Error);
                        }
                    }
                    finally
                    {
                        connectionMigrering.Close();
                    }
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaEngasjementsavtale engasjementsavtale = await context.FaEngasjementsavtales.Where(e => e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                            && medarbeider.ForLoepenr == e.ForLoepenr && (e.EngTildato >= FirstInYearOfMigration)).OrderBy(c => c.EngFradato).FirstOrDefaultAsync();

                        if (engasjementsavtale != null && engasjementsavtale.EngFradato < sak.startDato)
                        {
                            sak.startDato = engasjementsavtale.EngFradato;
                        }
                    }
                    sak.sakId = $"{sak.aktorId}__OPP__{Bydelsforkortelse}";
                    saker.Add(sak);
                    migrertAntall += 1;
                }

                List<FaTiltak> tiltakRaw;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    tiltakRaw = await context.FaTiltaks.Include(x => x.KliLoepenrNavigation).Include(x => x.Sak)
                        .Where(KlientTiltakFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .ToListAsync();
                }

                foreach (var tiltakFamilia in tiltakRaw)
                {
                    if (!mappings.IsOwner(tiltakFamilia.KliLoepenr))
                    {
                        continue;
                    }

                    string tiltakId = AddBydel(tiltakFamilia.TilLoepenr.ToString(), "TIL");
                    string sakId = GetSakId(tiltakFamilia.KliLoepenr.ToString());

                    List<FaEngasjementsavtale> engasjementsavtaler = null;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        engasjementsavtaler = await context.FaEngasjementsavtales.Include(m => m.ForLoepenrNavigation.ForLoepenrNavigation).Where(e => string.IsNullOrEmpty(e.ForLoepenrNavigation.ForLoepenrNavigation.ForGmlreferanse) && e.TilLoepenr == tiltakFamilia.TilLoepenr && e.DokLoepenr.HasValue && e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                              && (e.EngTildato >= FirstInYearOfMigration)).OrderByDescending(o => o.ForLoepenrNavigation.ForLoepenrNavigation).ThenByDescending(o => o.EngTildato).ToListAsync();
                    }
                    if (engasjementsavtaler != null)
                    {
                        decimal lastForLoepenr = 0;
                        foreach (FaEngasjementsavtale engasjementsavtale in engasjementsavtaler)
                        {
                            if (engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation.ForLoepenr != lastForLoepenr)
                            {
                                Aktivitet aktivitet = new()
                                {
                                    aktivitetId = AddBydel(engasjementsavtale.EngLoepenr.ToString(), "ODA"),
                                    sakId = $"{GetActorId(engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation, false)}__OPP__{Bydelsforkortelse}",
                                    aktivitetsType = "OPPDRAGSTAKER_AVTALE",
                                    aktivitetsUnderType = "ANNET",
                                    //TODO: Avtaler - Er EngFradato riktig felt her?
                                    hendelsesdato = engasjementsavtale.EngFradato,
                                    status = "UTFØRT",
                                    tittel = "Oppdragstakeravtale",
                                    notat = "Se dokument",
                                    utfortDato = engasjementsavtale.EngAvgjortdato,
                                    utlopsdato = engasjementsavtale.EngTildato,
                                    tiltaksId = tiltakId
                                };
                                if (!string.IsNullOrEmpty(engasjementsavtale.SbhInitialer))
                                {
                                    aktivitet.saksbehandlerId = GetBrukerId(engasjementsavtale.SbhInitialer);
                                    aktivitet.utfortAvId = GetBrukerId(engasjementsavtale.SbhInitialer);
                                }
                                oppdragstakeravtaleAktiviteter.Add(aktivitet);
                                DocumentToInclude documentToInclude = new()
                                {
                                    dokLoepenr = engasjementsavtale.DokLoepenr.Value,
                                    dokumentNr = engasjementsavtale.EngDokumentnr,
                                    sakId = aktivitet.sakId,
                                    tittel = aktivitet.tittel,
                                    journalDato = aktivitet.utfortDato,
                                    opprettetAvId = aktivitet.saksbehandlerId
                                };
                                documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                                documentsIncluded.Add(documentToInclude);
                                lastForLoepenr = engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation.ForLoepenr;
                            }
                        }
                    }
                }

                int toSkip = 0;
                int fileNumber = 1;
                List<Sak> sakerDistinct = saker.GroupBy(c => c.sakId).Select(s => s.First()).ToList();
                int antallEntiteter = sakerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = sakerDistinct.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(sakerPart, GetJsonFileName("saker", $"LokaleOppdragstakersaker{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                await GetDocumentsAsync(worker, "LokaleOppdragsavtaleaktiviteter", documentsIncluded);
                toSkip = 0;
                fileNumber = 1;
                List<Aktivitet> oppdragstakeravtaleAktiviteterDistinct = oppdragstakeravtaleAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                antallEntiteter = oppdragstakeravtaleAktiviteterDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> oppdragstakeravtaleAktiviteterPart = oppdragstakeravtaleAktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(oppdragstakeravtaleAktiviteterPart, GetJsonFileName("aktiviteter", $"LokaleOppdragsavtaleaktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                worker.ReportProgress(0, "Ferdig uttrekk lokale oppdragstakersaker...");
                return $"Antall lokale oppdragstakersaker: {migrertAntall}";
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Innbyggere - Barn
        public async Task<string> GetInnbyggereBarnAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk innbyggere - barn...");
                string statusText = $"Antall innbyggere barn: {await GetInnbyggereBarnHovedAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall innbyggere barn uten sak: {await GetInnbyggereBarnUtenSakAsync(worker)}" + Environment.NewLine;
                return statusText;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetInnbyggereBarnHovedAsync(BackgroundWorker worker)
        {
            int antall = 0;
            int migrertAntall = 0;
            List<FaKlient> rawData;
            int totalAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaKlients
                    .Where(KlientFilter())
                    .Where(KlientKunGyldigeTilsyn())
                    .OrderBy(o => o.KliLoepenr)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }

            List<Innbygger> innbyggere = new();

            foreach (var klient in rawData)
            {
                antall += 1;
                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk innbyggere - barn ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(klient.KliLoepenr) && klient.KliFraannenkommune == 0)
                {
                    continue;
                }
                Innbygger innbygger = new()
                {
                    fornavn = klient.KliFornavn?.Trim(),
                    registrertDato = klient.KliRegistrertdato.Value,
                    etternavn = klient.KliEtternavn?.Trim(),
                    sivilstand = "UOPPGITT",
                    fodselDato = klient.KliFoedselsdato,
                    kontonummer = klient.KliKontonr?.Trim(),
                    potensiellOppdragstaker = false,
                    oppdragstaker = false,
                    ikkeAktuellForOppdrag = true,
                    foedeland = mappings.GetLand(klient.NasKodenr),
                    deaktiver = false
                };
                if (!string.IsNullOrEmpty(innbygger.kontonummer) && (innbygger.kontonummer.Length != 11 || !IsValidMod11(innbygger.kontonummer)))
                {
                    innbygger.kontonummer = null;
                }
                if (klient.KliPersonnr.GetValueOrDefault() != 99999 && klient.KliPersonnr.GetValueOrDefault() != 00100 && klient.KliPersonnr.GetValueOrDefault() != 00200)
                {
                    if (!string.IsNullOrEmpty(klient.KliGmlreferanse))
                    {
                        innbygger.fodselsnummer = null;
                    }
                    else
                    {
                        innbygger.fodselsnummer = klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr;
                    }
                }
                innbygger.actorId = GetActorId(klient);

                if (klient.KliFremmedkontrollnr.HasValue && klient.KliFremmedkontrollnr.Value.ToString().Length > 11 && string.IsNullOrEmpty(klient.KliGmlreferanse))
                {
                    innbygger.dufNummer = klient.KliFremmedkontrollnr.Value.ToString();
                    innbygger.dufNavn = klient.KliFornavn?.Trim() + " " + klient.KliEtternavn?.Trim();
                }
                if (klient.KliKjoenn == "M" || klient.KliPersonnr.GetValueOrDefault() == 00100)
                {
                    innbygger.kjonn = "MANN";
                }
                else
                {
                    if (klient.KliKjoenn == "K" || klient.KliPersonnr.GetValueOrDefault() == 00200)
                    {
                        innbygger.kjonn = "KVINNE";
                    }
                }
                if (klient.KliPersonnr.HasValue && klient.KliPersonnr.Value == 99999)
                {
                    innbygger.ufodtBarn = true;
                    innbygger.terminDato = klient.KliFoedselsdato;
                }
                bool hovetelefonBestemt = false;
                if (!string.IsNullOrEmpty(klient.KliTelefonmobil))
                {
                    AktørTelefonnummer aktørTelefonnummerMobil = new()
                    {
                        telefonnummerType = "PRIVAT",
                        telefonnummer = GetTelefonnummer(klient.KliTelefonmobil?.Trim()),
                        hovedtelefon = true
                    };
                    hovetelefonBestemt = true;
                    innbygger.telefonnummer.Add(aktørTelefonnummerMobil);
                }
                if (!string.IsNullOrEmpty(klient.KliTelefonprivat))
                {
                    AktørTelefonnummer aktørTelefonnummerPrivat = new()
                    {
                        telefonnummerType = "PRIVAT",
                        telefonnummer = GetTelefonnummer(klient.KliTelefonprivat?.Trim())
                    };
                    if (!hovetelefonBestemt)
                    {
                        aktørTelefonnummerPrivat.hovedtelefon = true;
                        hovetelefonBestemt = true;
                    }
                    innbygger.telefonnummer.Add(aktørTelefonnummerPrivat);
                }
                if (!string.IsNullOrEmpty(klient.KliTelefonarbeid))
                {
                    AktørTelefonnummer aktørTelefonnummerArbeid = new()
                    {
                        telefonnummerType = "JOBB",
                        telefonnummer = GetTelefonnummer(klient.KliTelefonarbeid?.Trim())
                    };
                    if (!hovetelefonBestemt)
                    {
                        aktørTelefonnummerArbeid.hovedtelefon = true;
                        hovetelefonBestemt = true;
                    }
                    innbygger.telefonnummer.Add(aktørTelefonnummerArbeid);
                }
                if (!string.IsNullOrEmpty(klient.KliAdresse) || !string.IsNullOrEmpty(klient.PnrPostnr))
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(klient.KliLoepenr.ToString()),
                        adresseType = "FORETRUKKET_KONTAKTADRESSE",
                        linje1 = klient.KliAdresse?.Trim(),
                        postnummer = klient.PnrPostnr?.Trim(),
                        hovedadresse = false
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    innbygger.adresse.Add(adresse);
                }

                List<FaKlientplassering> klientplasserings;
                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    klientplasserings = await context.FaKlientplasserings.Where(k => k.KliLoepenr == klient.KliLoepenr && k.KplPlasseringbor == "3" && (!k.KplTildato.HasValue || k.KplTildato >= DateTime.Now)).ToListAsync();
                }
                if (klientplasserings.Count > 0)
                {
                    innbygger.adressesperre = "SKJULT_ADRESSE";
                }
                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    FaMeldinger melding = await context.FaMeldingers.Where(k => k.KliLoepenr == klient.KliLoepenr && k.MelAvsluttetgjennomgang.HasValue && k.MelMeldingstype != "UGR")
                        .OrderBy(o => o.MelRegistrertdato).FirstOrDefaultAsync();
                    if (melding != null && melding.MelRegistrertdato.HasValue)
                    {
                        if (innbygger.registrertDato > melding.MelRegistrertdato.Value)
                        {
                            innbygger.registrertDato = melding.MelRegistrertdato.Value;
                        }
                    }
                }
                switch (klient.KliFlyktningestatus?.Trim())
                {
                    case "ASYL":
                        innbygger.flyktningStatus = "ASYLSOEKER";
                        break;
                    case "FLYK":
                        innbygger.flyktningStatus = "FLYKTNING";
                        break;
                    case "OHGR":
                        innbygger.flyktningStatus = "HRG";
                        break;
                }
                innbyggere.Add(innbygger);
                migrertAntall += 1;
            }
            int toSkip = 0;
            int fileNumber = 1;
            List<Innbygger> innbyggereDistinct = innbyggere.GroupBy(c => c.actorId).Select(s => s.First()).ToList();
            int antallEntiteter = innbyggereDistinct.Count;
            while (antallEntiteter > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggereDistinct.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(innbyggerePart, GetJsonFileName("innbygger", $"InnbyggereBarn{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetInnbyggereBarnUtenSakAsync(BackgroundWorker worker)
        {
            int antall = 0;
            List<FaMeldinger> rawData;
            int totalAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak).ToListAsync();
                totalAntall = rawData.Count;
            }

            List<Innbygger> innbyggere = new();

            foreach (var melding in rawData)
            {
                antall += 1;
                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk innbyggere - barn uten sak ({antall} av {totalAntall})...");
                }
                if (string.IsNullOrEmpty(melding.MelGmlreferanse) && melding.MelPersonnr.HasValue && melding.MelFoedselsdato.HasValue && melding.MelPersonnr.GetValueOrDefault() != 99999 && melding.MelPersonnr.GetValueOrDefault() != 00100 && melding.MelPersonnr.GetValueOrDefault() != 00200)
                {
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        int antallOrdinæreSaker = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue)).Where(k => k.KliFraannenkommune == 0 && k.KliFoedselsdato == melding.MelFoedselsdato && k.KliPersonnr == melding.MelPersonnr).CountAsync();
                        if (antallOrdinæreSaker > 0)
                        {
                            continue;
                        }
                        else
                        {
                            FaMeldinger firstMelding = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak && m.MelFoedselsdato == melding.MelFoedselsdato && m.MelPersonnr == melding.MelPersonnr).OrderByDescending(m => m.MelLoepenr).FirstOrDefaultAsync();
                            if (firstMelding != null)
                            {
                                if (melding.MelLoepenr != firstMelding.MelLoepenr)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                };
                Innbygger innbygger = new()
                {
                    actorId = AddBydel(melding.MelLoepenr.ToString()),
                    registrertDato = melding.MelMottattdato,
                    fornavn = melding.MelFornavn?.Trim(),
                    etternavn = melding.MelEtternavn?.Trim(),
                    sivilstand = "UOPPGITT",
                    fodselDato = melding.MelFoedselsdato,
                    potensiellOppdragstaker = false,
                    oppdragstaker = false,
                    ikkeAktuellForOppdrag = true,
                    deaktiver = false
                };
                if (melding.MelPersonnr.HasValue && melding.MelFoedselsdato.HasValue && melding.MelPersonnr.GetValueOrDefault() != 99999 && melding.MelPersonnr.GetValueOrDefault() != 00100 && melding.MelPersonnr.GetValueOrDefault() != 00200)
                {
                    if (string.IsNullOrEmpty(melding.MelGmlreferanse))
                    {
                        innbygger.fodselsnummer = melding.MelFoedselsdato.Value.ToString("ddMMyy") + melding.MelPersonnr;
                        innbygger.actorId = GetUnikActorId(null, innbygger.fodselsnummer, null, null);
                    }
                }
                if (melding.MelPersonnr.HasValue && melding.MelPersonnr.Value == 99999)
                {
                    innbygger.ufodtBarn = true;
                    innbygger.terminDato = melding.MelFoedselsdato;
                }
                if (melding.MelPersonnr.GetValueOrDefault() == 00100)
                {
                    innbygger.kjonn = "MANN";
                }
                else
                {
                    if (melding.MelPersonnr.GetValueOrDefault() == 00200)
                    {
                        innbygger.kjonn = "KVINNE";
                    }
                }
                innbyggere.Add(innbygger);
            }
            int toSkip = 0;
            int fileNumber = 1;
            List<Innbygger> innbyggereDistinct = innbyggere.GroupBy(c => c.actorId).Select(s => s.First()).ToList();
            int antallEntiteter = innbyggereDistinct.Count;
            while (antallEntiteter > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggereDistinct.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(innbyggerePart, GetJsonFileName("innbygger", $"InnbyggereBarnUtenSak{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return antall;
        }
        #endregion

        #region Innbyggere
        public async Task<string> GetInnbyggereAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk innbyggere...");
                int totalAntall = 0;
                int antall = 0;

                List<FaForbindelser> rawDataMedarbeidere;
                List<FaForbindelser> rawDataKlienttilknytninger;
                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawDataMedarbeidere = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue)).Select(m => m.ForLoepenrNavigation).Distinct().ToListAsync();
                    var rollerInkludert = new string[] { "MOR", "FAR", "SØS", "FMO", "FFA", "FAM", "VRG", "BRH", "BSH", "FSA" };
                    rawDataKlienttilknytninger = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation).Include(n => n.KliLoepenrNavigation)
                        .Where(KlientTilknytningFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && ((!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue) || rollerInkludert.Contains(f.KtkRolle)))
                        .Select(m => m.ForLoepenrNavigation)
                        .Distinct()
                        .ToListAsync();
                }

                List<FaForbindelser> forbindelser = new(rawDataKlienttilknytninger);
                forbindelser.AddRange(rawDataMedarbeidere);
                forbindelser = forbindelser.Distinct().ToList();
                totalAntall = forbindelser.Count;

                List<Innbygger> innbyggere = new();

                foreach (var forbindelse in forbindelser)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk innbyggere ({antall} av {totalAntall})...");
                    }
                    FaForbindelser medarbeider = rawDataMedarbeidere.Find(m => m.ForLoepenr == forbindelse.ForLoepenr);
                    if (medarbeider is null)
                    {
                        bool inkludert = false;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            List<FaKlienttilknytning> klienttilknytninger = await context.FaKlienttilknytnings.Where(f => f.ForLoepenr == forbindelse.ForLoepenr).ToListAsync();
                            foreach (FaKlienttilknytning klienttilknytning in klienttilknytninger)
                            {
                                if (mappings.IsOwner(klienttilknytning.KliLoepenr))
                                {
                                    inkludert = true;
                                    break;
                                }
                                else
                                {
                                    int tilsyn = await context.FaKlients.Where(k => k.KliLoepenr == klienttilknytning.KliLoepenr && k.KliFraannenkommune == 1).CountAsync();
                                    if (tilsyn > 0)
                                    {
                                        inkludert = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!inkludert)
                        {
                            continue;
                        }
                    }
                    Innbygger innbygger = new()
                    {
                        actorId = GetActorId(forbindelse, false),
                        fornavn = forbindelse.ForFornavn?.Trim(),
                        etternavn = forbindelse.ForEtternavn?.Trim(),
                        sivilstand = "UOPPGITT",
                        kontonummer = forbindelse.ForKontonummer?.Trim(),
                        foedeland = mappings.GetLand(forbindelse.NasKodenr),
                        deaktiver = false
                    };
                    if (!string.IsNullOrEmpty(innbygger.kontonummer) && (innbygger.kontonummer.Length != 11 || !IsValidMod11(innbygger.kontonummer)))
                    {
                        innbygger.kontonummer = null;
                    }
                    else if (!string.IsNullOrEmpty(innbygger.kontonummer))
                    {
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            List<FaForbindelser> kontonummerForbindelser = await context.FaForbindelsers.Where(f => f.ForKontonummer == forbindelse.ForKontonummer).OrderByDescending(f => f.ForLoepenr).ToListAsync();
                            if (kontonummerForbindelser.Count > 1 && kontonummerForbindelser[0].ForLoepenr != forbindelse.ForLoepenr)
                            {
                                innbygger.kontonummer = null;
                            }
                        }
                    }
                    if (medarbeider is not null)
                    {
                        int numberOfActiveContracts = 0;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            numberOfActiveContracts = await context.FaEngasjementsavtales.Where(e => e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                                && medarbeider.ForLoepenr == e.ForLoepenr
                                && (e.EngTildato >= FirstInYearOfMigration)).CountAsync();
                        }
                        if (numberOfActiveContracts > 0)
                        {
                            bool actorAlreadyWritten = false;

                            SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                            SqlDataReader readerMigrering = null;
                            try
                            {
                                connectionMigrering.Open();

                                SqlCommand commandMigrering = new($"Select Count(*) From Oppdragstakere{MigreringsdbPostfix} Where AktorId='{innbygger.actorId}'", connectionMigrering)
                                {
                                    CommandTimeout = 300
                                };
                                readerMigrering = commandMigrering.ExecuteReader();
                                while (readerMigrering.Read())
                                {
                                    if (readerMigrering.GetInt32(0) > 0)
                                    {
                                        actorAlreadyWritten = true;
                                    }
                                }
                                readerMigrering.Close();

                                if (!actorAlreadyWritten)
                                {
                                    commandMigrering = new($"Insert Into Oppdragstakere{MigreringsdbPostfix} (AktorId,Bydel,Dato) Values ('{innbygger.actorId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                    {
                                        CommandTimeout = 300
                                    };
                                    commandMigrering.ExecuteNonQuery();
                                }
                            }
                            finally
                            {
                                connectionMigrering.Close();
                            }
                            if (actorAlreadyWritten)
                            {
                                continue;
                            }
                            innbygger.oppdragstaker = true;
                        }
                    }
                    else
                    {
                        innbygger.oppdragstaker = false;
                    }
                    if (forbindelse.ForRegistrertdato.HasValue)
                    {
                        innbygger.registrertDato = forbindelse.ForRegistrertdato.Value;
                    }
                    else
                    {
                        innbygger.registrertDato = LastDateNoMigration;
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForFoedselsnummer) && (forbindelse.ForFoedselsnummer.EndsWith("00000") || forbindelse.ForFoedselsnummer.EndsWith("00100") || forbindelse.ForFoedselsnummer.EndsWith("00200")))
                    {
                        innbygger.fodselsnummer = null;
                        if (forbindelse.ForDnummer.HasValue && forbindelse.ForDnummer.Value.ToString().Length == 11)
                        {
                            innbygger.fodselsnummer = forbindelse.ForDnummer.Value.ToString();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(forbindelse.ForFoedselsnummer) && forbindelse.ForFoedselsnummer.Length == 11 && IsDigitsOnly(forbindelse.ForFoedselsnummer))
                        {
                            innbygger.fodselsnummer = forbindelse.ForFoedselsnummer;
                            int kjonnsTall = int.Parse(forbindelse.ForFoedselsnummer[8].ToString());
                            if (kjonnsTall % 2 == 0)
                            {
                                innbygger.kjonn = "KVINNE";
                            }
                            else
                            {
                                innbygger.kjonn = "MANN";
                            }
                            int year = int.Parse(forbindelse.ForFoedselsnummer.Substring(4, 2));
                            if (year < 24)
                            {
                                year += 2000;
                            }
                            {
                                year += 1900;
                            }
                            int dag = int.Parse(forbindelse.ForFoedselsnummer[..2]);
                            if (dag > 40)
                            {
                                dag -= 40;
                            }
                            innbygger.fodselDato = new DateTime(year, int.Parse(forbindelse.ForFoedselsnummer.Substring(2, 2)), dag);
                        }
                        else
                        {
                            if (forbindelse.ForDnummer.HasValue && forbindelse.ForDnummer.Value.ToString().Length == 11)
                            {
                                innbygger.fodselsnummer = forbindelse.ForDnummer.Value.ToString();
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(innbygger.fodselsnummer))
                    {
                        innbygger.ukjentPerson = true;
                    }
                    bool hovetelefonBestemt = false;
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonmobil))
                    {
                        AktørTelefonnummer aktørTelefonnummerMobil = new()
                        {
                            telefonnummerType = "PRIVAT",
                            telefonnummer = GetTelefonnummer(forbindelse.ForTelefonmobil?.Trim()),
                            hovedtelefon = true
                        };
                        hovetelefonBestemt = true;
                        innbygger.telefonnummer.Add(aktørTelefonnummerMobil);
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonprivat))
                    {
                        AktørTelefonnummer aktørTelefonnummerPrivat = new()
                        {
                            telefonnummerType = "PRIVAT",
                            telefonnummer = GetTelefonnummer(forbindelse.ForTelefonprivat?.Trim())
                        };
                        if (!hovetelefonBestemt)
                        {
                            aktørTelefonnummerPrivat.hovedtelefon = true;
                            hovetelefonBestemt = true;
                        }
                        innbygger.telefonnummer.Add(aktørTelefonnummerPrivat);
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonarbeid))
                    {
                        AktørTelefonnummer aktørTelefonnummerArbeid = new()
                        {
                            telefonnummerType = "JOBB",
                            telefonnummer = GetTelefonnummer(forbindelse.ForTelefonarbeid?.Trim())
                        };
                        if (!hovetelefonBestemt)
                        {
                            aktørTelefonnummerArbeid.hovedtelefon = true;
                            hovetelefonBestemt = true;
                        }
                        innbygger.telefonnummer.Add(aktørTelefonnummerArbeid);
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForPostadresse) || !string.IsNullOrEmpty(forbindelse.ForBesoeksadresse))
                    {
                        AktørAdresse adresse = new()
                        {
                            adresseId = AddBydel(forbindelse.ForLoepenr.ToString(), "ADR"),
                            adresseType = "POSTADRESSE",
                            postnummer = forbindelse.PnrPostnr?.Trim(),
                            hovedadresse = false
                        };
                        if (string.IsNullOrEmpty(adresse.postnummer))
                        {
                            adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                        }
                        if (!string.IsNullOrEmpty(forbindelse.ForPostadresse))
                        {
                            adresse.linje1 = forbindelse.ForPostadresse?.Trim();
                            if (!string.IsNullOrEmpty(forbindelse.ForBesoeksadresse))
                            {
                                adresse.linje2 = forbindelse.ForBesoeksadresse?.Trim();
                            }
                        }
                        else
                        {
                            adresse.linje1 = forbindelse.ForBesoeksadresse?.Trim();
                        }
                        innbygger.adresse.Add(adresse);
                    }
                    innbyggere.Add(innbygger);
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Innbygger> innbyggereDistinct = innbyggere.GroupBy(c => c.actorId).Select(s => s.First()).ToList();
                int antallEntiteter = innbyggereDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Innbygger> innbyggerePart = innbyggereDistinct.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(innbyggerePart, GetJsonFileName("innbygger", $"Innbyggere{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall innbyggere: {antall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Organisasjoner
        public async Task<string> GetOrganisasjonerAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk organisasjoner...");
                List<FaForbindelser> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation)
                        .Where(KlientTilknytningFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .Where(f => string.IsNullOrEmpty(f.ForLoepenrNavigation.ForGmlreferanse) && !string.IsNullOrEmpty(f.ForLoepenrNavigation.ForOrganisasjonsnr) && f.ForLoepenrNavigation.ForOrganisasjonsnr.Length == 9 && f.ForLoepenrNavigation.ForOrganisasjonsnr.IndexOf(' ') == -1)
                        .Select(m => m.ForLoepenrNavigation)
                        .Distinct()
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Organisasjon> organisasjoner = new();
                int antall = 0;

                foreach (var forbindelse in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk organisasjoner ({antall} av {totalAntall})...");
                    }
                    Organisasjon organisasjon = new()
                    {
                        actorId = GetActorId(forbindelse, true),
                        organisasjonsnummer = GetOversattOrganisasjonsnr(forbindelse.ForOrganisasjonsnr?.Trim(), forbindelse.ForLoepenr),
                        deaktiver = false
                    };
                    if (!IsDigitsOnly(organisasjon.organisasjonsnummer))
                    {
                        continue;
                    }
                    organisasjon.eksternId = organisasjon.actorId;

                    GetOrganisasjonsKategori(organisasjon, forbindelse);

                    string betalingsmåte = forbindelse.ForBetalingsmaate?.Trim().ToUpper();
                    if (betalingsmåte == "L" || betalingsmåte == "I" || betalingsmåte == "F")
                    {
                        organisasjon.leverandørAvTiltak = "JA_ALM_TILTAK";
                    }
                    bool hovetelefonBestemt = false;
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonarbeid))
                    {
                        AktørTelefonnummer aktørTelefonnummerArbeid = new()
                        {
                            telefonnummerType = "ANNET",
                            telefonnummer = GetTelefonnummer(forbindelse.ForTelefonarbeid?.Trim()),
                            hovedtelefon = true
                        };
                        hovetelefonBestemt = true;
                        organisasjon.telefonnummer.Add(aktørTelefonnummerArbeid);
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonmobil))
                    {
                        AktørTelefonnummer aktørTelefonnummerMobil = new()
                        {
                            telefonnummerType = "ANNET",
                            telefonnummer = GetTelefonnummer(forbindelse.ForTelefonmobil?.Trim())
                        };
                        if (!hovetelefonBestemt)
                        {
                            aktørTelefonnummerMobil.hovedtelefon = true;
                            hovetelefonBestemt = true;
                        }
                        organisasjon.telefonnummer.Add(aktørTelefonnummerMobil);
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonprivat))
                    {
                        AktørTelefonnummer aktørTelefonnummerPrivat = new()
                        {
                            telefonnummerType = "ANNET",
                            telefonnummer = GetTelefonnummer(forbindelse.ForTelefonprivat?.Trim())
                        };
                        if (!hovetelefonBestemt)
                        {
                            aktørTelefonnummerPrivat.hovedtelefon = true;
                            hovetelefonBestemt = true;
                        }
                        organisasjon.telefonnummer.Add(aktørTelefonnummerPrivat);
                    }
                    if (!string.IsNullOrEmpty(forbindelse.ForPostadresse) || !string.IsNullOrEmpty(forbindelse.ForBesoeksadresse))
                    {
                        AktørAdresse adresse = new()
                        {
                            adresseId = AddBydel(forbindelse.ForLoepenr.ToString(), "ADR"),
                            adresseType = "POSTADRESSE",
                            postnummer = forbindelse.PnrPostnr?.Trim(),
                            hovedadresse = false
                        };
                        if (string.IsNullOrEmpty(adresse.postnummer))
                        {
                            adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                        }
                        if (!string.IsNullOrEmpty(forbindelse.ForPostadresse))
                        {
                            adresse.linje1 = forbindelse.ForPostadresse?.Trim();
                            if (!string.IsNullOrEmpty(forbindelse.ForBesoeksadresse))
                            {
                                adresse.linje2 = forbindelse.ForBesoeksadresse?.Trim();
                            }
                        }
                        else
                        {
                            adresse.linje1 = forbindelse.ForBesoeksadresse?.Trim();
                        }
                        organisasjon.adresse.Add(adresse);
                    }
                    organisasjoner.Add(organisasjon);
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Organisasjon> organisasjonerDistinct = organisasjoner.GroupBy(c => c.actorId).Select(s => s.First()).ToList();
                int antallEntiteter = organisasjonerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Organisasjon> organisasjonerPart = organisasjonerDistinct.OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(organisasjonerPart, GetJsonFileName("organisasjon", $"Organisasjoner{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall organisasjoner: {antall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Barnets nettverk - Barn
        public async Task<string> GetBarnetsNettverkBarnAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk barnets nettverk - barnet...");
                List<FaKlient> rawData;
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaKlients
                        .Where(KlientFilter())
                        .Where(KlientKunGyldigeTilsyn())
                        .OrderBy(o => o.KliLoepenr)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<BarnetsNettverk> forbindeler = new();
                int antall = 0;

                foreach (var klient in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk barnets nettverk - barnet ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(klient.KliLoepenr) && klient.KliFraannenkommune == 0)
                    {
                        continue;
                    }
                    BarnetsNettverk forbindelse = new()
                    {
                        sakId = GetSakId(klient.KliLoepenr.ToString()),
                        actorId = GetActorId(klient),
                        relasjonTilSak = "HOVEDPERSON",
                        rolle = "HOVEDPERSON",
                        foresatt = false
                    };
                    forbindeler.Add(forbindelse);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<BarnetsNettverk> forbindelerDistinct = forbindeler.GroupBy(c => new { c.sakId, c.actorId }).Select(s => s.First()).ToList();
                int antallEntiteter = forbindelerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<BarnetsNettverk> forbindelerPart = forbindelerDistinct.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(forbindelerPart, GetJsonFileName("barnetsNettverk", $"BarnetsNettverkBarn{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall barnets nettverk - barnet: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Barnets nettverk
        public async Task<string> GetBarnetsNettverkAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk barnets nettverk...");
                List<FaKlienttilknytning> rawData;
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    var rollerInkludert = new string[] { "MOR", "FAR", "SØS", "FMO", "FFA", "FAM", "VRG", "BRH", "BSH", "FSA" };
                    rawData = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation).Include(m => m.KliLoepenrNavigation)
                        .Where(KlientTilknytningFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .Where(k => string.IsNullOrEmpty(k.ForLoepenrNavigation.ForGmlreferanse) && ((!string.IsNullOrEmpty(k.ForLoepenrNavigation.ForFoedselsnummer) || k.ForLoepenrNavigation.ForDnummer.HasValue || (!string.IsNullOrEmpty(k.ForLoepenrNavigation.ForOrganisasjonsnr) && k.ForLoepenrNavigation.ForOrganisasjonsnr.Length == 9 && k.ForLoepenrNavigation.ForOrganisasjonsnr.IndexOf(' ') == -1)) || (rollerInkludert.Contains(k.KtkRolle))))
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<BarnetsNettverk> forbindeler = new();
                int antall = 0;

                foreach (var klientTilknytning in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk barnets nettverk ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(klientTilknytning.KliLoepenr) && klientTilknytning.KliLoepenrNavigation.KliFraannenkommune == 0)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(klientTilknytning.ForLoepenrNavigation.ForOrganisasjonsnr) && !IsDigitsOnly(klientTilknytning.ForLoepenrNavigation.ForOrganisasjonsnr))
                    {
                        continue;
                    }
                    BarnetsNettverk forbindelse = new()
                    {
                        sakId = GetSakId(klientTilknytning.KliLoepenr.ToString()),
                        actorId = GetActorId(klientTilknytning.ForLoepenrNavigation, true),
                        kommentar = klientTilknytning.KtkMerknad?.Trim()
                    };
                    if (klientTilknytning.KtkForesatt == 1)
                    {
                        forbindelse.foresatt = true;
                    }
                    else
                    {
                        forbindelse.foresatt = false;
                    }
                    GetNettverksRolle(klientTilknytning, forbindelse);
                    if (klientTilknytning.KtkRolle == "VRG" || klientTilknytning.KtkRolle == "AND")
                    {
                        forbindelse.foresatt = false;
                    }
                    forbindeler.Add(forbindelse);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<BarnetsNettverk> forbindelerDistinct = forbindeler.GroupBy(c => new { c.sakId, c.actorId }).Select(s => s.First()).ToList();
                int antallEntiteter = forbindelerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<BarnetsNettverk> forbindelerPart = forbindelerDistinct.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(forbindelerPart, GetJsonFileName("barnetsNettverk", $"BarnetsNettverk{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall barnets nettverk: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Meldinger
        public async Task<string> GetMeldingerAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk meldinger...");
                List<FaMeldinger> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMeldingers.Include(x => x.KliLoepenrNavigation)
                        .Where(KlientMeldingFilter())
                        .Where(m => m.MelAvsluttetgjennomgang.HasValue)
                        .OrderBy(o => o.KliLoepenr)
                        .Where(m => m.MelMeldingstype != "UGR")
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Melding> meldinger = new();
                List<DocumentToInclude> documentsIncluded = new();
                int migrertAntall = await UttrekkMeldingerAsync(false, rawData, meldinger, documentsIncluded, worker, totalAntall, "meldinger");
                await GetDocumentsAsync(worker, "Meldinger", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                List<Melding> meldingerDistinct = meldinger.GroupBy(c => c.meldingId).Select(s => s.First()).ToList();
                int antallEntiteter = meldingerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Melding> meldingerPart = meldingerDistinct.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(meldingerPart, GetJsonFileName("melding", $"Meldinger{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall meldinger: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> UttrekkMeldingerAsync(bool utenSak, List<FaMeldinger> rawData, List<Melding> meldinger, List<DocumentToInclude> documentsIncluded, BackgroundWorker worker, int totalAntall, string title, string sakId = "")
        {
            int antall = 0;
            int migrertAntall = 0;
            decimal behandlingsId = 0;

            try
            {
                foreach (var meldingFamilia in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk {title} ({antall} av {totalAntall})...");
                    }
                    behandlingsId = meldingFamilia.MelLoepenr;
                    if (!utenSak && !mappings.IsOwner(meldingFamilia.KliLoepenr.Value))
                    {
                        continue;
                    }
                    Melding melding = new()
                    {
                        meldingId = AddBydel(meldingFamilia.MelLoepenr.ToString(), "MEL")
                    };
                    if (utenSak)
                    {
                        if (!string.IsNullOrEmpty(sakId))
                        {
                            melding.sakId = sakId;
                        }
                        else
                        {
                            melding.sakId = GetSakId(meldingFamilia.MelLoepenr.ToString() + "__MUS");
                            if (string.IsNullOrEmpty(meldingFamilia.MelGmlreferanse) && meldingFamilia.MelPersonnr.HasValue && meldingFamilia.MelFoedselsdato.HasValue && meldingFamilia.MelPersonnr.GetValueOrDefault() != 99999 && meldingFamilia.MelPersonnr.GetValueOrDefault() != 00100 && meldingFamilia.MelPersonnr.GetValueOrDefault() != 00200)
                            {
                                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                                {
                                    FaKlient klientSak = await context.FaKlients.Where(k => k.KliFoedselsdato.HasValue && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue)).Where(k => k.KliFraannenkommune == 0 && k.KliFoedselsdato == meldingFamilia.MelFoedselsdato && k.KliPersonnr == meldingFamilia.MelPersonnr).FirstOrDefaultAsync();
                                    if (klientSak != null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        FaMeldinger firstMelding = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak && m.MelFoedselsdato == meldingFamilia.MelFoedselsdato && m.MelPersonnr == meldingFamilia.MelPersonnr).OrderByDescending(m => m.MelLoepenr).FirstOrDefaultAsync();
                                        if (firstMelding != null)
                                        {
                                            if (meldingFamilia.MelLoepenr != firstMelding.MelLoepenr)
                                            {
                                                melding.sakId = GetSakId(firstMelding.MelLoepenr.ToString() + "__MUS");
                                            }
                                        }
                                    }
                                }
                             }
                        };
                    }
                    else
                    {
                        melding.sakId = GetSakId(meldingFamilia.KliLoepenr.ToString());
                    }
                    melding.mottattBekymringsmelding = GetMottattBekymringsmelding(meldingFamilia, Bydelsforkortelse);
                    melding.behandlingAvBekymringsmelding = GetBehandlingAvBekymringsmelding(meldingFamilia, Bydelsforkortelse);
                    melding.tilbakemeldingTilMelder = await GetTilbakemeldingTilMelderAsync(meldingFamilia, Bydelsforkortelse);

                    if (meldingFamilia.PosMottattbrevAar.HasValue && meldingFamilia.PosMottattbrevLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = null;
                        FaDokumenter dokument = null;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            postJournal = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && meldingFamilia.PosMottattbrevAar.HasValue && p.PosAar == meldingFamilia.PosMottattbrevAar.Value && meldingFamilia.PosMottattbrevLoepenr.HasValue && p.PosLoepenr == meldingFamilia.PosMottattbrevLoepenr.Value && p.PosFerdigdato.HasValue).FirstOrDefaultAsync();
                            if (postJournal != null)
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && postJournal.DokLoepenr.HasValue && d.DokLoepenr == postJournal.DokLoepenr.Value).FirstOrDefaultAsync();
                            }
                        }
                        if (dokument != null)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = meldingFamilia.MelDokumentnr,
                                sakId = melding.sakId,
                                tittel = "Meldingsdokument",
                                journalDato = melding.mottattBekymringsmelding.mottattDato,
                                opprettetAvId = melding.mottattBekymringsmelding.utfortAvId
                            };
                            if (postJournal.PosUnndrattinnsyn == 1)
                            {
                                documentToInclude.merknadInnsyn = "Undratt innsyn";
                            }
                            else if (postJournal.PosVurderUnndratt == 1)
                            {
                                documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                            }
                            documentToInclude.aktivitetIdListe.Add(melding.meldingId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    if ((meldingFamilia.PosGjennomdokAar.HasValue && meldingFamilia.PosGjennomdokLoepenr.HasValue) || meldingFamilia.DokLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = null;
                        FaDokumenter dokument = null;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            postJournal = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && meldingFamilia.PosGjennomdokAar.HasValue && p.PosAar == meldingFamilia.PosGjennomdokAar.Value && meldingFamilia.PosGjennomdokLoepenr.HasValue && p.PosLoepenr == meldingFamilia.PosGjennomdokLoepenr.Value && p.PosFerdigdato.HasValue).FirstOrDefaultAsync();
                            if (postJournal != null)
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && postJournal.DokLoepenr.HasValue && d.DokLoepenr == postJournal.DokLoepenr.Value).FirstOrDefaultAsync();
                            }
                            else
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && meldingFamilia.DokLoepenr.HasValue && d.DokLoepenr == meldingFamilia.DokLoepenr).FirstOrDefaultAsync();
                            }
                        }
                        if (dokument != null)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                sakId = melding.sakId,
                                tittel = "Meldingsgjennomgang",
                                journalDato = melding.behandlingAvBekymringsmelding.konklusjonsdato,
                                opprettetAvId = melding.behandlingAvBekymringsmelding.utfortAvId
                            };
                            if (postJournal != null)
                            {
                                documentToInclude.dokumentNr = postJournal.PosDokumentnr;
                            }
                            documentToInclude.aktivitetIdListe.Add(melding.behandlingAvBekymringsmelding.behandlingId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    if (meldingFamilia.PosSendtkonklAar.HasValue && meldingFamilia.PosSendtkonklLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = null;
                        FaDokumenter dokument = null;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            postJournal = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && meldingFamilia.PosSendtkonklAar.HasValue && p.PosAar == meldingFamilia.PosSendtkonklAar.Value && meldingFamilia.PosSendtkonklLoepenr.HasValue && p.PosLoepenr == meldingFamilia.PosSendtkonklLoepenr.Value && p.PosFerdigdato.HasValue).FirstOrDefaultAsync();
                            if (postJournal != null)
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && postJournal.DokLoepenr.HasValue && d.DokLoepenr == postJournal.DokLoepenr.Value).FirstOrDefaultAsync();
                            }
                        }
                        if (dokument != null)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = postJournal.PosDokumentnr,
                                sakId = melding.sakId,
                                tittel = "Tilbakemelding til melder",
                                journalDato = melding.tilbakemeldingTilMelder.utfortDato,
                                opprettetAvId = melding.tilbakemeldingTilMelder.utfortAvId
                            };
                            if (postJournal.PosUnndrattinnsyn == 1)
                            {
                                documentToInclude.merknadInnsyn = "Undratt innsyn";
                            }
                            else if (postJournal.PosVurderUnndratt == 1)
                            {
                                documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                            }
                            documentToInclude.aktivitetIdListe.Add(melding.tilbakemeldingTilMelder.tilbakemeldingId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    meldinger.Add(melding);
                    migrertAntall += 1;
                }
                return migrertAntall;
            }
            catch (Exception ex)
            {
                string message = $"Behandlingsnr: {antall} Id: {behandlingsId} Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Meldinger uten sak
        public async Task<string> GetMeldingerUtenSakAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk meldinger uten sak...");
                List<FaMeldinger> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Melding> meldinger = new();
                List<DocumentToInclude> documentsIncluded = new();
                int migrertAntall = await UttrekkMeldingerAsync(true, rawData, meldinger, documentsIncluded, worker, totalAntall, "meldinger uten sak"); ;
                await GetDocumentsAsync(worker, "MeldingerUtensak", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                List<Melding> meldingerDistinct = meldinger.GroupBy(c => c.meldingId).Select(s => s.First()).ToList();
                int antallEntiteter = meldingerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Melding> meldingerPart = meldingerDistinct.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(meldingerPart, GetJsonFileName("melding", $"MeldingerUtenSak{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall meldinger uten sak: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task<string> GetMeldingerUtenSakForKlientAsync(BackgroundWorker worker, decimal kliLoepenr, Sak sak, DateTime fodselsDato, decimal personNummer)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk meldinger uten sak for klient...");
                List<FaMeldinger> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.MelMottattdato >= FirstDateOfMigrationMeldingerUtenSak
                        && m.MelFoedselsdato == fodselsDato && m.MelPersonnr == personNummer).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Melding> meldinger = new();
                List<DocumentToInclude> documentsIncluded = new();
                int migrertAntall = await UttrekkMeldingerAsync(true, rawData, meldinger, documentsIncluded, worker, totalAntall, "meldinger uten sak", sak.sakId); ;
                await GetDocumentsAsync(worker, "MeldingerUtensak", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                List<Melding> meldingerDistinct = meldinger.GroupBy(c => c.meldingId).Select(s => s.First()).ToList();
                int antallEntiteter = meldingerDistinct.Count;
                foreach (Melding melding in meldingerDistinct)
                {
                    if (melding.mottattBekymringsmelding.mottattDato < sak.startDato)
                    {
                        sak.startDato = melding.mottattBekymringsmelding.mottattDato;
                    }
                }
                while (antallEntiteter > toSkip)
                {
                    List<Melding> meldingerPart = meldingerDistinct.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(meldingerPart, GetJsonFileName("melding", $"MeldingerUtenSakKlient{kliLoepenr}{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall meldinger uten sak for klient: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Undersøkelser
        public async Task<string> GetUndersokelserAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk undersøkelser...");
                List<FaUndersoekelser> rawData;
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaUndersoekelsers.Include(x => x.MelLoepenrNavigation).Include(x => x.MelLoepenrNavigation.KliLoepenrNavigation)
                        .Where(KlientUndersøkelseFilter())
                        .OrderBy(o => o.MelLoepenrNavigation.KliLoepenrNavigation.KliLoepenr)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Undersøkelse> undersøkelser = new();
                List<Aktivitet> undersøkelsesAktiviteter = new();
                List<DocumentToInclude> documentsIncluded = new();
                List<TextDocumentToInclude> textDocumentsIncluded = new();
                int antall = 0;

                foreach (var undersøkelse in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk undersøkelser ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(undersøkelse.MelLoepenrNavigation.KliLoepenr.Value))
                    {
                        continue;
                    }
                    string sakStatus = "";
                    DateTime? sakSendtfylke  = null;
                    Undersøkelse undersoekelse = new()
                    {
                        undersokelseId = AddBydel(undersøkelse.MelLoepenr.ToString(), "UND"),
                        meldingId = AddBydel(undersøkelse.MelLoepenr.ToString(), "MEL"),
                        sakId = GetSakId(undersøkelse.MelLoepenrNavigation.KliLoepenr?.ToString()),
                        startDato = undersøkelse.UndStartdato
                    };
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaSaksjournal saksJournal = null;
                        if (undersøkelse.UndKonklusjon?.Trim() == "1")
                        {
                            saksJournal = await context.FaSaksjournals.Where(m => m.MelLoepenr == undersøkelse.MelLoepenr).FirstOrDefaultAsync();
                        }
                        else
                        {
                            saksJournal = await context.FaSaksjournals.Where(m => m.MelLoepenr == undersøkelse.MelLoepenr && m.SakStatus != "BEH").FirstOrDefaultAsync();
                        }
                        if (saksJournal != null)
                        {
                            undersoekelse.vedtakAktivitetId = AddBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED");
                            sakStatus = saksJournal.SakStatus;
                            sakSendtfylke = saksJournal.SakSendtfylke;
                        }
                    }
                    if (undersøkelse.UndRegistrertdato.HasValue)
                    {
                        undersoekelse.opprettetDato = undersøkelse.UndRegistrertdato.Value;
                    }
                    bool henlegges = false;
                    if (!string.IsNullOrEmpty(undersøkelse.UndKonklusjon))
                    {
                        if (undersøkelse.UndKonklusjon == "2" || undersøkelse.UndKonklusjon == "3" || undersøkelse.UndKonklusjon == "4" || undersøkelse.UndKonklusjon == "5" || undersøkelse.UndKonklusjon == "H")
                        {
                            undersoekelse.konklusjonsDato = undersøkelse.UndHenlagtdato;
                        }
                        else
                        {
                            if (undersøkelse.UndKonklusjon == "0" || undersøkelse.UndKonklusjon == "1" || undersøkelse.UndKonklusjon == "V")
                            {
                                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                                {
                                    List<FaSaksjournal> saksjournals = await context.FaSaksjournals.Where(s => s.MelLoepenr == undersøkelse.MelLoepenr).ToListAsync();
                                    if (saksjournals is not null && saksjournals.Count > 0)
                                    {
                                        if (undersøkelse.UndKonklusjon == "1")
                                        {
                                            undersoekelse.konklusjonsDato = saksjournals[0].SakSendtfylke;
                                        }
                                        else
                                        {
                                            undersoekelse.konklusjonsDato = saksjournals[0].SakAvgjortdato;
                                        }
                                    }
                                }
                            }
                        }
                        switch (undersøkelse.UndKonklusjon?.Trim())
                        {
                            case "0":
                            case "V":
                                undersoekelse.konklusjon = "BARNEVERNSTJENESTEN_GJØR_VEDTAK_OM_TILTAK";
                                break;
                            case "1":
                                undersoekelse.konklusjon = "BEGJÆRING_OM_TILTAK_FOR_BARNEVERNS_OG_HELSENEMNDA";
                                break;
                            case "2":
                            case "H":
                                undersoekelse.konklusjon = "UNDERSØKELSEN_HENLEGGES_ETTER_BARNEVERNETS_VURDERING";
                                henlegges = true;
                                break;
                            case "3":
                                undersoekelse.konklusjon = "UNDERSØKELSE_HENLEGGES_ETTER_PARTEN_SITT_ØNSKE";
                                henlegges = true;
                                break;
                            case "4":
                                undersoekelse.konklusjon = "UNDERSØKELSE_HENLEGGES_PÅ_GRUNN_AV_FLYTTING";
                                henlegges = true;
                                break;
                            case "5":
                                undersoekelse.konklusjon = "UNDERSØKELSEN_HENLAGT_ETTER_HENVISNING_TIL_ANNEN_INSTANS";
                                henlegges = true;
                                break;
                            default:
                                break;
                        }
                    }
                    if (undersoekelse.konklusjonsDato.HasValue)
                    {
                        undersoekelse.status = "KONKLUDERT_UNDERSØKELSE";
                    }
                    else
                    {
                        if (undersoekelse.startDato.HasValue)
                        {
                            undersoekelse.status = "IVERKSATT_UNDERSØKELSE";
                        }
                        else
                        {
                            undersoekelse.status = "VENTER_PÅ_UNDERSØKELSE";
                        }
                    }
                    if (undersøkelse.UndFristdato.HasValue)
                    {
                        undersoekelse.fristDato = undersøkelse.UndFristdato.Value;
                    }
                    else
                    {
                        if (undersøkelse.UndRegistrertdato.HasValue)
                        {
                            undersoekelse.fristDato = undersøkelse.UndRegistrertdato.Value.AddMonths(3);
                        }
                    }
                    if (undersøkelse.UndKonklusjon == "4")
                    {
                        if (undersøkelse.UndMeldtnykommune == 1)
                        {
                            undersoekelse.konklusjonPresisering = "Meldt ny kommune: Ja";
                        }
                        else
                        {
                            undersoekelse.konklusjonPresisering = "Meldt ny kommune: Nei";
                        }
                        if (!string.IsNullOrEmpty(undersøkelse.UndKonklusjontekst))
                        {
                            undersoekelse.konklusjonPresisering += " - " + undersøkelse.UndKonklusjontekst;
                        }
                    }
                    GetGrunnlagForTiltak(undersøkelse, undersoekelse, henlegges);
                    if (undersoekelse.konklusjon == "BEGJÆRING_OM_TILTAK_FOR_BARNEVERNS_OG_HELSENEMNDA" && sakStatus == "BEH")
                    {
                        Aktivitet saksframleggAktivitet = new()
                        {
                            aktivitetId = AddBydel(undersøkelse.MelLoepenr.ToString(), "SAKFR"),
                            sakId = undersoekelse.sakId,
                            aktivitetsType = "BARNEVERNS-_OG_HELSENEMND_OG_RETTSPROSESSER",
                            aktivitetsUnderType = "SAKSFREMLEGG",
                            status = "AKTIV",
                            saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer),
                            tittel = "Saksframlegg",
                            hendelsesdato = sakSendtfylke
                        };
                        undersoekelse.status = "IVERKSATT_UNDERSØKELSE";
                        undersoekelse.konklusjonsDato = null;
                        undersoekelse.vedtakAktivitetId = null;
                        undersøkelsesAktiviteter.Add(saksframleggAktivitet);
                    }
                    if (undersøkelse.UndBehandlingstid == "2")
                    {
                        Aktivitet undersøkelseUtvidelseFristAktivitet = new()
                        {
                            aktivitetId = AddBydel(undersøkelse.MelLoepenr.ToString(), "UNDUT"),
                            sakId = undersoekelse.sakId,
                            aktivitetsType = "UTVIDELSE_AV_FRIST",
                            aktivitetsUnderType = "UNDERSØKELSE",
                            status = "UTFØRT",
                            saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer),
                            tittel = "Beslutning om utvidet undersøkelsestid",
                            utfortAvId = GetBrukerId(undersøkelse.SbhInitialer),
                            notat = undersøkelse.Und6mndbegrunnelse,
                            fristDato = undersøkelse.UndFristdato,
                            fristLovpaalagt = true
                        };
                        if (string.IsNullOrEmpty(undersøkelse.SbhInitialer))
                        {
                            undersøkelseUtvidelseFristAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer);
                            undersøkelseUtvidelseFristAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer);
                            if (string.IsNullOrEmpty(undersøkelseUtvidelseFristAktivitet.utfortAvId))
                            {
                                undersøkelseUtvidelseFristAktivitet.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                undersøkelseUtvidelseFristAktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                            }
                        }
                        if (string.IsNullOrEmpty(undersøkelseUtvidelseFristAktivitet.notat))
                        {
                            undersøkelseUtvidelseFristAktivitet.notat = "Se dokument";
                        }
                        if (!undersøkelseUtvidelseFristAktivitet.fristDato.HasValue)
                        {
                            undersøkelseUtvidelseFristAktivitet.fristDato = undersoekelse.konklusjonsDato.Value.AddDays(180);
                        }
                        bool annetBrukt = false;
                        if (!string.IsNullOrEmpty(undersøkelse.FroKode1))
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add(mappings.GetÅrsakskodeUtvidelseFrist(undersøkelse.FroKode1));
                        }
                        if (!string.IsNullOrEmpty(undersøkelse.FroKode2))
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add(mappings.GetÅrsakskodeUtvidelseFrist(undersøkelse.FroKode2));
                        }
                        if (!string.IsNullOrEmpty(undersøkelse.FroKode3))
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add(mappings.GetÅrsakskodeUtvidelseFrist(undersøkelse.FroKode3));
                        }
                        if (undersøkelse.FroKode1 == "30" || undersøkelse.FroKode2 == "30" || undersøkelse.FroKode3 == "30")
                        {
                            annetBrukt = true;
                        }
                        if (undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Count == 0)
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add("30_ANNET_(BRUK_GJERNE_STIKKORD)");
                            annetBrukt = true;
                        }
                        if (annetBrukt)
                        {
                            undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse = undersøkelse.Und6mndbegrunnelse;
                            if (string.IsNullOrEmpty(undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse))
                            {
                                undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse = "Familia: Mangler presisering";
                            }
                        }
                        if (undersøkelseUtvidelseFristAktivitet.fristDato.HasValue)
                        {
                            undersøkelseUtvidelseFristAktivitet.hendelsesdato = undersøkelseUtvidelseFristAktivitet.fristDato.Value.AddDays(-90);
                            undersøkelseUtvidelseFristAktivitet.utfortDato = undersøkelseUtvidelseFristAktivitet.fristDato.Value.AddDays(-90);
                        }
                        undersoekelse.aktivitetIdListe.Add(undersøkelseUtvidelseFristAktivitet.aktivitetId);
                        TextDocumentToInclude textDocumentToInclude = new()
                        {
                            dokLoepenr = undersøkelse.MelLoepenr,
                            sakId = undersøkelseUtvidelseFristAktivitet.sakId,
                            tittel = undersøkelseUtvidelseFristAktivitet.tittel,
                            journalDato = undersøkelseUtvidelseFristAktivitet.utfortDato,
                            opprettetAvId = undersøkelseUtvidelseFristAktivitet.utfortAvId,
                            innhold = undersøkelseUtvidelseFristAktivitet.notat,
                            beskrivelse = undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse,
                            foedselsdato = undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation?.KliFoedselsdato,
                            forNavn = undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation?.KliFornavn,
                            etterNavn = undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation?.KliEtternavn
                        };
                        if (undersøkelseUtvidelseFristAktivitet.hendelsesdato.HasValue)
                        {
                            textDocumentToInclude.datonotat = undersøkelseUtvidelseFristAktivitet.hendelsesdato.Value;
                        }
                        textDocumentToInclude.aktivitetIdListe.Add(undersøkelseUtvidelseFristAktivitet.aktivitetId);
                        textDocumentsIncluded.Add(textDocumentToInclude);
                        undersøkelsesAktiviteter.Add(undersøkelseUtvidelseFristAktivitet);
                    }
                    else
                    {
                        undersoekelse.utvidetFrist = false;
                    }
                    if (undersøkelse.DokUplannr.HasValue && undersøkelse.UndFerdigdatoUplan.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == undersøkelse.DokUplannr.Value).FirstOrDefaultAsync();
                        }
                        if (dokument != null)
                        {
                            Aktivitet undersøkelsesplanAktivitet = new()
                            {
                                aktivitetId = AddBydel(undersøkelse.MelLoepenr.ToString(), "UNDPL"),
                                sakId = undersoekelse.sakId,
                                aktivitetsType = "UNDERSØKELSESPLAN",
                                aktivitetsUnderType = "UNDERSØKELSESPLAN",
                                status = "UTFØRT",
                                saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer),
                                tittel = "Undersøkelsesplan",
                                utfortAvId = GetBrukerId(undersøkelse.SbhInitialer),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdatoUplan,
                                utfortDato = undersøkelse.UndFerdigdatoUplan
                            };
                            if (string.IsNullOrEmpty(undersøkelse.SbhInitialer))
                            {
                                undersøkelsesplanAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer);
                                undersøkelsesplanAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer);
                                if (string.IsNullOrEmpty(undersøkelsesplanAktivitet.utfortAvId))
                                {
                                    undersøkelsesplanAktivitet.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                    undersøkelsesplanAktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                }
                            }
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = undersøkelse.UndDokumentnruplan,
                                sakId = undersoekelse.sakId,
                                tittel = undersøkelsesplanAktivitet.tittel,
                                journalDato = undersøkelsesplanAktivitet.utfortDato,
                                opprettetAvId = undersøkelsesplanAktivitet.utfortAvId
                            };
                            documentToInclude.aktivitetIdListe.Add(undersøkelsesplanAktivitet.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                            undersøkelsesAktiviteter.Add(undersøkelsesplanAktivitet);
                            undersoekelse.aktivitetIdListe.Add(undersøkelsesplanAktivitet.aktivitetId);
                        }
                    }
                    if (undersøkelse.DokLoepenr.HasValue && undersøkelse.UndFerdigdato.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == undersøkelse.DokLoepenr.Value).FirstOrDefaultAsync();
                        }
                        if (dokument != null)
                        {
                            Aktivitet undersøkelsesrapportAktivitet = new()
                            {
                                aktivitetId = AddBydel(undersøkelse.MelLoepenr.ToString(), "UNDRA"),
                                sakId = undersoekelse.sakId,
                                aktivitetsType = "UNDERSØKELSESRAPPORT",
                                aktivitetsUnderType = "UNDERSØKELSESRAPPORT",
                                status = "UTFØRT",
                                saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer),
                                tittel = "Sluttrapport undersøkelse",
                                utfortAvId = GetBrukerId(undersøkelse.SbhInitialer),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdato,
                                utfortDato = undersøkelse.UndFerdigdato
                            };
                            if (string.IsNullOrEmpty(undersøkelse.SbhInitialer))
                            {
                                if (!string.IsNullOrEmpty(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer))
                                {
                                    undersøkelsesrapportAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer);
                                    undersøkelsesrapportAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer2))
                                    {
                                        undersøkelsesrapportAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer2);
                                        undersøkelsesrapportAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer2);
                                    }
                                    else
                                    {
                                        undersøkelsesrapportAktivitet.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                        undersøkelsesrapportAktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                    }
                                }
                            }
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = undersøkelse.UndDokumentnr,
                                sakId = undersoekelse.sakId,
                                tittel = undersøkelsesrapportAktivitet.tittel,
                                journalDato = undersøkelsesrapportAktivitet.utfortDato,
                                opprettetAvId = undersøkelsesrapportAktivitet.utfortAvId
                            };
                            documentToInclude.aktivitetIdListe.Add(undersøkelsesrapportAktivitet.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                            undersøkelsesAktiviteter.Add(undersøkelsesrapportAktivitet);
                            undersoekelse.aktivitetIdListe.Add(undersøkelsesrapportAktivitet.aktivitetId);
                        }
                    }
                    undersøkelser.Add(undersoekelse);
                    migrertAntall += 1;
                }
                await GetTextDocumentsAsync(worker, textDocumentsIncluded);
                await GetDocumentsAsync(worker, "Undersøkelser", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> undersøkelsesAktiviteterDistinct = undersøkelsesAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int antallEntiteter = undersøkelsesAktiviteterDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> undersøkelsesAktiviteterPart = undersøkelsesAktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(undersøkelsesAktiviteterPart, GetJsonFileName("aktiviteter", $"UndersokelsesAktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                List<Undersøkelse> undersøkelserDistinct = undersøkelser.GroupBy(c => c.undersokelseId).Select(s => s.First()).ToList();
                antallEntiteter = undersøkelserDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Undersøkelse> undersøkelserPart = undersøkelserDistinct.OrderBy(o => o.sakId).OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(undersøkelserPart, GetJsonFileName("undersokelser", $"Undersokelser{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall undersøkelser: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Avdelingsmapping
        public async Task<string> GetAvdelingsmappingAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk avdelingsmappinger...");
                List<FaDistrikt> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaDistrikts.Where(d => !d.DisPassivisertdato.HasValue).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Avdelingsmapping> avdelingsmappinger = new();
                int antall = 0;

                foreach (var distrikt in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk avdelingsmappinger ({antall} av {totalAntall})...");
                    }
                    Avdelingsmapping avdelingsmapping = new()
                    {
                        avdelingId = GetEnhetskode(distrikt.DisDistriktskode),
                        enhetskodeModulusBarn = GetEnhetskode(distrikt.DisDistriktskode)
                    };
                    avdelingsmappinger.Add(avdelingsmapping);
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Avdelingsmapping> avdelingsmappingerDistinct = avdelingsmappinger.GroupBy(c => c.avdelingId).Select(s => s.First()).ToList();
                int antallEntiteter = avdelingsmappingerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Avdelingsmapping> avdelingsmappingerPart = avdelingsmappingerDistinct.OrderBy(o => o.avdelingId).OrderBy(o => o.enhetskodeModulusBarn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(avdelingsmappingerPart, GetJsonFileName("avdelingsmapping", $"Avdelingsmapping{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall avdelingsmappinger: {antall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Brukere
        public async Task<string> GetAlleBrukereFamiliaAsync(BackgroundWorker worker)
        {
            try
            {
                foreach (var bydel in mappings.GetAlleBydeler())
                {
                    Bydelsforkortelse = bydel;
                    ConnectionStringFamilia = mappings.GetConnectionstring(Bydelsforkortelse, MainDBServer);
                    await GetAvdelingsmappingAsync(worker);
                    await GetBrukereAsync(worker);
                }
                return "Uttrekk av alle avdelingsmappinger og brukere for alle bydeler utført.";
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task<string> GetBrukereAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk saksbehandlere...");
                List<FaSaksbehandlere> rawData;
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaSaksbehandleres.Include(d => d.DisDistriktskodes).Include(d => d.TggIdents).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Bruker> brukere = new();
                int antall = 0;
                foreach (var saksbehandler in rawData)
                {
                    antall += 1;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk saksbehandlere ({antall} av {totalAntall})...");
                    }
                    if (saksbehandler.TggIdents.Count == 1 && saksbehandler.TggIdents.First().TggIdent == "DRL")
                    {
                        continue;
                    }
                    string initialer = saksbehandler.SbhInitialer.Trim().ToUpper();
                    if (saksbehandler.TggIdents.Count == 1 && saksbehandler.TggIdents.First().TggIdent == "SEI")
                    {
                        if (initialer == "TRUE" || initialer == "SYS")
                        {
                            continue;
                        }
                        int antallSakerInvolvert = 0;

                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            antallSakerInvolvert += await context.FaKlients.Where(k =>
                                (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                (k.SbhInitialer2 != null && k.SbhInitialer2.ToUpper() == initialer) ||
                                (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                            if (antallSakerInvolvert == 0)
                            {
                                antallSakerInvolvert += await context.FaMeldingers.Where(k =>
                                    (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                    (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                    (k.SbhMottattav != null && k.SbhMottattav.ToUpper() == initialer) ||
                                    (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                if (antallSakerInvolvert == 0)
                                {
                                    antallSakerInvolvert += await context.FaUndersoekelsers.Where(k =>
                                        (k.SbhGodkjennSluttrapport != null && k.SbhGodkjennSluttrapport.ToUpper() == initialer) ||
                                        (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                        (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                        (k.SbhInitialer2 != null && k.SbhInitialer2.ToUpper() == initialer) ||
                                        (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                    if (antallSakerInvolvert == 0)
                                    {
                                        antallSakerInvolvert += await context.FaSaksjournals.Where(k =>
                                            (k.SbhOpphevetInitialer != null && k.SbhOpphevetInitialer.ToUpper() == initialer) ||
                                            (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                            (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                            (k.SbhBortfaltInitialer != null && k.SbhBortfaltInitialer.ToUpper() == initialer) ||
                                            (k.SbhAvgjortavInitialer != null && k.SbhAvgjortavInitialer.ToUpper() == initialer) ||
                                            (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                        if (antallSakerInvolvert == 0)
                                        {
                                            antallSakerInvolvert += await context.FaTiltaks.Where(k =>
                                                (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                                (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                                (k.SbhInitialer2 != null && k.SbhInitialer2.ToUpper() == initialer) ||
                                                (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                            if (antallSakerInvolvert == 0)
                                            {
                                                antallSakerInvolvert += await context.FaTiltaksplans.Where(k =>
                                                    (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                                    (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                                    (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                                if (antallSakerInvolvert == 0)
                                                {
                                                    antallSakerInvolvert += await context.FaPostjournals.Where(k =>
                                                        (k.SbhGodkjennInitialer != null && k.SbhGodkjennInitialer.ToUpper() == initialer) ||
                                                        (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                                        (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                                        (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                                    if (antallSakerInvolvert == 0)
                                                    {
                                                        antallSakerInvolvert += await context.FaJournals.Where(k =>
                                                            (k.SbhEndretav != null && k.SbhEndretav.ToUpper() == initialer) ||
                                                            (k.SbhInitialer != null && k.SbhInitialer.ToUpper() == initialer) ||
                                                            (k.SbhRegistrertav != null && k.SbhRegistrertav.ToUpper() == initialer)).CountAsync();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (antallSakerInvolvert == 0)
                        {
                            continue;
                        }
                    }
                    Bruker bruker = new()
                    {
                        brukerId = AddBydel(saksbehandler.SbhInitialer)
                    };
                    SqlConnection connection = new(ConnectionStringMigrering);
                    SqlDataReader reader = null;
                    try
                    {
                        connection.Open();
                        SqlCommand command = new($"Select PRKID,Fornavn,Etternavn,Epost From Brukere Where Upper(Virksomhet)='{Bydelsforkortelse}' And Upper(FamiliaID)='{initialer}'", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    bruker.okonomiEksternId = GetModifisertPRKId(Convert.ToInt32(reader["PRKID"]));
                                }
                                if (!string.IsNullOrEmpty(reader.GetString(1)))
                                {
                                    bruker.fulltNavn = reader.GetString(1).Trim();
                                }
                                if (!string.IsNullOrEmpty(reader.GetString(2)))
                                {
                                    if (!string.IsNullOrEmpty(bruker.fulltNavn))
                                    {
                                        bruker.fulltNavn += " ";
                                    }
                                    bruker.fulltNavn += reader.GetString(2).Trim();
                                }
                                bruker.email = reader.GetString(3);
                                bruker.brukerNokkelModulusBarn = bruker.email;
                                bruker.brukerId = bruker.email;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(saksbehandler.SbhFornavn))
                            {
                                bruker.fulltNavn = saksbehandler.SbhFornavn?.Trim();
                            }
                            if (!string.IsNullOrEmpty(saksbehandler.SbhEtternavn))
                            {
                                if (!string.IsNullOrEmpty(bruker.fulltNavn))
                                {
                                    bruker.fulltNavn += " ";
                                }
                                bruker.fulltNavn += saksbehandler.SbhEtternavn?.Trim();
                            }
                            bruker.brukerNokkelModulusBarn = bruker.brukerId;
                        }
                        reader.Close();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    List<FaDistrikt> distrikter = saksbehandler.DisDistriktskodes.Where(d => !d.DisPassivisertdato.HasValue).ToList();
                    foreach (var distrikt in distrikter)
                    {
                        bruker.enhetskodeModulusBarnListe.Add(GetEnhetskode(distrikt.DisDistriktskode));
                    }
                    brukere.Add(bruker);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Bruker> brukereDistinct = brukere.GroupBy(c => c.brukerId).Select(s => s.First()).ToList();
                int antallEntiteter = brukereDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Bruker> brukerePart = brukereDistinct.OrderBy(o => o.brukerId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(brukerePart, GetJsonFileName("brukere", $"Brukere{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall saksbehandlere: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Vedtak
        public async Task<string> GetVedtakAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk vedtak...");
                List<FaSaksjournal> rawData;
                List<DocumentToInclude> documentsIncluded = new();
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaSaksjournals.Include(x => x.KliLoepenrNavigation)
                        .Where(KlientSakJournalFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .Where(m => (m.SakStatus != "KLR" && m.SakStatus != "BEH") && !((m.MynVedtakstype == "FN" || m.MynVedtakstype == "LA" || m.MynVedtakstype == "TI") && !(m.SakStatus == "GOD" || m.SakStatus == "AVS" || m.SakStatus == "BOR")))
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Vedtak> vedtaksliste = new();
                int antall = 0;
                foreach (var saksJournal in rawData)
                {
                    antall += 1;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk vedtak ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(saksJournal.KliLoepenr) && saksJournal.KliLoepenrNavigation.KliFraannenkommune == 0)
                    {
                        continue;
                    }
                    Vedtak vedtak = new()
                    {
                        sakId = GetSakId(saksJournal.KliLoepenr.ToString()),
                        aktivitetId = AddBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED"),
                        tittel = saksJournal.SakEmne,
                        vedtaksdato = saksJournal.SakAvgjortdato,
                        startdato = saksJournal.SakIverksattdato,
                        barnetsMedvirkning = "Se dokument",
                        bakgrunnsopplysninger = "Se dokument",
                        vedtak = "Se dokument",
                        begrunnelse = "Se dokument",
                        godkjentStatusDato = saksJournal.SakAvgjortdato,
                        begjaeringOversendtNemndStatusDato = saksJournal.SakSendtfylke
                    };
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaTiltak tiltak = await context.FaTiltaks.Where(m => m.SakJournalnr == saksJournal.SakJournalnr && m.SakAar == saksJournal.SakAar && m.TilEttervern != null).FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            if (tiltak.TilEttervern == "1")
                            {
                                vedtak.arsakIkkeEttervern = "IKKE_VIDEREFØRT_ETTER_ØNSKE";
                            }
                            else if (tiltak.TilEttervern == "2")
                            {
                                vedtak.arsakIkkeEttervern = "IKKE_VIDEREFØRT_AVSLAG_KOMMUNE";
                            }
                            else if (tiltak.TilEttervern == "3")
                            {
                                vedtak.arsakIkkeEttervern = "AVSLÅTT_UNGDOM";
                            }
                        }
                    }
                    if (saksJournal.SakBehFylkesnemnda == "FE")
                    {
                        vedtak.behandlingIFylkesnemda = "FORENKLET";
                    }
                    else if (saksJournal.SakBehFylkesnemnda == "FT" || saksJournal.SakBehFylkesnemnda == "SP" || saksJournal.MynVedtakstype == "FN")
                    {
                        vedtak.behandlingIFylkesnemda = "FULLTALLIG";
                    }
                    string lovHovedParagraf = saksJournal.LovHovedParagraf?.Trim();
                    string lovJmfParagraf1 = saksJournal.LovJmfParagraf1?.Trim();
                    string lovJmfParagraf2 = saksJournal.LovJmfParagraf2?.Trim();
                    string mynVedtakstype = saksJournal.MynVedtakstype;

                    mynVedtakstype = GetVedtakstype(saksJournal, vedtak, lovHovedParagraf, lovJmfParagraf1, lovJmfParagraf2, mynVedtakstype);

                    if (!string.IsNullOrEmpty(saksJournal.SbhInitialer))
                    {
                        vedtak.saksbehandlerId = GetBrukerId(saksJournal.SbhInitialer);
                    }
                    if (!string.IsNullOrEmpty(saksJournal.SbhAvgjortavInitialer))
                    {
                        vedtak.godkjentAvSaksbehandlerId = GetBrukerId(saksJournal.SbhAvgjortavInitialer);
                    }
                    vedtak.aktivitetsUndertype = GetVedtakAktivitetsUnderType(mynVedtakstype, lovHovedParagraf, lovJmfParagraf1, lovJmfParagraf2, saksJournal.SakStatus);

                    if (vedtak.aktivitetsUndertype != "ADMINISTRATIV_BESLUTNING" && vedtak.aktivitetsUndertype == "AVSLUTNING_AV_BARNEVERNSSAK")
                    {
                        vedtak.avslutningsårsakPresisering = saksJournal.SakMerknaderAvslag;
                    }
                    if (saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR" || mynVedtakstype == "FN" || mynVedtakstype == "LA" || mynVedtakstype == "TI")
                    {
                        if (saksJournal.SakStatus == "AVS")
                        {
                            vedtak.beslutning = "KOMMUNEN_HAR_FÅTT_AVSLAG";
                        }
                        else
                        {
                            if (saksJournal.SakStatus == "GOD" || saksJournal.SakStatus == "BOR")
                            {
                                vedtak.beslutning = "KOMMUNEN_HAR_FÅTT_MEDHOLD";
                            }
                        }
                    }
                    if (saksJournal.SakAvgjortdato.HasValue || saksJournal.SakBortfaltdato.HasValue)
                    {
                        vedtak.status = "UTFØRT";
                    }
                    else
                    {
                        vedtak.status = "AKTIV";
                    }
                    if (mynVedtakstype == "BV")
                    {
                        vedtak.vedtakFraBarnevernsvakt = true;
                    }
                    else
                    {
                        vedtak.vedtakFraBarnevernsvakt = false;
                    }
                    if (saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR")
                    {
                        if (saksJournal.SakSlutningdato.HasValue)
                        {
                            vedtak.startdato = saksJournal.SakSlutningdato;
                        }
                    }
                    if (saksJournal.SakAvgjortetat == "FN")
                    {
                        vedtak.rettsinstans = "BARNEVERNSOGHELSENEMND";
                    }
                    else
                    {
                        if (saksJournal.SakAvgjortetat == "LR")
                        {
                            vedtak.rettsinstans = "LAGMANNSRETT";
                        }
                        else
                        {
                            if (saksJournal.SakAvgjortetat == "TR")
                            {
                                vedtak.rettsinstans = "TINGRETT";
                            }
                        }
                    }
                    if (vedtak.aktivitetsUndertype == "VEDTAK_FRA_RETTSINSTANSER" && string.IsNullOrEmpty(vedtak.rettsinstans))
                    {
                        vedtak.rettsinstans = "BARNEVERNSOGHELSENEMND";
                    }
                    if (saksJournal.SakStatus == "AVS")
                    {
                        vedtak.avsluttetStatusDato = saksJournal.SakAvgjortdato;
                        vedtak.godkjentStatusDato = null;
                    }
                    else if (saksJournal.SakStatus == "BOR" || saksJournal.SakStatus == "OHV")
                    {
                        vedtak.bortfaltStatusDato = saksJournal.SakBortfaltdato;
                        if (!vedtak.vedtaksdato.HasValue)
                        {
                            vedtak.vedtaksdato = vedtak.bortfaltStatusDato;
                        }
                        if (!vedtak.vedtaksdato.HasValue && !saksJournal.SakOpphevetdato.HasValue)
                        {
                            continue;
                        }
                    }
                    if (!vedtak.vedtaksdato.HasValue && vedtak.aktivitetsUndertype == "VEDTAK_FRA_RETTSINSTANSER" && (saksJournal.SakStatus == "BOR" || saksJournal.SakStatus == "OHV"))
                    {
                        vedtak.vedtaksdato = saksJournal.SakBortfaltdato;
                    }
                    if (vedtak.startdato.HasValue && vedtak.avsluttetStatusDato.HasValue && vedtak.startdato.Value > vedtak.avsluttetStatusDato.Value)
                    {
                        vedtak.startdato = vedtak.avsluttetStatusDato;
                    }
                    if (vedtak.startdato.HasValue && vedtak.startdato.Value.Year < 1998)
                    {
                        vedtak.startdato = saksJournal.SakAvgjortdato;
                    }
                    if (!vedtak.vedtaksdato.HasValue && vedtak.aktivitetsUndertype == "VEDTAK_FRA_RETTSINSTANSER")
                    {
                        vedtak.vedtaksdato = saksJournal.SakSlutningdato;
                    }
                    vedtaksliste.Add(vedtak);
                    migrertAntall += 1;

                    if (saksJournal.PosAar.HasValue && saksJournal.PosLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = await GetPostJournalAsync(saksJournal.PosAar, saksJournal.PosLoepenr);
                        if (postJournal != null && postJournal.DokLoepenr.HasValue)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = postJournal.DokLoepenr.Value,
                                dokumentNr = postJournal.PosDokumentnr,
                                sakId = vedtak.sakId,
                                tittel = vedtak.tittel,
                                journalDato = vedtak.vedtaksdato,
                                opprettetAvId = vedtak.saksbehandlerId
                            };
                            documentToInclude.aktivitetIdListe.Add(vedtak.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                }
                await GetDocumentsAsync(worker, "Vedtak", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                List<Vedtak> vedtakslisteDistinct = vedtaksliste.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int antallEntiteter = vedtakslisteDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Vedtak> vedtakslistePart = vedtakslisteDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(vedtakslistePart, GetJsonFileName("vedtak", $"Vedtak{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall vedtak: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private static string GetVedtakAktivitetsUnderType(string mynVedtakstype, string lovHovedParagraf, string lovJmfParagraf1, string lovJmfParagraf2, string sakStatus)
        {
            string aktivitetsUnderType = null;
            if (string.IsNullOrEmpty(lovHovedParagraf))
            {
                lovHovedParagraf = "";
            }
            if (string.IsNullOrEmpty(lovJmfParagraf1))
            {
                lovJmfParagraf1 = "";
            }
            if (string.IsNullOrEmpty(lovJmfParagraf2))
            {
                lovJmfParagraf2 = "";
            }

            switch (mynVedtakstype)
            {
                case "AV":
                    aktivitetsUnderType = "USPESIFISERT_VEDTAK";
                    if (lovHovedParagraf == "4-6,1.")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-6._1.LEDD";
                    }
                    else if (lovHovedParagraf == "4-6,2.")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-6._2.LEDD";
                    }
                    else if (lovHovedParagraf == "4-25,2.2")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-25._2.LEDD_2.PUNKTUM_JF_§4-24";
                    }
                    else if (lovHovedParagraf.StartsWith("4-29"))
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-29._4.LEDD";
                    }
                    else if (lovHovedParagraf.StartsWith("4-9") || lovJmfParagraf1.StartsWith("4-9") || lovJmfParagraf2.StartsWith("4-9"))
                    {
                        if (lovHovedParagraf == "4-8,2." || lovJmfParagraf1 == "4-8,2." || lovJmfParagraf2 == "4-8,2.")
                        {
                            aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-9_JF_§4-8_2.LEDD";
                        }
                        else if (lovHovedParagraf == "4-8,1." || lovJmfParagraf1 == "4-8,1." || lovJmfParagraf2 == "4-8,1.")
                        {
                            aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-9_JF_§4-8_1.LEDD";
                        }
                    }
                    else if (lovHovedParagraf.StartsWith("4-4") || lovJmfParagraf1.StartsWith("4-4") || lovJmfParagraf2.StartsWith("4-4")
                        || lovHovedParagraf == "1-3" || lovJmfParagraf1 == "1-3" || lovJmfParagraf2 == "1-3"
                        || lovHovedParagraf == "§ 3-1" || lovJmfParagraf1 == "§ 3-1" || lovJmfParagraf2 == "§ 3-1"
                        || lovHovedParagraf == "§ 3-2" || lovJmfParagraf1 == "§ 3-2" || lovJmfParagraf2 == "§ 3-2"
                        || lovHovedParagraf == "§ 3-6" || lovJmfParagraf1 == "§ 3-6" || lovJmfParagraf2 == "§ 3-6")
                    {
                        if (sakStatus == "GOD" || sakStatus == "BOR" || sakStatus == "OHV")
                        {
                            aktivitetsUnderType = "VEDTAK_OM_TILTAK";
                        }
                    }
                    else if (lovHovedParagraf.StartsWith("4-19") || lovJmfParagraf1.StartsWith("4-19") || lovJmfParagraf2.StartsWith("4-19")
                         || lovHovedParagraf == "§ 7-2,3." || lovJmfParagraf1 == "§ 7-2,3." || lovJmfParagraf2 == "§ 7-2,3.")
                    {
                        aktivitetsUnderType = "ADRESSESPERRE";
                    }
                    if (lovHovedParagraf == "§ 4-1")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_1";
                    }
                    else if (lovHovedParagraf.StartsWith("§ 4-2"))
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_2_JF_§_14_22";
                    }
                    else if (lovHovedParagraf == "§ 4-3")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_3_JF_§_14_22";
                    }
                    else if (lovHovedParagraf == "§ 4-4")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_4_JF_§_6_2_14_22";
                    }
                    else if (lovHovedParagraf == "§ 4-5")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_5_JF_§_14_22";
                    }
                    if (lovHovedParagraf == "4-17" || lovJmfParagraf1 == "4-17" || lovJmfParagraf2 == "4-17"
                        || lovHovedParagraf == "§ 5-5" || lovJmfParagraf1 == "§ 5-5" || lovJmfParagraf2 == "§ 5-5")
                    {
                        aktivitetsUnderType = "FLYTTING";
                    }
                    if (lovHovedParagraf == "fvl. 19" || lovJmfParagraf1 == "fvl. 19" || lovJmfParagraf2 == "fvl. 19" || lovHovedParagraf == "19a" || lovJmfParagraf1 == "19a" || lovJmfParagraf2 == "19a"
                        || lovHovedParagraf == "Fvl._§_19_(gammel_lov)" || lovJmfParagraf1 == "Fvl._§_19_(gammel_lov)" || lovJmfParagraf2 == "Fvl._§_19_(gammel_lov)")
                    {
                        aktivitetsUnderType = "UNNDRATT_INNSYN";
                    }
                    break;
                case "BV":
                    aktivitetsUnderType = "USPESIFISERT_VEDTAK";
                    if (lovHovedParagraf == "4-6,1.")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-6._1.LEDD";
                    }
                    else if (lovHovedParagraf == "4-6,2.")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-6._2.LEDD";
                    }
                    else if (lovHovedParagraf == "4-25,2.2")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-25._2.LEDD_2.PUNKTUM_JF_§4-24";
                    }
                    else if (lovHovedParagraf.StartsWith("4-29"))
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-29._4.LEDD";
                    }
                    else if (lovHovedParagraf.StartsWith("4-9") || lovJmfParagraf1.StartsWith("4-9") || lovJmfParagraf2.StartsWith("4-9"))
                    {
                        if (lovHovedParagraf == "4-8,2." || lovJmfParagraf1 == "4-8,2." || lovJmfParagraf2 == "4-8,2.")
                        {
                            aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-9_JF_§4-8_2.LEDD";
                        }
                        else if (lovHovedParagraf == "4-8,1." || lovJmfParagraf1 == "4-8,1." || lovJmfParagraf2 == "4-8,1.")
                        {
                            aktivitetsUnderType = "AKUTTVEDTAK_BVL._§4-9_JF_§4-8_1.LEDD";
                        }
                    }
                    if (lovHovedParagraf == "§ 4-1")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_1";
                    }
                    else if (lovHovedParagraf.StartsWith("§ 4-2"))
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_2_JF_§_14_22";
                    }
                    else if (lovHovedParagraf == "§ 4-3")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_3_JF_§_14_22";
                    }
                    else if (lovHovedParagraf == "§ 4-4")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_4_JF_§_6_2_14_22";
                    }
                    else if (lovHovedParagraf == "§ 4-5")
                    {
                        aktivitetsUnderType = "AKUTTVEDTAK_BVL._§_4_5_JF_§_14_22";
                    }
                    break;
                case "FM":
                    aktivitetsUnderType = "USPESIFISERT_VEDTAK";
                    break;
                case "FN":
                case "LA":
                case "TI":
                    aktivitetsUnderType = "VEDTAK_FRA_RETTSINSTANSER";
                    break;
                case "AB":
                case "OV":
                    aktivitetsUnderType = "ADMINISTRATIV_BESLUTNING";
                    break;
                default:
                    break;
            }
            return aktivitetsUnderType;
        }
        #endregion

        #region Tiltak
        public async Task<string> GetTiltakAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk tiltak...");
                List<FaTiltak> rawData;
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaTiltaks.Include(x => x.KliLoepenrNavigation).Include(x => x.Sak)
                        .Where(KlientTiltakFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Tiltak> tiltaksliste = new();
                List<DocumentToInclude> documentsIncluded = new();
                List<Aktivitet> oppdragstakeravtaleAktiviteter = new();
                List<Aktivitet> flyttingMedFosterhjemAktiviteter = new();

                int antall = 0;
                foreach (var tiltakFamilia in rawData)
                {
                    antall += 1;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk tiltak ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(tiltakFamilia.KliLoepenr))
                    {
                        continue;
                    }
                    Tiltak tiltak = new()
                    {
                        tiltakId = AddBydel(tiltakFamilia.TilLoepenr.ToString(), "TIL"),
                        sakId = GetSakId(tiltakFamilia.KliLoepenr.ToString()),
                        ssbHovedkategori = mappings.GetSSBHovedkategori(tiltakFamilia.TttTiltakstype),
                        ssbUnderkategori = mappings.GetSSBUnderkategori(tiltakFamilia.TttTiltakstype),
                        ssbUnderkategoriSpesifisering = tiltakFamilia.TilTiltakstypePres,
                        planlagtFraDato = tiltakFamilia.TilFradato,
                        planlagtTilDato = tiltakFamilia.TilTildato,
                        iverksattDato = tiltakFamilia.TilIverksattdato,
                        bortfaltDato = tiltakFamilia.TilBortfaltdato,
                        avsluttetDato = tiltakFamilia.TilAvsluttetdato,
                        notat = tiltakFamilia.TilKommentar
                    };
                    if (tiltak.iverksattDato.HasValue && tiltak.iverksattDato.Value > DateTime.Now)
                    {
                        tiltak.iverksattDato = DateTime.Now;
                    }
                    if (tiltak.planlagtFraDato.HasValue && tiltak.iverksattDato.HasValue && tiltak.planlagtFraDato.Value > tiltak.iverksattDato.Value)
                    {
                        tiltak.planlagtFraDato = tiltak.iverksattDato;
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_HJEMMEBASERTE_TILTAK" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_FOSTERHJEMSTILTAK" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_BOLIGTILTAK" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.planlagtTilDato.HasValue && tiltak.planlagtTilDato < tiltak.planlagtFraDato)
                    {
                        tiltak.planlagtTilDato = tiltak.planlagtFraDato;
                    }
                    if (tiltak.planlagtTilDato.HasValue && tiltak.planlagtTilDato < tiltak.iverksattDato)
                    {
                        tiltak.planlagtTilDato = null;
                    }
                    if (tiltakFamilia.TilUtenforhjemmet == 1)
                    {
                        tiltak.iEllerUtenforFamilie = "UTENFOR_FAMILIEN";
                    }
                    else
                    {
                        tiltak.iEllerUtenforFamilie = "I_FAMILIEN";
                    }
                    if (tiltakFamilia.SakAar.HasValue && tiltakFamilia.SakJournalnr.HasValue)
                    {
                        tiltak.aktivitetId = AddBydel(tiltakFamilia.SakAar.ToString() + "-" + tiltakFamilia.SakJournalnr.ToString(), "VED");
                    }
                    if (tiltak.avsluttetDato.HasValue && tiltak.iverksattDato.HasValue && tiltak.avsluttetDato.Value < tiltak.iverksattDato.Value)
                    {
                        tiltak.avsluttetDato = tiltak.iverksattDato;
                    }
                    if (tiltak.bortfaltDato.HasValue && tiltak.avsluttetDato.HasValue)
                    {
                        if (tiltakFamilia.TilIverksattdato.HasValue)
                        {
                            tiltak.bortfaltDato = null;
                        }
                        else
                        {
                            tiltak.avsluttetDato = null;
                        }
                    }
                    if (tiltak.bortfaltDato.HasValue)
                    {
                        tiltak.bortfaltKommentar = "Familia: Bortfalt";
                    }
                    if (!tiltak.bortfaltDato.HasValue && tiltak.avsluttetDato.HasValue && Mappings.IsSSBFosterhjemInstitusjon(tiltakFamilia.TttTiltakstype))
                    {
                        if (tiltakFamilia.TilHovedgrunnavsluttet == "1")
                        {
                            tiltak.avsluttetKode = "BARNET_TILBAKEFØRT_§_4-21";
                        }
                        else
                        {
                            if (tiltakFamilia.TilHovedgrunnavsluttet == "2")
                            {
                                tiltak.avsluttetKode = "BARNET_HAR_FYLT_18_ÅR";
                            }
                            else
                            {
                                if (tiltakFamilia.TilHovedgrunnavsluttet == "3")
                                {
                                    tiltak.avsluttetKode = "ADOPSJON_§_4-20";
                                }
                                else
                                {
                                    if (tiltakFamilia.TilHovedgrunnavsluttet == "4" || string.IsNullOrEmpty(tiltakFamilia.TilHovedgrunnavsluttet))
                                    {
                                        tiltak.avsluttetKode = "ANNET_(SPESIFISER)";
                                        tiltak.avsluttetSpesifisering = tiltakFamilia.TilPresisering;
                                        if (string.IsNullOrEmpty(tiltak.avsluttetSpesifisering))
                                        {
                                            tiltak.avsluttetSpesifisering = "Familia: Uspesifisert";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Mappings.IsSSBFosterhjem(tiltakFamilia.TttTiltakstype))
                    {
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            List<FaKlientadresser> adresser = await context.FaKlientadressers
                                .Where(a => a.KliLoepenr == tiltakFamilia.KliLoepenr && a.PahPassivisertdato.HasValue && a.PahPassivisertdato.Value.Year == DateTime.Now.Year && a.PahFraAarsak == "3.1").ToListAsync();

                            foreach (FaKlientadresser adresse in adresser)
                            {
                                Aktivitet flyttingMedFosterhjemAktivitet = new()
                                {
                                    aktivitetId = AddBydel(adresse.PahLoepenr.ToString(), "FMF"),
                                    sakId = tiltak.sakId,
                                    aktivitetsType = "FOSTERHJEM",
                                    aktivitetsUnderType = "FLYTTING_MED_FOSTERHJEM",
                                    status = "UTFØRT",
                                    tittel = "Flytting med fosterhjem",
                                    hendelsesdato = adresse.PahPassivisertdato,
                                    utfortDato = adresse.PahPassivisertdato,
                                    saksbehandlerId = GetBrukerId(adresse.SbhEndretav),
                                    utfortAvId = GetBrukerId(adresse.SbhEndretav)
                                };
                                flyttingMedFosterhjemAktiviteter.Add(flyttingMedFosterhjemAktivitet);
                            }
                        }
                    }
                    if (Mappings.IsSSBFosterhjemInstitusjon(tiltakFamilia.TttTiltakstype) && tiltakFamilia.TilAvsluttetdato.HasValue && tiltakFamilia.TilAvsluttetdato.Value.Year > 2022)
                    {
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            List<FaKlientadresser> adresser = await context.FaKlientadressers
                                .Where(a => a.KliLoepenr == tiltakFamilia.KliLoepenr && a.PahPassivisertdato.HasValue && a.PahPassivisertdato.Value.Year == DateTime.Now.Year && a.PahFraAarsak != "3.1").ToListAsync();

                            FaKlientadresser adresse = adresser.OrderBy(o => Math.Abs((o.PahPassivisertdato.Value - tiltakFamilia.TilAvsluttetdato.Value).TotalDays)).FirstOrDefault();
                            if (adresse != null)
                            {
                                string aarsak = adresse.PahFraAarsak;
                                if (aarsak != null && !string.IsNullOrEmpty(aarsak.Trim()))
                                {
                                    tiltak.arsakFlyttingFraFosterhjemInstitusjon = mappings.GetÅrsaksFylttingFra(aarsak.Trim());
                                }
                                aarsak = adresse.PahTilAarsak;
                                if (aarsak != null && !string.IsNullOrEmpty(aarsak.Trim()))
                                {
                                    tiltak.flyttingTil = mappings.GetÅrsaksFylttingTil(aarsak.Trim());
                                }
                                if (adresse.PahFraSpesifiser != null && !string.IsNullOrEmpty(adresse.PahFraSpesifiser.Trim()))
                                {
                                    tiltak.arsakFlyttingFraPresisering = adresse.PahFraSpesifiser.Trim();
                                }
                                if (adresse.PahTilSpesifiser != null && !string.IsNullOrEmpty(adresse.PahTilSpesifiser.Trim()))
                                {
                                    tiltak.presiseringAvBosted = adresse.PahTilSpesifiser.Trim();
                                }
                                if (string.IsNullOrEmpty(tiltak.arsakFlyttingFraFosterhjemInstitusjon) || string.IsNullOrEmpty(tiltak.flyttingTil))
                                {
                                    if (Mappings.IsSSBFosterhjem(tiltakFamilia.TttTiltakstype))
                                    {
                                        tiltak.arsakFlyttingFraFosterhjemInstitusjon = "1.2.3_ANDRE_GRUNNER_(F.EKS._UENIGHET_OM_OPPDRAGETS_OMFANG_ØKONOMI_FORSTERKNINGSTILTAK_MANGLENDE_ELLER_LITE_EFFEKTIV_VEILEDNING_MV.)_(KREVER_PRESISERING)";
                                    }
                                    else
                                    {
                                        tiltak.arsakFlyttingFraFosterhjemInstitusjon = "2.9_ANDRE_GRUNNER_(KREVER_PRESISERING)";
                                    };
                                    tiltak.arsakFlyttingFraPresisering = "Familia: Mangler presisering";
                                    tiltak.flyttingTil = "8_ANNET_BOSTED_(SPESIFISER)";
                                    tiltak.presiseringAvBosted = "Familia: Mangler presisering";
                                    tiltak.avsluttetKode = "ANNET_(SPESIFISER)";
                                    tiltak.avsluttetSpesifisering = "Familia: Uspesifisert";
                                }
                            }
                            else
                            {
                                if (Mappings.IsSSBFosterhjem(tiltakFamilia.TttTiltakstype))
                                {
                                    tiltak.arsakFlyttingFraFosterhjemInstitusjon = "1.2.3_ANDRE_GRUNNER_(F.EKS._UENIGHET_OM_OPPDRAGETS_OMFANG_ØKONOMI_FORSTERKNINGSTILTAK_MANGLENDE_ELLER_LITE_EFFEKTIV_VEILEDNING_MV.)_(KREVER_PRESISERING)";
                                }
                                else
                                {
                                    tiltak.arsakFlyttingFraFosterhjemInstitusjon = "2.9_ANDRE_GRUNNER_(KREVER_PRESISERING)";
                                };
                                tiltak.flyttingTil = "8_ANNET_BOSTED_(SPESIFISER)";
                                tiltak.arsakFlyttingFraPresisering = "Familia: Mangler presisering";
                                tiltak.presiseringAvBosted = "Familia: Mangler presisering";
                                tiltak.avsluttetKode = "ANNET_(SPESIFISER)";
                                tiltak.avsluttetSpesifisering = "Familia: Uspesifisert";
                            }
                            if (tiltak.arsakFlyttingFraFosterhjemInstitusjon == "2.9_ANDRE_GRUNNER_(KREVER_PRESISERING)" && string.IsNullOrEmpty(tiltak.arsakFlyttingFraPresisering))
                            {
                                tiltak.arsakFlyttingFraPresisering = "Familia: Mangler presisering";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovHovedParagraf))
                    {
                        tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovHovedParagraf, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf1))
                    {
                        if (!string.IsNullOrEmpty(tiltak.lovhjemmel))
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                        else
                        {
                            tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf2))
                    {
                        if (!string.IsNullOrEmpty(tiltak.jfLovhjemmelNr1))
                        {
                            tiltak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                        else
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                    }
                    if (tiltak.jfLovhjemmelNr2 == tiltak.lovhjemmel)
                    {
                        tiltak.jfLovhjemmelNr2 = null;
                    }
                    if (tiltak.jfLovhjemmelNr1 == tiltak.lovhjemmel)
                    {
                        tiltak.jfLovhjemmelNr1 = tiltak.jfLovhjemmelNr2;
                        tiltak.jfLovhjemmelNr2 = null;
                    }
                    if (tiltak.jfLovhjemmelNr1 == tiltak.jfLovhjemmelNr2)
                    {
                        tiltak.jfLovhjemmelNr2 = null;
                    }
                    string tiltakstype = tiltakFamilia.TttTiltakstype?.Trim();
                    if (tiltakstype == "14" || tiltakstype == "15" || tiltakstype == "16" || tiltakstype == "17" || tiltakstype == "18" || tiltakstype == "103" || tiltakstype == "104" || tiltakstype == "105" || tiltakstype == "106" || tiltakstype == "107" || tiltakstype == "108" || tiltakstype == "109")
                    {
                        if (tiltakFamilia.TilIverksattdato.HasValue)
                        {
                            tiltak.tilsynskommunenummer = await GetTilsynsKommunenummerAsync(tiltakFamilia.KliLoepenr, tiltakFamilia.TilIverksattdato);
                        }
                    }
                    FaEngasjementsavtale engasjementsavtale = null;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        engasjementsavtale = await context.FaEngasjementsavtales.Include(m => m.ForLoepenrNavigation.ForLoepenrNavigation).Where(e => string.IsNullOrEmpty(e.ForLoepenrNavigation.ForLoepenrNavigation.ForGmlreferanse) && e.TilLoepenr == tiltakFamilia.TilLoepenr && e.DokLoepenr.HasValue && e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                              && (e.EngTildato >= FirstInYearOfMigration)).OrderByDescending(o => o.EngTildato).FirstOrDefaultAsync();
                    }
                    if (engasjementsavtale != null)
                    {
                        Aktivitet aktivitet = new()
                        {
                            aktivitetId = AddBydel(engasjementsavtale.EngLoepenr.ToString(), "ODA"),
                            sakId = $"{GetActorId(engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation, false)}__OPP",
                            aktivitetsType = "OPPDRAGSTAKER_AVTALE",
                            aktivitetsUnderType = "ANNET",
                            hendelsesdato = engasjementsavtale.EngFradato,
                            status = "UTFØRT",
                            tittel = "Oppdragstakeravtale",
                            notat = "Se dokument",
                            utfortDato = engasjementsavtale.EngAvgjortdato,
                            utlopsdato = engasjementsavtale.EngTildato,
                            tiltaksId = tiltak.tiltakId
                        };
                        if (!string.IsNullOrEmpty(engasjementsavtale.SbhInitialer))
                        {
                            aktivitet.saksbehandlerId = GetBrukerId(engasjementsavtale.SbhInitialer);
                            aktivitet.utfortAvId = GetBrukerId(engasjementsavtale.SbhInitialer);
                        }
                        oppdragstakeravtaleAktiviteter.Add(aktivitet);
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = engasjementsavtale.DokLoepenr.Value,
                            dokumentNr = engasjementsavtale.EngDokumentnr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.saksbehandlerId
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                    }
                    if (tiltak.planlagtFraDato.HasValue && tiltak.planlagtFraDato.Value.Year < 1998)
                    {
                        tiltak.planlagtFraDato = null;
                    }
                    if (tiltak.iverksattDato.HasValue && tiltak.iverksattDato.Value.Year < 1998)
                    {
                        if (tiltakFamilia.TilRegistrertdato.HasValue)
                        {
                            tiltak.iverksattDato = tiltakFamilia.TilRegistrertdato.Value.Date;
                        }
                    }
                    if (tiltak.avsluttetDato.HasValue && tiltak.avsluttetDato.Value.Year < 1998)
                    {
                        if (tiltakFamilia.TilRegistrertdato.HasValue)
                        {
                            tiltak.avsluttetDato = tiltakFamilia.TilRegistrertdato.Value.Date;
                        }
                    }
                    tiltaksliste.Add(tiltak);
                    migrertAntall += 1;
                }
                await GetDocumentsAsync(worker, "Oppdragsavtaleaktiviteter", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> oppdragstakeravtaleAktiviteterDistinct = oppdragstakeravtaleAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int antallEntiteter = oppdragstakeravtaleAktiviteterDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> oppdragstakeravtaleAktiviteterPart = oppdragstakeravtaleAktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(oppdragstakeravtaleAktiviteterPart, GetJsonFileName("aktiviteter", $"Oppdragsavtaleaktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                await WriteFileAsync(flyttingMedFosterhjemAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()), GetJsonFileName("aktiviteter", "FlyttingMedFosterhjemAktiviteter"));

                toSkip = 0;
                fileNumber = 1;
                List<Tiltak> tiltakslisteDistinct = tiltaksliste.GroupBy(c => c.tiltakId).Select(s => s.First()).ToList();
                antallEntiteter = tiltakslisteDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Tiltak> tiltakslistePart = tiltakslisteDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(tiltakslistePart, GetJsonFileName("tiltak", $"Tiltak{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall tiltak: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Planer
        public async Task<string> GetPlanerAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk planer...");
                List<FaTiltaksplan> rawData;
                List<Aktivitet> aktiviteter = new();
                List<DocumentToInclude> documentsIncluded = new();
                int totalAntall = 0;
                int migrertAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaTiltaksplans.Include(t => t.FaTiltaksplanevalueringers).Include(x => x.KliLoepenrNavigation).Include(y => y.PtyPlankodeNavigation)
                        .Where(KlientPlanFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .Where(m => m.TtpSlettet == 0 && m.PtyPlankode != "8" && m.TtpFerdigdato.HasValue)
                        .ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Plan> planer = new();
                int antall = 0;
                foreach (var planFamilia in rawData)
                {
                    antall += 1;

                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk planer ({antall} av {totalAntall})...");
                    }
                    if (!mappings.IsOwner(planFamilia.KliLoepenr))
                    {
                        continue;
                    }
                    Plan plan = new()
                    {
                        planId = AddBydel(planFamilia.TtpLoepenr.ToString(), "PLA"),
                        sakId = GetSakId(planFamilia.KliLoepenr.ToString()),
                        situasjonsbeskrivelse = "Se dokument",
                        gyldigFraDato = planFamilia.TtpFradato,
                        gyldigTilDato = planFamilia.TtpTildato,
                        stoppetDato = planFamilia.TtpAvsluttdato,
                        hovedmaletBarnevernstjenestensTiltak = planFamilia.TtpHovedmaal,
                        varighetOgTilbakeforing = "Se dokument",
                        plasseringsted = "Se dokument",
                        intensjonForKontaktMedFamilie = "Se dokument",
                        barnetsBehovOverTid = "Se dokument",
                        bostedOgVarighet = "Se dokument",
                        skolegangDagtilbud = "Se dokument",
                        økonomi = "Se dokument",
                        tjenesterHjelpeapparatet = "Se dokument",
                        planForFlytting = "Se dokument",
                        nettverk = "Se dokument",
                        tidsperspektiv = "Se dokument"
                    };
                    if (planFamilia.PtyPlankodeNavigation != null)
                    {
                        plan.lovhjemmel = mappings.GetModulusLovhjemmelEtterPlankode(planFamilia.PtyPlankodeNavigation.PtyPlankode, planFamilia.TtpFradato);
                    }
                    if (!plan.gyldigTilDato.HasValue && plan.gyldigFraDato.HasValue)
                    {
                        plan.gyldigTilDato = plan.gyldigFraDato.Value.AddYears(1);
                    }
                    if (plan.gyldigTilDato.HasValue && plan.gyldigFraDato.HasValue && plan.gyldigTilDato < plan.gyldigFraDato)
                    {
                        plan.gyldigTilDato = plan.gyldigFraDato;
                    }
                    if (planFamilia.TtpAvsluttdato.HasValue)
                    {
                        plan.avsluttetDato = planFamilia.TtpAvsluttdato;
                        if  (plan.avsluttetDato < plan.gyldigFraDato)
                        {
                            plan.gyldigFraDato = plan.avsluttetDato;
                        }
                    }
                    else
                    {
                        if (planFamilia.TtpTildato < DateTime.Now)
                        {
                            plan.avsluttetDato = planFamilia.TtpTildato;
                        }
                    }
                    if (!string.IsNullOrEmpty(planFamilia.PtyPlankode))
                    {
                        switch (planFamilia.PtyPlankode?.Trim())
                        {
                            case "1":
                                plan.planType = "PLAN_FOR_TILTAK_-_HELPETILTAK";
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                plan.lovhjemmel = "Bvl._§_4-5_(gammel_lov)";
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "TILTAKSPLAN_ETTER_BVL_2021_8_1_TIDLIGERE_BVL_1992_4_5";
                                    plan.lovhjemmel = "Bvl._§_8-1";
                                }
                                break;
                            case "2":
                            case "3":
                                plan.planType = "PLAN_FOR_TILTAK_-_ADFERD";
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.varighetOgTilbakeforing = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.tidsperspektiv = null;
                                plan.plasseringsted = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.skolegangDagtilbud = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "TILTAKSPLAN_ETTER_BVL_2021_8_4_TIDLIGERE_BVL_1992_4_28";
                                }
                                break;
                            case "4":
                                plan.planType = "PLAN_FOR_TILTAK_-_FORELØPIG_OMSORGSPLAN";
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "OMSORGSPLAN_ETTER_BVL_2021_8_3_4_LEDD_TIDLIGERE_BVL_1992_4_15_3_LEDD";
                                }
                                break;
                            case "5":
                                plan.planType = "PLAN_FOR_TILTAK_-_OMSORGSPLAN";
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                plan.lovhjemmel = "Bvl._§_4-15_(gammel_lov)";
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "OMSORGSPLAN_ETTER_BVL_2021_8_3_4_LEDD_TIDLIGERE_BVL_1992_4_15_3_LEDD";
                                    plan.lovhjemmel = "Bvl._§_8-3._4.ledd";
                                }
                                break;
                            case "6":
                                plan.planType = "PLAN_FOR_FREMTIDIG_TILTAK_-_ETTERVERN";
                                plan.barnetsBehovOverTid = null;
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "TILTAKSPLAN_ETTERVERN_ETTER_BVL_2021_8_5_2_LEDD";
                                }
                                break;
                            case "7":
                                plan.planType = "HANDLINGSPLAN";
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                break;
                            case "9":
                                plan.planType = "SAMVÆRSPLAN_ETTER_BVL_2021_7_6";
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                break;
                        }
                    }
                    else
                    {
                        plan.planType = "PLAN_FOR_TILTAK_-_HELPETILTAK";
                        plan.varighetOgTilbakeforing = null;
                        plan.plasseringsted = null;
                        plan.intensjonForKontaktMedFamilie = null;
                        plan.barnetsBehovOverTid = null;
                        plan.bostedOgVarighet = null;
                        plan.skolegangDagtilbud = null;
                        plan.tjenesterHjelpeapparatet = null;
                        plan.planForFlytting = null;
                        plan.nettverk = null;
                        plan.tidsperspektiv = null;
                        plan.lovhjemmel = "Bvl._§_4-5_(gammel_lov)";
                        if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                        {
                            plan.planType = "TILTAKSPLAN_ETTER_BVL_2021_8_1_TIDLIGERE_BVL_1992_4_5";
                            plan.lovhjemmel = "Bvl._§_8-1";
                        }
                    }
                    if (!plan.avsluttetDato.HasValue && !(plan.gyldigTilDato.HasValue && DateTime.Now > plan.gyldigTilDato))
                    {
                        FaTiltaksplanevalueringer evaluering = planFamilia.FaTiltaksplanevalueringers.Where(t => !t.EvaUtfoertdato.HasValue && t.EvaPlanlagtdato > DateTime.Now).OrderBy(t => t.EvaPlanlagtdato).FirstOrDefault();
                        if (evaluering != null)
                        {
                            plan.nesteEvalueringDato = evaluering.EvaPlanlagtdato;
                        }
                    }
                    if (plan.gyldigTilDato.HasValue && plan.nesteEvalueringDato.HasValue && plan.gyldigTilDato.Value < plan.nesteEvalueringDato.Value)
                    {
                        plan.nesteEvalueringDato = null;
                    }
                    plan.planStatus = "UNDER_ARBEID";
                    if (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato < planFamilia.TtpTildato)
                    {
                        plan.planStatus = "STOPPET";
                        plan.gyldigTilDato = planFamilia.TtpAvsluttdato;
                    }
                    else
                    {
                        if ((planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato == planFamilia.TtpTildato) || (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpAvsluttdato < DateTime.Now))
                        {
                            plan.planStatus = "AVSLUTTET";
                        }
                        else
                        {
                            if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato <= DateTime.Now && planFamilia.TtpTildato.HasValue && planFamilia.TtpTildato >= DateTime.Now)
                            {
                                plan.planStatus = "AKTIV";
                            }
                            else
                            {
                                if (planFamilia.TtpFerdigdato.HasValue)
                                {
                                    plan.planStatus = "FERDIGSTILT";
                                }
                            }
                        }
                    }
                    if (plan.planStatus == "UNDER_ARBEID")
                    {
                        continue;
                    }
                    if (planFamilia.DokLoepenr.HasValue && planFamilia.TtpFerdigdato.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == planFamilia.DokLoepenr.Value).FirstOrDefaultAsync();
                        }
                        if (dokument != null)
                        {
                            Aktivitet aktivitet = new()
                            {
                                aktivitetId = AddBydel(planFamilia.TtpLoepenr.ToString(), "PLADOK"),
                                sakId = plan.sakId,
                                aktivitetsType = "PLANDOKUMENT",
                                aktivitetsUnderType = "PLANDOKUMENT",
                                hendelsesdato = planFamilia.TtpFerdigdato,
                                status = "UTFØRT",
                                tittel = "Plan",
                                notat = "Se dokument",
                                utfortDato = planFamilia.TtpFerdigdato,
                                utfortAvId = GetBrukerId(planFamilia.SbhInitialer)
                            };
                            if (string.IsNullOrEmpty(planFamilia.SbhInitialer))
                            {
                                if (!string.IsNullOrEmpty(planFamilia.KliLoepenrNavigation.SbhInitialer))
                                {
                                    aktivitet.utfortAvId = GetBrukerId(planFamilia.KliLoepenrNavigation.SbhInitialer);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(planFamilia.KliLoepenrNavigation.SbhInitialer2))
                                    {
                                        aktivitet.utfortAvId = GetBrukerId(planFamilia.KliLoepenrNavigation.SbhInitialer2);
                                    }
                                    else
                                    {
                                        aktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                    }
                                }
                            }
                            aktiviteter.Add(aktivitet);

                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = planFamilia.TtpDokumentnr,
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato
                            };
                            documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    if (planFamilia.FaTiltaksplanevalueringers?.Count > 0)
                    {
                        foreach (var tiltaksEvaluering in planFamilia.FaTiltaksplanevalueringers)
                        {
                            PlanEvaluering planEvaluering = new()
                            {
                                planlagtEvalueringsDato = tiltaksEvaluering.EvaPlanlagtdato,
                                utfortEvalueringsDato = tiltaksEvaluering.EvaUtfoertdato,
                                barnetsSynspunkt = "Se dokument",
                                foreldresSynspunkt = "Se dokument",
                                evaluering = "Se dokument"
                            };
                            if (!tiltaksEvaluering.EvaUtfoertdato.HasValue)
                            {
                                continue;
                            }
                            if (tiltaksEvaluering.EvaUtfoertdato.HasValue)
                            {
                                planEvaluering.status = "UTFØRT";
                            }
                            else
                            {
                                planEvaluering.status = "PLANLAGT";
                            }
                            plan.evalueringListe.Add(planEvaluering);
                            if (tiltaksEvaluering.DokLoepenr.HasValue && tiltaksEvaluering.EvaFerdigdato.HasValue)
                            {
                                FaDokumenter dokument;
                                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                                {
                                    dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == tiltaksEvaluering.DokLoepenr.Value).FirstOrDefaultAsync();
                                }
                                if (dokument != null)
                                {
                                    Aktivitet aktivitet = new()
                                    {
                                        aktivitetId = AddBydel(tiltaksEvaluering.EvaLoepenr.ToString(), "EVADOK"),
                                        sakId = plan.sakId,
                                        aktivitetsType = "EVALUERING",
                                        aktivitetsUnderType = "TILTAKSPLAN",
                                        hendelsesdato = tiltaksEvaluering.EvaUtfoertdato,
                                        status = "UTFØRT",
                                        tittel = "Evaluering",
                                        notat = "Se dokument",
                                        utfortDato = tiltaksEvaluering.EvaFerdigdato,
                                        utfortAvId = GetBrukerId(tiltaksEvaluering.SbhInitialer)
                                    };
                                    if (string.IsNullOrEmpty(aktivitet.utfortAvId))
                                    {
                                        aktivitet.utfortAvId = GetBrukerId(planFamilia.SbhInitialer);
                                        if (string.IsNullOrEmpty(aktivitet.utfortAvId))
                                        {
                                            aktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                                        }
                                    }
                                    if (aktivitet.hendelsesdato == null)
                                    {
                                        aktivitet.hendelsesdato = aktivitet.utfortDato;
                                    }
                                    aktiviteter.Add(aktivitet);

                                    DocumentToInclude documentToInclude = new()
                                    {
                                        dokLoepenr = dokument.DokLoepenr,
                                        dokumentNr = tiltaksEvaluering.EvaDokumentnr,
                                        sakId = aktivitet.sakId,
                                        tittel = aktivitet.tittel,
                                        journalDato = aktivitet.utfortDato
                                    };
                                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                                    documentsIncluded.Add(documentToInclude);
                                }
                            }
                        }
                    }
                    if (!plan.gyldigFraDato.HasValue)
                    {
                        plan.gyldigFraDato = planFamilia.TtpFerdigdato;
                    }
                    planer.Add(plan);
                    migrertAntall += 1;
                }
                await GetDocumentsAsync(worker, "Plandokumenter", documentsIncluded, false);
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int antallEntiteter = aktiviteterDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Plandokumentaktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                List<Plan> planerDistinct = planer.GroupBy(c => c.planId).Select(s => s.First()).ToList();
                antallEntiteter = planerDistinct.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Plan> planerPart = planerDistinct.OrderBy(o => o.planId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(planerPart, GetJsonFileName("plan", $"Planer{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall planer: {migrertAntall}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        #endregion

        #region Aktiviteter
        public async Task<string> GetAktiviteterAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk aktiviteter...");
                string statusText = $"Antall aktiviteter tilsynsrapporter: {await GetTilsynsrapporterAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter interne saksforberedelser: {await GetInterneSaksforberedelserAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter journaler: {await GetJournalAktiviteterAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter slettede journaler: {await GetSlettedeJournalAktiviteterAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter individuelle planer: {await GetIndividuellPlanAktiviteterAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter postjournaler: {await GetPostjournalAktiviteterAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter slettede postjournaler: {await GetSlettedePostjournalAktiviteterAsync(worker)}" + Environment.NewLine;
                statusText += $"Antall aktiviteter oppfølginger: {await GetOppfølgingAktiviteterAsync(worker)}" + Environment.NewLine;
                return statusText;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<int> GetTilsynsrapporterAsync(BackgroundWorker worker)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientPostJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosBrevtype == "TB" && p.PosFerdigdato.HasValue)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var postjournal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk tilsynsrapporter ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value) && postjournal.KliLoepenrNavigation.KliFraannenkommune == 0)
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT"),
                    sakId = GetSakId(postjournal.KliLoepenr.ToString()),
                    aktivitetsType = "TILSYN",
                    aktivitetsUnderType = "TILSYNSBESØK",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato,
                    lovPaalagt = true
                };
                if (aktivitet.hendelsesdato.HasValue)
                {
                    aktivitet.tilsynAnsvarligKommunenummer = await GetTilsynsKommunenummerAsync(postjournal.KliLoepenr.Value, aktivitet.hendelsesdato, false);
                }
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                }
                if (postjournal.KliLoepenrNavigation.KliFraannenkommune == 1)
                {
                    aktivitet.aktivitetsUnderType = "TILSYNSBESØK_I_TILSYNSKOMMUNE";
                    aktivitet.tilsynAnsvarligKommunenummer = null;
                }
                else
                {
                    if (postjournal.PosDato.Year >= FirstInYearOfMigration.Year)
                    {
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            FaTiltak tiltak = await context.FaTiltaks
                                .Where(m => m.KliLoepenr == postjournal.KliLoepenr
                                    && (
                                    m.TttTiltakstype == "14" ||
                                    m.TttTiltakstype == "15" ||
                                    m.TttTiltakstype == "16" ||
                                    m.TttTiltakstype == "17" ||
                                    m.TttTiltakstype == "18" ||
                                    m.TttTiltakstype == "103" ||
                                    m.TttTiltakstype == "104" ||
                                    m.TttTiltakstype == "105" ||
                                    m.TttTiltakstype == "106" ||
                                    m.TttTiltakstype == "107" ||
                                    m.TttTiltakstype == "108" ||
                                    m.TttTiltakstype == "109") &&
                                    (m.TilIverksattdato <= postjournal.PosDato))
                                .OrderByDescending(b => b.TilIverksattdato)
                                .FirstOrDefaultAsync();
                            if (tiltak != null)
                            {
                                aktivitet.tiltaksId = AddBydel(tiltak.TilLoepenr.ToString(), "TIL");
                            }
                        }
                    }
                }
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    if (postjournal.PosUnndrattinnsyn == 1)
                    {
                        documentToInclude.merknadInnsyn = "Undratt innsyn";
                    }
                    else if (postjournal.PosVurderUnndratt == 1)
                    {
                        documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                    }
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, "Tilsynsrapporter", documentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Tilsynsrapporter{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetPostjournalAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientPostJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "KK" || p.PosBrevtype == "AS" || p.PosBrevtype == "AN" || p.PosBrevtype == "RF" || p.PosBrevtype == "RA" || p.PosBrevtype == "BR" || p.PosBrevtype == "TU" || p.PosBrevtype == "RS" || p.PosBrevtype == "RK" || p.PosBrevtype == "RM" || p.PosBrevtype == "RV" || p.PosBrevtype == "X" || p.PosBrevtype == "TM"))
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var postjournal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk postjournalaktiviteter ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value) && postjournal.KliLoepenrNavigation.KliFraannenkommune == 0)
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT"),
                    sakId = GetSakId(postjournal.KliLoepenr.ToString()),
                    aktivitetsType = "ØVRIG_DOKUMENT",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato
                };
                if (postjournal.PosBrevtype == "TM")
                {
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        int antallMeldinger = await context.FaMeldingers
                            .Where(p => p.PosSendtkonklAar == postjournal.PosAar && p.PosSendtkonklLoepenr == postjournal.PosLoepenr)
                            .CountAsync();
                        if (antallMeldinger > 0)
                        {
                            continue;
                        }
                    }
                }
                if (postjournal.PosDokumentnr.HasValue)
                {
                    string formattedDokumentNr = postjournal.PosDokumentnr.Value.ToString();
                    if (formattedDokumentNr.Length < 5)
                    {
                        formattedDokumentNr = formattedDokumentNr.PadLeft(4, '0');
                    }
                    aktivitet.aktivitetId = $"{formattedDokumentNr}__" + aktivitet.aktivitetId;
                }
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                }
                if (!string.IsNullOrEmpty(postjournal.PosPosttype))
                {
                    switch (postjournal.PosPosttype?.Trim())
                    {
                        case "I":
                            aktivitet.aktivitetsUnderType = "INN";
                            aktivitet.notat = GetPostJounalMotpart(true, aktivitet.notat, postjournal.PosFornavn, postjournal.PosEtternavn);
                            break;
                        case "U":
                            aktivitet.aktivitetsUnderType = "UT";
                            aktivitet.notat = GetPostJounalMotpart(false, aktivitet.notat, postjournal.PosFornavn, postjournal.PosEtternavn);
                            break;
                        case "X":
                            aktivitet.aktivitetsUnderType = "NOTAT";
                            break;
                        default:
                            break;
                    }
                }
                if (aktivitet.utfortDato.HasValue && aktivitet.utfortDato.Value.Year < 1998)
                {
                    aktivitet.utfortDato = postjournal.PosRegistrertdato;
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year < 1998)
                {
                    aktivitet.hendelsesdato = postjournal.PosRegistrertdato;
                }
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    if (postjournal.PosUnndrattinnsyn == 1)
                    {
                        documentToInclude.merknadInnsyn = "Undratt innsyn";
                    }
                    else if (postjournal.PosVurderUnndratt == 1)
                    {
                        documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                    }
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, "Postjournalaktiviteter", documentsIncluded);

            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Postjournalaktiviteter{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetSlettedePostjournalAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaPostjournal> rawData;

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientPostJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(p => p.PosFerdigdato != null && p.PosSlettet == 1 && p.KliLoepenrNavigation.KliFraannenkommune == 0)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var postjournal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk slettede postjournalaktiviteter ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value))
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT"),
                    sakId = GetSakId(postjournal.KliLoepenr.ToString()),
                    aktivitetsType = "SLETTET",
                    aktivitetsUnderType = "SLETTET",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    utfortDato = postjournal.PosFerdigdato
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                }
                if (!string.IsNullOrEmpty(postjournal.PosBegrSlettet))
                {
                    aktivitet.notat = $"Årsak: {postjournal.PosBegrSlettet}";
                };
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
            }
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"SlettedePostjournalaktiviteter{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetOppfølgingAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientPostJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "OB") && p.KliLoepenrNavigation.KliFraannenkommune == 0)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var postjournal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk oppfølgingsaktiviteter ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value))
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT"),
                    sakId = GetSakId(postjournal.KliLoepenr.ToString()),
                    aktivitetsType = "OPPFØLGING",
                    aktivitetsUnderType = "OPPFØLGINGSBESØK",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato,
                    lovPaalagt = true
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                }
                if (postjournal.PosDato.Year >= FirstInYearOfMigration.Year)
                {
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaTiltak tiltak = await context.FaTiltaks
                            .Where(m => m.KliLoepenr == postjournal.KliLoepenr
                                && (
                                m.TttTiltakstype == "14" ||
                                m.TttTiltakstype == "15" ||
                                m.TttTiltakstype == "16" ||
                                m.TttTiltakstype == "17" ||
                                m.TttTiltakstype == "18" ||
                                m.TttTiltakstype == "103" ||
                                m.TttTiltakstype == "104" ||
                                m.TttTiltakstype == "105" ||
                                m.TttTiltakstype == "106" ||
                                m.TttTiltakstype == "107" ||
                                m.TttTiltakstype == "108" ||
                                m.TttTiltakstype == "109") &&
                                (m.TilIverksattdato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilIverksattdato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddBydel(tiltak.TilLoepenr.ToString(), "TIL");
                        }
                        else if (postjournal.KliLoepenrNavigation.KliFraannenkommune != 1)
                        {
                            aktivitet.aktivitetsType = "ØVRIG_DOKUMENT";
                            aktivitet.aktivitetsUnderType = "NOTAT";
                        }
                    }
                }
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    if (postjournal.PosUnndrattinnsyn == 1)
                    {
                        documentToInclude.merknadInnsyn = "Undratt innsyn";
                    }
                    else if (postjournal.PosVurderUnndratt == 1)
                    {
                        documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                    }
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, "Oppfølgingsaktiviteter", documentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Oppfølgingsaktiviteter{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetInterneSaksforberedelserAsync(BackgroundWorker worker)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientPostJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 1 && p.PosFerdigdato.HasValue)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var postjournal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk interne saksforberedelser ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value) && postjournal.KliLoepenrNavigation.KliFraannenkommune == 0)
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT"),
                    sakId = GetSakId(postjournal.KliLoepenr.ToString()),
                    aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)",
                    aktivitetsUnderType = "INGEN",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer);
                }
                if (postjournal.KliLoepenrNavigation.KliFraannenkommune == 1)
                {
                    aktivitet.aktivitetsType = "ØVRIG_DOKUMENT";
                    aktivitet.aktivitetsUnderType = "NOTAT";
                }

                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, "InterneSaksforberedelser", documentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"InterneSaksforberedelser{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetJournalAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaJournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();
            List<TextDocumentToInclude> textDocumentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaJournals.Include(x => x.KliLoepenrNavigation).Include(x => x.JotIdentNavigation)
                    .Where(KlientJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(m => m.JouSlettet != 1 && m.JouFerdigdato != null && m.KliLoepenr.HasValue)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var journal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk journaler ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(journal.KliLoepenr.Value) && journal.KliLoepenrNavigation.KliFraannenkommune == 0)
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(journal.JouLoepenr.ToString(), "JOU"),
                    sakId = GetSakId(journal.KliLoepenr.ToString()),
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = GetBrukerId(journal.SbhInitialer),
                    tittel = journal.JouEmne,
                    utfortAvId = GetBrukerId(journal.SbhInitialer),
                    utfortDato = journal.JouFerdigdato,
                    notat = journal.JouNotat
                };
                if (journal.JouUnndrattinnsynIs == 1)
                {
                    aktivitet.aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)";
                    aktivitet.aktivitetsUnderType = "INGEN";
                }
                else
                {
                    if (journal.JotIdent == "HJ" || journal.JotIdent == "KS" || journal.JotIdent == "NO" || journal.JotIdent == "NF" || journal.JotIdent == "OT" || journal.JotIdent == "SB" || journal.JotIdent == "TO" || journal.JotIdent == "ET")
                    {
                        aktivitet.aktivitetsType = "SAMLENOTAT";
                        switch (journal.JotIdent)
                        {
                            case "HJ":
                                aktivitet.aktivitetsUnderType = "HJEMMEBESØK";
                                break;
                            case "KS":
                            case "ET":
                            case "OT":
                                aktivitet.aktivitetsUnderType = "ANNET";
                                break;
                            case "NO":
                            case "NF":
                                aktivitet.aktivitetsUnderType = "NOTAT";
                                break;
                            case "SB":
                                aktivitet.aktivitetsUnderType = "SAMTALE_MED_BARNET";
                                break;
                            case "TO":
                                aktivitet.aktivitetsUnderType = "TELEFONSAMTALE";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (journal.JotIdent == "IN")
                        {
                            aktivitet.aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)";
                            aktivitet.aktivitetsUnderType = "INGEN";
                        }
                        else if (journal.JotIdent == "OP")
                        {
                            aktivitet.aktivitetsType = "OPPFØLGING";
                            aktivitet.aktivitetsUnderType = "OPPFØLGINGSBESØK";
                        }
                        else if (journal.JotIdent == "TB")
                        {
                            aktivitet.aktivitetsType = "TILSYN";
                            aktivitet.aktivitetsUnderType = "TILSYNSBESØK";
                            aktivitet.lovPaalagt = true;
                        }
                    }
                }
                if (string.IsNullOrEmpty(aktivitet.utfortAvId))
                {
                    aktivitet.utfortAvId = GetBrukerId(journal.KliLoepenrNavigation.SbhInitialer);
                }
                string merknadInnsyn = "";
                if (aktivitet.aktivitetsType == "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)" || journal.JouUnndrattinnsyn == 1)
                {
                    merknadInnsyn = $"Jmf.vedtak: {journal.SakUnndrAar}{journal.SakUnndrJournalnr}";
                }
                else if (journal.JouVurderUnndratt == 1)
                {
                    merknadInnsyn = "Vurder unndratt innsyn";
                }
                if (journal.KliLoepenrNavigation.KliFraannenkommune == 1 && aktivitet.aktivitetsType != "SAMLENOTAT")
                {
                    aktivitet.aktivitetsType = "ØVRIG_DOKUMENT";
                    aktivitet.aktivitetsUnderType = "NOTAT";
                }
                if (journal.DokLoepenr.HasValue)
                {
                    FaDokumenter dokument;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == journal.DokLoepenr.Value).FirstOrDefaultAsync();
                    }
                    if (dokument != null)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = dokument.DokLoepenr,
                            dokumentNr = journal.JouDokumentnr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            merknadInnsyn = merknadInnsyn
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                        aktivitet.notat = null;
                    }
                    else
                    {
                        if (aktivitet.aktivitetsType == "OPPFØLGING" && !string.IsNullOrEmpty(journal.JouNotat))
                        {
                            TextDocumentToInclude textDocumentToInclude = new()
                            {
                                dokLoepenr = journal.JouLoepenr,
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato,
                                opprettetAvId = aktivitet.utfortAvId,
                                innhold = journal.JouNotat,
                                beskrivelse = journal.JotIdentNavigation?.JotBeskrivelse,
                                datonotat = journal.JouDatonotat,
                                foedselsdato = journal.KliLoepenrNavigation?.KliFoedselsdato,
                                forNavn = journal.KliLoepenrNavigation?.KliFornavn,
                                etterNavn = journal.KliLoepenrNavigation?.KliEtternavn,
                                merknadInnsyn = merknadInnsyn
                            };
                            textDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                            textDocumentsIncluded.Add(textDocumentToInclude);
                            aktivitet.notat = null;
                        }
                    }
                }
                else
                {
                    if (aktivitet.aktivitetsType == "OPPFØLGING" && !string.IsNullOrEmpty(journal.JouNotat))
                    {
                        TextDocumentToInclude textDocumentToInclude = new()
                        {
                            dokLoepenr = journal.JouLoepenr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            innhold = journal.JouNotat,
                            beskrivelse = journal.JotIdentNavigation?.JotBeskrivelse,
                            datonotat = journal.JouDatonotat,
                            foedselsdato = journal.KliLoepenrNavigation?.KliFoedselsdato,
                            forNavn = journal.KliLoepenrNavigation?.KliFornavn,
                            etterNavn = journal.KliLoepenrNavigation?.KliEtternavn,
                            merknadInnsyn = merknadInnsyn
                        };
                        textDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        textDocumentsIncluded.Add(textDocumentToInclude);
                        aktivitet.notat = null;
                    }
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year < 1998)
                {
                    aktivitet.hendelsesdato = journal.JouRegistrertdato;
                }
                if (aktivitet.utfortDato.HasValue && aktivitet.utfortDato.Value.Year < 1998)
                {
                    aktivitet.utfortDato = journal.JouRegistrertdato;
                }
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
            }
            await GetDocumentsAsync(worker, "Journaler", documentsIncluded);
            await GetTextDocumentsAsync(worker, textDocumentsIncluded);

            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Journaler{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetSlettedeJournalAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaJournal> rawData;

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaJournals.Include(x => x.KliLoepenrNavigation).Include(x => x.JotIdentNavigation)
                    .Where(KlientJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(m => m.JouFerdigdato != null && m.JouSlettet == 1 && m.KliLoepenr.HasValue)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var journal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk slettede journaler ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(journal.KliLoepenr.Value) && journal.KliLoepenrNavigation.KliFraannenkommune == 0)
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(journal.JouLoepenr.ToString(), "JOU"),
                    sakId = GetSakId(journal.KliLoepenr.ToString()),
                    aktivitetsType = "SLETTET",
                    aktivitetsUnderType = "SLETTET",
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = GetBrukerId(journal.SbhInitialer),
                    tittel = journal.JouEmne,
                    utfortAvId = GetBrukerId(journal.SbhInitialer),
                    utfortDato = journal.JouFerdigdato,
                    notat = journal.JouNotat
                };
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
            }
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"SlettedeJournaler{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        private async Task<int> GetIndividuellPlanAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaTiltaksplan> rawData;
            List<DocumentToInclude> documentsIncluded = new();
            List<TextDocumentToInclude> textDocumentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaTiltaksplans.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientPlanFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Where(m => m.TtpSlettet == 0 && m.PtyPlankode == "8" && m.TtpFerdigdato.HasValue)
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var plan in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk individuelle planer ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(plan.KliLoepenr))
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(plan.TtpLoepenr.ToString(), "IVP"),
                    sakId = GetSakId(plan.KliLoepenr.ToString()),
                    status = "UTFØRT",
                    aktivitetsType = "ØVRIG_DOKUMENT",
                    aktivitetsUnderType = "INDIVIDUELL_PLAN",
                    tittel = "Individuell plan",
                    hendelsesdato = plan.TtpRegistrertdato,
                    saksbehandlerId = GetBrukerId(plan.SbhInitialer),
                    utfortAvId = GetBrukerId(plan.SbhRegistrertav),
                    utfortDato = plan.TtpFerdigdato,
                    notat = "Se dokument"
                };
                if (plan.TtpFradato.HasValue)
                {
                    aktivitet.tittel += $" fra {plan.TtpFradato.Value:dd.MM.yyyy}";
                };
                if (plan.TtpTildato.HasValue)
                {
                    aktivitet.tittel += $" til {plan.TtpTildato.Value:dd.MM.yyyy}";
                };
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (plan.DokLoepenr.HasValue)
                {
                    FaDokumenter dokument;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == plan.DokLoepenr.Value).FirstOrDefaultAsync();
                    }

                    if (dokument != null)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = dokument.DokLoepenr,
                            dokumentNr = plan.TtpDokumentnr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                    }
                }
            }
            await GetDocumentsAsync(worker, "IndividuellPlan", documentsIncluded);
            await GetTextDocumentsAsync(worker, textDocumentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"IndividuellPlan{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            return migrertAntall;
        }
        #endregion

        #region Tidligere bydeler
        private async Task GetDataTidligereBydelerAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak)
        {
            try
            {
                List<string> avdelinger = sak.tidligereAvdelingListe.Select(r => r.avdelingId[..3]).Distinct().ToList();
                foreach (var tidligereBydel in avdelinger)
                {
                    if (tidligereBydel != Bydelsforkortelse)
                    {
                        await GetMeldingerTidligereBydelAsync(worker, fodselsDato, personNummer, sak, tidligereBydel);
                        await GetPlanerTidligereBydelAsync(worker, fodselsDato, personNummer, sak, tidligereBydel);
                        await GetUndersokelserTidligereBydelAsync(worker, fodselsDato, personNummer, sak, tidligereBydel);
                        await GetVedtakTidligereBydelAsync(worker, fodselsDato, personNummer, sak.sakId, tidligereBydel);
                        await GetTiltakTidligereBydelAsync(worker, fodselsDato, personNummer, sak, tidligereBydel);
                        await GetAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sak, tidligereBydel);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task GetMeldingerTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            try
            {
                List<FaMeldinger> rawData;

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaMeldingers.Include(x => x.KliLoepenrNavigation)
                        .Where(m => m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue && m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                        .ToListAsync();
                }

                List<Melding> meldinger = new();
                List<DocumentToInclude> documentsIncluded = new();

                foreach (var meldingFamilia in rawData)
                {
                    Melding melding = new()
                    {
                        sakId = sak.sakId,
                        meldingId = AddSpecificBydel(meldingFamilia.MelLoepenr.ToString(), "MEL", bydel),
                        mottattBekymringsmelding = GetMottattBekymringsmelding(meldingFamilia, bydel),
                        behandlingAvBekymringsmelding = GetBehandlingAvBekymringsmelding(meldingFamilia, bydel),
                        tilbakemeldingTilMelder = await GetTilbakemeldingTilMelderAsync(meldingFamilia, bydel)
                    };
                    if (meldingFamilia.PosMottattbrevAar.HasValue && meldingFamilia.PosMottattbrevLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = null;
                        FaDokumenter dokument = null;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            postJournal = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && meldingFamilia.PosMottattbrevAar.HasValue && p.PosAar == meldingFamilia.PosMottattbrevAar.Value && meldingFamilia.PosMottattbrevLoepenr.HasValue && p.PosLoepenr == meldingFamilia.PosMottattbrevLoepenr.Value && p.PosFerdigdato.HasValue).FirstOrDefaultAsync();
                            if (postJournal != null)
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && postJournal.DokLoepenr.HasValue && d.DokLoepenr == postJournal.DokLoepenr.Value).FirstOrDefaultAsync();
                            }
                        }
                        if (dokument != null)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = meldingFamilia.MelDokumentnr,
                                sakId = melding.sakId,
                                tittel = "Meldingsdokument",
                                journalDato = melding.mottattBekymringsmelding.mottattDato,
                                opprettetAvId = melding.mottattBekymringsmelding.utfortAvId
                            };
                            if (postJournal.PosUnndrattinnsyn == 1)
                            {
                                documentToInclude.merknadInnsyn = "Undratt innsyn";
                            }
                            else if (postJournal.PosVurderUnndratt == 1)
                            {
                                documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                            }
                            documentToInclude.aktivitetIdListe.Add(melding.meldingId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    if ((meldingFamilia.PosGjennomdokAar.HasValue && meldingFamilia.PosGjennomdokLoepenr.HasValue) || meldingFamilia.DokLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = null;
                        FaDokumenter dokument = null;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            postJournal = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && meldingFamilia.PosGjennomdokAar.HasValue && p.PosAar == meldingFamilia.PosGjennomdokAar.Value && meldingFamilia.PosGjennomdokLoepenr.HasValue && p.PosLoepenr == meldingFamilia.PosGjennomdokLoepenr.Value && p.PosFerdigdato.HasValue).FirstOrDefaultAsync();
                            if (postJournal != null)
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && postJournal.DokLoepenr.HasValue && d.DokLoepenr == postJournal.DokLoepenr.Value).FirstOrDefaultAsync();
                            }
                            else
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && meldingFamilia.DokLoepenr.HasValue && d.DokLoepenr == meldingFamilia.DokLoepenr).FirstOrDefaultAsync();
                            }
                        }
                        if (dokument != null)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                sakId = melding.sakId,
                                tittel = "Meldingsgjennomgang",
                                journalDato = melding.behandlingAvBekymringsmelding.konklusjonsdato,
                                opprettetAvId = melding.behandlingAvBekymringsmelding.utfortAvId
                            };
                            if (postJournal != null)
                            {
                                documentToInclude.dokumentNr = postJournal.PosDokumentnr;
                            }
                            documentToInclude.aktivitetIdListe.Add(melding.behandlingAvBekymringsmelding.behandlingId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    if (meldingFamilia.PosSendtkonklAar.HasValue && meldingFamilia.PosSendtkonklLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = null;
                        FaDokumenter dokument = null;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            postJournal = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && meldingFamilia.PosSendtkonklAar.HasValue && p.PosAar == meldingFamilia.PosSendtkonklAar.Value && meldingFamilia.PosSendtkonklLoepenr.HasValue && p.PosLoepenr == meldingFamilia.PosSendtkonklLoepenr.Value && p.PosFerdigdato.HasValue).FirstOrDefaultAsync();
                            if (postJournal != null)
                            {
                                dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && postJournal.DokLoepenr.HasValue && d.DokLoepenr == postJournal.DokLoepenr.Value).FirstOrDefaultAsync();
                            }
                        }
                        if (dokument != null)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = postJournal.PosDokumentnr,
                                sakId = melding.sakId,
                                tittel = "Tilbakemelding til melder",
                                journalDato = melding.tilbakemeldingTilMelder.utfortDato,
                                opprettetAvId = melding.tilbakemeldingTilMelder.utfortAvId
                            };
                            if (postJournal.PosUnndrattinnsyn == 1)
                            {
                                documentToInclude.merknadInnsyn = "Undratt innsyn";
                            }
                            else if (postJournal.PosVurderUnndratt == 1)
                            {
                                documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                            }
                            documentToInclude.aktivitetIdListe.Add(melding.tilbakemeldingTilMelder.tilbakemeldingId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    meldinger.Add(melding);
                }
                await GetDocumentsAsync(worker, $"Meldinger{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, false, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                List<Melding> meldingerDistinct = meldinger.GroupBy(c => c.meldingId).Select(s => s.First()).ToList();
                int migrertAntall = meldingerDistinct.Count;
                foreach (Melding melding in meldingerDistinct)
                {
                    if (melding.mottattBekymringsmelding.mottattDato < sak.startDato)
                    {
                        sak.startDato = melding.mottattBekymringsmelding.mottattDato;
                    }
                }
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Melding> meldingerPart = meldingerDistinct.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(meldingerPart, GetJsonFileName("melding", $"Meldinger{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task GetPlanerTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            try
            {
                List<FaTiltaksplan> rawData;
                List<Aktivitet> aktiviteter = new();
                List<DocumentToInclude> documentsIncluded = new();

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaTiltaksplans.Include(t => t.FaTiltaksplanevalueringers).Include(x => x.KliLoepenrNavigation).Include(y => y.PtyPlankodeNavigation)
                        .Where(m => m.TtpSlettet == 0 && m.PtyPlankode != "8" && m.TtpFerdigdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer))
                        .ToListAsync();
                }
                List<Plan> planer = new();
                foreach (var planFamilia in rawData)
                {
                    Plan plan = new()
                    {
                        planId = AddSpecificBydel(planFamilia.TtpLoepenr.ToString(), "PLA", bydel),
                        sakId = sak.sakId,
                        situasjonsbeskrivelse = "Se dokument",
                        gyldigFraDato = planFamilia.TtpFradato,
                        gyldigTilDato = planFamilia.TtpTildato,
                        stoppetDato = planFamilia.TtpAvsluttdato,
                        hovedmaletBarnevernstjenestensTiltak = planFamilia.TtpHovedmaal,
                        varighetOgTilbakeforing = "Se dokument",
                        plasseringsted = "Se dokument",
                        intensjonForKontaktMedFamilie = "Se dokument",
                        barnetsBehovOverTid = "Se dokument",
                        bostedOgVarighet = "Se dokument",
                        skolegangDagtilbud = "Se dokument",
                        økonomi = "Se dokument",
                        tjenesterHjelpeapparatet = "Se dokument",
                        planForFlytting = "Se dokument",
                        nettverk = "Se dokument",
                        tidsperspektiv = "Se dokument"
                    };
                    if (planFamilia.PtyPlankodeNavigation != null)
                    {
                        plan.lovhjemmel = mappings.GetModulusLovhjemmelEtterPlankode(planFamilia.PtyPlankodeNavigation.PtyPlankode, planFamilia.TtpFradato);
                    }
                    if (!plan.gyldigTilDato.HasValue && plan.gyldigFraDato.HasValue)
                    {
                        plan.gyldigTilDato = plan.gyldigFraDato.Value.AddYears(1);
                    }
                    if (plan.gyldigTilDato.HasValue && plan.gyldigFraDato.HasValue && plan.gyldigTilDato < plan.gyldigFraDato)
                    {
                        plan.gyldigTilDato = plan.gyldigFraDato;
                    }
                    if (planFamilia.TtpAvsluttdato.HasValue)
                    {
                        plan.avsluttetDato = planFamilia.TtpAvsluttdato;
                        if (plan.avsluttetDato < plan.gyldigFraDato)
                        {
                            plan.gyldigFraDato = plan.avsluttetDato;
                        }
                    }
                    else
                    {
                        if (planFamilia.TtpTildato < DateTime.Now)
                        {
                            plan.avsluttetDato = planFamilia.TtpTildato;
                        }
                    }
                    if (!string.IsNullOrEmpty(planFamilia.PtyPlankode))
                    {
                        switch (planFamilia.PtyPlankode?.Trim())
                        {
                            case "1":
                                plan.planType = "PLAN_FOR_TILTAK_-_HELPETILTAK";
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                plan.lovhjemmel = "Bvl._§_4-5_(gammel_lov)";
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "TILTAKSPLAN_ETTER_BVL_2021_8_1_TIDLIGERE_BVL_1992_4_5";
                                    plan.lovhjemmel = "Bvl._§_8-1";
                                }
                                break;
                            case "2":
                            case "3":
                                plan.planType = "PLAN_FOR_TILTAK_-_ADFERD";
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.varighetOgTilbakeforing = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.tidsperspektiv = null;
                                plan.plasseringsted = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.skolegangDagtilbud = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "TILTAKSPLAN_ETTER_BVL_2021_8_4_TIDLIGERE_BVL_1992_4_28";
                                }
                                break;
                            case "4":
                                plan.planType = "PLAN_FOR_TILTAK_-_FORELØPIG_OMSORGSPLAN";
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "OMSORGSPLAN_ETTER_BVL_2021_8_3_4_LEDD_TIDLIGERE_BVL_1992_4_15_3_LEDD";
                                }
                                break;
                            case "5":
                                plan.planType = "PLAN_FOR_TILTAK_-_OMSORGSPLAN";
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                plan.lovhjemmel = "Bvl._§_4-15_(gammel_lov)";
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "OMSORGSPLAN_ETTER_BVL_2021_8_3_4_LEDD_TIDLIGERE_BVL_1992_4_15_3_LEDD";
                                    plan.lovhjemmel = "Bvl._§_8-3._4.ledd";
                                }
                                break;
                            case "6":
                                plan.planType = "PLAN_FOR_FREMTIDIG_TILTAK_-_ETTERVERN";
                                plan.barnetsBehovOverTid = null;
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                                {
                                    plan.planType = "TILTAKSPLAN_ETTERVERN_ETTER_BVL_2021_8_5_2_LEDD";
                                }
                                break;
                            case "7":
                                plan.planType = "HANDLINGSPLAN";
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                break;
                            case "9":
                                plan.planType = "SAMVÆRSPLAN_ETTER_BVL_2021_7_6";
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
                                plan.barnetsBehovOverTid = null;
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
                                break;
                        }
                    }
                    else
                    {
                        plan.planType = "PLAN_FOR_TILTAK_-_HELPETILTAK";
                        plan.varighetOgTilbakeforing = null;
                        plan.plasseringsted = null;
                        plan.intensjonForKontaktMedFamilie = null;
                        plan.barnetsBehovOverTid = null;
                        plan.bostedOgVarighet = null;
                        plan.skolegangDagtilbud = null;
                        plan.tjenesterHjelpeapparatet = null;
                        plan.planForFlytting = null;
                        plan.nettverk = null;
                        plan.tidsperspektiv = null;
                        plan.lovhjemmel = "Bvl._§_4-5_(gammel_lov)";
                        if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato.Value >= FirstInYearOfMigration)
                        {
                            plan.planType = "TILTAKSPLAN_ETTER_BVL_2021_8_1_TIDLIGERE_BVL_1992_4_5";
                            plan.lovhjemmel = "Bvl._§_8-1";
                        }
                    }
                    if (!plan.avsluttetDato.HasValue && !(plan.gyldigTilDato.HasValue && DateTime.Now > plan.gyldigTilDato))
                    {
                        FaTiltaksplanevalueringer evaluering = planFamilia.FaTiltaksplanevalueringers.Where(t => !t.EvaUtfoertdato.HasValue && t.EvaPlanlagtdato > DateTime.Now).OrderBy(t => t.EvaPlanlagtdato).FirstOrDefault();
                        if (evaluering != null)
                        {
                            plan.nesteEvalueringDato = evaluering.EvaPlanlagtdato;
                        }
                    }
                    if (plan.gyldigTilDato.HasValue && plan.nesteEvalueringDato.HasValue && plan.gyldigTilDato.Value < plan.nesteEvalueringDato.Value)
                    {
                        plan.nesteEvalueringDato = null;
                    }
                    plan.planStatus = "UNDER_ARBEID";
                    if (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato < planFamilia.TtpTildato)
                    {
                        plan.planStatus = "STOPPET";
                        plan.gyldigTilDato = planFamilia.TtpAvsluttdato;
                    }
                    else
                    {
                        if ((planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato == planFamilia.TtpTildato) || (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpAvsluttdato < DateTime.Now))
                        {
                            plan.planStatus = "AVSLUTTET";
                        }
                        else
                        {
                            if (planFamilia.TtpFradato.HasValue && planFamilia.TtpFradato <= DateTime.Now && planFamilia.TtpTildato.HasValue && planFamilia.TtpTildato >= DateTime.Now)
                            {
                                plan.planStatus = "AKTIV";
                            }
                            else
                            {
                                if (planFamilia.TtpFerdigdato.HasValue)
                                {
                                    plan.planStatus = "FERDIGSTILT";
                                }
                            }
                        }
                    }
                    if (plan.planStatus == "UNDER_ARBEID")
                    {
                        continue;
                    }
                    if (planFamilia.DokLoepenr.HasValue && planFamilia.TtpFerdigdato.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == planFamilia.DokLoepenr.Value).FirstOrDefaultAsync();
                        }
                        if (dokument != null)
                        {
                            Aktivitet aktivitet = new()
                            {
                                aktivitetId = AddSpecificBydel(planFamilia.TtpLoepenr.ToString(), "PLADOK", bydel),
                                sakId = plan.sakId,
                                aktivitetsType = "PLANDOKUMENT",
                                aktivitetsUnderType = "PLANDOKUMENT",
                                hendelsesdato = planFamilia.TtpFerdigdato,
                                status = "UTFØRT",
                                tittel = "Plan",
                                notat = "Se dokument",
                                utfortDato = planFamilia.TtpFerdigdato,
                                utfortAvId = GetBrukerId(planFamilia.SbhInitialer, bydel)
                            };
                            if (string.IsNullOrEmpty(planFamilia.SbhInitialer))
                            {
                                if (!string.IsNullOrEmpty(planFamilia.KliLoepenrNavigation.SbhInitialer))
                                {
                                    aktivitet.utfortAvId = GetBrukerId(planFamilia.KliLoepenrNavigation.SbhInitialer, bydel);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(planFamilia.KliLoepenrNavigation.SbhInitialer2))
                                    {
                                        aktivitet.utfortAvId = GetBrukerId(planFamilia.KliLoepenrNavigation.SbhInitialer2, bydel);
                                    }
                                    else
                                    {
                                        aktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel), bydel);
                                    }
                                }
                            }
                            if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                            {
                                sak.startDato = aktivitet.hendelsesdato;
                            }
                            aktiviteter.Add(aktivitet);

                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = planFamilia.TtpDokumentnr,
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato
                            };
                            documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                    if (planFamilia.FaTiltaksplanevalueringers?.Count > 0)
                    {
                        foreach (var tiltaksEvaluering in planFamilia.FaTiltaksplanevalueringers)
                        {
                            if (!tiltaksEvaluering.EvaUtfoertdato.HasValue)
                            {
                                continue;
                            }
                            PlanEvaluering planEvaluering = new()
                            {
                                planlagtEvalueringsDato = tiltaksEvaluering.EvaPlanlagtdato,
                                utfortEvalueringsDato = tiltaksEvaluering.EvaUtfoertdato,
                                barnetsSynspunkt = "Se dokument",
                                foreldresSynspunkt = "Se dokument",
                                evaluering = "Se dokument"
                            };
                            if (tiltaksEvaluering.EvaUtfoertdato.HasValue)
                            {
                                planEvaluering.status = "UTFØRT";
                            }
                            else
                            {
                                planEvaluering.status = "PLANLAGT";
                            }
                            plan.evalueringListe.Add(planEvaluering);
                            if (tiltaksEvaluering.DokLoepenr.HasValue && tiltaksEvaluering.EvaFerdigdato.HasValue)
                            {
                                FaDokumenter dokument;
                                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                                {
                                    dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == tiltaksEvaluering.DokLoepenr.Value).FirstOrDefaultAsync();
                                }
                                if (dokument != null)
                                {
                                    Aktivitet aktivitet = new()
                                    {
                                        aktivitetId = AddSpecificBydel(tiltaksEvaluering.EvaLoepenr.ToString(), "EVADOK", bydel),
                                        sakId = plan.sakId,
                                        aktivitetsType = "EVALUERING",
                                        aktivitetsUnderType = "TILTAKSPLAN",
                                        hendelsesdato = tiltaksEvaluering.EvaUtfoertdato,
                                        status = "UTFØRT",
                                        tittel = "Evaluering",
                                        notat = "Se dokument",
                                        utfortDato = tiltaksEvaluering.EvaFerdigdato,
                                        utfortAvId = GetBrukerId(tiltaksEvaluering.SbhInitialer, bydel)
                                    };
                                    if (string.IsNullOrEmpty(aktivitet.utfortAvId))
                                    {
                                        aktivitet.utfortAvId = GetBrukerId(planFamilia.SbhInitialer, bydel);
                                        if (string.IsNullOrEmpty(aktivitet.utfortAvId))
                                        {
                                            aktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel), bydel);
                                        }
                                    }
                                    if (aktivitet.hendelsesdato == null)
                                    {
                                        aktivitet.hendelsesdato = aktivitet.utfortDato;
                                    }
                                    if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                                    {
                                        sak.startDato = aktivitet.hendelsesdato;
                                    }
                                    aktiviteter.Add(aktivitet);

                                    DocumentToInclude documentToInclude = new()
                                    {
                                        dokLoepenr = dokument.DokLoepenr,
                                        dokumentNr = tiltaksEvaluering.EvaDokumentnr,
                                        sakId = aktivitet.sakId,
                                        tittel = aktivitet.tittel,
                                        journalDato = aktivitet.utfortDato
                                    };
                                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                                    documentsIncluded.Add(documentToInclude);
                                }
                            }
                        }
                    }
                    if (!plan.gyldigFraDato.HasValue)
                    {
                        plan.gyldigFraDato = planFamilia.TtpFerdigdato;
                    }
                    planer.Add(plan);
                }
                await GetDocumentsAsync(worker, $"Plandokumenter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, false, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int migrertAntall = aktiviteterDistinct.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Plandokumentaktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = planer.Count;
                guid = Guid.NewGuid().ToString().Replace('-', '_');
                foreach (Plan plan in planer)
                {
                    if (plan.gyldigFraDato.HasValue && plan.gyldigFraDato < sak.startDato)
                    {
                        sak.startDato = plan.gyldigFraDato;
                    }
                }
                while (migrertAntall > toSkip)
                {
                    List<Plan> planerPart = planer.OrderBy(o => o.planId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(planerPart, GetJsonFileName("plan", $"Planer{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task GetUndersokelserTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            try
            {
                List<FaUndersoekelser> rawData;

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaUndersoekelsers.Include(x => x.MelLoepenrNavigation).Include(x => x.MelLoepenrNavigation.KliLoepenrNavigation)
                        .Where(m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.MelLoepenrNavigation.KliLoepenrNavigation.KliPersonnr == personNummer)
                        .ToListAsync();
                }

                List<Undersøkelse> undersøkelser = new();
                List<Aktivitet> undersøkelsesAktiviteter = new();
                List<DocumentToInclude> documentsIncluded = new();
                List<TextDocumentToInclude> textDocumentsIncluded = new();

                foreach (var undersøkelse in rawData)
                {
                    Undersøkelse undersoekelse = new()
                    {
                        undersokelseId = AddSpecificBydel(undersøkelse.MelLoepenr.ToString(), "UND", bydel),
                        meldingId = AddSpecificBydel(undersøkelse.MelLoepenr.ToString(), "MEL", bydel),
                        sakId = sak.sakId,
                        startDato = undersøkelse.UndStartdato
                    };
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        FaSaksjournal saksJournal = null;
                        if (undersøkelse.UndKonklusjon?.Trim() == "1")
                        {
                            saksJournal = await context.FaSaksjournals.Where(m => m.MelLoepenr == undersøkelse.MelLoepenr).FirstOrDefaultAsync();
                        }
                        else
                        {
                            saksJournal = await context.FaSaksjournals.Where(m => m.MelLoepenr == undersøkelse.MelLoepenr && m.SakStatus != "BEH").FirstOrDefaultAsync();
                        }
                        if (saksJournal != null)
                        {
                            undersoekelse.vedtakAktivitetId = AddSpecificBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED", bydel);
                        }
                    }
                    if (undersøkelse.UndRegistrertdato.HasValue)
                    {
                        undersoekelse.opprettetDato = undersøkelse.UndRegistrertdato.Value;
                    }
                    bool henlegges = false;
                    if (!string.IsNullOrEmpty(undersøkelse.UndKonklusjon))
                    {
                        if (undersøkelse.UndKonklusjon == "2" || undersøkelse.UndKonklusjon == "3" || undersøkelse.UndKonklusjon == "4" || undersøkelse.UndKonklusjon == "5" || undersøkelse.UndKonklusjon == "H")
                        {
                            undersoekelse.konklusjonsDato = undersøkelse.UndHenlagtdato;
                        }
                        else
                        {
                            if (undersøkelse.UndKonklusjon == "0" || undersøkelse.UndKonklusjon == "1" || undersøkelse.UndKonklusjon == "V")
                            {
                                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                                {
                                    List<FaSaksjournal> saksjournals = await context.FaSaksjournals.Where(s => s.MelLoepenr == undersøkelse.MelLoepenr).ToListAsync();
                                    if (saksjournals is not null && saksjournals.Count > 0)
                                    {
                                        if (undersøkelse.UndKonklusjon == "1")
                                        {
                                            undersoekelse.konklusjonsDato = saksjournals[0].SakSendtfylke;
                                        }
                                        else
                                        {
                                            undersoekelse.konklusjonsDato = saksjournals[0].SakAvgjortdato;
                                        }
                                    }
                                }
                            }
                        }
                        switch (undersøkelse.UndKonklusjon?.Trim())
                        {
                            case "0":
                            case "V":
                                undersoekelse.konklusjon = "BARNEVERNSTJENESTEN_GJØR_VEDTAK_OM_TILTAK";
                                break;
                            case "1":
                                undersoekelse.konklusjon = "BEGJÆRING_OM_TILTAK_FOR_BARNEVERNS_OG_HELSENEMNDA";
                                break;
                            case "2":
                            case "H":
                                undersoekelse.konklusjon = "UNDERSØKELSEN_HENLEGGES_ETTER_BARNEVERNETS_VURDERING";
                                henlegges = true;
                                break;
                            case "3":
                                undersoekelse.konklusjon = "UNDERSØKELSE_HENLEGGES_ETTER_PARTEN_SITT_ØNSKE";
                                henlegges = true;
                                break;
                            case "4":
                                undersoekelse.konklusjon = "UNDERSØKELSE_HENLEGGES_PÅ_GRUNN_AV_FLYTTING";
                                henlegges = true;
                                break;
                            case "5":
                                undersoekelse.konklusjon = "UNDERSØKELSEN_HENLAGT_ETTER_HENVISNING_TIL_ANNEN_INSTANS";
                                henlegges = true;
                                break;
                            default:
                                break;
                        }
                    }
                    if (undersoekelse.konklusjonsDato.HasValue)
                    {
                        undersoekelse.status = "KONKLUDERT_UNDERSØKELSE";
                    }
                    else
                    {
                        if (undersoekelse.startDato.HasValue)
                        {
                            undersoekelse.status = "IVERKSATT_UNDERSØKELSE";
                        }
                        else
                        {
                            undersoekelse.status = "VENTER_PÅ_UNDERSØKELSE";
                        }
                    }
                    if (undersøkelse.UndFristdato.HasValue)
                    {
                        undersoekelse.fristDato = undersøkelse.UndFristdato.Value;
                    }
                    else
                    {
                        if (undersøkelse.UndRegistrertdato.HasValue)
                        {
                            undersoekelse.fristDato = undersøkelse.UndRegistrertdato.Value.AddMonths(3);
                        }
                    }
                    if (undersøkelse.UndKonklusjon == "4")
                    {
                        if (undersøkelse.UndMeldtnykommune == 1)
                        {
                            undersoekelse.konklusjonPresisering = "Meldt ny kommune: Ja";
                        }
                        else
                        {
                            undersoekelse.konklusjonPresisering = "Meldt ny kommune: Nei";
                        }
                        if (!string.IsNullOrEmpty(undersøkelse.UndKonklusjontekst))
                        {
                            undersoekelse.konklusjonPresisering += " - " + undersøkelse.UndKonklusjontekst;
                        }
                    }
                    GetGrunnlagForTiltak(undersøkelse, undersoekelse, henlegges);
                    if (undersøkelse.UndBehandlingstid == "2")
                    {
                        Aktivitet undersøkelseUtvidelseFristAktivitet = new()
                        {
                            aktivitetId = AddSpecificBydel(undersøkelse.MelLoepenr.ToString(), "UNDUT", bydel),
                            sakId = undersoekelse.sakId,
                            aktivitetsType = "UTVIDELSE_AV_FRIST",
                            aktivitetsUnderType = "UNDERSØKELSE",
                            status = "UTFØRT",
                            saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer, bydel),
                            tittel = "Beslutning om utvidet undersøkelsestid",
                            utfortAvId = GetBrukerId(undersøkelse.SbhInitialer, bydel),
                            notat = undersøkelse.Und6mndbegrunnelse,
                            fristDato = undersøkelse.UndFristdato,
                            fristLovpaalagt = true
                        };
                        if (string.IsNullOrEmpty(undersøkelse.SbhInitialer))
                        {
                            undersøkelseUtvidelseFristAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer, bydel);
                            undersøkelseUtvidelseFristAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer, bydel);
                            if (string.IsNullOrEmpty(undersøkelseUtvidelseFristAktivitet.utfortAvId))
                            {
                                undersøkelseUtvidelseFristAktivitet.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel));
                                undersøkelseUtvidelseFristAktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel));
                            }
                        }
                        if (string.IsNullOrEmpty(undersøkelseUtvidelseFristAktivitet.notat))
                        {
                            undersøkelseUtvidelseFristAktivitet.notat = "Se dokument";
                        }
                        if (!undersøkelseUtvidelseFristAktivitet.fristDato.HasValue)
                        {
                            undersøkelseUtvidelseFristAktivitet.fristDato = undersoekelse.konklusjonsDato.Value.AddDays(180);
                        }
                        bool annetBrukt = false;
                        if (!string.IsNullOrEmpty(undersøkelse.FroKode1))
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add(mappings.GetÅrsakskodeUtvidelseFrist(undersøkelse.FroKode1));
                        }
                        if (!string.IsNullOrEmpty(undersøkelse.FroKode2))
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add(mappings.GetÅrsakskodeUtvidelseFrist(undersøkelse.FroKode2));
                        }
                        if (!string.IsNullOrEmpty(undersøkelse.FroKode3))
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add(mappings.GetÅrsakskodeUtvidelseFrist(undersøkelse.FroKode3));
                        }
                        if (undersøkelse.FroKode1 == "30" || undersøkelse.FroKode2 == "30" || undersøkelse.FroKode3 == "30")
                        {
                            annetBrukt = true;
                        }
                        if (undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Count == 0)
                        {
                            undersøkelseUtvidelseFristAktivitet.årsakskoderUtvidelseAvFristUndersokelse.Add("30_ANNET_(BRUK_GJERNE_STIKKORD)");
                            annetBrukt = true;
                        }
                        if (annetBrukt)
                        {
                            undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse = undersøkelse.Und6mndbegrunnelse;
                            if (string.IsNullOrEmpty(undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse))
                            {
                                undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse = "Familia: Mangler presisering";
                            }
                        }
                        if (undersøkelseUtvidelseFristAktivitet.fristDato.HasValue)
                        {
                            undersøkelseUtvidelseFristAktivitet.hendelsesdato = undersøkelseUtvidelseFristAktivitet.fristDato.Value.AddDays(-90);
                            undersøkelseUtvidelseFristAktivitet.utfortDato = undersøkelseUtvidelseFristAktivitet.fristDato.Value.AddDays(-90);
                        }
                        undersoekelse.aktivitetIdListe.Add(undersøkelseUtvidelseFristAktivitet.aktivitetId);
                        TextDocumentToInclude textDocumentToInclude = new()
                        {
                            dokLoepenr = undersøkelse.MelLoepenr,
                            sakId = undersøkelseUtvidelseFristAktivitet.sakId,
                            tittel = undersøkelseUtvidelseFristAktivitet.tittel,
                            journalDato = undersøkelseUtvidelseFristAktivitet.utfortDato,
                            opprettetAvId = undersøkelseUtvidelseFristAktivitet.utfortAvId,
                            innhold = undersøkelseUtvidelseFristAktivitet.notat,
                            beskrivelse = undersøkelseUtvidelseFristAktivitet.presiseringAvÅrsaksUtvidelseAvFristUndersokelse,
                            foedselsdato = undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation?.KliFoedselsdato,
                            forNavn = undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation?.KliFornavn,
                            etterNavn = undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation?.KliEtternavn
                        };
                        if (undersøkelseUtvidelseFristAktivitet.hendelsesdato.HasValue)
                        {
                            textDocumentToInclude.datonotat = undersøkelseUtvidelseFristAktivitet.hendelsesdato.Value;
                        }
                        textDocumentToInclude.aktivitetIdListe.Add(undersøkelseUtvidelseFristAktivitet.aktivitetId);
                        textDocumentsIncluded.Add(textDocumentToInclude);
                        undersøkelsesAktiviteter.Add(undersøkelseUtvidelseFristAktivitet);
                    }
                    else
                    {
                        undersoekelse.utvidetFrist = false;
                    }
                    if (undersøkelse.DokUplannr.HasValue && undersøkelse.UndFerdigdatoUplan.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == undersøkelse.DokUplannr.Value).FirstOrDefaultAsync();
                        }
                        if (dokument != null)
                        {
                            Aktivitet undersøkelsesplanAktivitet = new()
                            {
                                aktivitetId = AddSpecificBydel(undersøkelse.MelLoepenr.ToString(), "UNDPL", bydel),
                                sakId = undersoekelse.sakId,
                                aktivitetsType = "UNDERSØKELSESPLAN",
                                aktivitetsUnderType = "UNDERSØKELSESPLAN",
                                status = "UTFØRT",
                                saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer, bydel),
                                tittel = "Undersøkelsesplan",
                                utfortAvId = GetBrukerId(undersøkelse.SbhInitialer, bydel),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdatoUplan,
                                utfortDato = undersøkelse.UndFerdigdatoUplan
                            };
                            if (string.IsNullOrEmpty(undersøkelse.SbhInitialer))
                            {
                                undersøkelsesplanAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer, bydel);
                                undersøkelsesplanAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer, bydel);
                                if (string.IsNullOrEmpty(undersøkelsesplanAktivitet.utfortAvId))
                                {
                                    undersøkelsesplanAktivitet.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel));
                                    undersøkelsesplanAktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel));
                                }
                            }
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = undersøkelse.UndDokumentnruplan,
                                sakId = undersoekelse.sakId,
                                tittel = undersøkelsesplanAktivitet.tittel,
                                journalDato = undersøkelsesplanAktivitet.utfortDato,
                                opprettetAvId = undersøkelsesplanAktivitet.utfortAvId
                            };
                            documentToInclude.aktivitetIdListe.Add(undersøkelsesplanAktivitet.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                            undersøkelsesAktiviteter.Add(undersøkelsesplanAktivitet);
                            undersoekelse.aktivitetIdListe.Add(undersøkelsesplanAktivitet.aktivitetId);
                        }
                    }
                    if (undersøkelse.DokLoepenr.HasValue && undersøkelse.UndFerdigdato.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == undersøkelse.DokLoepenr.Value).FirstOrDefaultAsync();
                        }
                        if (dokument != null)
                        {
                            Aktivitet undersøkelsesrapportAktivitet = new()
                            {
                                aktivitetId = AddSpecificBydel(undersøkelse.MelLoepenr.ToString(), "UNDRA", bydel),
                                sakId = undersoekelse.sakId,
                                aktivitetsType = "UNDERSØKELSESRAPPORT",
                                aktivitetsUnderType = "UNDERSØKELSESRAPPORT",
                                status = "UTFØRT",
                                saksbehandlerId = GetBrukerId(undersøkelse.SbhInitialer, bydel),
                                tittel = "Sluttrapport undersøkelse",
                                utfortAvId = GetBrukerId(undersøkelse.SbhInitialer, bydel),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdato,
                                utfortDato = undersøkelse.UndFerdigdato
                            };
                            if (string.IsNullOrEmpty(undersøkelse.SbhInitialer))
                            {
                                if (!string.IsNullOrEmpty(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer))
                                {
                                    undersøkelsesrapportAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer, bydel);
                                    undersøkelsesrapportAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer, bydel);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer2))
                                    {
                                        undersøkelsesrapportAktivitet.saksbehandlerId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer2, bydel);
                                        undersøkelsesrapportAktivitet.utfortAvId = GetBrukerId(undersøkelse.MelLoepenrNavigation.KliLoepenrNavigation.SbhInitialer2, bydel);
                                    }
                                    else
                                    {
                                        undersøkelsesrapportAktivitet.saksbehandlerId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel), bydel);
                                        undersøkelsesrapportAktivitet.utfortAvId = GetBrukerId(mappings.GetHovedsaksbehandlerBydel(bydel), bydel);
                                    }
                                }
                            }
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
                                dokumentNr = undersøkelse.UndDokumentnr,
                                sakId = undersoekelse.sakId,
                                tittel = undersøkelsesrapportAktivitet.tittel,
                                journalDato = undersøkelsesrapportAktivitet.utfortDato,
                                opprettetAvId = undersøkelsesrapportAktivitet.utfortAvId
                            };
                            documentToInclude.aktivitetIdListe.Add(undersøkelsesrapportAktivitet.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                            undersøkelsesAktiviteter.Add(undersøkelsesrapportAktivitet);
                            undersoekelse.aktivitetIdListe.Add(undersøkelsesrapportAktivitet.aktivitetId);
                        }
                    }
                    undersøkelser.Add(undersoekelse);
                }
                await GetTextDocumentsAsync(worker, textDocumentsIncluded, bydel: bydel);
                await GetDocumentsAsync(worker, $"Undersøkelser{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> undersøkelsesAktiviteterDistinct = undersøkelsesAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int migrertAntall = undersøkelsesAktiviteterDistinct.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> undersøkelsesAktiviteterPart = undersøkelsesAktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(undersøkelsesAktiviteterPart, GetJsonFileName("aktiviteter", $"UndersokelsesAktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = undersøkelser.Count;
                foreach (Undersøkelse undersøkelse in undersøkelser)
                {
                    if (undersøkelse.startDato.HasValue && undersøkelse.startDato < sak.startDato)
                    {
                        sak.startDato = undersøkelse.startDato;
                    }
                }
                guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Undersøkelse> undersøkelserPart = undersøkelser.OrderBy(o => o.undersokelseId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(undersøkelserPart, GetJsonFileName("undersokelser", $"Undersokelser{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task GetVedtakTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, string sakId, string bydel)
        {
            try
            {
                List<FaSaksjournal> rawData;
                List<DocumentToInclude> documentsIncluded = new();

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaSaksjournals.Include(x => x.KliLoepenrNavigation)
                        .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer
                        && (m.SakStatus != "KLR" && m.SakStatus != "BEH")
                        && !((m.MynVedtakstype == "FN" || m.MynVedtakstype == "LA" || m.MynVedtakstype == "TI") && !(m.SakStatus == "GOD" || m.SakStatus == "AVS" || m.SakStatus == "BOR")))
                        .ToListAsync();
                }
                List<Vedtak> vedtaksliste = new();
                foreach (var saksJournal in rawData)
                {
                    Vedtak vedtak = new()
                    {
                        sakId = sakId,
                        aktivitetId = AddSpecificBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED", bydel),
                        tittel = saksJournal.SakEmne,
                        vedtaksdato = saksJournal.SakAvgjortdato,
                        startdato = saksJournal.SakIverksattdato,
                        barnetsMedvirkning = "Se dokument",
                        bakgrunnsopplysninger = "Se dokument",
                        vedtak = "Se dokument",
                        begrunnelse = "Se dokument",
                        godkjentStatusDato = saksJournal.SakAvgjortdato,
                        begjaeringOversendtNemndStatusDato = saksJournal.SakSendtfylke
                    };
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        FaTiltak tiltak = await context.FaTiltaks.Where(m => m.SakJournalnr == saksJournal.SakJournalnr && m.SakAar == saksJournal.SakAar && m.TilEttervern != null).FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            if (tiltak.TilEttervern == "1")
                            {
                                vedtak.arsakIkkeEttervern = "IKKE_VIDEREFØRT_ETTER_ØNSKE";
                            }
                            else if (tiltak.TilEttervern == "2")
                            {
                                vedtak.arsakIkkeEttervern = "IKKE_VIDEREFØRT_AVSLAG_KOMMUNE";
                            }
                            else if (tiltak.TilEttervern == "3")
                            {
                                vedtak.arsakIkkeEttervern = "AVSLÅTT_UNGDOM";
                            }
                        }
                    }
                    if (saksJournal.SakBehFylkesnemnda == "FE")
                    {
                        vedtak.behandlingIFylkesnemda = "FORENKLET";
                    }
                    else if (saksJournal.SakBehFylkesnemnda == "FT" || saksJournal.SakBehFylkesnemnda == "SP" || saksJournal.MynVedtakstype == "FN")
                    {
                        vedtak.behandlingIFylkesnemda = "FULLTALLIG";
                    }
                    string lovHovedParagraf = saksJournal.LovHovedParagraf?.Trim();
                    string lovJmfParagraf1 = saksJournal.LovJmfParagraf1?.Trim();
                    string lovJmfParagraf2 = saksJournal.LovJmfParagraf2?.Trim();
                    string mynVedtakstype = saksJournal.MynVedtakstype;

                    mynVedtakstype = GetVedtakstype(saksJournal, vedtak, lovHovedParagraf, lovJmfParagraf1, lovJmfParagraf2, mynVedtakstype);

                    if (!string.IsNullOrEmpty(saksJournal.SbhInitialer))
                    {
                        vedtak.saksbehandlerId = GetBrukerId(saksJournal.SbhInitialer, bydel);
                    }
                    if (!string.IsNullOrEmpty(saksJournal.SbhAvgjortavInitialer))
                    {
                        vedtak.godkjentAvSaksbehandlerId = GetBrukerId(saksJournal.SbhAvgjortavInitialer, bydel);
                    }
                    vedtak.aktivitetsUndertype = GetVedtakAktivitetsUnderType(mynVedtakstype, lovHovedParagraf, lovJmfParagraf1, lovJmfParagraf2, saksJournal.SakStatus);

                    if (vedtak.aktivitetsUndertype != "ADMINISTRATIV_BESLUTNING" && vedtak.aktivitetsUndertype == "AVSLUTNING_AV_BARNEVERNSSAK")
                    {
                        vedtak.avslutningsårsakPresisering = saksJournal.SakMerknaderAvslag;
                    }
                    if (saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR" || mynVedtakstype == "FN" || mynVedtakstype == "LA" || mynVedtakstype == "TI")
                    {
                        if (saksJournal.SakStatus == "AVS")
                        {
                            vedtak.beslutning = "KOMMUNEN_HAR_FÅTT_AVSLAG";
                        }
                        else
                        {
                            if (saksJournal.SakStatus == "GOD" || saksJournal.SakStatus == "BOR")
                            {
                                vedtak.beslutning = "KOMMUNEN_HAR_FÅTT_MEDHOLD";
                            }
                        }
                    }
                    if (saksJournal.SakAvgjortdato.HasValue || saksJournal.SakBortfaltdato.HasValue)
                    {
                        vedtak.status = "UTFØRT";

                    }
                    else
                    {
                        vedtak.status = "AKTIV";
                    }
                    if (mynVedtakstype == "BV")
                    {
                        vedtak.vedtakFraBarnevernsvakt = true;
                    }
                    else
                    {
                        vedtak.vedtakFraBarnevernsvakt = false;
                    }
                    if (saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR")
                    {
                        if (saksJournal.SakSlutningdato.HasValue)
                        {
                            vedtak.startdato = saksJournal.SakSlutningdato;
                        }
                    }
                    if (saksJournal.SakAvgjortetat == "FN")
                    {
                        vedtak.rettsinstans = "BARNEVERNSOGHELSENEMND";
                    }
                    else
                    {
                        if (saksJournal.SakAvgjortetat == "LR")
                        {
                            vedtak.rettsinstans = "LAGMANNSRETT";
                        }
                        else
                        {
                            if (saksJournal.SakAvgjortetat == "TR")
                            {
                                vedtak.rettsinstans = "TINGRETT";
                            }
                        }
                    }
                    if (vedtak.aktivitetsUndertype == "VEDTAK_FRA_RETTSINSTANSER" && string.IsNullOrEmpty(vedtak.rettsinstans))
                    {
                        vedtak.rettsinstans = "BARNEVERNSOGHELSENEMND";
                    }
                    if (saksJournal.SakStatus == "AVS")
                    {
                        vedtak.avsluttetStatusDato = saksJournal.SakAvgjortdato;
                        vedtak.godkjentStatusDato = null;
                    }
                    else if (saksJournal.SakStatus == "BOR" || saksJournal.SakStatus == "OHV")
                    {
                        vedtak.bortfaltStatusDato = saksJournal.SakBortfaltdato;
                        if (!vedtak.vedtaksdato.HasValue)
                        {
                            vedtak.vedtaksdato = vedtak.bortfaltStatusDato;
                        }
                        if (!vedtak.vedtaksdato.HasValue && !saksJournal.SakOpphevetdato.HasValue)
                        {
                            continue;
                        }
                    }
                    if (!vedtak.vedtaksdato.HasValue && vedtak.aktivitetsUndertype == "VEDTAK_FRA_RETTSINSTANSER" && (saksJournal.SakStatus == "BOR" || saksJournal.SakStatus == "OHV"))
                    {
                        vedtak.vedtaksdato = saksJournal.SakBortfaltdato;
                    }
                    if (vedtak.startdato.HasValue && vedtak.avsluttetStatusDato.HasValue && vedtak.startdato.Value > vedtak.avsluttetStatusDato.Value)
                    {
                        vedtak.startdato = vedtak.avsluttetStatusDato;
                    }
                    if (vedtak.startdato.HasValue && vedtak.startdato.Value.Year < 1998)
                    {
                        vedtak.startdato = saksJournal.SakAvgjortdato;
                    }
                    if (!vedtak.vedtaksdato.HasValue && vedtak.aktivitetsUndertype == "VEDTAK_FRA_RETTSINSTANSER")
                    {
                        vedtak.vedtaksdato = saksJournal.SakSlutningdato;
                    }
                    vedtaksliste.Add(vedtak);

                    if (saksJournal.PosAar.HasValue && saksJournal.PosLoepenr.HasValue)
                    {
                        FaPostjournal postJournal = await GetPostJournalSpecificBydelAsync(saksJournal.PosAar, saksJournal.PosLoepenr, bydel);
                        if (postJournal != null && postJournal.DokLoepenr.HasValue)
                        {
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = postJournal.DokLoepenr.Value,
                                dokumentNr = postJournal.PosDokumentnr,
                                sakId = vedtak.sakId,
                                tittel = vedtak.tittel,
                                journalDato = vedtak.vedtaksdato,
                                opprettetAvId = vedtak.saksbehandlerId
                            };
                            documentToInclude.aktivitetIdListe.Add(vedtak.aktivitetId);
                            documentsIncluded.Add(documentToInclude);
                        }
                    }
                }
                await GetDocumentsAsync(worker, $"Vedtak{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                List<Vedtak> vedtakslisteDistinct = vedtaksliste.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int migrertAntall = vedtakslisteDistinct.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Vedtak> vedtakslistePart = vedtakslisteDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(vedtakslistePart, GetJsonFileName("vedtak", $"Vedtak{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task GetTiltakTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            try
            {
                List<FaTiltak> rawData;

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaTiltaks.Include(x => x.KliLoepenrNavigation).Include(c => c.Sak)
                        .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer && m.Sak != null && m.Sak.SakStatus != "BEH")
                        .ToListAsync();
                }
                List<Tiltak> tiltaksliste = new();
                List<DocumentToInclude> documentsIncluded = new();
                List<Aktivitet> oppdragstakeravtaleAktiviteter = new();
                List<Aktivitet> flyttingMedFosterhjemAktiviteter = new();

                foreach (var tiltakFamilia in rawData)
                {
                    Tiltak tiltak = new()
                    {
                        tiltakId = AddSpecificBydel(tiltakFamilia.TilLoepenr.ToString(), "TIL", bydel),
                        sakId = sak.sakId,
                        ssbHovedkategori = mappings.GetSSBHovedkategori(tiltakFamilia.TttTiltakstype),
                        ssbUnderkategori = mappings.GetSSBUnderkategori(tiltakFamilia.TttTiltakstype),
                        ssbUnderkategoriSpesifisering = tiltakFamilia.TilTiltakstypePres,
                        planlagtFraDato = tiltakFamilia.TilFradato,
                        planlagtTilDato = tiltakFamilia.TilTildato,
                        iverksattDato = tiltakFamilia.TilIverksattdato,
                        bortfaltDato = tiltakFamilia.TilBortfaltdato,
                        avsluttetDato = tiltakFamilia.TilAvsluttetdato,
                        notat = tiltakFamilia.TilKommentar
                    };
                    if (tiltak.iverksattDato.HasValue && tiltak.iverksattDato.Value > DateTime.Now)
                    {
                        tiltak.iverksattDato = DateTime.Now;
                    }
                    if (tiltak.planlagtFraDato.HasValue && tiltak.iverksattDato.HasValue && tiltak.planlagtFraDato.Value > tiltak.iverksattDato.Value)
                    {
                        tiltak.planlagtFraDato = tiltak.iverksattDato;
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_HJEMMEBASERTE_TILTAK" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_FOSTERHJEMSTILTAK" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.ssbUnderkategori == "ANDRE_BOLIGTILTAK" && string.IsNullOrEmpty(tiltak.ssbUnderkategoriSpesifisering))
                    {
                        tiltak.ssbUnderkategoriSpesifisering = "Familia: Mangler presisering";
                    }
                    if (tiltak.planlagtTilDato.HasValue && tiltak.planlagtTilDato < tiltak.planlagtFraDato)
                    {
                        tiltak.planlagtTilDato = tiltak.planlagtFraDato;
                    }
                    if (tiltak.planlagtTilDato.HasValue && tiltak.planlagtTilDato < tiltak.iverksattDato)
                    {
                        tiltak.planlagtTilDato = null;
                    }
                    if (tiltakFamilia.TilUtenforhjemmet == 1)
                    {
                        tiltak.iEllerUtenforFamilie = "UTENFOR_FAMILIEN";
                    }
                    else
                    {
                        tiltak.iEllerUtenforFamilie = "I_FAMILIEN";
                    }
                    if (tiltakFamilia.SakAar.HasValue && tiltakFamilia.SakJournalnr.HasValue)
                    {
                        tiltak.aktivitetId = AddSpecificBydel(tiltakFamilia.SakAar.ToString() + "-" + tiltakFamilia.SakJournalnr.ToString(), "VED", bydel);
                    }
                    if (tiltak.avsluttetDato.HasValue && tiltak.iverksattDato.HasValue && tiltak.avsluttetDato.Value < tiltak.iverksattDato.Value)
                    {
                        tiltak.avsluttetDato = tiltak.iverksattDato;
                    }
                    if (tiltak.bortfaltDato.HasValue && tiltak.avsluttetDato.HasValue)
                    {
                        if (tiltakFamilia.TilIverksattdato.HasValue)
                        {
                            tiltak.bortfaltDato = null;
                        }
                        else
                        {
                            tiltak.avsluttetDato = null;
                        }
                    }
                    if (tiltak.bortfaltDato.HasValue)
                    {
                        tiltak.bortfaltKommentar = "Familia: Bortfalt";
                    }
                    if (!tiltak.bortfaltDato.HasValue && tiltak.avsluttetDato.HasValue && Mappings.IsSSBFosterhjemInstitusjon(tiltakFamilia.TttTiltakstype))
                    {
                        if (tiltakFamilia.TilHovedgrunnavsluttet == "1")
                        {
                            tiltak.avsluttetKode = "BARNET_TILBAKEFØRT_§_4-21";
                        }
                        else
                        {
                            if (tiltakFamilia.TilHovedgrunnavsluttet == "2")
                            {
                                tiltak.avsluttetKode = "BARNET_HAR_FYLT_18_ÅR";
                            }
                            else
                            {
                                if (tiltakFamilia.TilHovedgrunnavsluttet == "3")
                                {
                                    tiltak.avsluttetKode = "ADOPSJON_§_4-20";
                                }
                                else
                                {
                                    if (tiltakFamilia.TilHovedgrunnavsluttet == "4" || string.IsNullOrEmpty(tiltakFamilia.TilHovedgrunnavsluttet))
                                    {
                                        tiltak.avsluttetKode = "ANNET_(SPESIFISER)";
                                        tiltak.avsluttetSpesifisering = tiltakFamilia.TilPresisering;
                                        if (string.IsNullOrEmpty(tiltak.avsluttetSpesifisering))
                                        {
                                            tiltak.avsluttetSpesifisering = "Familia: Uspesifisert";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Mappings.IsSSBFosterhjem(tiltakFamilia.TttTiltakstype))
                    {
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            List<FaKlientadresser> adresser = await context.FaKlientadressers
                                .Where(a => a.KliLoepenr == tiltakFamilia.KliLoepenr && a.PahPassivisertdato.HasValue && a.PahPassivisertdato.Value.Year == DateTime.Now.Year && a.PahFraAarsak == "3.1").ToListAsync();

                            foreach (FaKlientadresser adresse in adresser)
                            {
                                Aktivitet flyttingMedFosterhjemAktivitet = new()
                                {
                                    aktivitetId = AddSpecificBydel(adresse.PahLoepenr.ToString(), "FMF", bydel),
                                    sakId = tiltak.sakId,
                                    aktivitetsType = "FOSTERHJEM",
                                    aktivitetsUnderType = "FLYTTING_MED_FOSTERHJEM",
                                    status = "UTFØRT",
                                    tittel = "Flytting med fosterhjem",
                                    hendelsesdato = adresse.PahPassivisertdato,
                                    utfortDato = adresse.PahPassivisertdato,
                                    saksbehandlerId = GetBrukerId(adresse.SbhEndretav, bydel),
                                    utfortAvId = GetBrukerId(adresse.SbhEndretav, bydel)
                                };
                                flyttingMedFosterhjemAktiviteter.Add(flyttingMedFosterhjemAktivitet);
                            }
                        }
                    }
                    if (Mappings.IsSSBFosterhjemInstitusjon(tiltakFamilia.TttTiltakstype) && tiltakFamilia.TilAvsluttetdato.HasValue && tiltakFamilia.TilAvsluttetdato.Value.Year > 2022)
                    {
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            List<FaKlientadresser> adresser = await context.FaKlientadressers
                                .Where(a => a.KliLoepenr == tiltakFamilia.KliLoepenr && a.PahPassivisertdato.HasValue && a.PahPassivisertdato.Value.Year == DateTime.Now.Year && a.PahFraAarsak != "3.1").ToListAsync();

                            FaKlientadresser adresse = adresser.OrderBy(o => Math.Abs((o.PahPassivisertdato.Value - tiltakFamilia.TilAvsluttetdato.Value).TotalDays)).FirstOrDefault();
                            if (adresse != null)
                            {
                                string aarsak = adresse.PahFraAarsak;
                                if (aarsak != null && !string.IsNullOrEmpty(aarsak.Trim()))
                                {
                                    tiltak.arsakFlyttingFraFosterhjemInstitusjon = mappings.GetÅrsaksFylttingFra(aarsak.Trim());
                                }
                                aarsak = adresse.PahTilAarsak;
                                if (aarsak != null && !string.IsNullOrEmpty(aarsak.Trim()))
                                {
                                    tiltak.flyttingTil = mappings.GetÅrsaksFylttingTil(aarsak.Trim());
                                }
                                if (adresse.PahFraSpesifiser != null && !string.IsNullOrEmpty(adresse.PahFraSpesifiser.Trim()))
                                {
                                    tiltak.arsakFlyttingFraPresisering = adresse.PahFraSpesifiser.Trim();
                                }
                                if (adresse.PahTilSpesifiser != null && !string.IsNullOrEmpty(adresse.PahTilSpesifiser.Trim()))
                                {
                                    tiltak.presiseringAvBosted = adresse.PahTilSpesifiser.Trim();
                                }
                                if (string.IsNullOrEmpty(tiltak.arsakFlyttingFraFosterhjemInstitusjon) || string.IsNullOrEmpty(tiltak.flyttingTil))
                                {
                                    if (Mappings.IsSSBFosterhjem(tiltakFamilia.TttTiltakstype))
                                    {
                                        tiltak.arsakFlyttingFraFosterhjemInstitusjon = "1.2.3_ANDRE_GRUNNER_(F.EKS._UENIGHET_OM_OPPDRAGETS_OMFANG_ØKONOMI_FORSTERKNINGSTILTAK_MANGLENDE_ELLER_LITE_EFFEKTIV_VEILEDNING_MV.)_(KREVER_PRESISERING)";
                                    }
                                    else
                                    {
                                        tiltak.arsakFlyttingFraFosterhjemInstitusjon = "2.9_ANDRE_GRUNNER_(KREVER_PRESISERING)";
                                    };
                                    tiltak.arsakFlyttingFraPresisering = "Familia: Mangler presisering";
                                    tiltak.flyttingTil = "8_ANNET_BOSTED_(SPESIFISER)";
                                    tiltak.presiseringAvBosted = "Familia: Mangler presisering";
                                    tiltak.avsluttetKode = "ANNET_(SPESIFISER)";
                                    tiltak.avsluttetSpesifisering = "Familia: Uspesifisert";
                                }
                            }
                            else
                            {
                                if (Mappings.IsSSBFosterhjem(tiltakFamilia.TttTiltakstype))
                                {
                                    tiltak.arsakFlyttingFraFosterhjemInstitusjon = "1.2.3_ANDRE_GRUNNER_(F.EKS._UENIGHET_OM_OPPDRAGETS_OMFANG_ØKONOMI_FORSTERKNINGSTILTAK_MANGLENDE_ELLER_LITE_EFFEKTIV_VEILEDNING_MV.)_(KREVER_PRESISERING)";
                                }
                                else
                                {
                                    tiltak.arsakFlyttingFraFosterhjemInstitusjon = "2.9_ANDRE_GRUNNER_(KREVER_PRESISERING)";
                                };
                                tiltak.flyttingTil = "8_ANNET_BOSTED_(SPESIFISER)";
                                tiltak.arsakFlyttingFraPresisering = "Familia: Mangler presisering";
                                tiltak.presiseringAvBosted = "Familia: Mangler presisering";
                                tiltak.avsluttetKode = "ANNET_(SPESIFISER)";
                                tiltak.avsluttetSpesifisering = "Familia: Uspesifisert";
                            }
                            if (tiltak.arsakFlyttingFraFosterhjemInstitusjon == "2.9_ANDRE_GRUNNER_(KREVER_PRESISERING)" && string.IsNullOrEmpty(tiltak.arsakFlyttingFraPresisering))
                            {
                                tiltak.arsakFlyttingFraPresisering = "Familia: Mangler presisering";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovHovedParagraf))
                    {
                        tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovHovedParagraf, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf1))
                    {
                        if (!string.IsNullOrEmpty(tiltak.lovhjemmel))
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                        else
                        {
                            tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf2))
                    {
                        if (!string.IsNullOrEmpty(tiltak.jfLovhjemmelNr1))
                        {
                            tiltak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                        else
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.Sak.SakIverksattdato, tiltakFamilia.Sak.SakSlutningdato);
                        }
                    }
                    if (tiltak.jfLovhjemmelNr2 == tiltak.lovhjemmel)
                    {
                        tiltak.jfLovhjemmelNr2 = null;
                    }
                    if (tiltak.jfLovhjemmelNr1 == tiltak.lovhjemmel)
                    {
                        tiltak.jfLovhjemmelNr1 = tiltak.jfLovhjemmelNr2;
                        tiltak.jfLovhjemmelNr2 = null;
                    }
                    if (tiltak.jfLovhjemmelNr1 == tiltak.jfLovhjemmelNr2)
                    {
                        tiltak.jfLovhjemmelNr2 = null;
                    }
                    string tiltakstype = tiltakFamilia.TttTiltakstype?.Trim();
                    if (tiltakstype == "14" || tiltakstype == "15" || tiltakstype == "16" || tiltakstype == "17" || tiltakstype == "18" || tiltakstype == "103" || tiltakstype == "104" || tiltakstype == "105" || tiltakstype == "106" || tiltakstype == "107" || tiltakstype == "108" || tiltakstype == "109")
                    {
                        if (tiltakFamilia.TilIverksattdato.HasValue)
                        {
                            tiltak.tilsynskommunenummer = await GetTilsynsKommunenummerSpecificBydelAsync(tiltakFamilia.KliLoepenr, bydel, tiltakFamilia.TilIverksattdato);
                        }
                    }
                    FaEngasjementsavtale engasjementsavtale = null;
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        engasjementsavtale = await context.FaEngasjementsavtales.Include(m => m.ForLoepenrNavigation.ForLoepenrNavigation).Where(e => string.IsNullOrEmpty(e.ForLoepenrNavigation.ForLoepenrNavigation.ForGmlreferanse) && e.TilLoepenr == tiltakFamilia.TilLoepenr && e.DokLoepenr.HasValue && e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                              && (e.EngTildato >= FirstInYearOfMigration)).OrderByDescending(o => o.EngTildato).FirstOrDefaultAsync();
                    }
                    if (engasjementsavtale != null)
                    {
                        Aktivitet aktivitet = new()
                        {
                            aktivitetId = AddSpecificBydel(engasjementsavtale.EngLoepenr.ToString(), "ODA", bydel),
                            sakId = $"{GetActorId(engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation, false)}__OPP",
                            aktivitetsType = "OPPDRAGSTAKER_AVTALE",
                            aktivitetsUnderType = "ANNET",
                            hendelsesdato = engasjementsavtale.EngFradato,
                            status = "UTFØRT",
                            tittel = "Oppdragstakeravtale",
                            notat = "Se dokument",
                            utfortDato = engasjementsavtale.EngAvgjortdato,
                            utlopsdato = engasjementsavtale.EngTildato,
                            tiltaksId = tiltak.tiltakId
                        };
                        if (!string.IsNullOrEmpty(engasjementsavtale.SbhInitialer))
                        {
                            aktivitet.saksbehandlerId = GetBrukerId(engasjementsavtale.SbhInitialer, bydel);
                            aktivitet.utfortAvId = GetBrukerId(engasjementsavtale.SbhInitialer, bydel);
                        }
                        oppdragstakeravtaleAktiviteter.Add(aktivitet);
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = engasjementsavtale.DokLoepenr.Value,
                            dokumentNr = engasjementsavtale.EngDokumentnr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.saksbehandlerId
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                    }
                    if (tiltak.planlagtFraDato.HasValue && tiltak.planlagtFraDato.Value.Year < 1998)
                    {
                        tiltak.planlagtFraDato = null;
                    }
                    if (tiltak.iverksattDato.HasValue && tiltak.iverksattDato.Value.Year < 1998)
                    {
                        if (tiltakFamilia.TilRegistrertdato.HasValue)
                        {
                            tiltak.iverksattDato = tiltakFamilia.TilRegistrertdato.Value.Date;
                        }
                    }
                    if (tiltak.avsluttetDato.HasValue && tiltak.avsluttetDato.Value.Year < 1998)
                    {
                        if (tiltakFamilia.TilRegistrertdato.HasValue)
                        {
                            tiltak.avsluttetDato = tiltakFamilia.TilRegistrertdato.Value.Date;
                        }
                    }
                    tiltaksliste.Add(tiltak);
                }
                await GetDocumentsAsync(worker, $"Oppdragsavtaleaktiviteter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                List<Aktivitet> oppdragstakeravtaleAktiviteterDistinct = oppdragstakeravtaleAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                int migrertAntall = oppdragstakeravtaleAktiviteterDistinct.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> oppdragstakeravtaleAktiviteterPart = oppdragstakeravtaleAktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(oppdragstakeravtaleAktiviteterPart, GetJsonFileName("aktiviteter", $"Oppdragsavtaleaktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                List<Aktivitet> flyttingMedFosterhjemAktiviteterDistinct = flyttingMedFosterhjemAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
                migrertAntall = flyttingMedFosterhjemAktiviteterDistinct.Count;
                guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> flyttingMedFosterhjemAktiviteterPart = flyttingMedFosterhjemAktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(flyttingMedFosterhjemAktiviteterPart, GetJsonFileName("aktiviteter", $"FlyttingMedFosterhjemAktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                List<Tiltak> tiltakslisteDistinct = tiltaksliste.GroupBy(c => c.tiltakId).Select(s => s.First()).ToList();
                migrertAntall = tiltakslisteDistinct.Count;
                foreach (Tiltak tiltak in tiltakslisteDistinct)
                {
                    if (tiltak.planlagtFraDato.HasValue && tiltak.planlagtFraDato < sak.startDato)
                    {
                        sak.startDato = tiltak.planlagtFraDato;
                    }
                    if (tiltak.iverksattDato.HasValue && tiltak.iverksattDato < sak.startDato)
                    {
                        sak.startDato = tiltak.iverksattDato;
                    }
                }
                guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Tiltak> tiltakslistePart = tiltakslisteDistinct.OrderBy(o => o.tiltakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(tiltakslistePart, GetJsonFileName("tiltak", $"Tiltak{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task GetAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            try
            {
                await GetTilsynsrapporterTidligereBydelAsync(worker, fodselsDato, personNummer, sak, bydel);
                await GetInterneSaksforberedelserTidligereBydelAsync(worker, fodselsDato, personNummer, sak, bydel);
                await GetJournalAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sak, bydel);
                await GetSlettedeJournalAktiviteterTidligereBydelAsync(fodselsDato, personNummer, sak, bydel);
                await GetIndividuellPlanAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sak, bydel);
                await GetPostjournalAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sak, bydel);
                await GetSlettedePostjournalAktiviteterTidligereBydelAsync(fodselsDato, personNummer, sak, bydel);
                await GetOppfølgingAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sak, bydel);
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task GetTilsynsrapporterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosBrevtype == "TB" && p.PosFerdigdato.HasValue)
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var postjournal in rawData)
            {
                if (postjournal.KliLoepenrNavigation.KliFraannenkommune == 1)
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sak.sakId,
                    aktivitetsType = "TILSYN",
                    aktivitetsUnderType = "TILSYNSBESØK",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato,
                    lovPaalagt = true
                };
                if (aktivitet.hendelsesdato.HasValue)
                {
                    aktivitet.tilsynAnsvarligKommunenummer = await GetTilsynsKommunenummerSpecificBydelAsync(postjournal.KliLoepenr.Value, bydel, aktivitet.hendelsesdato, false);
                }
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                }
                if (postjournal.PosDato.Year >= FirstInYearOfMigration.Year)
                {
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        FaTiltak tiltak = await context.FaTiltaks
                            .Where(m => m.KliLoepenr == postjournal.KliLoepenr
                                && (
                                m.TttTiltakstype == "14" ||
                                m.TttTiltakstype == "15" ||
                                m.TttTiltakstype == "16" ||
                                m.TttTiltakstype == "17" ||
                                m.TttTiltakstype == "18" ||
                                m.TttTiltakstype == "103" ||
                                m.TttTiltakstype == "104" ||
                                m.TttTiltakstype == "105" ||
                                m.TttTiltakstype == "106" ||
                                m.TttTiltakstype == "107" ||
                                m.TttTiltakstype == "108" ||
                                m.TttTiltakstype == "109") &&
                                (m.TilIverksattdato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilIverksattdato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddSpecificBydel(tiltak.TilLoepenr.ToString(), "TIL", bydel);
                        }
                    }
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    if (postjournal.PosUnndrattinnsyn == 1)
                    {
                        documentToInclude.merknadInnsyn = "Undratt innsyn";
                    }
                    else if (postjournal.PosVurderUnndratt == 1)
                    {
                        documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                    }
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, $"Tilsynsrapporter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Tilsynsrapporter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetPostjournalAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "KK" || p.PosBrevtype == "AS" || p.PosBrevtype == "AN" || p.PosBrevtype == "RF" || p.PosBrevtype == "RA" || p.PosBrevtype == "BR" || p.PosBrevtype == "TU" || p.PosBrevtype == "RS" || p.PosBrevtype == "RK" || p.PosBrevtype == "RM" || p.PosBrevtype == "RV" || p.PosBrevtype == "X" || p.PosBrevtype == "TM"))
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var postjournal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sak.sakId,
                    aktivitetsType = "ØVRIG_DOKUMENT",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato
                };
                if (postjournal.PosBrevtype == "TM")
                {
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        int antallMeldinger = await context.FaMeldingers
                            .Where(p => p.PosSendtkonklAar == postjournal.PosAar && p.PosSendtkonklLoepenr == postjournal.PosLoepenr)
                            .CountAsync();
                        if (antallMeldinger > 0)
                        {
                            continue;
                        }
                    }
                }
                if (postjournal.PosDokumentnr.HasValue)
                {
                    string formattedDokumentNr = postjournal.PosDokumentnr.Value.ToString();
                    if (formattedDokumentNr.Length < 5)
                    {
                        formattedDokumentNr = formattedDokumentNr.PadLeft(4, '0');
                    }
                    aktivitet.aktivitetId = $"{formattedDokumentNr}__" + aktivitet.aktivitetId;
                }
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                }
                if (!string.IsNullOrEmpty(postjournal.PosPosttype))
                {
                    switch (postjournal.PosPosttype?.Trim())
                    {
                        case "I":
                            aktivitet.aktivitetsUnderType = "INN";
                            aktivitet.notat = GetPostJounalMotpart(true, aktivitet.notat, postjournal.PosFornavn, postjournal.PosEtternavn);
                            break;
                        case "U":
                            aktivitet.aktivitetsUnderType = "UT";
                            aktivitet.notat = GetPostJounalMotpart(false, aktivitet.notat, postjournal.PosFornavn, postjournal.PosEtternavn);
                            break;
                        case "X":
                            aktivitet.aktivitetsUnderType = "NOTAT";
                            break;
                        default:
                            break;
                    }
                }
                if (aktivitet.utfortDato.HasValue && aktivitet.utfortDato.Value.Year < 1998)
                {
                    aktivitet.utfortDato = postjournal.PosRegistrertdato;
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year < 1998)
                {
                    aktivitet.hendelsesdato = postjournal.PosRegistrertdato;
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    if (postjournal.PosUnndrattinnsyn == 1)
                    {
                        documentToInclude.merknadInnsyn = "Undratt innsyn";
                    }
                    else if (postjournal.PosVurderUnndratt == 1)
                    {
                        documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                    }
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, $"Postjournalaktiviteter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Postjournalaktiviteter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetSlettedePostjournalAktiviteterTidligereBydelAsync(DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaPostjournal> rawData;

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(p => p.PosFerdigdato != null && p.PosSlettet == 1)
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var postjournal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sak.sakId,
                    aktivitetsType = "SLETTET",
                    aktivitetsUnderType = "SLETTET",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    utfortDato = postjournal.PosFerdigdato
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                }
                if (!string.IsNullOrEmpty(postjournal.PosBegrSlettet))
                {
                    aktivitet.notat = $"Årsak: {postjournal.PosBegrSlettet}";
                };
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
            }
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"SlettedePostjournalaktiviteter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetOppfølgingAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "OB"))
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var postjournal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sak.sakId,
                    aktivitetsType = "OPPFØLGING",
                    aktivitetsUnderType = "OPPFØLGINGSBESØK",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato,
                    lovPaalagt = true
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                }
                if (postjournal.PosDato.Year >= FirstInYearOfMigration.Year)
                {
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        FaTiltak tiltak = await context.FaTiltaks
                            .Where(m => m.KliLoepenr == postjournal.KliLoepenr
                                && (
                                m.TttTiltakstype == "14" ||
                                m.TttTiltakstype == "15" ||
                                m.TttTiltakstype == "16" ||
                                m.TttTiltakstype == "17" ||
                                m.TttTiltakstype == "18" ||
                                m.TttTiltakstype == "103" ||
                                m.TttTiltakstype == "104" ||
                                m.TttTiltakstype == "105" ||
                                m.TttTiltakstype == "106" ||
                                m.TttTiltakstype == "107" ||
                                m.TttTiltakstype == "108" ||
                                m.TttTiltakstype == "109") &&
                                (m.TilIverksattdato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilIverksattdato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddSpecificBydel(tiltak.TilLoepenr.ToString(), "TIL", bydel);
                        }
                        else if (postjournal.KliLoepenrNavigation.KliFraannenkommune != 1)
                        {
                            aktivitet.aktivitetsType = "ØVRIG_DOKUMENT";
                            aktivitet.aktivitetsUnderType = "NOTAT";
                        }
                    }
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    if (postjournal.PosUnndrattinnsyn == 1)
                    {
                        documentToInclude.merknadInnsyn = "Undratt innsyn";
                    }
                    else if (postjournal.PosVurderUnndratt == 1)
                    {
                        documentToInclude.merknadInnsyn = "Familia: Vurder unndratt";
                    }
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, $"Oppfølgingsaktiviteter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Oppfølgingsaktiviteter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetInterneSaksforberedelserTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 1 && p.PosFerdigdato.HasValue)
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var postjournal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sak.sakId,
                    aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)",
                    aktivitetsUnderType = "INGEN",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.SbhRegistrertav, bydel);
                }
                else
                {
                    aktivitet.saksbehandlerId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                    aktivitet.utfortAvId = GetBrukerId(postjournal.KliLoepenrNavigation.SbhInitialer, bydel);
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
                        dokumentNr = postjournal.PosDokumentnr,
                        sakId = aktivitet.sakId,
                        tittel = aktivitet.tittel,
                        journalDato = aktivitet.utfortDato,
                        opprettetAvId = aktivitet.saksbehandlerId
                    };
                    documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    documentsIncluded.Add(documentToInclude);
                }
            }
            await GetDocumentsAsync(worker, $"InterneSaksforberedelser{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"InterneSaksforberedelser{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetJournalAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaJournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();
            List<TextDocumentToInclude> textDocumentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaJournals.Include(x => x.KliLoepenrNavigation).Include(x => x.JotIdentNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(m => m.JouSlettet != 1 && m.JouFerdigdato != null && m.KliLoepenr.HasValue)
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var journal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(journal.JouLoepenr.ToString(), "JOU", bydel),
                    sakId = sak.sakId,
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = GetBrukerId(journal.SbhInitialer, bydel),
                    tittel = journal.JouEmne,
                    utfortAvId = GetBrukerId(journal.SbhInitialer, bydel),
                    utfortDato = journal.JouFerdigdato,
                    notat = journal.JouNotat
                };
                if (journal.JouUnndrattinnsynIs == 1)
                {
                    aktivitet.aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)";
                    aktivitet.aktivitetsUnderType = "INGEN";
                }
                else
                {
                    if (journal.JotIdent == "HJ" || journal.JotIdent == "KS" || journal.JotIdent == "NO" || journal.JotIdent == "NF" || journal.JotIdent == "OT" || journal.JotIdent == "SB" || journal.JotIdent == "TO" || journal.JotIdent == "ET")
                    {
                        aktivitet.aktivitetsType = "SAMLENOTAT";
                        switch (journal.JotIdent)
                        {
                            case "HJ":
                                aktivitet.aktivitetsUnderType = "HJEMMEBESØK";
                                break;
                            case "KS":
                            case "ET":
                            case "OT":
                                aktivitet.aktivitetsUnderType = "ANNET";
                                break;
                            case "NO":
                            case "NF":
                                aktivitet.aktivitetsUnderType = "NOTAT";
                                break;
                            case "SB":
                                aktivitet.aktivitetsUnderType = "SAMTALE_MED_BARNET";
                                break;
                            case "TO":
                                aktivitet.aktivitetsUnderType = "TELEFONSAMTALE";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (journal.JotIdent == "IN")
                        {
                            aktivitet.aktivitetsType = "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)";
                            aktivitet.aktivitetsUnderType = "INGEN";
                        }
                        else if (journal.JotIdent == "OP")
                        {
                            aktivitet.aktivitetsType = "OPPFØLGING";
                            aktivitet.aktivitetsUnderType = "OPPFØLGINGSBESØK";
                        }
                        else if (journal.JotIdent == "TB")
                        {
                            aktivitet.aktivitetsType = "TILSYN";
                            aktivitet.aktivitetsUnderType = "TILSYNSBESØK";
                            aktivitet.lovPaalagt = true;
                        }
                    }
                }
                if (string.IsNullOrEmpty(aktivitet.utfortAvId))
                {
                    aktivitet.utfortAvId = GetBrukerId(journal.KliLoepenrNavigation.SbhInitialer, bydel);
                }
                string merknadInnsyn = "";
                if (aktivitet.aktivitetsType == "INTERN_SAKSFORBEREDELSE(FVL_§_18.A)" || journal.JouUnndrattinnsyn == 1)
                {
                    merknadInnsyn = $"Jmf.vedtak: {journal.SakUnndrAar}{journal.SakUnndrJournalnr}";
                }
                else if (journal.JouVurderUnndratt == 1)
                {
                    merknadInnsyn = "Vurder unndratt innsyn";
                }
                if (journal.DokLoepenr.HasValue)
                {
                    FaDokumenter dokument;
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == journal.DokLoepenr.Value).FirstOrDefaultAsync();
                    }
                    if (dokument != null)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = dokument.DokLoepenr,
                            dokumentNr = journal.JouDokumentnr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            merknadInnsyn = merknadInnsyn
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                        aktivitet.notat = null;
                    }
                    else
                    {
                        if (aktivitet.aktivitetsType == "OPPFØLGING" && !string.IsNullOrEmpty(journal.JouNotat))
                        {
                            TextDocumentToInclude textDocumentToInclude = new()
                            {
                                dokLoepenr = journal.JouLoepenr,
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato,
                                opprettetAvId = aktivitet.utfortAvId,
                                innhold = journal.JouNotat,
                                beskrivelse = journal.JotIdentNavigation?.JotBeskrivelse,
                                datonotat = journal.JouDatonotat,
                                foedselsdato = journal.KliLoepenrNavigation?.KliFoedselsdato,
                                forNavn = journal.KliLoepenrNavigation?.KliFornavn,
                                etterNavn = journal.KliLoepenrNavigation?.KliEtternavn,
                                merknadInnsyn = merknadInnsyn
                            };
                            textDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                            textDocumentsIncluded.Add(textDocumentToInclude);
                            aktivitet.notat = null;
                        }
                    }
                }
                else
                {
                    if (aktivitet.aktivitetsType == "OPPFØLGING" && !string.IsNullOrEmpty(journal.JouNotat))
                    {
                        TextDocumentToInclude textDocumentToInclude = new()
                        {
                            dokLoepenr = journal.JouLoepenr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            innhold = journal.JouNotat,
                            beskrivelse = journal.JotIdentNavigation?.JotBeskrivelse,
                            datonotat = journal.JouDatonotat,
                            foedselsdato = journal.KliLoepenrNavigation?.KliFoedselsdato,
                            forNavn = journal.KliLoepenrNavigation?.KliFornavn,
                            etterNavn = journal.KliLoepenrNavigation?.KliEtternavn,
                            merknadInnsyn = merknadInnsyn
                        };
                        textDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        textDocumentsIncluded.Add(textDocumentToInclude);
                        aktivitet.notat = null;
                    }
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year < 1998)
                {
                    aktivitet.hendelsesdato = journal.JouRegistrertdato;
                }
                if (aktivitet.utfortDato.HasValue && aktivitet.utfortDato.Value.Year < 1998)
                {
                    aktivitet.utfortDato = journal.JouRegistrertdato;
                }
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
            }
            await GetDocumentsAsync(worker, $"Journaler{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            await GetTextDocumentsAsync(worker, textDocumentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Journaler{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetSlettedeJournalAktiviteterTidligereBydelAsync(DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaJournal> rawData;

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaJournals.Include(x => x.KliLoepenrNavigation).Include(x => x.JotIdentNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(m => m.JouFerdigdato != null && m.JouSlettet == 1 && m.KliLoepenr.HasValue)
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var journal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(journal.JouLoepenr.ToString(), "JOU", bydel),
                    sakId = sak.sakId,
                    aktivitetsType = "SLETTET",
                    aktivitetsUnderType = "SLETTET",
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = GetBrukerId(journal.SbhInitialer, bydel),
                    tittel = journal.JouEmne,
                    utfortAvId = GetBrukerId(journal.SbhInitialer, bydel),
                    utfortDato = journal.JouFerdigdato,
                    notat = journal.JouNotat
                };
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
            }
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"SlettedeJournaler{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetIndividuellPlanAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, decimal personNummer, Sak sak, string bydel)
        {
            List<FaTiltaksplan> rawData;
            List<DocumentToInclude> documentsIncluded = new();
            List<TextDocumentToInclude> textDocumentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaTiltaksplans.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(m => m.TtpSlettet == 0 && m.PtyPlankode == "8" && m.TtpFerdigdato.HasValue)
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var plan in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(plan.TtpLoepenr.ToString(), "IVP", bydel),
                    sakId = sak.sakId,
                    status = "UTFØRT",
                    aktivitetsType = "ØVRIG_DOKUMENT",
                    aktivitetsUnderType = "INDIVIDUELL_PLAN",
                    tittel = "Individuell plan",
                    hendelsesdato = plan.TtpRegistrertdato,
                    saksbehandlerId = GetBrukerId(plan.SbhInitialer, bydel),
                    utfortAvId = GetBrukerId(plan.SbhRegistrertav, bydel),
                    utfortDato = plan.TtpFerdigdato,
                    notat = "Se dokument"
                };
                if (plan.TtpFradato.HasValue)
                {
                    aktivitet.tittel += $" fra {plan.TtpFradato.Value:dd.MM.yyyy}";
                };
                if (plan.TtpTildato.HasValue)
                {
                    aktivitet.tittel += $" til {plan.TtpTildato.Value:dd.MM.yyyy}";
                };
                if (aktivitet.hendelsesdato.HasValue && aktivitet.hendelsesdato.Value.Year > 1997 && sak.startDato > aktivitet.hendelsesdato)
                {
                    sak.startDato = aktivitet.hendelsesdato;
                }
                aktiviteter.Add(aktivitet);
                if (plan.DokLoepenr.HasValue)
                {
                    FaDokumenter dokument;
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == plan.DokLoepenr.Value).FirstOrDefaultAsync();
                    }

                    if (dokument != null)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = dokument.DokLoepenr,
                            dokumentNr = plan.TtpDokumentnr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                    }
                }
            }
            await GetDocumentsAsync(worker, $"IndividuellPlan{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            await GetTextDocumentsAsync(worker, textDocumentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            List<Aktivitet> aktiviteterDistinct = aktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()).ToList();
            int migrertAntall = aktiviteterDistinct.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteterDistinct.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"IndividuellPlan{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        #endregion

        #region Helper functions
        private async Task<string> GetDocumentsAsync(BackgroundWorker worker, string sourceName, List<DocumentToInclude> documentsIncluded, bool showProgress = true, string bydel = "")
        {
            try
            {
                if (showProgress)
                {
                    worker.ReportProgress(0, $"Starter uttrekk dokumenter og filer for {sourceName} ...");
                }
                if (string.IsNullOrEmpty(bydel))
                {
                    bydel = Bydelsforkortelse;
                }
                List<InternalDocument> rawData;
                int totalNumberExtracted = 0;
                int numberProcessed = 0;
                int maxNumberOfDocumentsEachBatch = 200;
                List<Document> documents = new();

                List<decimal> simpleDocumentsIncluded = new();
                documentsIncluded.ForEach(l => simpleDocumentsIncluded.Add(l.dokLoepenr));
                int numberOfDocumentsIncluded = simpleDocumentsIncluded.Count;
                int documentsToSkip = 0;

                while (numberOfDocumentsIncluded > documentsToSkip)
                {
                    List<decimal> partSimpleDocumentsIncluded = simpleDocumentsIncluded.Skip(documentsToSkip).Take(maxNumberOfDocumentsEachBatch).ToList();

                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        rawData = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && partSimpleDocumentsIncluded.Contains(d.DokLoepenr))
                        .Select(d => new InternalDocument
                        {
                            dokLoepenr = d.DokLoepenr,
                            dokMimetype = d.DokMimetype
                        }).ToListAsync();
                        totalNumberExtracted += rawData.Count;
                    }
                    SqlConnection connection = new(mappings.GetConnectionstring(bydel, MainDBServer));
                    SqlDataReader reader = null;
                    SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                    SqlDataReader readerMigrering = null;
                    try
                    {
                        connection.Open();
                        connectionMigrering.Open();

                        foreach (var documentFamilia in rawData)
                        {
                            numberProcessed += 1;

                            if (showProgress && numberProcessed % 10 == 0)
                            {
                                worker.ReportProgress(0, $"Behandler uttrekk dokumenter og filer for {sourceName} ({numberProcessed} av foreløpig {totalNumberExtracted})...");
                            }
                            DocumentToInclude documentToInclude = documentsIncluded.Find(d => d.dokLoepenr == documentFamilia.dokLoepenr);
                            Document document = new()
                            {
                                filId = AddSpecificBydel(documentFamilia.dokLoepenr.ToString(), bydel),
                                ferdigstilt = true,
                                opprettetAvId = documentToInclude.opprettetAvId,
                                sakId = documentToInclude.sakId,
                                aktivitetIdListe = documentToInclude.aktivitetIdListe,
                                tittel = documentToInclude.tittel,
                                journalDato = documentToInclude.journalDato,
                                merknadInnsyn = documentToInclude.merknadInnsyn,
                                filFormat = "PDF"
                            };
                            if (!string.IsNullOrEmpty(document.tittel) && document.tittel.Length > 250)
                            {
                                document.tittel = document.tittel[..250];
                            }
                            if (documentToInclude.dokumentNr.HasValue)
                            {
                                string formattedDokumentNr = documentToInclude.dokumentNr.Value.ToString();
                                if (formattedDokumentNr.Length < 5)
                                {
                                    formattedDokumentNr = formattedDokumentNr.PadLeft(4, '0');
                                }
                                document.dokumentId = AddSpecificBydel($"{formattedDokumentNr}__" + documentFamilia.dokLoepenr.ToString(), bydel);
                            }
                            else
                            {
                                document.dokumentId = AddSpecificBydel("INTE__" + documentFamilia.dokLoepenr.ToString(), bydel);
                            }
                            string fileExtension = ".doc";
                            string dokMimetype = documentFamilia.dokMimetype?.Trim();
                            if (!string.IsNullOrEmpty(dokMimetype))
                            {
                                dokMimetype = dokMimetype.ToLower();
                                if (dokMimetype == "application/pdf")
                                {
                                    fileExtension = ".pdf";
                                }
                                else
                                {
                                    if (dokMimetype == "text/html")
                                    {
                                        fileExtension = ".html";
                                    }
                                }
                            }
                            document.filId += fileExtension;
                            documents.Add(document);

                            bool fileAlreadyWritten = false;
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer{MigreringsdbPostfix} Where Filnavn='{document.filId}'", connectionMigrering)
                            {
                                CommandTimeout = 300
                            };
                            readerMigrering = commandMigrering.ExecuteReader();
                            while (readerMigrering.Read())
                            {
                                if (readerMigrering.GetInt32(0) > 0)
                                {
                                    fileAlreadyWritten = true;
                                }
                            }
                            readerMigrering.Close();

                            if (!fileAlreadyWritten)
                            {
                                SqlCommand command = new($"Select Dok_Dokument From FA_DOKUMENTER Where DOK_Loepenr={documentFamilia.dokLoepenr}", connection)
                                {
                                    CommandTimeout = 300
                                };
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + document.filId, (byte[])reader["Dok_Dokument"]);
                                }
                                reader.Close();
                                commandMigrering = new($"Insert Into Filer{MigreringsdbPostfix} (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
                                {
                                    CommandTimeout = 300
                                };
                                commandMigrering.ExecuteNonQuery();
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                        connectionMigrering.Close();
                    }
                    documentsToSkip += maxNumberOfDocumentsEachBatch;
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Document> documentsDistinct = documents.GroupBy(c => new { c.dokumentId, c.sakId }).Select(s => s.First()).ToList();
                int migrertAntall = documentsDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<Document> documentsPart = documentsDistinct.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"Dokumenter{fileNumber}{Guid.NewGuid().ToString().Replace('-', '_')}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall dokumenter {sourceName}: {numberProcessed}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task<string> GetTextDocumentsAsync(BackgroundWorker worker, List<TextDocumentToInclude> textDocumentsIncluded, string bydel = "")
        {
            try
            {
                worker.ReportProgress(0, $"Starter opprettelse av dokumenter og tekstfiler ...");

                if (string.IsNullOrEmpty(bydel))
                {
                    bydel = Bydelsforkortelse;
                }
                int totalNumber = textDocumentsIncluded.Count;
                int numberProcessed = 0;
                List<Document> documents = new();

                foreach (var documentFamilia in textDocumentsIncluded)
                {
                    numberProcessed += 1;

                    if (numberProcessed % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler opprettelse dokumenter og tekstfiler ({numberProcessed} av {totalNumber})...");
                    }
                    Document document = new()
                    {
                        dokumentId = AddSpecificBydel(documentFamilia.dokLoepenr.ToString() + "TXT", bydel),
                        filId = AddSpecificBydel(documentFamilia.dokLoepenr.ToString() + "TXT", bydel),
                        ferdigstilt = true,
                        opprettetAvId = documentFamilia.opprettetAvId,
                        sakId = documentFamilia.sakId,
                        aktivitetIdListe = documentFamilia.aktivitetIdListe,
                        tittel = documentFamilia.tittel,
                        journalDato = documentFamilia.journalDato,
                        filFormat = "PDF",
                        merknadInnsyn = documentFamilia.merknadInnsyn
                    };
                    if (!string.IsNullOrEmpty(document.tittel) && document.tittel.Length > 250)
                    {
                        document.tittel = document.tittel[..250];
                    }
                    document.filId += ".txt";
                    documents.Add(document);
                    string header = "Navn: ";
                    if (!string.IsNullOrEmpty(documentFamilia.forNavn))
                    {
                        header += documentFamilia.forNavn;
                        if (!string.IsNullOrEmpty(documentFamilia.etterNavn))
                        {
                            header += " " + documentFamilia.etterNavn;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(documentFamilia.etterNavn))
                        {
                            header += documentFamilia.etterNavn;
                        }
                        else
                        {
                            header += "-";
                        }
                    }
                    if (documentFamilia.foedselsdato.HasValue)
                    {
                        header += $" Fødselsdato: {documentFamilia.foedselsdato.Value:dd.MM.yyyy}";
                    }
                    header += Environment.NewLine;
                    header += $"Dato: {documentFamilia.datonotat:dd.MM.yyyy}";
                    if (documentFamilia.journalDato.HasValue)
                    {
                        header += $" Ferdigdato: {documentFamilia.journalDato:dd.MM.yyyy}";
                    }
                    header += Environment.NewLine;
                    if (!string.IsNullOrEmpty(documentFamilia.tittel))
                    {
                        header += $"Emne: {documentFamilia.tittel}" + Environment.NewLine;
                    }
                    if (!string.IsNullOrEmpty(documentFamilia.beskrivelse))
                    {
                        header += $"Beskrivelse: {documentFamilia.beskrivelse}";
                    }
                    header += Environment.NewLine + Environment.NewLine;
                    if (!OnlyWriteDocumentFiles)
                    {
                        await File.WriteAllTextAsync(OutputFolderName + "filer\\" + document.filId, header + documentFamilia.innhold);
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Document> documentsDistinct = documents.GroupBy(c => new { c.dokumentId, c.sakId }).Select(s => s.First()).ToList();
                int migrertAntall = documentsDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<Document> documentsPart = documentsDistinct.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"DokumenterTekst{fileNumber}{Guid.NewGuid().ToString().Replace('-', '_')}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                return $"Antall dokumenter tekst: {numberProcessed}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task GetHtmlDocumentsAsync(BackgroundWorker worker, List<HtmlDocumentToInclude> htmlDocumentsIncluded, string category, string abbreviation, bool showStatus = true)
        {
            try
            {
                if (showStatus)
                {
                    worker.ReportProgress(0, $"Starter opprettelse av Visma Flyt Barnevernvakt dokumenter og html-filer ...");
                }
                int totalNumber = htmlDocumentsIncluded.Count;
                int numberProcessed = 0;
                List<Document> documents = new();

                foreach (var documentBVV in htmlDocumentsIncluded)
                {
                    numberProcessed += 1;

                    if (showStatus && numberProcessed % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler opprettelse Visma Flyt Barnevernvakt dokumenter og html-filer ({numberProcessed} av {totalNumber})...");
                    }
                    Document document = new()
                    {
                        dokumentId = AddBydel(documentBVV.dokLoepenr.ToString(), abbreviation),
                        filId = AddBydel(documentBVV.dokLoepenr.ToString(), abbreviation),
                        ferdigstilt = true,
                        opprettetAvId = documentBVV.opprettetAvId,
                        sakId = documentBVV.sakId,
                        aktivitetIdListe = documentBVV.aktivitetIdListe,
                        tittel = documentBVV.tittel,
                        journalDato = documentBVV.journalDato,
                        filFormat = "PDF"
                    };
                    if (!string.IsNullOrEmpty(document.tittel) && document.tittel.Length > 250)
                    {
                        document.tittel = document.tittel[..250];
                    }
                    document.filId += ".html";
                    documents.Add(document);
                    if (!OnlyWriteDocumentFiles)
                    {
                        await File.WriteAllTextAsync(OutputFolderName + "filer\\" + document.filId, $"<html>{documentBVV.innhold}</html>");
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                List<Document> documentsDistinct = documents.GroupBy(c => new { c.dokumentId, c.sakId }).Select(s => s.First()).ToList();
                int migrertAntall = documentsDistinct.Count;
                while (migrertAntall > toSkip)
                {
                    List<Document> documentsPart = documentsDistinct.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"BVVDokumenterHTML{category}{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private void GetOrganisasjonsKategori(Organisasjon organisasjon, FaForbindelser forbindelse)
        {
            SqlConnection connection = new(ConnectionStringFamilia);
            SqlDataReader reader;

            try
            {
                connection.Open();
                SqlCommand command = new($"Select Fot_Ident From FA_FORBINDELSESROLLER Where For_loepenr={forbindelse.ForLoepenr}", connection)
                {
                    CommandTimeout = 300
                };
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string typeCode = null;
                    if (!string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        typeCode = reader.GetString(0);
                        typeCode = typeCode?.Trim().ToUpper();

                        switch (typeCode)
                        {
                            case "ADV":
                                organisasjon.kategori.Add("ADVOKAT");
                                break;
                            case "AND":
                            case "VEI":
                                organisasjon.kategori.Add("ANNET");
                                break;
                            case "BHB":
                                organisasjon.kategori.Add("BARNEHAGE");
                                break;
                            case "INS":
                                organisasjon.kategori.Add("BARNEVERNSINSTITUSJON");
                                break;
                            case "BAR":
                                organisasjon.kategori.Add("BARNEVERNSTJENESTE");
                                break;
                            case "BER":
                                organisasjon.kategori.Add("BEREDSKAPSHJEM");
                                break;
                            case "BUP":
                                organisasjon.kategori.Add("BUP");
                                break;
                            case "DPS":
                                organisasjon.kategori.Add("DPS");
                                break;
                            case "FMR":
                                organisasjon.kategori.Add("FAMILIERÅDSKOORDINATOR");
                                break;
                            case "FVK":
                                organisasjon.kategori.Add("FAMILIEVERNKONTOR");
                                break;
                            case "FRI":
                                organisasjon.kategori.Add("FRITIDSTILBUD");
                                break;
                            case "HEL":
                                organisasjon.kategori.Add("HELSESTASJON_SKOLEHELSETJENESTE");
                                break;
                            case "KOM":
                                organisasjon.kategori.Add("KOMMUNE_BYDEL");
                                break;
                            case "KRS":
                                organisasjon.kategori.Add("KRISESENTER");
                                break;
                            case "LEG":
                                organisasjon.kategori.Add("LEGE_LEGESENTER_LEGEVAKT");
                                break;
                            case "SOS":
                                organisasjon.kategori.Add("NAV");
                                break;
                            case "POL":
                                organisasjon.kategori.Add("POLITI");
                                break;
                            case "PPT":
                                organisasjon.kategori.Add("PPT");
                                break;
                            case "PSY":
                                organisasjon.kategori.Add("PSYKOLOG_PRIVAT");
                                break;
                            case "RUS":
                                organisasjon.kategori.Add("RUSBEHANDLING");
                                break;
                            case "SAK":
                                organisasjon.kategori.Add("SAKKYNDIG");
                                break;
                            case "SFO":
                                organisasjon.kategori.Add("SFO_AKTIVITETSSKOLE");
                                break;
                            case "SKO":
                                organisasjon.kategori.Add("SKOLE");
                                break;
                            case "SYK":
                                organisasjon.kategori.Add("SYKEHUS");
                                break;
                            case "TAN":
                                organisasjon.kategori.Add("TANNHELSE");
                                break;
                            case "PRA":
                                organisasjon.kategori.Add("TILTAKSUTFØRER");
                                break;
                            case "TLK":
                                organisasjon.kategori.Add("TOLK");
                                break;
                            case "TRA":
                                organisasjon.kategori.Add("TRANSPORT");
                                break;
                            case "VEN":
                                organisasjon.kategori.Add("VENTEHJEM");
                                break;
                            case "MYN":
                                organisasjon.kategori.Add("ØVRIGE_MYNDIGHETER");
                                break;
                        }

                    }
                }
                reader.Close();
            }
            finally
            {
                connection.Close();
            }
        }
        private static void GetNettverksRolle(FaKlienttilknytning klientTilknytning, BarnetsNettverk forbindelse)
        {
            if (klientTilknytning.KtkRolle == "MOR" || klientTilknytning.KtkRolle == "FAR" || klientTilknytning.KtkRolle == "SØS" || klientTilknytning.KtkRolle == "FAM" || klientTilknytning.KtkRolle == "FSA")
            {
                forbindelse.relasjonTilSak = "FAMILIE";
            }
            else
            {
                if (klientTilknytning.KtkRolle == "ADV" || klientTilknytning.KtkRolle == "FRK" || klientTilknytning.KtkRolle == "BHA" || klientTilknytning.KtkRolle == "SFO" ||
                    klientTilknytning.KtkRolle == "SKL" || klientTilknytning.KtkRolle == "PPT" || klientTilknytning.KtkRolle == "HST" || klientTilknytning.KtkRolle == "PSG" ||
                    klientTilknytning.KtkRolle == "LGE" || klientTilknytning.KtkRolle == "BUP" || klientTilknytning.KtkRolle == "ANS" || klientTilknytning.KtkRolle == "MJØ" ||
                    klientTilknytning.KtkRolle == "TSF" || klientTilknytning.KtkRolle == "STØ" || klientTilknytning.KtkRolle == "BSH" || klientTilknytning.KtkRolle == "BRH" ||
                    klientTilknytning.KtkRolle == "INS" || klientTilknytning.KtkRolle == "FMO" || klientTilknytning.KtkRolle == "FFA")
                {
                    forbindelse.relasjonTilSak = "PROFESJONELL_KONTAKT";
                }
                else
                {
                    if (klientTilknytning.KtkRolle == "VRG" || klientTilknytning.KtkRolle == "AND")
                    {
                        forbindelse.relasjonTilSak = "ANNEN";
                    }
                }
            }
            switch (klientTilknytning.KtkRolle?.Trim())
            {
                case "AND":
                case "ANS":
                    forbindelse.rolle = "ANDRE";
                    break;
                case "STØ":
                case "BSH":
                    forbindelse.rolle = "STØTTEKONTAKT";
                    break;
                case "FMO":
                case "FFA":
                    forbindelse.rolle = "FOSTERHJEM";
                    break;
                case "ADV":
                    forbindelse.rolle = "ADVOKAT";
                    break;
                case "FRK":
                    forbindelse.rolle = "FAMILIERÅDSKOORDINATOR";
                    break;
                case "MOR":
                    forbindelse.rolle = "MOR";
                    break;
                case "FAR":
                    forbindelse.rolle = "FAR";
                    break;
                case "SØS":
                    forbindelse.rolle = "SØSKEN";
                    break;
                case "BHA":
                    forbindelse.rolle = "BARNEHAGE";
                    break;
                case "FAM":
                    forbindelse.rolle = "ØVRIG_FAMILIE_SLEKT";
                    break;
                case "SFO":
                    forbindelse.rolle = "SFO_AKTIVITETSSKOLE";
                    break;
                case "SKL":
                    forbindelse.rolle = "SKOLE";
                    break;
                case "PPT":
                    forbindelse.rolle = "PPT";
                    break;
                case "HST":
                    forbindelse.rolle = "HELSESTASJON_SKOLEHELSETJENESTE";
                    break;
                case "PSG":
                    forbindelse.rolle = "PSYKOLOG_PRIVAT";
                    break;
                case "LGE":
                    forbindelse.rolle = "LEGE_LEGESENTER";
                    break;
                case "BUP":
                    forbindelse.rolle = "BUP";
                    break;
                case "MJØ":
                    forbindelse.rolle = "MILJØARBEIDER";
                    break;
                case "TSF":
                    forbindelse.rolle = "TILSYNSPERSON";
                    break;
                case "BRH":
                    forbindelse.rolle = "BEREDSKAPSHJEM";
                    break;
                case "INS":
                    forbindelse.rolle = "BARNEVERNSINSTITUSJON";
                    break;
                case "VRG":
                    forbindelse.rolle = "VERGE";
                    break;
                case "FSA":
                    forbindelse.rolle = "FORESATT";
                    break;
            }
        }
        private MottattBekymringsmelding GetMottattBekymringsmelding(FaMeldinger meldingFamilia, string bydel)
        {
            MottattBekymringsmelding mottattBekymringsmelding = new()
            {
                mottattDato = meldingFamilia.MelMottattdato,
                innhold = meldingFamilia.MelMelding?.Trim(),
                typeMelderPresisering = meldingFamilia.MelMeldtPresAndreOffent?.Trim(),
                mottattBekymringsmeldingsType = "OFFENTLIG",
                melderKode = (meldingFamilia.MelMottattmaate?.Trim()) switch
                {
                    "ANN" or "MØT" => "MØTE",
                    "SKR" => "BREV",
                    "TLF" => "TELEFON",
                    _ => "BREV",
                },
                melderFritekst = AddTextToStringIfNotEmpty("", meldingFamilia.MelMelderfornavn?.Trim(), "")
            };
            mottattBekymringsmelding.melderFritekst = AddTextToStringIfNotEmpty(mottattBekymringsmelding.melderFritekst, meldingFamilia.MelMelderetternavn?.Trim(), " ");
            mottattBekymringsmelding.melderFritekst = AddTextToStringIfNotEmpty(mottattBekymringsmelding.melderFritekst, meldingFamilia.MelMelderadresse?.Trim(), ", ");
            mottattBekymringsmelding.melderFritekst = AddTextToStringIfNotEmpty(mottattBekymringsmelding.melderFritekst, meldingFamilia.MelMeldertelefonprivat?.Trim(), ", P:");
            mottattBekymringsmelding.melderFritekst = AddTextToStringIfNotEmpty(mottattBekymringsmelding.melderFritekst, meldingFamilia.MelMeldertelefonarbeid?.Trim(), ", A:");
            mottattBekymringsmelding.melderFritekst = AddTextToStringIfNotEmpty(mottattBekymringsmelding.melderFritekst, meldingFamilia.MelMeldertelefonmobil?.Trim(), ", M:");

            GetTypeMelder(meldingFamilia, mottattBekymringsmelding);
            if (mottattBekymringsmelding.typeMelder.Contains("ANDRE_OFFENTLIG_INSTANSER"))
            {
                if (string.IsNullOrEmpty(mottattBekymringsmelding.typeMelderPresisering))
                {
                    mottattBekymringsmelding.typeMelderPresisering = "Familia: Presisering mangler";
                }
            }
            GetSaksinnholdMelding(meldingFamilia, mottattBekymringsmelding);

            switch (meldingFamilia.MelMeldingstype?.Trim())
            {
                case "ORD":
                case "ABY":
                    mottattBekymringsmelding.meldingstype = "ORDINÆR_BEKYMRINGSMELDING";
                    break;
                case "SØK":
                    if (mottattBekymringsmelding.mottattBekymringsmeldingsType == "OFFENTLIG")
                    {
                        mottattBekymringsmelding.meldingstype = "ORDINÆR_BEKYMRINGSMELDING";
                    }
                    else
                    {
                        mottattBekymringsmelding.meldingstype = "SØKNAD";
                    }
                    break;
                case "UFØ":
                    mottattBekymringsmelding.meldingstype = "UFØDT_BARN";
                    break;
                default:
                    break;
            }
            if (mottattBekymringsmelding.meldingstype == "SØKNAD" && mottattBekymringsmelding.typeMelder.Contains("ANDRE_PRIVATPERSONER"))
            {
                mottattBekymringsmelding.meldingstype = "ORDINÆR_BEKYMRINGSMELDING";
            }
            if (meldingFamilia.MelAvsluttetgjennomgang.HasValue)
            {
                mottattBekymringsmelding.status = "UTFØRT";
            }
            else
            {
                mottattBekymringsmelding.status = "AKTIV";
            }
            if (mottattBekymringsmelding.status != "AKTIV" && !string.IsNullOrEmpty(meldingFamilia.SbhMottattav))
            {
                mottattBekymringsmelding.utfortAvId = GetBrukerId(meldingFamilia.SbhMottattav, bydel);
            }
            if (meldingFamilia.MelAnonymmelder == 1)
            {
                if (mottattBekymringsmelding.mottattBekymringsmeldingsType == "PRIVAT")
                {
                    mottattBekymringsmelding.anonymMelder = true;
                    mottattBekymringsmelding.typeMelder.Add("ANONYM");
                }
            }
            else
            {
                mottattBekymringsmelding.anonymMelder = false;
            }
            return mottattBekymringsmelding;
        }
        private BehandlingAvBekymringsmelding GetBehandlingAvBekymringsmelding(FaMeldinger meldingFamilia, string bydel)
        {
            BehandlingAvBekymringsmelding behandlingAvBekymringsmelding = new()
            {
                behandlingId = AddSpecificBydel(meldingFamilia.MelLoepenr.ToString(), "BEH", bydel),
                pabegyntDato = meldingFamilia.MelPaabegyntdato,
                konklusjonsdato = meldingFamilia.MelAvsluttetgjennomgang,
                vurderingGrunnlagForUndersokelse = meldingFamilia.MelHypotese?.Trim()
            };
            if (!behandlingAvBekymringsmelding.pabegyntDato.HasValue)
            {
                behandlingAvBekymringsmelding.pabegyntDato = meldingFamilia.MelMottattdato;
            }
            if (!string.IsNullOrEmpty(meldingFamilia.MelTidligerekjennskap))
            {
                behandlingAvBekymringsmelding.vurderingTilstrekkeligBelyst = $"Tidligere kjennskap til familien (fra Familia): {meldingFamilia.MelTidligerekjennskap?.Trim()}";
            }
            switch (meldingFamilia.MelKonklusjon?.Trim())
            {
                case "P":
                    behandlingAvBekymringsmelding.konklusjon = "MELDING_I_PÅGÅENDE_UNDERSØKELSE";
                    break;
                case "H":
                    behandlingAvBekymringsmelding.konklusjon = "HENLAGT";
                    break;
                case "T":
                    behandlingAvBekymringsmelding.konklusjon = "HENLAGT_PGA_AKTIVE_TILTAK";
                    break;
                case "U":
                    behandlingAvBekymringsmelding.konklusjon = "TIL_UNDERSØKELSE_(SSB_KODE_2_IKKE_HENLAGT)";
                    break;
                default:
                    break;
            }
            if (meldingFamilia.MelAvsluttetgjennomgang.HasValue)
            {
                behandlingAvBekymringsmelding.status = "UTFØRT";
            }
            else
            {
                behandlingAvBekymringsmelding.status = "AKTIV";
            }
            if (behandlingAvBekymringsmelding.status != "AKTIV" && !string.IsNullOrEmpty(meldingFamilia.SbhInitialer))
            {
                behandlingAvBekymringsmelding.utfortAvId = GetBrukerId(meldingFamilia.SbhInitialer, bydel);
            }
            if (behandlingAvBekymringsmelding.konklusjon == "HENLAGT")
            {
                if (meldingFamilia.MelHenlagtTilAnnenBv == 1)
                {
                    behandlingAvBekymringsmelding.henlagtKodeverk = "OVERFØRT_TIL_ANNEN_KOMMUNE";
                }
                else
                {
                    if (meldingFamilia.MelHenlagtAnnenInstans == 1)
                    {
                        behandlingAvBekymringsmelding.henlagtKodeverk = "VIST_TIL_ANNEN_INSTANS";
                    }
                    else
                    {
                        if (meldingFamilia.MelHenlagtPgaUtenforbvl == 1)
                        {
                            behandlingAvBekymringsmelding.henlagtKodeverk = "UTENFOR_BVL";
                        }
                    }
                }
                if (string.IsNullOrEmpty(behandlingAvBekymringsmelding.henlagtKodeverk))
                {
                    behandlingAvBekymringsmelding.henlagtKodeverk = "UTENFOR_BVL";
                }
            }
            return behandlingAvBekymringsmelding;
        }
        private async Task<TilbakemeldingTilMelder> GetTilbakemeldingTilMelderAsync(FaMeldinger meldingFamilia, string bydel)
        {
            TilbakemeldingTilMelder tilbakemeldingTilMelder = new()
            {
                tilbakemeldingId = AddSpecificBydel(meldingFamilia.MelLoepenr.ToString(), "TBM", bydel),
                notat = meldingFamilia.MelKonklTilmelderbegrunnelse?.Trim(),
            };
            if (meldingFamilia.MelSvarmaatemelder == "ANN" || meldingFamilia.MelSvarmaatemelder == "MØT" || meldingFamilia.MelSvarmaatemelder == "TLF")
            {
                tilbakemeldingTilMelder.status = "UTFØRT";
                tilbakemeldingTilMelder.utfortDato = meldingFamilia.MelAvsluttetgjennomgang;
                if (!string.IsNullOrEmpty(meldingFamilia.SbhInitialer))
                {
                    tilbakemeldingTilMelder.utfortAvId = GetBrukerId(meldingFamilia.SbhInitialer, bydel);
                }
            }
            else
            {
                tilbakemeldingTilMelder.status = "AKTIV";
                if (meldingFamilia.PosSendtkonklAar.HasValue && meldingFamilia.PosSendtkonklLoepenr.HasValue)
                {
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        List<FaPostjournal> postJournals = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosAar == meldingFamilia.PosSendtkonklAar.Value && p.PosLoepenr == meldingFamilia.PosSendtkonklLoepenr.Value && p.PosFerdigdato.HasValue).ToListAsync();
                        if (postJournals.Count > 0)
                        {
                            tilbakemeldingTilMelder.status = "UTFØRT";
                            tilbakemeldingTilMelder.utfortDato = postJournals[0].PosFerdigdato;
                            if (!string.IsNullOrEmpty(postJournals[0].SbhInitialer))
                            {
                                tilbakemeldingTilMelder.utfortAvId = GetBrukerId(postJournals[0].SbhInitialer, bydel);
                            }
                        }
                    }
                }
            }
            if (tilbakemeldingTilMelder.status == "AKTIV" && meldingFamilia.MelMottattdato.AddDays(21) < DateTime.Now)
            {
                tilbakemeldingTilMelder.status = "UTFØRT";
                tilbakemeldingTilMelder.utfortDato = meldingFamilia.MelAvsluttetgjennomgang;
                if (!string.IsNullOrEmpty(tilbakemeldingTilMelder.notat))
                {
                    tilbakemeldingTilMelder.notat += " - Modulus: Automatisk opprettet aktivitet ved migrering";
                }
                else
                {
                    tilbakemeldingTilMelder.notat = "Modulus: Automatisk opprettet aktivitet ved migrering";
                }
            }
            return tilbakemeldingTilMelder;
        }
        private static void GetSaksinnholdMelding(FaMeldinger meldingFamilia, MottattBekymringsmelding mottattBekymringsmelding)
        {
            mottattBekymringsmelding.saksinnhold = new List<string>();
            if (meldingFamilia.MelInnhForeSomatiskSykdom == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_SOMATISKE_SYKDOM");
            }
            if (meldingFamilia.MelInnhForePsykiskProblem == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_PSYKISKE_PROBLEM_LIDELSE");
            }
            if (meldingFamilia.MelInnhForeRusmisbruk == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_RUSMISBRUK");
            }
            if (meldingFamilia.MelInnhForeManglerFerdigh == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_MANGLENDE_FORELDREFERDIGHETER");
            }
            if (meldingFamilia.MelInnhForeKriminalitet == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_KRIMINALITET");
            }
            if (meldingFamilia.MelInnhKonfliktHjemme == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("HØY_GRAD_AV_KONFLIKT_HJEMME");
            }
            if (meldingFamilia.MelInnhVoldHjemme == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("VOLD_I_HJEMMET_BARNET_VITNE_TIL_VOLD_I_NÆRE_RELASJONER");
            }
            if (meldingFamilia.MelInnhBarnSeksuOvergr == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_UTSATT_FOR_SEKSUELLE_OVERGREP");
            }
            if (meldingFamilia.MelInnhBarnNedsFunk == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_HAR_NEDSATT_FUNKSJONSEVNE");
            }
            if (meldingFamilia.MelInnhBarnPsykProb == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_PSYKISKE_PROBLEM_LIDELSE");
            }
            if (meldingFamilia.MelInnhBarnRusmisbruk == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_RUSMISBRUK");
            }
            if (meldingFamilia.MelInnhBarnVansjotsel == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_UTSATT_FOR_VANSKJØTSEL");
            }
            if (meldingFamilia.MelInnhBarnFysiskMish == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_UTSATT_FOR_FYSISK_VOLD");
            }
            if (meldingFamilia.MelInnhBarnPsykiskMish == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_UTSATT_FOR_PSYKISK_VOLD");
            }
            if (meldingFamilia.MelInnhBarnAdferdKrim == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_ATFERD");
            }
            if (meldingFamilia.MelInnhBarnRelasvansker == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_RELASJONSVANSKER_MISTANKE_OM_TILKNYTNINGSVANSKER_PROBLEMATIKK_KNYTTET_TIL_SAMSPILLET_MELLOM_BARN_OG_OMSORGSPERSONER");
            }
            if (meldingFamilia.MelInnhBarnMangOmsorgp == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_MANGLER_OMSORGSPERSON");
            }
            if (meldingFamilia.MelInnhAndreForeFami == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("ANDRE_FORHOLD_VED_FORELDRE_FAMILIEN");
            }
            if (meldingFamilia.MelInnhAndreBarnSitu == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("ANDRE_FORHOLD_VED_BARNETS_SITUASJON");
            }
            if (meldingFamilia.MelInnhAdferd == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_ATFERD");
            }
            if (meldingFamilia.MelInnhAnnet == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("ANDRE_FORHOLD_VED_FORELDRE_FAMILIEN");
                mottattBekymringsmelding.saksinnholdPresiseringKode18 = "Familia: Gammel kode 2.Annet";
            }
            if (meldingFamilia.MelInnhForeManglBeskyt == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_MANGLENDE_BESKYTTELSE_AV_BARNET");
            }
            if (meldingFamilia.MelInnhForeManglStimu == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_MANGLENDE_STIMULERING_OG_REGULERING_AV_BARNET");
            }
            if (meldingFamilia.MelInnhForeTilgjeng == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_MANGLENDE_SENSITIVITET_OG_FØLELSESMESSIGE_TILGJENGELIGHET_FOR_BARNET");
            }
            if (meldingFamilia.MelInnhForeOppfoelgBehov == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_MANGLENDE_OPPFØLGING_AV_BARNETS_BEHOV_FOR_BARNEHAGE_OG_SKOLE_OG_PEDAGOGISKE_TJENESTER");
            }
            if (meldingFamilia.MelInnhBarnForeKonflikt == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("KONFLIKT_MELLOM_FORELDRE_SOM_IKKE_BOR_SAMMEN");
            }
            if (meldingFamilia.MelInnhBarnAdferd == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_ATFERD");
            }
            if (meldingFamilia.MelInnhBarnKrim == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_KRIMINELLE_HANDLINGER");
            }
            if (meldingFamilia.MelInnhBarnMennHandel == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNET_UTSATT_FOR_MENNESKEHANDEL");
            }
            if (meldingFamilia.MelInnhOmsorg == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("ANDRE_FORHOLD_VED_FORELDRE_FAMILIEN");
                mottattBekymringsmelding.saksinnholdPresiseringKode18 = "Familia: Gammel kode 4.Omsorg";
            }
            if (meldingFamilia.MelInnhForhold == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("ANDRE_FORHOLD_VED_FORELDRE_FAMILIEN");
                mottattBekymringsmelding.saksinnholdPresiseringKode18 = "Familia: Gammel kode 3.Forhold";
            }
            if (meldingFamilia.MelInnhPunkt28 == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_MANGLENDE_OPPFØLGING_AV_BARNETS_BEHOV_FOR_HELSETJENESTER");
            }
            if (meldingFamilia.MelInnhPunkt29 == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRES_KOGNITIVE_VANSKER");
            }
            if (meldingFamilia.MelInnhPunkt30 == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("BARNETS_KOGNITIVE_VANSKER");
            }
            if (meldingFamilia.MelInnhPunkt31 == 1)
            {
                mottattBekymringsmelding.saksinnhold.Add("FORELDRE_HAR_VEDVARENDE_ØKONOMISKE_PROBLEMER_VEDVARENDE_LAVINNTEKT");
            }
            if (mottattBekymringsmelding.saksinnhold.Count == 0)
            {
                mottattBekymringsmelding.saksinnhold.Add("ANDRE_FORHOLD_VED_FORELDRE_FAMILIEN");
                mottattBekymringsmelding.saksinnholdPresiseringKode18 = "Familia: Mangler kategori meldingsinnhold";
            }
            if (!string.IsNullOrEmpty(meldingFamilia.MelInnhPresFamilie))
            {
                if (string.IsNullOrEmpty(mottattBekymringsmelding.saksinnholdPresiseringKode18))
                {
                    mottattBekymringsmelding.saksinnholdPresiseringKode18 = meldingFamilia.MelInnhPresFamilie?.Trim();
                }
                else
                {
                    mottattBekymringsmelding.saksinnholdPresiseringKode18 += " - " + meldingFamilia.MelInnhPresFamilie?.Trim();
                }
            }
            if (!string.IsNullOrEmpty(meldingFamilia.MelInnhPresBarnet))
            {
                mottattBekymringsmelding.saksinnholdPresiseringKode19 = meldingFamilia.MelInnhPresBarnet?.Trim();
            }
            if (mottattBekymringsmelding.saksinnhold.Count > 0)
            {
                mottattBekymringsmelding.saksinnhold = mottattBekymringsmelding.saksinnhold.Distinct().ToList();
            }
        }
        private static void GetTypeMelder(FaMeldinger meldingFamilia, MottattBekymringsmelding mottattBekymringsmelding)
        {
            mottattBekymringsmelding.typeMelder = new List<string>();

            if (meldingFamilia.MelMeldtSosialkt == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("NAV_KOMMUNE_OG_STAT");
            }
            if (meldingFamilia.MelMeldtBarnevt == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("BARNEVERNSTJENESTE");
            }
            if (meldingFamilia.MelMeldtBarnevvakt == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("BARNEVERNSVAKT");
            }
            if (meldingFamilia.MelMeldtBarnehage == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("BARNEHAGE");
            }
            if (meldingFamilia.MelMeldtHelsestasjon == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("HELSESTASJON_SKOLEHELSETJENESTEN");
            }
            if (meldingFamilia.MelMeldtLege == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("LEGE_SYKEHUS_TANNLEGE");
            }
            if (meldingFamilia.MelMeldtSkole == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("SKOLE");
            }
            if (meldingFamilia.MelMeldtPedPpt == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("PEDAGOGISK_PSYKOLOGISK_TJENESTE_PPT");
            }
            if (meldingFamilia.MelMeldtPoliti == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("POLITI_LENSMANN");
            }
            if (meldingFamilia.MelMeldtBup == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("PSYKISK_HELSEVERN_FOR_BARN_OG_UNGE_KOMMUNE_OG_STAT");
            }
            if (meldingFamilia.MelMeldtOffentlig == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("ANDRE_OFFENTLIG_INSTANSER");
            }
            if (meldingFamilia.MelMeldtPsykiskHelseBarn == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("PSYKISK_HELSEVERN_FOR_BARN_OG_UNGE_KOMMUNE_OG_STAT");
            }
            if (meldingFamilia.MelMeldtPsykiskHelseVoksne == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("PSYKISK_HELSEVERN_FOR_VOKSNE_KOMMUNE_OG_STAT");
            }
            if (meldingFamilia.MelMeldtUdi == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("ASYLMOTTAK_UDI_INNVANDRINGSMYNDIGHET");
            }
            if (meldingFamilia.MelMeldtKrisesenter == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("KRISESENTER");
            }
            if (meldingFamilia.MelMeldtFrivillige == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("FRIVILLIGE_ORGANISASJONER_IDRETTSLAG");
            }
            if (meldingFamilia.MelMeldtUtekontakt == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("UTEKONTAKT_FRITIDSKLUBB");
            }
            if (meldingFamilia.MelMeldtFamilievernkontor == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("FAMILIEVERNKONTOR");
            }
            if (meldingFamilia.MelMeldtTjenesteInstans == 1)
            {
                mottattBekymringsmelding.typeMelder.Add("TJENESTER_OG_INSTANSER_MED_ANSVAR_FOR_OPPFØLGING_AV_PERSONERS_RUSPROBLEMER_KOMMUNE_OG_STAT");
            }
            if (mottattBekymringsmelding.typeMelder.Count == 0)
            {
                if (meldingFamilia.MelMeldtBarnet == 1)
                {
                    mottattBekymringsmelding.typeMelder.Add("BARNET_SELV");
                }
                if (meldingFamilia.MelMeldtForeldre == 1)
                {
                    mottattBekymringsmelding.typeMelder.Add("MOR_FAR_FORESATTE");
                }
                if (meldingFamilia.MelMeldtFamilie == 1)
                {
                    if (meldingFamilia.MelMeldingstype?.Trim() == "SØK")
                    {
                        mottattBekymringsmelding.typeMelder.Add("MOR_FAR_FORESATTE");
                    }
                    else
                    {
                        mottattBekymringsmelding.typeMelder.Add("FAMILIE_FOR_ØVRIGT");
                    }
                }
                if (meldingFamilia.MelMeldtNaboer == 1)
                {
                    mottattBekymringsmelding.typeMelder.Add("ANDRE_PRIVATPERSONER");
                }
                if (meldingFamilia.MelMeldtAndre == 1)
                {
                    mottattBekymringsmelding.typeMelder.Add("ANDRE_PRIVATPERSONER");
                }
                if (mottattBekymringsmelding.typeMelder.Count > 0)
                {
                    mottattBekymringsmelding.mottattBekymringsmeldingsType = "PRIVAT";
                }
                else
                {
                    mottattBekymringsmelding.typeMelder.Add("ANDRE_OFFENTLIG_INSTANSER");
                    mottattBekymringsmelding.typeMelderPresisering = "Familia: Type melder ikke oppgitt";
                }
            }
            if (mottattBekymringsmelding.typeMelder.Count > 0)
            {
                mottattBekymringsmelding.typeMelder = mottattBekymringsmelding.typeMelder.Distinct().ToList();
            }
        }
        private void GetGrunnlagForTiltak(FaUndersoekelser undersøkelse, Undersøkelse undersoekelse, bool henlegges)
        {
            if (!henlegges)
            {
                undersoekelse.grunnlagForTiltak = new List<string>();
                if (undersøkelse.UndInnhForeSomatiskSykdom == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_SOMATISKE_SYKDOM");
                }
                if (undersøkelse.UndInnhForePsykiskProblem == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_PSYKISKE_PROBLEM_LIDELSE");
                }
                if (undersøkelse.UndInnhForeRusmisbruk == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_RUSMISBRUK");
                }
                if (undersøkelse.UndInnhForeManglerFerdigh == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_MANGLENDE_FORELDREFERDIGHETER");
                }
                if (undersøkelse.UndInnhForeKriminalitet == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_KRIMINALITET");
                }
                if (undersøkelse.UndInnhKonfliktHjemme == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("HØY_GRAD_AV_KONFLIKT_HJEMME");
                }
                if (undersøkelse.UndInnhVoldHjemme == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("VOLD_I_HJEMMET_BARNET_VITNE_TIL_VOLD_I_NÆRE_RELASJONER");
                }
                if (undersøkelse.UndInnhBarnSeksuOvergr == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_UTSATT_FOR_SEKSUELLE_OVERGREP");
                }
                if (undersøkelse.UndInnhBarnNedsFunk == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_HAR_NEDSATT_FUNKSJONSEVNE");
                }
                if (undersøkelse.UndInnhBarnPsykProb == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_PSYKISKE_PROBLEM_LIDELSE");
                }
                if (undersøkelse.UndInnhBarnRusmisbruk == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_RUSMISBRUK");
                }
                if (undersøkelse.UndInnhBarnVansjotsel == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_UTSATT_FOR_VANSKJØTSEL");
                }
                if (undersøkelse.UndInnhBarnFysiskMish == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_UTSATT_FOR_FYSISK_VOLD");
                }
                if (undersøkelse.UndInnhBarnPsykiskMish == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_UTSATT_FOR_PSYKISK_VOLD");
                }
                if (undersøkelse.UndInnhBarnAdferdKrim == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_ATFERD_KRIMINALITET");
                }
                if (undersøkelse.UndInnhBarnRelasvansker == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_RELASJONSVANSKER_MISTANKE_OM_TILKNYTNINGSVANSKER_PROBLEMATIKK_KNYTTET_TIL_SAMSPILLET_MELLOM_BARN_OG_OMSORGSPERSONER");
                }
                if (undersøkelse.UndInnhBarnMangOmsorgp == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_MANGLER_OMSORGSPERSON");
                }
                if (undersøkelse.UndInnhAndreForeFami == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("ANDRE_FORHOLD_VED_FORELDRE_FAMILIEN");
                }
                if (undersøkelse.UndInnhAndreBarnSitu == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("ANDRE_FORHOLD_VED_BARNETS_SITUASJON");
                }
                if (undersøkelse.UndInnhForeManglBeskyt == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_MANGLENDE_BESKYTTELSE_AV_BARNET");
                }
                if (undersøkelse.UndInnhForeManglStimu == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_MANGLENDE_STIMULERING_OG_REGULERING_AV_BARNET");
                }
                if (undersøkelse.UndInnhForeTilgjeng == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_MANGLENDE_SENSITIVITET_OG_FØLELSESMESSIGE_TILGJENGELIGHET_FOR_BARNET");
                }
                if (undersøkelse.UndInnhForeOppfoelgBehov == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_MANGLENDE_OPPFØLGING_AV_BARNETS_BEHOV_FOR_BARNEHAGE_OG_SKOLE_OG_PEDAGOGISKE_TJENESTER");
                }
                if (undersøkelse.UndInnhBarnForeKonflikt == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("KONFLIKT_MELLOM_FORELDRE_SOM_IKKE_BOR_SAMMEN");
                }
                if (undersøkelse.UndInnhBarnAdferd == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_ATFERD");
                }
                if (undersøkelse.UndInnhBarnKrim == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_KRIMINELLE_HANDLINGER");
                }
                if (undersøkelse.UndInnhBarnMennHandel == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNET_UTSATT_FOR_MENNESKEHANDEL");
                }
                if (undersøkelse.UndInnhPunkt28 == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_MANGLENDE_OPPFØLGING_AV_BARNETS_BEHOV_FOR_HELSETJENESTER");
                }
                if (undersøkelse.UndInnhPunkt29 == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRES_KOGNITIVE_VANSKER");
                }
                if (undersøkelse.UndInnhPunkt30 == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("BARNETS_KOGNITIVE_VANSKER");
                }
                if (undersøkelse.UndInnhPunkt31 == 1)
                {
                    undersoekelse.grunnlagForTiltak.Add("FORELDRE_HAR_VEDVARENDE_ØKONOMISKE_PROBLEMER_VEDVARENDE_LAVINNTEKT");
                }
                if (!string.IsNullOrEmpty(undersøkelse.UndInnhPresFamilie))
                {
                    undersoekelse.grunnlagForTiltakPresiseringKode18 = undersøkelse.UndInnhPresFamilie?.Trim();
                }
                if (!string.IsNullOrEmpty(undersøkelse.UndInnhPresBarnet))
                {
                    undersoekelse.grunnlagForTiltakPresiseringKode19 = undersøkelse.UndInnhPresBarnet?.Trim();
                }
                if (undersoekelse.grunnlagForTiltak.Count == 0)
                {
                    if ((undersoekelse.konklusjon == "BARNEVERNSTJENESTEN_GJØR_VEDTAK_OM_TILTAK") || (undersøkelse.UndFerdigdato.HasValue && undersøkelse.UndFerdigdato.Value < FirstInYearOfMigration))
                    {
                        undersoekelse.grunnlagForTiltak.Add("MIGRERT_INGEN_KODE");
                    }
                }
            }
        }
        private string GetVedtakstype(FaSaksjournal saksJournal, Vedtak vedtak, string lovHovedParagraf, string lovJmfParagraf1, string lovJmfParagraf2, string mynVedtakstype)
        {
            if (
                (mynVedtakstype == "FN" && (!string.IsNullOrEmpty(lovHovedParagraf) && (lovHovedParagraf.StartsWith("4-6") || lovHovedParagraf.StartsWith("4-25")))) 
                || ((mynVedtakstype == "FN" || mynVedtakstype == "LA" || mynVedtakstype == "TI") && (lovHovedParagraf == "4-6,2." || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-9")) || (!string.IsNullOrEmpty(lovJmfParagraf1) && lovJmfParagraf1.StartsWith("4-9")) || (!string.IsNullOrEmpty(lovJmfParagraf2) && lovJmfParagraf2.StartsWith("4-9"))))
                || lovHovedParagraf == "4-6,3."
                )
            {
                mynVedtakstype = "AV";
            }
            if ((saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR")
                && (mynVedtakstype == "FN" || mynVedtakstype == "LA" || mynVedtakstype == "TI"))
            {
                if (lovHovedParagraf == "4-4,3." || lovHovedParagraf == "4-4,3.1" || lovHovedParagraf == "4-4,3.2" || lovHovedParagraf == "4-4,3.3"
                    || (lovHovedParagraf == "4-4,5." && (lovJmfParagraf1 == "4-24" || lovJmfParagraf2 == "4-24"))
                    || (lovHovedParagraf == "4-4,2.") || (lovHovedParagraf == "4-4,4.")
                    || lovHovedParagraf == "§ 3-4" || lovHovedParagraf == "§ 3-5" || (lovHovedParagraf == "§ 3-5" && ((!string.IsNullOrEmpty(lovJmfParagraf1) && lovJmfParagraf1.StartsWith("§ 6-2")) || (!string.IsNullOrEmpty(lovJmfParagraf2) && lovJmfParagraf2.StartsWith("§ 6-2"))))
                    || (lovHovedParagraf == "4-4,6.")
                    || (lovHovedParagraf == "4-4" && string.IsNullOrEmpty(lovJmfParagraf1) && string.IsNullOrEmpty(lovJmfParagraf2))
                    )
                {
                    vedtak.vedtakstype = "PÅLAGT_HJELPETILTAK";
                }
                else if (lovHovedParagraf == "4-11" || lovHovedParagraf == "§ 3-8")
                {
                    vedtak.vedtakstype = "BEHANDLING_AV_BARN_MED_SÆRSKILTE_BEHOV";
                }
                else if (lovHovedParagraf == "4-10" || lovHovedParagraf == "§ 3-7")
                {
                    vedtak.vedtakstype = "MEDISINSK_UNDERSØKELSE_OG_BEHANDLING";
                }
                else if (lovHovedParagraf == "4-8,1." || lovHovedParagraf == "§ 4-3")
                {
                    vedtak.vedtakstype = "FORBUD_MOT_FLYTTING";
                }
                else if (lovHovedParagraf == "4-8,2." || lovHovedParagraf == "4-8,3." || lovHovedParagraf == "7-3" || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-12"))
                    || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 5-1")) || lovHovedParagraf == "§ 4-4"
                    || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 4-2"))
                    || (!string.IsNullOrEmpty(lovHovedParagraf) && (lovHovedParagraf.StartsWith("4-8") && (lovJmfParagraf1 == "4-12" || lovJmfParagraf2 == "4-12")))
                    || (lovHovedParagraf == "4-8" && (string.IsNullOrEmpty(lovJmfParagraf1) && string.IsNullOrEmpty(lovJmfParagraf2)))
                    || (lovHovedParagraf == "4-8" && lovJmfParagraf1 == "4-12,1.a")
                    || (lovHovedParagraf == "4-8" && (lovJmfParagraf1 == "4-19" || lovJmfParagraf2 == "4-19"))
                    || (lovHovedParagraf == "4-8" && lovJmfParagraf1 == "4-4,5.")
                    || (lovHovedParagraf == "4-14")
                    || (lovHovedParagraf == "4-17")
                    || (lovHovedParagraf == "4-4,1.")
                    || (lovHovedParagraf == "4-12" && string.IsNullOrEmpty(lovJmfParagraf1) && string.IsNullOrEmpty(lovJmfParagraf2))
                    || (lovHovedParagraf == "4-3" && lovJmfParagraf1 == "4-12")
                    )
                {
                    vedtak.vedtakstype = "OMSORGSOVERTAKELSE";
                }
                else if ((!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-24")) || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 6-2")) || lovHovedParagraf == "§ 8-4")
                {
                    vedtak.vedtakstype = "ADFERDSTILTAK";
                }
                else if ((!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-20")) 
                    || lovHovedParagraf == "§ 5-8"
                    || lovHovedParagraf == "5-8"
                    || lovHovedParagraf == "§ 5-10" 
                    || lovHovedParagraf == "§ 5-11")
                {
                    vedtak.vedtakstype = "FRATAKELSE_AV_FORELDREANSVAR._ADOPSJON";
                }
                else if ((!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-19")) 
                    || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 7-2")) 
                    || lovHovedParagraf == "§ 7-1"
                    || lovHovedParagraf == "7-13" 
                    || lovHovedParagraf == "7-24" 
                    || lovHovedParagraf == "4-22"
                    || lovHovedParagraf == "4-19,2."
                    )
                {
                    vedtak.vedtakstype = "SAMVÆRSRETT/SKJULT_ADRESSE";
                }
                else if ((!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-21")) 
                    || lovHovedParagraf == "§ 5-7"
                    || lovHovedParagraf == "5-7"
                    || (lovHovedParagraf == "4-4" && lovJmfParagraf1 == "4-21"))
                {
                    vedtak.vedtakstype = "OPPHEVING_AV_VEDTAK_OM_OMSORGSOVERTAGELSE";
                }
                else if (lovHovedParagraf == "4-29" || lovHovedParagraf == "4-29,2." || lovHovedParagraf == "4-29,3." || lovHovedParagraf == "§ 6-6" || lovHovedParagraf == "§ 4-5")
                {
                    vedtak.vedtakstype = "MENNESKEHANDEL";
                }
                if (string.IsNullOrEmpty(vedtak.behandlingIFylkesnemda))
                {
                    vedtak.behandlingIFylkesnemda = "FULLTALLIG";
                }
            }
            if (!string.IsNullOrEmpty(saksJournal.LovHovedParagraf))
            {
                vedtak.lovhjemmel = mappings.GetModulusLovhjemmel(saksJournal.LovHovedParagraf, saksJournal.SakIverksattdato, saksJournal.SakSlutningdato);
            }
            if (!string.IsNullOrEmpty(saksJournal.LovJmfParagraf1))
            {
                if (!string.IsNullOrEmpty(vedtak.lovhjemmel))
                {
                    vedtak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf1, saksJournal.SakIverksattdato, saksJournal.SakSlutningdato);
                }
                else
                {
                    vedtak.lovhjemmel = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf1, saksJournal.SakIverksattdato, saksJournal.SakSlutningdato);
                }
            }
            if (!string.IsNullOrEmpty(saksJournal.LovJmfParagraf2))
            {
                if (!string.IsNullOrEmpty(vedtak.jfLovhjemmelNr1))
                {
                    vedtak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf2, saksJournal.SakIverksattdato, saksJournal.SakSlutningdato);
                }
                else
                {
                    vedtak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf2, saksJournal.SakIverksattdato, saksJournal.SakSlutningdato);
                }
            }
            if (vedtak.jfLovhjemmelNr2 == vedtak.lovhjemmel)
            {
                vedtak.jfLovhjemmelNr2 = null;
            }
            if (vedtak.jfLovhjemmelNr1 == vedtak.lovhjemmel)
            {
                vedtak.jfLovhjemmelNr1 = vedtak.jfLovhjemmelNr2;
                vedtak.jfLovhjemmelNr2 = null;
            }
            if (vedtak.jfLovhjemmelNr1 == vedtak.jfLovhjemmelNr2)
            {
                vedtak.jfLovhjemmelNr2 = null;
            }
            return mynVedtakstype;
        }
        private async Task<string> GetTilsynsKommunenummerAsync(Decimal kliLoepenr, DateTime? datoHendelse, bool brukOslo = true)
        {
            string kommunenr = "";
            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                if (datoHendelse.HasValue)
                {
                    List<FaKlientplassering> klientplasseringer = await context.FaKlientplasserings
                    .Where(m => m.KliLoepenr == kliLoepenr && m.KplBorhos == "6").ToListAsync();
                    FaKlientplassering klientplassering = klientplasseringer.OrderBy(o => Math.Abs((o.KplFradato - datoHendelse.Value).TotalDays)).FirstOrDefault();
                    if (klientplassering != null)
                    {
                        if (klientplassering.KomKommunenr < 1000)
                        {
                            kommunenr = $"0{klientplassering.KomKommunenr}";
                        }
                        else
                        {
                            kommunenr = klientplassering.KomKommunenr.ToString();
                        }
                        if (!mappings.IsValidKommunenummer(kommunenr))
                        {
                            kommunenr = "0301";
                        }
                    }
                    else
                    {
                        kommunenr = "0301";
                    }
                }
                else
                {
                    kommunenr = "0301";
                }
            }
            if (!brukOslo && kommunenr == "0301")
            {
                if (datoHendelse.HasValue && datoHendelse.Value.Year < 2023)
                {
                    kommunenr = null;
                }
            }
            return kommunenr;
        }
        private async Task<string> GetTilsynsKommunenummerSpecificBydelAsync(Decimal kliLoepenr, string bydel, DateTime? datoHendelse, bool brukOslo = true)
        {
            string kommunenr = "";
            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                if (datoHendelse.HasValue)
                {
                    List<FaKlientplassering> klientplasseringer = await context.FaKlientplasserings
                        .Where(m => m.KliLoepenr == kliLoepenr && m.KplBorhos == "6").ToListAsync();
                    FaKlientplassering klientplassering = klientplasseringer.OrderBy(o => Math.Abs((o.KplFradato - datoHendelse.Value).TotalDays)).FirstOrDefault();
                    if (klientplassering != null)
                    {
                        if (klientplassering.KomKommunenr < 1000)
                        {
                            kommunenr = $"0{klientplassering.KomKommunenr}";
                        }
                        else
                        {
                            kommunenr = klientplassering.KomKommunenr.ToString();
                        }
                        if (!mappings.IsValidKommunenummer(kommunenr))
                        {
                            kommunenr = "0301";
                        }
                    }
                    else
                    {
                        kommunenr = "0301";
                    }
                }
                else
                {
                    kommunenr = "0301";
                }
            }
            if (!brukOslo && kommunenr == "0301")
            {
                if (datoHendelse.HasValue && datoHendelse.Value.Year < 2023)
                {
                    kommunenr = null;
                }
            }
            return kommunenr;
        }
        private async Task<FaPostjournal> GetPostJournalAsync(decimal? posAar, decimal? posLoepenr)
        {
            FaPostjournal postJournal;
            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                postJournal = await context.FaPostjournals.Where(d => d.PosSlettet != 1 && d.PosAar == posAar && d.PosLoepenr == posLoepenr && d.PosFerdigdato.HasValue).SingleOrDefaultAsync();
            }
            return postJournal;
        }
        private async Task<FaPostjournal> GetPostJournalSpecificBydelAsync(decimal? posAar, decimal? posLoepenr, string bydel)
        {
            FaPostjournal postJournal;
            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                postJournal = await context.FaPostjournals.Where(d => d.PosSlettet != 1 && d.PosAar == posAar && d.PosLoepenr == posLoepenr && d.PosFerdigdato.HasValue).SingleOrDefaultAsync();
            }
            return postJournal;
        }
        private async Task WriteFileAsync(object list, string fileName)
        {
            try
            {
                if (!OnlyWriteDocumentFiles)
                {
                    var options = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
                    };
                    var jsonData = JsonSerializer.Serialize(list, options);
                    if (jsonData != "[]")
                    {
                        await File.WriteAllTextAsync(OutputFolderName + fileName, jsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public string GetJsonFileName(string folderName, string fileName)
        {
            return $"{folderName}\\{Bydelsforkortelse}_{DateTime.Now:yyyyMMdd_HHmm_}{fileName}.json";
        }
        private string GetSakId(string name)
        {
            string sakId;
            if (string.IsNullOrEmpty(Bydelsforkortelse))
            {
                sakId = name;
            }
            else
            {
                sakId = $"{name}__{Bydelsforkortelse}";
            }
            return $"{sakId}__SAK";
        }
        private string AddBydel(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim().ToUpper();
            }
            if (string.IsNullOrEmpty(Bydelsforkortelse))
            {
                return name;
            }
            else
            {
                return $"{name}__{Bydelsforkortelse}";
            }
        }
        private string AddBydel(string name, string postfix)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim().ToUpper();
            }
            if (string.IsNullOrEmpty(Bydelsforkortelse))
            {
                return $"{name}__{postfix}";
            }
            else
            {
                return $"{name}__{Bydelsforkortelse}__{postfix}";
            }
        }
        private static string AddSpecificBydel(string name, string bydel)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim().ToUpper();
            }
            if (string.IsNullOrEmpty(bydel))
            {
                return name;
            }
            else
            {
                return $"{name}__{bydel}";
            }
        }
        private static string AddSpecificBydel(string name, string postfix, string bydel)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim().ToUpper();
            }
            if (string.IsNullOrEmpty(bydel))
            {
                return $"{name}__{postfix}";
            }
            else
            {
                return $"{name}__{bydel}__{postfix}";
            }
        }
        private string GetEnhetskode(string name)
        {
            if (string.IsNullOrEmpty(Bydelsforkortelse))
            {
                return name.Trim();
            }
            else
            {
                return $"{Bydelsforkortelse}{name.Trim()}";
            }
        }
        private static string GetEnhetskode(string name, string bydel)
        {
            if (string.IsNullOrEmpty(bydel))
            {
                return name.Trim();
            }
            else
            {
                return $"{bydel}{name.Trim()}";
            }
        }
        private static string GetModifisertPRKId(int prkId)
        {
            string modifisertPRKId = prkId.ToString();

            if (modifisertPRKId.Length == 4)
            {
                modifisertPRKId = "90" + modifisertPRKId;
            }
            else if (modifisertPRKId.Length == 5)
            {
                modifisertPRKId = "9" + modifisertPRKId;
            }
            return modifisertPRKId;
        }
        private static string AddTextToStringIfNotEmpty(string original, string stringToAdd, string prefix)
        {
            string newString;
            if (string.IsNullOrEmpty(stringToAdd))
            {
                newString = original;
            }
            else
            {
                newString = original + prefix + stringToAdd;
            }
            return newString;
        }
        private void CreateFolderIfNotExist(string folderName)
        {
            System.IO.Directory.CreateDirectory($"{OutputFolderName}{folderName}");
        }
        private static bool IsDigitsOnly(string stringToTest)
        {
            foreach (char c in stringToTest)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }
        private static string GetInfoTabellerMSSQL(string connectionString)
        {
            string information = "";
            SqlConnection connectionTables = new(connectionString);
            SqlDataReader readerTables;
            try
            {
                connectionTables.Open();
                List<string> tableNames = new();
                SqlCommand command = new($"SELECT table_name FROM information_schema.tables", connectionTables)
                {
                    CommandTimeout = 300
                };
                readerTables = command.ExecuteReader();
                while (readerTables.Read())
                {
                    tableNames.Add(readerTables.GetString(0));
                }
                if (tableNames != null)
                {
                    int totalTables = tableNames.Count;
                    int totalRows = 0;
                    information += "TABELLER I DATABASEN:" + Environment.NewLine + Environment.NewLine;
                    tableNames.Sort();
                    foreach (var tableName in tableNames)
                    {
                        command = new SqlCommand($"SELECT count(*) FROM {tableName}", connectionTables)
                        {
                            CommandTimeout = 300
                        };
                        if (readerTables != null && !readerTables.IsClosed)
                        {
                            readerTables.Close();
                        }
                        readerTables = command.ExecuteReader();
                        while (readerTables.Read())
                        {
                            int rows = readerTables.GetInt32(0);
                            information += $"{rows,10} rader: {tableName}" + Environment.NewLine;
                            totalRows += rows;
                        }
                    }
                    information += Environment.NewLine + $"{totalTables} TABELLER MED TILSAMMEN {totalRows} RADER";
                }
            }
            finally
            {
                connectionTables.Close();
            }
            return information;
        }
        private static string GetInfoTabellerOracle(string connectionString, string schema)
        {
            string information = "";

            OracleConnection connection = new(connectionString);
            OracleDataReader reader = null;
            try
            {
                connection.Open();
                List<string> tableNames = [];
                OracleCommand command = new($"SELECT owner, table_name FROM all_tables Where owner = '{schema}'", connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tableNames.Add(reader.GetString(1));
                }
                if (tableNames != null)
                {
                    int totalTables = tableNames.Count;
                    int totalRows = 0;
                    information += "TABELLER I DATABASEN:" + Environment.NewLine + Environment.NewLine;
                    tableNames.Sort();
                    foreach (var tableName in tableNames)
                    {
                        command = new OracleCommand($"SELECT count(*) FROM {schema}.{tableName}", connection)
                        {
                            CommandType = System.Data.CommandType.Text
                        };
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int rows = reader.GetInt32(0);
                            information += $"{rows,10} rader: {tableName}" + Environment.NewLine;
                            totalRows += rows;
                        }
                    }
                    information += Environment.NewLine + $"{totalTables} TABELLER MED TILSAMMEN {totalRows} RADER";
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return information;
        }
        private async Task<string> GetActorIdAsync(int personId)
        {
            string actorId = "";
            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                PersonPerson person = await context.PersonPeople.Where(p => p.PersonPersonId == personId).FirstOrDefaultAsync();
                actorId = GetActorId(person);
            }
            return actorId;
        }
        private string GetActorId(FaForbindelser forbindelse, bool organisasjon)
        {
            if (organisasjon)
            {
                return GetUnikActorId(GetOversattOrganisasjonsnr(forbindelse.ForOrganisasjonsnr, forbindelse.ForLoepenr), forbindelse.ForFoedselsnummer, forbindelse.ForFornavn, forbindelse.ForEtternavn);
            }
            else
            {
                return GetUnikActorId(null, forbindelse.ForFoedselsnummer, forbindelse.ForFornavn, forbindelse.ForEtternavn);
            }
        }
        private string GetActorId(FaKlient klient)
        {
            if (klient.KliFoedselsdato.HasValue)
            {
                return GetUnikActorId(null, klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr, klient.KliFornavn, klient.KliEtternavn);
            }
            else
            {
                if (klient.KliPersonnr.HasValue)
                {
                    return GetUnikActorId(null, klient.KliPersonnr.Value.ToString(), klient.KliFornavn, klient.KliEtternavn);
                }
                else
                {
                    return GetUnikActorId(null, null, klient.KliFornavn, klient.KliEtternavn, klient.KliLoepenr.ToString());
                }
            }
        }
        private string GetActorId(PersonPerson person)
        {
            if (!string.IsNullOrEmpty(person.BirthNumber))
            {
                return GetUnikActorId(null, person.BirthNumber, person.FirstName, person.LastName);
            }
            else
            {
                return $"{person.PersonPersonId}__BVV__INN";
            }
        }
        private string GetUnikActorId(string organisasjonsnummer, string fodselsNummer, string forNavn, string etterNavn, string klientId="")
        {
            string actorId;

            if (!string.IsNullOrEmpty(organisasjonsnummer))
            {
                actorId = organisasjonsnummer;
            }
            else
            {
                string basisId;
                if (!string.IsNullOrEmpty(fodselsNummer))
                {
                    basisId = fodselsNummer;
                }
                else
                {
                    basisId = forNavn + etterNavn + klientId + Bydelsforkortelse;
                }
                actorId = GetStringFromByteArray(basisId);
            }
            return actorId;
        }
        private string GetOversattOrganisasjonsnr(string orgnr, decimal forbindelsesId)
        {
            string oversattOrgnr = orgnr?.Trim();

            SqlConnection connectionMigrering = new(ConnectionStringMigrering);
            SqlDataReader readerMigrering;
            try
            {
                connectionMigrering.Open();
                SqlCommand commandMigrering = new($"Select Orgnr From Organisasjonsnummer Where ForbindelsesId='{forbindelsesId}' And Bydel='{Bydelsforkortelse}'", connectionMigrering)
                {
                    CommandTimeout = 300
                };
                readerMigrering = commandMigrering.ExecuteReader();
                while (readerMigrering.Read())
                {
                    if (!string.IsNullOrEmpty(readerMigrering.GetString(0)))
                    {
                        oversattOrgnr = readerMigrering.GetString(0);
                    }
                }
                readerMigrering.Close();
            }
            finally
            {
                connectionMigrering.Close();
            }
            return oversattOrgnr;
        }
        private string GetBrukerId(string initialer, string bydel = "")
        {
            if (string.IsNullOrEmpty(initialer))
            {
                return null;
            }
            string brukerId = "";
            if (string.IsNullOrEmpty(bydel))
            {
                bydel = Bydelsforkortelse;
            }
            SqlConnection connection = new(ConnectionStringMigrering);
            SqlDataReader reader;
            try
            {
                connection.Open();
                SqlCommand command = new($"Select Epost From Brukere Where Upper(Virksomhet)='{bydel}' And Upper(FamiliaID)='{initialer.ToUpper()}'", connection)
                {
                    CommandTimeout = 300
                };
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        brukerId = reader.GetString(0);
                    }
                }
                else
                {
                    brukerId = AddSpecificBydel(initialer, bydel);
                }
                reader.Close();
            }
            finally
            {
                connection.Close();
            }
            return brukerId;
        }
        private static bool IsValidMod11(string number)
        {
            string cleanNumber = new(number.Where(char.IsDigit).ToArray());

            if (cleanNumber.Length < 2)
            {
                return false;
            }

            int sum = 0;
            int factor = 2;

            for (int i = cleanNumber.Length - 2; i >= 0; i--)
            {
                int digit = int.Parse(cleanNumber[i].ToString());
                sum += digit * factor;

                if (++factor > 7)
                {
                    factor = 2;
                }
            }

            int remainder = sum % 11;
            int calculatedCheckDigit = remainder == 0 ? 0 : 11 - remainder;

            int actualCheckDigit = int.Parse(cleanNumber[cleanNumber.Length - 1].ToString());

            return calculatedCheckDigit == actualCheckDigit;
        }
        private static string GetStringFromByteArray(string basis)
        {
            return ByteArrayToString(System.Security.Cryptography.MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(basis)));
        }
        private static string GetPostJounalMotpart(bool incoming, string notat, string posFornavn, string posEtternavn)
        {
            string fullText = notat;
            if (!string.IsNullOrEmpty(posFornavn) || !string.IsNullOrEmpty(posEtternavn))
            {
                string name;

                if (!string.IsNullOrEmpty(posFornavn))
                {
                    name = posFornavn.Trim();
                    if (!string.IsNullOrEmpty(posEtternavn))
                    {
                        name += $" {posEtternavn.Trim()}";
                    }
                }
                else
                {
                    name = posEtternavn.Trim();
                }
                if (!string.IsNullOrEmpty(fullText))
                {
                    if (incoming)
                    {
                        fullText += $" fra {name}";
                    }
                    else
                    {
                        fullText += $" til {name}";
                    }
                }
                else
                {
                    if (incoming)
                    {
                        fullText = $"Fra {name}";
                    }
                    else
                    {
                        fullText = $"Til {name}";
                    }
                }
            }
            return fullText;
        }
        private static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder sOutput = new(arrInput.Length);
            for (int index = 0; index < arrInput.Length - 1; index++)
            {
                sOutput.Append(arrInput[index].ToString("X2"));
            }
            return sOutput.ToString();
        }
        private static string GetTelefonnummer(string number)
        {
            string formattedNumber = number;
            if (formattedNumber != null)
            {
                formattedNumber = formattedNumber.Replace(" ", "");
                formattedNumber = formattedNumber.Replace("-", "");
            }
            return formattedNumber;
        }
        #endregion

        #region Filter
        private Expression<Func<FaKlient, bool>> KlientFilter()
        {
            if (OnlyActiveCases)
            {
                return k => k.KliFoedselsdato.HasValue && !k.KliAvsluttetdato.HasValue;
            }
            else if (OnlyPassiveCases)
            {
                return k => k.KliFoedselsdato.HasValue && k.KliFoedselsdato > LastDateNoMigration && k.KliAvsluttetdato.HasValue;
            }
            else
            {
                return k => k.KliFoedselsdato.HasValue && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue);
            }
        }
        private Expression<Func<FaKlient, bool>> KlientKunGyldigeTilsyn()
        {
            return p => !(p.KliFraannenkommune == 1 && (p.KliAvsluttetdato.HasValue || (p.KliFoedselsdato.HasValue && p.KliFoedselsdato <= FromDateMigrationTilsyn)));
        }
        private Expression<Func<FaPostjournal, bool>> KlientPostJournalFilter()
        {
            if (OnlyActiveCases)
            {
                return p => p.KliLoepenrNavigation.KliFoedselsdato.HasValue && !p.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(p.KliLoepenrNavigation.KliFraannenkommune == 1 && (p.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (p.KliLoepenrNavigation.KliFoedselsdato.HasValue && p.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else if (OnlyPassiveCases)
            {
                return p => p.KliLoepenrNavigation.KliFoedselsdato.HasValue && p.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && p.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(p.KliLoepenrNavigation.KliFraannenkommune == 1 && (p.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (p.KliLoepenrNavigation.KliFoedselsdato.HasValue && p.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else
            {
                return p => p.KliLoepenrNavigation.KliFoedselsdato.HasValue && (p.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !p.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && !(p.KliLoepenrNavigation.KliFraannenkommune == 1 && (p.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (p.KliLoepenrNavigation.KliFoedselsdato.HasValue && p.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
        }
        private Expression<Func<FaSaksjournal, bool>> KlientSakJournalFilter()
        {
            if (OnlyActiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(m.KliLoepenrNavigation.KliFraannenkommune == 1 && (m.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else if (OnlyPassiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(m.KliLoepenrNavigation.KliFraannenkommune == 1 && (m.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && !(m.KliLoepenrNavigation.KliFraannenkommune == 1 && (m.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
        }
        private Expression<Func<FaKlienttilknytning, bool>> KlientTilknytningFilter()
        {
            if (OnlyActiveCases)
            {
                return f => f.KliLoepenrNavigation.KliFoedselsdato.HasValue && !f.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(f.KliLoepenrNavigation.KliFraannenkommune == 1 && (f.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (f.KliLoepenrNavigation.KliFoedselsdato.HasValue && f.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else if (OnlyPassiveCases)
            {
                return f => f.KliLoepenrNavigation.KliFoedselsdato.HasValue && f.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && f.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(f.KliLoepenrNavigation.KliFraannenkommune == 1 && (f.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (f.KliLoepenrNavigation.KliFoedselsdato.HasValue && f.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else
            {
                return f => f.KliLoepenrNavigation.KliFoedselsdato.HasValue && (f.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !f.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && !(f.KliLoepenrNavigation.KliFraannenkommune == 1 && (f.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (f.KliLoepenrNavigation.KliFoedselsdato.HasValue && f.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
        }
        private Expression<Func<FaMeldinger, bool>> KlientMeldingFilter()
        {
            if (OnlyActiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else if (OnlyPassiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
        }
        private Expression<Func<FaUndersoekelser, bool>> KlientUndersøkelseFilter()
        {
            if (OnlyActiveCases)
            {
                return m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato.HasValue && !m.MelLoepenrNavigation.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.MelLoepenrNavigation.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else if (OnlyPassiveCases)
            {
                return m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && m.MelLoepenrNavigation.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.MelLoepenrNavigation.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else
            {
                return m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.MelLoepenrNavigation.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && m.MelLoepenrNavigation.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
        }
        private Expression<Func<FaTiltak, bool>> KlientTiltakFilter()
        {
            if (OnlyActiveCases)
            {
                return m => m.Sak != null && m.Sak.SakStatus != "BEH" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else if (OnlyPassiveCases)
            {
                return m => m.Sak != null && m.Sak.SakStatus != "BEH" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else
            {
                return m => m.Sak != null && m.Sak.SakStatus != "BEH" && m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
        }
        private Expression<Func<FaTiltaksplan, bool>> KlientPlanFilter()
        {
            if (OnlyActiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else if (OnlyPassiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
            else
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && m.KliLoepenrNavigation.KliFraannenkommune == 0;
            }
        }
        private Expression<Func<FaJournal, bool>> KlientJournalFilter()
        {
            if (OnlyActiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(m.KliLoepenrNavigation.KliFraannenkommune == 1 && (m.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else if (OnlyPassiveCases)
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration && m.KliLoepenrNavigation.KliAvsluttetdato.HasValue
                    && !(m.KliLoepenrNavigation.KliFraannenkommune == 1 && (m.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
            else
            {
                return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue)
                    && !(m.KliLoepenrNavigation.KliFraannenkommune == 1 && (m.KliLoepenrNavigation.KliAvsluttetdato.HasValue || (m.KliLoepenrNavigation.KliFoedselsdato.HasValue && m.KliLoepenrNavigation.KliFoedselsdato <= FromDateMigrationTilsyn)));
            }
        }
        #endregion
    }
}
