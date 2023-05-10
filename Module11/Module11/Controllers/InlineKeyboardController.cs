using Module11.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Module11.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            switch (callbackQuery.Data)
            {
                case "length":

                    _memoryStorage.GetSession(callbackQuery.From.Id).Sum = false;
                    break;

                case "sum":

                    _memoryStorage.GetSession(callbackQuery.From.Id).Sum = true;
                    break;

                default:

                    _memoryStorage.GetSession(callbackQuery.From.Id).Sum = false;
                    break;
            }


            // Генерим информационное сообщение
            string modeText = callbackQuery.Data switch
            {
                "len" => " Длина сообщения. Введите сообщение для подсчета символов",
                "sum" => " Сумма чисел. Введите числа через пробел",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Режим работы - {modeText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Режим работы можно поменять в главном меню", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}