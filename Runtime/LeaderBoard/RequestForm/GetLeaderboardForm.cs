﻿using System;
using Response;
using Util;

namespace LeaderBoard.RequestForm
{
    [Serializable]
    public class GetLeaderboardForm : IValidatedForm
    {
        public string aroundPlayerLimit;
        public string isAroundPlayer;
        public string limit;
        public string offset;
        public string profileId;

        public bool IsValid(out string description)
        {
            return limit.IsNumber("Limit", out description, out var limitNumber)
                   && offset.IsNumber("Offset", out description, out var offsetNumber)
                   && limitNumber.IsInRange("Limit", out description, 0, 100)
                   && offsetNumber.IsInRange("Offset", out description, 0);
        }
    }
}