﻿using osu.Framework.IO.Network;

namespace Piously.Game.Online.API
{
    public class PiouslyJsonWebRequest<T> : JsonWebRequest<T>
    {
        public PiouslyJsonWebRequest(string uri)
            : base(uri)
        {
        }

        public PiouslyJsonWebRequest()
        { 
        }

        protected override string UserAgent => "Piously";
    }
}
