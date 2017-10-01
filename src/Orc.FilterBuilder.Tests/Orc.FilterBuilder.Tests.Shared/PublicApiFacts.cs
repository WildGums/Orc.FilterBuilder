// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests
{
    using ApiApprover;
    using FilterBuilder.Models;
    using NUnit.Framework;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test]
        public void Orc_FilterBuilder_HasNoBreakingChanges()
        {
            var assembly = typeof(FilterScheme).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}