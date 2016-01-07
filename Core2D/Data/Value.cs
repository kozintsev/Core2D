﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Portable.Xaml.Markup;

namespace Core2D
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class Value : ObservableObject
    {
        private string _content;

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { Update(ref _content, value); }
        }

        /// <summary>
        /// Creates a new <see cref="Value"/> instance.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Value Create(string content)
        {
            return new Value()
            {
                Content = content,
            };
        }
    }
}
