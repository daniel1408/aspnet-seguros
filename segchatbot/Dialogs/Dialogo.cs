using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using segchatbot.Dialogs;
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
        //HashSet<DadosSeguro> DicionarioDados = new HashSet<DadosSeguro>();
        DadosSeguro dados = new DadosSeguro();
        DadosUser user = new DadosUser();

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
                if (p != "basic" && p != "premium" && p != "recreation" && p != "short trip" && p != "business" &&
                p != "long Trip" && p != "enterprise" && p != "personal"){
                    await context.PostAsync("Infelizmente não encontramos um pacote com esse nome.");
                    await context.PostAsync("Veja as opções predefinidas que oferecemos");
                    
                    List<Package> pacotes = ApiPacotes.GetAllPackages();

                    IMessageActivity menuMessage = context.MakeMessage();
                    menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    menuMessage.Attachments = await getAttachment.HirePackage(pacotes);
                    await context.PostAsync(menuMessage);
                }else
                {
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
            PromptDialog.Choice(context, SaidaBrasil, Option.Escolha, "Sua saída será do Brasil?");
        }

        private async Task hire(IDialogContext context, LuisResult result, string Package)
        {
            await context.PostAsync($"Muito bem. Pacote {Package} Confirmado!");
            await Delay();
            PromptDialog.Text(context, afterHire, "Agora informe por favor seu CPF");
        }

        private async Task afterHire(IDialogContext context, IAwaitable<string> result)
        {
            string cpf = await result;
            CpfValidation validation = new CpfValidation();
            bool valid = validation.Valida(cpf);

            if (valid == false)
            {
                await context.PostAsync("CPF incorreto");
                PromptDialog.Text(context, afterHire, "Informe novamente seu CPF");
            }
            else
            {
                user.cpf = cpf;
                await context.PostAsync("Os detalhes da contratação serão enviados por e-mail.");
                PromptDialog.Text(context, addEmail, "Nos informe seu e-mail por favor.");
            }
        }

        string StoreMail = null;
        private async Task addEmail(IDialogContext context, IAwaitable<string> result)
        {
            string email = await result;
            EmailValidation validation = new EmailValidation();
            bool valid = validation.IsValidEmail(email);

            if (valid == false)
            {
               await context.PostAsync("E-mail incorreto");
               PromptDialog.Text(context, addEmail, "Por favor entre com um e-mail válido.");
            }
            else
            {
                StoreMail = email;
                PromptDialog.Text(context, confirmationEmail, "Digite novamente seu e-mail para confirmação.");
            }
        }

        private async Task confirmationEmail(IDialogContext context, IAwaitable<string> result)
        {
            string email = await result;
            if (email != StoreMail)
            {
                await context.PostAsync("E-mail não confere com e-mail anterior.");
                PromptDialog.Text(context, confirmationEmail, "Digite novamente.");
            }
            else
            {
                user.email = email;
                StoreMail = email;
                await Delay();
                await context.PostAsync($"\n- CPF: {user.cpf}" +
                                        $"\n- E-mail de contato: {user.email}");

                await Delay();
                PromptDialog.Choice(context, dataConfirmation, Option.Escolha, "Deseja confirmar estes dados?");
            }
        }

        private async Task dataConfirmation(IDialogContext context, IAwaitable<string> result)
        {
            string confirmation = await result;
            if (confirmation == "Sim")
            {
                await context.PostAsync("Contratação efetivada.");

                Email envioEmail = new Email();
                envioEmail.EnviaEmail(user.email);

                await Delay();
                await context.PostAsync("Os dados da contratação foram enviadas por e-mail");
                await context.PostAsync("Iremos avaliar os dados e logo mais mandaremos o boleto de cobrança por e-mail");
                await Delay();

                await context.PostAsync(await BackMenu(context));
            }
            else
            {
                await context.PostAsync("Muito bem, mas teremos que fazer novamente as últimas perguntas.");
                PromptDialog.Text(context, afterHire, "Nos informe por favor seu CPF");

            }
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

        private async Task SaidaBrasil(IDialogContext context, IAwaitable<string> result)
        {
            string confimacaoSaida = await result;
            if (confimacaoSaida == "Sim")
            {
                dados.SaidaBrasil = true;
                await context.PostAsync("Muito bem, vamos prosseguir");
                //PromptDialog.Text(context, EstadoSaida, "Qual o estado de Saída?");
                //Testes
                await EstadoSaida(context);
            }

            else
            {
                await context.PostAsync("Infelizmente não oferecemos pacotes para saída em outros países");
                await printOptions(context, "Temos estas Opções!");
            }
        }

        private async Task EstadoSaida(IDialogContext context)
        {
            await context.PostAsync("Qual o Estado de saída?");
            List<HeroCardModel> cardModels = new List<HeroCardModel>
            {
                new HeroCardModel
                {
                    Title = "Estados",
                    Text = "Escolha algum dos estados abaixo como ponto de partida",
                }
            };
            context.Call(new CarouselChoiceSai(cardModels), EscolhaEstado);
        }

        private async Task EscolhaEstado(IDialogContext context, IAwaitable<string> result)
        {
            string estadoSaida = await result;
            if(estadoSaida != "Outro")
            {
                await context.PostAsync($"Muito bem, {estadoSaida} confirmado");
            }
            dados.EstadoSaida = estadoSaida;
            await Destino(context);

        }

        private async Task Destino(IDialogContext context)
        {
            await context.PostAsync("Agora me diga o lugar destino");
            await context.PostAsync($"Temos as seguintes opções de destinos");

            List<HeroCardModel> cardModels = new List<HeroCardModel>
            {
                new HeroCardModel
                {
                    Title = "Destinos",
                    Text = "Escolha algum dos destinos",
                }
            };
            context.Call(new CarouselChoiceDes(cardModels), EscolhaDestino);
        }

        private async Task EscolhaDestino(IDialogContext context, IAwaitable<string> result)
        {
            string destino = await result;

            await context.PostAsync($"Muito bem, {destino} anotado");
            dados.Destino = destino;
            PromptDialog.Choice(context, EsporteRadical, Option.Escolha, "Vai praticar algum esporte radical tais como Alpinismo, Rapel, Mergulho, etc?");

        }

        private async Task EsporteRadical(IDialogContext context, IAwaitable<string> result)
        {
            string esporte = await result;

            if (esporte == "Sim")
            {
                dados.EsporteRadical = true;
                await context.PostAsync("Legal. se divirta com cuidado!");
            }

            else
            {
                dados.EsporteRadical = false;
                await context.PostAsync("Legal. Menos risco para sua vida!");
            }

            PromptDialog.Text(context, DataPartida, "Mas em que data pretende viajar? DD-MM-AAAA");
        }


        private async Task DataPartida(IDialogContext context, IAwaitable<string> result)
        {
            string dataPartida = await result;

            try
            {
                DateTime dateSaida = DateTime.Parse(dataPartida);
                DateTime thisDay = DateTime.Today;
                if(thisDay > dateSaida)
                {
                    await context.PostAsync("Não é possível escolher uma data menor do que a data atual");
                    PromptDialog.Text(context, DataPartida, "Entre novamente com a data de partida.");
                }
                else
                {
                    dados.DataSaida = dateSaida;
                    await context.PostAsync($"Data: {dateSaida} confirmada");
                    PromptDialog.Text(context, DataDesembarque, "E a data de desembarque? DD-MM-AAAA");
                }
            }
            catch
            {
                await context.PostAsync("Infelizmente não consegui entender essa data");
                PromptDialog.Text(context, DataPartida, "Entre novamente com a data de partida.");
            }
        }

        private async Task DataDesembarque(IDialogContext context, IAwaitable<string> result)
        {
            string dataVolta = await result;
            try
            {
                DateTime dateDesembarque = DateTime.Parse(dataVolta);

                dados.DataDesembarque = dateDesembarque;
                await context.PostAsync($"Data: {dateDesembarque} confirmada");
                PromptDialog.Number(context, QuantidadePessoas, "Quantas pessoas pretende levar na viagem");
                
            }
            catch
            {
                await context.PostAsync("Infelizmente não consegui entender essa data");
                PromptDialog.Text(context, DataDesembarque, "Entre com a data de desembarque novamente");
            }
        }

        private async Task QuantidadePessoas(IDialogContext context, IAwaitable<long> result)
        {
            long pessoas = await result;
            dados.QuantidadePessoas = pessoas;
            await printDetails(context);
        }

        private async Task printDetails(IDialogContext context)
        {
            
                await context.PostAsync("Dados informados: ");
                await context.PostAsync($"\n- Estado de Partida: {dados.EstadoSaida.ToString()}" +
                                        $"\n- Destino: {dados.Destino.ToString()}" +
                                        $"\n- Data de embarque: {dados.DataSaida.ToString()}" +
                                        $"\n- Data de desembarque: {dados.DataDesembarque.ToString()}" +
                                        $"\n- Quantidade de pessoas: {dados.QuantidadePessoas.ToString()}");
            

            PromptDialog.Choice(context, Confirmacao, Option.Escolha, "Deseja confirmar estes dados?");
        }

        private async Task Confirmacao(IDialogContext context, IAwaitable<string> result)
        {
            string aux = await result;
            if (aux == "Sim")
            {
                await context.PostAsync("Esse é o seu pacote personalizado");
                Package pacotes = PersonalPackage.GetPackage(dados);

                IMessageActivity menuMessage = context.MakeMessage();
                menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                menuMessage.Attachments = await getAttachment.PackageAttachments(pacotes);
                await context.PostAsync(menuMessage);
            }
            else
            {
                await context.PostAsync("Muito bem, mas teremos que fazer novamente as últimas perguntas.");
                await EstadoSaida(context);
            }
        }

        private async Task printOptions(IDialogContext context, string saudacao)
        {
            IMessageActivity menuMessage = context.MakeMessage();
            menuMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            menuMessage.Attachments = await getAttachment.Greeting(saudacao);

            await context.PostAsync(menuMessage);
            context.Wait(MessageReceived);
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
    }
}