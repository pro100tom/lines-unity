using System;
using Lines.Scripts.Base.Api;
using Lines.Scripts.General.Helpers;
using Lines.Scripts.General.Models;
using Lines.Scripts.General.Services;
using Newtonsoft.Json;

namespace Lines.Scripts.TileStuff.Styles
{
    [Serializable]
    public class TileStyle
    {
        public float Size;
        public string[] MaterialNames;
        public TileColorStyle ColorStyle;
    }

    public class TileStyleRepository : IBaseRepository<TileStyle>
    {
        public TileStyle ObtainDefault()
        {
            var content = FileReader.Read(
                DirectoryHelper.GetScriptDataDirectory(),
                "TileStyle",
                FileExtensions.Json
            );

            return JsonConvert.DeserializeObject<TileStyle>(content);
        }
    }
}
