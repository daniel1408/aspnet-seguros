﻿using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace segchatbot.Models
{
    [Serializable]
    public class HeroCardModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string ButtonTitle { get; set; }
        public string ButtonValue { get; set; }
        public string ButtonType { get; set; }
    }
}