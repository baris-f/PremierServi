using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{
    public class JoyConColors
    {
        [Serializable]
        public enum ColorName
        {
            None,
            Black,
            Gray,
            NeonRed,
            NeonBlue,
            NeonYellow,
            NeonGreen,
            NeonPink,
            Red,
            Blue,
            NeonPurple,
            NeonOrange,
            White,
            PastelPink,
            PastelYellow,
            PastelPurple,
            PastelGreen
        }

        private string bodyHex;
        private string buttonHex;

        private Color bodyColor;
        private Color buttonColor;

        public string BodyHex
        {
            set
            {
                ColorUtility.TryParseHtmlString(value, out bodyColor);
                bodyHex = value;
            }
            get => bodyHex;
        }
        public string ButtonHex
        {
            set
            {
                ColorUtility.TryParseHtmlString(value, out buttonColor);
                buttonHex = value;
            }
            get => buttonHex;
        }

        public Color BodyColor => bodyColor;
        public Color ButtonColor => buttonColor;

        public static Dictionary<ColorName, JoyConColors> Colors = new()
        {
            { ColorName.Black, new JoyConColors { BodyHex = "#313131", ButtonHex = "#0F0F0F" } },
            { ColorName.Gray, new JoyConColors { BodyHex = "#828282", ButtonHex = "#0F0F0F" } },
            { ColorName.NeonRed, new JoyConColors { BodyHex = "#FF3C28", ButtonHex = "#1E0A0A" } },
            { ColorName.NeonBlue, new JoyConColors { BodyHex = "#0AB9E6", ButtonHex = "#001E1E" } },
            { ColorName.NeonYellow, new JoyConColors { BodyHex = "#E6FF00", ButtonHex = "#142800" } },
            { ColorName.NeonGreen, new JoyConColors { BodyHex = "#1EDC00", ButtonHex = "#002800" } },
            { ColorName.NeonPink, new JoyConColors { BodyHex = "#FF3278", ButtonHex = "#28001E" } },
            { ColorName.Red, new JoyConColors { BodyHex = "#E10F00", ButtonHex = "#280A0A" } },
            { ColorName.Blue, new JoyConColors { BodyHex = "#4655F5", ButtonHex = "#00000A" } },
            { ColorName.NeonPurple, new JoyConColors { BodyHex = "#B400E6", ButtonHex = "#140014" } },
            { ColorName.NeonOrange, new JoyConColors { BodyHex = "#FAA005", ButtonHex = "#0F0A00" } },
            { ColorName.White, new JoyConColors { BodyHex = "#E6E6E6", ButtonHex = "#323232" } },
            { ColorName.PastelPink, new JoyConColors { BodyHex = "#FFAFAF", ButtonHex = "#372D2D" } },
            { ColorName.PastelYellow, new JoyConColors { BodyHex = "#F5FF82", ButtonHex = "#32332D" } },
            { ColorName.PastelPurple, new JoyConColors { BodyHex = "#F0CBEB", ButtonHex = "#373037" } },
            { ColorName.PastelGreen, new JoyConColors { BodyHex = "#BCFFC8", ButtonHex = "#2D322D" } },
        };
    }
}