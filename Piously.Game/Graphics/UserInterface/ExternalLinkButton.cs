﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;

namespace Piously.Game.Graphics.UserInterface
{
    public class ExternalLinkButton : CompositeDrawable, IHasTooltip
    {
        public string Link { get; set; }

        private Color4 hoverColour;

        [Resolved]
        private GameHost host { get; set; }

        public ExternalLinkButton(string link = null)
        {
            Link = link;
            Size = new Vector2(12);
            InternalChild = new SpriteIcon
            {
                Icon = FontAwesome.Solid.ExternalLinkAlt,
                RelativeSizeAxes = Axes.Both
            };
        }

        [BackgroundDependencyLoader]
        private void load(PiouslyColor colors)
        {
            hoverColour = colors.Yellow;
        }

        protected override bool OnHover(HoverEvent e)
        {
            InternalChild.FadeColour(hoverColour, 500, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            InternalChild.FadeColour(Color4.White, 500, Easing.OutQuint);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (Link != null)
                host.OpenUrlExternally(Link);
            return true;
        }

        public string TooltipText => "view in browser";
    }
}