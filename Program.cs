using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("enter your token");

using CancellationTokenSource cts = new();


ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();


cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    
    if (update.Message is not { } message)
        return;
    
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    string name = $"{message.From.Username} {message.From.FirstName} {message.From.LastName}";
    string nameUser = $"{message.From.FirstName} {message.From.LastName}";

    Console.WriteLine($"Отправлено сообщение '{messageText} ' пользователем' {name}' в чате {chatId}.");

    
   


    switch (messageText)
    {
        case "/start":
            string text = $"{name}, Вот список <b>команд</b>\n";
         await botClient.SendTextMessageAsync(message.From.Id, text, ParseMode.Html);
            break;
        case "/data":
            string data =
                           @"Фамилию Имя Отчество быстро написал";
            botClient.SendTextMessageAsync(message.From.Id, data);
            break;
        case "/numberPhone":
            List<List<KeyboardButton>> buttons = new();
            buttons.Add(new List<KeyboardButton>()
            { 
                new KeyboardButton("Поделиться номером") 
                { 
                    RequestContact = true
                }

            });
            ReplyKeyboardMarkup kb = new ReplyKeyboardMarkup(buttons) { OneTimeKeyboard = true, ResizeKeyboard = true };
            string numberPhone =
                                 @"Цыферки свои оставь";
           await botClient.SendTextMessageAsync(message.From.Id, numberPhone, replyMarkup:kb);
            break;
        default:
            break;
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
