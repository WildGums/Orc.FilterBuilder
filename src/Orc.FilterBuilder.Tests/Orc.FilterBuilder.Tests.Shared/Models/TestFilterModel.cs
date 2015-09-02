// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestFilterModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests.Models
{
    using Catel.Data;

    public class TestFilterModel : ModelBase
    {
        public string StringProperty { get; set; }

        public bool BoolProperty { get; set; }

        public int IntProperty { get; set; }
    }
}