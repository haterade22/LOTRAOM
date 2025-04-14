using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace LOTRAOM.Extensions
{
    internal static class ModelExtension
    {
        public static TBaseModel GetExistingModel<TBaseModel>(this IGameStarter campaignGameStarter) where TBaseModel : GameModel
        {
            return (TBaseModel)campaignGameStarter.Models.Last(model => model.GetType().IsSubclassOf(typeof(TBaseModel)));
        }
    }
}
