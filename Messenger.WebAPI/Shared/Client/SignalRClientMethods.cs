using Messenger.Domain.Enums;
using Messenger.Domain.Results;
using Messenger.WebAPI.Shared.SharedModels;

namespace Messenger.WebAPI.Shared.Client;

public static class SignalRClientMethods
{
    /// <summary>
    /// Получение сообщения от других пользователей
    /// </summary>
    /// <remarks>Работает с моделькой <see cref="MessageContext"/></remarks>
    public const string ReceiveMessage = "ReceiveMessage";

    /// <summary>
    /// Уведомление о том, что отправленное сообщение не будет доставлено
    /// </summary>
    /// <remarks>
    /// Отправляет <see cref="BaseResult"/>, где в <see cref="BaseResult.Message"/>
    /// описана причина, почему сообщение не отправлено
    /// </remarks>
    public const string MessageNotDelivered = "MessageNotDelivered";

    /// <summary>
    /// Изменение статуса пользователя (онлайн/оффлайн)
    /// </summary>
    /// <remarks>
    /// Работает с моделькой <see cref="UserStatusContext"/>,
    /// статус определяется через перечисление <see cref="UserOnlineStatus"/>
    /// </remarks>
    public const string StatusChanged = "StatusChanged";

    /// <summary>
    /// Вызывается при окончании времени действия токена доступа к чату,
    /// означает необходимость получить токе доступа заново
    /// </summary>
    /// <remarks>Работает с моделькой <see cref="UnauthorizedAccessContext"/></remarks>
    public const string Unauthorized = "Unauthorized";
}