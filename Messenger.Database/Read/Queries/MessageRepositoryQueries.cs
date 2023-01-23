namespace Messenger.Database.Read.Queries
{
    internal static class MessageRepositoryQueries
    {
        internal const string GetMessage = @"SELECT c.id, c.content , c.message_id, c.type_id,
                                                    p.id ""m_id"", p.date_of_dispatch, p.is_edited, p.is_deleted, p.sender_id, p.chat_id, p.replied_message_id, p.forwarded_message_id
                                            FROM message_content c 
                                            INNER JOIN message p ON p.id = c.message_id
                                            WHERE p.Id = @id";

        internal const string DeleteMessageForAllUsers = "UPDATE message SET is_deleted = true WHERE id = @id";
    }
}