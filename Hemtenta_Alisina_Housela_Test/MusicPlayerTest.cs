using Hemtenta_Alisina_Housela.music;
using Moq;
using System.Linq;
using System;
using System.Collections.Generic;
using Xunit;
namespace Hemtenta_Alisina_Housela_Test
{
    public class MusicPlayerTest
    {
        const string Song_1 = "song_1";
        const string Song_2 = "song_2";

        MusicPlayer musicplayer;
        Mock<IMediaDatabase> db;
        ISoundMaker sm;
        List<ISong> songs;

        public MusicPlayerTest()
        {
            db = new Mock<IMediaDatabase>();
            sm = new SoundMaker();

            songs = new List<ISong>
            {
                new Song(Song_1),
                new Song(Song_2)
            };

            db.Setup(x => x.IsConnected).Returns(true);
            db.Setup(x => x.FetchSongs(It.IsAny<string>())).Returns(songs);

            musicplayer = new MusicPlayer();
            musicplayer.SoundMaker = sm;
            musicplayer.MediaDatabase = db.Object;
        }

        [Fact]
        public void NumSongsInQueue_LoadingSongsWithSearchString_Success()
        {
            db.Setup(x => x.FetchSongs(It.IsAny<string>()))
                .Returns(songs.Where(s => s.Title.Contains(Song_1))
                .ToList());

            musicplayer.LoadSongs(Song_1);

            Assert.Equal(1, musicplayer.NumSongsInQueue);
        }

        [Fact]
        public void OpenConnection_DatabaseAlreadyOpen_Throws()
        {
            Assert.Throws<DatabaseAlreadyOpenException>(() => musicplayer.OpenConnection());
        }

        [Fact]
        public void LoadSongs_DatabaseClosed_Throws()
        {
            db.Setup(x => x.IsConnected).Returns(false);
            Assert.Throws<DatabaseClosedException>(() => musicplayer.LoadSongs("song"));
        }

        [Fact]
        public void NowPlaying_Play()
        {
            musicplayer.LoadSongs("song_1");
            musicplayer.Play();

            Assert.Equal(string.Format("Spelar {0}", Song_1), musicplayer.NowPlaying());
        }

        [Fact]
        public void NowPlaying_NextSong()
        {
            musicplayer.LoadSongs("search");
            musicplayer.NextSong();

            Assert.Equal(string.Format("Spelar {0}", Song_2), musicplayer.NowPlaying());
        }

        [Fact]
        public void NowPlaying_Stop()
        {
            musicplayer.LoadSongs("search");
            musicplayer.Play();
            musicplayer.Stop();

            Assert.Equal("Tystnad råder", musicplayer.NowPlaying());
        }
    }
}
