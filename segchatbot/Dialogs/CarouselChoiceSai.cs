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
    public class CarouselChoiceSai : IDialog<string>
    {
        List<HeroCardModel> options;

        public CarouselChoiceSai(List<HeroCardModel> options)
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
                        Title = "São Paulo",
                        Value = "São Paulo",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Rio de Janeiro",
                        Value = "Rio de Janeiro",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Minas Gerais",
                        Value = "Minas Gerais",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Espirito Santo",
                        Value = "Espirito Santo",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Outro",
                        Value = "Outro",
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