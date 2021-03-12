using System;
using Lines.Scripts.Base.Api;
using Lines.Scripts.General.Helpers;
using Lines.Scripts.General.Models;
using Lines.Scripts.General.Services;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Settings
{
    [Serializable]
    public class BallLifetimeManagerSettings
    {
        [SerializeField] protected int spawnQtyMin;
        [SerializeField] protected int spawnQtyMax;

        public BallLifetimeManagerSettings(int spawnQtyMin, int spawnQtyMax)
        {
            this.spawnQtyMin = spawnQtyMin;
            this.spawnQtyMax = spawnQtyMax;
        }

        public int GetSpawnQtyMin()
        {
            return spawnQtyMin;
        }

        public int GetSpawnQtyMax()
        {
            return spawnQtyMax;
        }
    }

    public class BallLifetimeManagerSettingsRepository : IBaseRepository<BallLifetimeManagerSettings>
    {
        public BallLifetimeManagerSettings ObtainDefault()
        {
            return JsonUtility.FromJson<BallLifetimeManagerSettings>(
                FileReader.Read(
                    DirectoryHelper.GetScriptDataDirectory(),
                    "BallManagerSettings",
                    FileExtensions.Json
                )
            );
        }
    }
}
