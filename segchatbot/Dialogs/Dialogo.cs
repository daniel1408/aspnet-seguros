﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using segchatbot.Models;
using segchatbot.Service;
using segchatbot.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace segchatbot
{
    [Serializable]
    [LuisModel("9573b30d-3ef9-4970-88cd-673d7b28a876", "0b9d35219fee43e3b9571d43b27288dd")]
    public class Dialogo : LuisDialog<Object>
    {
        List<string> information = new List<string>();

        [LuisIntent("Greeting")]
        public async Task Saudacao(IDialogContext context, LuisResult result)
        {
            string userSaudacao;
            string nome = context.Activity.From.Name.Split(' ')[0];

            if (result.Query.Contains("bom dia"))
                userSaudacao = $"Bom dia {nome}, tudo bem?";
            else
                if (result.Query.Contains("boa tarde"))
                userSaudacao = $"Boa tarde {nome}, tudo bem?";
            else
                if (result.Query.Contains("boa noite"))
                userSaudacao = $"Boa noite {nome}, tudo bem?";
            else
                userSaudacao = $"Olá {nome}! ";

            await printOptions(context, userSaudacao);

        }

        [LuisIntent("Menu")]
        public async Task Menu(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Clique na opção desejada ou caso prefira, digite algo do tipo 'Preciso de ajuda' ou  'Quero estimar um pacote' ");

            IMessageActivity menuMessage = context.MakeMessage();
            menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            menuMessage.Attachments = await getAttachment.Menu();

            await context.PostAsync(menuMessage);
        }

        [LuisIntent("GoodBye")]
        public async Task Despedida(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Até mais, volte sempre");
            context.Wait(MessageReceived);
        }

        [LuisIntent("HiringInsurance")]
        public async Task HiringInsurance(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count == 0)
            {
                await context.PostAsync("A seguir tenho todos os pacotes de seguro oferecidos por nós");
                await context.PostAsync("Lembrando que você pode encontrar um pacote específico para suas necessidades digitando 'fazer cotação de seguro' a qualquer momento");
                List<Package> pacotes = ApiPacotes.GetAllPackages();

                IMessageActivity menuMessage = context.MakeMessage();
                menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                menuMessage.Attachments = await getAttachment.HirePackage(pacotes);
                await context.PostAsync(menuMessage);
            }
            else
            {
                string p = result.Entities[0].Entity;
                if(p != "Basic" || p != "Premium" || p != "Recreation" || p != "Short Trip" || p != "Business" ||
                p != "Long Trip" || p != "Enterprise"){
                    await context.PostAsync("Infelizmente não encontramos um pacote com esse nome.");
                    await context.PostAsync("Veja as opções predefinidas que oferecemos");
                    
                    List<Package> pacotes = ApiPacotes.GetAllPackages();

                    IMessageActivity menuMessage = context.MakeMessage();
                    menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    menuMessage.Attachments = await getAttachment.HirePackage(pacotes);
                    await context.PostAsync(menuMessage);
                }else
                {
                    await context.PostAsync($"Muito bem. Pacote {p} Confirmado!");
                    await hire(context, result, p);
                }
            }
        }

        [LuisIntent("InformationCompany")]
        public async Task InformationCompany(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Texas Seguros");
            await Delay();
            await context.PostAsync("O empenho em analisar a constante divulgação das informações maximiza as possibilidades por conta do sistema de formação de quadros que corresponde às necessidades.");
            await Delay();
            await context.PostAsync("Acima de tudo, é fundamental ressaltar que o comprometimento entre as equipes exige a precisão e a definição dos índices pretendidos. Pensando mais a longo prazo, a " +
                                    "contínua expansão de nossa atividade oferece uma interessante oportunidade para verificação das condições inegavelmente apropriadas.");

            await Delay();
            await context.PostAsync("Acesse nosso menu de funcionalidades ou acesse nosso site.");
            await context.PostAsync(await BackMenu(context));
        }

        [LuisIntent("ShowPackage")]
        public async Task ShowPackage(IDialogContext context, LuisResult result)
        {

            IMessageActivity menuMessage = context.MakeMessage();
            menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            menuMessage.Attachments = await getAttachment.showPackage();

            await context.PostAsync(menuMessage);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Posso lhe oferecer as seguintes funcionalidades");
            await Delay();
            await context.PostAsync("\n- Estimar uma pacote de Seguro que seja específico às suas necessidades" +
                                    "\n- Contratar um pacote específico sem passar pela estimativa personalizada" +
                                    "\n- Acessar o FAQ de dúvidas frequentes realizadas pelo nossos usuários");
            await Delay();
            await context.PostAsync("Acesse o menu principal ou digite a qualquer momento algo do tipo \"Fazer cotação\".");
            await Delay();
            await context.PostAsync(await BackMenu(context));
        }

        [LuisIntent("Faq")]
        public async Task Faq(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("Temos aqui algumas dúvidas frequentes");
            await Delay();

            await context.PostAsync("É possível cancelar o seguro?" +
                                    "\n- Sim. É possível realizar o cancelamento em até 5 dias antes da viagem");
            await Delay();

            await context.PostAsync("Em situações de extorno o valor é retornado integralmente?" +
                                    "\n- Não. Retornaremos 85% do valor total do seguro.");
            await Delay();

            await context.PostAsync("Posso alterar o seguro no meio da viagem?" +
                                    "\n- Sim. É possúivel renegociar o pacote mesmo depois de iniciado a viagem.");
            await Delay();

            await context.PostAsync("Em quanto tempo recebo o contrato?" +
                                    "\n- Em aproximadamente 2 dias após a confirmação do pagamento");
            await Delay();

            await context.PostAsync("Acesse o menu principal ou digite a qualquer momento algo do tipo \"Fazer cotação\".");
            await Delay();

            await context.PostAsync(await BackMenu(context));
        }

        [LuisIntent("InsuranceQuote")]
        public async Task InsuranceQuote(IDialogContext context, LuisResult result)
        {
            PromptDialog.Choice(context, afterInicio, Option.Escolha, "Sua saída será do Brasil?");
        }

        private async Task hire(IDialogContext context, LuisResult result, string Package)
        {


        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Não seja idiota! Entre com uma frase válida.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("MelhorDestino")]
        public async Task MelhorDestino(IDialogContext context, LuisResult result)
        {
            //Melhor cidade
            await context.PostAsync("Indico duas cidades que são opções maravilhosas.");
            await context.PostAsync("A primeira é Bagdá no Iraque onde viverá uma experiência única e a segunda é Damasco na Síria onde há uma bela guerra civil para ser contemplada");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Descontente")]
        public async Task Descontente(IDialogContext context, LuisResult result)
        {
            //Não gostei de você
            await context.PostAsync("Não dá pra agradar todo mundo!");
            context.Wait(MessageReceived);
        }
        

        [LuisIntent("QuoteCoin")]
        public async Task QuoteCoin(IDialogContext context, LuisResult result)
        {
            if (result.Entities.Count == 0)
            {
                IMessageActivity replyCarouselMoedas = await ReplyMessageCoins(context, result);
                await context.PostAsync(replyCarouselMoedas);
                context.Wait(MessageReceived);
            }
            else
            {
                IMessageActivity replyQuote = await ReplyMessageWithQuote(context, result);
                await context.PostAsync(replyQuote);
                context.Wait(MessageReceived);
            }
        }

        //QuoteCoin
        private async Task<IMessageActivity> ReplyMessageCoins(IDialogContext context, LuisResult result)
        {
            RootObject moedas = ApiCotacao.ValorMoeda();

            IMessageActivity reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = await getAttachment.GetCarouselAllCoin(moedas);
            return reply;
        }

        //QuoteCoin
        private async Task<IMessageActivity> ReplyMessageWithQuote(IDialogContext context, LuisResult result)
        {
            RootObject moeda = ApiCotacao.ValorMoeda();

            if (moeda.status == false)
            {
                IMessageActivity reply = context.MakeMessage();
                reply.Text = $"Me desculpe. Não consegui encontrar a moeda { result.Entities[0].Entity.ToUpper()}.";
                return reply;
            }
            else
            {
                IMessageActivity reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = await getAttachment.informationQuotes(moeda, result);
                return reply;
            }
        }

        
        private static async Task<IMessageActivity> BackMenu(IDialogContext context)
        {
            IMessageActivity menuMessage = context.MakeMessage();
            menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            menuMessage.Attachments = await getAttachment.informationCompany();
            return menuMessage;
        }

        
        private static async Task Delay()
        {
            await Task.Delay(1600);
        }

        private async Task afterInicio(IDialogContext context, IAwaitable<string> result)
        {
            string aux = await result;
            if (aux == "Sim")
            {
                information.Add("Brasil");
                await context.PostAsync("Muito bem, vamos prosseguir");
                PromptDialog.Text(context, afterSaida, "Qual o estado de Saída?");
            }

            else
            {
                await context.PostAsync("Infelizmente não oferecemos pacotes para saída em outros países");
                await printOptions(context, "Temos estas Opções!");
            }
        }

        private async Task afterSaida(IDialogContext context, IAwaitable<string> result)
        {
            string estado = await result;
            information.Add(estado);
            await context.PostAsync($"Muito bem, {estado} confirmado");

            PromptDialog.Text(context, afterEsporte, "Agora me diga o lugar destino");
        }

        private async Task afterDestino(IDialogContext context, IAwaitable<string> result)
        {
            string destino = await result;

            information.Add(destino);
            await context.PostAsync($"Muito bem, {destino} é um ótimo destino");

            PromptDialog.Choice(context, europeu, Option.Escolha, "Terá algum país europeu como conexão?  Preciso saber pois as politicas de seguro nos países europeus são diferentes");

        }

        private async Task europeu(IDialogContext context, IAwaitable<string> result)
        {
            string aux = await result;
            if (aux == "Sim")
            {
                PromptDialog.Text(context, AfterEuropeu, "Informe o país europeu em que vai passar");
            }
            else
            {
                information.Add("S/País Europeu");
                await context.PostAsync("OK");
                PromptDialog.Choice(context, afterPaisEuro, Option.Escolha, "Vai praticar algum esporte radical tais como Alpinismo, Rapel, Mergulho, etc?");
            }
        }

        private async Task AfterEuropeu(IDialogContext context, IAwaitable<string> result)
        {
            string paisEuropeu = await result;

            information.Add(paisEuropeu);
            await context.PostAsync($"País {paisEuropeu} confirmado");

            PromptDialog.Choice(context, afterPaisEuro, Option.Escolha, "Vai praticar algum esporte radical tais como Alpinismo, Rapel, Mergulho, etc?");
        }
        private async Task afterPaisEuro(IDialogContext context, IAwaitable<string> result)
        {
            String esporte = await result;

            if (esporte == "Sim")
            {
                information.Add(esporte);
                await context.PostAsync("Legal. se divirta com cuidado!");
            }

            else
            {
                information.Add("Não");
                await context.PostAsync("Legal. Menos risco para sua vida!");
            }

            PromptDialog.Text(context, afterEsporte, "Mas em que data pretende viajar? DD-MM-AAAA");
        }


        private async Task afterEsporte(IDialogContext context, IAwaitable<string> result)
        {
            // Validação do modelo de datas. (Limites anteriores e superiores).
            string data = await result;

            try
            {
                DateTime dateValue = DateTime.Parse(data);
                DateTime thisDay = DateTime.Today;
                if(thisDay < dateValue)
                {
                    await context.PostAsync("Não é possível escolher uma data menor do que a data atual");
                    PromptDialog.Text(context, afterEsporte, "Entre novamente com a data de partida.");
                }
                else
                {
                    await context.PostAsync($"Data: {dateValue} confirmada");
                    information.Add(dateValue.ToString());
                    PromptDialog.Text(context, afterDataIda, "E a data de desembarque? DD-MM-AAAA");
                }
            }
            catch
            {
                await context.PostAsync("Infelizmente não consegui entender essa data");
                PromptDialog.Text(context, afterEsporte, "Entre novamente com a data de partida.");
            }
        }

        private async Task afterDataIda(IDialogContext context, IAwaitable<string> result)
        {
            string dataVolta = await result;
            try
            {
                DateTime dateValue = DateTime.Parse(dataVolta);
                DateTime dataIda = DateTime.Parse(information[information.Count]);
                if (dateValue < dataIda)
                {
                    await context.PostAsync("Não é possível escolher uma data menor do que a data de embarque");
                    PromptDialog.Text(context, afterEsporte, "Entre com a data de desembarque novamente");
                }
                else
                {
                    await context.PostAsync($"Data: {dateValue} confirmada");
                    information.Add(dateValue.ToString());
                    PromptDialog.Text(context, afterDataDes, "Quantas pessoas pretende levar na viagem");
                }
            }
            catch
            {
                await context.PostAsync("Infelizmente não consegui entender essa data");
                PromptDialog.Text(context, afterEsporte, "Entre com a data de desembarque novamente");
            }
        }

        private async Task afterDataDes(IDialogContext context, IAwaitable<string> result)
        {
            string pessoas = await result;
            information.Add(pessoas);

            await printDetails(context);
            information.Clear();

        }

        private async Task printDetails(IDialogContext context)
        {
            String Subtitle = "*DADOS INFORMADOS*\nSaída: " + information[0] + "\nEstado de saída: " + information[1] + "\nPaís de destino: " + information[2] + "\nPais Europeu: " + information[3] + "\nEsporte Radical? " +
                information[4] + "\nData de embarque: " + information[5] + "\nData de desembarque: " + information[6] + "\nPessoas na viagem: " + information[7];

            await context.PostAsync(Subtitle);

            PromptDialog.Choice(context, afterInformation, Option.Escolha, "Deseja alterar algum dos dados?");
        }

        private async Task afterInformation(IDialogContext context, IAwaitable<string> result)
        {
            string aux = await result;
            if (aux == "Sim")
            {
                PromptDialog.Text(context, AlterarInformation, "Qual dado pretende alterar?");
            }
            else
            {
                await context.PostAsync("Esses são os pacotes que podemos oferecer");

                IMessageActivity menuMessage = context.MakeMessage();
                menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                menuMessage.Attachments = await getAttachment.showPackage();

                await context.PostAsync(menuMessage);
                context.Wait(MessageReceived);
            }
        }

        private async Task AlterarInformation(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Dodos a implementar");
            context.Wait(MessageReceived);
        }

        private async Task printOptions(IDialogContext context, string saudacao)
        {
            IMessageActivity menuMessage = context.MakeMessage();
            menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            menuMessage.Attachments = await getAttachment.Greeting(saudacao);

            await context.PostAsync(menuMessage);
            context.Wait(MessageReceived);
        }

        /*
        public async Task Rentabilidade(IDialogContext context, LuisResult result)
        {
            //Rentabilidade
            //Rentabilidade rentabilidade = await ITAUSolClient.ObterRentabilidade(accesstoken);

            List<Double> value = new List<double>()
            {
                0.0069,
                0.0133,
                0.0120,
                0.0127,
                0.0116,
                0.0075,
                0.0067,
                0.0082,
                0.0113,
                0.0101,
                0.0083,
                0.0054,
            };

            List<UnidadeRentabilidade> unRet = new List<UnidadeRentabilidade>();
            foreach (Double item in value)
            {
                unRet.Add(new UnidadeRentabilidade() { acumuladoMes = item });
            }

            List<List<UnidadeRentabilidade>> rentabilidade = new List<List<UnidadeRentabilidade>>();
            rentabilidade.Add(unRet);


            if (rentabilidade == null)
            {
                await context.PostAsync("Não possível encontrar a rentabilidade deste usuário");
            }
            else
            {
                double rentabilidadeconsolidada = 1;
                double i;
                int meses = 0;
                foreach (List<UnidadeRentabilidade> lur in rentabilidade)
                {
                    foreach (UnidadeRentabilidade ur in lur)
                    {
                        i = 1 + ur.acumuladoMes;
                        rentabilidadeconsolidada = rentabilidadeconsolidada * i;
                        meses += 1;
                    }
                }
                rentabilidadeconsolidada = rentabilidadeconsolidada - 1;
                rentabilidadeconsolidada = rentabilidadeconsolidada * 100;

                string resposta = String.Format("Sua rentabilidade consolidada nos últimos {0} meses foi de {1:N4} %", meses.ToString(), rentabilidadeconsolidada);
                await context.PostAsync(resposta);
            }
        }
        */
    }
}