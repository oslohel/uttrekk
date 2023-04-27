#region Usings
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
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
        private string ConnectionStringFamilia = "";
        private readonly string ConnectionStringSokrates = "";
        private readonly string ConnectionStringVFB = "";
        private readonly string ConnectionStringMigrering = "";
        private readonly string MainDBServer = "";
        private const string SchemaSokrates = "SOKRATES";
        private readonly string OutputFolderName = "";
        private string Bydelsforkortelse = "";
        private readonly int MaksAntall;
        private readonly bool OnlyWriteDocumentFiles;
        private readonly int AntallFilerPerZip;
        private readonly Mappings mappings;
        private readonly string BVVLeder = "18";
        private readonly int MaxAntallEntiteterPerFil = 1000;
        #endregion

        #region Contructors
        public Uttrekk(string connSokrates, string mainDBServer, string extraDBServer, string outputFolderName, string bydelsidentifikator, int maksAntall, bool useSokrates, bool onlyWriteDocumentFiles, int antallFilerPerZip)
        {
            mappings = new Mappings(useSokrates);
            ConnectionStringFamilia = mappings.GetConnectionstring(bydelsidentifikator, mainDBServer);
            ConnectionStringVFB = mappings.GetConnectionstring("BVV", extraDBServer);
            ConnectionStringMigrering = mappings.GetConnectionstring("MIG", extraDBServer);
            ConnectionStringSokrates = connSokrates;
            MainDBServer = mainDBServer;
            OutputFolderName = outputFolderName;
            Bydelsforkortelse = bydelsidentifikator;
            if (maksAntall == 0)
            {
                MaksAntall = int.MaxValue;
            }
            else
            {
                MaksAntall = maksAntall;
            }
            OnlyWriteDocumentFiles = onlyWriteDocumentFiles;
            AntallFilerPerZip = antallFilerPerZip;
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
        public async Task WriteSokratesAsync(BackgroundWorker worker)
        {
            try
            {
                await ExtractSokratesAsync(worker, true);
                CreateFolderIfNotExist("saker");
                await WriteFileAsync(mappings.GetSokratesList(), GetJsonFileName("saker", "Soktrates oversikt"));
                worker.ReportProgress(0, $"Klar");
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
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
                                sokrates.tidligereAvdelinger.Add(tidligereBydel);
                                tidligereBydelerFinnes = true;
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
                                Decimal sizeDecimal = Decimal.Divide(size, 1024);
                                sizeDecimal = Decimal.Divide(sizeDecimal, 1024);
                                sizeDecimal = Decimal.Divide(sizeDecimal, 1024);
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
                                Decimal sizeDecimal = Decimal.Divide(size, 1024);
                                sizeDecimal = Decimal.Divide(sizeDecimal, 1024);
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
        public async Task GetOneFileFamiliaAsync(BackgroundWorker worker, decimal dokLoepenr)
        {
            try
            {
                CreateFolderIfNotExist("filer");

                FaDokumenter dokument;
                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    dokument = await context.FaDokumenters.Where(p => p.DokLoepenr == dokLoepenr).FirstOrDefaultAsync();
                    if (dokument == null)
                    {
                        MessageBox.Show($"Dokumentet med løpenr {dokLoepenr} ble ikke funnet i {Bydelsforkortelse}", "Dokument ikke funnet", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
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
                        await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + dokLoepenr.ToString() + fileExtension, (byte[])reader["Dok_Dokument"]);
                    }
                    reader.Close();
                    worker.ReportProgress(0, $"Hentet filen {dokLoepenr.ToString() + fileExtension}");
                }
                finally
                {
                    connection.Close();
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

        #region Information Sokrates
        public async Task GetInformationSokratesAsync(BackgroundWorker worker)
        {
            try
            {
                worker.ReportProgress(0, "Innhenter informasjon fra Sokrates...");
                string information = "INFORMASJON SOKRATES" + Environment.NewLine + Environment.NewLine;
                worker.ReportProgress(0, "Teller antall rader alle tabeller i Sokrates...");
                information += GetInfoTabellerOracle(ConnectionStringSokrates, SchemaSokrates);
                string fileName = $"{OutputFolderName}{DateTime.Now:yyyyMMdd_HHmm_}InformasjonSokrates.txt";
                await File.WriteAllTextAsync(fileName, information);
                worker.ReportProgress(0, $"Informasjon lagret i filen {fileName}");
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

        #region Tidstest
        public async Task TidstestAsync(BackgroundWorker worker)
        {
            try
            {
                CreateFolderIfNotExist("aktiviteter");
                CreateFolderIfNotExist("dokumenter");
                CreateFolderIfNotExist("filer");

                worker.ReportProgress(0, "Starter tidstest...");
                List<FaPostjournal> rawData;
                List<DocumentToInclude> documentsIncluded = new();

                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaPostjournals.Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "KK" || p.PosBrevtype == "AS" || p.PosBrevtype == "AN" || p.PosBrevtype == "RF" || p.PosBrevtype == "RA" || p.PosBrevtype == "BR" || p.PosBrevtype == "TU" || p.PosBrevtype == "RS" || p.PosBrevtype == "RK" || p.PosBrevtype == "RM" || p.PosBrevtype == "RV") && (p.KliLoepenrNavigation.KliFoedselsdato.HasValue && (p.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !p.KliLoepenrNavigation.KliAvsluttetdato.HasValue))).ToListAsync();
                    totalAntall = rawData.Count;
                }
                int antall = 0;
                foreach (var postjournal in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Tester uttrekk dokumenter ({antall} av {MaksAntall}, totalt {totalAntall})...");
                    }
                    if (postjournal.DokLoepenr.HasValue)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = postjournal.DokLoepenr.Value,
                            sakId = GetSakId(postjournal.KliLoepenr.ToString()),
                            tittel = postjournal.PosEmne,
                            journalDato = postjournal.PosFerdigdato
                        };
                        documentToInclude.aktivitetIdListe.Add(AddBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT"));
                        documentsIncluded.Add(documentToInclude);
                    }
                    if (antall >= MaksAntall)
                    {
                        break;
                    }
                }
                await GetDocumentsAsync(worker, "Dokumentaktiviteter", documentsIncluded);
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
        public void DoZipAsync(BackgroundWorker worker)
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
                        zip.CreateEntryFromFile(fil, fil[(fil.LastIndexOf("\\") + 1)..], CompressionLevel.SmallestSize);
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
                        string initialer = saksbehandler.EmployeeEmployeeId.ToString();
                        connection.Open();
                        SqlCommand command = new($"Select Fornavn,Etternavn,Epost From Brukere Where Virksomhet='{Bydelsforkortelse}' And Upper(FamiliaID)='{initialer}'", connection)
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
                int migrertAntall = brukere.Count;
                while (migrertAntall > toSkip)
                {
                    List<Bruker> brukerePart = brukere.OrderBy(o => o.brukerId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawData =  await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<Sak> saker = new();

                foreach (var caseCase in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnevernvaktsaker ({antall} av {totalAntall})...");
                    }
                    Sak sak = new()
                    {
                        sakId = AddBydel(caseCase.Number, "SAK"),
                        aktorId = GetActorId(await GetBVVPersonFromClientAsync(caseCase.ClientId)),
                        avdelingId = "BVV1",
                        startDato = caseCase.CreatedDate,
                        sluttDato = caseCase.StatusClosedDate,
                        merknad = caseCase.Description?.Trim(),
                        arbeidsbelastning = "LAV",
                        saksnavn = caseCase.Title,
                        sakstype = "BARNEVERNSVAKT"
                    };
                    if (caseCase.OwnedBy.HasValue)
                    {
                        sak.saksbehandlerId = AddBydel(caseCase.OwnedBy.ToString());
                    }
                    else
                    {
                        sak.saksbehandlerId = AddBydel(BVVLeder);
                    }
                    sak.typeBarnevernsvaktsak = mappings.GetBVVTypeBarnevernsvaktsak(caseCase.PersoncaseTypeRegistryId);
                    sak.hovedkategori = mappings.GetBVVHovedkategori(caseCase.MainCategoryRegistryId);
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        EnquiryEnquiry enquiry = await context.EnquiryEnquiries.Where(k => k.CaseId == caseCase.CaseCaseId).FirstOrDefaultAsync();
                        if (enquiry != null)
                        {
                            sak.melder = mappings.GetBVVMelder(enquiry.ReporterTypeRegistryId);
                        }
                        if (string.IsNullOrEmpty(sak.melder))
                        {
                            sak.melder = "ANDRE";
                        }
                    }
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        List<CaseCasecaseworker> caseWorkers = await context.CaseCasecaseworkers.Where(k => k.CaseId == caseCase.CaseCaseId).ToListAsync();
                        foreach (var caseWorker in caseWorkers)
                        {
                            sak.sekunderSaksbehandlerId.Add(AddBydel(caseWorker.CaseworkerId.ToString()));
                        }
                    }
                    if (string.IsNullOrEmpty(sak.saksbehandlerId))
                    {
                        sak.saksbehandlerId = AddBydel(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                    }
                    if (!caseCase.StatusClosedDate.HasValue)
                    {
                        sak.status = "ÅPEN";
                    }
                    else
                    {
                        sak.status = "LUKKET";
                    }
                    //TODO BVV - Avvente endring slik at "Interne saksforberedelser" også blir gyldig for barnevernvaktsaker og kan brukes til logger
                    Aktivitet aktivitet = new()
                    {
                        aktivitetId = AddBydel(sak.sakId, "LOG"),
                        sakId = sak.sakId,
                        aktivitetsType = "BARNEVERNSVAKT",
                        aktivitetsUnderType = "ANNEN_DOKUMENTASJON",
                        status = "UTFØRT",
                        hendelsesdato = DateTime.Now,
                        saksbehandlerId = AddBydel(BVVLeder),
                        tittel = "Logg",
                        utfortAvId = AddBydel(BVVLeder),
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
                        innhold = await GetBVVLoggAsync(caseCase.CaseCaseId)
                    };
                    htmlDocumentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                    htmlDocumentsIncluded.Add(htmlDocumentToInclude);
                    await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, $"Logg{Guid.NewGuid().ToString().Replace('-', '_')}", "Log", false);
                    aktiviteter.Add(aktivitet);
                    saker.Add(sak);
                }
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = aktiviteter.Count;
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        private async Task<string> GetBVVLoggAsync(int caseCaseId)
        {
            StringBuilder logg = new();

            using (var context = new BVVDBContext(ConnectionStringVFB))
            {
                List<ActivitylogActivitylog> activitylogs = await context.ActivitylogActivitylogs.Where(k => k.CaseId == caseCaseId).OrderByDescending(o => o.CreatedDate).Take(2000).ToListAsync();
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
                int currentClientId = 0;
                bool included = false;
                using (var context = new BVVDBContext(ConnectionStringVFB))
                {
                    ClientClient client = await context.ClientClients.Where(c => c.PersonId == person.PersonPersonId).FirstOrDefaultAsync();
                    if (client is not null)
                    {
                        currentClientId = client.ClientClientId;
                        included = true;
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
                        telefonnummer = person.PhonePrefix?.Trim() + person.Phone?.Trim(),
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
                        telefonnummer = person.SecondaryPhonePrefix?.Trim() + person.SecondaryPhone?.Trim()
                    };
                    if (!hovetelefonBestemt)
                    {
                        aktørTelefonnummerPrivat.hovedtelefon = true;
                    }
                    innbygger.telefonnummer.Add(aktørTelefonnummerPrivat);
                }
                bool hovedAdresseBestemt = false;
                if (!string.IsNullOrEmpty(person.Address) || person.PostCodeRegistryId.HasValue || person.MunicipalityRegistryId.HasValue)
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(person.PersonPersonId.ToString(), "1"),
                        adresseType = "BOSTEDSADRESSE",
                        linje1 = person.Address?.Trim(),
                        postnummer = await GetBVVBaseregistryValueAsync(person.PostCodeRegistryId, true),
                        poststed = await GetBVVBaseregistryValueAsync(person.MunicipalityRegistryId),
                        hovedadresse = true
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    hovedAdresseBestemt = true;
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
                    if (!hovedAdresseBestemt)
                    {
                        adresse.hovedadresse = true;
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
                innbyggere.Add(innbygger);
            }
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = innbyggere.Count;
            while (migrertAntall > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggere.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                rawData = await context.PersonPeople.ToListAsync();
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
                    if (client is null)
                    {
                        included = true;
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
                        telefonnummer = person.PhonePrefix?.Trim() + person.Phone?.Trim(),
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
                        telefonnummer = person.SecondaryPhonePrefix?.Trim() + person.SecondaryPhone?.Trim()
                    };
                    if (!hovetelefonBestemt)
                    {
                        aktørTelefonnummerPrivat.hovedtelefon = true;
                    }
                    innbygger.telefonnummer.Add(aktørTelefonnummerPrivat);
                }
                bool hovedAdresseBestemt = false;
                if (!string.IsNullOrEmpty(person.Address) || person.PostCodeRegistryId.HasValue || person.MunicipalityRegistryId.HasValue)
                {
                    AktørAdresse adresse = new()
                    {
                        adresseId = AddBydel(person.PersonPersonId.ToString(), "FOR__1"),
                        adresseType = "BOSTEDSADRESSE",
                        linje1 = person.Address?.Trim(),
                        postnummer = await GetBVVBaseregistryValueAsync(person.PostCodeRegistryId, true),
                        poststed = await GetBVVBaseregistryValueAsync(person.MunicipalityRegistryId),
                        hovedadresse = true
                    };
                    if (string.IsNullOrEmpty(adresse.postnummer))
                    {
                        adresse.adresseType = "UFULDSTÆNDIG_ADRESSE";
                    }
                    hovedAdresseBestemt = true;
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
                    if (!hovedAdresseBestemt)
                    {
                        adresse.hovedadresse = true;
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
            int migrertAntall = innbyggere.Count;
            while (migrertAntall > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggere.OrderBy(v => v.actorId).OrderBy(w => w.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawData = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).ToListAsync();
                    totalAntall = rawData.Count;
                }
                List<BarnetsNettverk> barnetsNettverk = new();
                int antall = 0;

                foreach (var caseCase in rawData)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnets nettverk - barnet ({antall} av {totalAntall})...");
                    }
                    BarnetsNettverk forbindelse = new()
                    {
                        sakId = AddBydel(caseCase.Number, "SAK"),
                        actorId = GetActorId(await GetBVVPersonFromClientAsync(caseCase.ClientId)),
                        relasjonTilSak = "HOVEDPERSON",
                        rolle = "HOVEDPERSON",
                        foresatt = false
                    };
                    barnetsNettverk.Add(forbindelse);
                }
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = barnetsNettverk.Count;
                while (migrertAntall > toSkip)
                {
                    List<BarnetsNettverk> barnetsNettverkPart = barnetsNettverk.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawCases = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).ToListAsync();
                    totalAntall = rawCases.Count;
                }
                List<BarnetsNettverk> barnetsNettverk = new();
                int antall = 0;

                foreach (var caseCase in rawCases)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnets nettverk - familie ({antall} av {totalAntall})...");
                    }
                    int childrenPersonId = (await GetBVVPersonFromClientAsync(caseCase.ClientId)).PersonPersonId;

                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        List<PersonPersonrole> rawRelations = await context.PersonPersonroles.Where(k => k.RelatedPersonId == childrenPersonId).ToListAsync();
                        foreach (var relation in rawRelations)
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
                    rawCases = await context.CaseCases.Where(k => k.Type != 2 && k.Type != 3).ToListAsync();
                    totalAntall = rawCases.Count;
                }
                List<BarnetsNettverk> barnetsNettverk = new();
                int antall = 0;

                foreach (var caseCase in rawCases)
                {
                    antall += 1;
                    if (antall % 10 == 0)
                    {
                        worker.ReportProgress(0, $"Behandler uttrekk Visma Flyt Barnevernvakt barnets nettverk ({antall} av {totalAntall})...");
                    }
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        List<PersonNetworkpersonrole> rawRelations = await context.PersonNetworkpersonroles.Where(k => k.RelatedClientId == caseCase.ClientId).ToListAsync();
                        foreach (var relation in rawRelations)
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
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = barnetsNettverk.Count;
                while (migrertAntall > toSkip)
                {
                    List<BarnetsNettverk> barnetsNettverkPart = barnetsNettverk.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
            List<Aktivitet> aktiviteter = new();
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
                    EnquiryEnquiry henvendelse;
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        henvendelse = await context.EnquiryEnquiries.Where(e => e.EnquiryEnquiryId == enquiryDocument.EnquiryId).FirstOrDefaultAsync();
                        if (!henvendelse.CaseId.HasValue)
                        {
                            if (!henvendelse.ClientId.HasValue)
                            {
                                continue;
                            }
                            int numberOfCases = await context.CaseCases.Where(e => e.ClientId == henvendelse.ClientId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            else
                            {
                                CaseCase singleCase = await context.CaseCases.Where(e => e.ClientId == henvendelse.ClientId && e.Type == 1).FirstOrDefaultAsync();
                                caseId = singleCase.CaseCaseId;
                            }
                        }
                        else
                        {
                            int numberOfCases = await context.CaseCases.Where(e => e.CaseCaseId == henvendelse.CaseId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            caseId = henvendelse.CaseId;
                        }
                    }
                    Aktivitet aktivitet = new()
                    {
                        aktivitetId = AddBydel(enquiryDocument.EnquiryDocumentId.ToString(), "HEN"),
                        aktivitetsType = "BARNEVERNSVAKT",
                        aktivitetsUnderType = "ANNEN_DOKUMENTASJON",
                        status = "UTFØRT",
                        hendelsesdato = henvendelse.ReportedDate,
                        saksbehandlerId = AddBydel(henvendelse.CreatedBy.ToString()),
                        tittel = henvendelse.Subject,
                        utfortAvId = AddBydel(henvendelse.CreatedBy.ToString()),
                        utfortDato = henvendelse.FinishedDate,
                        notat = "Se dokument"
                    };
                    if (!aktivitet.utfortDato.HasValue)
                    {
                        aktivitet.utfortDato = aktivitet.hendelsesdato;
                    }
                    using (var context = new BVVDBContext(ConnectionStringVFB))
                    {
                        CaseCase rawCase = await context.CaseCases.Where(e => e.CaseCaseId == caseId).FirstOrDefaultAsync();
                        aktivitet.sakId = AddBydel(rawCase.Number, "SAK");
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
                        documents.Add(document);

                        SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                        SqlDataReader readerMigrering = null;
                        try
                        {
                            connectionMigrering.Open();
                            bool fileAlreadyWritten = false;
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer Where Filnavn='{document.filId}'", connectionMigrering)
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
                                    await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + document.filId + ".pdf", (byte[])reader["FileDataBlob"]);
                                }
                                commandMigrering = new($"Insert Into Filer (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
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
                                sakId = aktivitet.sakId,
                                tittel = aktivitet.tittel,
                                journalDato = aktivitet.utfortDato,
                                opprettetAvId = aktivitet.utfortAvId,
                                innhold = enquiryDocument.DocumentText
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
            await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, "Henvendelser", "Hen");
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = documents.Count;
            while (migrertAntall > toSkip)
            {
                List<Document> documentsPart = documents.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"DokumenterBVVHen{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
            toSkip = 0;
            fileNumber = 1;
            migrertAntall = aktiviteter.Count;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"BVVHenvendelser{fileNumber}"));
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
                            int numberOfCases = await context.CaseCases.Where(e => e.ClientId == journal.ClientId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            else
                            {
                                CaseCase singleCase = await context.CaseCases.Where(e => e.ClientId == journal.ClientId && e.Type == 1).FirstOrDefaultAsync();
                                caseId = singleCase.CaseCaseId;
                            }
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
                        saksbehandlerId = AddBydel(journal.OwnedBy.ToString()),
                        tittel = journal.Title,
                        utfortAvId = AddBydel(journal.OwnedBy.ToString()),
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
                        aktivitet.sakId = AddBydel(rawCase.Number, "SAK");
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
                        documents.Add(document);

                        SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                        SqlDataReader readerMigrering = null;
                        try
                        {
                            connectionMigrering.Open();
                            bool fileAlreadyWritten = false;
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer Where Filnavn='{document.filId}'", connectionMigrering)
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
                                    await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + document.filId + ".pdf", (byte[])reader["FileDataBlob"]);
                                }
                                commandMigrering = new($"Insert Into Filer (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
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
            int migrertAntall = documents.Count;
            while (migrertAntall > toSkip)
            {
                List<Document> documentsPart = documents.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                            int numberOfCases = await context.CaseCases.Where(e => e.ClientId == correspondence.ClientId && e.Type == 1).CountAsync();
                            if (numberOfCases != 1)
                            {
                                continue;
                            }
                            else
                            {
                                CaseCase singleCase = await context.CaseCases.Where(e => e.ClientId == correspondence.ClientId && e.Type == 1).FirstOrDefaultAsync();
                                caseId = singleCase.CaseCaseId;
                            }
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
                    Aktivitet aktivitet = new()
                    {
                        aktivitetId = AddBydel(correspondenceDocument.CorrespondenceDocumentId.ToString(), "POS"),
                        aktivitetsType = "BARNEVERNSVAKT",
                        aktivitetsUnderType = mappings.GetBVVCorrespondenceCategory(correspondence.CorrespondenceCategoryRegistryId),
                        status = "UTFØRT",
                        hendelsesdato = correspondence.CorrespondenceDate,
                        saksbehandlerId = AddBydel(correspondence.OwnedBy.ToString()),
                        tittel = correspondence.Title,
                        utfortAvId = AddBydel(correspondence.OwnedBy.ToString()),
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
                        aktivitet.sakId = AddBydel(rawCase.Number, "SAK");
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
                        documents.Add(document);

                        SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                        SqlDataReader readerMigrering = null;
                        try
                        {
                            connectionMigrering.Open();
                            bool fileAlreadyWritten = false;
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer Where Filnavn='{document.filId}'", connectionMigrering)
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
                                    await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + document.filId + ".pdf", (byte[])reader["FileDataBlob"]);
                                }
                                commandMigrering = new($"Insert Into Filer (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
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
                            documents.Add(attachmentDocument);

                            SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                            SqlDataReader readerMigrering = null;
                            try
                            {
                                connectionMigrering.Open();
                                bool fileAlreadyWritten = false;
                                SqlCommand commandMigrering = new($"Select Count(*) From Filer Where Filnavn='{attachmentDocument.filId}'", connectionMigrering)
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
                                    await File.WriteAllBytesAsync(OutputFolderName + "filer\\" + attachmentDocument.filId + ".pdf", attachment.FileDataBlob);
                                    commandMigrering = new($"Insert Into Filer (FilNavn,Bydel,Dato) Values ('{attachmentDocument.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
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
            finally
            {
                connection.Close();
            }            
            await GetHtmlDocumentsAsync(worker, htmlDocumentsIncluded, "Post", "Pos");
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = documents.Count;
            while (migrertAntall > toSkip)
            {
                List<Document> documentsPart = documents.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
            return antall;
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
        public async Task<string> GetSakerAsync(BackgroundWorker worker, bool meldingerUtenSakIsChecked)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk saker...");
                string statusText = $"Antall barnevernsaker: {await GetBarnevernsakerAsync(worker)}" + Environment.NewLine;
                if (meldingerUtenSakIsChecked && MaksAntall == int.MaxValue)
                {
                    statusText += $"Antall barnevernsaker uten sak: {await GetBarnevernsakerUtenSakAsync(worker)}" + Environment.NewLine;
                }
                statusText += $"Antall tilsynssaker: {await GetTilsynssakerAsync(worker)}" + Environment.NewLine;
                if (MaksAntall == int.MaxValue)
                {
                    statusText += $"Antall oppdragstakersaker: {await GetOppdragstakersakerAsync(worker)}" + Environment.NewLine;
                }
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
                        .Take(MaksAntall)
                        . Where(k => k.KliFraannenkommune == 0)
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
                    string fodselsnummer = GetDEMValue(klient.KliLoepenr, klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr);
                    sak.aktorId = GetActorId(klient, fodselsnummer);

                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        sak.tidligereAvdelingListe = mappings.GetTidligereBydeler(klient.KliLoepenr);
                    }
                    if (!string.IsNullOrEmpty(klient.SbhInitialer))
                    {
                        sak.saksbehandlerId = AddBydel(klient.SbhInitialer);
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.sekunderSaksbehandlerId.Add(AddBydel(klient.SbhInitialer2));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.saksbehandlerId = AddBydel(klient.SbhInitialer2);
                        }
                    }
                    if (string.IsNullOrEmpty(sak.saksbehandlerId))
                    {
                        sak.saksbehandlerId = AddBydel(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                    }
                    if (!klient.KliAvsluttetdato.HasValue)
                    {
                        sak.status = "ÅPEN";
                    }
                    else
                    {
                        sak.status = "LUKKET";
                    }
                    saker.Add(sak);
                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        await GetDataTidligereBydelerAsync(worker, klient.KliFoedselsdato.Value, klient.KliPersonnr.Value, sak.sakId, sak.tidligereAvdelingListe);
                    }
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = saker.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = saker.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue).ToListAsync();
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
                    if (!string.IsNullOrEmpty(melding.SbhInitialer))
                    {
                        sak.saksbehandlerId = AddBydel(melding.SbhInitialer);
                    }
                    else
                    {
                        sak.saksbehandlerId = AddBydel(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
                    }
                    saker.Add(sak);
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = saker.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = saker.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                        .Where(m => !m.KliAvsluttetdato.HasValue && m.KliFoedselsdato.HasValue && m.KliFoedselsdato > FromDateMigrationTilsyn)
                        .OrderBy(o => o.KliLoepenr)
                        .Take(MaksAntall)
                        .Where(k => k.KliFraannenkommune == 1)
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
                        arbeidsbelastning = "LAV"
                    };
                    string fodselsnummer = GetDEMValue(klient.KliLoepenr, klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr);
                    sak.aktorId = GetActorId(klient, fodselsnummer);

                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        sak.tidligereAvdelingListe = mappings.GetTidligereBydeler(klient.KliLoepenr);
                    }
                    if (!string.IsNullOrEmpty(klient.SbhInitialer))
                    {
                        sak.saksbehandlerId = AddBydel(klient.SbhInitialer);
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.sekunderSaksbehandlerId.Add(AddBydel(klient.SbhInitialer2));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(klient.SbhInitialer2))
                        {
                            sak.saksbehandlerId = AddBydel(klient.SbhInitialer2);
                        }
                    }
                    if (string.IsNullOrEmpty(sak.saksbehandlerId))
                    {
                        sak.saksbehandlerId = AddBydel(mappings.GetHovedsaksbehandlerBydel(Bydelsforkortelse));
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
                    if (mappings.HarTidligereBydeler(klient.KliLoepenr))
                    {
                        await GetDataTidligereBydelerAsync(worker, klient.KliFoedselsdato.Value, klient.KliPersonnr.Value, sak.sakId, sak.tidligereAvdelingListe);
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = saker.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = saker.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawData = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => !string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue).ToListAsync();
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
                            && (e.EngTildato >= FirstInYearOfMigration || e.EngStoppetdato >= FirstInYearOfMigration)).CountAsync();
                    }
                    if (numberOfActiveContracts == 0)
                    {
                        continue;
                    }
                    Sak sak = new()
                    {
                        avdelingId = "SuppliersAndContractorsTeam",
                        aktorId = GetActorId(medarbeider.ForLoepenrNavigation),
                        startDato = medarbeider.MedBegyntdato,
                        status = "ÅPEN",
                        arbeidsbelastning = "LAV",
                        sakstype = "OPPDRAGSTAKER"
                    };
                    sak.sakId = $"{sak.aktorId}__OPP";

                    bool caseAlreadyWritten = false;

                    SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                    SqlDataReader readerMigrering = null;
                    try
                    {
                        connectionMigrering.Open();

                        SqlCommand commandMigrering = new($"Select Count(*) From Oppdragstakere Where SakId='{sak.sakId}'", connectionMigrering)
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
                            commandMigrering = new($"Update Oppdragstakere Set SakId='{sak.sakId}', Dato=GETDATE() Where AktorId='{sak.aktorId}'", connectionMigrering)
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
                    if (!string.IsNullOrEmpty(medarbeider.SbhEndretav))
                    {
                        sak.saksbehandlerId = AddBydel(medarbeider.SbhEndretav);
                    }
                    else if (!string.IsNullOrEmpty(medarbeider.SbhRegistrertav))
                    {
                        sak.saksbehandlerId = AddBydel(medarbeider.SbhRegistrertav);
                    }
                    saker.Add(sak);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = saker.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Sak> sakerPart = saker.OrderBy(o => o.sakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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

        #region Innbyggere - Barn
        public async Task<string> GetInnbyggereBarnAsync(BackgroundWorker worker, bool meldingerUtenSakIsChecked)
        {
            try
            {
                worker.ReportProgress(0, "Starter uttrekk innbyggere - barn...");
                string statusText = $"Antall innbyggere barn: {await GetInnbyggereBarnHovedAsync(worker)}" + Environment.NewLine;
                if (meldingerUtenSakIsChecked && MaksAntall == int.MaxValue)
                {
                    statusText += $"Antall innbyggere barn uten sak: {await GetInnbyggereBarnUtenSakAsync(worker)}" + Environment.NewLine;
                }
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
                    .OrderBy(o => o.KliLoepenr)
                    .Take(MaksAntall)
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
                if (!mappings.IsOwner(klient.KliLoepenr))
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
                if (klient.KliPersonnr.GetValueOrDefault() != 99999 && klient.KliPersonnr.GetValueOrDefault() != 00100 && klient.KliPersonnr.GetValueOrDefault() != 00200)
                {
                    innbygger.fodselsnummer = GetDEMValue(klient.KliLoepenr, klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr);
                }
                innbygger.actorId = GetActorId(klient, innbygger.fodselsnummer);

                if (klient.KliFremmedkontrollnr.HasValue)
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
                }
                bool hovetelefonBestemt = false;
                if (!string.IsNullOrEmpty(klient.KliTelefonmobil))
                {
                    AktørTelefonnummer aktørTelefonnummerMobil = new()
                    {
                        telefonnummerType = "PRIVAT",
                        telefonnummer = klient.KliTelefonmobil?.Trim(),
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
                        telefonnummer = klient.KliTelefonprivat?.Trim()
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
                        telefonnummer = klient.KliTelefonarbeid?.Trim()
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
                        hovedadresse = true
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
            int antallEntiteter = innbyggere.Count;
            while (antallEntiteter > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggere.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue).ToListAsync();
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
                Innbygger innbygger = new()
                {
                    actorId = AddBydel(melding.MelLoepenr.ToString()),
                    registrertDato = melding.MelRegistrertdato.Value,
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
                    innbygger.fodselsnummer = melding.MelFoedselsdato.Value.ToString("ddMMyy") + melding.MelPersonnr;
                }
                if (melding.MelPersonnr.HasValue && melding.MelPersonnr.Value == 99999)
                {
                    innbygger.ufodtBarn = true;
                }
                innbyggere.Add(innbygger);
            }
            int toSkip = 0;
            int fileNumber = 1;
            int antallEntiteter = innbyggere.Count;
            while (antallEntiteter > toSkip)
            {
                List<Innbygger> innbyggerePart = innbyggere.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawDataMedarbeidere = await context.FaMedarbeideres.Include(m => m.ForLoepenrNavigation).Where(f => !string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue).Select(m => m.ForLoepenrNavigation).Distinct().ToListAsync();
                    var rollerInkludert = new string[] { "MOR", "FAR", "SØS", "FMO", "FFA", "FAM", "VRG", "BRH", "BSH", "FSA" };
                    rawDataKlienttilknytninger = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation)
                        .Where(KlientTilknytningFilter())
                        .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                        .Take(MaksAntall)
                        .Where(f => (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue) || rollerInkludert.Contains(f.KtkRolle))
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
                    Innbygger innbygger = new()
                    {
                        actorId = GetActorId(forbindelse),
                        fornavn = forbindelse.ForFornavn?.Trim(),
                        etternavn = forbindelse.ForEtternavn?.Trim(),
                        sivilstand = "UOPPGITT",
                        kontonummer = forbindelse.ForKontonummer?.Trim(),
                        foedeland = mappings.GetLand(forbindelse.NasKodenr),
                        deaktiver = false
                    };
                    FaForbindelser medarbeider = rawDataMedarbeidere.Find(m => m.ForLoepenr == forbindelse.ForLoepenr);
                    if (medarbeider is not null)
                    {
                        bool actorAlreadyWritten = false;

                        SqlConnection connectionMigrering = new(ConnectionStringMigrering);
                        SqlDataReader readerMigrering = null;
                        try
                        {
                            connectionMigrering.Open();

                            SqlCommand commandMigrering = new($"Select Count(*) From Oppdragstakere Where AktorId='{innbygger.actorId}'", connectionMigrering)
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
                                commandMigrering = new($"Insert Into Oppdragstakere (AktorId,Bydel,Dato) Values ('{innbygger.actorId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
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
                        innbygger.potensiellOppdragstaker = true;
                    }
                    else
                    {
                        innbygger.potensiellOppdragstaker = false;
                    }
                    if (forbindelse.ForRegistrertdato.HasValue)
                    {
                        innbygger.registrertDato = forbindelse.ForRegistrertdato.Value;
                    }
                    else
                    {
                        innbygger.registrertDato = LastDateNoMigration;
                    }
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
                        if (Bydelsforkortelse != "DEM")
                        {
                            innbygger.fodselDato = new DateTime(year, int.Parse(forbindelse.ForFoedselsnummer.Substring(2, 2)), int.Parse(forbindelse.ForFoedselsnummer[..2]));
                        }
                        else
                        {
                            innbygger.fodselDato = DateTime.Now.AddYears(-32);
                        }
                    }
                    else
                    {
                        if (forbindelse.ForDnummer.HasValue)
                        {
                            innbygger.fodselsnummer = forbindelse.ForDnummer.Value.ToString();
                        }
                    }
                    bool hovetelefonBestemt = false;
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonmobil))
                    {
                        AktørTelefonnummer aktørTelefonnummerMobil = new()
                        {
                            telefonnummerType = "PRIVAT",
                            telefonnummer = forbindelse.ForTelefonmobil?.Trim(),
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
                            telefonnummer = forbindelse.ForTelefonprivat?.Trim()
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
                            telefonnummer = forbindelse.ForTelefonarbeid?.Trim()
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
                            hovedadresse = true
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
                int antallEntiteter = innbyggere.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Innbygger> innbyggerePart = innbyggere.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                        .Take(MaksAntall)
                        .Where(f => !string.IsNullOrEmpty(f.ForLoepenrNavigation.ForOrganisasjonsnr) && f.ForLoepenrNavigation.ForOrganisasjonsnr.Length != 9 && f.ForLoepenrNavigation.ForOrganisasjonsnr.IndexOf(" ") == -1)
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
                        actorId = GetActorId(forbindelse),
                        organisasjonsnummer = forbindelse.ForOrganisasjonsnr?.Trim(),
                        kontonummer = forbindelse.ForKontonummer?.Trim(),
                        deaktiver = false
                    };
                    organisasjon.eksternId = organisasjon.actorId;
                    if (forbindelse.FotIdents != null)
                    {
                        foreach (FaForbindelsestyper type in forbindelse.FotIdents)
                        {
                            GetOrganisasjonsKategori(organisasjon, type);
                        }
                    }
                    if (forbindelse.ForBetalingsmaate == "L")
                    {
                        organisasjon.leverandørAvTiltak = "JA_ALM_TILTAK";
                    }

                    bool hovetelefonBestemt = false;
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonarbeid))
                    {
                        AktørTelefonnummer aktørTelefonnummerArbeid = new()
                        {
                            telefonnummerType = "ANNET",
                            telefonnummer = forbindelse.ForTelefonarbeid?.Trim(),
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
                            telefonnummer = forbindelse.ForTelefonmobil?.Trim()
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
                            telefonnummer = forbindelse.ForTelefonprivat?.Trim()
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
                            hovedadresse = true
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

        #region BarnetsNettverk - Barn
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
                        .OrderBy(o => o.KliLoepenr)
                        .Take(MaksAntall)
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
                    if (!mappings.IsOwner(klient.KliLoepenr))
                    {
                        continue;
                    }
                    BarnetsNettverk forbindelse = new()
                    {
                        sakId = GetSakId(klient.KliLoepenr.ToString()),
                        relasjonTilSak = "HOVEDPERSON",
                        rolle = "HOVEDPERSON",
                        foresatt = false
                    };
                    string fodselsnummer = GetDEMValue(klient.KliLoepenr, klient.KliFoedselsdato.Value.ToString("ddMMyy") + klient.KliPersonnr);
                    forbindelse.actorId = GetActorId(klient, fodselsnummer);
                    forbindeler.Add(forbindelse);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = forbindeler.Count;
                while (antallEntiteter > toSkip)
                {
                    List<BarnetsNettverk> forbindelerPart = forbindeler.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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

        #region BarnetsNettverk
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
                        .Take(MaksAntall)
                        .Where(k => (!string.IsNullOrEmpty(k.ForLoepenrNavigation.ForFoedselsnummer) || k.ForLoepenrNavigation.ForDnummer.HasValue || (!string.IsNullOrEmpty(k.ForLoepenrNavigation.ForOrganisasjonsnr) && k.ForLoepenrNavigation.ForOrganisasjonsnr.Length != 9 && k.ForLoepenrNavigation.ForOrganisasjonsnr.IndexOf(" ") == -1)) || (rollerInkludert.Contains(k.KtkRolle)))
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
                    if (!mappings.IsOwner(klientTilknytning.KliLoepenr))
                    {
                        continue;
                    }
                    BarnetsNettverk forbindelse = new()
                    {
                        sakId = GetSakId(klientTilknytning.KliLoepenr.ToString()),
                        actorId = GetActorId(klientTilknytning.ForLoepenrNavigation),
                        kommentar = klientTilknytning.KtkMerknad?.Trim()
                    };
                    GetNettverksRolle(klientTilknytning, forbindelse);
                    if (klientTilknytning.KtkForesatt == 1)
                    {
                        forbindelse.foresatt = true;
                    }
                    else
                    {
                        forbindelse.foresatt = false;
                    }
                    forbindeler.Add(forbindelse);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = forbindeler.Count;
                while (antallEntiteter > toSkip)
                {
                    List<BarnetsNettverk> forbindelerPart = forbindeler.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                        .Take(MaksAntall)
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
                int antallEntiteter = meldinger.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Melding> meldingerPart = meldinger.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        private async Task<int> UttrekkMeldingerAsync(bool utenSak, List<FaMeldinger> rawData, List<Melding> meldinger, List<DocumentToInclude> documentsIncluded, BackgroundWorker worker, int totalAntall, string title)
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
                        melding.sakId = GetSakId(meldingFamilia.MelLoepenr.ToString() + "__MUS");
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
                    if (meldingFamilia.DokLoepenr.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && meldingFamilia.DokLoepenr.HasValue && d.DokLoepenr == meldingFamilia.DokLoepenr.Value).FirstOrDefaultAsync();
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
                if (MaksAntall != int.MaxValue)
                {
                    return null;
                }
                worker.ReportProgress(0, "Starter uttrekk meldinger uten sak...");
                List<FaMeldinger> rawData;
                int totalAntall = 0;

                using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                {
                    rawData = await context.FaMeldingers.Where(m => !m.KliLoepenr.HasValue && m.MelMeldingstype != "UGR" && m.MelAvsluttetgjennomgang.HasValue).ToListAsync();
                    totalAntall = rawData.Count;
                }

                List<Melding> meldinger = new();
                List<DocumentToInclude> documentsIncluded = new();
                int migrertAntall = await UttrekkMeldingerAsync(true, rawData, meldinger, documentsIncluded, worker, totalAntall, "meldinger uten sak"); ;
                await GetDocumentsAsync(worker, "MeldingerUtensak", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = meldinger.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Melding> meldingerPart = meldinger.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        #endregion

        #region Undersokelser
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
                        .Take(MaksAntall)
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
                    Undersøkelse undersoekelse = new()
                    {
                        undersokelseId = AddBydel(undersøkelse.MelLoepenr.ToString(), "UND"),
                        meldingId = AddBydel(undersøkelse.MelLoepenr.ToString(), "MEL"),
                        sakId = GetSakId(undersøkelse.MelLoepenrNavigation.KliLoepenr?.ToString()),
                        startDato = undersøkelse.UndStartdato
                    };
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
                    if (undersøkelse.UndBehandlingstid == "2")
                    {
                        Aktivitet undersøkelseUtvidelseFristAktivitet = new()
                        {
                            aktivitetId = AddBydel(undersøkelse.MelLoepenr.ToString(), "UNDUT"),
                            sakId = undersoekelse.sakId,
                            aktivitetsType = "UTVIDELSE_AV_FRIST",
                            aktivitetsUnderType = "UNDERSØKELSE",
                            status = "UTFØRT",
                            saksbehandlerId = AddBydel(undersøkelse.SbhInitialer),
                            tittel = "Beslutning om utvidet undersøkelsestid",
                            utfortAvId = AddBydel(undersøkelse.SbhInitialer),
                            notat = undersøkelse.Und6mndbegrunnelse,
                            fristDato = undersøkelse.UndFristdato,
                            fristLovpaalagt = true
                        };
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
                        if (undersøkelse.UndFristdato.HasValue)
                        {
                            undersøkelseUtvidelseFristAktivitet.hendelsesdato = undersøkelse.UndFristdato.Value.AddDays(-90);
                            undersøkelseUtvidelseFristAktivitet.utfortDato = undersøkelse.UndFristdato.Value.AddDays(-90);
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
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        FaSaksjournal saksJournal = await context.FaSaksjournals.Where(m => m.MelLoepenr == undersøkelse.MelLoepenr).FirstOrDefaultAsync();
                        if (saksJournal != null)
                        {
                            undersoekelse.vedtakAktivitetId = AddBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED");
                        }
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
                                saksbehandlerId = AddBydel(undersøkelse.SbhInitialer),
                                tittel = "Undersøkelsesplan",
                                utfortAvId = AddBydel(undersøkelse.SbhInitialer),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdatoUplan,
                                utfortDato = undersøkelse.UndFerdigdatoUplan
                            };
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
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
                                saksbehandlerId = AddBydel(undersøkelse.SbhInitialer),
                                tittel = "Sluttrapport undersøkelse",
                                utfortAvId = AddBydel(undersøkelse.SbhInitialer),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdato,
                                utfortDato = undersøkelse.UndFerdigdato
                            };
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
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
                int antallEntiteter = undersøkelsesAktiviteter.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> undersøkelsesAktiviteterPart = undersøkelsesAktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(undersøkelsesAktiviteterPart, GetJsonFileName("aktiviteter", $"UndersokelsesAktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                antallEntiteter = undersøkelser.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Undersøkelse> undersøkelserPart = undersøkelser.OrderBy(o => o.sakId).OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    rawData = await context.FaDistrikts.ToListAsync();
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
                int antallEntiteter = avdelingsmappinger.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Avdelingsmapping> avdelingsmappingerPart = avdelingsmappinger.OrderBy(o => o.avdelingId).OrderBy(o => o.enhetskodeModulusBarn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    if (Bydelsforkortelse != "DEM" && saksbehandler.TggIdents.Count == 1 && (saksbehandler.TggIdents.First().TggIdent == "SEI" || saksbehandler.TggIdents.First().TggIdent == "DRL"))
                    {
                        continue;
                    }
                    Bruker bruker = new()
                    {
                        brukerId = AddBydel(saksbehandler.SbhInitialer)
                    };
                    SqlConnection connection = new(ConnectionStringMigrering);
                    SqlDataReader reader = null;
                    try
                    {
                        string initialer = saksbehandler.SbhInitialer.ToUpper();
                        connection.Open();
                        SqlCommand command = new($"Select HRID,Fornavn,Etternavn,Epost From Brukere Where Upper(Virksomhet)='{Bydelsforkortelse}' And Upper(FamiliaID)='{initialer}'", connection)
                        {
                            CommandTimeout = 300
                        };
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                bruker.okonomiEksternId = reader.GetString(0);
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
                            bruker.email = saksbehandler.SbhMailadresse?.Trim();
                            bruker.brukerNokkelModulusBarn = bruker.brukerId;
                        }
                        reader.Close();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    foreach (var distrikt in saksbehandler.DisDistriktskodes)
                    {
                        bruker.enhetskodeModulusBarnListe.Add(GetEnhetskode(distrikt.DisDistriktskode));
                    }
                    brukere.Add(bruker);
                    migrertAntall += 1;
                }
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = brukere.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Bruker> brukerePart = brukere.OrderBy(o => o.brukerId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                        .Take(MaksAntall)
                        .Where(m => (m.SakStatus != "KLR" && m.SakStatus != "BEH") && (m.MynVedtakstype != "OV" && m.MynVedtakstype != "AB") && !((m.MynVedtakstype == "FN" || m.MynVedtakstype == "LA" || m.MynVedtakstype == "TI") && !(m.SakStatus == "GOD" || m.SakStatus == "AVS" || m.SakStatus == "BOR")))
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
                    if (!mappings.IsOwner(saksJournal.KliLoepenr))
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

                    if (mynVedtakstype == "FN" && (lovHovedParagraf.StartsWith("4-6") || lovHovedParagraf.StartsWith("4-25")))
                    {
                        mynVedtakstype = "AV";
                    }
                    if ((saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR")
                        && (mynVedtakstype == "FN" || mynVedtakstype == "LA" || mynVedtakstype == "TI"))
                    {
                        if (lovHovedParagraf == "4-4,3." || lovHovedParagraf == "4-4,3.1" || lovHovedParagraf == "4-4,3.2" || lovHovedParagraf == "4-4,3.3"
                            || (lovHovedParagraf == "4-4,5." && (lovJmfParagraf1 == "4-24" || lovJmfParagraf2 == "4-24"))
                            || lovHovedParagraf == "§ 3-4" || lovHovedParagraf == "§ 3-5" || (lovHovedParagraf == "§ 3-5" && ((!string.IsNullOrEmpty(lovJmfParagraf1) && lovJmfParagraf1.StartsWith("§ 6-2")) || (!string.IsNullOrEmpty(lovJmfParagraf2) && lovJmfParagraf2.StartsWith("§ 6-2")))))
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
                        else if (lovHovedParagraf == "4-8,2." || lovHovedParagraf == "4-8,3." || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-12"))
                            || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 5-1")) || lovHovedParagraf == "§ 4-4"
                            || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 4-2")))
                        {
                            vedtak.vedtakstype = "OMSORGSOVERTAKELSE";
                        }
                        else if (lovHovedParagraf == "4-24" || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 6-2")) || lovHovedParagraf == "§ 8-4")
                        {
                            vedtak.vedtakstype = "ADFERDSTILTAK";
                        }
                        else if (lovHovedParagraf == "4-20" || lovHovedParagraf == "§ 5-8" || lovHovedParagraf == "§ 5-10" || lovHovedParagraf == "§ 5-11")
                        {
                            vedtak.vedtakstype = "FRATAKELSE_AV_FORELDREANSVAR._ADOPSJON";
                        }
                        else if (lovHovedParagraf == "4-19" || lovHovedParagraf == "§ 7-2,3.")
                        {
                            vedtak.vedtakstype = "SAMVÆRSRETT/SKJULT_ADRESSE";
                        }
                        else if (lovHovedParagraf == "4-21" || lovHovedParagraf == "§ 5-7")
                        {
                            vedtak.vedtakstype = "OPPHEVING_AV_VEDTAK_OM_OMSORGSOVERTAGELSE";
                        }
                        else if (lovHovedParagraf == "4-29" || lovHovedParagraf == "4-29,2." || lovHovedParagraf == "§ 6-6" || lovHovedParagraf == "§ 4-5")
                        {
                            vedtak.vedtakstype = "MENNESKEHANDEL";
                        }
                    }
                    if (!string.IsNullOrEmpty(saksJournal.LovHovedParagraf))
                    {
                        vedtak.lovhjemmel = mappings.GetModulusLovhjemmel(saksJournal.LovHovedParagraf, saksJournal.SakAar, saksJournal.SakIverksattdato);
                    }
                    if (!string.IsNullOrEmpty(saksJournal.LovJmfParagraf1))
                    {
                        if (!string.IsNullOrEmpty(vedtak.lovhjemmel))
                        {
                            vedtak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf1, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                        else
                        {
                            vedtak.lovhjemmel = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf1, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(saksJournal.LovJmfParagraf2))
                    {
                        if (!string.IsNullOrEmpty(vedtak.jfLovhjemmelNr1))
                        {
                            vedtak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf2, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                        else
                        {
                            vedtak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf2, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(saksJournal.SbhInitialer))
                    {
                        vedtak.saksbehandlerId = AddBydel(saksJournal.SbhInitialer);
                    }
                    if (!string.IsNullOrEmpty(saksJournal.SbhAvgjortavInitialer))
                    {
                        vedtak.godkjentAvSaksbehandlerId = AddBydel(saksJournal.SbhAvgjortavInitialer);
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
                    if (saksJournal.SakAvgjortdato.HasValue)
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
                    if (saksJournal.SakStatus == "AVS")
                    {
                        vedtak.avsluttetStatusDato = saksJournal.SakAvgjortdato;
                    }
                    else if (saksJournal.SakStatus == "BOR")
                    {
                        vedtak.bortfaltStatusDato = saksJournal.SakBortfaltdato;
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
                int antallEntiteter = vedtaksliste.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Vedtak> vedtakslistePart = vedtaksliste.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                    else if (lovHovedParagraf.StartsWith("4-4") || lovJmfParagraf1.StartsWith("4-4") || lovJmfParagraf2.StartsWith("4-4") || lovHovedParagraf == "1-3" || lovJmfParagraf1 == "1-3" || lovJmfParagraf2 == "1-3")
                    {
                        if (sakStatus == "GOD" || sakStatus == "BOR" || sakStatus == "OHV")
                        {
                            aktivitetsUnderType = "VEDTAK_OM_TILTAK";
                        }
                    }
                    else if (lovHovedParagraf.StartsWith("4-19") || lovJmfParagraf1.StartsWith("4-19") || lovJmfParagraf2.StartsWith("4-19"))
                    {
                        aktivitetsUnderType = "ADRESSESPERRE";
                    }
                    if (lovHovedParagraf == "4-17" || lovJmfParagraf1 == "4-17" || lovJmfParagraf2 == "4-17")
                    {
                        aktivitetsUnderType = "FLYTTING";
                    }

                    if (lovHovedParagraf == "fvl. 19" || lovJmfParagraf1 == "fvl. 19" || lovJmfParagraf2 == "fvl. 19" || lovHovedParagraf == "19a" || lovJmfParagraf1 == "19a" || lovJmfParagraf2 == "19a")
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
                    break;
                case "FM":
                    aktivitetsUnderType = "USPESIFISERT_VEDTAK";
                    break;
                case "FN":
                case "LA":
                case "TI":
                    aktivitetsUnderType = "VEDTAK_FRA_RETTSINSTANSER";
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
                        .Take(MaksAntall)
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
                    if (tiltak.planlagtTilDato.HasValue && tiltak.planlagtTilDato < tiltak.planlagtFraDato)
                    {
                        tiltak.planlagtTilDato = tiltak.planlagtFraDato;
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
                    if (!tiltak.bortfaltDato.HasValue && tiltak.avsluttetDato.HasValue)
                    {
                        string lovHovedParagraf = tiltakFamilia.LovHovedParagraf;
                        if (!string.IsNullOrEmpty(lovHovedParagraf))
                        {
                            lovHovedParagraf = lovHovedParagraf.Trim();
                            if (lovHovedParagraf.StartsWith("4-12") || lovHovedParagraf == "4-8,2" || lovHovedParagraf == "4-8,3.")
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
                                            if (tiltakFamilia.TilHovedgrunnavsluttet == "4")
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
                                    saksbehandlerId = AddBydel(adresse.SbhEndretav),
                                    utfortAvId = AddBydel(adresse.SbhEndretav)
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
                                if (adresse.PahFraSpesifiser != null && string.IsNullOrEmpty(adresse.PahFraSpesifiser.Trim()))
                                {
                                    tiltak.arsakFlyttingFraPresisering = adresse.PahFraSpesifiser.Trim();
                                }
                                if (adresse.PahTilSpesifiser != null && string.IsNullOrEmpty(adresse.PahTilSpesifiser.Trim()))
                                {
                                    tiltak.presiseringAvBosted = adresse.PahTilSpesifiser.Trim();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovHovedParagraf))
                    {
                        tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovHovedParagraf, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf1))
                    {
                        if (!string.IsNullOrEmpty(tiltak.lovhjemmel))
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                        else
                        {
                            tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf2))
                    {
                        if (!string.IsNullOrEmpty(tiltak.jfLovhjemmelNr1))
                        {
                            tiltak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                        else
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                    }
                    string tiltakstype = tiltakFamilia.TttTiltakstype?.Trim();
                    if (tiltakstype == "14" || tiltakstype == "15" || tiltakstype == "16" || tiltakstype == "17" || tiltakstype == "18" || tiltakstype == "103" || tiltakstype == "104" || tiltakstype == "105" || tiltakstype == "106" || tiltakstype == "107" || tiltakstype == "108" || tiltakstype == "109")
                    {
                        if (tiltakFamilia.TilIverksattdato.HasValue)
                        {
                            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                            {
                                List<FaKlientplassering> klientplasseringer = await context.FaKlientplasserings
                                    .Where(m => m.KliLoepenr == tiltakFamilia.KliLoepenr && m.KplBorhos == "6").ToListAsync();
                                FaKlientplassering klientplassering = klientplasseringer.OrderBy(o => Math.Abs((o.KplFradato - tiltakFamilia.TilIverksattdato.Value).TotalDays)).FirstOrDefault();
                                if (klientplassering != null)
                                {
                                    string kommunenr = "";
                                    if (klientplassering.KomKommunenr < 1000)
                                    {
                                        kommunenr = $"0{klientplassering.KomKommunenr}";
                                    }
                                    else
                                    {
                                        kommunenr = klientplassering.KomKommunenr.ToString();
                                    }
                                    if (mappings.IsValidKommunenummer(kommunenr))
                                    {
                                        tiltak.tilsynskommunenummer = kommunenr;
                                    }
                                    else
                                    {
                                        tiltak.tilsynskommunenummer = "0301";
                                    }
                                }
                                else
                                {
                                    tiltak.tilsynskommunenummer = "0301";
                                }
                            }
                        }
                    }
                    FaEngasjementsavtale engasjementsavtale = null;
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        engasjementsavtale = await context.FaEngasjementsavtales.Include(m => m.ForLoepenrNavigation.ForLoepenrNavigation).Where(e => e.TilLoepenr == tiltakFamilia.TilLoepenr && e.DokLoepenr.HasValue && e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                              && (e.EngTildato >= FirstInYearOfMigration || e.EngStoppetdato >= FirstInYearOfMigration)).FirstOrDefaultAsync();
                    }
                    if (engasjementsavtale != null)
                    {
                        Aktivitet aktivitet = new()
                        {
                            aktivitetId = AddBydel(engasjementsavtale.EngLoepenr.ToString(), "ODA"),
                            sakId = $"{GetActorId(engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation)}__OPP",
                            aktivitetsType = "OPPDRAGSTAKER_AVTALE",
                            aktivitetsUnderType = "ANNET",
                            hendelsesdato = engasjementsavtale.EngAvgjortdato,
                            status = "UTFØRT",
                            tittel = "Oppdragstakeravtale",
                            notat = "Se dokument",
                            utfortDato = engasjementsavtale.EngAvgjortdato,
                            tiltaksId = tiltak.tiltakId
                        };
                        if (!string.IsNullOrEmpty(engasjementsavtale.SbhInitialer))
                        {
                            aktivitet.saksbehandlerId = AddBydel(engasjementsavtale.SbhInitialer);
                            aktivitet.utfortAvId = AddBydel(engasjementsavtale.SbhInitialer);
                        }
                        oppdragstakeravtaleAktiviteter.Add(aktivitet);
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = engasjementsavtale.DokLoepenr.Value,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.saksbehandlerId
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                    }
                    tiltaksliste.Add(tiltak);
                    migrertAntall += 1;
                }
                //TODO Tiltak - Oppdragsavtaleaktiviteter på eksisterende Oppdragstakersaker ikke foreløpig støttet av Modulus Barn
                await GetDocumentsAsync(worker, "Oppdragsavtaleaktiviteter", documentsIncluded);
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = oppdragstakeravtaleAktiviteter.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> oppdragstakeravtaleAktiviteterPart = oppdragstakeravtaleAktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(oppdragstakeravtaleAktiviteterPart, GetJsonFileName("aktiviteter", $"Oppdragsavtaleaktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                await WriteFileAsync(flyttingMedFosterhjemAktiviteter.GroupBy(c => c.aktivitetId).Select(s => s.First()), GetJsonFileName("aktiviteter", "FlyttingMedFosterhjemAktiviteter"));

                toSkip = 0;
                fileNumber = 1;
                antallEntiteter = tiltaksliste.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Tiltak> tiltakslistePart = tiltaksliste.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                        .Take(MaksAntall)
                        .Where(m => m.TtpSlettet == 0 && m.PtyPlankode != "8")
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
                        plan.lovhjemmel = mappings.GetModulusLovhjemmel(planFamilia.PtyPlankodeNavigation.PtyLovhjemmel, null, null);
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
                                break;
                            case "4":
                                plan.planType = "PLAN_FOR_TILTAK_-_FORELØPIG_OMSORGSPLAN";
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
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
                                    plan.lovhjemmel = "Bvl._§_8-3";
                                }
                                break;
                            case "6":
                                plan.planType = "PLAN_FOR_FREMTIDIG_TILTAK_-_ETTERVERN";
                                plan.barnetsBehovOverTid = null;
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
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
                    plan.planStatus = "UNDER_ARBEID";
                    if (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato < planFamilia.TtpTildato)
                    {
                        plan.planStatus = "STOPPET";
                    }
                    else
                    {
                        if ((planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato == planFamilia.TtpTildato) || (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpAvsluttdato < DateTime.Now))
                        {
                            plan.planStatus = "AVSLUTTET";
                        }
                        else
                        {
                            if (planFamilia.TtpFerdigdato.HasValue)
                            {
                                plan.planStatus = "FERDIGSTILT";
                            }
                        }
                    }
                    if (plan.planStatus == "UNDER_ARBEID")
                    {
                        continue;
                    }
                    if (planFamilia.DokLoepenr.HasValue)
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
                                utfortAvId = AddBydel(planFamilia.SbhInitialer)
                            };
                            aktiviteter.Add(aktivitet);

                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
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
                                        hendelsesdato = tiltaksEvaluering.EvaFerdigdato,
                                        status = "UTFØRT",
                                        tittel = "Evaluering",
                                        notat = "Se dokument",
                                        utfortDato = tiltaksEvaluering.EvaUtfoertdato,
                                        utfortAvId = AddBydel(tiltaksEvaluering.SbhInitialer)
                                    };
                                    aktiviteter.Add(aktivitet);

                                    DocumentToInclude documentToInclude = new()
                                    {
                                        dokLoepenr = dokument.DokLoepenr,
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
                    planer.Add(plan);
                    migrertAntall += 1;
                }
                await GetDocumentsAsync(worker, "Plandokumenter", documentsIncluded, false);
                int toSkip = 0;
                int fileNumber = 1;
                int antallEntiteter = aktiviteter.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Plandokumentaktiviteter{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                antallEntiteter = planer.Count;
                while (antallEntiteter > toSkip)
                {
                    List<Plan> planerPart = planer.OrderBy(o => o.planId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
                //TODO Adminitrative belutninger overføre fra aktivitet til vedtak
                //statusText += $"Antall aktiviteter vedtak administrative beslutninger: {await GetVedtakAdministrativBeslutningerAktiviteterAsync(worker)}" + Environment.NewLine;
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
                    .Take(MaksAntall)
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
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value))
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
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhRegistrertav);
                }
                if (postjournal.PosAar >= FirstInYearOfMigration.Year)
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
                                (m.TilFradato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilFradato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddBydel(tiltak.TilLoepenr.ToString(), "TIL");
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
                    .Take(MaksAntall)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "KK" || p.PosBrevtype == "AS" || p.PosBrevtype == "AN" || p.PosBrevtype == "RF" || p.PosBrevtype == "RA" || p.PosBrevtype == "BR" || p.PosBrevtype == "TU" || p.PosBrevtype == "RS" || p.PosBrevtype == "RK" || p.PosBrevtype == "RM" || p.PosBrevtype == "RV" || p.PosBrevtype == "X"))
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
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value))
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
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhRegistrertav);
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
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
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
                    .Take(MaksAntall)
                    .Where(p => p.PosFerdigdato != null && p.PosSlettet == 1)
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
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhRegistrertav);
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
                    .Take(MaksAntall)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "OB"))
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
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhRegistrertav);
                }
                if (postjournal.PosAar >= FirstInYearOfMigration.Year)
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
                                (m.TilFradato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilFradato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddBydel(tiltak.TilLoepenr.ToString(), "TIL");
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
                    .Take(MaksAntall)
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
                if (!mappings.IsOwner(postjournal.KliLoepenr.Value))
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
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhInitialer);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhInitialer);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddBydel(postjournal.SbhRegistrertav);
                    aktivitet.utfortAvId = AddBydel(postjournal.SbhRegistrertav);
                }
                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
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
        private async Task<int> GetVedtakAdministrativBeslutningerAktiviteterAsync(BackgroundWorker worker)
        {
            List<FaSaksjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            int totalAntall = 0;
            int migrertAntall = 0;

            using (var context = new FamiliaDBContext(ConnectionStringFamilia))
            {
                rawData = await context.FaSaksjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(KlientSakJournalFilter())
                    .OrderBy(o => o.KliLoepenrNavigation.KliLoepenr)
                    .Take(MaksAntall)
                    .Where(m => (m.SakStatus != "KLR" && m.SakStatus != "BEH") && (m.MynVedtakstype == "OV" || m.MynVedtakstype == "AB"))
                    .ToListAsync();
                totalAntall = rawData.Count;
            }
            List<Aktivitet> aktiviteter = new();
            int antall = 0;
            foreach (var saksJournal in rawData)
            {
                antall += 1;

                if (antall % 10 == 0)
                {
                    worker.ReportProgress(0, $"Behandler uttrekk vedtak administrative beslutninger ({antall} av {totalAntall})...");
                }
                if (!mappings.IsOwner(saksJournal.KliLoepenr))
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED"),
                    sakId = GetSakId(saksJournal.KliLoepenr.ToString()),
                    aktivitetsType = "ADMINISTRATIV_BESLUTNING",
                    aktivitetsUnderType = "INGEN",
                    status = "UTFØRT",
                    hendelsesdato = saksJournal.SakIverksattdato,
                    saksbehandlerId = AddBydel(saksJournal.SbhInitialer),
                    tittel = saksJournal.SakEmne,
                    utfortAvId = AddBydel(saksJournal.SbhAvgjortavInitialer),
                    utfortDato = saksJournal.SakAvgjortdato,
                    notat = "Se dokument"
                };

                aktiviteter.Add(aktivitet);
                migrertAntall += 1;
                if (saksJournal.PosAar.HasValue && saksJournal.PosLoepenr.HasValue)
                {
                    FaPostjournal postJournal = await GetPostJournalAsync(saksJournal.PosAar, saksJournal.PosLoepenr);
                    if (postJournal != null && postJournal.DokLoepenr.HasValue)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = postJournal.DokLoepenr.Value,
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
            await GetDocumentsAsync(worker, "VedtakAdministrativeBeslutninger", documentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"VedtakAdministrativeBeslutninger{fileNumber}"));
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
                    .Take(MaksAntall)
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
                if (!mappings.IsOwner(journal.KliLoepenr.Value))
                {
                    continue;
                }
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddBydel(journal.JouLoepenr.ToString(), "JOU"),
                    sakId = GetSakId(journal.KliLoepenr.ToString()),
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = AddBydel(journal.SbhInitialer),
                    tittel = journal.JouEmne,
                    utfortAvId = AddBydel(journal.SbhInitialer),
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
                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && d.DokLoepenr == journal.DokLoepenr.Value).FirstOrDefaultAsync();
                    }
                    if (dokument != null)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = dokument.DokLoepenr,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            merknadInnsyn = merknadInnsyn
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
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
                    .Take(MaksAntall)
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
                if (!mappings.IsOwner(journal.KliLoepenr.Value))
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
                    saksbehandlerId = AddBydel(journal.SbhInitialer),
                    tittel = journal.JouEmne,
                    utfortAvId = AddBydel(journal.SbhInitialer),
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
                    .Take(MaksAntall)
                    .Where(m => m.PtyPlankode == "8")
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
                    saksbehandlerId = AddBydel(plan.SbhInitialer),
                    utfortAvId = AddBydel(plan.SbhRegistrertav),
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
        private async Task GetDataTidligereBydelerAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, List<TidligereAvdeling> tidligereAvdelinger)
        {
            try
            {
                foreach (var tidligereBydel in tidligereAvdelinger)
                {
                    await GetInnbyggereTidligereBydelAsync(fodselsDato, personNummer, tidligereBydel.avdelingId);
                    await GetOrganisasjonerTidligereBydelAsync(fodselsDato, personNummer, tidligereBydel.avdelingId);
                    await GetBarnetsNettverkTidligereBydelAsync(fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                    await GetMeldingerTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                    await GetPlanerTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                    await GetUndersokelserTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                    await GetVedtakTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                    await GetTiltakTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                    await GetAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, tidligereBydel.avdelingId);
                }
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task GetInnbyggereTidligereBydelAsync(DateTime fodselsDato, Decimal personNummer, string bydel)
        {
            try
            {
                List<FaForbindelser> forbindelser;
                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    var rollerInkludert = new string[] { "MOR", "FAR", "SØS", "FMO", "FFA", "FAM", "VRG", "BRH", "BSH", "FSA" };
                    forbindelser = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation)
                        .Where(f => f.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && f.KliLoepenrNavigation.KliPersonnr == personNummer && rollerInkludert.Contains(f.KtkRolle) && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForFoedselsnummer) || f.ForLoepenrNavigation.ForDnummer.HasValue))
                        .Select(m => m.ForLoepenrNavigation)
                        .Distinct()
                        .ToListAsync();
                }

                List<Innbygger> innbyggere = new();

                foreach (var forbindelse in forbindelser)
                {
                    Innbygger innbygger = new()
                    {
                        actorId = GetActorId(forbindelse),
                        fornavn = forbindelse.ForFornavn?.Trim(),
                        etternavn = forbindelse.ForEtternavn?.Trim(),
                        sivilstand = "UOPPGITT",
                        kontonummer = forbindelse.ForKontonummer?.Trim(),
                        foedeland = mappings.GetLand(forbindelse.NasKodenr),
                        deaktiver = false
                    };
                    if (forbindelse.ForRegistrertdato.HasValue)
                    {
                        innbygger.registrertDato = forbindelse.ForRegistrertdato.Value;
                    }
                    else
                    {
                        innbygger.registrertDato = LastDateNoMigration;
                    }
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
                        innbygger.fodselDato = new DateTime(year, int.Parse(forbindelse.ForFoedselsnummer.Substring(2, 2)), int.Parse(forbindelse.ForFoedselsnummer[..2]));
                    }
                    else
                    {
                        if (forbindelse.ForDnummer.HasValue)
                        {
                            innbygger.fodselsnummer = forbindelse.ForDnummer.Value.ToString();
                        }
                    }
                    bool hovetelefonBestemt = false;
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonmobil))
                    {
                        AktørTelefonnummer aktørTelefonnummerMobil = new()
                        {
                            telefonnummerType = "PRIVAT",
                            telefonnummer = forbindelse.ForTelefonmobil?.Trim(),
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
                            telefonnummer = forbindelse.ForTelefonprivat?.Trim()
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
                            telefonnummer = forbindelse.ForTelefonarbeid?.Trim()
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
                            adresseId = AddSpecificBydel(forbindelse.ForLoepenr.ToString(), "ADR", bydel),
                            adresseType = "POSTADRESSE",
                            postnummer = forbindelse.PnrPostnr?.Trim(),
                            hovedadresse = true
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
                int migrertAntall = innbyggere.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Innbygger> innbyggerePart = innbyggere.OrderBy(o => o.actorId).OrderBy(o => o.etternavn).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(innbyggerePart, GetJsonFileName("innbygger", $"Innbyggere{bydel}_{guid}_{fileNumber}"));
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
        private async Task GetOrganisasjonerTidligereBydelAsync(DateTime fodselsDato, Decimal personNummer, string bydel)
        {
            try
            {
                List<FaForbindelser> rawData;

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation)
                        .Where(f => f.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && f.KliLoepenrNavigation.KliPersonnr == personNummer && (!string.IsNullOrEmpty(f.ForLoepenrNavigation.ForOrganisasjonsnr) && f.ForLoepenrNavigation.ForOrganisasjonsnr.Length != 9 && f.ForLoepenrNavigation.ForOrganisasjonsnr.IndexOf(" ") == -1))
                        .Select(m => m.ForLoepenrNavigation)
                        .Distinct()
                        .ToListAsync();
                }
                List<Organisasjon> organisasjoner = new();

                foreach (var forbindelse in rawData)
                {
                    Organisasjon organisasjon = new()
                    {
                        actorId = GetActorId(forbindelse),
                        organisasjonsnummer = forbindelse.ForOrganisasjonsnr?.Trim(),
                        kontonummer = forbindelse.ForKontonummer?.Trim(),
                        deaktiver = false
                    };
                    organisasjon.eksternId = organisasjon.actorId;

                    if (forbindelse.FotIdents != null)
                    {
                        foreach (FaForbindelsestyper type in forbindelse.FotIdents)
                        {
                            GetOrganisasjonsKategori(organisasjon, type);
                        }
                    }
                    if (forbindelse.ForBetalingsmaate == "L")
                    {
                        organisasjon.leverandørAvTiltak = "JA_ALM_TILTAK";
                    }

                    bool hovetelefonBestemt = false;
                    if (!string.IsNullOrEmpty(forbindelse.ForTelefonarbeid))
                    {
                        AktørTelefonnummer aktørTelefonnummerArbeid = new()
                        {
                            telefonnummerType = "ANNET",
                            telefonnummer = forbindelse.ForTelefonarbeid?.Trim(),
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
                            telefonnummer = forbindelse.ForTelefonmobil?.Trim()
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
                            telefonnummer = forbindelse.ForTelefonprivat?.Trim()
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
                            adresseId = AddSpecificBydel(forbindelse.ForLoepenr.ToString(), "ADR", bydel),
                            adresseType = "POSTADRESSE",
                            postnummer = forbindelse.PnrPostnr?.Trim(),
                            hovedadresse = true
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
                int migrertAntall = organisasjonerDistinct.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Organisasjon> organisasjonerPart = organisasjonerDistinct.OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(organisasjonerPart, GetJsonFileName("organisasjon", $"Organisasjoner{bydel}_{guid}_{fileNumber}"));
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
        private async Task GetBarnetsNettverkTidligereBydelAsync(DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            try
            {
                List<FaKlienttilknytning> rawData;

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    var rollerInkludert = new string[] { "MOR", "FAR", "SØS", "FMO", "FFA", "FAM", "VRG", "BRH", "BSH", "FSA" };
                    rawData = await context.FaKlienttilknytnings.Include(m => m.ForLoepenrNavigation).Include(m => m.KliLoepenrNavigation)
                        .Where(k => k.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && k.KliLoepenrNavigation.KliPersonnr == personNummer && ((!string.IsNullOrEmpty(k.ForLoepenrNavigation.ForFoedselsnummer) || k.ForLoepenrNavigation.ForDnummer.HasValue) || (rollerInkludert.Contains(k.KtkRolle))))
                        .ToListAsync();
                }

                List<BarnetsNettverk> forbindeler = new();

                foreach (var klientTilknytning in rawData)
                {
                    BarnetsNettverk forbindelse = new()
                    {
                        sakId = sakId,
                        actorId = GetActorId(klientTilknytning.ForLoepenrNavigation),
                        kommentar = klientTilknytning.KtkMerknad?.Trim()
                    };
                    GetNettverksRolle(klientTilknytning, forbindelse);
                    if (klientTilknytning.KtkForesatt == 1)
                    {
                        forbindelse.foresatt = true;
                    }
                    else
                    {
                        forbindelse.foresatt = false;
                    }
                    forbindeler.Add(forbindelse);
                }
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = forbindeler.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<BarnetsNettverk> forbindelerPart = forbindeler.OrderBy(o => o.sakId).OrderBy(o => o.actorId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(forbindelerPart, GetJsonFileName("barnetsNettverk", $"BarnetsNettverk{bydel}_{guid}_{fileNumber}"));
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
        private async Task GetMeldingerTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                        sakId = sakId,
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
                    if (meldingFamilia.DokLoepenr.HasValue)
                    {
                        FaDokumenter dokument;
                        using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                        {
                            dokument = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && meldingFamilia.DokLoepenr.HasValue && d.DokLoepenr == meldingFamilia.DokLoepenr.Value).FirstOrDefaultAsync();
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
                int migrertAntall = meldinger.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Melding> meldingerPart = meldinger.OrderBy(o => o.meldingId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        public async Task GetPlanerTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            try
            {
                List<FaTiltaksplan> rawData;
                List<Aktivitet> aktiviteter = new();
                List<DocumentToInclude> documentsIncluded = new();

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaTiltaksplans.Include(t => t.FaTiltaksplanevalueringers).Include(x => x.KliLoepenrNavigation).Include(y => y.PtyPlankodeNavigation)
                        .Where(m => m.TtpSlettet == 0 && m.PtyPlankode != "8" && (m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer))
                        .ToListAsync();
                }
                List<Plan> planer = new();
                foreach (var planFamilia in rawData)
                {
                    Plan plan = new()
                    {
                        planId = AddSpecificBydel(planFamilia.TtpLoepenr.ToString(), "PLA", bydel),
                        sakId = sakId,
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
                        plan.lovhjemmel = mappings.GetModulusLovhjemmel(planFamilia.PtyPlankodeNavigation.PtyLovhjemmel, null, null);
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
                                break;
                            case "4":
                                plan.planType = "PLAN_FOR_TILTAK_-_FORELØPIG_OMSORGSPLAN";
                                plan.bostedOgVarighet = null;
                                plan.skolegangDagtilbud = null;
                                plan.tjenesterHjelpeapparatet = null;
                                plan.planForFlytting = null;
                                plan.nettverk = null;
                                plan.tidsperspektiv = null;
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
                                    plan.lovhjemmel = "Bvl._§_8-3";
                                }
                                break;
                            case "6":
                                plan.planType = "PLAN_FOR_FREMTIDIG_TILTAK_-_ETTERVERN";
                                plan.barnetsBehovOverTid = null;
                                plan.varighetOgTilbakeforing = null;
                                plan.plasseringsted = null;
                                plan.intensjonForKontaktMedFamilie = null;
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
                    plan.planStatus = "UNDER_ARBEID";
                    if (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato < planFamilia.TtpTildato)
                    {
                        plan.planStatus = "STOPPET";
                    }
                    else
                    {
                        if ((planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpTildato.HasValue && planFamilia.TtpAvsluttdato == planFamilia.TtpTildato) || (planFamilia.TtpAvsluttdato.HasValue && planFamilia.TtpAvsluttdato < DateTime.Now))
                        {
                            plan.planStatus = "AVSLUTTET";
                        }
                        else
                        {
                            if (planFamilia.TtpFerdigdato.HasValue)
                            {
                                plan.planStatus = "FERDIGSTILT";
                            }
                        }
                    }
                    if (plan.planStatus == "UNDER_ARBEID")
                    {
                        continue;
                    }
                    if (planFamilia.DokLoepenr.HasValue)
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
                                utfortAvId = AddSpecificBydel(planFamilia.SbhInitialer, bydel)
                            };
                            aktiviteter.Add(aktivitet);

                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
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
                                        hendelsesdato = tiltaksEvaluering.EvaFerdigdato,
                                        status = "UTFØRT",
                                        tittel = "Evaluering",
                                        notat = "Se dokument",
                                        utfortDato = tiltaksEvaluering.EvaUtfoertdato,
                                        utfortAvId = AddSpecificBydel(tiltaksEvaluering.SbhInitialer, bydel)
                                    };
                                    aktiviteter.Add(aktivitet);

                                    DocumentToInclude documentToInclude = new()
                                    {
                                        dokLoepenr = dokument.DokLoepenr,
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
                    planer.Add(plan);
                }
                await GetDocumentsAsync(worker, $"Plandokumenter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, false, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = aktiviteter.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Plandokumentaktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = planer.Count;
                guid = Guid.NewGuid().ToString().Replace('-', '_');
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
        public async Task GetUndersokelserTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer,string sakId, string bydel)
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
                        sakId = sakId,
                        startDato = undersøkelse.UndStartdato
                    };
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
                            saksbehandlerId = AddSpecificBydel(undersøkelse.SbhInitialer, bydel),
                            tittel = "Beslutning om utvidet undersøkelsestid",
                            utfortAvId = AddSpecificBydel(undersøkelse.SbhInitialer, bydel),
                            notat = undersøkelse.Und6mndbegrunnelse,
                            fristDato = undersøkelse.UndFristdato,
                            fristLovpaalagt = true
                        };
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
                        if (undersøkelse.UndFristdato.HasValue)
                        {
                            undersøkelseUtvidelseFristAktivitet.hendelsesdato = undersøkelse.UndFristdato.Value.AddDays(-90);
                            undersøkelseUtvidelseFristAktivitet.utfortDato = undersøkelse.UndFristdato.Value.AddDays(-90);
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
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        FaSaksjournal saksJournal = await context.FaSaksjournals.Where(m => m.MelLoepenr == undersøkelse.MelLoepenr).FirstOrDefaultAsync();
                        if (saksJournal != null)
                        {
                            undersoekelse.vedtakAktivitetId = AddSpecificBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED", bydel);
                        }
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
                                saksbehandlerId = AddSpecificBydel(undersøkelse.SbhInitialer, bydel),
                                tittel = "Undersøkelsesplan",
                                utfortAvId = AddSpecificBydel(undersøkelse.SbhInitialer, bydel),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdatoUplan,
                                utfortDato = undersøkelse.UndFerdigdatoUplan
                            };
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
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
                                saksbehandlerId = AddSpecificBydel(undersøkelse.SbhInitialer, bydel),
                                tittel = "Sluttrapport undersøkelse",
                                utfortAvId = AddSpecificBydel(undersøkelse.SbhInitialer, bydel),
                                notat = "Se dokument",
                                hendelsesdato = undersøkelse.UndFerdigdato,
                                utfortDato = undersøkelse.UndFerdigdato
                            };
                            DocumentToInclude documentToInclude = new()
                            {
                                dokLoepenr = dokument.DokLoepenr,
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
                await GetTextDocumentsAsync(worker, textDocumentsIncluded);
                await GetDocumentsAsync(worker, $"Undersøkelser{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = undersøkelsesAktiviteter.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> undersøkelsesAktiviteterPart = undersøkelsesAktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(undersøkelsesAktiviteterPart, GetJsonFileName("aktiviteter", $"UndersokelsesAktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = undersøkelser.Count;
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
        public async Task GetVedtakTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                        && (m.MynVedtakstype != "OV" && m.MynVedtakstype != "AB")
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

                    if (mynVedtakstype == "FN" && (lovHovedParagraf.StartsWith("4-6") || lovHovedParagraf.StartsWith("4-25")))
                    {
                        mynVedtakstype = "AV";
                    }
                    if ((saksJournal.SakAvgjortetat == "FN" || saksJournal.SakAvgjortetat == "LR" || saksJournal.SakAvgjortetat == "TR")
                        && (mynVedtakstype == "FN" || mynVedtakstype == "LA" || mynVedtakstype == "TI"))
                    {
                        if (lovHovedParagraf == "4-4,3." || lovHovedParagraf == "4-4,3.1" || lovHovedParagraf == "4-4,3.2" || lovHovedParagraf == "4-4,3.3"
                            || (lovHovedParagraf == "4-4,5." && (lovJmfParagraf1 == "4-24" || lovJmfParagraf2 == "4-24"))
                            || lovHovedParagraf == "§ 3-4" || lovHovedParagraf == "§ 3-5" || (lovHovedParagraf == "§ 3-5" && ((!string.IsNullOrEmpty(lovJmfParagraf1) && lovJmfParagraf1.StartsWith("§ 6-2")) || (!string.IsNullOrEmpty(lovJmfParagraf2) && lovJmfParagraf2.StartsWith("§ 6-2")))))
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
                        else if (lovHovedParagraf == "4-8,2." || lovHovedParagraf == "4-8,3." || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("4-12"))
                            || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 5-1")) || lovHovedParagraf == "§ 4-4"
                            || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 4-2")))
                        {
                            vedtak.vedtakstype = "OMSORGSOVERTAKELSE";
                        }
                        else if (lovHovedParagraf == "4-24" || (!string.IsNullOrEmpty(lovHovedParagraf) && lovHovedParagraf.StartsWith("§ 6-2")) || lovHovedParagraf == "§ 8-4")
                        {
                            vedtak.vedtakstype = "ADFERDSTILTAK";
                        }
                        else if (lovHovedParagraf == "4-20" || lovHovedParagraf == "§ 5-8" || lovHovedParagraf == "§ 5-10" || lovHovedParagraf == "§ 5-11")
                        {
                            vedtak.vedtakstype = "FRATAKELSE_AV_FORELDREANSVAR._ADOPSJON";
                        }
                        else if (lovHovedParagraf == "4-19" || lovHovedParagraf == "§ 7-2,3.")
                        {
                            vedtak.vedtakstype = "SAMVÆRSRETT/SKJULT_ADRESSE";
                        }
                        else if (lovHovedParagraf == "4-21" || lovHovedParagraf == "§ 5-7")
                        {
                            vedtak.vedtakstype = "OPPHEVING_AV_VEDTAK_OM_OMSORGSOVERTAGELSE";
                        }
                        else if (lovHovedParagraf == "4-29" || lovHovedParagraf == "4-29,2." || lovHovedParagraf == "§ 6-6" || lovHovedParagraf == "§ 4-5")
                        {
                            vedtak.vedtakstype = "MENNESKEHANDEL";
                        }
                    }
                    if (!string.IsNullOrEmpty(saksJournal.LovHovedParagraf))
                    {
                        vedtak.lovhjemmel = mappings.GetModulusLovhjemmel(saksJournal.LovHovedParagraf, saksJournal.SakAar, saksJournal.SakIverksattdato);
                    }
                    if (!string.IsNullOrEmpty(saksJournal.LovJmfParagraf1))
                    {
                        if (!string.IsNullOrEmpty(vedtak.lovhjemmel))
                        {
                            vedtak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf1, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                        else
                        {
                            vedtak.lovhjemmel = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf1, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(saksJournal.LovJmfParagraf2))
                    {
                        if (!string.IsNullOrEmpty(vedtak.jfLovhjemmelNr1))
                        {
                            vedtak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf2, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                        else
                        {
                            vedtak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(saksJournal.LovJmfParagraf2, saksJournal.SakAar, saksJournal.SakIverksattdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(saksJournal.SbhInitialer))
                    {
                        vedtak.saksbehandlerId = AddSpecificBydel(saksJournal.SbhInitialer, bydel);
                    }
                    if (!string.IsNullOrEmpty(saksJournal.SbhAvgjortavInitialer))
                    {
                        vedtak.godkjentAvSaksbehandlerId = AddSpecificBydel(saksJournal.SbhAvgjortavInitialer, bydel);
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
                    if (saksJournal.SakAvgjortdato.HasValue)
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
                    if (saksJournal.SakStatus == "AVS")
                    {
                        vedtak.avsluttetStatusDato = saksJournal.SakAvgjortdato;
                    }
                    else if (saksJournal.SakStatus == "BOR")
                    {
                        vedtak.bortfaltStatusDato = saksJournal.SakBortfaltdato;
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
                int migrertAntall = vedtaksliste.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Vedtak> vedtakslistePart = vedtaksliste.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        public async Task GetTiltakTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            try
            {
                List<FaTiltak> rawData;

                using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                {
                    rawData = await context.FaTiltaks.Include(x => x.KliLoepenrNavigation).Include(c => c.Sak)
                        .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
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
                        sakId = sakId,
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
                    if (tiltak.planlagtTilDato.HasValue && tiltak.planlagtTilDato < tiltak.planlagtFraDato)
                    {
                        tiltak.planlagtTilDato = tiltak.planlagtFraDato;
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
                    if (!tiltak.bortfaltDato.HasValue && tiltak.avsluttetDato.HasValue)
                    {
                        string lovHovedParagraf = tiltakFamilia.LovHovedParagraf;
                        if (!string.IsNullOrEmpty(lovHovedParagraf))
                        {
                            lovHovedParagraf = lovHovedParagraf.Trim();
                            if (lovHovedParagraf.StartsWith("4-12") || lovHovedParagraf == "4-8,2" || lovHovedParagraf == "4-8,3.")
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
                                            if (tiltakFamilia.TilHovedgrunnavsluttet == "4")
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
                                    saksbehandlerId = AddSpecificBydel(adresse.SbhEndretav, bydel),
                                    utfortAvId = AddSpecificBydel(adresse.SbhEndretav, bydel)
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
                                if (adresse.PahFraSpesifiser != null && string.IsNullOrEmpty(adresse.PahFraSpesifiser.Trim()))
                                {
                                    tiltak.arsakFlyttingFraPresisering = adresse.PahFraSpesifiser.Trim();
                                }
                                if (adresse.PahTilSpesifiser != null && string.IsNullOrEmpty(adresse.PahTilSpesifiser.Trim()))
                                {
                                    tiltak.presiseringAvBosted = adresse.PahTilSpesifiser.Trim();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovHovedParagraf))
                    {
                        tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovHovedParagraf, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf1))
                    {
                        if (!string.IsNullOrEmpty(tiltak.lovhjemmel))
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                        else
                        {
                            tiltak.lovhjemmel = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf1, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                    }
                    if (!string.IsNullOrEmpty(tiltakFamilia.LovJmfParagraf2))
                    {
                        if (!string.IsNullOrEmpty(tiltak.jfLovhjemmelNr1))
                        {
                            tiltak.jfLovhjemmelNr2 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                        else
                        {
                            tiltak.jfLovhjemmelNr1 = mappings.GetModulusLovhjemmel(tiltakFamilia.LovJmfParagraf2, tiltakFamilia.SakAar, tiltakFamilia.Sak.SakIverksattdato);
                        }
                    }
                    string tiltakstype = tiltakFamilia.TttTiltakstype?.Trim();
                    if (tiltakstype == "14" || tiltakstype == "15" || tiltakstype == "16" || tiltakstype == "17" || tiltakstype == "18" || tiltakstype == "103" || tiltakstype == "104" || tiltakstype == "105" || tiltakstype == "106" || tiltakstype == "107" || tiltakstype == "108" || tiltakstype == "109")
                    {
                        if (tiltakFamilia.TilIverksattdato.HasValue)
                        {
                            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                            {
                                List<FaKlientplassering> klientplasseringer = await context.FaKlientplasserings
                                    .Where(m => m.KliLoepenr == tiltakFamilia.KliLoepenr && m.KplBorhos == "6").ToListAsync();
                                FaKlientplassering klientplassering = klientplasseringer.OrderBy(o => Math.Abs((o.KplFradato - tiltakFamilia.TilIverksattdato.Value).TotalDays)).FirstOrDefault();
                                if (klientplassering != null)
                                {
                                    string kommunenr = "";
                                    if (klientplassering.KomKommunenr < 1000)
                                    {
                                        kommunenr = $"0{klientplassering.KomKommunenr}";
                                    }
                                    else
                                    {
                                        kommunenr = klientplassering.KomKommunenr.ToString();
                                    }
                                    if (mappings.IsValidKommunenummer(kommunenr))
                                    {
                                        tiltak.tilsynskommunenummer = kommunenr;
                                    }
                                    else
                                    {
                                        tiltak.tilsynskommunenummer = "0301";
                                    }
                                }
                                else
                                {
                                    tiltak.tilsynskommunenummer = "0301";
                                }
                            }
                        }
                    }
                    FaEngasjementsavtale engasjementsavtale = null;
                    using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
                    {
                        engasjementsavtale = await context.FaEngasjementsavtales.Include(m => m.ForLoepenrNavigation.ForLoepenrNavigation).Where(e => e.TilLoepenr == tiltakFamilia.TilLoepenr && e.DokLoepenr.HasValue && e.EngAvgjortdato.HasValue && e.EngStatus != "BOR" && e.EngStatus != "BEH" && e.EngStatus != "KLR"
                              && (e.EngTildato >= FirstInYearOfMigration || e.EngStoppetdato >= FirstInYearOfMigration)).FirstOrDefaultAsync();
                    }
                    if (engasjementsavtale != null)
                    {
                        Aktivitet aktivitet = new()
                        {
                            aktivitetId = AddSpecificBydel(engasjementsavtale.EngLoepenr.ToString(), "ODA", bydel),
                            sakId = $"{GetActorId(engasjementsavtale.ForLoepenrNavigation.ForLoepenrNavigation)}__OPP",
                            aktivitetsType = "OPPDRAGSTAKER_AVTALE",
                            aktivitetsUnderType = "ANNET",
                            hendelsesdato = engasjementsavtale.EngAvgjortdato,
                            status = "UTFØRT",
                            tittel = "Oppdragstakeravtale",
                            notat = "Se dokument",
                            utfortDato = engasjementsavtale.EngAvgjortdato,
                            tiltaksId = tiltak.tiltakId
                        };
                        if (!string.IsNullOrEmpty(engasjementsavtale.SbhInitialer))
                        {
                            aktivitet.saksbehandlerId = AddSpecificBydel(engasjementsavtale.SbhInitialer, bydel);
                            aktivitet.utfortAvId = AddSpecificBydel(engasjementsavtale.SbhInitialer, bydel);
                        }
                        oppdragstakeravtaleAktiviteter.Add(aktivitet);
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = engasjementsavtale.DokLoepenr.Value,
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.saksbehandlerId
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
                    }
                    tiltaksliste.Add(tiltak);
                }
                //TODO Tiltak - Oppdragsavtaleaktiviteter på eksisterende Oppdragstakersaker ikke foreløpig støttet av Modulus Barn
                await GetDocumentsAsync(worker, $"Oppdragsavtaleaktiviteter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = oppdragstakeravtaleAktiviteter.Count;
                string guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> oppdragstakeravtaleAktiviteterPart = oppdragstakeravtaleAktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(oppdragstakeravtaleAktiviteterPart, GetJsonFileName("aktiviteter", $"Oppdragsavtaleaktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = flyttingMedFosterhjemAktiviteter.Count;
                guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Aktivitet> flyttingMedFosterhjemAktiviteterPart = flyttingMedFosterhjemAktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(flyttingMedFosterhjemAktiviteterPart, GetJsonFileName("aktiviteter", $"FlyttingMedFosterhjemAktiviteter{bydel}_{guid}_{fileNumber}"));
                    fileNumber += 1;
                    toSkip += MaxAntallEntiteterPerFil;
                }
                toSkip = 0;
                fileNumber = 1;
                migrertAntall = tiltaksliste.Count;
                guid = Guid.NewGuid().ToString().Replace('-', '_');
                while (migrertAntall > toSkip)
                {
                    List<Tiltak> tiltakslistePart = tiltaksliste.OrderBy(o => o.tiltakId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        public async Task GetAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            try
            {
                await GetTilsynsrapporterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
                await GetInterneSaksforberedelserTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
                //TODO Adminitrative belutninger overføre fra aktivitet til vedtak
                //await GetVedtakAdministrativBeslutningerAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
                await GetJournalAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
                await GetSlettedeJournalAktiviteterTidligereBydelAsync(fodselsDato, personNummer, sakId, bydel);
                await GetIndividuellPlanAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
                await GetPostjournalAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
                await GetSlettedePostjournalAktiviteterTidligereBydelAsync(fodselsDato, personNummer, sakId, bydel);
                await GetOppfølgingAktiviteterTidligereBydelAsync(worker, fodselsDato, personNummer, sakId, bydel);
            }
            catch (Exception ex)
            {
                string message = $"Exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering uttrekk - exception", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async Task GetTilsynsrapporterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sakId,
                    aktivitetsType = "TILSYN",
                    aktivitetsUnderType = "TILSYNSBESØK",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato,
                    lovPaalagt = true
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                }
                if (postjournal.PosAar >= FirstInYearOfMigration.Year)
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
                                (m.TilFradato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilFradato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddSpecificBydel(tiltak.TilLoepenr.ToString(), "TIL", bydel);
                        }
                    }
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
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
            await GetDocumentsAsync(worker, $"Tilsynsrapporter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel:bydel);
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Tilsynsrapporter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetPostjournalAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            List<FaPostjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaPostjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(p => p.PosSlettet != 1 && p.PosUnndrattinnsynIs == 0 && p.PosFerdigdato.HasValue && (p.PosBrevtype == "KK" || p.PosBrevtype == "AS" || p.PosBrevtype == "AN" || p.PosBrevtype == "RF" || p.PosBrevtype == "RA" || p.PosBrevtype == "BR" || p.PosBrevtype == "TU" || p.PosBrevtype == "RS" || p.PosBrevtype == "RK" || p.PosBrevtype == "RM" || p.PosBrevtype == "RV" || p.PosBrevtype == "X"))
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var postjournal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(postjournal.PosAar.ToString() + postjournal.PosLoepenr.ToString(), "AKT", bydel),
                    sakId = sakId,
                    aktivitetsType = "ØVRIG_DOKUMENT",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    notat = "Se dokument",
                    utfortDato = postjournal.PosFerdigdato
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
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
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
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
            await GetDocumentsAsync(worker, $"Postjournalaktiviteter{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel:bydel);
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Postjournalaktiviteter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetSlettedePostjournalAktiviteterTidligereBydelAsync(DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                    sakId = sakId,
                    aktivitetsType = "SLETTET",
                    aktivitetsUnderType = "SLETTET",
                    hendelsesdato = postjournal.PosDato,
                    status = "UTFØRT",
                    tittel = postjournal.PosEmne,
                    utfortDato = postjournal.PosFerdigdato
                };
                if (!string.IsNullOrEmpty(postjournal.SbhInitialer))
                {
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                }
                if (!string.IsNullOrEmpty(postjournal.PosBegrSlettet))
                {
                    aktivitet.notat = $"Årsak: {postjournal.PosBegrSlettet}";
                };
                aktiviteter.Add(aktivitet);
            }
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"SlettedePostjournalaktiviteter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetOppfølgingAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                    sakId = sakId,
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
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                {
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                }
                if (postjournal.PosAar >= FirstInYearOfMigration.Year)
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
                                (m.TilFradato <= postjournal.PosDato))
                            .OrderByDescending(b => b.TilFradato)
                            .FirstOrDefaultAsync();
                        if (tiltak != null)
                        {
                            aktivitet.tiltaksId = AddSpecificBydel(tiltak.TilLoepenr.ToString(), "TIL", bydel);
                        }
                    }
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
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
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Oppfølgingsaktiviteter{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetInterneSaksforberedelserTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                    sakId = sakId,
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
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhInitialer, bydel);
                }
                else if (!string.IsNullOrEmpty(postjournal.SbhRegistrertav))
                { 
                    aktivitet.saksbehandlerId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                    aktivitet.utfortAvId = AddSpecificBydel(postjournal.SbhRegistrertav, bydel);
                }
                aktiviteter.Add(aktivitet);
                if (postjournal.DokLoepenr.HasValue)
                {
                    DocumentToInclude documentToInclude = new()
                    {
                        dokLoepenr = postjournal.DokLoepenr.Value,
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
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"InterneSaksforberedelser{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetVedtakAdministrativBeslutningerAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            List<FaSaksjournal> rawData;
            List<DocumentToInclude> documentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaSaksjournals.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(m => (m.SakStatus != "KLR" && m.SakStatus != "BEH") && (m.MynVedtakstype == "OV" || m.MynVedtakstype == "AB"))
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var saksJournal in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(saksJournal.SakAar.ToString() + "-" + saksJournal.SakJournalnr.ToString(), "VED", bydel),
                    sakId = sakId,
                    aktivitetsType = "ADMINISTRATIV_BESLUTNING",
                    aktivitetsUnderType = "INGEN",
                    status = "UTFØRT",
                    hendelsesdato = saksJournal.SakIverksattdato,
                    saksbehandlerId = AddSpecificBydel(saksJournal.SbhInitialer, bydel),
                    tittel = saksJournal.SakEmne,
                    utfortAvId = AddSpecificBydel(saksJournal.SbhAvgjortavInitialer, bydel),
                    utfortDato = saksJournal.SakAvgjortdato,
                    notat = "Se dokument"
                };

                aktiviteter.Add(aktivitet);
                if (saksJournal.PosAar.HasValue && saksJournal.PosLoepenr.HasValue)
                {
                    FaPostjournal postJournal = await GetPostJournalAsync(saksJournal.PosAar, saksJournal.PosLoepenr);
                    if (postJournal != null && postJournal.DokLoepenr.HasValue)
                    {
                        DocumentToInclude documentToInclude = new()
                        {
                            dokLoepenr = postJournal.DokLoepenr.Value,
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
            await GetDocumentsAsync(worker, $"VedtakAdministrativeBeslutninger{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"VedtakAdministrativeBeslutninger{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetJournalAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                    sakId = sakId,
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = AddSpecificBydel(journal.SbhInitialer, bydel),
                    tittel = journal.JouEmne,
                    utfortAvId = AddSpecificBydel(journal.SbhInitialer, bydel),
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
                            sakId = aktivitet.sakId,
                            tittel = aktivitet.tittel,
                            journalDato = aktivitet.utfortDato,
                            opprettetAvId = aktivitet.utfortAvId,
                            merknadInnsyn = merknadInnsyn
                        };
                        documentToInclude.aktivitetIdListe.Add(aktivitet.aktivitetId);
                        documentsIncluded.Add(documentToInclude);
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
                aktiviteter.Add(aktivitet);
            }
            await GetDocumentsAsync(worker, $"Journaler{bydel}_{Guid.NewGuid().ToString().Replace('-', '_')}", documentsIncluded, bydel: bydel);
            await GetTextDocumentsAsync(worker, textDocumentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"Journaler{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetSlettedeJournalAktiviteterTidligereBydelAsync(DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
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
                    sakId = sakId,
                    aktivitetsType = "SLETTET",
                    aktivitetsUnderType = "SLETTET",
                    status = "UTFØRT",
                    hendelsesdato = journal.JouDatonotat,
                    saksbehandlerId = AddSpecificBydel(journal.SbhInitialer, bydel),
                    tittel = journal.JouEmne,
                    utfortAvId = AddSpecificBydel(journal.SbhInitialer, bydel),
                    utfortDato = journal.JouFerdigdato,
                    notat = journal.JouNotat
                };
                aktiviteter.Add(aktivitet);
            }
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                await WriteFileAsync(aktiviteterPart, GetJsonFileName("aktiviteter", $"SlettedeJournaler{bydel}_{guid}_{fileNumber}"));
                fileNumber += 1;
                toSkip += MaxAntallEntiteterPerFil;
            }
        }
        private async Task GetIndividuellPlanAktiviteterTidligereBydelAsync(BackgroundWorker worker, DateTime fodselsDato, Decimal personNummer, string sakId, string bydel)
        {
            List<FaTiltaksplan> rawData;
            List<DocumentToInclude> documentsIncluded = new();
            List<TextDocumentToInclude> textDocumentsIncluded = new();

            using (var context = new FamiliaDBContext(mappings.GetConnectionstring(bydel, MainDBServer)))
            {
                rawData = await context.FaTiltaksplans.Include(x => x.KliLoepenrNavigation)
                    .Where(m => m.KliLoepenrNavigation.KliFoedselsdato == fodselsDato && m.KliLoepenrNavigation.KliPersonnr == personNummer)
                    .Where(m => m.PtyPlankode == "8")
                    .ToListAsync();
            }
            List<Aktivitet> aktiviteter = new();
            foreach (var plan in rawData)
            {
                Aktivitet aktivitet = new()
                {
                    aktivitetId = AddSpecificBydel(plan.TtpLoepenr.ToString(), "IVP", bydel),
                    sakId = sakId,
                    status = "UTFØRT",
                    aktivitetsType = "ØVRIG_DOKUMENT",
                    aktivitetsUnderType = "INDIVIDUELL_PLAN",
                    tittel = "Individuell plan",
                    hendelsesdato = plan.TtpRegistrertdato,
                    saksbehandlerId = AddSpecificBydel(plan.SbhInitialer, bydel),
                    utfortAvId = AddSpecificBydel(plan.SbhRegistrertav, bydel),
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
            await GetTextDocumentsAsync(worker, textDocumentsIncluded);
            int toSkip = 0;
            int fileNumber = 1;
            int migrertAntall = aktiviteter.Count;
            string guid = Guid.NewGuid().ToString().Replace('-', '_');
            while (migrertAntall > toSkip)
            {
                List<Aktivitet> aktiviteterPart = aktiviteter.OrderBy(o => o.aktivitetId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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

                    using (var context = new FamiliaDBContext(ConnectionStringFamilia))
                    {
                        rawData = await context.FaDokumenters.Where(d => d.DokProdusert == 1 && d.DokDokument != null && partSimpleDocumentsIncluded.Contains(d.DokLoepenr))
                        .Select(d => new InternalDocument
                        {
                            dokLoepenr = d.DokLoepenr,
                            dokMimetype = d.DokMimetype
                        }).ToListAsync();
                        totalNumberExtracted += rawData.Count;
                    }
                    SqlConnection connection = new(ConnectionStringFamilia);
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
                                dokumentId = AddBydel(documentFamilia.dokLoepenr.ToString()),
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
                            SqlCommand commandMigrering = new($"Select Count(*) From Filer Where Filnavn='{document.filId}'", connectionMigrering)
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
                                commandMigrering = new($"Insert Into Filer (FilNavn,Bydel,Dato) Values ('{document.filId}','{Bydelsforkortelse}',GETDATE())", connectionMigrering)
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
                int migrertAntall = documents.Count;
                while (migrertAntall > toSkip)
                {
                    List<Document> documentsPart = documents.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"Dokumenter{fileNumber}"));
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
        private async Task<string> GetTextDocumentsAsync(BackgroundWorker worker, List<TextDocumentToInclude> textDocumentsIncluded)
        {
            try
            {
                worker.ReportProgress(0, $"Starter opprettelse av dokumenter og tekstfiler ...");

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
                        dokumentId = AddBydel(documentFamilia.dokLoepenr.ToString()),
                        filId = AddBydel(documentFamilia.dokLoepenr.ToString()),
                        ferdigstilt = true,
                        opprettetAvId = documentFamilia.opprettetAvId,
                        sakId = documentFamilia.sakId,
                        aktivitetIdListe = documentFamilia.aktivitetIdListe,
                        tittel = documentFamilia.tittel,
                        journalDato = documentFamilia.journalDato,
                        filFormat = "PDF",
                        merknadInnsyn = documentFamilia.merknadInnsyn
                    };
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
                int migrertAntall = documents.Count;
                while (migrertAntall > toSkip)
                {
                    List<Document> documentsPart = documents.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
                    await WriteFileAsync(documentsPart, GetJsonFileName("dokumenter", $"DokumenterTekst{fileNumber}"));
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
                    document.filId += ".html";
                    documents.Add(document);
                    if (!OnlyWriteDocumentFiles)
                    {
                        await File.WriteAllTextAsync(OutputFolderName + "filer\\" + document.filId, $"<html>{documentBVV.innhold}</html>");
                    }
                }
                int toSkip = 0;
                int fileNumber = 1;
                int migrertAntall = documents.Count;
                while (migrertAntall > toSkip)
                {
                    List<Document> documentsPart = documents.OrderBy(o => o.dokumentId).Skip(toSkip).Take(MaxAntallEntiteterPerFil).ToList();
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
        private static void GetOrganisasjonsKategori(Organisasjon organisasjon, FaForbindelsestyper type)
        {
            string typeCode = type.FotIdent?.Trim().ToUpper();
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
        private static void GetNettverksRolle(FaKlienttilknytning klientTilknytning, BarnetsNettverk forbindelse)
        {
            if (klientTilknytning.KtkRolle == "MOR" || klientTilknytning.KtkRolle == "FAR" || klientTilknytning.KtkRolle == "SØS" || klientTilknytning.KtkRolle == "FAM")
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
                    if (klientTilknytning.KtkRolle == "VRG" || klientTilknytning.KtkRolle == "AND" || klientTilknytning.KtkRolle == "FSA")
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
        private static MottattBekymringsmelding GetMottattBekymringsmelding(FaMeldinger meldingFamilia, string bydel)
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
                    mottattBekymringsmelding.meldingstype = "SØKNAD";
                    break;
                case "UFØ":
                    mottattBekymringsmelding.meldingstype = "UFØDT_BARN";
                    break;
                default:
                    break;
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
                mottattBekymringsmelding.utfortAvId = AddSpecificBydel(meldingFamilia.SbhMottattav, bydel);
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
        private static BehandlingAvBekymringsmelding GetBehandlingAvBekymringsmelding(FaMeldinger meldingFamilia, string bydel)
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
                behandlingAvBekymringsmelding.utfortAvId = AddSpecificBydel(meldingFamilia.SbhInitialer, bydel);
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
                    tilbakemeldingTilMelder.utfortAvId = AddSpecificBydel(meldingFamilia.SbhInitialer, bydel);
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
                                tilbakemeldingTilMelder.utfortAvId = AddSpecificBydel(postJournals[0].SbhInitialer, bydel);
                            }
                        }
                    }
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
                mottattBekymringsmelding.saksinnhold.Add("HØY_GRAD_AV_KONFLIKT_HJEMME");
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
            }
        }
        private static void GetGrunnlagForTiltak(FaUndersoekelser undersøkelse, Undersøkelse undersoekelse, bool henlegges)
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
                    undersoekelse.grunnlagForTiltak.Add("MIGRERT_INGEN_KODE");
                }
            }
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
        private string GetDEMValue(decimal klientId, string defaultValue)
        {
            string value = defaultValue;
            if (Bydelsforkortelse == "DEM")
            {
                SqlConnection connection = new(ConnectionStringMigrering);
                try
                {
                    SqlDataReader reader;
                    connection.Open();
                    SqlCommand command = new($"Select FNR From Barn Where KlientId={klientId}", connection)
                    {
                        CommandTimeout = 300
                    };
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            value = reader.GetString(0);
                        }
                    }
                    reader.Close();
                }
                finally
                {
                    connection.Close();
                }
            }
            return value;
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
                name = name.Trim();
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
                name = name.Trim();
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
                name = name.Trim();
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
                name = name.Trim();
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
                List<string> tableNames = new();
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
        private static string GetActorId(FaForbindelser forbindelse)
        {
            return GetUnikActorId(forbindelse.ForOrganisasjonsnr, forbindelse.ForFoedselsnummer, forbindelse.ForFornavn, forbindelse.ForEtternavn);
        }
        private string GetActorId(FaKlient klient, string fodselsnummer)
        {
            if (Bydelsforkortelse == "DEM")
            {
                return GetUnikActorId(null, fodselsnummer, klient.KliFornavn, klient.KliEtternavn);
            }
            else
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
                        return GetUnikActorId(null, null, klient.KliFornavn, klient.KliEtternavn);
                    }
                }
            }
        }
        private static string GetActorId(PersonPerson person)
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
        private static string GetUnikActorId(string organisasjonsnummer, string fodselsNummer, string forNavn, string etterNavn)
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
                    basisId = forNavn + etterNavn;
                }
                actorId = ByteArrayToString(System.Security.Cryptography.MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(basisId)));
            }
            return actorId;
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
        #endregion

        #region Filter
        private Expression<Func<FaKlient, bool>> KlientFilter()
        {
            return k => k.KliFoedselsdato.HasValue && (k.KliFoedselsdato > LastDateNoMigration || !k.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaPostjournal, bool>> KlientPostJournalFilter()
        {
            return p => p.KliLoepenrNavigation.KliFoedselsdato.HasValue && (p.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !p.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaSaksjournal, bool>> KlientSakJournalFilter()
        {
            return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaKlienttilknytning, bool>> KlientTilknytningFilter()
        {
            return f => f.KliLoepenrNavigation.KliFoedselsdato.HasValue && (f.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !f.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaMeldinger, bool>> KlientMeldingFilter()
        {
            return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaUndersoekelser, bool>> KlientUndersøkelseFilter()
        {
            return m => m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.MelLoepenrNavigation.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.MelLoepenrNavigation.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaTiltak, bool>> KlientTiltakFilter()
        {
            return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaTiltaksplan, bool>> KlientPlanFilter()
        {
            return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        private Expression<Func<FaJournal, bool>> KlientJournalFilter()
        {
            return m => m.KliLoepenrNavigation.KliFoedselsdato.HasValue && (m.KliLoepenrNavigation.KliFoedselsdato > LastDateNoMigration || !m.KliLoepenrNavigation.KliAvsluttetdato.HasValue);
        }
        #endregion
    }
}
