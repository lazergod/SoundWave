using AxWMPLib;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SoundWave
{
    public partial class Form1 : Form
    {
        string[] musicFiles;
        int currentSong = 0;

        public Form1()
        {
            InitializeComponent();

            // Deshabilitar el botón de maximizar en la barra de título del formulario
            this.MaximizeBox = false;

            // Establecer el tamaño del formulario
            this.Size = new Size(650, 200);

            // Establecer la carpeta de música y buscar archivos MP3
            string musicFolderPath = @"ArchivosMusica";
            musicFiles = Directory.GetFiles(musicFolderPath, "*.mp3");

            // Establecer la primera canción si se encuentran archivos MP3
            if (musicFiles.Length > 0)
            {
                axWindowsMediaPlayer1.URL = musicFiles[0];
                lblFileName.Text = Path.GetFileName(musicFiles[0]);
            }

            //Ocultar el Media Player para personalizar el diseño
            axWindowsMediaPlayer1.Visible = false;

            // Iniciar el temporizador de progreso
            tmrProgress.Start();

            // Ubicar y cambiar el tamaño del label con el nombre de la canción
            lblFileName.Location = new Point(10, 10);
            lblFileName.AutoSize = true;

            // Ubicar y cambiar el tamaño de la progress bar
            progressBar1.Location = new Point(10, lblFileName.Bottom + 10);
            progressBar1.Size = new Size(this.ClientSize.Width - 20, 20);

            // Ubicar y cambiar el tamaño de los segundos transcurridos y la duración total
            lblElapsed.Location = new Point(10, progressBar1.Bottom + 10);
            lblElapsed.AutoSize = true;
            lblDuration.Location = new Point(this.ClientSize.Width - lblDuration.Width - 10, progressBar1.Bottom + 10);
            lblDuration.AutoSize = true;

            // Ubicar y cambiar el tamaño de los controles para pausar, retroceder, etc.
            btnPrevious.Location = new Point(10, lblElapsed.Bottom + 10);
            btnPlay.Location = new Point(btnPrevious.Right + 10, lblElapsed.Bottom + 10);
            btnPause.Location = new Point(btnPlay.Right + 10, lblElapsed.Bottom + 10);
            btnStop.Location = new Point(btnPause.Right + 10, lblElapsed.Bottom + 10);
            btnNext.Location = new Point(btnStop.Right + 10, lblElapsed.Bottom + 10);
        }

        ////Este método se llama cada vez que cambia la canción actual, 
        ////Actualiza la etiqueta Label con el nombre del archivo de música actual
        ////para que el usuario pueda ver qué canción está reproduciendo.
        private void axWindowsMediaPlayer1_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            // Obtener el nombre del archivo actual
            string fileName = axWindowsMediaPlayer1.currentMedia.name;

            // Actualizar la etiqueta Label con el nombre del archivo
            lblFileName.Text = Path.GetFileName(fileName);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentSong > 0)
            {
                currentSong--;
                axWindowsMediaPlayer1.URL = musicFiles[currentSong];
                axWindowsMediaPlayer1.Ctlcontrols.play();
                lblFileName.Text = Path.GetFileName(musicFiles[currentSong]);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentSong < musicFiles.Length - 1)
            {
                currentSong++;
                axWindowsMediaPlayer1.URL = musicFiles[currentSong];
                axWindowsMediaPlayer1.Ctlcontrols.play();
                lblFileName.Text = Path.GetFileName(musicFiles[currentSong]);
            }
        }

        // Calcula el porcentaje de progreso de la canción actual y lo usa para actualizar la barra de progreso. 
        // También muestra el tiempo transcurrido y la duración total de la canción actual en etiquetas Label.
        private void tmrProgress_Tick(object sender, EventArgs e)
        {
            // Obtener la duración total del archivo
            double duration = axWindowsMediaPlayer1.currentMedia.duration;

            // Obtener la posición actual de reproducción
            double position = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;

            // Calcular el porcentaje de progreso
            int progress = (int)((position / duration) * 100);

            // Actualizar la barra de progreso
            progressBar1.Value = progress;

            // Actualizar el tiempo transcurrido y la duración total
            TimeSpan elapsed = TimeSpan.FromSeconds(position);
            TimeSpan remaining = TimeSpan.FromSeconds(duration);
            lblElapsed.Text = $"{elapsed:mm\\:ss}";
            lblDuration.Text = $"{remaining:mm\\:ss}";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
