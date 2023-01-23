using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Database.Read.Queries
{
    internal static class MessageRepositoryQueries
    {
        internal const string GetMessage = @"SELECT *
                                            FROM message p 
                                            INNER JOIN message_content c ON p.Id = c.Id
                                            WHERE p.Id = @id";
        internal const string DeleteMessageForAllUsers = "UPDATE message SET is_deleted = true WHERE id = @id";
    }
}
