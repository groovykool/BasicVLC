using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LibVLCSharp.Shared;
using Windows.Storage;
using LibVLCSharp.Platforms.UWP;
using System.Diagnostics;


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

    private async void Snapshot_Click(object sender, RoutedEventArgs e)
    {
      //Get path for temporary folder for this app package
      StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
      //create path and filename for libVLC method
      //var pathstring = tempFolder.Path + "\\tempsnap.png";
      var pathstring = tempFolder.Path + "\\tempsnap.jpeg";
      //save snapshot in temporary folder
      var bb = _mediaPlayer.TakeSnapshot(0, pathstring, 0, 0);

      //copy file to Pictures Library
      StorageFile tempImage = await tempFolder.GetFileAsync("tempsnap.jpeg");
      StorageFolder picFolder = KnownFolders.PicturesLibrary;
      StorageFile copiedFile = await tempImage.CopyAsync(picFolder, "snap.jpeg", NameCollisionOption.ReplaceExisting);


    }



    private void Pause_Click(object sender, RoutedEventArgs e)
    {
      _mediaPlayer.Pause();

    }

    public MainPage()
    {
      //_libVLC = new LibVLC(VideoView.SwapChainOptions);
      InitializeComponent();
      TestException();
      Loaded += (s, e) =>
      {
        _libVLC = new LibVLC(VideoView.SwapChainOptions);
        //_libVLC.Log += (sender, ee) => Debug.WriteLine($"[{ee.Level}] {ee.Module}:{ee.Message}");
        _mediaPlayer = new MediaPlayer(_libVLC);
        VideoView.MediaPlayer = _mediaPlayer;
        //this._mediaPlayer.Play(new Media(_libVLC, "https://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4", FromType.FromLocation));
        //rtsp://b1.dnsdojo.com:1935/live/sys3.stream
        //_mediaPlayer.Volume = 0;
        //var mmm = new Media(_libVLC, "http://video.ch9.ms/ch9/70cc/83e17e76-8be8-441b-b469-87cf0e6a70cc/ASPNETwithScottHunter_high.mp4", FromType.FromLocation);
        var mmm = new Media(_libVLC, "rtsp://:@tonyw.selfip.com:6002/cam/realmonitor?channel=1&subtype=0&unicast=true&proto=Onvif", FromType.FromLocation);
        mmm.AddOption($":rtsp-tcp");
        mmm.AddOption($":network-caching=1200");
        _mediaPlayer.Play(mmm);
        _mediaPlayer.Mute = true;
        _mediaPlayer.Buffering += (ss, ee) =>
        {
          _mediaPlayer.Volume = 0;

        };

      };

      Unloaded += (s, e) =>
      {
        VideoView.MediaPlayer = null;
        this._mediaPlayer.Dispose();
        this._libVLC.Dispose();

      };
    }
    private void TestException()
    {
      try    //Try1 
      {
        _libVLC = new LibVLC(VideoView.SwapChainOptions);
        //_libVLC = new LibVLC("Bad Pass");
        Message.Text = "No exception in Try1\n";
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
        Message.Text += $"Try1 Exception:  {ex}\n"; 
      }

      try    //Try2
      {
        //VideoView2 = null;
        var Bad = VideoView2.ActualHeight;
        Message.Text += "No exception in Try2\n";
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
        Message.Text += $"Try2 Exception:  {ex}\n";
      }
      try    //Try3
      {
        //VideoView2 = new VideoView();
        VideoView2.Loaded += VideoView2_Loaded;
        
        var Bad = VideoView2.ActualHeight;
        VideoView2.Height = 0;
        Message.Text += "No exception in Try3\n";
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
        Message.Text += $"Try3 Exception:  {ex}\n";
      }


    }

    private void VideoView2_Loaded(object sender, RoutedEventArgs e)
    {
      //throw new NotImplementedException();
      var l = new LibVLC(VideoView2.SwapChainOptions);
      var m= new MediaPlayer(l);
      VideoView2.MediaPlayer = m;
      var md = new Media(l, "http://video.ch9.ms/ch9/70cc/83e17e76-8be8-441b-b469-87cf0e6a70cc/ASPNETwithScottHunter_high.mp4", FromType.FromLocation);
      //var mmm = new Media(_libVLC, "rtsp://:@tonyw.selfip.com:6002/cam/realmonitor?channel=1&subtype=0&unicast=true&proto=Onvif", FromType.FromLocation);
      md.AddOption($":rtsp-tcp");
      md.AddOption($":network-caching=1200");
      m.Play(md);
    }

    private void Vol0_Click(object sender, RoutedEventArgs e)
    {
      _mediaPlayer.Mute = true;

    }

    private void Vol40_Click(object sender, RoutedEventArgs e)
    {
      _mediaPlayer.Mute = false;
      _mediaPlayer.Volume = 80;
    }
  }
}
