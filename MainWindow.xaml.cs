#region Usings
using System;
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
        private int MaksimumAntall;
        private bool SakerIsChecked;
        private bool InnbyggereBarnIsChecked;
        private bool InnbyggereIsChecked;
        private bool OrganisasjonerIsChecked;
        private bool BarnetsNettverkBarnIsChecked;
        private bool BarnetsNettverkIsChecked;
        private bool MeldingerIsChecked;
        private bool MeldingerUtenSakIsChecked;
        private bool UndersokelserIsChecked;
        private bool AvdelingsmappingIsChecked;
        private bool BrukereIsChecked;
        private bool VedtakIsChecked;
        private bool TiltakIsChecked;
        private bool PlanerIsChecked;
        private bool AktiviteterIsChecked;
        private bool UseSokrates;
        private bool WriteFiles;
        private int AntallFilerPerZip;
        private string DokumentNumber;
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
        private void WriteSokrates_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerWriteSokrates = new())
            {
                workerWriteSokrates.WorkerReportsProgress = true;
                workerWriteSokrates.DoWork += WorkerWriteSokrates_DoWork;
                workerWriteSokrates.ProgressChanged += Worker_ProgressChanged;
                workerWriteSokrates.RunWorkerAsync();
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
        private void Tidstest_Click(object sender, RoutedEventArgs e)
        {
            SetParameters();

            using (BackgroundWorker workerTidstest = new())
            {
                workerTidstest.WorkerReportsProgress = true;
                workerTidstest.DoWork += WorkerTidstest_DoWork;
                workerTidstest.ProgressChanged += Worker_ProgressChanged;
                workerTidstest.RunWorkerAsync();
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
        private void WorkerZip_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
                var worker = sender as BackgroundWorker;
                uttrekk.DoZipAsync(worker);
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
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
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
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.GetOneFileFamiliaAsync(worker, Convert.ToDecimal(DokumentNumber));
                task.Wait();
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
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
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
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, "BVV", MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
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
        private void WorkerWriteSokrates_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
                var worker = sender as BackgroundWorker;
                var task = uttrekk.WriteSokratesAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WorkerTidstest_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
                var task = uttrekk.TidstestAsync(worker);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                string message = $"Unhandled exception ({ex.Source}): {ex.Message} Stack trace: {ex.StackTrace}";
                MessageBox.Show(message, "Migrering - Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            stopwatch.Stop();
            worker.ReportProgress(0, string.Format("Testen for {0} dokumenter tok ({1} timer {2} minutter {3} sekunder)", MaksimumAntall, stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds));
        }
        private void WorkerInfoDatabaseSokrates_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
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
            string information = $"UTTREKK {DateTime.Now:dd.MM.yyyy HH:mm} Bydelsid: {Bydelsidentifikator} Maks antall: {MaksimumAntall}" + Environment.NewLine + Environment.NewLine;
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, Bydelsidentifikator, MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
                uttrekk.CreateAllfolders();

                if (UseSokrates)
                {
                    var task = uttrekk.ExtractSokratesAsync(worker);
                    task.Wait();
                    information += task.Result;
                }
                if (SakerIsChecked)
                {
                    var task = uttrekk.GetSakerAsync(worker, MeldingerUtenSakIsChecked);
                    task.Wait();
                    information += task.Result;
                }
                if (InnbyggereBarnIsChecked)
                {
                    var task = uttrekk.GetInnbyggereBarnAsync(worker, MeldingerUtenSakIsChecked);
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
                }
                if (MeldingerUtenSakIsChecked)
                {
                    var task = uttrekk.GetMeldingerUtenSakAsync(worker);
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
            information += Environment.NewLine + string.Format("Uttrekket ferdig ({0} min {1} sek)", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
            string fileName = $"{OutputFolderName}\\{Bydelsidentifikator}_{DateTime.Now:yyyyMMdd_HHmm_}Uttrekk.txt";
            var taskStatus = File.WriteAllTextAsync(fileName, information);
            taskStatus.Wait();
        }
        private void WorkerUttrekkBVV_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var worker = sender as BackgroundWorker;
            string information = $"UTTREKK {DateTime.Now:dd.MM.yyyy HH:mm} Visma Flyt Barnevernvakt Maks antall: {MaksimumAntall}" + Environment.NewLine + Environment.NewLine;
            try
            {
                Uttrekk uttrekk = new(ConnSokrates, MainDBServer, ExtraDBServer, OutputFolderName, "BVV", MaksimumAntall, UseSokrates, WriteFiles, AntallFilerPerZip);
                uttrekk.CreateAllfolders();
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
            information += Environment.NewLine + string.Format("Uttrekket ferdig ({0} min {1} sek)", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
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
            chkMeldingerUtenSak.IsChecked = true;
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
            chkMeldingerUtenSak.IsChecked = false;
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
            MaksimumAntall = int.Parse(MaksAntall.Text);
            DokumentNumber = Doknumber.Text;
            SakerIsChecked = chkSaker.IsChecked.Value;
            InnbyggereBarnIsChecked = chkInnbyggereBarn.IsChecked.Value;
            InnbyggereIsChecked = chkInnbyggere.IsChecked.Value;
            OrganisasjonerIsChecked = chkOrganisasjoner.IsChecked.Value;
            BarnetsNettverkBarnIsChecked = chkBarnetsNettverkBarn.IsChecked.Value;
            BarnetsNettverkIsChecked = chkBarnetsNettverk.IsChecked.Value;
            MeldingerIsChecked = chkMeldinger.IsChecked.Value;
            MeldingerUtenSakIsChecked = chkMeldingerUtenSak.IsChecked.Value;
            UndersokelserIsChecked = chkUndersokelser.IsChecked.Value;
            AvdelingsmappingIsChecked = chkAvdelingsmapping.IsChecked.Value;
            BrukereIsChecked = chkBrukere.IsChecked.Value;
            VedtakIsChecked = chkVedtak.IsChecked.Value;
            TiltakIsChecked = chkTiltak.IsChecked.Value;
            PlanerIsChecked = chkPlaner.IsChecked.Value;
            AktiviteterIsChecked = chkAktiviteter.IsChecked.Value;
            UseSokrates = chkUseSokrates.IsChecked.Value;
            WriteFiles = chkWriteFiles.IsChecked.Value;
            AntallFilerPerZip = int.Parse(AntallPerZip.Text);
        }
        #endregion
    }
}
