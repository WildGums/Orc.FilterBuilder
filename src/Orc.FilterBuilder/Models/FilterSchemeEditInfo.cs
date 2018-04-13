// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeEditInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System.Collections;
    using Catel;

    public class FilterSchemeEditInfo
    {
        #region Constructors
        public FilterSchemeEditInfo(FilterScheme filterScheme, IEnumerable rawCollection, bool allowLivePreview, bool enableAutoCompletion)
        {
            Argument.IsNotNull(() => filterScheme);
            Argument.IsNotNull(() => rawCollection);

            FilterScheme = filterScheme;
            RawCollection = rawCollection;
            AllowLivePreview = allowLivePreview;
            EnableAutoCompletion = enableAutoCompletion;
        }
        #endregion

        public FilterScheme FilterScheme { get; private set; }

        public IEnumerable RawCollection { get; private set; }

        public bool AllowLivePreview { get; private set; }

        public bool EnableAutoCompletion { get; private set; }
    }
}