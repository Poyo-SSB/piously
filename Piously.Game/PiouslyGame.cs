﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Physics;
using osuTK;
using osuTK.Graphics;

namespace Piously.Game
{
    public class PiouslyGame : osu.Framework.Game
    {
        RigidBodySimulation sim;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = sim = new RigidBodySimulation
            {
                FrictionCoefficient = 0.3f,
                RelativeSizeAxes = Axes.Both,
            };

            RigidBodyContainer<Drawable> rbc = new RigidBodyContainer<Drawable>
            {
                Child = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(200, 200),
                    Colour = Color4.Chartreuse,
                },
                Position = new Vector2(750, 500),
                Size = new Vector2(200, 200),
                FrictionCoefficient = 0.3f,
                Rotation = 60,
                Masking = true,
            };

            RigidBodyContainer<Drawable> rbc2 = new RigidBodyContainer<Drawable>
            {
                Child = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(200, 200),
                    Colour = Color4.HotPink,
                },
                Position = new Vector2(300, 500),
                Size = new Vector2(200, 200),
                Rotation = 60,
                FrictionCoefficient = 0.3f,
                Masking = true,
            };

            sim.Add(rbc);
            sim.Add(rbc2);
        }
    }
}