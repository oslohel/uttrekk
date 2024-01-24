#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
#endregion

namespace UttrekkFamilia
{
    public partial class MainWindow : Window
    {
        #region Members
        private string MainDBServer = "";
        private string ExtraDBServer = "";
        private string ConnSokrates = "";
        private string OutputFolderName = "";
        private string Bydelsidentifikator = "";
        private bool SakerIsChecked;
        private bool InnbyggereBarnIsChecked;
        private bool InnbyggereIsChecked;
        private bool OrganisasjonerIsChecked;
        private bool BarnetsNettverkBarnIsChecked;
        private bool BarnetsNettverkIsChecked;
        private bool MeldingerIsChecked;
        private bool UndersokelserIsChecked;
        private bool AvdelingsmappingIsChecked;
        private bool BrukereIsChecked;
        private bool VedtakIsChecked;
        private bool TiltakIsChecked;
        private bool PlanerIsChecked;
        private bool AktiviteterIsChecked;
        private bool UseSokrates;
        private bool OnlyWriteDocumentFiles;
        private bool OnlyActiveCases;
        private bool OnlyPassiveCases;
        private int AntallFilerPerZip;
        private bool ProduksjonIsChecked;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Main
        private void InfoFamiliaDatabase_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerInfoDatabase = new())
            {
                workerInfoDatabase.WorkerReportsProgress = true;
                workerInfoDatabase.DoWork += WorkerInfoFamiliaDatabase_DoWork;
                workerInfoDatabase.ProgressChanged += Worker_ProgressChanged;
                workerInfoDatabase.RunWorkerAsync();
            }
        }
        private void OneFileFamilia_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerInfoDatabase = new())
            {
                workerInfoDatabase.WorkerReportsProgress = true;
                workerInfoDatabase.DoWork += WorkerOneFileFamilia_DoWork;
                workerInfoDatabase.ProgressChanged += Worker_ProgressChanged;
                workerInfoDatabase.RunWorkerAsync();
            }
        }
        private void JsonInnhold_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerJsonInnhold = new())
            {
                workerJsonInnhold.WorkerReportsProgress = true;
                workerJsonInnhold.DoWork += WorkerJsonInnhold_DoWork;
                workerJsonInnhold.ProgressChanged += Worker_ProgressChanged;
                workerJsonInnhold.RunWorkerAsync();
            }
        }
        private void OrgNoSjekk_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker worker = new())
            {
                worker.WorkerReportsProgress = true;
                worker.DoWork += WorkerOrgNoSjekk_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
        }
        private void TranslateBetweenFamilaAndModulusBarn_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker worker = new())
            {
                worker.WorkerReportsProgress = true;
                worker.DoWork += WorkerTranslateBetweenFamilaAndModulusBarn_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
        }
        private void AntallEntiteter_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerJsonInnhold = new())
            {
                workerJsonInnhold.WorkerReportsProgress = true;
                workerJsonInnhold.DoWork += WorkerAntallEntiteter_DoWork;
                workerJsonInnhold.ProgressChanged += Worker_ProgressChanged;
                workerJsonInnhold.RunWorkerAsync();
            }
        }
        private void InfoBVVDatabase_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerInfoDatabase = new())
            {
                workerInfoDatabase.WorkerReportsProgress = true;
                workerInfoDatabase.DoWork += WorkerInfoBVVDatabase_DoWork;
                workerInfoDatabase.ProgressChanged += Worker_ProgressChanged;
                workerInfoDatabase.RunWorkerAsync();
            }
        }
        private void UttrekkFamiliaButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Vil du starte uttrekket av data fra Familia?", "Start uttrekk", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                SetParameters();

                using (BackgroundWorker workerUttrekk = new())
                {
                    workerUttrekk.WorkerReportsProgress = true;
                    workerUttrekk.DoWork += WorkerUttrekkFamilia_DoWork;
                    workerUttrekk.ProgressChanged += Worker_ProgressChanged;
                    workerUttrekk.RunWorkerAsync();
                }
            }
        }
        private void UttrekkHeleFamilia_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Vil du starte uttrekket av data fra alle Familia-databasene?", "Start uttrekk", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                SetParameters();

                using (BackgroundWorker workerUttrekk = new())
                {
                    workerUttrekk.WorkerReportsProgress = true;
                    workerUttrekk.DoWork += WorkerUttrekkFamiliaAll_DoWork;
                    workerUttrekk.ProgressChanged += Worker_ProgressChanged;
                    workerUttrekk.RunWorkerAsync();
                }
            }
        }
        private void AlleBrukere_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Vil du starte uttrekket av alle brukere i alle bydeler fra Familia?", "Start uttrekk", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                SetParameters();

                using (BackgroundWorker workerUttrekk = new())
                {
                    workerUttrekk.WorkerReportsProgress = true;
                    workerUttrekk.DoWork += WorkerAlleBrukere_DoWork;
                    workerUttrekk.ProgressChanged += Worker_ProgressChanged;
                    workerUttrekk.RunWorkerAsync();
                }
            }
        }
        private void UttrekkBVVButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Vil du starte uttrekket av data fra Visma Flyt Barnevernvakt?", "Start uttrekk", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                SetParameters();

                using (BackgroundWorker workerUttrekk = new())
                {
                    workerUttrekk.WorkerReportsProgress = true;
                    workerUttrekk.DoWork += WorkerUttrekkBVV_DoWork;
                    workerUttrekk.ProgressChanged += Worker_ProgressChanged;
                    workerUttrekk.RunWorkerAsync();
                }
            }
        }
        private void InfoSokratesDatabase_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerSokrates = new())
            {
                workerSokrates.WorkerReportsProgress = true;
                workerSokrates.DoWork += WorkerInfoDatabaseSokrates_DoWork;
                workerSokrates.ProgressChanged += Worker_ProgressChanged;
                workerSokrates.RunWorkerAsync();
            }
        }
        private void ZipButton_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerZip = new())
            {
                workerZip.WorkerReportsProgress = true;
                workerZip.DoWork += WorkerZip_DoWork;
                workerZip.ProgressChanged += Worker_ProgressChanged;
                workerZip.RunWorkerAsync();
            }
        }
        private void ReplaceInFilesButton_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerZip = new())
            {
                workerZip.WorkerReportsProgress = true;
                workerZip.DoWork += WorkerReplaceInFiles_DoWork;
                workerZip.ProgressChanged += Worker_ProgressChanged;
                workerZip.RunWorkerAsync();
            }
        }
        private void AktorIdButton_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerZip = new())
            {
                workerZip.WorkerReportsProgress = true;
                workerZip.DoWork += WorkerAktorId_DoWork;
                workerZip.ProgressChanged += Worker_ProgressChanged;
                workerZip.RunWorkerAsync();
            }
        }
        private void WorkerZip_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                uttrekk.DoZip(worker);
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerReplaceInFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                uttrekk.DoReplaceInFiles(worker);
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerAktorId_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                uttrekk.DoAktorIds(worker);
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerInfoFamiliaDatabase_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetInformationFamiliaAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerOneFileFamilia_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetOneFileFamiliaAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerJsonInnhold_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                uttrekk.GetJsonInnholdAsync(worker);
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerOrgNoSjekk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.DoOrgNoSjekkAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerTranslateBetweenFamilaAndModulusBarn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.DoTranslateBetweenFamilaAndModulusBarnAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerAntallEntiteter_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetInformationAntallEntiteterAsync(worker);
                task.Wait();
                worker.ReportProgress(0, "Telling antall entiteter alle bydeler ferdig.");
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerAlleBrukere_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                uttrekk.CreateAllfolders();
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetAlleBrukereFamiliaAsync(worker);
                task.Wait();
                worker.ReportProgress(0, "Uttrekk alle brukere alle bydeler ferdig.");
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerInfoBVVDatabase_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, "BVV", UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetInformationBVVAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerInfoDatabaseSokrates_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetInformationSokratesAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerUttrekkFamilia_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var worker = sender as BackgroundWorker;
            string information = $"UTTREKK {DateTime.Now:dd.MM.yyyy HH:mm} Bydelsid: {Bydelsidentifikator}" + Environment.NewLine + Environment.NewLine;
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                uttrekk.CreateAllfolders(OnlyWriteDocumentFiles);

                if (UseSokrates)
                {
                    var task = uttrekk.ExtractSokratesAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (InnbyggereBarnIsChecked)
                {
                    var task = uttrekk.GetInnbyggereBarnAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (InnbyggereIsChecked)
                {
                    var task = uttrekk.GetInnbyggereAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (OrganisasjonerIsChecked)
                {
                    var task = uttrekk.GetOrganisasjonerAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (SakerIsChecked)
                {
                    var task = uttrekk.GetSakerAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (BarnetsNettverkBarnIsChecked)
                {
                    var task = uttrekk.GetBarnetsNettverkBarnAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (BarnetsNettverkIsChecked)
                {
                    var task = uttrekk.GetBarnetsNettverkAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (MeldingerIsChecked)
                {
                    var task = uttrekk.GetMeldingerAsync(worker);
                    task.Wait();
                    information += task.Result;

                    task = uttrekk.GetMeldingerUtenSakAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (UndersokelserIsChecked)
                {
                    var task = uttrekk.GetUndersokelserAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (AvdelingsmappingIsChecked)
                {
                    var task = uttrekk.GetAvdelingsmappingAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (BrukereIsChecked)
                {
                    var task = uttrekk.GetBrukereAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (VedtakIsChecked)
                {
                    var task = uttrekk.GetVedtakAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (TiltakIsChecked)
                {
                    var task = uttrekk.GetTiltakAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (PlanerIsChecked)
                {
                    var task = uttrekk.GetPlanerAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (AktiviteterIsChecked)
                {
                    var task = uttrekk.GetAktiviteterAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            stopwatch.Stop();
            worker.ReportProgress(0, string.Format("Uttrekket ferdig ({0} timer {1} minutter {2} sekunder)", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds));
            information += Environment.NewLine + string.Format("Uttrekket ferdig (({0} timer {1} minutter {2} sekunder))", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);

            string fileName = $"{OutputFolderName}\\{Bydelsidentifikator}_{DateTime.Now:yyyyMMdd_HHmm_}Uttrekk.txt";
            var taskStatus = File.WriteAllTextAsync(fileName, information);
            taskStatus.Wait();
        }
        private void WorkerUttrekkFamiliaAll_DoWork(object sender, DoWorkEventArgs e)
        {
            Mappings mappings = new(true);
            List<string> bydeler = mappings.GetAlleBydeler();

            foreach (string bydel in bydeler)
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                var worker = sender as BackgroundWorker;
                string information = $"UTTREKK {DateTime.Now:dd.MM.yyyy HH:mm} Bydelsid: {bydel}" + Environment.NewLine + Environment.NewLine;
                try
                {
                    Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, bydel, UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                    uttrekk.CreateAllfolders(OnlyWriteDocumentFiles);

                    if (UseSokrates)
                    {
                        var task = uttrekk.ExtractSokratesAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (InnbyggereBarnIsChecked)
                    {
                        var task = uttrekk.GetInnbyggereBarnAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (InnbyggereIsChecked)
                    {
                        var task = uttrekk.GetInnbyggereAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (OrganisasjonerIsChecked)
                    {
                        var task = uttrekk.GetOrganisasjonerAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (SakerIsChecked)
                    {
                        var task = uttrekk.GetSakerAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (BarnetsNettverkBarnIsChecked)
                    {
                        var task = uttrekk.GetBarnetsNettverkBarnAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (BarnetsNettverkIsChecked)
                    {
                        var task = uttrekk.GetBarnetsNettverkAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (MeldingerIsChecked)
                    {
                        var task = uttrekk.GetMeldingerAsync(worker);
                        task.Wait();
                        information += task.Result;
                        
                        task = uttrekk.GetMeldingerUtenSakAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (UndersokelserIsChecked)
                    {
                        var task = uttrekk.GetUndersokelserAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (AvdelingsmappingIsChecked)
                    {
                        var task = uttrekk.GetAvdelingsmappingAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (BrukereIsChecked)
                    {
                        var task = uttrekk.GetBrukereAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (VedtakIsChecked)
                    {
                        var task = uttrekk.GetVedtakAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (TiltakIsChecked)
                    {
                        var task = uttrekk.GetTiltakAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (PlanerIsChecked)
                    {
                        var task = uttrekk.GetPlanerAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                    if (AktiviteterIsChecked)
                    {
                        var task = uttrekk.GetAktiviteterAsync(worker);
                        task.Wait();
                        information += task.Result;
                    }
                }
                catch (AggregateException ex)
                {
                    string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                    MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                stopwatch.Stop();
                worker.ReportProgress(0, string.Format("Uttrekket ferdig ({0} timer {1} minutter {2} sekunder)", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds));
                information += Environment.NewLine + string.Format("Uttrekket ferdig (({0} timer {1} minutter {2} sekunder))", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);

                string fileName = $"{OutputFolderName}\\{bydel}_{DateTime.Now:yyyyMMdd_HHmm_}Uttrekk.txt";
                var taskStatus = File.WriteAllTextAsync(fileName, information);
                taskStatus.Wait();
            }
        }
        private void WorkerUttrekkBVV_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var worker = sender as BackgroundWorker;
            string information = $"UTTREKK {DateTime.Now:dd.MM.yyyy HH:mm} Visma Flyt Barnevernvakt" + Environment.NewLine + Environment.NewLine;
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, "BVV", UseSokrates, OnlyWriteDocumentFiles, AntallFilerPerZip, OnlyActiveCases, OnlyPassiveCases, ProduksjonIsChecked);
                uttrekk.CreateAllfolders(OnlyWriteDocumentFiles);
                var task = uttrekk.GetBVVAsync(worker);
                task.Wait();
                information += task.Result;
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            stopwatch.Stop();
            worker.ReportProgress(0, string.Format("Uttrekket ferdig ({0} timer {1} minutter {2} sekunder)", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds));
            information += Environment.NewLine + string.Format("Uttrekket ferdig (({0} timer {1} minutter {2} sekunder))", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
            string fileName = $"{OutputFolderName}\\BVV_{DateTime.Now:yyyyMMdd_HHmm_}Uttrekk.txt";
            var taskStatus = File.WriteAllTextAsync(fileName, information);
            taskStatus.Wait();
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Status.Content = (string)e.UserState;
        }
        private void MarkAll_Click(object sender, RoutedEventArgs e)
        {
            chkSaker.IsChecked = true;
            chkInnbyggereBarn.IsChecked = true;
            chkInnbyggere.IsChecked = true;
            chkOrganisasjoner.IsChecked = true;
            chkBarnetsNettverkBarn.IsChecked = true;
            chkBarnetsNettverk.IsChecked = true;
            chkMeldinger.IsChecked = true;
            chkUndersokelser.IsChecked = true;
            chkAvdelingsmapping.IsChecked = true;
            chkBrukere.IsChecked = true;
            chkVedtak.IsChecked = true;
            chkTiltak.IsChecked = true;
            chkPlaner.IsChecked = true;
            chkAktiviteter.IsChecked = true;
        }
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            chkSaker.IsChecked = false;
            chkInnbyggereBarn.IsChecked = false;
            chkInnbyggere.IsChecked = false;
            chkOrganisasjoner.IsChecked = false;
            chkBarnetsNettverkBarn.IsChecked = false;
            chkBarnetsNettverk.IsChecked = false;
            chkMeldinger.IsChecked = false;
            chkUndersokelser.IsChecked = false;
            chkAvdelingsmapping.IsChecked = false;
            chkBrukere.IsChecked = false;
            chkVedtak.IsChecked = false;
            chkTiltak.IsChecked = false;
            chkPlaner.IsChecked = false;
            chkAktiviteter.IsChecked = false;
        }
        private static string GetFullFolderName(string folderName)
        {
            string newFolderName = folderName;
            if (string.IsNullOrEmpty(newFolderName))
            {
                throw new ArgumentNullException("Uttrekksfolder", "Uttrekksfolder ikke angitt.");
            }
            if (!newFolderName.EndsWith("\\"))
            {
                newFolderName += "\\";
            }
            return newFolderName;
        }
        private void SetParameters()
        {
            MainDBServer = MainDBServerBox.Text;
            ExtraDBServer = ExtraDBServerBox.Text;
            ConnSokrates = ConnSokratesBox.Password;
            OutputFolderName = GetFullFolderName(OutputFolder.Text);
            Bydelsidentifikator = BydelsidentifikatorBox.Text;
            SakerIsChecked = chkSaker.IsChecked.Value;
            InnbyggereBarnIsChecked = chkInnbyggereBarn.IsChecked.Value;
            InnbyggereIsChecked = chkInnbyggere.IsChecked.Value;
            OrganisasjonerIsChecked = chkOrganisasjoner.IsChecked.Value;
            BarnetsNettverkBarnIsChecked = chkBarnetsNettverkBarn.IsChecked.Value;
            BarnetsNettverkIsChecked = chkBarnetsNettverk.IsChecked.Value;
            MeldingerIsChecked = chkMeldinger.IsChecked.Value;
            UndersokelserIsChecked = chkUndersokelser.IsChecked.Value;
            AvdelingsmappingIsChecked = chkAvdelingsmapping.IsChecked.Value;
            BrukereIsChecked = chkBrukere.IsChecked.Value;
            VedtakIsChecked = chkVedtak.IsChecked.Value;
            TiltakIsChecked = chkTiltak.IsChecked.Value;
            PlanerIsChecked = chkPlaner.IsChecked.Value;
            AktiviteterIsChecked = chkAktiviteter.IsChecked.Value;
            UseSokrates = chkUseSokrates.IsChecked.Value;
            OnlyWriteDocumentFiles = chkOnlyWriteDocumentFiles.IsChecked.Value;
            OnlyActiveCases = chkOnlyActiveCases.IsChecked.Value;
            OnlyPassiveCases = chkOnlyPassiveCases.IsChecked.Value;
            AntallFilerPerZip = int.Parse(AntallPerZip.Text);
            ProduksjonIsChecked = chkProduksjon.IsChecked.Value;
        }
        #endregion
    }
}
