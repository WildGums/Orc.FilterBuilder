using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.FilterBuilder.AlternativeExample.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Services;

    public class MainViewModel: ViewModelBase
    {
        private IDataProvider _dataProvider;

        public MainViewModel(IDataProvider dataProvider)
        {
            Argument.IsNotNull(() => dataProvider);

            _dataProvider = dataProvider;
        }

        public IEnumerable<object> OriginalCollection
        {
            get
            {
                return _dataProvider.GetData();
            }
        }
    }
}
