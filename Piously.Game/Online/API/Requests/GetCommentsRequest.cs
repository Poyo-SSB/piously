﻿using osu.Framework.IO.Network;
using Humanizer;
using Piously.Game.Online.API.Requests.Responses;
using Piously.Game.Overlays.Comments;

namespace Piously.Game.Online.API.Requests
{
    public class GetCommentsRequest : APIRequest<CommentBundle>
    {
        private readonly long commentableId;
        private readonly CommentableType type;
        private readonly CommentsSortCriteria sort;
        private readonly int page;
        private readonly long? parentId;

        public GetCommentsRequest(long commentableId, CommentableType type, CommentsSortCriteria sort = CommentsSortCriteria.New, int page = 1, long? parentId = null)
        {
            this.commentableId = commentableId;
            this.type = type;
            this.sort = sort;
            this.page = page;
            this.parentId = parentId;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();

            req.AddParameter("commentable_id", commentableId.ToString());
            req.AddParameter("commentable_type", type.ToString().Underscore().ToLowerInvariant());
            req.AddParameter("page", page.ToString());
            req.AddParameter("sort", sort.ToString().ToLowerInvariant());

            if (parentId != null)
                req.AddParameter("parent_id", parentId.ToString());

            return req;
        }

        protected override string Target => "comments";
    }

    public enum CommentableType
    {
        Build,
        Beatmapset,
        NewsPost
    }
}
