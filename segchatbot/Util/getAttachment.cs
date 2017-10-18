using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using segchatbot.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace segchatbot.Util
{
    public class getAttachment
    {
        public async static Task<List<Attachment>> Greeting(string saudacao)
        {
            List<Attachment> listAttachments = new List<Attachment>();
            HeroCard h = new HeroCard
            {
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "https://www.avaus.fi/content/uploads/2015/08/AMI_newsletter_blog_strategia_tiimi_850x650.jpg"
                    }
                },
                Title = saudacao,
                Text = "Sou o BOT da empresa Texas Seguros, acesse o menu principal e veja o que posso lhe oferecer!",

                Buttons =
                {
                    new CardAction
                    {
                        Title = "Menu Principal",
                        Value = "Menu Principal",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title = "Informações da Empresa",
                        Value = "Informações da Empresa",
                        Type = "imBack"
                    }
                }
            };

            listAttachments.Add(h.ToAttachment());

            return listAttachments;
        }

        public async static Task<List<Attachment>> informationCompany()
        {
            List<Attachment> listAttachments = new List<Attachment>();

            HeroCard a = new HeroCard
            {
                Title = "Texas Seguros!",
                Subtitle = "Segurança em primeiro lugar",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "http://3.bp.blogspot.com/-24IYCBeDfXo/TW_nNJUVfUI/AAAAAAAAB9g/QGpUG-DRAeg/s1600/R-2.jpg"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title ="Menu Principal",
                        Value = "Ver menu inicial",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title ="Visitar site",
                        Value = "https://www.google.com.br/",
                        Type = "openUrl"
                    }
                }
            };

            listAttachments.Add(a.ToAttachment());
            return listAttachments;
        }
        public async static Task<List<Attachment>> informationQuotes(RootObject moedas, LuisResult result)
        {
            List<Attachment> listAttachments = new List<Attachment>();
            string title = null;
            string subtitle = null;
            string text = null;

            switch (result.Entities[0].Entity)
            {
                case "dolar":
                    title = $"{moedas.valores.USD.nome}: {moedas.valores.USD.valor}";
                    subtitle = $"Fonte: {moedas.valores.USD.fonte}";
                    text = $"Última Consulta: {moedas.valores.USD.ultima_consulta}";
                    break;
                case "euro":
                    title = $"{moedas.valores.EUR.nome}: {moedas.valores.EUR.valor}";
                    subtitle = $"Fonte: {moedas.valores.EUR.fonte}";
                    text = $"Última Consulta: {moedas.valores.EUR.ultima_consulta}";
                    break;
                case "bitcoin":
                    title = $"{moedas.valores.BTC.nome}: {moedas.valores.BTC.valor}";
                    subtitle = $"Fonte: {moedas.valores.BTC.fonte}";
                    text = $"Última Consulta: {moedas.valores.BTC.ultima_consulta}";
                    break;
                case "libra":
                    title = $"{moedas.valores.GBP.nome}: {moedas.valores.GBP.valor}";
                    subtitle = $"Fonte: {moedas.valores.GBP.fonte}";
                    text = $"Última Consulta: {moedas.valores.GBP.ultima_consulta}";
                    break;
                case "peso":
                    title = $"{moedas.valores.ARS.nome}: {moedas.valores.ARS.valor}";
                    subtitle = $"Fonte: {moedas.valores.ARS.fonte}";
                    text = $"Última Consulta: {moedas.valores.ARS.ultima_consulta}";
                    break;
            }

            HeroCard a = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,

                Buttons =
                {
                    new CardAction
                    {
                        Title ="Menu Principal",
                        Value = "Ver menu inicial",
                        Type = "imBack"
                    }
                }
            };

            listAttachments.Add(a.ToAttachment());
            return listAttachments;
        }

        public async static Task<List<Attachment>> showPackage()
        {
            List<Attachment> listAttachments = new List<Attachment>();

            HeroCard a = new HeroCard
            {
                Title = "Pacote 1",
                Text = "Valor R$ 2.000,00",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "http://s3.amazonaws.com/meticulous-website/contents/547/full.png"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Contratar",
                        Value = "Contratar",
                        Type = "imBack"
                    }
                }
            };

            HeroCard b = new HeroCard
            {
                Title = "Pacote 2",
                Text = "Valor R$ 3.000,00",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "http://s3.amazonaws.com/meticulous-website/contents/547/full.png"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Contratar",
                        Value = "Contratar",
                        Type = "imBack"
                    }
                }
            };

            HeroCard c = new HeroCard
            {
                Title = "Pacote 3",
                Text = "Valor R$ 1.500,00",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "http://s3.amazonaws.com/meticulous-website/contents/547/full.png"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Contratar",
                        Value = "Contratar",
                        Type = "imBack"
                    }
                }

            };

            listAttachments.Add(a.ToAttachment());
            listAttachments.Add(b.ToAttachment());
            listAttachments.Add(c.ToAttachment());

            return listAttachments;
        }
        public async static Task<List<Attachment>> Menu()
        {
            List<Attachment> listAttachments = new List<Attachment>();

            HeroCard a = new HeroCard
            {
                Title = "Estimar pacote",
                Text = "Faça uma cotação personalizada",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "https://image.ibb.co/fL2z95/Estimar.jpg"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Clique aqui",
                        Value = "Estimar Pacote",
                        Type = "imBack"
                    }
                }
            };

            HeroCard b = new HeroCard
            {
                Title = "Contratar Pacote Especifico",
                Text = "Contrate um pacote predefinido",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "https://image.ibb.co/bFfmp5/Contratar.jpg"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Clique aqui",
                        Value = "Contratar pacote",
                        Type = "imBack"
                    }
                }
            };

            HeroCard c = new HeroCard
            {
                Title = "FAQ de dúvidas",
                Text = "Acesse o nosso FAQ de dúvidas",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "https://image.ibb.co/cHEPbk/FAQ.jpg"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Clique aqui",
                        Value = "Acessar FAQ",
                        Type = "imBack"
                    }
                }

            };

            HeroCard d = new HeroCard
            {
                Title = "Cotação de moedas",
                Text = "Verifique a cotação das principais moedas",
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = "https://image.ibb.co/mHQ3P5/Cotacao.jpg"
                    }
                },
                Buttons =
                {
                    new CardAction
                    {
                        Title = "Clique aqui",
                        Value = "Saber valor das moedas",
                        Type = "imBack"
                    }
                }

            };

            listAttachments.Add(a.ToAttachment());
            listAttachments.Add(b.ToAttachment());
            listAttachments.Add(c.ToAttachment());
            listAttachments.Add(d.ToAttachment());

            return listAttachments;
        }

        public async static Task<List<Attachment>> GetCarouselAllCoin(RootObject moedas)
        {
            List<Attachment> listAttachments = new List<Attachment>();

            HeroCard a = new HeroCard
            {
                //Usar entidade
                Title = "Moedas disponíveis",
                Subtitle = "Clique na opção desejada",
                Buttons =
                {
                    new CardAction
                    {
                        Title =moedas.valores.GBP.nome,
                        Value = "Saber valor da libra",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title =moedas.valores.ARS.nome,
                        Value = "Saber valor do peso argentino",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title =moedas.valores.USD.nome,
                        Value = "Saber valor do dolar",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title =moedas.valores.BTC.nome,
                        Value = "Saber valor do bitcoin",
                        Type = "imBack"
                    },
                    new CardAction
                    {
                        Title =moedas.valores.EUR.nome,
                        Value = "Saber valor do euro",
                        Type = "imBack"
                    }

                }
            };

            listAttachments.Add(a.ToAttachment());
            return listAttachments;
        }
    }
}