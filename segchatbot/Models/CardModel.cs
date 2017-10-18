using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace segchatbot.Models
{
    public class CardModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Text { get; set; }
        public List<CardImage> Images { get; set; }
        public List<CardAction> Buttons { get; set; }
    }
}