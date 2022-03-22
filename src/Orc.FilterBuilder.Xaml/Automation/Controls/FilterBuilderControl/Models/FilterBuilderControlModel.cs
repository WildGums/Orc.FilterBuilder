namespace Orc.FilterBuilder.Automation
{
    using System.Collections;
    using Orc.Automation;

    public class FilterBuilderControlModel : FrameworkElementModel
    {
        public FilterBuilderControlModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        [ActiveAutomationProperty]
        public IList FilteredCollection { get; set; }
        [ActiveAutomationProperty]
        public FilterBuilderMode Mode { get; set; }
        [ActiveAutomationProperty]
        public IEnumerable RawCollection { get; set; }

        public FilterBuilderFilteringFunctionBase FilteringFunc
        {
            get;
            set;
        }

        [ActiveAutomationProperty]
        public object Scope { get; set; }
        [ActiveAutomationProperty]
        public bool AllowDelete { get; set; }
        [ActiveAutomationProperty]
        public bool AllowReset { get; set; }
        [ActiveAutomationProperty]
        public bool EnableAutoCompletion { get; set; }
        [ActiveAutomationProperty]
        public bool AllowLivePreview { get; set; }
        [ActiveAutomationProperty]
        public bool AutoApplyFilter { get; set; }
    }

    public abstract class FilterBuilderFilteringFunctionBase
    {
        public abstract bool Invoke(object func);
    }
}
