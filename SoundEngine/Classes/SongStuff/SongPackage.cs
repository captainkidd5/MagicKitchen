using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundEngine.Classes.SongStuff
{
    internal class SongPackage
    {
        public string Name { get; set; }
        public List<string> ViableStages { get; set; }
        public Song Song { get; set; }

        public SongPackage()
        {

        }
        public void LoadContent(ContentManager content, string name)
        {
            Song = content.Load<Song>($"{SongManager.songRootPath}/{name}");
        }
    }
}
