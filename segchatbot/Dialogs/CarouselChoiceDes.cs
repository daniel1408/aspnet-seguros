using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using segchatbot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace segchatbot.Dialogs
{
    [Serializable]
    public class CarouselChoiceDes : IDialog<string>
    {
        List<HeroCardModel> options;

        public CarouselChoiceDes(List<HeroCardModel> options)
        {
            this.options = options;
        }

        public async Task StartAsync(IDialogContext context)
        {
            IMessageActivity message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments = new List<Attachment>();

            foreach (var model in options)
            {
                HeroCard card = new HeroCard
                {
                    Title = model.Title,
                    Subtitle = model.Subtitle,
                    Text = model.Text
                };

                CardImage image = new CardImage
                {
                    Url = model.ImageUrl
                };

                card.Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Title = "América do Norte",
                        Value = "América do Norte",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "América Central",
                        Value = "América Central",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "América do Sul",
                        Value = "América do Sul",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "África",
                        Value = "África",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Oriente Médio",
                        Value = "Oriente Médio",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Europa",
                        Value = "Europa",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Oceania",
                        Value = "Oceania",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Ásia",
                        Value = "Ásia",
                        Type = "imBack"
                    }
                };
                card.Images = new List<CardImage> { image };
                message.Attachments.Add(card.ToAttachment());
            }

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity message = await result;
            context.Done(message.Text);
        }
    }
}