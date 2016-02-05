// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemes.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{   
    using System.Collections.ObjectModel;
    using Catel.Data;
    using Catel.Runtime.Serialization;

    public class FilterSchemes : ModelBase
    {
        private object _tag;

        #region Constructors
        public FilterSchemes()
        {
            Schemes = new ObservableCollection<FilterScheme>();
        }
        #endregion

        #region Properties
        [ExcludeFromSerialization]
        public object Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                foreach (var filterScheme in Schemes)
                {
                    filterScheme.Tag = Tag;
                }
            }
        }

        public ObservableCollection<FilterScheme> Schemes { get; private set; }
        #endregion
    }
}