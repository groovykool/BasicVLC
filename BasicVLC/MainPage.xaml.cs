using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LibVLCSharp.Shared;
using Windows.Storage;
using System.Diagnostics;
using System.Threading;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BasicVLC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LibVLC _libVLC;

        private MediaPlayer _mediaPlayer;

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
        }

       

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Play();
        }

        private async void  Snapshot_Click(object sender, RoutedEventArgs e)
        {
            //Get path for temporary folder for this app package
            StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
            //create path and filename for libVLC method
            var pathstring = tempFolder.Path + "\\tempsnap.png";

            //save snapshot in temporary folder
            var bb=_mediaPlayer.TakeSnapshot(0,pathstring,0,0);
            
            //copy file to Pictures Library
            StorageFile tempImage = await tempFolder.GetFileAsync("tempsnap.png");
            StorageFolder picFolder = KnownFolders.PicturesLibrary;
            StorageFile copiedFile = await tempImage.CopyAsync(picFolder, "snap.png", NameCollisionOption.ReplaceExisting);
            
            
    }

       

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Pause();
            
        }

        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                _libVLC = new LibVLC(VideoView.SwapChainOptions);
                _libVLC.Log += (sender, ee) => Debug.WriteLine($"[{ee.Level}] {ee.Module}:{ee.Message}");
                _mediaPlayer = new MediaPlayer(_libVLC);
                VideoView.MediaPlayer = _mediaPlayer;
                //this._mediaPlayer.Play(new Media(_libVLC, "https://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4", FromType.FromLocation));
                //rtsp://b1.dnsdojo.com:1935/live/sys3.stream
                this._mediaPlayer.Play(new Media(_libVLC, "rtsp://b1.dnsdojo.com:1935/live/sys3.stream", FromType.FromLocation));
            };

            Unloaded += (s, e) =>
            {
                VideoView.MediaPlayer = null;
                this._mediaPlayer.Dispose();
                this._libVLC.Dispose();
               
            };
        }
    }
}
