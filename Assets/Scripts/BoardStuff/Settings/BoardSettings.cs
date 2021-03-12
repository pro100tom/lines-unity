using System;
using Lines.Scripts.Base.Api;
using Lines.Scripts.General.Helpers;
using Lines.Scripts.General.Models;
using Lines.Scripts.General.Services;
using UnityEngine;

namespace Lines.Scripts.BoardStuff.Settings
{
    [Serializable]
    public class BoardSettings
    {
        [SerializeField] protected Dimension dimension;

        public BoardSettings(Dimension dimension)
        {
            this.dimension = dimension;
        }

        public Dimension GetDimension()
        {
            return dimension;
        }
    }

    public class BoardSettingsRepository : IBaseRepository<BoardSettings>
    {
        public BoardSettings ObtainDefault()
        {
            return JsonUtility.FromJson<BoardSettings>(
                FileReader.Read(
                    DirectoryHelper.GetScriptDataDirectory(),
                    "BoardSettings",
                    FileExtensions.Json
                )
            );
        }
    }
}
