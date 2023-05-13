namespace Messenger.Database.Read.Queries;

internal static class ChatRepositoryQueries
{
    internal const string GetChatParticipantsByName = $@"SELECT u.id,
                                                          u.username,
                                                          u.name, 
                                                          u.email
                                                    FROM public.user u 
                                                        JOIN public.user_chat uc 
                                                            ON uc.user_id = u.id 
                                                        JOIN public.chat c 
                                                            ON uc.chat_id = c.id 
                                                    WHERE c.name = @name";
    internal const string GetChatParticipantsById = $@"SELECT u.id,
                                                          u.username,
                                                          u.name, 
                                                          u.email
                                                    FROM public.user u 
                                                        JOIN public.user_chat uc 
                                                            ON uc.user_id = u.id 
                                                        JOIN public.chat c 
                                                            ON uc.chat_id = c.id 
                                                    WHERE c.id = @chatId";

    internal const string IsChatExists = $@"SELECT EXISTS(SELECT FROM public.chat c WHERE name=@chatId)";
    internal const string GetChatByName = $@"SELECT * FROM chat WHERE name = @name";
    internal const string GetChatById = $@"SELECT * FROM chat WHERE id = @id";
}