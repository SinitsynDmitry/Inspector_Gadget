using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector_Gadget
{
    public static class PublicExtensions
    {
        public static string processModelEnums(this whisperModels model)
        {
            switch (model)
            {
                case whisperModels.tiny:
                    return "tiny";
                    break;
                case whisperModels.base_:
                    return "base";
                    break;
                case whisperModels.small:
                    return "small";
                    break;
                case whisperModels.medium:
                    return "medium";
                    break;
                case whisperModels.large:
                    return "large";
                    break;
                case whisperModels.medium_en:
                    return "medium.en";
                    break;
                default:
                    return "tiny";
                    break;
            }
        }
    }
}
