﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Core2D.Avalonia.Controls.Shapes
{
    /// <summary>
    /// Interaction logic for <see cref="PathControl"/> xaml.
    /// </summary>
    public class PathControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathControl"/> class.
        /// </summary>
        public PathControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initialize the Xaml components.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
