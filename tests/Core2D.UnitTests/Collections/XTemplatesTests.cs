﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Core2D.Collections;
using Xunit;

namespace Core2D.UnitTests
{
    public class XTemplatesTests
    {
        [Fact]
        [Trait("Core2D", "Collections")]
        public void Inherits_From_ObservableResource()
        {
            var target = new XTemplates();
            Assert.True(target is ObservableResource);
        }

        [Fact]
        [Trait("Core2D", "Collections")]
        public void Children_Not_Null()
        {
            var target = new XTemplates();
            Assert.NotNull(target.Children);
        }
    }
}
