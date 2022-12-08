using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector_Gadget_Maui
{
    public static class PublicExtensions
    {
        public static string processModelEnums(this whisperModels model)
        {
            switch (model)
            {
                case whisperModels.tiny:
                    return "tiny";                   
                case whisperModels.base_:
                    return "base";
                case whisperModels.small:
                    return "small";
                case whisperModels.medium:
                    return "medium";
                case whisperModels.large:
                    return "large";
                case whisperModels.medium_en:
                    return "medium.en";
                default:
                    return "tiny";
            }
        }
    }
}
