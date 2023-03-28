namespace Messenger.Database.Read.Queries;

internal static class ChatRepositoryQueries
{
    internal const string GetChatsForUserAsync = $@"SELECT c.id, 
                                                           c.name
                                                    FROM public.chat c 
                                                        JOIN public.user_chat uc 
                                                            ON uc.chat_id = c.id 
                                                        JOIN public.user u
                                                            ON u.id = uc.user_id
                                                    WHERE u.email = @email";

    internal const string GetChatParticipants = $@"SELECT u.id,
                                                          u.username,
                                                          u.name, 
                                                          u.email 
                                                    FROM public.user u 
                                                        JOIN public.user_chat uc 
                                                            ON uc.user_id = u.id 
                                                        JOIN public.chat c 
                                                            ON uc.chat_id = c.id 
                                                    WHERE c.name = @chatName";

    internal const string IsChatExists = $@"SELECT EXISTS(SELECT FROM public.chat c WHERE name=@chatId)";
}