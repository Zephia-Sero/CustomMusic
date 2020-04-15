using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PiTung.Console;

namespace CustomMusic {
    public class CommandPlay : Command {
        public override string Name => "play";
        public override string Usage => $"{Name} (#)";
        public override string Description => "Forces music to play. Leave # blank for random, use # to pick a song.";

        public override bool Execute(IEnumerable<string> args) {
            if(args.Count()==0)
                CustomMusic.bmp.PlayRandom();
            else {
                int result;
                if(int.TryParse(args.ElementAt(0), out result))
                    CustomMusic.bmp.Play(result);
                else return false;
            }
                
            return true;
        }
    }
    public class CommandBeginPlaylist : Command {
        public override string Name => "playlist";
        public override string Usage => $"{Name} (random/#)";
        public override string Description => "Starts playing music. Put random to shuffle the music, or a number to start at that track.";

        public override bool Execute(IEnumerable<string> args) {
            if(args.Count()==0)
                CustomMusic.bmp.BeginPlaylist();
            else if(args.ElementAt(0)=="random")
                CustomMusic.bmp.BeginPlaylist_Shuffle();
            else {
                int result;
                if(int.TryParse(args.ElementAt(0), out result))
                    CustomMusic.bmp.BeginPlaylistAt(result);
                else return false;
            }
            return true;
        }
    }
    public class CommandStopPlaying : Command {
        public override string Name => "stopmusic";
        public override string Usage => $"{Name}";
        public override string Description => "Forces music to stop.";

        public override bool Execute(IEnumerable<string> args) {
            if(args.Count()==0)
                CustomMusic.bmp.StopTrack();
            else return false;
            return true;
        }
    }
    public class CommandViewTracks : Command {
        public override string Name => "listtracks";
        public override string Usage => $"{Name}";
        public override string Description => "Lists all tracks and IDs.";

        public override bool Execute(IEnumerable<string> args) {
            if(args.Count()==0)
                IGConsole.Log(CustomMusic.bmp.GetListOfSongs());
            else return false;
            return true;
        }
    }
    public class CommandNextSong : Command {
        public override string Name => "nextsong";
        public override string Usage => $"{Name}";
        public override string Description => "Skips to the next song.";

        public override bool Execute(IEnumerable<string> args) {
            if(args.Count()==0)
                CustomMusic.bmp.PlayNext();
            else return false;
            return true;
        }
    }
    public class CommandLastSong : Command {
        public override string Name => "lastsong";
        public override string Usage => $"{Name}";
        public override string Description => "Goes back one song.";

        public override bool Execute(IEnumerable<string> args) {
            if(args.Count()==0)
                CustomMusic.bmp.PlayLast();
            else return false;
            return true;
        }
    }
}
