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
        public static List<SaveFile> SaveFiles { get; private set; }
        public static SaveFile CurrentSave { get; set; }
        public static string BasePath;

        private static Action _loadAction;
        private static Action _saveAction;

        /// <summary>
        /// Call once on game open, loads all the metadata files into memory
        /// </summary>
        public static void InitialLoad(Action saveAction, Action loadAction)
        {
            _saveAction = saveAction;
            _loadAction = loadAction;
            SaveFiles = new List<SaveFile>();
            BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\SaveFiles";
            Directory.CreateDirectory(BasePath);

            List<string> saveFileFolderPaths = GetSaveFileFolders(true);


            foreach(string path in saveFileFolderPaths)
            {
                string metaDataFile = Directory.GetFiles(path).ToList().FirstOrDefault(x => x.Contains("MetaData"));
                if (string.IsNullOrEmpty(metaDataFile))
                    throw new Exception("Metadata file for save not found!");
                BinaryReader reader = CreateReader(metaDataFile);
                SaveFile saveFile = new SaveFile();
                saveFile.LoadSave(reader);
                SaveFiles.Add(saveFile);
            }
            //Temporary
            if(SaveFiles.Count > 0)
                CurrentSave = SaveFiles[0];
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
            {
                return false;
                throw new Exception("SaveFile with name " + name + " already exists");
            }
            return true;
        }

        public static void CreateNewSave(string name)
        {
            
            SaveFile saveFile = new SaveFile();
            saveFile.CreateNew(name);
            File.WriteAllText(saveFile.Name, string.Empty);

            BinaryWriter writer = CreateWriter(saveFile.MetaDataPath);

            saveFile.Save(writer);
            SaveFiles.Add(saveFile);


            //Temporary
            if (SaveFiles.Count > 0)
                CurrentSave = SaveFiles[0];
            DestroyWriter(writer);

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

        public static void DestroyReader(BinaryReader writer)
        {
            writer.Close();
        }

        public static BinaryWriter GetCurrentSaveFileWriter(string extension = null)
        {
            if (!string.IsNullOrEmpty(extension))
            {
                return CreateWriter(CurrentSave.FolderPath + extension);

            }
            else
            {
                return CreateWriter(CurrentSave.MainSaveFilePath);
            }

           

        }
        public static BinaryReader GetCurrentSaveFileReader(string extension = null)
        {
            if (!string.IsNullOrEmpty(extension))
            {
                return CreateReader(CurrentSave.FolderPath + extension);

            }
            else
            {
                return CreateReader(CurrentSave.MainSaveFilePath);

            }


        }

        public static void Load() => _loadAction();
        public static void Save() => _saveAction();

    }
}
