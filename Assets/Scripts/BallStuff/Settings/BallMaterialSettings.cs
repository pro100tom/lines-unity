using System;
using Lines.Scripts.Base.Api;
using Lines.Scripts.General.Helpers;
using Lines.Scripts.General.Models;
using Lines.Scripts.General.Services;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Settings
{
    [Serializable]
    public class BallMaterialSettings
    {
        public string ShaderName;
        public Color32 Color;
    }

    public class BallMaterialSettingsRepository : IBaseRepository<BallMaterialSettings>
    {
        public BallMaterialSettings ObtainDefault()
        {
            var content = FileReader.Read(
                DirectoryHelper.GetScriptDataDirectory(),
                "BallMaterialSettings",
                FileExtensions.Json
            );

            return JsonUtility.FromJson<BallMaterialSettings>(content);
        }
    }
}
