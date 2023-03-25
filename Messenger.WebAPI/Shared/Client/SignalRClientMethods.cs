using Messenger.Domain.Enums;
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
    /// Изменение статуса пользователя (онлайн/оффлайн)
    /// <remarks>
    /// Работает с моделькой <see cref="UserStatusContext"/>,
    /// статус определяется через перечисление <see cref="UserOnlineStatus"/>
    /// </remarks>
    /// </summary>
    public const string StatusChanged = "StatusChanged";

    /// <summary>
    /// Вызывается при окончании времени действия токена доступа к чату,
    /// означает необходимость получить токе доступа заново
    /// <remarks>Работает с моделькой <see cref="UnauthorizedAccessContext"/></remarks>
    /// </summary>
    public const string Unauthorized = "Unauthorized";
}