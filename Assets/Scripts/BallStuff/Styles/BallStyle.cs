using System;
using Lines.Scripts.BallStuff.Settings;
using Lines.Scripts.Base.Api;
using Lines.Scripts.General.Helpers;
using Lines.Scripts.General.Models;
using Lines.Scripts.General.Services;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Styles
{
    [Serializable]
    public class BallStyle
    {
        public float UniformScale;
        public Material Material;
    }

    public class BallStyleRepository : IBaseRepository<BallStyle>
    {
        protected BallMaterialSettingsRepository ballMaterialSettingsRepository;

        protected BallStyleRepository()
        {

        }

        public BallStyle ObtainDefault()
        {
            var ballStyle = JsonUtility.FromJson<BallStyle>(FileReader.Read(
                DirectoryHelper.GetScriptDataDirectory(),
                "BallStyle",
                FileExtensions.Json
            ));
            var ballMaterialSettings = ballMaterialSettingsRepository.ObtainDefault();
            ballStyle.Material = new Material(Shader.Find(ballMaterialSettings.ShaderName)) {
                color = ballMaterialSettings.Color
            };

            return ballStyle;
        }

        public class BallStyleRepositoryFactory : IBaseFactory<BallStyleRepository>
        {
            protected readonly BallMaterialSettingsRepository ballMaterialSettingsRepository;

            public BallStyleRepositoryFactory(BallMaterialSettingsRepository ballMaterialSettingsRepository)
            {
                this.ballMaterialSettingsRepository = ballMaterialSettingsRepository;
            }

            public BallStyleRepository Create()
            {
                return new BallStyleRepository {
                    ballMaterialSettingsRepository = ballMaterialSettingsRepository,
                };
            }
        }
    }
}
