﻿using System;
using System.Linq;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using Piously.Game.Graphics.Sprites;

namespace Piously.Game.Graphics.UserInterface
{
    public class PiouslyTabControl<T> : TabControl<T>
    {
        private Color4 accentColor;

        public const float HORIZONTAL_SPACING = 10;

        public virtual Color4 AccentColor
        {
            get => accentColor;
            set
            {
                accentColor = value;

                if (Dropdown is IHasAccentColor dropdown)
                    dropdown.AccentColor = value;
                foreach (var i in TabContainer.Children.OfType<IHasAccentColor>())
                    i.AccentColor = value;
            }
        }

        private readonly Box strip;

        protected override Dropdown<T> CreateDropdown() => new PiouslyTabDropdown<T>();

        protected override TabItem<T> CreateTabItem(T value) => new PiouslyTabItem(value);

        protected virtual float StripWidth => TabContainer.Children.Sum(c => c.IsPresent ? c.DrawWidth + TabContainer.Spacing.X : 0) - TabContainer.Spacing.X;

        /// <summary>
        /// Whether entries should be automatically populated if <typeparamref name="T"/> is an <see cref="Enum"/> type.
        /// </summary>
        protected virtual bool AddEnumEntriesAutomatically => true;

        private static bool isEnumType => typeof(T).IsEnum;

        public PiouslyTabControl()
        {
            TabContainer.Spacing = new Vector2(HORIZONTAL_SPACING, 0f);

            AddInternal(strip = new Box
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Height = 1,
                Colour = Color4.White.Opacity(0),
            });

            if (isEnumType && AddEnumEntriesAutomatically)
            {
                foreach (var val in (T[])Enum.GetValues(typeof(T)))
                    AddItem(val);
            }
        }

        [BackgroundDependencyLoader]
        private void load(PiouslyColor colors)
        {
            if (accentColor == default)
                AccentColor = colors.Blue;
        }

        public Color4 StripColour
        {
            get => strip.Colour;
            set => strip.Colour = value;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            // dont bother calculating if the strip is invisible
            if (strip.Colour.MaxAlpha > 0)
                strip.Width = Interpolation.ValueAt(Math.Clamp(Clock.ElapsedFrameTime, 0, 1000), strip.Width, StripWidth, 0, 500, Easing.OutQuint);
        }

        public class PiouslyTabItem : TabItem<T>, IHasAccentColor
        {
            protected readonly SpriteText Text;
            protected readonly Box Bar;

            private Color4 accentColor;

            public Color4 AccentColor
            {
                get => accentColor;
                set
                {
                    accentColor = value;
                    if (!Active.Value)
                        Text.Colour = value;
                }
            }

            private const float transition_length = 500;

            protected void FadeHovered()
            {
                Bar.FadeIn(transition_length, Easing.OutQuint);
                Text.FadeColour(Color4.White, transition_length, Easing.OutQuint);
            }

            protected void FadeUnhovered()
            {
                Bar.FadeTo(IsHovered ? 1 : 0, transition_length, Easing.OutQuint);
                Text.FadeColour(IsHovered ? Color4.White : AccentColor, transition_length, Easing.OutQuint);
            }

            protected override bool OnHover(HoverEvent e)
            {
                if (!Active.Value)
                    FadeHovered();
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                if (!Active.Value)
                    FadeUnhovered();
            }

            [BackgroundDependencyLoader]
            private void load(PiouslyColor colors)
            {
                if (accentColor == default)
                    AccentColor = colors.Blue;
            }

            public PiouslyTabItem(T value)
                : base(value)
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;

                Children = new Drawable[]
                {
                    Text = new PiouslySpriteText
                    {
                        Margin = new MarginPadding { Top = 5, Bottom = 5 },
                        Origin = Anchor.BottomLeft,
                        Anchor = Anchor.BottomLeft,
                        Text = (value as IHasDescription)?.Description ?? (value as Enum)?.GetDescription() ?? value.ToString(),
                        Font = PiouslyFont.GetFont(size: 14)
                    },
                    Bar = new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 1,
                        Alpha = 0,
                        Colour = Color4.White,
                        Origin = Anchor.BottomLeft,
                        Anchor = Anchor.BottomLeft,
                    },
                    new HoverClickSounds()
                };
            }

            protected override void OnActivated()
            {
                Text.Font = Text.Font.With(weight: FontWeight.Bold);
                FadeHovered();
            }

            protected override void OnDeactivated()
            {
                Text.Font = Text.Font.With(weight: FontWeight.Medium);
                FadeUnhovered();
            }
        }
    }
}