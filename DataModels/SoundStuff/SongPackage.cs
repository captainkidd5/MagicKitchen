using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.SoundStuff
{
    public class SongPackage
    {
        public string Name { get; set; }
        public List<string> ViableStages { get; set; }

        public string LocalPath { get; set; }

        [ContentSerializerIgnoreAttribute]

        public SoundEffect Song { get; set; }


        public SongPackage()
        {

        }
        public void LoadContent(ContentManager content,string songRootPath)
        {
            Song = content.Load<SoundEffect>($"{songRootPath}/{LocalPath}");
        }
    }
}
