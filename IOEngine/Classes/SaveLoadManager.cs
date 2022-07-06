using Globals.Classes;
using Globals.Classes.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IOEngine.Classes
{
    //TODO: string SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyGame");
    //Save here at some point
    public static class SaveLoadManager
    {
        public static Dictionary<string, SaveFile> SaveFiles { get; private set; }
        public static SaveFile CurrentSave { get; private set; }
        public static string BasePath;

        public static EventHandler<FileCreatedEventArgs> SaveCreated;
        public static void Create(SaveFile file) => SaveCreated.Invoke(null, new FileCreatedEventArgs() { BinaryWriter = CreateWriter(file.MetadataSaveData.MainSaveFilePath) });

        public static EventHandler<FileLoadedEventArgs> SaveLoaded;
        public static void Load(SaveFile file) => SaveLoaded.Invoke(null, new FileLoadedEventArgs() { BinaryReader = CreateReader(file.MetadataSaveData.MainSaveFilePath) });

        public static EventHandler<FileSavedEventArgs> SaveSaved;
        public static void Save(SaveFile file) => SaveSaved.Invoke(null, new FileSavedEventArgs() { BinaryWriter = CreateWriter(file.MetadataSaveData.MainSaveFilePath) });


        /// <summary>
        /// Call once on game open, loads all the metadata files into memory
        /// </summary>
        public static void FetchAllMetadata()
        {

            SaveFiles = new Dictionary<string, SaveFile>();
            BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\SaveFiles";
            Directory.CreateDirectory(BasePath);

            List<string> saveFileFolderPaths = GetSaveFileFolders(true);


            foreach(string path in saveFileFolderPaths)
            {
                string metaDataFile = Directory.GetFiles(path).ToList().FirstOrDefault(x => x.Contains("MetaData"));
                if (string.IsNullOrEmpty(metaDataFile))
                    throw new Exception("Metadata file for save not found!");

                SaveFile saveFile = new SaveFile();
                saveFile.LoadSave(metaDataFile);
                SaveFiles.Add(saveFile.MetadataSaveData.Name, saveFile);

            }
        }

        public static void SaveGame(string[] commands)
        {
            Flags.IsNewGame = false;
            Save(CurrentSave);

        }

        private static List<string> GetSaveFileFolders(bool fullPath = false)
        {
            if (fullPath)
                return Directory.GetDirectories(BasePath).ToList();

            return Directory.EnumerateDirectories(BasePath).Select(f => Path.GetFileName(f)).ToList();
        }

        /// <summary>
        /// Returns true if save file name does not exist in the save directory
        /// </summary>
        public static bool IsSaveNameAvailable(string name)
        {
            List<string> saveFileFolders = GetSaveFileFolders();
            if (saveFileFolders.Contains(name))
                return false;
            return true;
        }

        public static void SetCurrentSave(string name)
        {
            CurrentSave = SaveFiles[name];
        }
        public static void CreateNewSave(string name)
        {
            
            SaveFile saveFile = new SaveFile();
            saveFile.CreateNew(name);


            saveFile.Save();


            SaveFiles.Add(name, saveFile);
            CurrentSave = saveFile;
            //Fire up game to save empty initial values to the save for everything. This triggers the event in game1.cs
            Create(saveFile);
            Flags.IsNewGame = true;
        }

        /// <summary>
        /// Creates new writer, don't forget to flush and close it with <see cref="DestroyWriter(BinaryWriter)"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static BinaryWriter CreateWriter(string path)
        {
            FileStream fileStream = File.OpenWrite(path);
            return new BinaryWriter(fileStream);
        }


        public static void DestroyWriter(BinaryWriter writer)
        {
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Creates new reader, don't forget to flush and close it with <see cref="DestroyReader(BinaryReader)"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static BinaryReader CreateReader(string path)
        {
            FileStream fileStream = File.OpenRead(path);
            return new BinaryReader(fileStream);
        }

        public static void DestroyReader(BinaryReader reader)
        {
            reader.Close();
        }

        public static BinaryWriter GetCurrentSaveFileWriter(string extension = null)
        {
            if (!string.IsNullOrEmpty(extension))
                return CreateWriter(CurrentSave.MetadataSaveData.FolderPath + extension);
            else       
                return CreateWriter(CurrentSave.MetadataSaveData.MainSaveFilePath);         
        }
        public static BinaryReader GetCurrentSaveFileReader(string extension = null)
        {
            if (!string.IsNullOrEmpty(extension))
                return CreateReader(CurrentSave.MetadataSaveData.FolderPath + extension);
            else
                return CreateReader(CurrentSave.MetadataSaveData.MainSaveFilePath);
        }


    }
}
