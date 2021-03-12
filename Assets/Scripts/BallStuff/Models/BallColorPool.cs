using Lines.Scripts.Base.Api;
using Lines.Scripts.General.Helpers;
using Lines.Scripts.General.Models;
using Lines.Scripts.General.Services;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Models
{
    public class BallColorPool
    {
        public Color32[] Colors { get; set; }

        protected readonly System.Random random;

        public BallColorPool()
        {
            random = new System.Random();
        }

        public Color32 GetRandomColor()
        {
            return Colors[random.Next(Colors.Length)];
        }
    }
    
    public class BallColorPoolRepository : IBaseRepository<BallColorPool>
    {
        public BallColorPool ObtainDefault()
        {
            return new BallColorPool {
                Colors = JsonHelper.FromJson<Color32>(
                    FileReader.Read(
                        DirectoryHelper.GetScriptDataDirectory(),
                        "BallColors",
                        FileExtensions.Json
                    )
                )
            };
        }
    }
}
