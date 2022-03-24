namespace Orc.FilterBuilder.Automation
{
    using System.Collections.Generic;
    using Catel;

    public static class EditFilterViewExtensions
    {
        public static void Initialize<T>(this EditFilterView target, IEnumerable<T> testCollection)
        {
            Argument.IsNotNull(() => target);

            var model = target.Current;

            target.Clear();
            model.FilterSchemeEditInfo = new FilterSchemeEditInfo
            (
                FilterSchemeBuilder.StartGroup<T>().ToFilterScheme(),
                testCollection,
                true,
                true
            );
        }
    }
}
