using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Globals.Classes;
using Globals.XPlatformHelpers;
using Java.Lang;
using MagicKitchen;
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;

namespace _Android_
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;

        //if you get Content not found exception with no other explanation, re-copy Game.dll
        //into androidx64 folder
        protected override void OnCreate(Bundle bundle)
        {
            Globals.Classes.Flags.DeviceType = DeviceType.Android;
            base.OnCreate(bundle);
            _game = new MagicKitchen.Game1();
            _view = _game.Services.GetService(typeof(View)) as View;
            AssetLocator.GetFiles = GetFiles;
            AssetLocator.GetStaticFileDirectory = GetBaseDirectory;
            AssetLocator.GetContentFileDirectory = () => { return "/Content/";};

            SetContentView(_view);
            _game.Run();
        }

        protected string[] GetFiles(string path)
        {
            string[] content = Android.App.Application.Context.Assets.List(path);
            if(content.Length == 0)
                throw new System.Exception("Could not find files");

            return content;
            
        }  

        protected string GetBaseDirectory(string path)
        {
            return path + "/";
        }
    }
}
