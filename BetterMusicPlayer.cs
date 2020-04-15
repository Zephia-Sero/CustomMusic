using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PiTung.Console;

namespace CustomMusic {
    public class BetterMusicPlayer : MonoBehaviour {
        List<Track> playlist;
        static GameObject boombox;
        static GameObject player;
        float secondsBetweenTracks = 200f;
        int currentSongId = -1;
        bool paused = false;

        public static BetterMusicPlayer CreateSelf() {
            player=GameObject.Find("FirstPersonCharacter");
            boombox=new GameObject();boombox.transform.SetParent(player.transform);
            boombox.AddComponent<AudioSource>().spatialBlend=0.0f;
            return player.AddComponent<BetterMusicPlayer>();
        }
        
        void Start() {
            boombox.GetComponent<AudioSource>().spatialBlend=0.0f;
        }

        public BetterMusicPlayer() {
            playlist=new List<Track>();
        }
        public void AddNewTrack(string trackName, AudioClip ac) {
            paused=false;
            playlist.Add(new Track(trackName,ac));
        }
        public void SetTimeBetweenTracks(float newTime) {
            secondsBetweenTracks=newTime;
        }
        public void Play(int songId) {
            paused=false;
            StopTrack();
            playlist[songId].Play(boombox.GetComponent<AudioSource>());
        }
        public void PlayRandom() {
            paused=false;
            StopTrack();
            int num = UnityEngine.Random.Range(0, playlist.Count());
            playlist[num].Play(boombox.GetComponent<AudioSource>());
            currentSongId=num;
        }
        private void SetCurrentSongId(int SongId) {
            currentSongId=SongId;
        }
        public void BeginPlaylist() {
            paused=false;
            SetCurrentSongId(-1);
            PlayNext();
            StartCoroutine(Playlist_Normal());
        }
        public void BeginPlaylistAt(int start) {
            paused=false;
            SetCurrentSongId(start);
            PlayNext();
            StartCoroutine(Playlist_Normal());
        }
        public void BeginPlaylist_Shuffle() {
            paused=false;
            PlayRandom();
            StartCoroutine(Playlist_Shuffle());
        }
        private IEnumerator<object> Playlist_Normal() {
            yield return (object) new WaitForSecondsRealtime(secondsBetweenTracks);
            PlayNext();
            AudioSource audsrc = boombox.GetComponent<AudioSource>();
            yield return (object) new WaitUntil(() => audsrc.time>=audsrc.clip.length);
        }
        private IEnumerator<object> Playlist_Shuffle() {
            yield return (object) new WaitForSecondsRealtime(secondsBetweenTracks);
            PlayRandom();
            AudioSource audsrc = boombox.GetComponent<AudioSource>();
            yield return (object) new WaitUntil(() => audsrc.time >= audsrc.clip.length);
        }
        public void PlayNext() {
            paused=false;
            currentSongId++;
            if(currentSongId>=playlist.Count()) currentSongId=0;
            Play(currentSongId);
        }
        public void PlayLast() {
            paused=false;
            currentSongId--;
            if(currentSongId<0) currentSongId=playlist.Count()-1;
            Play(currentSongId);
        }
        public void StopTrack() {
            foreach(AudioSource a in GameObject.FindObjectsOfType<AudioSource>()) {
                a.Stop();
            }
        }
        public string GetListOfSongs() {
            string retVal = "";
            for(int i = 0; i < playlist.Count(); i++) {
                retVal+=i+": "+playlist[i].trackName+"\n";
            }
            return retVal;
        }
        public string GetCurrentSong() {
            return currentSongId+": "+playlist[currentSongId].trackName;
        }
        public void TogglePause() {
            paused=!paused;
            if(paused) boombox.GetComponent<AudioSource>().Pause();
            else boombox.GetComponent<AudioSource>().UnPause();
        }
    }
    class Track : MonoBehaviour {
        public string trackName;
        AudioClip ac;
        public Track(string trackName, AudioClip ac) {
            this.trackName=trackName;this.ac=ac;
        }
        public void Play(AudioSource audSrc) {
            IGConsole.Log("Now playing... "+trackName);
            audSrc.clip=ac;
            audSrc.Play();
        }
        public float GetTrackLength() {
            return ac.length;
        }
    }
}
