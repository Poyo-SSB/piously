﻿using Newtonsoft.Json;
using Piously.Game.Users;
using System.Collections.Generic;

namespace Piously.Game.Online.API.Requests.Responses
{
    public class CommentBundle
    {
        [JsonProperty(@"comments")]
        public List<Comment> Comments { get; set; }

        [JsonProperty(@"has_more")]
        public bool HasMore { get; set; }

        [JsonProperty(@"has_more_id")]
        public long? HasMoreId { get; set; }

        [JsonProperty(@"user_follow")]
        public bool UserFollow { get; set; }

        [JsonProperty(@"included_comments")]
        public List<Comment> IncludedComments { get; set; }

        private List<long> userVotes;

        [JsonProperty(@"user_votes")]
        public List<long> UserVotes
        {
            get => userVotes;
            set
            {
                userVotes = value;

                Comments.ForEach(c => c.IsVoted = value.Contains(c.Id));
                IncludedComments.ForEach(c => c.IsVoted = value.Contains(c.Id));
            }
        }

        private List<User> users;

        [JsonProperty(@"users")]
        public List<User> Users
        {
            get => users;
            set
            {
                users = value;

                value.ForEach(u =>
                {
                    Comments.ForEach(c =>
                    {
                        if (c.UserId == u.Id)
                            c.User = u;

                        if (c.EditedById == u.Id)
                            c.EditedUser = u;
                    });

                    IncludedComments.ForEach(c =>
                    {
                        if (c.UserId == u.Id)
                            c.User = u;

                        if (c.EditedById == u.Id)
                            c.EditedUser = u;
                    });
                });
            }
        }

        [JsonProperty(@"total")]
        public int Total { get; set; }

        [JsonProperty(@"top_level_count")]
        public int TopLevelCount { get; set; }
    }
}