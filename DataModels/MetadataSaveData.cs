using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class MetadataSaveData
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string StageFilePath { get; set; }

        public string ChunkPath { get; set; }
        public string FolderPath { get; set; }
        public string MainSaveFilePath { get; set; }

        //METADATA - the file where the main menu reads about each save file
        //without having to actually read the entirety of each save file bc that would be v slow
        public string MetaDataPath { get; set; }


    }
}
