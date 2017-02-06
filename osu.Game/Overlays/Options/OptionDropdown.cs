﻿using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using System.Collections.Generic;

namespace osu.Game.Overlays.Options
{
    public class OptionDropDown<T> : FlowContainer
    {
        private DropDownMenu<T> dropdown;
        private SpriteText text;

        public string LabelText
        {
            get { return text.Text; }
            set
            {
                text.Text = value;
            }
        }

        public Bindable<T> Bindable
        {
            get { return bindable; }
            set
            {
                if (bindable != null)
                    bindable.ValueChanged -= Bindable_ValueChanged;
                bindable = value;
                bindable.ValueChanged += Bindable_ValueChanged;
                Bindable_ValueChanged(null, null);
            }
        }

        private Bindable<T> bindable;

        void Bindable_ValueChanged(object sender, EventArgs e)
        {
            dropdown.SelectedValue = bindable.Value;
        }

        void Dropdown_ValueChanged(object sender, EventArgs e)
        {
            bindable.Value = dropdown.SelectedValue;
        }

        protected override void Dispose(bool isDisposing)
        {
            bindable.ValueChanged -= Bindable_ValueChanged;
            dropdown.ValueChanged -= Dropdown_ValueChanged;
            base.Dispose(isDisposing);
        }

        private IEnumerable<KeyValuePair<string, T>> items;
        public IEnumerable<KeyValuePair<string, T>> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                if(dropdown != null)
                    dropdown.Items = value;
            }
        }

        public OptionDropDown()
        {
            Items = new KeyValuePair<string, T>[0];

            Direction = FlowDirection.VerticalOnly;
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Children = new Drawable[]
            {
                text = new OsuSpriteText {
                    Alpha = 0,
                },
                dropdown = new OsuDropDownMenu<T>
                {
                    Margin = new MarginPadding { Top = 5 },
                    RelativeSizeAxes = Axes.X,
                    Items = this.Items,
                }
            };
            dropdown.ValueChanged += Dropdown_ValueChanged;
        }
    }
}
