using Messenger.Domain.Enums;
using Messenger.Domain.Results;
using Messenger.WebAPI.Shared.SharedModels;

namespace Messenger.WebAPI.Shared.SignalR;

public static class SignalRClientMethods
{
    /// <summary>
    /// Получение сообщения от других пользователей
    /// </summary>
    /// <remarks>Работает с моделькой <see cref="MessageContext"/></remarks>
    public const string ReceiveMessage = "ReceiveMessage";

    /// <summary>
    /// Редактирование существующего сообщения
    /// </summary>
    /// <remarks>Работает с моделькой <see cref="EditMessageContext"/></remarks>
    public const string MessageEdited = "MessageEdited";

    /// <summary>
    /// Уведомление о том, что пользователь прочитал сообщение
    /// </summary>
    /// <remarks>Работает с моделькой <see cref="MessageReadContext"/></remarks>
    public const string MessageRead = "MessageRead";   
    
    /// <summary>
    /// Уведомление о том, что сообщение было удалено для всех
    /// </summary>
    public const string MessageDeleted = "MessageDeleted";

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
    public const string Unauthorized = "Unauthorized";

    /// <summary>
    /// Уведомление о создании нового чата
    /// </summary>
    public const string ChatCreated = "ChatCreated";
    
    /// <summary>
    /// Уведомление о добавлении нового пользователя в чат
    /// </summary>
    public const string MemberAdded = "MemberAdded";
}