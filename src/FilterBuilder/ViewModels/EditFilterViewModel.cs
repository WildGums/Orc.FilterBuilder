// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditFilterViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Runtime.Serialization.Xml;
    using Orc.FilterBuilder.Models;
    using Orc.FilterBuilder.Services;

    public class EditFilterViewModel : ViewModelBase
    {
        #region Fields
        private readonly FilterScheme _originalFilterScheme;
        private readonly IReflectionService _reflectionService;
        private readonly IXmlSerializer _xmlSerializer;
        #endregion

        #region Constructors
        public EditFilterViewModel(FilterSchemeEditInfo filterSchemeEditInfo, IReflectionService reflectionService, IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => filterSchemeEditInfo);
            Argument.IsNotNull(() => reflectionService);
            Argument.IsNotNull(() => xmlSerializer);

            PreviewItems = new FastObservableCollection<object>();
            RawCollection = filterSchemeEditInfo.RawCollection;
            EnableAutoCompletion = filterSchemeEditInfo.EnableAutoCompletion;
            AllowLivePreview = filterSchemeEditInfo.AllowLivePreview;
            EnableLivePreview = filterSchemeEditInfo.AllowLivePreview;
            
            var filterScheme = filterSchemeEditInfo.FilterScheme;

            _originalFilterScheme = filterScheme;
            _reflectionService = reflectionService;
            _xmlSerializer = xmlSerializer;

            DeferValidationUntilFirstSaveCall = true;

            InstanceProperties = _reflectionService.GetInstanceProperties(filterScheme.TargetType).Properties;

            // Serializing gives us a *real* deep clone, there was a bug in Copy()
            //FilterScheme = _originalFilterScheme.Copy();

            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(_originalFilterScheme, memoryStream);
                memoryStream.Position = 0L;
                FilterScheme = (FilterScheme) xmlSerializer.Deserialize(typeof (FilterScheme), memoryStream);
            }

            FilterSchemeTitle = FilterScheme.Title;

            AddGroupCommand = new Command<ConditionGroup>(OnAddGroup);
            AddExpressionCommand = new Command<ConditionGroup>(OnAddExpression);
            DeleteConditionItem = new Command<ConditionTreeItem>(OnDeleteCondition);
        }
        #endregion

        #region Properties
        public override string Title { get { return "Filter scheme"; } }

        public string FilterSchemeTitle { get; set; }
        public FilterScheme FilterScheme { get; private set; }
        public bool EnableAutoCompletion { get; private set; }
        public bool AllowLivePreview { get; private set; }
        public bool EnableLivePreview { get; set; }

        public IEnumerable RawCollection { get; private set; }
        public FastObservableCollection<object> PreviewItems { get; private set; }

        public List<IPropertyMetadata> InstanceProperties { get; private set; }

        public Command<ConditionGroup> AddGroupCommand { get; private set; }
        public Command<ConditionGroup> AddExpressionCommand { get; private set; }
        public Command<ConditionTreeItem> DeleteConditionItem { get; private set; }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            UpdatePreviewItems();

            FilterScheme.Updated += OnFilterSchemeUpdated;
        }

        protected override void Close()
        {
            FilterScheme.Updated -= OnFilterSchemeUpdated;

            base.Close();
        }

        private void OnFilterSchemeUpdated(object sender, EventArgs e)
        {
            UpdatePreviewItems();
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrEmpty(FilterSchemeTitle))
            {
                validationResults.Add(FieldValidationResult.CreateError("FilterSchemeTitle", "Field is required"));
            }

            base.ValidateFields(validationResults);
        }

        protected override bool Save()
        {
            FilterScheme.Title = FilterSchemeTitle;
            _originalFilterScheme.Update(FilterScheme);

            return true;
        }

        private void OnDeleteCondition(ConditionTreeItem item)
        {
            if (item.Parent == null)
            {
                item.Items.Clear();
            }
            else
            {
                item.Parent.Items.Remove(item);
            }
        }

        private void OnAddExpression(ConditionGroup group)
        {
            var propertyExpression = new PropertyExpression();
            propertyExpression.Property = InstanceProperties.FirstOrDefault();
            group.Items.Add(propertyExpression);
            propertyExpression.Parent = group;
        }

        private void OnAddGroup(ConditionGroup group)
        {
            var newGroup = new ConditionGroup();
            group.Items.Add(newGroup);
            newGroup.Parent = group;
        }

        private void OnEnableLivePreviewChanged()
        {
            UpdatePreviewItems();
        }

        private void UpdatePreviewItems()
        {
            if (FilterScheme == null || RawCollection == null)
            {
                return;
            }

            if (!AllowLivePreview)
            {
                return;
            }

            if (!EnableLivePreview)
            {
                PreviewItems.Clear();
                return;
            }

            FilterScheme.Apply(RawCollection, PreviewItems);
        }
        #endregion
    }
}