using PiTung;
using PiTung.Console;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace CustomMusic {
    public class CustomMusic : Mod {
        public override string Name => "Custom Music";
        public override string PackageName => "com.repsi0.custommusic";
        public override string Author => "Repsi0";
        public override System.Version ModVersion => new System.Version("1.0.0");

        public static BetterMusicPlayer bmp;

        public override void OnWorldLoaded(string worldName) {

            bmp=BetterMusicPlayer.CreateSelf();
            MusicPlayer[] mps = GameObject.FindObjectsOfType<MusicPlayer>();

            string musicDir = Directory.GetCurrentDirectory()+"\\music";
            if(!Directory.Exists(musicDir)) {
                Directory.CreateDirectory(musicDir);
            }

            string[] musFiles = Directory.GetFiles(musicDir);
            foreach(string path in Directory.GetFiles(musicDir)) {
                string[] splitPath = path.Split('.');
                string ext = splitPath[splitPath.Length-1].ToLower();
                string[] availableExtensions = { "wav", "ogg" };
                if(!availableExtensions.Contains(ext))
                    IGConsole.Log("[Custom Music] "+ext+" file type not supported! Try using .WAV or .OGG instead!");
                else {
                    AudioType at = (ext=="ogg") ? AudioType.OGGVORBIS : ((ext=="mp3")?AudioType.MPEG :AudioType.WAV);
                    UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file:///"+path, at);

                    try {
                        uwr.SendWebRequest();
                        while(uwr.downloadProgress!=1.0f || !uwr.isDone) {
                            //IGConsole.Log($"[Custom Music] File {path} downloaded: {100f*uwr.downloadProgress}%");
                        }
                        AudioClip ac=DownloadHandlerAudioClip.GetContent(uwr);

                        string[] splitPath2 = path.Split('/');
                        bmp.AddNewTrack(splitPath2[splitPath2.Length-1], ac);
                    } catch(Exception e) {
                        IGConsole.Log(e);
                    }
                }
            }
            foreach(MusicPlayer mp in mps) mp.Tracks=new AudioClip[]{};
            bmp.BeginPlaylist();
        }

        public override void BeforePatch() {
            Shell.RegisterCommand<CommandPlay>();
            Shell.RegisterCommand<CommandBeginPlaylist>();
            Shell.RegisterCommand<CommandStopPlaying>();
            Shell.RegisterCommand<CommandViewTracks>();
            Shell.RegisterCommand<CommandNextSong>();
            Shell.RegisterCommand<CommandLastSong>();
            Shell.RegisterCommand<CommandCurrentSong>();
            Shell.RegisterCommand<CommandPauseMusic>();
        }
    }
    
}
