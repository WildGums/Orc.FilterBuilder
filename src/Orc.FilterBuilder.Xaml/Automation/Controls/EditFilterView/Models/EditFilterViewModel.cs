namespace Orc.FilterBuilder.Automation
{
    using System.Collections.Generic;
    using System.Windows;
    using Catel.IoC;
    using Orc.Automation;

    public class EditFilterViewModel : FrameworkElementModel
    {
        public EditFilterViewModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public FilterSchemeEditInfo FilterSchemeEditInfo
        {
            get
            {
                return _accessor.Execute<FilterSchemeEditInfo>(nameof(EditFilterViewPeer.GetFilterSchemeEditInfo));
            }

            set
            {
                _accessor.Execute(nameof(EditFilterViewPeer.SetFilterSchemeEditInfo), value);
            }
        }
    }

    //public class InitializeEditFilterViewMethodRun : NamedAutomationMethodRun
    //{
    //    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    //    {
    //        result = AutomationValue.NotSetValue;

    //        var filterScheme = method.Parameters[0].ExtractValue() as FilterScheme;

    //        if (owner is not EditFilterView editFilterView)
    //        {
    //            return false;
    //        }

    //        var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme,
    //            new List<EditFilterItemTestClass>
    //            {
    //                new()
    //                {
    //                    Field1 = 1,
    //                    Field2 = "Record 1",
    //                    Field3 = 1.0
    //                },

    //                new()
    //                {
    //                    Field1 = 2,
    //                    Field2 = "Record 2",
    //                    Field3 = 2.0d
    //                },

    //                new()
    //                {
    //                    Field1 = 3,
    //                    Field2 = "Record 3",
    //                    Field3 = 3.0d
    //                },
    //            },
    //            true, true);

    //        var typeFactory = this.GetTypeFactory();
    //        var editFilterViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<EditFilterViewModel>(filterSchemeEditInfo);
    //        editFilterView.DataContext = editFilterViewModel;

    //        result = AutomationValue.FromValue(true);

    //        return true;
    //    }
    //}
}
